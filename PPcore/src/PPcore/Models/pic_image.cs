using System;
using System.Collections.Generic;

namespace PPcore.Models
{
    public partial class pic_image
    {
        public string image_code { get; set; }
        public string image_name { get; set; }
        public string image_desc { get; set; }
        public string ref_doc_type { get; set; }
        public string ref_doc_code { get; set; }
        public string image_file { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
    }
}
