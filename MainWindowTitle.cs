using System;

namespace The_Oracle
{
    public static class MainWindowTitle
    {
        public static String TitleString;
        public static String PathString = AppDomain.CurrentDomain.BaseDirectory;
        public static String OracleFileName = "EventHorizonRemoteDatabase.mdb";

        public static void SetMainWindowTitle()
        {
            PathString = XMLReaderWriter.DatabaseLocationString;

            string swv = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy");
            TitleString = "Event Horizon - Build ";
            TitleString += swv;
            TitleString += " - User ";
            TitleString += XMLReaderWriter.UserID + " ";
            TitleString += XMLReaderWriter.UserNameString;
            TitleString += " - Connected to database ";
            TitleString += PathString;
            TitleString += "\\";
            TitleString += OracleFileName;
            TitleString += " - GPLv3(2023)davepauljones";

            MainWindow.HSE_LOG_GlobalMDBConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source = " + XMLReaderWriter.DatabaseLocationString + "\\" + OracleFileName;

            MainWindow.mw.Title = TitleString;
        }
    }
}