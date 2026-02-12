using System;
using System.Data;

namespace Event_Horizon
{
    public class EventHorizonRams : DataTable
    {
        public EventHorizonRams()
        {
            this.Columns.Add("ID", typeof(Int32));
            this.Columns["ID"].DefaultValue = 0;
            this.Columns.Add("EventTypeID", typeof(Int32));
            this.Columns["EventTypeID"].DefaultValue = 0;
            this.Columns.Add("UserID", typeof(Int32));
            this.Columns["UserID"].DefaultValue = 0;
            this.Columns.Add("CreatedDateTime", typeof(DateTime));
            this.Columns["CreatedDateTime"].DefaultValue = DateTime.MinValue;
            this.Columns.Add("TargetUserID", typeof(Int32));
            this.Columns["TargetUserID"].DefaultValue = 0;
            this.Columns.Add("TargetDateTime", typeof(DateTime));
            this.Columns["TargetDateTime"].DefaultValue = DateTime.MinValue;
            this.Columns.Add("Details", typeof(string));
            this.Columns["Details"].DefaultValue = string.Empty;
            this.Columns.Add("SourceID", typeof(Int32));
            this.Columns["SourceID"].DefaultValue = 0;
            this.Columns.Add("FrequencyID", typeof(Int32));
            this.Columns["FrequencyID"].DefaultValue = 0;
            this.Columns.Add("StatusID", typeof(Int32));
            this.Columns["StatusID"].DefaultValue = 0;
            this.Columns.Add("ReadByMeID", typeof(Int32));
            this.Columns["ReadByMeID"].DefaultValue = 0;
            this.Columns.Add("LastViewedDateTime", typeof(DateTime));
            this.Columns["LastViewedDateTime"].DefaultValue = DateTime.MinValue;
            this.Columns.Add("RemindMeID", typeof(Int32));
            this.Columns["RemindMeID"].DefaultValue = 0;
            this.Columns.Add("RemindMeDateTime", typeof(DateTime));
            this.Columns["RemindMeDateTime"].DefaultValue = DateTime.MinValue;
            this.Columns.Add("NotificationAcknowledged", typeof(Int32));
            this.Columns["NotificationAcknowledged"].DefaultValue = 0;
            this.Columns.Add("ParentEventID", typeof(Int32));
            this.Columns["ParentEventID"].DefaultValue = 0;
            this.Columns.Add("EventModeID", typeof(Int32));
            this.Columns["EventModeID"].DefaultValue = 0;
        }
    }
}