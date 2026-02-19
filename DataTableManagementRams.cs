using FontAwesome.WPF;
using System;
using System.CodeDom.Compiler;
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

        public static List<EventHorizonRamsLINQ> GetRamss()
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
                            
                            sqlcmd = "SELECT * FROM Rams;";

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

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetRams - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonRams in EventHorizon_Rams.AsEnumerable()      
                    orderby eventHorizonRams.Field<DateTime>("CreatedDateTime") descending
                    select eventHorizonRams;

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRamsLINQ eventHorizonRamsLINQ = new EventHorizonRamsLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRamsLINQ.ID)) eventHorizonRamsLINQ.ID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.CreationDate = createdDateTime;

                if (!int.TryParse(dataRow["JobNo"].ToString(), out eventHorizonRamsLINQ.JobNo)) eventHorizonRamsLINQ.JobNo = 0;

                eventHorizonRamsLINQ.Description = dataRow["Description"].ToString();

                if (!int.TryParse(dataRow["RamsProfileTypeID"].ToString(), out eventHorizonRamsLINQ.RamsProfileTypeID)) eventHorizonRamsLINQ.RamsProfileTypeID = 0;

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                eventHorizonRamsLINQ.ClientName = dataRow["ClientName"].ToString();
                eventHorizonRamsLINQ.Site = dataRow["Site"].ToString();
                eventHorizonRamsLINQ.LocationActivity = dataRow["LocationActivity"].ToString();

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonRamsLINQ.RevisionNo)) eventHorizonRamsLINQ.RevisionNo = 0;

                eventHorizonRamsLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRamsLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                eventHorizonRamsLINQ.MSContractTitle = dataRow["MSContractTitle"].ToString();

                if (!int.TryParse(dataRow["MSRevisionNo"].ToString(), out eventHorizonRamsLINQ.MSRevisionNo)) eventHorizonRamsLINQ.MSRevisionNo = 0;

                eventHorizonRamsLINQ.MSContractor = dataRow["MSContractor"].ToString();

                _EventHorizonRamsLINQReturnList.Add(eventHorizonRamsLINQ);
            }
            return _EventHorizonRamsLINQReturnList;
        }

        public static EventHorizonRamsLINQ GetRams(Int32 eventID)
        {
            EventHorizonRamsLINQ _EventHorizonLINQReturn = new EventHorizonRamsLINQ();

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd = "SELECT * FROM Rams ORDER BY ID DESC LIMIT 1;";
                                 
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

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetRams - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from ramsHorizonEvent in EventHorizon_Rams.AsEnumerable()
                    where ramsHorizonEvent.Field<Int32>("ID") == eventID
                    select ramsHorizonEvent;                  

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRamsLINQ eventHorizonRamsLINQ = new EventHorizonRamsLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRamsLINQ.ID)) eventHorizonRamsLINQ.ID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonRamsLINQ.CreationDate = createdDateTime;

                if (!int.TryParse(dataRow["JobNo"].ToString(), out eventHorizonRamsLINQ.JobNo)) eventHorizonRamsLINQ.JobNo = 0;

                eventHorizonRamsLINQ.Description = dataRow["Description"].ToString();

                if (!int.TryParse(dataRow["RamsProfileTypeID"].ToString(), out eventHorizonRamsLINQ.RamsProfileTypeID)) eventHorizonRamsLINQ.RamsProfileTypeID = 0;
                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                eventHorizonRamsLINQ.ClientName = dataRow["ClientName"].ToString();
                eventHorizonRamsLINQ.Site = dataRow["Site"].ToString();
                eventHorizonRamsLINQ.LocationActivity = dataRow["LocationActivity"].ToString();

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonRamsLINQ.RevisionNo)) eventHorizonRamsLINQ.RevisionNo = 0;

                eventHorizonRamsLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRamsLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                Color iconEllipeColor = Colors.Pink;

                iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");


                _EventHorizonLINQReturn = eventHorizonRamsLINQ;
            }
            return _EventHorizonLINQReturn;
        }
 
        public static void SaveRams(RamsWindow ramsWindow, EventHorizonRamsLINQ eventHorizonRamsLINQ, int ramsMode)
        {
            if (CheckFormFields(ramsWindow))
            {
                bool saveSuccessFull = false;

                string descriptionSafeString = ramsWindow.DescriptionTextBox.Text.Replace("'", "''");
                string clientNameSafeString = ramsWindow.ClientNameTextBox.Text.Replace("'", "''");
                string siteSafeString = ramsWindow.SiteTextBox.Text.Replace("'", "''");
                string locationActivitySafeString = ramsWindow.LocationActivityTextBox.Text.Replace("'", "''");
                string elementReviewedSafeString = ramsWindow.ElementReviewedTextBox.Text.Replace("'", "''");
                string mSContractTitleSafeString = ramsWindow.MSContractTitleTextBox.Text.Replace("'", "''");
                string mSContractorSafeString = ramsWindow.MSContractorTextBox.Text.Replace("'", "''");

                DateTime? createdDateTime = DateTime.Now;

                DateTime reviewedDateTimeNow = DateTime.Now;

                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE Rams SET Description = ?, RamsProfileTypeID = ?, ClientName = ?, Site = ?, LocationActivity = ?, RevisionNo = ?, ElementReviewed = ?, ReviewedDateTime = ?, MSContractTitle = ?, MSRevisionNo = ?, MSContractor = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@Description", DbType.String).Value = descriptionSafeString;
                                command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;
                                command.Parameters.Add("@ClientName", DbType.String).Value = clientNameSafeString;
                                command.Parameters.Add("@Site", DbType.String).Value = siteSafeString;
                                command.Parameters.Add("@LocationActivity", DbType.String).Value = locationActivitySafeString;

                                command.Parameters.Add("@RevisionNo", DbType.Int32).Value = ramsWindow.RevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = reviewedDateTimeNow;

                                command.Parameters.Add("@MSContractTitle", DbType.String).Value = mSContractTitleSafeString;

                                command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = ramsWindow.MSRevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@MSContractor", DbType.String).Value = mSContractorSafeString;

                                if (ramsMode == EventWindowModes.ViewMainEvent || ramsMode == EventWindowModes.ViewNote || ramsMode == EventWindowModes.ViewReply || ramsMode == EventWindowModes.EditMainEvent || ramsMode == EventWindowModes.EditNote || ramsMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || ramsMode == EventWindowModes.NewEvent || ramsMode == EventWindowModes.NewNote || ramsMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Rams (CreatedDateTime, JobNo, Description, RamsProfileTypeID, UserID, ClientName, Site, LocationActivity, RevisionNo, ElementReviewed, ReviewedDateTime, MSContractTitle, MSRevisionNo, MSContractor) VALUES (@CreatedDateTime, @JobNo, @Description, @RamsProfileTypeID, @UserID, @ClientName, @Site, @LocationActivity, @RevisionNo, @ElementReviewed, @ReviewedDateTime, @MSContractTitle, @MSRevisionNo, @MSContractor);";

                                    command.Parameters.Add("@CreatedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@JobNo", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;

                                    command.Parameters.Add("@Description", DbType.String).Value = descriptionSafeString;

                                    command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = ramsWindow.RamsProfileTypeComboBox.SelectedIndex;
                                    
                                    command.Parameters.Add("@UserID", DbType.Int32).Value = XMLReaderWriter.UserID;

                                    command.Parameters.Add("@ClientName", DbType.String).Value = clientNameSafeString;
                                    command.Parameters.Add("@Site", DbType.String).Value = siteSafeString;
                                    command.Parameters.Add("@LocationActivity", DbType.String).Value = locationActivitySafeString;

                                    command.Parameters.Add("@RevisionNo", DbType.Int32).Value = ramsWindow.RevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                    command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@MSContractTitle", DbType.String).Value = mSContractTitleSafeString;

                                    command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = ramsWindow.MSRevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@MSContractor", DbType.String).Value = mSContractorSafeString;

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

            if (ramsWindow.JobNoTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.DescriptionTextBox.Text.Length > 0)
            {
                result++;
            }

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

            if (ramsWindow.ClientNameTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.SiteTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.LocationActivityTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.RevisionNoComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (ramsWindow.ElementReviewedTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.MSContractTitleTextBox.Text.Length > 0)
            {
                result++;
            }

            if (ramsWindow.MSRevisionNoComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (ramsWindow.MSContractorTextBox.Text.Length > 0)
            {
                result++;
            }

            if (result == 11)
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