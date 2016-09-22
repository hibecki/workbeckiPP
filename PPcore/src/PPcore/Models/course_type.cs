using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class course_type
    {
        [Display(Name = "รหัสกลุ่มหลักสูตร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string cgroup_code { get; set; }
        [Display(Name = "รหัสประเภทหลักสูตร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string ctype_code { get; set; }
        [Display(Name = "ชื่อ / คำอธิบาย")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string ctype_desc { get; set; }
        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
