using System;
using System.Data;

namespace Event_Horizon
{
    public class EventHorizonEngineer : DataTable
    {
        public EventHorizonEngineer()
        {
            this.Columns.Add("ID", typeof(Int32));
            this.Columns["ID"].DefaultValue = 0;
            this.Columns.Add("CreatedDateTime", typeof(DateTime));
            this.Columns["CreatedDateTime"].DefaultValue = DateTime.MinValue;
            this.Columns.Add("Name", typeof(string));
            this.Columns["Name"].DefaultValue = string.Empty;
            this.Columns.Add("Role", typeof(string));
            this.Columns["Role"].DefaultValue = string.Empty;
            this.Columns.Add("CompetenceDetails", typeof(string));
            this.Columns["CompetenceDetails"].DefaultValue = string.Empty;
        }
    }
}