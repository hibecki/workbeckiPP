using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class course_train_place
    {
        [Display(Name = "รหัสสถานที่")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string place_code { get; set; }
        public string course_code { get; set; }
        [Display(Name = "วันที่เริ่มใช้สถานที่จัดอบรม")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? confirm_date { get; set; }
        public string ref_doc { get; set; }
        [Display(Name = "ค่าสถานที่")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        //[DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? place_cost { get; set; }
        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
    }
}
