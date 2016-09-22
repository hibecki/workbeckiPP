using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class mem_group
    {
        public string mem_group_code { get; set; }
        public string mem_group_desc { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
