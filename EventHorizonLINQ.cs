using System;
using System.Windows.Media;

namespace The_Oracle
{
    public class EventHorizonLINQ : ICloneable
    {
        public Int32 Source_ID = 0;
        public Int32 Source_UserID = 0;
        public Int32 Source_Mode = EventWindowModes.ViewMainEvent;
        public Int32 Source_ParentEventID = 0;

        public Int32 ID = 0;
        public DateTime CreationDate = DateTime.Now;
        public String CreationTime = "00:00";
        public Int32 EventTypeID = 0;
        public Int32 SourceID = 0;
        public String Details = string.Empty;
        public Int32 FrequencyID = EventFrequencys.Common_OneTime;
        public Int32 StatusID = Statuses.Active;
        public DateTime TargetDate = DateTime.Now;
        public String TargetTime = "00:00";
        public Int32 UserID = 0;
        public Int32 TargetUserID = 0;
        public Int32 ReadByMeID = 0;
        public DateTime LastViewedDate = DateTime.Now;
        public Int32 RemindMeID = 0;
        public DateTime RemindMeDateTime = DateTime.Now;
        public Int32 NotificationAcknowledged = 0;
        public Int32 EventModeID = 0;
        
        public Int32 EventAttributeID = 0;
        public String PathFileName = string.Empty;
        public Double UnitCost = 0;
        public Int32 Qty = 0;
        public Double Discount = 0;
        public Int32 Stock = 0;

        public Int32 Attributes_TotalDays = 0;
        public Color Attributes_TotalDaysEllipseColor = Colors.Black;
        public Int32 Attributes_Replies = 0;

        public object Clone()
        {
            var eventHorizonLINQClone = (EventHorizonLINQ)MemberwiseClone();
            return eventHorizonLINQClone;
        }
    }
}