using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class instructor
    {
        [Display(Name = "รหัสวิทยากร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string instructor_code { get; set; }
        [Display(Name = "ชื่อวิทยากร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string instructor_desc { get; set; }
        [Display(Name = "วันที่ยืนยันการอบรม")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? confirm_date { get; set; }
        [Display(Name = "เอกสารอ้างอิง")]
        public string ref_doc { get; set; }
        [Display(Name = "ตัวแทน/ผู้ติดต่อ")]
        public string contactor { get; set; }
        [Display(Name = "ข้อมูลในการติดต่อ")]
        public string contactor_detail { get; set; }
        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
    }
}
