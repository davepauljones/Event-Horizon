using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.IO;

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

        public static string GetFirstCharsOfString(string sourceString)
        {
            string workingString = string.Empty;
            string returnString = string.Empty;

            sourceString.Split(' ').ToList().ForEach(i => workingString += (i[0]));

            if (workingString.Length > 1)
            {
                returnString = workingString.Substring(0, 1);
                returnString += workingString.Substring(workingString.Length - 1, 1);
            }
            else if (workingString.Length == 1)
            {
                returnString = workingString;
            }

            return returnString;
        }

        public static string GetUsersInitalsFromUserID(List<User> usersList, Int32 userID)
        {
            string returnString = string.Empty;

            foreach (User user in usersList)
            {
                if (user.ID == userID)
                {
                    returnString = GetFirstCharsOfString(user.UserName);
                }
            }

            return returnString;
        }
        
        public static string GetUsersInitalsFromID(List<User> usersList, Int32 userID)
        {
            string returnString = "★";

            foreach (User user in usersList)
            {
                if (user.ID == userID)
                {
                    returnString = GetFirstCharsOfString(user.UserName);
                }
            }

            return returnString;
        }
        
        public static string GetUserNameFromUserID(List<User> usersList, Int32 userID)
        {
            string returnString = string.Empty;

            foreach (User user in usersList)
            {
                if (user.ID == userID)
                {
                    returnString = user.UserName;
                }
            }

            return returnString;
        }
        
        public static string GetUserNameFromTargetUserID(List<User> usersList, Int32 targetUserID)
        {
            string returnUserName = string.Empty;

            foreach (User user in usersList)
            {
                if (user.ID == targetUserID)
                {
                    returnUserName = user.UserName;
                }
            }

            return returnUserName;
        }
    }
}