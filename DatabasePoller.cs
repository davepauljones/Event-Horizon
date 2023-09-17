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
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    Console.Write("--------------------PollDatabase connectionString = ");
                    Console.WriteLine(connectionString);
                    // Perform a query to check for changes (e.g., SELECT COUNT(*) FROM TableName)
                    using (var command = new SQLiteCommand("SELECT COUNT(*) FROM EventLog", connection))
                    {
                        var rowCount = Convert.ToInt32(command.ExecuteScalar());

                        // Compare the current row count with the previous row count
                        if (rowCount != previousRowCount)
                        {
                            Console.WriteLine("Changes detected in the database.");
                            // Handle the changes here (e.g., update UI, trigger actions)

                            if (MainWindow.mw.ReminderListView.SelectedItems.Count == 0)
                            {
                                if (MainWindow.mw.DisplayMode == DisplayModes.Reminders)
                                    MainWindow.mw.RefreshLog(ListViews.Reminder);
                                else
                                    MainWindow.mw.RefreshLog(ListViews.Log);
                            }

                            previousRowCount = rowCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while polling the database: " + ex.Message);
            }
        }

        public int previousRowCount = -1; // Store the previous row count to compare changes
    }
}