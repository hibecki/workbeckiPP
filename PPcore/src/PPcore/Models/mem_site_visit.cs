using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class mem_site_visit
    {
        public string member_code { get; set; }
        [Display(Name = "ลำดับ")]
        public int rec_no { get; set; }
        [Display(Name = "ชื่อ / คำอธิบาย")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string site_visit_desc { get; set; }
        [Display(Name = "ประเทศ")]
        public int? country_code { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
