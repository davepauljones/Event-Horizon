using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace The_Oracle
{
    public class EventHorizonPrerequisites
    {
        //Prerequisites for Event Horizon Startup

        //Check EventHorizonLocalSettings.xml file exists
        //Report the files contents, ie Current User Details / Path of EventHorizonDatabase.mdb
        
        //Check EventHorizonRemoteSettings.xml file exists
        //Report the files contents, ie All Users, All Event Types, All Source Types

        //Check EventHorizonRemoteDatabase.mdb file exists
        //Report the files contents, ie number of rows, last time accessed

        public struct PrerequisitesProgress
        {
            public const int Passed_EventHorizonLocalSettings = 1;
            public const int Passed_EventHorizonLocalSettings_EventHorizonRemoteSettings = 2;
            public const int Passed_EventHorizonLocalSettings_EventHorizonRemoteSettings_EventHorizonRemoteDatabase = 3;
        }

        public struct PrerequisitesMessages
        {
            public const string Missing_EventHorizonLocalSettings = "Event Horizon Local Settings File Not Found!";
            public const string Missing_EventHorizonRemoteSettings = "Event Horizon Remote Setting File Not Found!";
            public const string Missing_EventHorizonRemoteDatabase = "Event Horizon Remote Database File Not Found!";
            public const string Missing_EventHorizonLocalSettings_UserDetailsNotFound = "Event Horizon Local Settings User Details Not Found!";
            public const string Missing_EventHorizonLocalSettings_RemoteDatabasePathNotFound = "Event Horizon Local Settings Remote Database Path Not Found!";
        }

        public static void CheckPrerequisites()
        {
            int results = 0;

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\EventHorizonLocalSettings.xml"))
                results = PrerequisitesProgress.Passed_EventHorizonLocalSettings;
            else
            {
                OracleMessagesNotification msg = new OracleMessagesNotification(MainWindow.mw, 1);
                msg.ShowDialog();
                results = PrerequisitesProgress.Passed_EventHorizonLocalSettings;
            }

            Console.WriteLine(results);
        }
    }
}
