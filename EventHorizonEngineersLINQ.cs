using System;
using System.Windows.Media;

namespace Event_Horizon
{
    public class EventHorizonEngineersLINQ : ICloneable
    {
        public Int32 ID = 0;
        public DateTime CreationDate = DateTime.Now;
        public String CreationTime = "00:00";
        public String Name = string.Empty;
        public String Role = string.Empty;
        public String CompetenceDetails = string.Empty;

        public object Clone()
        {
            var eventHorizonRamsLINQClone = (EventHorizonRamsLINQ)MemberwiseClone();
            return eventHorizonRamsLINQClone;
        }
    }
}