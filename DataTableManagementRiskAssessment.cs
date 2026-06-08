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
    public class DataTableManagementRiskAssessment
    {
        public static EventHorizonRiskAssessment EventHorizon_RiskAssessment = new EventHorizonRiskAssessment();
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

        public static List<EventHorizonRiskAssessmentLINQ> GetRiskAssessment()
        {
            List<EventHorizonRiskAssessmentLINQ> _EventHorizonRiskAssessmentLINQReturnList = new List<EventHorizonRiskAssessmentLINQ>();

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
                            
                            sqlcmd = "SELECT * FROM RiskAssessments;";

                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_RiskAssessment);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetRiskAssessment - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonRams in EventHorizon_RiskAssessment.AsEnumerable()      
                    select eventHorizonRams;

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRiskAssessmentLINQ eventHorizonRiskAssessmentLINQ = new EventHorizonRiskAssessmentLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRiskAssessmentLINQ.ID)) eventHorizonRiskAssessmentLINQ.ID = 0;

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonRiskAssessmentLINQ.RevisionNo)) eventHorizonRiskAssessmentLINQ.RevisionNo = 0;

                eventHorizonRiskAssessmentLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRiskAssessmentLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRiskAssessmentLINQ.StatusID)) eventHorizonRiskAssessmentLINQ.StatusID = 0;

                _EventHorizonRiskAssessmentLINQReturnList.Add(eventHorizonRiskAssessmentLINQ);
            }
            return _EventHorizonRiskAssessmentLINQReturnList;
        }

        public static EventHorizonRiskAssessmentLINQ GetRiskAssessment(Int32 eventID)
        {
            EventHorizonRiskAssessmentLINQ _EventHorizonRiskAssessmentLINQReturn = new EventHorizonRiskAssessmentLINQ();

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd = "SELECT * FROM RiskAssessments ORDER BY ID DESC LIMIT 1;";
                                 
                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_RiskAssessment);
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

            query = from ramsHorizonEvent in EventHorizon_RiskAssessment.AsEnumerable()
                    where ramsHorizonEvent.Field<Int32>("ID") == eventID
                    select ramsHorizonEvent;                  

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonRiskAssessmentLINQ eventHorizonRiskAssessmentLINQ = new EventHorizonRiskAssessmentLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonRiskAssessmentLINQ.ID)) eventHorizonRiskAssessmentLINQ.ID = 0;

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonRiskAssessmentLINQ.RevisionNo)) eventHorizonRiskAssessmentLINQ.RevisionNo = 0;

                eventHorizonRiskAssessmentLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonRiskAssessmentLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                Color iconEllipeColor = Colors.Pink;

                iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonRiskAssessmentLINQ.StatusID)) eventHorizonRiskAssessmentLINQ.StatusID = 0;

                _EventHorizonRiskAssessmentLINQReturn = eventHorizonRiskAssessmentLINQ;
            }
            return _EventHorizonRiskAssessmentLINQReturn;
        }
 
        public static void SaveRiskAssessment(RiskAssessmentWindow riskAssessmentWindow, EventHorizonJobLINQ eventHorizonJobLINQ, EventHorizonRiskAssessmentLINQ eventHorizonRiskAssessmentLINQ, int ramsMode)
        {
            if (CheckFormFields(riskAssessmentWindow))
            {
                bool saveSuccessFull = false;

                string elementReviewedSafeString = riskAssessmentWindow.ElementReviewedTextBox.Text.Replace("'", "''");

                DateTime reviewedDateTimeNow = DateTime.Now;

                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE RiskAssessments SET RevisionNo = ?, ElementReviewed = ?, ReviewedDateTime = ?, StatusID = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@RevisionNo", DbType.Int32).Value = riskAssessmentWindow.RevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = reviewedDateTimeNow;

                                command.Parameters.Add("@StatusID", DbType.Int32).Value = riskAssessmentWindow.RamsStatusIDComboBox.SelectedIndex;

                                command.Parameters.AddWithValue("@ID", eventHorizonJobLINQ.ID);

                                if (ramsMode == EventWindowModes.ViewMainEvent || ramsMode == EventWindowModes.ViewNote || ramsMode == EventWindowModes.ViewReply || ramsMode == EventWindowModes.EditMainEvent || ramsMode == EventWindowModes.EditNote || ramsMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || ramsMode == EventWindowModes.NewEvent || ramsMode == EventWindowModes.NewNote || ramsMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO RiskAssessments (ID, RevisionNo, ElementReviewed, ReviewedDateTime, StatusID) VALUES (@ID, @RevisionNo, @ElementReviewed, @ReviewedDateTime, @StatusID);";

                                    command.Parameters.Add("@ID", DbType.Int32).Value = eventHorizonJobLINQ.ID;

                                    command.Parameters.Add("@RevisionNo", DbType.Int32).Value = riskAssessmentWindow.RevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                    command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = reviewedDateTimeNow;

                                    command.Parameters.Add("@StatusID", DbType.Int32).Value = riskAssessmentWindow.RamsStatusIDComboBox.SelectedIndex;

                                    command.ExecuteNonQuery();

                                    MainWindow.activeJobsWindow.Status.Content = "Successfully added a new risk assessment";
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.activeJobsWindow.Status.Content = "Successfully updated a Risk Assessment";
                    MainWindow.activeJobsWindow.ActiveJobsListView.SelectedItem = null;
                    MainWindow.activeJobsWindow.RefreshActiveJobs();
                }

                if (saveSuccessFull)
                {
                    riskAssessmentWindow.Close();
                    if (riskAssessmentWindow.riskAssessmentWindow != null) riskAssessmentWindow.riskAssessmentWindow.Close();
                    MainWindow.activeJobsWindow.ActiveJobsListView.SelectedItem = null;
                    MainWindow.activeJobsWindow.RefreshActiveJobs();
                }
            }
        }

        public static void DeleteRiskAssessment(Int32 UserID, Int32 EventID)
        {
            if (XMLReaderWriter.UserID != 1)
            {
                if (UserID != XMLReaderWriter.UserID)
                {
                    EventHorizonRequesterNotification rorn = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Error, you can only delete your own Risk Assessment.", InformationTextBlock = "You could ask the user who created it, to delete it." }, RequesterTypes.OK);
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
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM RiskAssessments WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@ID", DbType.Int32).Value = EventID;

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Successfully deleted risk assessment.";
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

        private static bool CheckFormFields(RiskAssessmentWindow riskAssessmentWindow)
        {
            int result = 0;

            if (riskAssessmentWindow.RevisionNoComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (riskAssessmentWindow.ElementReviewedTextBox.Text.Length > 0)
            {
                result++;
            }

            if (result == 2)
            {
                return true;
            }
            else
            {
                riskAssessmentWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}