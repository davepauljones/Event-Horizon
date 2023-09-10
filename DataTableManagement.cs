using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace The_Oracle
{
    public class DataTableManagement
    {
        public static string ConnectionString = string.Empty;

        public static EventHorizonEvent EventHorizon_Event = new EventHorizonEvent();

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static List<EventHorizonLINQ> GetEvents(int listViewToPopulate, Int32 eventTypeID, Int32 filterMode, Int32 displayMode)
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            try
            {
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEvents - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
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
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Log:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    default:
                        query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                where eventHorizonEvent.Field<Int32>("EventTypeID") == eventTypeID
                                where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
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
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Log:
                        switch (filterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    default:
                        query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
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
                eventHorizonLINQ.Source_Mode = EventWindowModes.ViewMainEvent;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["ParentEventID"].ToString(), out eventHorizonLINQ.Source_ParentEventID)) eventHorizonLINQ.Source_ParentEventID = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                if (displayMode == DisplayModes.Normal)
                    _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
                else if ((DateTime.Today + timeSpan) > targetDateTime.Date)
                    _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
            }

            return _EventHorizonLINQReturnList;
        }

        public static List<EventHorizonLINQ> GetReplies(Int32 eventID)
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("ParentEventID") == eventID
                    where eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.ReplyEvent
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
                eventHorizonLINQ.Source_Mode = EventWindowModes.ViewReplyNote;
                eventHorizonLINQ.Source_ParentEventID = eventID;

                if (!int.TryParse(dataRow["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                string remindMeDateTimeString = dataRow["RemindMeDateTime"].ToString();
                DateTime remindMeDateTime = DateTime.MinValue;
                if (DateTime.TryParse(remindMeDateTimeString, out remindMeDateTime)) remindMeDateTimeString = remindMeDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonLINQ.RemindMeDateTime = remindMeDateTime;

                if (!int.TryParse(dataRow["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                if (!int.TryParse(dataRow["EventModeID"].ToString(), out eventHorizonLINQ.EventModeID)) eventHorizonLINQ.EventModeID = 0;

                eventHorizonLINQ.Attributes_TotalDays = totalDays;
                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = iconEllipeColor;

                _EventHorizonLINQReturnList.Add(eventHorizonLINQ);
            }

            return _EventHorizonLINQReturnList;
        }

        private static bool saveSuccessFull = false;

        public static void SaveEvent(EventWindow eventWindow, EventHorizonLINQ eventHorizonLINQ, int eventMode)
        {
            if (CheckFormFields(eventWindow))
            {
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

                int id;

                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    int rowsAffected = 0;
                    using (var command = new OleDbCommand("UPDATE EventLog SET[EventTypeID] = ?, [SourceID] = ?, [Details] = ?, [FrequencyID] = ?, [StatusID] = ?, [TargetDateTime] = ?, [TargetUserID] = ?, [ReadByMeID] = ?, [LastViewedDateTime] = ?, [RemindMeID] = ?, [RemindMeDateTime] = ?, [NotificationAcknowledged] = ?, [ParentEventID] = ?, [EventModeID] = ? WHERE [ID] = ?", connection))
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

                        command.Parameters.AddWithValue("@ID", eventHorizonLINQ.ID);

                        if (eventMode == EventWindowModes.EditMainEvent || eventMode == EventWindowModes.EditReplyNote)
                            rowsAffected = command.ExecuteNonQuery();
                        else if (rowsAffected == 0 || eventMode == EventWindowModes.NewEvent || eventMode == EventWindowModes.NewNote || eventMode == EventWindowModes.NewReply)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "INSERT INTO EventLog (EventTypeID, SourceID, Details, FrequencyID, StatusID, CreatedDateTime, TargetDateTime, UserID, TargetUserID, ReadByMeID, LastViewedDateTime, RemindMeID, RemindMeDateTime, NotificationAcknowledged, ParentEventID, EventModeID) VALUES (@EventTypeID, @SourceID, @Details, @FrequencyID, @StatusID, @CreatedDateTime, @TargetDateTime, @UserID, @TargetUserID, @ReadByMeID, @LastViewedDateTime, @RemindMeID, @RemindMeDateTime, @NotificationAcknowledged, @ParentEventID, @EventModeID);";
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
                                    command.Parameters.AddWithValue("@EventModeID", EventModes.ReplyEvent);
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
                        }
                    }
                    saveSuccessFull = true;
                    if (rowsAffected > 0 ) MainWindow.mw.Status.Content = "Successfully updated an event";
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

            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUserID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            return ReturnUserID;
        }
        
        public static void DeleteEvent(Int32 EventID)
        {
            if (GetUserID(EventID) != XMLReaderWriter.UserID)
            {
                OracleRequesterNotification rorn = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, you can only delete your own events.", InformationTextBlock = "You could ask the user who created it, to delete it." }, RequesterTypes.OK);
                rorn.ShowDialog();
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand command = new OleDbCommand("DELETE FROM EventLog WHERE ID = ?", connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@ID", EventID);

                        command.ExecuteNonQuery();

                        saveSuccessFull = true;

                        MainWindow.mw.EventLogListViewTagged = -1;

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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "DeleteEvent - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            if (saveSuccessFull)
            {
                if (MainWindow.mw.DisplayMode == DisplayModes.Reminders)
                    MainWindow.mw.RefreshLog(ListViews.Reminder);
                else
                    MainWindow.mw.RefreshLog(ListViews.Log);
            }
        }

        public static void GetUsersLastTimeOnline()
        {
            DateTime LastTimeOnlineDateTime = DateTime.MinValue;

            //String SqlString = "SELECT * FROM Users WHERE ID IN(SELECT ID FROM Users WHERE LastTimeOnline = (SELECT MAX(LastTimeOnline) FROM Users)) AND ID = " + UsersID + " ORDER BY LastTimeOnline DESC;";
            String SqlString = "SELECT * FROM Users;";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            MainWindow.UsersLastTimeOnlineDictionary.Clear();

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

                                    Console.Write("LastTimeOnline = ");
                                    Console.WriteLine(LastTimeOnlineString);

                                    if (!MainWindow.UsersLastTimeOnlineDictionary.ContainsKey(UserID))
                                    {
                                        MainWindow.UsersLastTimeOnlineDictionary.Add(UserID, LastTimeOnlineDateTime);
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetUsersLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }
        }
        
        public static void InsertOrUpdateLastTimeOnline(Int32 UserID)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "InsertOrUpdateLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }
        }
        
        public static void UpdateLastTimeOnline(Int32 UserID)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand updateCommand = new OleDbCommand("UPDATE Users SET [LastTimeOnline] = ? WHERE [ID] = ?", connection))
                    {
                        connection.Open();

                        updateCommand.Parameters.Add("LastTimeOnline", OleDbType.Date);
                        updateCommand.Parameters["LastTimeOnline"].Value = DateTime.Now;
                        updateCommand.Parameters.AddWithValue("@ID", UserID);

                        updateCommand.ExecuteNonQuery();
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }
        }

        public static List<EventHorizonLINQ> GetMyUnread()
        {
            List<EventHorizonLINQ> ReturnUnread = new List<EventHorizonLINQ>();
            EventHorizonLINQ eventHorizonLINQ;

            String SqlString = "SELECT * FROM EventLog WHERE StatusID=" + Statuses.Active + " AND TargetUserID=" + XMLReaderWriter.UserID + " ORDER BY LastViewedDateTime DESC;";

            Console.Write("SqlString = ");
            Console.WriteLine(SqlString);

            try
            {
                using (OleDbConnection conn = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, conn))
                    {
                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                eventHorizonLINQ = new EventHorizonLINQ();


                                if (!int.TryParse(reader["ID"].ToString(), out eventHorizonLINQ.ID)) eventHorizonLINQ.ID = 0;
                                if (!int.TryParse(reader["EventTypeID"].ToString(), out eventHorizonLINQ.EventTypeID)) eventHorizonLINQ.EventTypeID = 0;
                                if (!int.TryParse(reader["SourceID"].ToString(), out eventHorizonLINQ.SourceID)) eventHorizonLINQ.SourceID = 0;

                                eventHorizonLINQ.Details = reader["Details"].ToString();

                                if (!int.TryParse(reader["FrequencyID"].ToString(), out eventHorizonLINQ.FrequencyID)) eventHorizonLINQ.FrequencyID = 0;
                                if (!int.TryParse(reader["StatusID"].ToString(), out eventHorizonLINQ.StatusID)) eventHorizonLINQ.StatusID = 0;

                                String CreatedDateTimeString = reader[EventLogFields.CreatedDateTime].ToString();
                                DateTime cdt = DateTime.MinValue;
                                if (DateTime.TryParse(CreatedDateTimeString, out cdt)) CreatedDateTimeString = cdt.ToString("dd/MM/y HH:mm");
                                eventHorizonLINQ.CreationDate = cdt;

                                String TargetDateTimeString = reader[EventLogFields.TargetDateTime].ToString();
                                DateTime tdt = DateTime.MinValue;
                                if (DateTime.TryParse(TargetDateTimeString, out tdt))
                                {
                                    if (tdt.TimeOfDay == TimeSpan.Zero)
                                        TargetDateTimeString = tdt.ToString("dd/MM/y");
                                    else
                                        TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");

                                    eventHorizonLINQ.TargetDate = tdt;
                                }
                                else
                                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);

                                if (!int.TryParse(reader["UserID"].ToString(), out eventHorizonLINQ.UserID)) eventHorizonLINQ.UserID = 0;

                                if (!int.TryParse(reader["TargetUserID"].ToString(), out eventHorizonLINQ.TargetUserID)) eventHorizonLINQ.TargetUserID = 0;

                                if (!int.TryParse(reader["ReadByMeID"].ToString(), out eventHorizonLINQ.ReadByMeID)) eventHorizonLINQ.ReadByMeID = 0;

                                String LastViewedDateTimeString = reader[EventLogFields.LastViewedDateTime].ToString();
                                DateTime lvdt = DateTime.MinValue;
                                if (DateTime.TryParse(LastViewedDateTimeString, out lvdt)) LastViewedDateTimeString = lvdt.ToString("dd/MM/y HH:mm");
                                eventHorizonLINQ.LastViewedDate = lvdt;

                                TimeSpan ts = MainWindow.mw.ReminderListTimeSpan;

                                int TotalDays = Convert.ToInt32((tdt.Date - DateTime.Today).Days);
                                Color IconEllipeColor = Colors.Pink;

                                if ((DateTime.Today + ts) > tdt.Date)
                                {
                                    switch (TotalDays)
                                    {
                                        case int n when (n <= 0):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");
                                            break;
                                        case int n when (n > 0 && n <= 3):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFff7800");
                                            break;
                                        case int n when (n > 3 && n <= 7):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF4cbb17");
                                            break;
                                        case int n when (n > 7 && n <= 14):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FF9fee79");
                                            break;
                                        case int n when (n > 14 && n <= 28):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFcff6bb");
                                            break;
                                        case int n when (n > 28):
                                            IconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe7fadd");
                                            break;
                                    }
                                }

                                eventHorizonLINQ.Attributes_TotalDays = TotalDays;
                                eventHorizonLINQ.Attributes_TotalDaysEllipseColor = IconEllipeColor;

                                if (!int.TryParse(reader["RemindMeID"].ToString(), out eventHorizonLINQ.RemindMeID)) eventHorizonLINQ.RemindMeID = 0;

                                String RemindMeDateTimeString = reader[EventLogFields.RemindMeDateTime].ToString();
                                DateTime rmdt = DateTime.MinValue;
                                if (DateTime.TryParse(RemindMeDateTimeString, out rmdt)) RemindMeDateTimeString = rmdt.ToString("dd/MM/y HH:mm");
                                eventHorizonLINQ.RemindMeDateTime = rmdt;

                                if (!int.TryParse(reader["NotificationAcknowledged"].ToString(), out eventHorizonLINQ.NotificationAcknowledged)) eventHorizonLINQ.NotificationAcknowledged = 0;

                                eventHorizonLINQ.Source_Mode = EventWindowModes.ViewReplyNote;

                                if (!int.TryParse(reader["ParentEventID"].ToString(), out eventHorizonLINQ.Source_ParentEventID)) eventHorizonLINQ.Source_ParentEventID = 0;

                                //Test to see if user has already viewed the notification
                                if (eventHorizonLINQ.RemindMeID == RemindMeModes.No && eventHorizonLINQ.NotificationAcknowledged == NotificationAcknowlegedModes.No) ReturnUnread.Add(eventHorizonLINQ);

                                Console.Write("eventHorizonLINQ.ID = ");
                                Console.WriteLine(eventHorizonLINQ.ID);
                                Console.Write("eventHorizonLINQ.UserID = ");
                                Console.WriteLine(eventHorizonLINQ.UserID);
                                Console.Write("eventHorizonLINQ.Details = ");
                                Console.WriteLine(eventHorizonLINQ.Details);
                                Console.Write("eventHorizonLINQ.TargetUserID = ");
                                Console.WriteLine(eventHorizonLINQ.TargetUserID);
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetMyUnread - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            return ReturnUnread;
        }
        
        public static  List<EventHorizonLINQ> GetMyReminders()
        {
            List<EventHorizonLINQ> ReturnReminders = new List<EventHorizonLINQ>();
            EventHorizonLINQ oe;

            String SqlString = "SELECT * FROM EventLog WHERE StatusID=" + Statuses.Active + " AND TargetUserID=" + XMLReaderWriter.UserID + " ORDER BY RemindMeDateTime DESC;";

            Console.Write("SqlString = ");
            Console.WriteLine(SqlString);

            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(SqlString, connection))
                    {
                        connection.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                oe = new EventHorizonLINQ();
                                oe.ID = int.Parse(reader["ID"].ToString());
                                oe.UserID = int.Parse(reader["UserID"].ToString());
                                oe.Details = reader["Details"].ToString();
                                oe.TargetUserID = int.Parse(reader["TargetUserID"].ToString());

                                Int32 RemindMe;

                                bool success = int.TryParse(reader["RemindMeID"].ToString(), out RemindMe);
                                if (!success) RemindMe = 0;

                                DateTime rmdt = DateTime.MinValue;
                                String rmdtString = reader["RemindMeDateTime"].ToString();

                                Int32 NotificationAcknowleged;

                                bool success2 = int.TryParse(reader["NotificationAcknowledged"].ToString(), out NotificationAcknowleged);
                                if (!success2) NotificationAcknowleged = 0;

                                if (DateTime.TryParse(rmdtString, out rmdt))
                                {
                                    Console.WriteLine(rmdt);

                                    if (rmdt.TimeOfDay == TimeSpan.Zero)
                                    {
                                        rmdtString = rmdt.ToString("dd/MM/y");
                                    }
                                    else
                                    {
                                        rmdtString = rmdt.ToString("dd/MM/y HH:mm");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Unable to parse rmdtString '{0}'", rmdtString);
                                }

                                if (DateTime.Now >= rmdt && RemindMe == RemindMeModes.Yes)
                                {
                                    if (NotificationAcknowleged == NotificationAcknowlegedModes.No) ReturnReminders.Add(oe);
                                }

                                Console.Write("oe.ID = ");
                                Console.WriteLine(oe.ID);
                                Console.Write("oe.UserID = ");
                                Console.WriteLine(oe.UserID);
                                Console.Write("oe.Details = ");
                                Console.WriteLine(oe.Details);
                                Console.Write("oe.TargetUserID = ");
                                Console.WriteLine(oe.TargetUserID);
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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetMyReminders - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            return ReturnReminders;
        }

        public static void UpdateMyReminder(Int32 EventID, int ReminderMeID, DateTime RemindMeDateTime, int NotificationAcknowledged)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
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

                        saveSuccessFull = true;

                        MainWindow.mw.EventLogListViewTagged = -1;

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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateMyReminder - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            if (saveSuccessFull)
            {
                //if (DisplayMode == DisplayModes.Reminders)
                //    RefreshLog(ListViews.Reminder);
                //else
                //    RefreshLog(ListViews.Log);
            }
        }
        
        public static void UpdateStatusID(Int32 EventID, int StatusID)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand updateCommand = new OleDbCommand("UPDATE EventLog SET [StatusID] = ? WHERE [ID] = ?", connection))
                    {
                        connection.Open();

                        updateCommand.Parameters.AddWithValue("@StatusID", StatusID);
                        updateCommand.Parameters.AddWithValue("@ID", EventID);

                        updateCommand.ExecuteNonQuery();

                        saveSuccessFull = true;

                        MainWindow.mw.EventLogListViewTagged = -1;

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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateStatusID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            if (saveSuccessFull)
            {
                //if (DisplayMode == DisplayModes.Reminders)
                //    RefreshLog(ListViews.Reminder);
                //else
                //    RefreshLog(ListViews.Log);
            }
        }
        
        public static void UpdateReadByMeID(Int32 EventID, int ReadByMeID)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (OleDbCommand updateCommand = new OleDbCommand("UPDATE EventLog SET [ReadByMeID] = ?, [LastViewedDateTime] = ? WHERE [ID] = ?", connection))
                    {
                        connection.Open();

                        updateCommand.Parameters.AddWithValue("@ReadByMeID", ReadByMeID);
                        updateCommand.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                        updateCommand.Parameters["LastViewedDateTime"].Value = DateTime.Now;
                        updateCommand.Parameters.AddWithValue("@ID", EventID);

                        updateCommand.ExecuteNonQuery();

                        saveSuccessFull = true;

                        MainWindow.mw.EventLogListViewTagged = -1;

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

                    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "UpdateReadByMeID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState }, RequesterTypes.OK);
                    msg.ShowDialog();
                }
            }

            if (saveSuccessFull)
            {
                //if (DisplayMode == DisplayModes.Reminders)
                //    RefreshLog(ListViews.Reminder);
                //else
                //    RefreshLog(ListViews.Log);
            }
        }

        private static bool CheckFormFields(EventWindow eventWindow)
        {
            int result = 0;

            if (eventWindow.EventTypeComboBox.SelectedIndex == 0)
            {
                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckFormFields", InformationTextBlock = "You can not choose 'All Events' as an event type." }, RequesterTypes.OK);
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
        
        public static Int32 GetTotalRows()
        {
            int count = 0;
            string cmdstr = "SELECT COUNT(*) FROM EventLog";

            using (OleDbConnection conn = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
            using (OleDbCommand command = new OleDbCommand(cmdstr, conn))
            {
                conn.Open();
                count = (int)command.ExecuteScalar();
            }

            return count;
        }
    }
}