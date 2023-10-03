using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Threading;

namespace The_Oracle
{
    public class DatabasePoller
    {
        private string connectionString;
        DispatcherTimer pollingTimer;

        public DatabasePoller(string dbConnectionString)
        {
            connectionString = dbConnectionString;

            pollingTimer = new DispatcherTimer();
            pollingTimer.Interval = TimeSpan.FromSeconds(1);// Set the polling interval in milliseconds (e.g., every 5 seconds)
            pollingTimer.Tick += PollDatabase;
            pollingTimer.Start();
        }

        public void StartPolling()
        {
            pollingTimer.Start();
        }

        public void StopPolling()
        {
            pollingTimer.Stop();
        }

        private void PollDatabase(object sender, EventArgs e)
        {
            int recordCount;
            object polledID;
            Int32 ID = -1;

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    Console.WriteLine("Polling Database");

                    connection.Open();

                    // Get the count of records
                    using (SQLiteCommand countCommand = new SQLiteCommand("SELECT COUNT(*) FROM EventLog", connection))
                    {
                        recordCount = Convert.ToInt32(countCommand.ExecuteScalar());
                    }

                    // Get the ID of the last record
                    using (SQLiteCommand lastRecordCommand = new SQLiteCommand("SELECT ID FROM EventLog ORDER BY ID DESC LIMIT 1", connection))
                    {
                        polledID = lastRecordCommand.ExecuteScalar();
                    }

                    if (polledID != null)
                    {
                        Console.WriteLine($"Last Record ID: {polledID}");

                        Console.WriteLine($"Record Count: {recordCount}");

                        ID = Convert.ToInt32(polledID);
                    }
                    else
                    {
                        Console.WriteLine("No records in the table.");
                    }

                    connection.Close();

                    if (recordCount != previousPolledCount || ID != previousPolledID)
                    {
                        MainWindow.mw.RunningTask();

                        EventHorizonLINQ EventHorizonLINQList = DataTableManagement.GetEvent(ID);

                        if (MainWindow.mw.justLoaded == true && EventHorizonLINQList.UserID != XMLReaderWriter.UserID)
                        {
                            EventHorizonBriefNotification eventHorizonBriefNotification = new EventHorizonBriefNotification(MainWindow.mw, ID, 1, 1, EventHorizonLINQList);
                            eventHorizonBriefNotification.Show();
                        }

                        previousPolledID = ID;
                        previousPolledCount = recordCount;
                    }
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while polling the database: " + ex.Message);
                EventHorizonRequesterNotification msg = new EventHorizonRequesterNotification(MainWindow.mw, new OracleCustomMessage { MessageTitleTextBlock = "PollDatabase - ", InformationTextBlock = ex.Message }, RequesterTypes.OK);
                msg.ShowDialog();
            }

            MainWindow.mw.RunCycle();
        }

        public Int32 previousPolledID = -1; // Store the previous polled ID to compare changes
        public Int32 previousPolledCount = -1; // Store the previous row count to compare changes
    }
}