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
    public class DataTableManagementMethodStatement
    {
        public static EventHorizonMethodStatement EventHorizon_MethodStatement = new EventHorizonMethodStatement();
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

        public static List<EventHorizonMethodStatementLINQ> GetMethodStatement()
        {
            List<EventHorizonMethodStatementLINQ> _EventHorizonMethodStatementLINQReturnList = new List<EventHorizonMethodStatementLINQ>();

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
                            
                            sqlcmd = "SELECT * FROM MethodStatements;";

                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_MethodStatement);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetMethodStatement - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonRams in EventHorizon_MethodStatement.AsEnumerable()      
                    select eventHorizonRams;

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonMethodStatementLINQ eventHorizonMethodStatementLINQ = new EventHorizonMethodStatementLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonMethodStatementLINQ.ID)) eventHorizonMethodStatementLINQ.ID = 0;

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonMethodStatementLINQ.MSRevisionNo)) eventHorizonMethodStatementLINQ.MSRevisionNo = 0;

                eventHorizonMethodStatementLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonMethodStatementLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                if (!int.TryParse(dataRow["MSRevisionNo"].ToString(), out eventHorizonMethodStatementLINQ.MSRevisionNo)) eventHorizonMethodStatementLINQ.MSRevisionNo = 0;

                eventHorizonMethodStatementLINQ.MSContractor = dataRow["MSContractor"].ToString();

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonMethodStatementLINQ.StatusID)) eventHorizonMethodStatementLINQ.StatusID = 0;

                _EventHorizonMethodStatementLINQReturnList.Add(eventHorizonMethodStatementLINQ);
            }
            return _EventHorizonMethodStatementLINQReturnList;
        }

        public static EventHorizonMethodStatementLINQ GetMethodStatement(Int32 eventID)
        {
            EventHorizonMethodStatementLINQ _EventHorizonMethodStatementLINQReturn = new EventHorizonMethodStatementLINQ();

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd = "SELECT * FROM MethodStatements ORDER BY ID DESC LIMIT 1;";
                                 
                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_MethodStatement);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetMethodStatement - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from ramsHorizonEvent in EventHorizon_MethodStatement.AsEnumerable()
                    where ramsHorizonEvent.Field<Int32>("ID") == eventID
                    select ramsHorizonEvent;                  

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonMethodStatementLINQ eventHorizonMethodStatementLINQ = new EventHorizonMethodStatementLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonMethodStatementLINQ.ID)) eventHorizonMethodStatementLINQ.ID = 0;

                if (!int.TryParse(dataRow["RevisionNo"].ToString(), out eventHorizonMethodStatementLINQ.MSRevisionNo)) eventHorizonMethodStatementLINQ.MSRevisionNo = 0;

                eventHorizonMethodStatementLINQ.ElementReviewed = dataRow["ElementReviewed"].ToString();

                string reviewedDateTimeString = dataRow["ReviewedDateTime"].ToString();
                DateTime reviewedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(reviewedDateTimeString, out reviewedDateTime))
                {
                    if (reviewedDateTime.TimeOfDay == TimeSpan.Zero)
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y");
                    else
                        reviewedDateTimeString = reviewedDateTime.ToString("dd/MM/y HH:mm");

                    eventHorizonMethodStatementLINQ.ReviewedDateTime = reviewedDateTime;
                }
                else
                    Console.WriteLine("Unable to parse reviewedDateTimeString '{0}'", reviewedDateTimeString);

                Color iconEllipeColor = Colors.Pink;

                iconEllipeColor = (Color)ColorConverter.ConvertFromString("#FFe60000");

                if (!int.TryParse(dataRow["StatusID"].ToString(), out eventHorizonMethodStatementLINQ.StatusID)) eventHorizonMethodStatementLINQ.StatusID = 0;

                _EventHorizonMethodStatementLINQReturn = eventHorizonMethodStatementLINQ;
            }
            return _EventHorizonMethodStatementLINQReturn;
        }
 
        public static void SaveMethodStatement(MethodStatementWindow methodStatementWindow, EventHorizonJobLINQ eventHorizonJobLINQ, EventHorizonMethodStatementLINQ eventHorizonMethodStatementLINQ, int ramsMode)
        {
            if (CheckFormFields(methodStatementWindow))
            {
                bool saveSuccessFull = false;

                string elementReviewedSafeString = methodStatementWindow.ElementReviewedTextBox.Text.Replace("'", "''");
                string mSContractTitleSafeString = methodStatementWindow.MSContractTitleTextBox.Text.Replace("'", "''");
                string mSContractorSafeString = methodStatementWindow.MSContractorTextBox.Text.Replace("'", "''");

                DateTime reviewedDateTimeNow = DateTime.Now;

                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE MethodStatements SET ElementReviewed = ?, ReviewedDateTime = ?, MSRevisionNo = ?, MSContractor = ?, StatusID = ? WHERE ID = ?", connection))
                            {
                                connection.Open();

                                command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = methodStatementWindow.RevisionNoComboBox.SelectedIndex;

                                command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = reviewedDateTimeNow;

                                command.Parameters.Add("@MSContractor", DbType.String).Value = mSContractorSafeString;

                                command.Parameters.Add("@StatusID", DbType.Int32).Value = methodStatementWindow.RamsStatusIDComboBox.SelectedIndex;

                                command.Parameters.AddWithValue("@ID", eventHorizonMethodStatementLINQ.ID);

                                if (ramsMode == EventWindowModes.ViewMainEvent || ramsMode == EventWindowModes.ViewNote || ramsMode == EventWindowModes.ViewReply || ramsMode == EventWindowModes.EditMainEvent || ramsMode == EventWindowModes.EditNote || ramsMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || ramsMode == EventWindowModes.NewEvent || ramsMode == EventWindowModes.NewNote || ramsMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO MethodStatements (ID, ElementReviewed, ReviewedDateTime, MSRevisionNo, MSContractor, StatusID) VALUES (@ElementReviewed, @ReviewedDateTime, @MSRevisionNo, @MSContractor, @StatusID);";

                                    command.Parameters.Add("@ID", DbType.Int32).Value = eventHorizonJobLINQ.ID;

                                    command.Parameters.Add("@ElementReviewed", DbType.String).Value = elementReviewedSafeString;

                                    command.Parameters.Add("@ReviewedDateTime", DbType.DateTime).Value = reviewedDateTimeNow;

                                    command.Parameters.Add("@MSRevisionNo", DbType.Int32).Value = methodStatementWindow.RevisionNoComboBox.SelectedIndex;

                                    command.Parameters.Add("@MSContractor", DbType.String).Value = mSContractorSafeString;

                                    command.Parameters.Add("@StatusID", DbType.Int32).Value = methodStatementWindow.RamsStatusIDComboBox.SelectedIndex;

                                    command.ExecuteNonQuery();

                                    MainWindow.activeRamsWindow.Status.Content = "Successfully added a new Method Statement";
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.activeRamsWindow.Status.Content = "Successfully updated a Method Statement";
                    MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = null;
                    MainWindow.activeRamsWindow.RefreshActiveJobs();
                }

                if (saveSuccessFull)
                {
                    methodStatementWindow.Close();
                    if (methodStatementWindow.methodStatementWindow != null) methodStatementWindow.methodStatementWindow.Close();
                    MainWindow.activeRamsWindow.ActiveRamsListView.SelectedItem = null;
                    MainWindow.activeRamsWindow.RefreshActiveJobs();
                }
            }
        }
        
        private static bool CheckFormFields(MethodStatementWindow methodStatementWindow)
        {
            int result = 0;

            if (methodStatementWindow.ElementReviewedTextBox.Text.Length > 0)
            {
                result++;
            }

            if (methodStatementWindow.RevisionNoComboBox.SelectedIndex > -1)
            {
                result++;
            }

            if (methodStatementWindow.MSContractorTextBox.Text.Length > 0)
            {
                result++;
            }

            if (result == 3)
            {
                return true;
            }
            else
            {
                methodStatementWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}