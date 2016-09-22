﻿using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class ini_list_zip
    {
        public string province_code { get; set; }
        public int country_code { get; set; }
        public string district_code { get; set; }
        public string subdistrict_code { get; set; }
        public string zip_code { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
