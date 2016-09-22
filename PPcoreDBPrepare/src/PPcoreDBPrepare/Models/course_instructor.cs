using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class course_instructor
    {
        public string instructor_code { get; set; }
        public string course_code { get; set; }
        public DateTime? confirm_date { get; set; }
        public string ref_doc { get; set; }
        public decimal? instructor_cost { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
    }
}
