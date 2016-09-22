using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class album
    {
        public string album_code { get; set; }
        public string album_name { get; set; }
        public string album_desc { get; set; }
        public string created_by { get; set; }
        public DateTime album_date { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
