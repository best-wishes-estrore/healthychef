using System;

namespace HealthyChef.WebModules.Reports.Admin
{
    public class QueryDataObject
    {
        public int ProgramId
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }
    }
}