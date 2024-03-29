﻿using System;
using System.Data.OleDb;
using System.IO;
using System.Data.SQLite;
using Microsoft.Win32;

namespace Event_Horizon
{
    public class EventHorizonDatabaseCreate
    {
        internal static bool CheckIfDatabaseExists()
        {
            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\" + XMLReaderWriter.GlobalDatabaseString + XMLReaderWriter.GlobalDatabaseFileExtensionString))
            {
                return true;
            }
            else
            {
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "Event Horizon could not connect to a database", InformationTextBlock = "You have a few options, you can browse for the database, create a new database or close Event Horizon and seek IT Support." }, RequesterTypes.OK);
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "EventHorizonRemoteDatabase"; // Default file name
            saveFileDialog.DefaultExt = ".db3"; // Default file extension
            saveFileDialog.Filter = "SQLite databases (.db3)|*.db3"; // Filter files by extension
            saveFileDialog.InitialDirectory = XMLReaderWriter.DatabaseLocationString;
            saveFileDialog.OverwritePrompt = false;

            // Show open file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                string filename = saveFileDialog.FileName;

                if (!File.Exists(filename))
                {
                    XMLReaderWriter.DatabaseSystem = DatabaseSystems.SQLite;
                    System.Data.SQLite.SQLiteConnection.CreateFile(filename);// Create the file which will be hosting our database
                    XMLReaderWriter.GlobalConnectionString = "Data Source=" + filename;
                    CreateEventLogTable();
                    CreateUsersTable();
                    EventHorizonRequesterNotification fileCreated = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = ReusableMessages.SuccessfullyCreatedFile, InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility }, RequesterTypes.OK);
                    fileCreated.ShowDialog();
                }
                else
                {
                    EventHorizonRequesterNotification fileExistWarning = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = ReusableMessages.ThatFileAlreadyExists, InformationTextBlock = ReusableMessages.WithGreatPowerComesGreatResponsibility }, RequesterTypes.OK);
                    fileExistWarning.ShowDialog();
                }
            }   
        }

        internal static string OpenRemoteSettingsXMLFile()
        {
            string pathFileName = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.FileName = "EventHorizonRemoteDatabase"; // Default file name
            openFileDialog.DefaultExt = ".xml"; // Default file extension
            openFileDialog.Filter = "XML file (.xml)|*.xml"; // Filter files by extension
            openFileDialog.InitialDirectory = XMLReaderWriter.DatabaseLocationString;

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                pathFileName = openFileDialog.FileName;

                XMLReaderWriter.SetGlobalRemoteSettingsPathFileName(Path.GetFileNameWithoutExtension(openFileDialog.FileName));

                Console.Write("OpenRemoteSettingsXMLFile - Path.GetFileNameWithoutExtension(openFileDialog.FileName) = ");
                Console.WriteLine(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
            }

            return pathFileName;
        }

        internal static string OpenSQLiteDatabaseFile()
        {
            string pathFileName = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.FileName = "EventHorizonRemoteDatabase"; // Default file name
            openFileDialog.DefaultExt = ".db3"; // Default file extension
            openFileDialog.Filter = "SQLite databases (.db3)|*.db3"; // Filter files by extension
            openFileDialog.InitialDirectory = XMLReaderWriter.DatabaseLocationString;

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                pathFileName = openFileDialog.FileName;

                XMLReaderWriter.GlobalDatabaseString = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                XMLReaderWriter.SetDatabaseConnectionString();

                Console.Write("OpenSQLiteDatabaseFile - Path.GetFileNameWithoutExtension(openFileDialog.FileName) = ");
                Console.WriteLine(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
            }

            return pathFileName;
        }
        
        internal static string OpenFile(string filter, string initialDirectory=null)
        {
            string pathFileName = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.FileName = "EventHorizonRemoteDatabase"; // Default file name
            openFileDialog.DefaultExt = ".*"; // Default file extension
            //openFileDialog.Filter = "PNG file (.png)|*.png"; // Filter files by extension
            openFileDialog.Filter = filter;
              //"All supported files|*.jpg;*.jpeg;*.png|" +
              //"JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              //"Portable Network Graphic (*.png)|*.png|" +
              //"PDF (*.pdf)|*.pdf|" +
              //"All files (*.*)|*.*";
            
            if (initialDirectory != null) openFileDialog.InitialDirectory = initialDirectory;

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                pathFileName = openFileDialog.FileName;

                Console.Write("OpenImageFile - openFileDialog.FileName = ");
                Console.WriteLine(openFileDialog.FileName);
            }

            return pathFileName;
        }
        internal static string OpenImageFile()
        {
            string pathFileName = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.FileName = "EventHorizonRemoteDatabase"; // Default file name
            openFileDialog.DefaultExt = ".png"; // Default file extension
            //openFileDialog.Filter = "PNG file (.png)|*.png"; // Filter files by extension
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            //openFileDialog.InitialDirectory = XMLReaderWriter.DatabaseLocationString;

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                pathFileName = openFileDialog.FileName;

                Console.Write("OpenImageFile - openFileDialog.FileName = ");
                Console.WriteLine(openFileDialog.FileName);
            }

            return pathFileName;
        }

        internal static void CreateEventLogTable()
        {
            string sqlquery = @"CREATE TABLE [EventLog]([ID] COUNTER, [EventTypeID] INTEGER, [SourceID] INTEGER, [Details] MEMO, [FrequencyID] INTEGER, [StatusID] INTEGER, [CreatedDateTime] DATETIME, [TargetDateTime] DATETIME, [UserID] INTEGER, [TargetUserID] INTEGER, [ReadByMeID] INTEGER, [LastViewedDateTime] DATETIME, [RemindMeID] INTEGER, [RemindMeDateTime] DATETIME, [NotificationAcknowledged] INTEGER, [ParentEventID] INTEGER, [EventModeID] INTEGER, [EventAttributeID] INTEGER, [PathFileName] MEMO, [UnitCost] CURRENCY);";

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
                            using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE EventLog (ID INTEGER PRIMARY KEY, EventTypeID INTEGER, SourceID INTEGER, Details MEMO, FrequencyID INTEGER, StatusID INTEGER, CreatedDateTime DATETIME, TargetDateTime DATETIME, UserID INTEGER, TargetUserID INTEGER, ReadByMeID INTEGER, LastViewedDateTime DATETIME, RemindMeID INTEGER, RemindMeDateTime DATETIME, NotificationAcknowledged INTEGER, ParentEventID INTEGER, EventModeID INTEGER, EventAttributeID INTEGER, PathFileName TEXT, UnitCost REAL, Qty INTEGER, Discount REAL, Stock INTEGER);", connection))
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

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CreateEventLogTable - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
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

                        EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "CreateUsersTable - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                        msg.ShowDialog();
                    }
                    break;
            }
        }
        internal static void AddFieldsToExistingTable()
        {
            switch (XMLReaderWriter.DatabaseSystem)
            {
                case DatabaseSystems.AccessMDB:                  
                    break;
                case DatabaseSystems.SQLite:
                    //try
                    //{
                    //    using (SQLiteConnection connection = new SQLiteConnection(XMLReaderWriter.GlobalConnectionString))
                    //    {
                    //        using (SQLiteCommand command = new SQLiteCommand("ALTER TABLE EventLog ADD COLUMN Stock INTEGER;", connection))
                    //        {
                    //            connection.Open();

                    //            command.ExecuteNonQuery();

                    //            MainWindow.mw.Status.Content = "Added Fields";
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    // Handle exceptions here
                    //    Console.WriteLine("Error: " + ex.Message);
                    //    Console.WriteLine("-------------------*---------------------");

                    //    OracleRequesterNotification msg = new OracleRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "AddFieldsToExistingTable - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                    //    msg.ShowDialog();
                    //}
                    break;
            }
        }
    }
}