using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class mem_train_record
    {
        public string course_code { get; set; }
        public string member_code { get; set; }
        public string course_grade { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
    }
}
