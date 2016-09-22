using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class project
    {
        public string project_code { get; set; }
        public string project_desc { get; set; }
        public DateTime? project_date { get; set; }
        public DateTime? project_approve_date { get; set; }
        public string ref_doc { get; set; }
        public decimal? budget { get; set; }
        public string project_manager { get; set; }
        public int? target_member_join { get; set; }
        public int? active_member_join { get; set; }
        public int? passed_member { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
    }
}
