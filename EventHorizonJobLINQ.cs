using System;
using System.Windows.Media;

namespace Event_Horizon
{
    public class EventHorizonJobLINQ : ICloneable
    {
        public Int32 Source_Mode = EventWindowModes.ViewMainEvent;

        public Int32 ID = 0;
        public DateTime CreationDate = DateTime.Now;
        public String CreationTime = "00:00";
        public String JobNo = string.Empty;
        public String Description = string.Empty;
        public Int32 RamsProfileTypeID = 0;
        public Int32 UserID = 0;
        public String ClientName = string.Empty;
        public String Site = string.Empty;
        public String LocationActivity = string.Empty;
        public Int32 RevisionNo = 0;
        public String ElementReviewed = string.Empty;
        public DateTime ReviewedDateTime = DateTime.Now;
        public String ReviewedTime = "00:00";
        public String MSContractTitle = string.Empty;
        public Int32 MSRevisionNo = 0;
        public String MSContractor = string.Empty;
        public Int32 StatusID = 0;

        public object Clone()
        {
            var eventHorizonJobLINQClone = (EventHorizonJobLINQ)MemberwiseClone();
            return eventHorizonJobLINQClone;
        }
    }
}