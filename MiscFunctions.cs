using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Media;
using System.IO;

using System.Reflection;

namespace The_Oracle
{
    public class MiscFunctions
    {
        static MediaPlayer mediaPlayer;
        
        public static void PlayFile(string filename)
        {
            string directory = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + @"");
            string filePath = System.IO.Path.Combine(directory, filename);

            if (File.Exists(filePath))
            {
                try
                {
                    mediaPlayer = new MediaPlayer();
                    mediaPlayer.Open(new Uri(filePath));
                    mediaPlayer.Volume = 0.5;
                    mediaPlayer.Play();
                }
                catch (IOException iox)
                {
                    Console.WriteLine("Error, could not play " + filename + "Reported error was " + iox);
                }
            }
            else
            {
                Console.WriteLine("Filename does not exists, specify a file that exists!");
            }
        }

        public static String GetFirstCharsOfString(string SourceString)
        {
            string WorkingString = string.Empty;
            string ReturnString = string.Empty;

            SourceString.Split(' ').ToList().ForEach(i => WorkingString += (i[0]));

            if (WorkingString.Length > 1)
            {
                ReturnString = WorkingString.Substring(0, 1);
                ReturnString += WorkingString.Substring(WorkingString.Length - 1, 1);
            }
            else if (WorkingString.Length == 1)
            {
                ReturnString = WorkingString;
            }

            return ReturnString;
        }

        public static String GetUsersInitalsFromUserID(List<User> UsersList, Int32 UserID)
        {
            String ReturnString = string.Empty;

            foreach (User u in UsersList)
            {
                if (u.ID == UserID)
                {
                    ReturnString = GetFirstCharsOfString(u.UserName);
                }
            }

            return ReturnString;
        }
        
        public static String GetUsersInitalsFromID(List<User> UsersList, Int32 UserID)
        {
            String ReturnString = "★";

            foreach (User u in UsersList)
            {
                if (u.ID == UserID)
                {
                    ReturnString = GetFirstCharsOfString(u.UserName);
                }
            }

            return ReturnString;
        }
        
        public static String GetUserNameFromUserID(List<User> UsersList, Int32 UserID)
        {
            String ReturnString = string.Empty;

            foreach (User u in UsersList)
            {
                if (u.ID == UserID)
                {
                    ReturnString = u.UserName;
                }
            }

            return ReturnString;
        }
        
        public static String GetUserNameFromTargetUserID(List<User> UsersList, Int32 TargetUserID)
        {
            String ReturnUserName = string.Empty;

            foreach (User u in UsersList)
            {
                if (u.ID == TargetUserID)
                {
                    ReturnUserName = u.UserName;
                }
            }

            return ReturnUserName;
        }

    }
}