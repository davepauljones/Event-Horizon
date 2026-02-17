using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Event_Horizon
{
    public class DataTableManagementRams
    {
        public static EventHorizonRams EventHorizon_Rams = new EventHorizonRams();
        public static EventHorizonRamsProfile EventHorizon_RamsProfile = new EventHorizonRamsProfile();
        public static int RowLimitMode = RowLimitModes.LimitOnly;
        public static Int32 RowLimitStep = 30;
        public static Int32 RowLimit = RowLimitStep;
        public static Int32 RowLimitMin = 30;
        public static Int32 RowLimitMax = 300;
        public static Int32 RowOffsetStep = 30;
        public static Int32 RowOffset = 0;
        public static Int32 RowOffsetMin = 0;
        public static Int32 RowOffsetMax = 300;

        public static List<RamsProfileType> RamsProfileTypesList = new List<RamsProfileType>();

        public static List<EventHorizonRamsLINQ> GetRams(int listViewToPopulate, Int32 eventTypeID, Int32 filterMode, Int32 displayMode, string searchString)
        {
            List<EventHorizonRamsLINQ> _EventHorizonRamsLINQReturnList = new List<EventHorizonRamsLINQ>();

            MiscFunctions.PlayFile(AppDomain.CurrentDomain.BaseDirectory + "\\Audio\\claves.wav");
            MainWindow.mw.widgetDatabaseHealth.UpdateLastWriteLabel(true);

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd;

                            switch (RowLimitMode)
                            {
                                case RowLimitModes.NoLimit:
                                    sqlcmd = "SELECT * FROM Rams;";
                                    break;
                                case RowLimitModes.LimitOnly:
                                    sqlcmd = "SELECT * FROM Rams LIMIT @Limit;";
                                    break;
                                case RowLimitModes.LimitWithOffset:
                                    sqlcmd = "SELECT * FROM Rams LIMIT @Limit OFFSET @Offset;";
                                    break;
                                default:
                                    sqlcmd = "SELECT * FROM Rams LIMIT @Limit;";
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

                            adapter.Fill(EventHorizon_Rams);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetRams - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Rams.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                    where eventHorizonEvent.Field<string>("Details").Contains(searchString)
                    orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                    select eventHorizonEvent;
 
            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRamsLINQ eventHorizonRamsLINQ = new EventHorizonRamsLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRamsLINQ.ID)) eventHorizonRamsLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonRamsLINQ.EventTypeID)) eventHorizonRamsLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonRamsLINQ.SourceID)) eventHorizonRamsLINQ.SourceID = 0;

                eventHorizonRamsLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonRamsLINQ.FrequencyID)) eventHorizonRamsLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRamsLINQ.StatusID)) eventHorizonRamsLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRamsLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse targetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonRamsLINQ.TargetUserID)) eventHorizonRamsLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonRamsLINQ.ReadByMeID)) eventHorizonRamsLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.LastViewedDate = lastViewedDateTime;

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

                eventHorizonRamsLINQ.Source_ID = eventHorizonRamsLINQ.ID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonRamsLINQ.RemindMeID)) eventHorizonRamsLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonRamsLINQ.NotificationAcknowledged)) eventHorizonRamsLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["ParentEventID"].ToString(), out eventHorizonRamsLINQ.Source_ParentEventID)) eventHorizonRamsLINQ.Source_ParentEventID = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonRamsLINQ.EventModeID)) eventHorizonRamsLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonRamsLINQ.EventAttributeID)) eventHorizonRamsLINQ.EventAttributeID = 0;

                eventHorizonRamsLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonRamsLINQ.UnitCost)) eventHorizonRamsLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonRamsLINQ.Qty)) eventHorizonRamsLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonRamsLINQ.Discount)) eventHorizonRamsLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonRamsLINQ.Stock)) eventHorizonRamsLINQ.Stock = 0;

                eventHorizonRamsLINQ.Attributes_TotalDays = totalDays;
                eventHorizonRamsLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                if (displayMode == DisplayModes.Active)
                    _EventHorizonRamsLINQReturnList.Add(eventHorizonRamsLINQ);
                else if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                    _EventHorizonRamsLINQReturnList.Add(eventHorizonRamsLINQ);
            }
            return _EventHorizonRamsLINQReturnList;
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

                            adapter.Fill(EventHorizon_Rams);
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

                            adapter.Fill(EventHorizon_Rams);
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

            query = from eventHorizonEvent in EventHorizon_Rams.AsEnumerable()
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

        public static List<EventHorizonRamsLINQ> GetReplies(Int32 eventID)
        {
            List<EventHorizonRamsLINQ> _EventHorizonRamsLINQReturnList = new List<EventHorizonRamsLINQ>();

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Rams.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("ParentEventID") == eventID
                    where eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.NoteEvent || eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.ReplyEvent
                    orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") ascending
                    select eventHorizonEvent;

            DataView dataView = query.AsDataView();

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRamsLINQ eventHorizonRamsLINQ = new EventHorizonRamsLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRamsLINQ.ID)) eventHorizonRamsLINQ.ID = 0;
                if (!int.TryParse(dataRow["EventTypeID"].ToString(), out eventHorizonRamsLINQ.EventTypeID)) eventHorizonRamsLINQ.EventTypeID = 0;
                if (!int.TryParse(dataRow["SourceID"].ToString(), out eventHorizonRamsLINQ.SourceID)) eventHorizonRamsLINQ.SourceID = 0;

                eventHorizonRamsLINQ.Details = dataRow["Details"].ToString();

                if (!int.TryParse(dataRow["FrequencyID"].ToString(), out eventHorizonRamsLINQ.FrequencyID)) eventHorizonRamsLINQ.FrequencyID = 0;
                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRamsLINQ.StatusID)) eventHorizonRamsLINQ.StatusID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.CreationDate = createdDateTime;

                string targetDateTimeString = dataRow["TargetDateTime"].ToString();
                DateTime targetDateTime = DateTime.MinValue;
                if (DateTime.TryParse(targetDateTimeString, out targetDateTime))
                {
                    if (targetDateTime.TimeOfDay == TimeSpan.Zero)
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y");
                    else
                        targetDateTimeString = targetDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRamsLINQ.TargetDate = targetDateTime;
                }
                else
                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", targetDateTimeString);

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                if (!int.TryParse(dataRow["TargetUserID"].ToString(), out eventHorizonRamsLINQ.TargetUserID)) eventHorizonRamsLINQ.TargetUserID = 0;

                if (!int.TryParse(dataRow["ReadByMeID"].ToString(), out eventHorizonRamsLINQ.ReadByMeID)) eventHorizonRamsLINQ.ReadByMeID = 0;

                string lastViewedDateTimeString = dataRow["LastViewedDateTime"].ToString();
                DateTime lastViewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(lastViewedDateTimeString, out lastViewedDateTime)) lastViewedDateTimeString = lastViewedDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.LastViewedDate = lastViewedDateTime;

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

                eventHorizonRamsLINQ.Source_ID = eventHorizonRamsLINQ.ID;
                eventHorizonRamsLINQ.Source_ParentEventID = eventID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonRamsLINQ.RemindMeID)) eventHorizonRamsLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonRamsLINQ.NotificationAcknowledged)) eventHorizonRamsLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonRamsLINQ.EventModeID)) eventHorizonRamsLINQ.EventModeID = 0;

                if (!int.TryParse(dataRow["EventAttributeID"].ToString(), out eventHorizonRamsLINQ.EventAttributeID)) eventHorizonRamsLINQ.EventAttributeID = 0;

                eventHorizonRamsLINQ.PathFileName = dataRow["PathFileName"].ToString();

                if (!double.TryParse(dataRow["UnitCost"].ToString(), out eventHorizonRamsLINQ.UnitCost)) eventHorizonRamsLINQ.UnitCost = 0;

                if (!int.TryParse(dataRow["Qty"].ToString(), out eventHorizonRamsLINQ.Qty)) eventHorizonRamsLINQ.Qty = 0;

                if (!double.TryParse(dataRow["Discount"].ToString(), out eventHorizonRamsLINQ.Discount)) eventHorizonRamsLINQ.Discount = 0;

                if (!int.TryParse(dataRow["Stock"].ToString(), out eventHorizonRamsLINQ.Stock)) eventHorizonRamsLINQ.Stock = 0;

                eventHorizonRamsLINQ.Attributes_TotalDays = totalDays;
                eventHorizonRamsLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                _EventHorizonRamsLINQReturnList.Add(eventHorizonRamsLINQ);
            }

            return _EventHorizonRamsLINQReturnList;
        }

        public static void SaveEvent(RamsWindow ramsWindow, EventHorizonRamsLINQ eventHorizonRamsLINQ, int eventMode)
        {
            if (CheckFormFields(ramsWindow))
            {
                bool saveSuccessFull = false;

                string detailsSafeString = ramsWindow.DetailsTextBox.Text.Replace("'", "''");

                DateTime? createdDateTime = DateTime.Now;

                DateTime targetDateTimeNow = DateTime.Now;

                DateTime? targetDateTime = DateTime.Now;

                string query2 = "Select @@Identity";
                string query3 = "UPDATE Rams SET[ParentEventID] = ? WHERE [ID] = ?";

                Int32 id;
                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE Rams SET Description = ?, RamsProfileTypeID = ?, ClientName = ?, Site = ?, LocationActivity = ?, RevisionNo = ?, ElementReviewed = ?, ReviewedDateTime = ?, MSContractTitle = ?, MSRevisionNo = ?, MSContractor = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@Description", DbType.String).Value = detailsSafeString;
                                command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;
                                command.Parameters.Add("@ClientName", DbType.String).Value = detailsSafeString;
                                command.Parameters.Add("@Site", DbType.String).Value = detailsSafeString;
                                command.Parameters.Add("@LocationActivity", DbType.String).Value = detailsSafeString;

                                command.Parameters.Add("@RevisionNo", DbType.Int32).Value = ramsWindow.RevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@ElementReviewed", DbType.String).Value = detailsSafeString;

                                command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = targetDateTime;

                                command.Parameters.Add("@MSContractTitle", DbType.String).Value = detailsSafeString;

                                command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = ramsWindow.MSRevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@MSContractor", DbType.String).Value = detailsSafeString;

                                if (eventMode == EventWindowModes.ViewMainEvent || eventMode == EventWindowModes.ViewNote || eventMode == EventWindowModes.ViewReply || eventMode == EventWindowModes.EditMainEvent || eventMode == EventWindowModes.EditNote || eventMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || eventMode == EventWindowModes.NewEvent || eventMode == EventWindowModes.NewNote || eventMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Rams (CreatedDateTime, JobNo, Description, RamsProfileTypeID, UserID, ClientName, Site, LocationActivity, RevisionNo, ElementReviewed, ReviewedDateTime, MSContractTitle, MSRevisionNo, MSContractor) VALUES (@CreatedDateTime, @JobNo, @Description, @RamsProfileTypeID, @UserID, @ClientName, @Site, @LocationActivity, @RevisionNo, @ElementReviewed, @ReviewedDateTime, @MSContractTitle, @MSRevisionNo, @MSContractor);";

                                    command.Parameters.Add("@CreatedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@JobNo", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;

                                    command.Parameters.Add("@Description", DbType.String).Value = detailsSafeString;

                                    command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;
                                    
                                    command.Parameters.Add("@UserID", DbType.Int32).Value = XMLReaderWriter.UserID;

                                    command.Parameters.Add("@ClientName", DbType.String).Value = detailsSafeString;
                                    command.Parameters.Add("@Site", DbType.String).Value = detailsSafeString;
                                    command.Parameters.Add("@LocationActivity", DbType.String).Value = detailsSafeString;

                                    command.Parameters.Add("@RevisionNo", DbType.Int32).Value = ramsWindow.RevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@ElementsReviewed", DbType.String).Value = detailsSafeString;

                                    command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@MSContractTitle", DbType.String).Value = detailsSafeString;

                                    command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = ramsWindow.MSRevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@MSContractor", DbType.String).Value = detailsSafeString;

                                    command.ExecuteNonQuery();

                                    MainWindow.mw.Status.Content = "Successfully added a new rams";
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.mw.Status.Content = "Successfully updated a rams";
                    MainWindow.mw.ReminderListView.SelectedItem = null;
                    MainWindow.mw.RunningTask();
                }

                if (saveSuccessFull)
                {
                    ramsWindow.Close();
                    if (ramsWindow.ramsWindow != null) ramsWindow.ramsWindow.Close();
                    MainWindow.mw.ReminderListView.SelectedItem = null;
                }
            }
        }
        
        private static Int32 GetUserID(Int32 EventID)
        {
            Int32 ReturnUserID = 0;

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("SELECT UserID FROM Rams WHERE ID = ?", connection))
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
        
        public static void DeleteRams(Int32 EventID)
        {
            bool saveSuccessFull = false;

            if (XMLReaderWriter.UserID != 1)
            {
                if (GetUserID(EventID) != XMLReaderWriter.UserID)
                {
                    EventHorizonRequesterNotification rorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, you can only delete your own rams.", InformationTextBlock = "You could ask the user who created it, to delete it." }, RequesterTypes.OK);
                    rorn.ShowDialog();
                    return;
                }
            }

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Rams WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                saveSuccessFull = true;

                                MainWindow.mw.Status.Content = "Successfully deleted rams.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "DeleteRams - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
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

        public static void GetRamsProfiles()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                {
                    string sqlcmd = "SELECT * FROM RamsProfiles ORDER BY ID DESC;";

                    SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                    conn.Open();

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                    adapter.Fill(EventHorizon_RamsProfile);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("-------------------*---------------------");

                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetRamsProfiles - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }
        }
        public static bool GetRamsProfileTypes()
        {
            RamsProfileTypesList.Clear();

            RamsProfileTypesList.Add(new RamsProfileType { ID = 0, ProfileName = "All Rams Profile Types", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });
            RamsProfileTypesList.Add(new RamsProfileType { ID = 1, ProfileName = "Number 1", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });
            RamsProfileTypesList.Add(new RamsProfileType { ID = 2, ProfileName = "Number 2", Icon = FontAwesomeIcon.Star, Color = (Color)ColorConverter.ConvertFromString("#FFAAAAAA") });

            foreach (DataRow row in EventHorizon_RamsProfile.Rows)
            {
                RamsProfileTypesList.Add(new RamsProfileType { ID = row.Field<Int32>("ID"), ProfileName = row.Field<string>("ProfileName") });
            }

            return true;
        }
        
        private static bool CheckFormFields(RamsWindow ramsWindow)
        {
            int result = 0;

            if (ramsWindow.RamsProfileTypeComboBox.SelectedIndex == 0)
            {
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckFormFields", InformationTextBlock = "You can not choose 'All Rams' as an Rams Profile type." }, RequesterTypes.OK);
                msg.ShowDialog();
                return false;
            }

            if (ramsWindow.RamsProfileTypeComboBox.SelectedIndex > -1)
            {
                result++;
            }
            if (ramsWindow.DetailsTextBox.Text.Length > 0)
            {
                result++;
            }
            if (ramsWindow.RevisionNoComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (result == 5)
            {
                return true;
            }
            else
            {
                ramsWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}