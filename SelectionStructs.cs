using System;
using FontAwesome.WPF;
using System.Windows.Media;

namespace The_Oracle
{
    public class User
    {
        public Int32 ID = 0;
        public String UserName = string.Empty;
        public Color Color = Colors.White;
    }
    public class FunctionKeyEventType
    {
        public Int32 EventTypeID = 0;
        public String ShortName = string.Empty;
    }
    public class EventType
    {
        public Int32 ID = 0;
        public String Name = string.Empty;
        public FontAwesomeIcon Icon = FontAwesomeIcon.Star;
        public Color Color = Colors.White;
    }
    public class SourceType
    {
        public Int32 ID = 0;
        public String Name = string.Empty;
        public FontAwesomeIcon Icon = FontAwesomeIcon.Star;
        public Color Color = Colors.White;
    }
    public class FrequencyType
    {
        public Int32 ID = 0;
        public String Name = string.Empty;
        public FontAwesomeIcon Icon = FontAwesomeIcon.Star;
        public Color Color = Colors.White;
        public TimeSpan TimeSpan = TimeSpan.MinValue;
    }
    public class OracleSettings
    {
        public Int32 UserID = 0;
        public String UserName = string.Empty;
        public String DatabaseLocation = string.Empty;
        public String HoverDatabaseLocation = string.Empty;
    }
    public class OracleCustomMessage
    {
        public String MessageTitleTextBlock = string.Empty;
        public String InformationTextBlock = string.Empty;
    }
    public class FileModified
    {
        public string Size;
        public string LastWriteTime;
    }
    public struct OracleMessagesNotificationModes
    {
        public const int Welcome = 0;
        public const int OracleSettingsXmlMissing = 1;
        public const int OracleDatabaseSettingsXmlMissing = 2;
        public const int OracleDatabaseNotFound = 3;
        public const int OracleDatabaseError = 4;
        public const int Custom = 5;
    }
    public struct EventLogFields
    {
        public const int ID = 0;
        public const int EventType = 1;
        public const int SourceID = 2;
        public const int Details = 3;
        public const int FrequencyID = 4;
        public const int StatusID = 5;
        public const int CreatedDateTime = 6;
        public const int TargetDateTime = 7;
        public const int UserID = 8;
        public const int TargetUserID = 9;
        public const int ReadByMeID = 10;
        public const int LastViewedDateTime = 11;
        public const int RemindMeID = 12;
        public const int RemindMeDateTime = 13;
        public const int NotificationAcknowledged = 14;
    }
    public struct DisplayModes
    {
        public const int Normal = 0;
        public const int Reminders = 1;
    }
    public struct FilterModes
    {
        public const int None = 0;
        public const int OriginIsMe = 1;
        public const int OriginOrTargetIsMe = 2;
        public const int OriginAndTargetIsMe = 3;
    }
    public struct ListViews
    {
        public const byte Reminder = 0;
        public const byte Log = 1;
    }
    public struct EventFrequencys
    {
        public const int Common_OneTime = 0;
        public const int Common_Quarterly = 1;
        public const int Common_SixMonthly = 2;
        public const int Common_Yearly = 3;
        public const int Common_TwoYearly = 4;
        public const int Common_ThreeYearly = 5;
        public const int Common_FiveYearly = 6;
        public const int Minutes_01 = 7;
        public const int Minutes_05 = 8;
        public const int Minutes_10 = 9;
        public const int Minutes_30 = 10;
        public const int Hours_01 = 11;
        public const int Hours_02 = 12;
        public const int Hours_03 = 13;
        public const int Hours_08 = 14;
        public const int Hours_12 = 15;
        public const int Days_01 = 16;
        public const int Days_02 = 17;
        public const int Days_03 = 18;
        public const int Weeks_01 = 19;
        public const int Weeks_02 = 20;
        public const int Months_01 = 21;
        public const int Months_02 = 22;
        public const int Months_03 = 23;
        public const int Months_09 = 24;
    }
    public struct ReminderListTimeSpans
    {
        public const int OverDue = 0;
        public const int TimeSpan_1to3_Day = 1;
        public const int TimeSpan_4to7_Days = 2;
        public const int TimeSpan_8to14_Days = 3;
        public const int TimeSpan_15to28_Days = 4;
    }
    public struct Statuses
    {
        public const int Inactive = 0;
        public const int Active = 1;
        public const int ActiveNotified = 2;
        public const int ActiveNotifiedRead = 3;
        public const int ActiveNotifiedReadArchived = 4;
    }
    public struct MarkAsReadModes
    {
        public const int No = 0;
        public const int Yes = 1;
    }
    public struct RemindMeModes
    {
        public const int No = 0;
        public const int Yes = 1;
    }
    public struct RemindMeDateTimes
    {
        public const int FiveMinutes = 0;
        public const int OneHour = 1;
        public const int OneDay = 2;
        public const int TwoDays = 3;
        public const int NextWeek = 4;
        public const int NextMonth = 5;
    }
    public struct ReadByMeModes
    {
        public const int No = 0;
        public const int Yes = 1;
    }
    public struct NotificationAcknowlegedModes
    {
        public const int No = 0;
        public const int Yes = 1;
    }
    public struct TargetDateButtons
    {
        public const int Today = 0;
        public const int OneDay = 1;
        public const int TwoDays = 2;
        public const int ThreeDays = 3;
        public const int FiveDays = 4;
        public const int SevenDays = 5;
    }
    public struct TargetTimeButtons
    {
        public const int Now = 0;
        public const int OneHour = 1;
        public const int TwoHours = 2;
        public const int ThreeHours = 3;
        public const int FourHours = 4;
        public const int FiveHours = 5;
    }
    public struct EventFormCloseButtons
    {
        public const int Cancel = 0;
        public const int Note = 1;
        public const int Reply = 2;
        public const int Save = 3;
    }
    public struct EventModes
    {
        public const int MainEvent = 0;
        public const int NoteEvent = 1;
        public const int ReplyEvent = 2;
    }
    public struct EventWindowModes
    {
        public const int ViewMainEvent = 0;
        public const int ViewNote = 1;
        public const int ViewReply = 2;
        public const int EditMainEvent = 3;
        public const int EditNote = 4;
        public const int EditReply = 5;
        public const int NewEvent = 6;
        public const int NewNote = 7;
        public const int NewReply = 8;
    }
    public struct RequesterTypes
    {
        public const int NoYes = 0;
        public const int OK = 1;
    }
}