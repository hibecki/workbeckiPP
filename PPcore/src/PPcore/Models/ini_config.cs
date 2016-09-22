using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class ini_config
    {
        public string client_code { get; set; }
        public string system { get; set; }
        public string module { get; set; }
        public string cnfig_item { get; set; }
        public string text_value { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
