using System;
using System.Windows.Media;

namespace Event_Horizon
{
    public class EventHorizonRiskAssessmentLINQ : ICloneable
    {
        public Int32 Source_Mode = EventWindowModes.ViewMainEvent;

        public Int32 ID = 0;
        public Int32 RevisionNo = 0;
        public String ElementReviewed = string.Empty;
        public DateTime ReviewedDateTime = DateTime.Now;
        public String ReviewedTime = "00:00";
        public Int32 StatusID = 0;

        public object Clone()
        {
            var eventHorizonRiskAssessmentLINQClone = (EventHorizonRiskAssessmentLINQ)MemberwiseClone();
            return eventHorizonRiskAssessmentLINQClone;
        }
    }
}