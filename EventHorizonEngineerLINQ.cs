using System;
using System.Windows.Media;

namespace Event_Horizon
{
    public class EventHorizonEngineerLINQ : ICloneable
    {
        public Int32 ID = 0;
        public DateTime CreationDate = DateTime.Now;
        public String CreationTime = "00:00";
        public Int32 UserID = 0;
        public String Name = string.Empty;
        public String Role = string.Empty;
        public String CompetenceDetails = string.Empty;

        public object Clone()
        {
            var eventHorizonEngineerLINQClone = (EventHorizonEngineerLINQ)MemberwiseClone();
            return eventHorizonEngineerLINQClone;
        }
    }
}