﻿using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class mem_social
    {
        public string member_code { get; set; }
        public int rec_no { get; set; }
        public string social_desc { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
