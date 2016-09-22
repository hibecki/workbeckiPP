using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class project_course
    {
        public string course_code { get; set; }
        public string project_code { get; set; }
        public string ctype_code { get; set; }
        public string cgroup_code { get; set; }
        public string course_desc { get; set; }
        public DateTime? course_date { get; set; }
        public DateTime? course_approve_date { get; set; }
        public DateTime? course_begin { get; set; }
        public DateTime? course_end { get; set; }
        public string ref_doc { get; set; }
        public decimal? budget { get; set; }
        public string charge_head { get; set; }
        public string support_head { get; set; }
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
