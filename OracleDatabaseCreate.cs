using System;
using System.Data.OleDb;
using System.IO;
using System.Data.SQLite;

namespace The_Oracle
{
    public class OracleDatabaseCreate
    {
        internal static bool CheckIfDatabaseExists()
        {
            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb"))
            {
                return true;
            }
            else
            {
                OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Event Horizon could not connect to a database", InformationTextBlock = "You have a few options, you can browse for the database, create a new database or close Event Horizon and seek IT Support." }, RequesterTypes.OK);
                msg.ShowDialog();
                return false;
            }
        }
        
        internal static void Create_Oracle()
        {
            if (!File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteDatabase.mdb"))
            { 
                File.WriteAllBytes("EventHorizonRemoteDatabase.mdb", Properties.Resources.EventHorizonRemoteDatabase);
                
                CreateEventLogTable();
                CreateUsersTable();
            }
        }
        
        internal static void CreateNew_EventHorizonLocalSettings()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonLocalSettings.xml"))
            {
                File.WriteAllText("EventHorizonLocalSettings.xml", Properties.Resources.EventHorizonLocalSettings);

                MainWindow.mw.RefreshXML();
            }
        }
        
        internal static void CreateNew_EventHorizonRemoteSettings()
        {
            if (!File.Exists(XMLReaderWriter.DatabaseLocationString + "\\EventHorizonRemoteSettings.xml"))
            {
                File.WriteAllText("EventHorizonRemoteSettings.xml", Properties.Resources.EventHorizonRemoteSettings);

                MainWindow.mw.RefreshXML();
            }
        }

        internal static void CreateSQLiteDatabaseFile()
        {
            XMLReaderWriter.DatabaseSystem = DatabaseSystems.SQLite;
            System.Data.SQLite.SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "\\databaseFile.db3");        // Create the file which will be hosting our database
            XMLReaderWriter.GlobalConnectionString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\databaseFile.db3;";
            CreateEventLogTable();
            CreateUsersTable();
        }
        
        internal static void CreateEventLogTable()
        {
            string sqlquery = @"CREATE TABLE [EventLog]([ID] COUNTER, [EventTypeID] INTEGER, [SourceID] INTEGER, [Details] MEMO, [FrequencyID] INTEGER, [StatusID] INTEGER, [CreatedDateTime] DATETIME, [TargetDateTime] DATETIME, [UserID] INTEGER, [TargetUserID] INTEGER, [ReadByMeID] INTEGER, [LastViewedDateTime] DATETIME, [RemindMeID] INTEGER, [RemindMeDateTime] DATETIME, [NotificationAcknowledged] INTEGER, [ParentEventID] INTEGER, [EventModeID] INTEGER);";

            Console.WriteLine(sqlquery);

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand command = new OleDbCommand(sqlquery, connection))
                            {
                                connection.Open();

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Created Tables";
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
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE EventLog (ID INTEGER PRIMARY KEY, EventTypeID INTEGER, SourceID INTEGER, Details MEMO, FrequencyID INTEGER, StatusID INTEGER, CreatedDateTime DATETIME, TargetDateTime DATETIME, UserID INTEGER, TargetUserID INTEGER, ReadByMeID INTEGER, LastViewedDateTime DATETIME, RemindMeID INTEGER, RemindMeDateTime DATETIME, NotificationAcknowledged INTEGER, ParentEventID INTEGER, EventModeID INTEGER);", connection))
                            {
                                connection.Open();

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Created Tables";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CreateEventLogTable - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
        
        internal static void CreateUsersTable()
        {
            string sqlquery = @"CREATE TABLE [Users]([ID] INTEGER, [LastTimeOnline] DATETIME);";

            Console.WriteLine(sqlquery);

            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (OleDbCommand updateCommand = new OleDbCommand(sqlquery, connection))
                            {
                                connection.Open();

                                updateCommand.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Created Tables";
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
                        }
                    }
                    break;
                case DatabaseSystems.SQLite:
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                        {
                            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE Users (ID INTEGER PRIMARY KEY, LastTimeOnline DATETIME, CONSTRAINT unique_id UNIQUE (ID));", connection))
                            {
                                connection.Open();

                                command.ExecuteNonQuery();

                                MainWindow.mw.Status.Content = "Created Tables";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine("-------------------*---------------------");

                        OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CreateUsersTable - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
    }
}