using System;
using System.Collections.Generic;

namespace PPcoreDBPrepare.Models
{
    public partial class mem_worklist
    {
        public int rec_no { get; set; }
        public string member_code { get; set; }
        public string company_name_th { get; set; }
        public string company_name_eng { get; set; }
        public string position_name_th { get; set; }
        public string position_name_eng { get; set; }
        public string work_year { get; set; }
        public string office_address { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        public byte[] rowversion { get; set; }
    }
}
