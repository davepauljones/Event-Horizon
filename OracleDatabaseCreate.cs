﻿using System;

using System.Data.OleDb;
using System.IO;

namespace The_Oracle
{
    public class OracleDatabaseCreate
    {
        internal static bool CheckIfDatabaseExists()
        {
            if (File.Exists(XMLReaderWriter.DatabaseLocationString + "\\Oracle.mdb"))
            {
                return true;
            }
            else
            {
                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, OracleMessagesNotificationModes.OracleDatabaseNotFound);
                msg.ShowDialog();
                return false;
            }
        }
        
        internal static void Create_Oracle()
        {
            if (!File.Exists(XMLReaderWriter.DatabaseLocationString + "\\Oracle.mdb"))
            { 
                File.WriteAllBytes("Oracle.mdb", Properties.Resources.BlankDB);

                CreateEventLogTable();
                CreateUsersTable();
            }
        }
        
        internal static void CreateNew_OracleSettingsXML()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\OracleSettingsXML.xml"))
            {
                File.WriteAllText("OracleSettingsXML.xml", Properties.Resources.OracleSettingsXML);

                MainWindow.mw.RefreshXML();
            }
        }
        
        internal static void CreateNew_OracleDatabaseSettingsXML()
        {
            if (!File.Exists(XMLReaderWriter.DatabaseLocationString + "\\OracleDatabaseSettingsXML.xml"))
            {
                File.WriteAllText("OracleDatabaseSettingsXML.xml", Properties.Resources.OracleDatabaseSettingsXML);

                MainWindow.mw.RefreshXML();
            }
        }
        
        internal static void CreateEventLogTable()
        {
            String sqlquery = @"CREATE TABLE [EventLog]([ID] COUNTER, [EventTypeID] INTEGER, [SourceID] INTEGER, [Details] MEMO, [FrequencyID] INTEGER, [StatusID] INTEGER, [CreatedDateTime] DATETIME, [TargetDateTime] DATETIME, [UserID] INTEGER, [TargetUserID] INTEGER, [ReadByMeID] INTEGER, [LastViewedDateTime] DATETIME, [RemindMeID] INTEGER, [RemindMeDateTime] DATETIME, [NotificationAcknowledged] INTEGER, [ParentEventID] INTEGER, [EventModeID] INTEGER);";

            Console.WriteLine(sqlquery);

            try
            {
                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0; Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\\Oracle.mdb"))
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
        }
        
        internal static void CreateUsersTable()
        {
            String sqlquery = @"CREATE TABLE [Users]([ID] INTEGER, [LastTimeOnline] DATETIME);";

            Console.WriteLine(sqlquery);

            try
            {
                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0; Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\\Oracle.mdb"))
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
        }
    }
}