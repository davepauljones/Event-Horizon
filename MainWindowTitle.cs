using System;

namespace Event_Horizon
{
    public static class MainWindowTitle
    {
        public static void SetMainWindowTitle()
        {
            String TitleString;
            TitleString = "Event Horizon";
            TitleString += " - User ";
            TitleString += XMLReaderWriter.UserID + " ";
            TitleString += XMLReaderWriter.UserNameString;
            TitleString += " - GPL V3 (2023 - ";
            TitleString += System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy");
            TitleString += ") davepauljones";

            MainWindow.mw.Title = TitleString;
        }
    }
}