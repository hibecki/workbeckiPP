using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class album
    {
        [HiddenInput]
        public string album_code { get; set; }

        [Display(Name = "ชื่ออัลบั้ม")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string album_name { get; set; }

        [Display(Name = "คำอธิบาย")]
        public string album_desc { get; set; }

        [HiddenInput]
        public string album_type { get; set; }

        [Display(Name = "สร้างโดย")]
        [HiddenInput]
        public string created_by { get; set; }

        [Display(Name = "วันที่")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime album_date { get; set; }

        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }

        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
