﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Data.SQLite;

namespace Event_Horizon
{
    public class DataTableManagement
    {
        public static EventHorizonEvent EventHorizon_Event = new EventHorizonEvent();
        public static int RowLimitMode = RowLimitModes.LimitOnly;
        public static Int32 RowLimitStep = 30;
        public static Int32 RowLimit = RowLimitStep;
        public static Int32 RowLimitMin = 30;
        public static Int32 RowLimitMax = 300;
        public static Int32 RowOffsetStep = 30;
        public static Int32 RowOffset = 0;
        public static Int32 RowOffsetMin = 0;
        public static Int32 RowOffsetMax = 300;

        public static List<EventHorizonLINQ> GetEvents(int listViewToPopulate, Int32 eventTypeID, Int32 filterMode, Int32 displayMode, string searchString)
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            MiscFunctions.PlayFile(AppDomain.CurrentDomain.BaseDirectory + "\\Audio\\claves.wav");
            MainWindow.mw.widgetDatabaseHealth.UpdateLastWriteLabel(true);

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection conn = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM EventLog", conn);

                            conn.Open();

                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Event);
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        Console.WriteLine("-------------------*---------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEvents - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd;

                            switch (RowLimitMode)
                            {
                                case RowLimitModes.NoLimit:
                                    sqlcmd = "SELECT * FROM EventLog;";
                                    break;
                                case RowLimitModes.LimitOnly:
                                    sqlcmd = "SELECT * FROM EventLog LIMIT @Limit;";
                                    break;
                                case RowLimitModes.LimitWithOffset:
                                    sqlcmd = "SELECT * FROM EventLog LIMIT @Limit OFFSET @Offset;";
                                    break;
                                default:
                                    sqlcmd = "SELECT * FROM EventLog LIMIT @Limit;";
                                    break;
                            }

                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            switch (RowLimitMode)
                            {
                                case RowLimitModes.NoLimit:
                                    break;
                                case RowLimitModes.LimitOnly:
                                    adapter.SelectCommand.Parameters.AddWithValue("@Limit", RowLimit);
                                    break;
                                case RowLimitModes.LimitWithOffset:
                                    adapter.SelectCommand.Parameters.AddWithValue("@Limit", RowLimit);
                                    adapter.SelectCommand.Parameters.AddWithValue("@Offset", RowOffset);
                                    break;
                                default:
                                    adapter.SelectCommand.Parameters.AddWithValue("@Limit", RowLimit);
                                    break;
                            }

                            adapter.Fill(EventHorizon_Event);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEvents - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            if (eventTypeID != 0)
            {
                switch (listViewToPopulate)
                {
                    case ListViews.Reminder:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Active:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    default:
                        query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") descending
                                select eventHorizonEvent;
                        break;
                }
            }
            else
            {
                switch (listViewToPopulate)
                {
                    case ListViews.Reminder:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Active:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Archived:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotifiedReadArchived
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotifiedReadArchived
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotifiedReadArchived
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotifiedReadArchived
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotifiedReadArchived
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Inactive:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Inactive
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Inactive
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Inactive
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Inactive
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Inactive
                                        where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    default:
                        query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                                orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                select eventHorizonEvent;
                        break;
                }
            }
 
            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonLINQ eventHorizonLINQ = new EventHorizonLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonLINQ.ID)) eventHorizonLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonLINQ.EventTypeID)) eventHorizonLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonLINQ.SourceID)) eventHorizonLINQ.SourceID = 0;

                eventHorizonLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonLINQ.FrequencyID)) eventHorizonLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonLINQ.StatusID)) eventHorizonLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse targetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonLINQ.UserID)) eventHorizonLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonLINQ.TargetUserID)) eventHorizonLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonLINQ.ReadByMeID)) eventHorizonLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.LastViewedDate = lastViewedDateTime;

                TimeSpan timeSpan = MainWindow.mw.ReminderListTimeSpan;

                int totalDays = Convert.ToInt32((targetDateTime.Date - DateTime.Today).Days);
                Color iconEllipeColor = Colors.Pink;

                Console.Write("totalDays = ");
                Console.WriteLine(totalDays);

                //if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                //{
                    switch (totalDays)
                    {
                        case int n when (n <= 0):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
                            break;
                        case int n when (n > 0 && n <= 3):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
                            break;
                        case int n when (n > 3 && n <= 7):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
                            break;
                        case int n when (n > 7 && n <= 14):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
                            break;
                        case int n when (n > 14 && n <= 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
                            break;
                        case int n when (n > 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
                            break;
                    }
                //}

                eventHorizonLINQ.Source_ID = eventHorizonLINQ.ID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["ParentEventID"].ToString(), out eventHorizonLINQ.Source_ParentEventID)) eventHorizonLINQ.Source_ParentEventID = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonLINQ.EventAttributeID)) eventHorizonLINQ.EventAttributeID = 0;
                
                eventHorizonLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonLINQ.UnitCost)) eventHorizonLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonLINQ.Qty)) eventHorizonLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonLINQ.Discount)) eventHorizonLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonLINQ.Stock)) eventHorizonLINQ.Stock = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                if (displayMode == DisplayModes.Active)
                    _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
                else if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                    _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
            }
            return _EventHorizonLINQReturnList;
        }

        public static EventHorizonLINQ GetEvent(Int32 eventID)
        {
            EventHorizonLINQ _EventHorizonLINQReturn = new EventHorizonLINQ();

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection conn = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            OleDbCommand cmd = new OleDbCommand($"SELECT * FROM EventLog", conn);

                            conn.Open();

                            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Event);
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        Console.WriteLine("-------------------*---------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEvent - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd = "SELECT * FROM EventLog ORDER BY ID DESC LIMIT 1;";
                                 
                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Event);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEvent - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("ID") == eventID
                    select eventHorizonEvent;                  

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonLINQ eventHorizonLINQ = new EventHorizonLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonLINQ.ID)) eventHorizonLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonLINQ.EventTypeID)) eventHorizonLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonLINQ.SourceID)) eventHorizonLINQ.SourceID = 0;

                eventHorizonLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonLINQ.FrequencyID)) eventHorizonLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonLINQ.StatusID)) eventHorizonLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse targetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonLINQ.UserID)) eventHorizonLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonLINQ.TargetUserID)) eventHorizonLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonLINQ.ReadByMeID)) eventHorizonLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.LastViewedDate = lastViewedDateTime;

                TimeSpan timeSpan = MainWindow.mw.ReminderListTimeSpan;

                int totalDays = Convert.ToInt32((targetDateTime.Date - DateTime.Today).Days);
                Color iconEllipeColor = Colors.Pink;

                if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                {
                    switch (totalDays)
                    {
                        case int n when (n <= 0):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
                            break;
                        case int n when (n > 0 && n <= 3):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
                            break;
                        case int n when (n > 3 && n <= 7):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
                            break;
                        case int n when (n > 7 && n <= 14):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
                            break;
                        case int n when (n > 14 && n <= 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
                            break;
                        case int n when (n > 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
                            break;
                    }
                }

                eventHorizonLINQ.Source_ID = eventHorizonLINQ.ID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["ParentEventID"].ToString(), out eventHorizonLINQ.Source_ParentEventID)) eventHorizonLINQ.Source_ParentEventID = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonLINQ.EventAttributeID)) eventHorizonLINQ.EventAttributeID = 0;

                eventHorizonLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonLINQ.UnitCost)) eventHorizonLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonLINQ.Qty)) eventHorizonLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonLINQ.Discount)) eventHorizonLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonLINQ.Stock)) eventHorizonLINQ.Stock = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                _EventHorizonLINQReturn = eventHorizonLINQ;
            }
            return _EventHorizonLINQReturn;
        }

        public static List<EventHorizonLINQ> GetReplies(Int32 eventID)
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("ParentEventID") == eventID
                    where eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.NoteEvent || eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.ReplyEvent
                    orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") ascending
                    select eventHorizonEvent;

            DataView dataView = query.AsDataView();

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonLINQ eventHorizonLINQ = new EventHorizonLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonLINQ.ID)) eventHorizonLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonLINQ.EventTypeID)) eventHorizonLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonLINQ.SourceID)) eventHorizonLINQ.SourceID = 0;

                eventHorizonLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonLINQ.FrequencyID)) eventHorizonLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonLINQ.StatusID)) eventHorizonLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonLINQ.UserID)) eventHorizonLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonLINQ.TargetUserID)) eventHorizonLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonLINQ.ReadByMeID)) eventHorizonLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.LastViewedDate = lastViewedDateTime;

                TimeSpan timeSpan = MainWindow.mw.ReminderListTimeSpan;

                int totalDays = Convert.ToInt32((targetDateTime.Date - DateTime.Today).Days);
                Color iconEllipeColor = Colors.Pink;

                if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                {
                    switch (totalDays)
                    {
                        case int n when (n <= 0):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
                            break;
                        case int n when (n > 0 && n <= 3):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
                            break;
                        case int n when (n > 3 && n <= 7):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
                            break;
                        case int n when (n > 7 && n <= 14):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
                            break;
                        case int n when (n > 14 && n <= 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
                            break;
                        case int n when (n > 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
                            break;
                    }
                }

                eventHorizonLINQ.Source_ID = eventHorizonLINQ.ID;
                eventHorizonLINQ.Source_ParentEventID = eventID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonLINQ.EventAttributeID)) eventHorizonLINQ.EventAttributeID = 0;

                eventHorizonLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonLINQ.UnitCost)) eventHorizonLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonLINQ.Qty)) eventHorizonLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonLINQ.Discount)) eventHorizonLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonLINQ.Stock)) eventHorizonLINQ.Stock = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
            }

            return _EventHorizonLINQReturnList;
        }

        

        public static void SaveEvent(EventWindow eventWindow, EventHorizonLINQ eventHorizonLINQ, int eventMode)
        {
            if (CheckFormFields(eventWindow))
            {
                bool saveSuccessFull = false;

                string detailsSafeString = eventWindow.DetailsTextBox.Text.Replace("'", "''");

                DateTime? createdDateTime = DateTime.Now;

                string targetTimeString = eventWindow.TargetTimeHoursPicker.Text;
                targetTimeString += ":";
                targetTimeString += eventWindow.TargetTimeMinutesPicker.Text;
                targetTimeString += ":00";

                DateTime ttimedt = DateTime.ParseExact(targetTimeString, "HH:mm:ss", CultureInfo.InvariantCulture);

                DateTime targetDateTimeNow = DateTime.Now;

                DateTime? targetDateTime = DateTime.Now;

                if (targetTimeString == "00:00:00")
                    targetDateTime = new DateTime(eventWindow.TargetDatePicker.SelectedDate.Value.Year, eventWindow.TargetDatePicker.SelectedDate.Value.Month, eventWindow.TargetDatePicker.SelectedDate.Value.Day, 0, 0, 0);
                else
                    targetDateTime = new DateTime(eventWindow.TargetDatePicker.SelectedDate.Value.Year, eventWindow.TargetDatePicker.SelectedDate.Value.Month, eventWindow.TargetDatePicker.SelectedDate.Value.Day, ttimedt.Hour, ttimedt.Minute, ttimedt.Second);

                string query2 = "Select @@Identity";
                string query3 = "UPDATE EventLog SET[ParentEventID] = ? WHERE [ID] = ?";

                Int32 id;
                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.AccessMDB:
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (var command = new OleDbCommand("UPDATE EventLog SET[EventTypeID] = ?, [SourceID] = ?, [Details] = ?, [FrequencyID] = ?, [StatusID] = ?, [TargetDateTime] = ?, [TargetUserID] = ?, [ReadByMeID] = ?, [LastViewedDateTime] = ?, [RemindMeID] = ?, [RemindMeDateTime] = ?, [NotificationAcknowledged] = ?, [ParentEventID] = ?, [EventModeID] = ?, [EventAttributeID] = ?, [PathFileName] = ?, [UnitCost] = ? , [Qty] = ?, [Discount] = ?, [Stock] = ? WHERE [ID] = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@EventTypeID", eventWindow.EventTypeComboBox.SelectedIndex);
                                command.Parameters.AddWithValue("@SourceID", eventWindow.SourceComboBox.SelectedIndex);
                                command.Parameters.AddWithValue("@Details", detailsSafeString);
                                command.Parameters.AddWithValue("@FrequencyID", eventWindow.FrequencyComboBox.SelectedIndex);
                                command.Parameters.AddWithValue("@StatusID", eventWindow.StatusComboBox.SelectedIndex);

                                command.Parameters.Add("TargetDateTime", OleDbType.Date);
                                command.Parameters["TargetDateTime"].Value = targetDateTime;

                                command.Parameters.AddWithValue("@TargetUserID", eventWindow.TargetUserIDComboBox.SelectedIndex);
                                command.Parameters.AddWithValue("@ReadByMeID", eventHorizonLINQ.ReadByMeID);

                                command.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                                command.Parameters["LastViewedDateTime"].Value = DateTime.MinValue;

                                command.Parameters.AddWithValue("@RemindMeID", eventHorizonLINQ.RemindMeID);

                                command.Parameters.Add("RemindMeDateTime", OleDbType.Date);
                                command.Parameters["RemindMeDateTime"].Value = eventHorizonLINQ.RemindMeDateTime;

                                command.Parameters.AddWithValue("@NotificationAcknowledged", 0);

                                command.Parameters.AddWithValue("@ParentEventID", eventHorizonLINQ.Source_ParentEventID);
                                command.Parameters.AddWithValue("@EventModeID", eventHorizonLINQ.EventModeID);

                                command.Parameters.AddWithValue("@EventAttributeID", eventHorizonLINQ.EventAttributeID);
                                command.Parameters.AddWithValue("@PathFileName", eventHorizonLINQ.PathFileName);
                                command.Parameters.AddWithValue("@UnitCost", eventHorizonLINQ.UnitCost);
                                command.Parameters.AddWithValue("@Qty", eventHorizonLINQ.Qty);
                                command.Parameters.AddWithValue("@Discount", eventHorizonLINQ.Discount);
                                command.Parameters.AddWithValue("@Stock", eventHorizonLINQ.Stock);


                                command.Parameters.AddWithValue("@ID", eventHorizonLINQ.ID);

                                if (eventMode == EventWindowModes.ViewMainEvent || eventMode == EventWindowModes.ViewNote || eventMode == EventWindowModes.ViewReply || eventMode == EventWindowModes.EditMainEvent || eventMode == EventWindowModes.EditNote || eventMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || eventMode == EventWindowModes.NewEvent || eventMode == EventWindowModes.NewNote || eventMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO EventLog (EventTypeID, SourceID, Details, FrequencyID, StatusID, CreatedDateTime, TargetDateTime, UserID, TargetUserID, ReadByMeID, LastViewedDateTime, RemindMeID, RemindMeDateTime, NotificationAcknowledged, ParentEventID, EventModeID, EventAttributeID, PathFileName, UnitCost, Qty, Discount, Stock) VALUES (@EventTypeID, @SourceID, @Details, @FrequencyID, @StatusID, @CreatedDateTime, @TargetDateTime, @UserID, @TargetUserID, @ReadByMeID, @LastViewedDateTime, @RemindMeID, @RemindMeDateTime, @NotificationAcknowledged, @ParentEventID, @EventModeID, @EventAttributeID, @PathFileName, @UnitCost, @Qty, @Discount, @Stock);";
                                    command.Parameters.AddWithValue("@EventTypeID", eventWindow.EventTypeComboBox.SelectedIndex);
                                    command.Parameters.AddWithValue("@SourceID", eventWindow.SourceComboBox.SelectedIndex);
                                    command.Parameters.AddWithValue("@Details", detailsSafeString);
                                    command.Parameters.AddWithValue("@FrequencyID", eventWindow.FrequencyComboBox.SelectedIndex);
                                    command.Parameters.AddWithValue("@StatusID", eventWindow.StatusComboBox.SelectedIndex);

                                    command.Parameters.Add("CreatedDateTime", OleDbType.Date);
                                    command.Parameters["CreatedDateTime"].Value = createdDateTime;

                                    command.Parameters.Add("TargetDateTime", OleDbType.Date);
                                    command.Parameters["TargetDateTime"].Value = targetDateTime;

                                    command.Parameters.AddWithValue("@UserID", XMLReaderWriter.UserID);
                                    command.Parameters.AddWithValue("@TargetUserID", eventWindow.TargetUserIDComboBox.SelectedIndex);
                                    command.Parameters.AddWithValue("@ReadByMeID", 0);

                                    command.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                                    command.Parameters["LastViewedDateTime"].Value = DateTime.MinValue;

                                    command.Parameters.AddWithValue("@RemindMeID", 0);

                                    command.Parameters.Add("RemindMeDateTime", OleDbType.Date);
                                    command.Parameters["RemindMeDateTime"].Value = DateTime.MinValue;

                                    command.Parameters.AddWithValue("@NotificationAcknowledged", 0);

                                    switch (eventMode)
                                    {
                                        case EventWindowModes.NewEvent:
                                            command.Parameters.AddWithValue("@ParentEventID", 0);
                                            command.Parameters.AddWithValue("@EventModeID", EventModes.MainEvent);
                                            break;
                                        case EventWindowModes.NewNote:
                                            command.Parameters.AddWithValue("@ParentEventID", eventHorizonLINQ.Source_ParentEventID);
                                            command.Parameters.AddWithValue("@EventModeID", EventModes.NoteEvent);
                                            break;
                                        case EventWindowModes.NewReply:
                                            command.Parameters.AddWithValue("@ParentEventID", eventHorizonLINQ.Source_ParentEventID);
                                            command.Parameters.AddWithValue("@EventModeID", EventModes.ReplyEvent);
                                            break;
                                        default:
                                            command.Parameters.AddWithValue("@ParentEventID", 0);
                                            command.Parameters.AddWithValue("@EventModeID", EventModes.MainEvent);
                                            break;
                                    }

                                    command.Parameters.AddWithValue("@EventAttributeID", eventHorizonLINQ.EventAttributeID);
                                    command.Parameters.AddWithValue("@PathFileName", eventHorizonLINQ.PathFileName);
                                    command.Parameters.AddWithValue("@UnitCost", eventHorizonLINQ.UnitCost);
                                    command.Parameters.AddWithValue("@Qty", eventHorizonLINQ.Qty);
                                    command.Parameters.AddWithValue("@Discount", eventHorizonLINQ.Discount);
                                    command.Parameters.AddWithValue("@Stock", eventHorizonLINQ.Stock);

                                    command.ExecuteNonQuery();

                                    //gets the new ID number and makes ID & ParentEventID the same new ID
                                    if (eventMode == EventWindowModes.NewEvent)
                                    {
                                        command.CommandText = query2;

                                        id = (int)command.ExecuteScalar();
                                        Console.Write("id = (int)command.ExecuteScalar(); = ");
                                        Console.WriteLine(id);

                                        command.Parameters.Clear();
                                        command.CommandText = query3;
                                        command.Parameters.AddWithValue("@ParentEventID", id);
                                        command.Parameters.AddWithValue("@ID", id);
                                        command.ExecuteNonQuery();
                                    }
                                    MainWindow.mw.Status.Content = "Successfully added a new event";
                                    saveSuccessFull = true;
                                }
                            }
                        }
                        break;
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE EventLog SET EventTypeID = ?, SourceID = ?, Details = ?, FrequencyID = ?, StatusID = ?, TargetDateTime = ?, TargetUserID = ?, ReadByMeID = ?, LastViewedDateTime = ?, RemindMeID = ?, RemindMeDateTime = ?, NotificationAcknowledged = ?, ParentEventID = ?, EventModeID = ?, EventAttributeID = ?, PathFileName = ?, UnitCost = ?, Qty = ?, Discount = ?, Stock = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@EventTypeID", DbType.Int32).Value = eventWindow.EventTypeComboBox.SelectedIndex;
                                command.Parameters.Add("@SourceID", DbType.Int32).Value = eventWindow.SourceComboBox.SelectedIndex;
                                command.Parameters.Add("@Details", DbType.String).Value = detailsSafeString;

                                command.Parameters.Add("@FrequencyID", DbType.Int32).Value = eventWindow.FrequencyComboBox.SelectedIndex;
                                command.Parameters.Add("@StatusID", DbType.Int32).Value = eventWindow.StatusComboBox.SelectedIndex;

                                command.Parameters.Add("@TargetDateTime", DbType.DateTime).Value = targetDateTime;

                                command.Parameters.Add("@TargetUserID", DbType.Int32).Value = eventWindow.TargetUserIDComboBox.SelectedIndex;
                                command.Parameters.Add("@ReadByMeID", DbType.Int32).Value = eventHorizonLINQ.ReadByMeID;

                                command.Parameters.Add("@LastViewedDateTime", DbType.DateTime).Value = DateTime.MinValue;

                                command.Parameters.Add("@RemindMeID", DbType.Int32).Value = eventHorizonLINQ.RemindMeID;

                                command.Parameters.Add("@RemindMeDateTime", DbType.DateTime).Value = eventHorizonLINQ.RemindMeDateTime;

                                command.Parameters.Add("@NotificationAcknowledged", DbType.Int32).Value = 0;

                                command.Parameters.Add("@ParentEventID", DbType.Int32).Value = eventHorizonLINQ.Source_ParentEventID;
                                command.Parameters.Add("@EventModeID", DbType.Int32).Value = eventHorizonLINQ.EventModeID;

                                command.Parameters.Add("@EventAttributeID", DbType.Int32).Value = eventHorizonLINQ.EventAttributeID;
                                command.Parameters.Add("@PathFileName", DbType.String).Value = eventHorizonLINQ.PathFileName;
                                command.Parameters.Add("@UnitCost", DbType.Double).Value = eventHorizonLINQ.UnitCost;
                                command.Parameters.Add("@Qty", DbType.Int32).Value = eventHorizonLINQ.Qty;
                                command.Parameters.Add("@Discount", DbType.Double).Value = eventHorizonLINQ.Discount;
                                command.Parameters.Add("@Stock", DbType.Int32).Value = eventHorizonLINQ.Stock;

                                command.Parameters.Add("@ID", DbType.Int32).Value = eventHorizonLINQ.ID;

                                if (eventMode == EventWindowModes.ViewMainEvent || eventMode == EventWindowModes.ViewNote || eventMode == EventWindowModes.ViewReply || eventMode == EventWindowModes.EditMainEvent || eventMode == EventWindowModes.EditNote || eventMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || eventMode == EventWindowModes.NewEvent || eventMode == EventWindowModes.NewNote || eventMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO EventLog (EventTypeID, SourceID, Details, FrequencyID, StatusID, CreatedDateTime, TargetDateTime, UserID, TargetUserID, ReadByMeID, LastViewedDateTime, RemindMeID, RemindMeDateTime, NotificationAcknowledged, ParentEventID, EventModeID, EventAttributeID, PathFileName, UnitCost, Qty, Discount, Stock) VALUES (@EventTypeID, @SourceID, @Details, @FrequencyID, @StatusID, @CreatedDateTime, @TargetDateTime, @UserID, @TargetUserID, @ReadByMeID, @LastViewedDateTime, @RemindMeID, @RemindMeDateTime, @NotificationAcknowledged, @ParentEventID, @EventModeID, @EventAttributeID, @PathFileName, @UnitCost, @Qty, @Discount, @Stock);";

                                    command.Parameters.Add("@EventTypeID", DbType.Int32).Value = eventWindow.EventTypeComboBox.SelectedIndex;
                                    command.Parameters.Add("@SourceID", DbType.Int32).Value = eventWindow.SourceComboBox.SelectedIndex;
                                    command.Parameters.Add("@Details", DbType.String).Value = detailsSafeString;

                                    command.Parameters.Add("@FrequencyID", DbType.Int32).Value = eventWindow.FrequencyComboBox.SelectedIndex;
                                    command.Parameters.Add("@StatusID", DbType.Int32).Value = eventWindow.StatusComboBox.SelectedIndex;

                                    command.Parameters.Add("@CreatedDateTime", DbType.DateTime).Value = createdDateTime;
                                    command.Parameters.Add("@TargetDateTime", DbType.DateTime).Value = targetDateTime;

                                    command.Parameters.Add("@UserID", DbType.Int32).Value = XMLReaderWriter.UserID;
                                    command.Parameters.Add("@TargetUserID", DbType.Int32).Value = eventWindow.TargetUserIDComboBox.SelectedIndex;
                                    command.Parameters.Add("@ReadByMeID", DbType.Int32).Value = 0;

                                    command.Parameters.Add("@LastViewedDateTime", DbType.DateTime).Value = DateTime.MinValue;

                                    command.Parameters.Add("@RemindMeID", DbType.Int32).Value = 0;

                                    command.Parameters.Add("@RemindMeDateTime", DbType.DateTime).Value = DateTime.MinValue;

                                    command.Parameters.Add("@NotificationAcknowledged", DbType.Int32).Value = 0;

                                    switch (eventMode)
                                    {
                                        case EventWindowModes.NewEvent:
                                            command.Parameters.Add("@ParentEventID", DbType.Int32).Value = 0;
                                            command.Parameters.Add("@EventModeID", DbType.Int32).Value = EventModes.MainEvent;
                                            break;
                                        case EventWindowModes.NewNote:
                                            command.Parameters.Add("@ParentEventID", DbType.Int32).Value = eventHorizonLINQ.Source_ParentEventID;
                                            command.Parameters.Add("@EventModeID", DbType.Int32).Value = EventModes.NoteEvent;
                                            break;
                                        case EventWindowModes.NewReply:
                                            command.Parameters.Add("@ParentEventID", DbType.Int32).Value = eventHorizonLINQ.Source_ParentEventID;
                                            command.Parameters.Add("@EventModeID", DbType.Int32).Value = EventModes.ReplyEvent;
                                            break;
                                        default:
                                            command.Parameters.Add("@ParentEventID", DbType.Int32).Value = 0;
                                            command.Parameters.Add("@EventModeID", DbType.Int32).Value = EventModes.MainEvent;
                                            break;
                                    }

                                    command.Parameters.Add("@EventAttributeID", DbType.Int32).Value = eventHorizonLINQ.EventAttributeID;
                                    command.Parameters.Add("@PathFileName", DbType.String).Value = eventHorizonLINQ.PathFileName;
                                    command.Parameters.Add("@UnitCost", DbType.Double).Value = eventHorizonLINQ.UnitCost;
                                    command.Parameters.Add("@Qty", DbType.Int32).Value = eventHorizonLINQ.Qty;
                                    command.Parameters.Add("@Discount", DbType.Double).Value = eventHorizonLINQ.Discount;
                                    command.Parameters.Add("@Stock", DbType.Int32).Value = eventHorizonLINQ.Stock;

                                    command.ExecuteNonQuery();

                                    //gets the new ID number and makes ID & ParentEventID the same new ID
                                    if (eventMode == EventWindowModes.NewEvent)
                                    {
                                        query2 = "SELECT last_insert_rowid()";
                                        command.CommandText = query2;

                                        id = Convert.ToInt32(command.ExecuteScalar());
                                        Console.Write("id = (int)command.ExecuteScalar(); = ");
                                        Console.WriteLine(id);

                                        command.Parameters.Clear();
                                        query3 = "UPDATE EventLog SET ParentEventID = ? WHERE ID = ?";
                                        command.CommandText = query3;
                                        command.Parameters.Add("@ParentEventID", DbType.Int32).Value = id;
                                        command.Parameters.Add("@ID", DbType.Int32).Value = id;
                                        command.ExecuteNonQuery();
                                    }
                                    MainWindow.mw.Status.Content = "Successfully added a new event"; 
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.mw.Status.Content = "Successfully updated an event";
                    MainWindow.mw.ReminderListView.SelectedItem = null;
                    MainWindow.mw.RunningTask();
                }

                if (saveSuccessFull)
                {
                    eventWindow.Close();
                    if (eventWindow.eventWindow != null) eventWindow.eventWindow.Close();
                    MainWindow.mw.ReminderListView.SelectedItem = null;
                }
            }
        }
        
        private static Int32 GetUserID(Int32 EventID)
        {
            Int32 ReturnUserID = 0;

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand("SELECT UserID FROM EventLog WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@ID", EventID);

                                using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        ReturnUserID = int.Parse(reader["UserID"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUserID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("SELECT UserID FROM EventLog WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                using (SQLiteDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        ReturnUserID = int.Parse(reader["UserID"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUserID - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
            return ReturnUserID;
        }
        
        public static void DeleteEvent(Int32 EventID)
        {
            bool saveSuccessFull = false;

            if (XMLReaderWriter.UserID != 1)
            {
                if (GetUserID(EventID) != XMLReaderWriter.UserID)
                {
                    EventHorizonRequesterNotification rorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, you can only delete your own events.", InformationTextBlock = "You could ask the user who created it, to delete it." }, RequesterTypes.OK);
                    rorn.ShowDialog();
                    return;
                }
            }

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand("DELETE FROM EventLog WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@ID", EventID);

                                command.ExecuteNonQuery();

                                saveSuccessFull = true;

                                MainWindow.mw.Status.Content = "Successfully deleted event.";
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        MainWindow.mw.Status.Content = "Problem deleting event from log.";

                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "DeleteEvent - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM EventLog WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                saveSuccessFull = true;

                                MainWindow.mw.Status.Content = "Successfully deleted event.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "DeleteEvent - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            if (saveSuccessFull)
            {
                if (MainWindow.mw.DisplayMode == DisplayModes.Reminders)
                    MainWindow.mw.RefreshLog(ListViews.Reminder);
                else
                    MainWindow.mw.RefreshLog(ListViews.Active);
            }
        }

        public static void GetUsersLastTimeOnline()
        {
            DateTime LastTimeOnlineDateTime = DateTime.MinValue;

            String SqlString = "SELECT * FROM Users;";

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection conn = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                            {
                                conn.Open();
                                using (OleDbDataReader reader = cmd.ExecuteReader())
                                {
                                    EventHorizonTokens.UsersLastTimeOnlineDictionary.Clear();

                                    while (reader.Read())
                                    {
                                        Int32 UserID = int.Parse(reader["ID"].ToString());

                                        String LastTimeOnlineString = reader["LastTimeOnline"].ToString();

                                        if (DateTime.TryParse(LastTimeOnlineString, out LastTimeOnlineDateTime))
                                        {
                                            if (LastTimeOnlineDateTime.TimeOfDay == TimeSpan.Zero)
                                                LastTimeOnlineString = LastTimeOnlineDateTime.ToString("dd/MM/y");
                                            else
                                                LastTimeOnlineString = LastTimeOnlineDateTime.ToString("dd/MM/y HH:mm");

                                            //Console.Write("LastTimeOnline = ");
                                            //Console.WriteLine(LastTimeOnlineString);

                                            if (!EventHorizonTokens.UsersLastTimeOnlineDictionary.ContainsKey(UserID))
                                            {
                                                EventHorizonTokens.UsersLastTimeOnlineDictionary.Add(UserID, LastTimeOnlineDateTime);
                                            }
                                        }
                                        else
                                            Console.WriteLine("Unable to parse LastTimeOnlineString '{0}'", LastTimeOnlineString);
                                    }
                                }
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUsersLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand(SqlString, connection))
                            {
                                connection.Open();
                                using (SQLiteDataReader reader = command.ExecuteReader())
                                {
                                    EventHorizonTokens.UsersLastTimeOnlineDictionary.Clear();

                                    while (reader.Read())
                                    {
                                        Int32 UserID = int.Parse(reader["ID"].ToString());

                                        String LastTimeOnlineString = reader["LastTimeOnline"].ToString();

                                        if (DateTime.TryParse(LastTimeOnlineString, out LastTimeOnlineDateTime))
                                        {
                                            if (LastTimeOnlineDateTime.TimeOfDay == TimeSpan.Zero)
                                                LastTimeOnlineString = LastTimeOnlineDateTime.ToString("dd/MM/y");
                                            else
                                                LastTimeOnlineString = LastTimeOnlineDateTime.ToString("dd/MM/y HH:mm");

                                            //Console.Write("LastTimeOnline = ");
                                            //Console.WriteLine(LastTimeOnlineString);

                                            if (!EventHorizonTokens.UsersLastTimeOnlineDictionary.ContainsKey(UserID))
                                            {
                                                EventHorizonTokens.UsersLastTimeOnlineDictionary.Add(UserID, LastTimeOnlineDateTime);
                                            }
                                        }
                                        else
                                            Console.WriteLine("Unable to parse LastTimeOnlineString '{0}'", LastTimeOnlineString);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUsersLastTimeOnline - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
        
        public static void InsertOrUpdateLastTimeOnline(Int32 UserID)
        {
            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (var command = new OleDbCommand("UPDATE Users SET [LastTimeOnline] = ? WHERE [ID] = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("LastTimeOnline", OleDbType.Date);
                                command.Parameters["LastTimeOnline"].Value = DateTime.Now;
                                command.Parameters.AddWithValue("@ID", UserID);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Users (ID, LastTimeOnline) VALUES (@ID, @LastTimeOnline)";
                                    command.Parameters.AddWithValue("@ID", UserID);
                                    command.Parameters.Add("LastTimeOnline", OleDbType.Date);
                                    command.Parameters["LastTimeOnline"].Value = DateTime.Now;
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "InsertOrUpdateLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE Users SET LastTimeOnline = ? WHERE ID = ?", connection))
                            {
                                connection.Open();
  
                                command.Parameters.Add("@LastTimeOnline", DbType.DateTime).Value = DateTime.Now;
                                command.Parameters.Add("@ID", DbType.Int32).Value = UserID;

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Users (ID, LastTimeOnline) VALUES (@ID, @LastTimeOnline)";
                                    command.Parameters.Add("@ID", DbType.Int32).Value = UserID;
                                    command.Parameters.Add("@LastTimeOnline", DbType.DateTime).Value = DateTime.Now;
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "InsertOrUpdateLastTimeOnline - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }

        public static List<EventHorizonLINQ> GetMyUnreadAndMyReminders()
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                    where (eventHorizonEvent.Field<Int32>("StatusID") == Statuses.Active || eventHorizonEvent.Field<Int32>("StatusID") == Statuses.ActiveNotified) && eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                    orderby eventHorizonEvent.Field<DateTime>("LastViewedDateTime") descending, eventHorizonEvent.Field<DateTime>("RemindMeDateTime") descending
                    select eventHorizonEvent;

            DataView dataView = query.AsDataView();

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonLINQ eventHorizonLINQ = new EventHorizonLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonLINQ.ID)) eventHorizonLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonLINQ.EventTypeID)) eventHorizonLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonLINQ.SourceID)) eventHorizonLINQ.SourceID = 0;

                eventHorizonLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonLINQ.FrequencyID)) eventHorizonLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonLINQ.StatusID)) eventHorizonLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonLINQ.UserID)) eventHorizonLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonLINQ.TargetUserID)) eventHorizonLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonLINQ.ReadByMeID)) eventHorizonLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.LastViewedDate = lastViewedDateTime;

                TimeSpan timeSpan = MainWindow.mw.ReminderListTimeSpan;

                int totalDays = Convert.ToInt32((targetDateTime.Date - DateTime.Today).Days);
                Color iconEllipeColor = Colors.Pink;

                if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                {
                    switch (totalDays)
                    {
                        case int n when (n <= 0):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
                            break;
                        case int n when (n > 0 && n <= 3):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
                            break;
                        case int n when (n > 3 && n <= 7):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
                            break;
                        case int n when (n > 7 && n <= 14):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
                            break;
                        case int n when (n > 14 && n <= 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
                            break;
                        case int n when (n > 28):
                            iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
                            break;
                    }
                }

                if (!int.TryParse(dataRow["ParentEventID"].ToString(), out eventHorizonLINQ.Source_ParentEventID)) eventHorizonLINQ.Source_ParentEventID = 0;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonLINQ.EventAttributeID)) eventHorizonLINQ.EventAttributeID = 0;

                eventHorizonLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonLINQ.UnitCost)) eventHorizonLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonLINQ.Qty)) eventHorizonLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonLINQ.Discount)) eventHorizonLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonLINQ.Stock)) eventHorizonLINQ.Stock = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                if (eventHorizonLINQ.UserID != XMLReaderWriter.UserID)
                {
                    if (eventHorizonLINQ.NotificationAcknowledged == NotificationAcknowlegedModes.No)
                    {
                        if (eventHorizonLINQ.RemindMeID == RemindMeModes.Yes && DateTime.Now >= remindMeDateTime)
                            _EventHorizonLINQReturnList.Add(eventHorizonLINQ);

                        if (eventHorizonLINQ.RemindMeID == RemindMeModes.No && eventHorizonLINQ.StatusID == Statuses.Active)
                            _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
                    }
                    else if (eventHorizonLINQ.NotificationAcknowledged == NotificationAcknowlegedModes.Yes)
                    {
                        if (eventHorizonLINQ.RemindMeID == RemindMeModes.Yes && DateTime.Now >= remindMeDateTime)
                            _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
                    }
                }
            }

            return _EventHorizonLINQReturnList;
        }

        public static void UpdateMyReminder(Int32 EventID, int ReminderMeID, DateTime RemindMeDateTime, int NotificationAcknowledged)
        {
            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand("UPDATE EventLog SET[RemindMeID] = ?, [RemindMeDateTime] = ?, [NotificationAcknowledged] = ? WHERE [ID] = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@RemindMeID", ReminderMeID);

                                command.Parameters.Add("RemindMeDateTime", OleDbType.Date);
                                command.Parameters["RemindMeDateTime"].Value = RemindMeDateTime;

                                command.Parameters.AddWithValue("@NotificationAcknowledged", NotificationAcknowledged);

                                command.Parameters.AddWithValue("@ID", EventID);

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event status.";
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        MainWindow.mw.Status.Content = "Problem deleting event from log.";

                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateMyReminder - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE EventLog SET RemindMeID = ?, RemindMeDateTime = ?, NotificationAcknowledged = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@RemindMeID", DbType.Int32).Value = ReminderMeID;
                                command.Parameters.Add("@RemindMeDateTime", DbType.DateTime).Value = RemindMeDateTime;

                                command.Parameters.Add("@NotificationAcknowledged", DbType.Int32).Value = NotificationAcknowledged;

                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event status.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateMyReminder - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
        
        public static void UpdateStatusID(Int32 EventID, int StatusID)
        {
            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand("UPDATE EventLog SET [StatusID] = ? WHERE [ID] = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@StatusID", StatusID);
                                command.Parameters.AddWithValue("@ID", EventID);

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event status.";
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        MainWindow.mw.Status.Content = "Problem setting event status.";

                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateStatusID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE EventLog SET StatusID = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@StatusID", DbType.Int32).Value = StatusID;
                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event status.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateStatusID - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
        
        public static void UpdateReadByMeID(Int32 EventID, int ReadByMeID)
        {
            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand("UPDATE EventLog SET [ReadByMeID] = ?, [LastViewedDateTime] = ? WHERE [ID] = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@ReadByMeID", ReadByMeID);
                                command.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                                command.Parameters["LastViewedDateTime"].Value = DateTime.Now;
                                command.Parameters.AddWithValue("@ID", EventID);

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event read status.";
                            }
                        }
                    }
                    catch (OleDbException myOLEDBException)
                    {
                        MainWindow.mw.Status.Content = "Problem setting event read status.";

                        Console.WriteLine("----------------------------------------");
                        for (int i = 0; i <= myOLEDBException.Errors.Count - 1; i++)
                        {
                            Console.WriteLine("Message " + (i + 1) + ": " + myOLEDBException.Errors[i].Message);
                            Console.WriteLine("Native: " + myOLEDBException.Errors[i].NativeError.ToString());
                            Console.WriteLine("Source: " + myOLEDBException.Errors[i].Source);
                            Console.WriteLine("SQL: " + myOLEDBException.Errors[i].SQLState);
                            Console.WriteLine("----------------------------------------");

                            EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateReadByMeID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                            msg.ShowDialog();
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE EventLog SET ReadByMeID = ?, LastViewedDateTime = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ReadByMeID", DbType.Int32).Value = ReadByMeID;
                                command.Parameters.Add("@LastViewedDateTime", DbType.DateTime).Value = DateTime.Now;
                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully set event status.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateReadByMeID - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }

        private static bool CheckFormFields(EventWindow eventWindow)
        {
            int result = 0;

            if (eventWindow.EventTypeComboBox.SelectedIndex == 0)
            {
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckFormFields", InformationTextBlock = "You can not choose 'All Events' as an event type." }, RequesterTypes.OK);
                msg.ShowDialog();
                return false;
            }

            if (eventWindow.EventTypeComboBox.SelectedIndex > -1)
            {
                result++;
            }
            if (eventWindow.SourceComboBox.SelectedIndex > -1)
            {
                result++;
            }
            if (eventWindow.DetailsTextBox.Text.Length > 0)
            {
                result++;
            }
            if (eventWindow.FrequencyComboBox.SelectedIndex > -1)
            {
                result++;
            }
            if (eventWindow.StatusComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (result == 5)
            {
                return true;
            }
            else
            {
                eventWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}