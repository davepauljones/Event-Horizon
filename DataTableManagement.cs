using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.OleDb;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace The_Oracle
{
    public class DataTableManagement
    {
        public static string ConnectionString = string.Empty;

        public static EventHorizonEvent EventHorizon_Event = new EventHorizonEvent();

        public static void SetConnectionString(string _ConnectionString)
        {
            ConnectionString = _ConnectionString;
        }

        public static List<EventHorizonLINQ> GetEvents(int ListViewToPopulate, Int32 EventTypeID, Int32 FilterMode, Int32 DisplayMode)
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "GetEvents - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.Show();
                }
            }

            EnumerableRowCollection<DataRow> query;

            if (EventTypeID != 0)
            {
                switch (ListViewToPopulate)
                {
                    case ListViews.Reminder:
                        switch (FilterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                        orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") ascending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    case ListViews.Log:
                        switch (FilterMode)
                        {
                            case FilterModes.None:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginOrTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID || eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            case FilterModes.OriginAndTargetIsMe:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        where eventHorizonEvent.Field<Int32>("UserID") == XMLReaderWriter.UserID
                                        where eventHorizonEvent.Field<Int32>("TargetUserID") == XMLReaderWriter.UserID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                            default:
                                query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                        where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                        orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") descending
                                        select eventHorizonEvent;
                                break;
                        }
                        break;
                    default:
                        query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                                where eventHorizonEvent.Field<Int32>("EventTypeID") == EventTypeID
                                where eventHorizonEvent.Field<Int32>("StatusID") >= Statuses.Active && eventHorizonEvent.Field<Int32>("StatusID") <= Statuses.ActiveNotifiedRead
                                orderby eventHorizonEvent.Field<DateTime>("TargetDateTime") descending
                                select eventHorizonEvent;
                        break;
                }
            }
            else
            {
                switch (ListViewToPopulate)
                {
                    case ListViews.Reminder:
                        switch (FilterMode)
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
                        switch (FilterMode)
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
 
            DataView view = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dr in view.ToTable().Rows)
            {
                EventHorizonLINQ evhl = new EventHorizonLINQ();

                if (!int.TryParse(dr["ID"].ToString(), out evhl.ID)) evhl.ID = 0;
                if (!int.TryParse(dr["EventTypeID"].ToString(), out evhl.EventTypeID)) evhl.EventTypeID = 0;
                if (!int.TryParse(dr["SourceID"].ToString(), out evhl.SourceID)) evhl.SourceID = 0;

                evhl.Details = dr["Details"].ToString();

                if (!int.TryParse(dr["FrequencyID"].ToString(), out evhl.FrequencyID)) evhl.FrequencyID = 0;
                if (!int.TryParse(dr["StatusID"].ToString(), out evhl.StatusID)) evhl.StatusID = 0;

                String CreatedDateTimeString = dr["CreatedDateTime"].ToString();
                DateTime cdt = DateTime.MinValue;
                if (DateTime.TryParse(CreatedDateTimeString, out cdt)) CreatedDateTimeString = cdt.ToString("dd/MM/y HH:mm");
                evhl.CreationDate = cdt;

                String TargetDateTimeString = dr["TargetDateTime"].ToString();
                DateTime tdt = DateTime.MinValue;
                if (DateTime.TryParse(TargetDateTimeString, out tdt))
                {
                    if (tdt.TimeOfDay == TimeSpan.Zero)
                        TargetDateTimeString = tdt.ToString("dd/MM/y");
                    else
                        TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");

                    evhl.TargetDate = tdt;
                }
                else
                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);

                if (!int.TryParse(dr["UserID"].ToString(), out evhl.UserID)) evhl.UserID = 0;

                if (!int.TryParse(dr["TargetUserID"].ToString(), out evhl.TargetUserID)) evhl.TargetUserID = 0;

                if (!int.TryParse(dr["ReadByMeID"].ToString(), out evhl.ReadByMeID)) evhl.ReadByMeID = 0;

                String LastViewedDateTimeString = dr["LastViewedDateTime"].ToString();
                DateTime lvdt = DateTime.MinValue;
                if (DateTime.TryParse(LastViewedDateTimeString, out lvdt)) LastViewedDateTimeString = lvdt.ToString("dd/MM/y HH:mm");
                evhl.LastViewedDate = lvdt;

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

                evhl.Source_ID = evhl.ID;
                evhl.Source_Mode = EventWindowModes.ViewEvent;
                //evhl.Source_ParentEventID = evhl.ID;

                if (!int.TryParse(dr["RemindMeID"].ToString(), out evhl.RemindMeID)) evhl.RemindMeID = 0;

                String RemindMeDateTimeString = dr["RemindMeDateTime"].ToString();
                DateTime rmdt = DateTime.MinValue;
                if (DateTime.TryParse(RemindMeDateTimeString, out rmdt)) RemindMeDateTimeString = rmdt.ToString("dd/MM/y HH:mm");
                evhl.RemindMeDateTime = rmdt;

                if (!int.TryParse(dr["NotificationAcknowledged"].ToString(), out evhl.NotificationAcknowledged)) evhl.NotificationAcknowledged = 0;

                if (!int.TryParse(dr["ParentEventID"].ToString(), out evhl.Source_ParentEventID)) evhl.Source_ParentEventID = 0;

                if (!int.TryParse(dr["EventModeID"].ToString(), out evhl.EventModeID)) evhl.EventModeID = 0;

                evhl.Attributes_TotalDays = TotalDays;
                evhl.Attributes_TotalDaysEllipseColor = IconEllipeColor;

                if (DisplayMode == DisplayModes.Normal)
                    _EventHorizonLINQReturnList.Add(evhl);
                else if ((DateTime.Today + ts) > tdt.Date)
                    _EventHorizonLINQReturnList.Add(evhl);
            }

            return _EventHorizonLINQReturnList;
        }

        public static List<EventHorizonLINQ> GetReplies(Int32 EventID)
        {
            List<EventHorizonLINQ> _EventHorizonLINQReturnList = new List<EventHorizonLINQ>();

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonEvent in EventHorizon_Event.AsEnumerable()
                    where eventHorizonEvent.Field<Int32>("ParentEventID") == EventID
                    where eventHorizonEvent.Field<Int32>("EventModeID") == EventModes.ReplyEvent
                    orderby eventHorizonEvent.Field<DateTime>("CreatedDateTime") ascending
                    select eventHorizonEvent;

            DataView view = query.AsDataView();

            foreach (DataRow dr in view.ToTable().Rows)
            {
                EventHorizonLINQ evhl = new EventHorizonLINQ();

                if (!int.TryParse(dr["ID"].ToString(), out evhl.ID)) evhl.ID = 0;
                if (!int.TryParse(dr["EventTypeID"].ToString(), out evhl.EventTypeID)) evhl.EventTypeID = 0;
                if (!int.TryParse(dr["SourceID"].ToString(), out evhl.SourceID)) evhl.SourceID = 0;

                evhl.Details = dr["Details"].ToString();

                if (!int.TryParse(dr["FrequencyID"].ToString(), out evhl.FrequencyID)) evhl.FrequencyID = 0;
                if (!int.TryParse(dr["StatusID"].ToString(), out evhl.StatusID)) evhl.StatusID = 0;

                String CreatedDateTimeString = dr["CreatedDateTime"].ToString();
                DateTime cdt = DateTime.MinValue;
                if (DateTime.TryParse(CreatedDateTimeString, out cdt)) CreatedDateTimeString = cdt.ToString("dd/MM/y HH:mm");
                evhl.CreationDate = cdt;

                String TargetDateTimeString = dr["TargetDateTime"].ToString();
                DateTime tdt = DateTime.MinValue;
                if (DateTime.TryParse(TargetDateTimeString, out tdt))
                {
                    if (tdt.TimeOfDay == TimeSpan.Zero)
                        TargetDateTimeString = tdt.ToString("dd/MM/y");
                    else
                        TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");

                    evhl.TargetDate = tdt;
                }
                else
                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);

                if (!int.TryParse(dr["UserID"].ToString(), out evhl.UserID)) evhl.UserID = 0;

                if (!int.TryParse(dr["TargetUserID"].ToString(), out evhl.TargetUserID)) evhl.TargetUserID = 0;

                if (!int.TryParse(dr["ReadByMeID"].ToString(), out evhl.ReadByMeID)) evhl.ReadByMeID = 0;

                String LastViewedDateTimeString = dr["LastViewedDateTime"].ToString();
                DateTime lvdt = DateTime.MinValue;
                if (DateTime.TryParse(LastViewedDateTimeString, out lvdt)) LastViewedDateTimeString = lvdt.ToString("dd/MM/y HH:mm");
                evhl.LastViewedDate = lvdt;

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

                evhl.Source_ID = evhl.ID;
                evhl.Source_Mode = EventWindowModes.ViewEvent;
                evhl.Source_ParentEventID = EventID;

                if (!int.TryParse(dr["RemindMeID"].ToString(), out evhl.RemindMeID)) evhl.RemindMeID = 0;

                String RemindMeDateTimeString = dr["RemindMeDateTime"].ToString();
                DateTime rmdt = DateTime.MinValue;
                if (DateTime.TryParse(RemindMeDateTimeString, out rmdt)) RemindMeDateTimeString = rmdt.ToString("dd/MM/y HH:mm");
                evhl.RemindMeDateTime = rmdt;

                if (!int.TryParse(dr["NotificationAcknowledged"].ToString(), out evhl.NotificationAcknowledged)) evhl.NotificationAcknowledged = 0;

                if (!int.TryParse(dr["EventModeID"].ToString(), out evhl.EventModeID)) evhl.EventModeID = 0;

                evhl.Attributes_TotalDays = TotalDays;
                evhl.Attributes_TotalDaysEllipseColor = IconEllipeColor;

                _EventHorizonLINQReturnList.Add(evhl);
            }

            return _EventHorizonLINQReturnList;
        }

        private static bool SaveSuccessFull = false;

        public static void SaveEvent(EventWindow ev, EventHorizonLINQ ehl, int EventMode)
        {
            if (CheckFormFields(ev))
            {
                string DetailsSafeString = ev.DetailsTextBox.Text.Replace("'", "''");

                DateTime? cdt = DateTime.Now;

                string ttt = string.Empty;

                //ttt = TargetTimeTextBox.Text;
                //ttt += ":00";

                ttt = ev.TargetTimeHoursPicker.Text;
                ttt += ":";
                ttt += ev.TargetTimeMinutesPicker.Text;
                ttt += ":00";

                DateTime ttimedt = DateTime.ParseExact(ttt, "HH:mm:ss", CultureInfo.InvariantCulture);

                DateTime tdtnow = DateTime.Now;

                DateTime? tdt = DateTime.Now;

                if (ttt == "00:00:00")
                    tdt = new DateTime(ev.TargetDatePicker.SelectedDate.Value.Year, ev.TargetDatePicker.SelectedDate.Value.Month, ev.TargetDatePicker.SelectedDate.Value.Day, 0, 0, 0);
                else
                    tdt = new DateTime(ev.TargetDatePicker.SelectedDate.Value.Year, ev.TargetDatePicker.SelectedDate.Value.Month, ev.TargetDatePicker.SelectedDate.Value.Day, ttimedt.Hour, ttimedt.Minute, ttimedt.Second);

                string query2 = "Select @@Identity";
                string query3 = "UPDATE EventLog SET[ParentEventID] = ? WHERE [ID] = ?";

                int ID;

                using (OleDbConnection connection = new OleDbConnection(MainWindow.HSE_LOG_GlobalMDBConnectionString))
                {
                    using (var command = new OleDbCommand("UPDATE EventLog SET[EventTypeID] = ?, [SourceID] = ?, [Details] = ?, [FrequencyID] = ?, [StatusID] = ?, [TargetDateTime] = ?, [TargetUserID] = ?, [ReadByMeID] = ?, [LastViewedDateTime] = ?, [RemindMeID] = ?, [RemindMeDateTime] = ?, [NotificationAcknowledged] = ?, [ParentEventID] = ?, [EventModeID] = ? WHERE [ID] = ?", connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@EventTypeID", ev.EventTypeComboBox.SelectedIndex);
                        command.Parameters.AddWithValue("@SourceID", ev.SourceComboBox.SelectedIndex);
                        command.Parameters.AddWithValue("@Details", ev.DetailsTextBox.Text);
                        command.Parameters.AddWithValue("@FrequencyID", ev.FrequencyComboBox.SelectedIndex);
                        command.Parameters.AddWithValue("@StatusID", ev.StatusComboBox.SelectedIndex);

                        command.Parameters.Add("TargetDateTime", OleDbType.Date);
                        command.Parameters["TargetDateTime"].Value = tdt;

                        command.Parameters.AddWithValue("@TargetUserID", ev.TargetUserIDComboBox.SelectedIndex);
                        command.Parameters.AddWithValue("@ReadByMeID", ehl.ReadByMeID);

                        command.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                        command.Parameters["LastViewedDateTime"].Value = DateTime.MinValue;

                        command.Parameters.AddWithValue("@RemindMeID", ehl.RemindMeID);

                        command.Parameters.Add("RemindMeDateTime", OleDbType.Date);
                        command.Parameters["RemindMeDateTime"].Value = ehl.RemindMeDateTime;

                        command.Parameters.AddWithValue("@NotificationAcknowledged", 0);

                        command.Parameters.AddWithValue("@ParentEventID", ehl.Source_ParentEventID);
                        command.Parameters.AddWithValue("@EventModeID", ehl.EventModeID);

                        command.Parameters.AddWithValue("@ID", ehl.ID);

                        int rowsAffected = 0;

                        if (EventMode == EventWindowModes.EditEvent || EventMode == EventWindowModes.EditReply)
                            rowsAffected = command.ExecuteNonQuery();
                        else if (rowsAffected == 0 || EventMode == EventWindowModes.NewEvent || EventMode == EventWindowModes.NewNote || EventMode == EventWindowModes.NewReply)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "INSERT INTO EventLog (EventTypeID, SourceID, Details, FrequencyID, StatusID, CreatedDateTime, TargetDateTime, UserID, TargetUserID, ReadByMeID, LastViewedDateTime, RemindMeID, RemindMeDateTime, NotificationAcknowledged, ParentEventID, EventModeID) VALUES (@EventTypeID, @SourceID, @Details, @FrequencyID, @StatusID, @CreatedDateTime, @TargetDateTime, @UserID, @TargetUserID, @ReadByMeID, @LastViewedDateTime, @RemindMeID, @RemindMeDateTime, @NotificationAcknowledged, @ParentEventID, @EventModeID);";
                            command.Parameters.AddWithValue("@EventTypeID", ev.EventTypeComboBox.SelectedIndex);
                            command.Parameters.AddWithValue("@SourceID", ev.SourceComboBox.SelectedIndex);
                            command.Parameters.AddWithValue("@Details", ev.DetailsTextBox.Text);
                            command.Parameters.AddWithValue("@FrequencyID", ev.FrequencyComboBox.SelectedIndex);
                            command.Parameters.AddWithValue("@StatusID", ev.StatusComboBox.SelectedIndex);

                            command.Parameters.Add("CreatedDateTime", OleDbType.Date);
                            command.Parameters["CreatedDateTime"].Value = cdt;

                            command.Parameters.Add("TargetDateTime", OleDbType.Date);
                            command.Parameters["TargetDateTime"].Value = tdt;

                            command.Parameters.AddWithValue("@UserID", XMLReaderWriter.UserID);
                            command.Parameters.AddWithValue("@TargetUserID", ev.TargetUserIDComboBox.SelectedIndex);
                            command.Parameters.AddWithValue("@ReadByMeID", 0);

                            command.Parameters.Add("LastViewedDateTime", OleDbType.Date);
                            command.Parameters["LastViewedDateTime"].Value = DateTime.MinValue;

                            command.Parameters.AddWithValue("@RemindMeID", 0);

                            command.Parameters.Add("RemindMeDateTime", OleDbType.Date);
                            command.Parameters["RemindMeDateTime"].Value = DateTime.MinValue;

                            command.Parameters.AddWithValue("@NotificationAcknowledged", 0);

                            switch (EventMode)
                            {
                                case EventWindowModes.NewEvent:
                                    command.Parameters.AddWithValue("@ParentEventID", 0);
                                    command.Parameters.AddWithValue("@EventModeID", EventModes.MainEvent);
                                    break;
                                case EventWindowModes.NewNote:
                                    command.Parameters.AddWithValue("@ParentEventID", ehl.Source_ParentEventID);
                                    command.Parameters.AddWithValue("@EventModeID", EventModes.ReplyEvent);
                                    break;
                                case EventWindowModes.NewReply:
                                    command.Parameters.AddWithValue("@ParentEventID", ehl.Source_ParentEventID);
                                    command.Parameters.AddWithValue("@EventModeID", EventModes.ReplyEvent);
                                    break;
                                default:
                                    command.Parameters.AddWithValue("@ParentEventID", 0);
                                    command.Parameters.AddWithValue("@EventModeID", EventModes.MainEvent);
                                    break;
                            }

                            command.ExecuteNonQuery();

                            if (EventMode == EventWindowModes.NewEvent)
                            {
                                command.CommandText = query2;

                                ID = (int)command.ExecuteScalar();
                                Console.Write("ID = (int)command.ExecuteScalar(); = ");
                                Console.WriteLine(ID);

                                command.Parameters.Clear();
                                command.CommandText = query3;
                                command.Parameters.AddWithValue("@ParentEventID", ID);
                                command.Parameters.AddWithValue("@ID", ID);
                                command.ExecuteNonQuery();
                            }

                            MainWindow.mw.Status.Content = "Successfully added a new reply";
                        }
                    }
                }
                
                SaveSuccessFull = true;

                if (SaveSuccessFull)
                {
                    //if (mw.DisplayMode == DisplayModes.Reminders)
                    //    mw.RefreshLog(ListViews.Reminder);
                    //else
                    //    mw.RefreshLog(ListViews.Log);
                    ev.Close();
                    if (ev.ew != null) ev.ew.Close();
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "GetUserID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }

            return ReturnUserID;
        }
        
        public static void DeleteEvent(Int32 EventID)
        {
            if (GetUserID(EventID) != XMLReaderWriter.UserID)
            {
                var Result = MessageBox.Show("You can only delete your own events.", "Delete this event", MessageBoxButton.OK, MessageBoxImage.Information);
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

                        SaveSuccessFull = true;

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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "DeleteEvent - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }

            if (SaveSuccessFull)
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "GetUsersLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "InsertOrUpdateLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "UpdateLastTimeOnline - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }
        }

        public static List<EventHorizonLINQ> GetMyUnread()
        {
            List<EventHorizonLINQ> ReturnUnread = new List<EventHorizonLINQ>();
            EventHorizonLINQ _OracleEvent;

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
                                _OracleEvent = new EventHorizonLINQ();


                                if (!int.TryParse(reader["ID"].ToString(), out _OracleEvent.ID)) _OracleEvent.ID = 0;
                                if (!int.TryParse(reader["EventTypeID"].ToString(), out _OracleEvent.EventTypeID)) _OracleEvent.EventTypeID = 0;
                                if (!int.TryParse(reader["SourceID"].ToString(), out _OracleEvent.SourceID)) _OracleEvent.SourceID = 0;

                                _OracleEvent.Details = reader["Details"].ToString();

                                if (!int.TryParse(reader["FrequencyID"].ToString(), out _OracleEvent.FrequencyID)) _OracleEvent.FrequencyID = 0;
                                if (!int.TryParse(reader["StatusID"].ToString(), out _OracleEvent.StatusID)) _OracleEvent.StatusID = 0;

                                String CreatedDateTimeString = reader[EventLogFields.CreatedDateTime].ToString();
                                DateTime cdt = DateTime.MinValue;
                                if (DateTime.TryParse(CreatedDateTimeString, out cdt)) CreatedDateTimeString = cdt.ToString("dd/MM/y HH:mm");
                                _OracleEvent.CreationDate = cdt;

                                String TargetDateTimeString = reader[EventLogFields.TargetDateTime].ToString();
                                DateTime tdt = DateTime.MinValue;
                                if (DateTime.TryParse(TargetDateTimeString, out tdt))
                                {
                                    if (tdt.TimeOfDay == TimeSpan.Zero)
                                        TargetDateTimeString = tdt.ToString("dd/MM/y");
                                    else
                                        TargetDateTimeString = tdt.ToString("dd/MM/y HH:mm");

                                    _OracleEvent.TargetDate = tdt;
                                }
                                else
                                    Console.WriteLine("Unable to parse TargetDateTimeString '{0}'", TargetDateTimeString);

                                if (!int.TryParse(reader["UserID"].ToString(), out _OracleEvent.UserID)) _OracleEvent.UserID = 0;

                                if (!int.TryParse(reader["TargetUserID"].ToString(), out _OracleEvent.TargetUserID)) _OracleEvent.TargetUserID = 0;

                                if (!int.TryParse(reader["ReadByMeID"].ToString(), out _OracleEvent.ReadByMeID)) _OracleEvent.ReadByMeID = 0;

                                String LastViewedDateTimeString = reader[EventLogFields.LastViewedDateTime].ToString();
                                DateTime lvdt = DateTime.MinValue;
                                if (DateTime.TryParse(LastViewedDateTimeString, out lvdt)) LastViewedDateTimeString = lvdt.ToString("dd/MM/y HH:mm");
                                _OracleEvent.LastViewedDate = lvdt;

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

                                //_OracleEvent.Attributes = new OracleEvent.AttributesHeader();
                                _OracleEvent.Attributes_TotalDays = TotalDays;
                                _OracleEvent.Attributes_TotalDaysEllipseColor = IconEllipeColor;

                                if (!int.TryParse(reader["RemindMeID"].ToString(), out _OracleEvent.RemindMeID)) _OracleEvent.RemindMeID = 0;

                                String RemindMeDateTimeString = reader[EventLogFields.RemindMeDateTime].ToString();
                                DateTime rmdt = DateTime.MinValue;
                                if (DateTime.TryParse(RemindMeDateTimeString, out rmdt)) RemindMeDateTimeString = rmdt.ToString("dd/MM/y HH:mm");
                                _OracleEvent.RemindMeDateTime = rmdt;

                                if (!int.TryParse(reader["NotificationAcknowledged"].ToString(), out _OracleEvent.NotificationAcknowledged)) _OracleEvent.NotificationAcknowledged = 0;

                                //_OracleEvent.Source = new OracleEvent.SourceHeader();
                                _OracleEvent.Source_Mode = EventWindowModes.ViewReply;

                                if (!int.TryParse(reader["ParentEventID"].ToString(), out _OracleEvent.Source_ParentEventID)) _OracleEvent.Source_ParentEventID = 0;

                                //Test to see if user has already viewed the notification
                                if (_OracleEvent.RemindMeID == RemindMeModes.No && _OracleEvent.NotificationAcknowledged == NotificationAcknowlegedModes.No) ReturnUnread.Add(_OracleEvent);

                                Console.Write("_OracleEvent.ID = ");
                                Console.WriteLine(_OracleEvent.ID);
                                Console.Write("_OracleEvent.UserID = ");
                                Console.WriteLine(_OracleEvent.UserID);
                                Console.Write("_OracleEvent.Details = ");
                                Console.WriteLine(_OracleEvent.Details);
                                Console.Write("_OracleEvent.TargetUserID = ");
                                Console.WriteLine(_OracleEvent.TargetUserID);
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "GetMyUnread - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "GetMyReminders - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
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

                        SaveSuccessFull = true;

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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "UpdateMyReminder - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }

            if (SaveSuccessFull)
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

                        SaveSuccessFull = true;

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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "UpdateStatusID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }

            if (SaveSuccessFull)
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

                        SaveSuccessFull = true;

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

                    OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.Custom, new OracleCustomMessage { MessageTitleTextBlock = "UpdateReadByMeID - " + myOLEDBException.Errors[i].Source, InformationTextBlock = myOLEDBException.Errors[i].Message + " SQL: " + myOLEDBException.Errors[i].SQLState });
                    msg.ShowDialog();
                }
            }

            if (SaveSuccessFull)
            {
                //if (DisplayMode == DisplayModes.Reminders)
                //    RefreshLog(ListViews.Reminder);
                //else
                //    RefreshLog(ListViews.Log);
            }
        }

        private static bool CheckFormFields(EventWindow ew)
        {
            int Result = 0;

            if (ew.EventTypeComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("You can not choose 'All Events' as an event type.", "Oracle Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ew.EventTypeComboBox.SelectedIndex > -1)
            {
                Result++;
            }
            if (ew.SourceComboBox.SelectedIndex > -1)
            {
                Result++;
            }
            if (ew.DetailsTextBox.Text.Length > 0)
            {
                Result++;
            }
            if (ew.FrequencyComboBox.SelectedIndex > -1)
            {
                Result++;
            }
            if (ew.StatusComboBox.SelectedIndex > -1)
            {
                Result++;
            }

            Console.WriteLine(Result);

            if (Result == 5)
            {
                return true;
            }
            else
            {
                ew.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }
    }
}
