using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class product_type
    {
        [Display(Name = "รหัสกลุ่มผลิตผล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [HiddenInput]
        public string product_group_code { get; set; }
        [Display(Name = "รหัสประเภทผลิตผล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string product_type_code { get; set; }
        [Display(Name = "ประเภทผลิตผล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string product_type_desc { get; set; }
        [Display(Name = "สถานะ")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
