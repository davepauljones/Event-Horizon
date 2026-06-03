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
    public class DataTableManagementJob
    {
        public static EventHorizonJob EventHorizon_Job = new EventHorizonJob();
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
                            
                            sqlcmd = "SELECT * FROM Jobs;";

                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Job);
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

            query = from eventHorizonRams in EventHorizon_Job.AsEnumerable()      
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

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                eventHorizonRamsLINQ.JobNo = dataRow["JobNo"].ToString();

                eventHorizonRamsLINQ.Description = dataRow["Description"].ToString();

                if (!int.TryParse(dataRow["RamsProfileTypeID"].ToString(), out eventHorizonRamsLINQ.RamsProfileTypeID)) eventHorizonRamsLINQ.RamsProfileTypeID = 0;

                eventHorizonRamsLINQ.ClientName = dataRow["ClientName"].ToString();
                eventHorizonRamsLINQ.Site = dataRow["Site"].ToString();
                eventHorizonRamsLINQ.LocationActivity = dataRow["LocationActivity"].ToString();

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRamsLINQ.StatusID)) eventHorizonRamsLINQ.StatusID = 0;

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
                            string sqlcmd = "SELECT * FROM Jobs ORDER BY ID DESC LIMIT 1;";
                                 
                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Job);
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

            query = from ramsHorizonEvent in EventHorizon_Job.AsEnumerable()
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

                if (!int.TryParse(dataRow["UserID"].ToString(), out eventHorizonRamsLINQ.UserID)) eventHorizonRamsLINQ.UserID = 0;

                eventHorizonRamsLINQ.JobNo = dataRow["JobNo"].ToString();

                eventHorizonRamsLINQ.Description = dataRow["Description"].ToString();

                if (!int.TryParse(dataRow["RamsProfileTypeID"].ToString(), out eventHorizonRamsLINQ.RamsProfileTypeID)) eventHorizonRamsLINQ.RamsProfileTypeID = 0;

                eventHorizonRamsLINQ.ClientName = dataRow["ClientName"].ToString();
                eventHorizonRamsLINQ.Site = dataRow["Site"].ToString();
                eventHorizonRamsLINQ.LocationActivity = dataRow["LocationActivity"].ToString();

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRamsLINQ.StatusID)) eventHorizonRamsLINQ.StatusID = 0;

                _EventHorizonLINQReturn = eventHorizonRamsLINQ;
            }
            return _EventHorizonLINQReturn;
        }
 
        public static void SaveJob(JobWindow jobWindow, EventHorizonRamsLINQ eventHorizonRamsLINQ, int ramsMode)
        {
            if (CheckFormFields(jobWindow))
            {
                bool saveSuccessFull = false;

                string jobNoSafeString = jobWindow.JobNoTextBox.Text.Replace("'", "''");
                string descriptionSafeString = jobWindow.DescriptionTextBox.Text.Replace("'", "''");
                string clientNameSafeString = jobWindow.ClientNameTextBox.Text.Replace("'", "''");
                string siteSafeString = jobWindow.SiteTextBox.Text.Replace("'", "''");
                string locationActivitySafeString = jobWindow.LocationActivityTextBox.Text.Replace("'", "''");

                DateTime? createdDateTime = DateTime.Now;

                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE Jobs SET JobNo = ?, Description = ?, RamsProfileTypeID = ?, ClientName = ?, Site = ?, LocationActivity = ?, StatusID = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@JobNo", DbType.String).Value = jobNoSafeString;
                                command.Parameters.Add("@Description", DbType.String).Value = descriptionSafeString;
                                command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = jobWindow.RamsProfileTypeComboBox.SelectedIndex;
                                command.Parameters.Add("@ClientName", DbType.String).Value = clientNameSafeString;
                                command.Parameters.Add("@Site", DbType.String).Value = siteSafeString;
                                command.Parameters.Add("@LocationActivity", DbType.String).Value = locationActivitySafeString;
                                command.Parameters.Add("@StatusID", DbType.Int32).Value = jobWindow.RamsStatusIDComboBox.SelectedIndex;

                                command.Parameters.AddWithValue("@ID", eventHorizonRamsLINQ.ID);

                                if (ramsMode == EventWindowModes.ViewMainEvent || ramsMode == EventWindowModes.ViewNote || ramsMode == EventWindowModes.ViewReply || ramsMode == EventWindowModes.EditMainEvent || ramsMode == EventWindowModes.EditNote || ramsMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || ramsMode == EventWindowModes.NewEvent || ramsMode == EventWindowModes.NewNote || ramsMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Jobs (CreatedDateTime, UserID, JobNo, Description, RamsProfileTypeID, ClientName, Site, LocationActivity, StatusID) VALUES (@CreatedDateTime, @UserID, @JobNo, @Description, @RamsProfileTypeID, @ClientName, @Site, @LocationActivity, @StatusID);";

                                    command.Parameters.Add("@CreatedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@UserID", DbType.Int32).Value = XMLReaderWriter.UserID;

                                    command.Parameters.Add("@JobNo", DbType.String).Value = jobNoSafeString;

                                    command.Parameters.Add("@Description", DbType.String).Value = descriptionSafeString;

                                    command.Parameters.Add("@RamsProfileTypeID", DbType.Int32).Value = jobWindow.RamsProfileTypeComboBox.SelectedIndex;

                                    command.Parameters.Add("@ClientName", DbType.String).Value = clientNameSafeString;
                                    command.Parameters.Add("@Site", DbType.String).Value = siteSafeString;
                                    command.Parameters.Add("@LocationActivity", DbType.String).Value = locationActivitySafeString;

                                    command.Parameters.Add("@StatusID", DbType.Int32).Value = jobWindow.RamsStatusIDComboBox.SelectedIndex;

                                    command.ExecuteNonQuery();

                                    MainWindow.mw.Status.Content = "Successfully added a new Job";
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.mw.Status.Content = "Successfully updated a Job";
                    MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = null;
                    MainWindow.activeRamsWindow.RefreshActiveRams();
                }

                if (saveSuccessFull)
                {
                    jobWindow.Close();
                    if (jobWindow.jobWindow != null) jobWindow.jobWindow.Close();
                    MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = null;
                    MainWindow.activeRamsWindow.RefreshActiveRams();
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
                            using (SQLiteCommand command = new SQLiteCommand("SELECT UserID FROM Jobs WHERE ID = ?", connection))
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
        
        public static void DeleteJob(Int32 JobID)
        {
            if (XMLReaderWriter.UserID != 1)
            {
                if (GetUserID(JobID) != XMLReaderWriter.UserID)
                {
                    EventHorizonRequesterNotification rorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, you can only delete your own jobs.", InformationTextBlock = "You could ask the user who created it, to delete it." }, RequesterTypes.OK);
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
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM MethodStatements WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = JobID;

                                command.ExecuteNonQuery();

                                MainWindow.activeRamsWindow.Status.Content = "Successfully deleted attached method statement";
                            }
                        }
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM RiskAssessments WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = JobID;

                                command.ExecuteNonQuery();

                                MainWindow.activeRamsWindow.Status.Content = "Successfully deleted attached risk assessment";
                            }
                        }
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Jobs WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = JobID;

                                command.ExecuteNonQuery();

                                MainWindow.activeRamsWindow.Status.Content = "Successfully deleted Job and attached risk assessment and method statement.";
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
        
        private static bool CheckFormFields(JobWindow jobWindow)
        {
            int result = 0;

            if (jobWindow.JobNoTextBox.Text.Length > 0)
            {
                result++;
            }

            if (jobWindow.DescriptionTextBox.Text.Length > 0)
            {
                result++;
            }

            if (jobWindow.RamsProfileTypeComboBox.SelectedIndex == 0)
            {
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CheckFormFields", InformationTextBlock = "You can not choose 'All Rams' as an Rams Profile type." }, RequesterTypes.OK);
                msg.ShowDialog();
                return false;
            }

            if (jobWindow.RamsProfileTypeComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (jobWindow.ClientNameTextBox.Text.Length > 0)
            {
                result++;
            }

            if (jobWindow.SiteTextBox.Text.Length > 0)
            {
                result++;
            }

            if (jobWindow.LocationActivityTextBox.Text.Length > 0)
            {
                result++;
            }

            if (result == 6)
            {
                return true;
            }
            else
            {
                jobWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}