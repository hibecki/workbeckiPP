using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class train_place
    {
        public string place_code { get; set; }
        public string instructor_desc { get; set; }
        public DateTime? confirm_date { get; set; }
        public string ref_doc { get; set; }
        public string contactor { get; set; }
        public string contactor_detail { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
    }
}
