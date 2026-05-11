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
    public class DataTableManagementEngineer
    {
        public static EventHorizonEngineer EventHorizon_Engineer = new EventHorizonEngineer();
        public static int RowLimitMode = RowLimitModes.LimitOnly;
        public static Int32 RowLimitStep = 30;
        public static Int32 RowLimit = RowLimitStep;
        public static Int32 RowLimitMin = 30;
        public static Int32 RowLimitMax = 300;
        public static Int32 RowOffsetStep = 30;
        public static Int32 RowOffset = 0;
        public static Int32 RowOffsetMin = 0;
        public static Int32 RowOffsetMax = 300;

        public static List<EventHorizonEngineersLINQ> GetEngineers()
        {
            List<EventHorizonEngineersLINQ> _EventHorizonEngineersLINQReturnList = new List<EventHorizonEngineersLINQ>();

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
                            
                            sqlcmd = "SELECT * FROM Engineers;";

                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Engineer);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEngineers - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from eventHorizonRams in EventHorizon_Engineer.AsEnumerable()      
                    orderby eventHorizonRams.Field<DateTime>("CreatedDateTime") descending
                    select eventHorizonRams;

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonEngineersLINQ eventHorizonEngineersLINQ = new EventHorizonEngineersLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonEngineersLINQ.ID)) eventHorizonEngineersLINQ.ID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonEngineersLINQ.CreationDate = createdDateTime;

                eventHorizonEngineersLINQ.Name = dataRow["Name"].ToString();
                eventHorizonEngineersLINQ.Role = dataRow["Role"].ToString();
                eventHorizonEngineersLINQ.CompetenceDetails = dataRow["CompetenceDetails"].ToString();

                _EventHorizonEngineersLINQReturnList.Add(eventHorizonEngineersLINQ);
            }
            return _EventHorizonEngineersLINQReturnList;
        }

        public static EventHorizonEngineersLINQ GetEngineer(Int32 engineerID)
        {
            EventHorizonEngineersLINQ _EventHorizonEngineerLINQReturn = new EventHorizonEngineersLINQ();

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            string sqlcmd = "SELECT * FROM Engineers ORDER BY ID DESC LIMIT 1;";
                                 
                            SQLiteCommand cmd = new SQLiteCommand(sqlcmd, conn);

                            conn.Open();

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                            adapter.Fill(EventHorizon_Engineer);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "GetEngineer - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }

            EnumerableRowCollection<DataRow> query;

            query = from engineersHorizonEvent in EventHorizon_Engineer.AsEnumerable()
                    where engineersHorizonEvent.Field<Int32>("ID") == engineerID
                    select engineersHorizonEvent;                  

            DataView dataView = query.AsDataView();

            Console.WriteLine("*** Start of view.ToTable().Rows ***");

            foreach (DataRow dataRow in dataView.ToTable().Rows)
            {
                EventHorizonEngineersLINQ eventHorizonEngineersLINQ = new EventHorizonEngineersLINQ();

                if (!int.TryParse(dataRow["ID"].ToString(), out eventHorizonEngineersLINQ.ID)) eventHorizonEngineersLINQ.ID = 0;

                string createdDateTimeString = dataRow["CreatedDateTime"].ToString();
                DateTime createdDateTime = DateTime.MinValue;
                if (DateTime.TryParse(createdDateTimeString, out createdDateTime)) createdDateTimeString = createdDateTime.ToString("dd/MM/y HH:mm");
                eventHorizonEngineersLINQ.CreationDate = createdDateTime;

                eventHorizonEngineersLINQ.Name = dataRow["Name"].ToString();
                eventHorizonEngineersLINQ.Role = dataRow["Role"].ToString();
                eventHorizonEngineersLINQ.CompetenceDetails = dataRow["CompetenceDetails"].ToString();

                _EventHorizonEngineerLINQReturn = eventHorizonEngineersLINQ;
            }
            return _EventHorizonEngineerLINQReturn;
        }
 
        public static void SaveEngineer(EngineerWindow engineerWindow, EventHorizonEngineersLINQ eventHorizonEngineersLINQ, int ramsMode)
        {
            if (CheckFormFields(engineerWindow))
            {
                bool saveSuccessFull = false;

                string nameSafeString = engineerWindow.NameTextBox.Text.Replace("'", "''");
                string roleSafeString = engineerWindow.RoleTextBox.Text.Replace("'", "''");
                string competenceDetailsSafeString = engineerWindow.CompetenceDetailsTextBox.Text.Replace("'", "''");

                DateTime? createdDateTime = DateTime.Now;

                DateTime reviewedDateTimeNow = DateTime.Now;

                int rowsAffected = 0;

                switch (XMLReaderWriter.DatabaseSystem)
                {
                    case DatabaseSystems.SQLite:
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("UPDATE Engineers SET Name = ?, Role = ?, CompetenceDetails = ? WHERE ID = ?", connection))
                            {
                                connection.Open();
                            
                                command.Parameters.Add("@Name", DbType.String).Value = nameSafeString;                          
                                command.Parameters.Add("@Role", DbType.String).Value = roleSafeString;
                                command.Parameters.Add("@CompetenceDetails", DbType.String).Value = competenceDetailsSafeString;

                                if (ramsMode == EventWindowModes.ViewMainEvent || ramsMode == EventWindowModes.ViewNote || ramsMode == EventWindowModes.ViewReply || ramsMode == EventWindowModes.EditMainEvent || ramsMode == EventWindowModes.EditNote || ramsMode == EventWindowModes.EditReply)
                                    rowsAffected = command.ExecuteNonQuery();
                                else if (rowsAffected == 0 || ramsMode == EventWindowModes.NewEvent || ramsMode == EventWindowModes.NewNote || ramsMode == EventWindowModes.NewReply)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "INSERT INTO Engineers (CreatedDateTime, Name, Role, CompetenceDetails) VALUES (@CreatedDateTime, @Name, @Role, @CompetenceDetails);";

                                    command.Parameters.Add("@CreatedDateTime", DbType.DateTime).Value = createdDateTime;

                                    command.Parameters.Add("@Name", DbType.String).Value = nameSafeString;
                                    command.Parameters.Add("@Role", DbType.String).Value = roleSafeString;
                                    command.Parameters.Add("@CompetenceDetails", DbType.String).Value = competenceDetailsSafeString;
                                    
                                    command.ExecuteNonQuery();

                                    MainWindow.mw.Status.Content = "Successfully added a new Engineer";
                                }
                            }
                        }
                        saveSuccessFull = true;
                        break;
                }

                if (rowsAffected > 0)
                {
                    MainWindow.mw.Status.Content = "Successfully updated a Engineer";
                    MainWindow.engineersWindow.EngineersListView.SelectedItem = null;
                    MainWindow.engineersWindow.RefreshEngineers();
                    MainWindow.mw.RunningTask();
                }

                if (saveSuccessFull)
                {
                    engineerWindow.Close();
                    if (engineerWindow.engineerWindow != null) engineerWindow.engineerWindow.Close();
                    MainWindow.engineersWindow.EngineersListView.SelectedItem = null;
                    MainWindow.engineersWindow.RefreshEngineers();
                }
            }
        }

        private static bool CheckFormFields(EngineerWindow engineerWindow)
        {
            int result = 0;

            if (engineerWindow.NameTextBox.Text.Length > 0)
            {
                result++;
            }

            if (engineerWindow.RoleTextBox.Text.Length > 0)
            {
                result++;
            }

            if (engineerWindow.CompetenceDetailsTextBox.Text.Length > 0)
            {
                result++;
            }

            if (result == 3)
            {
                return true;
            }
            else
            {
                engineerWindow.StatusLabel.Content = "Please fill in all details!";
                return false;
            }
        }

    }
}