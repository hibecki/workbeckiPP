using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class mem_health
    {
        public string member_code { get; set; }
        public string medical_history { get; set; }
        public string blood_group { get; set; }
        public string hobby { get; set; }
        public string restrict_food { get; set; }
        public string special_skill { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
