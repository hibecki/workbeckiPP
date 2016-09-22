using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class mem_site_visit
    {
        public string member_code { get; set; }
        public int rec_no { get; set; }
        public string site_visit_desc { get; set; }
        public int? country_code { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
