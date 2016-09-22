using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class product_group
    {
        public string product_group_code { get; set; }
        public string product_group_desc { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
