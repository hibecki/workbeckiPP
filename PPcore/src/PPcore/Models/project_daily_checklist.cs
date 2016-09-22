using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class project_daily_checklist
    {
        public string course_code { get; set; }
        public string member_code { get; set; }
        public DateTime course_date { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
    }
}
