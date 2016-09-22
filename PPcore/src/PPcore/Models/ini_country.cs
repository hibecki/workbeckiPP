using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class ini_country
    {
        public int country_code { get; set; }
        public string country_desc { get; set; }
        public string area_part { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
