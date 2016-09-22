using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class mem_level
    {
        public string mlevel_code { get; set; }
        public string mlevel_desc { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
