using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class project_supporter
    {
        [Display(Name = "รหัสโครงการ")]
        public string project_code { get; set; }
        [Display(Name = "รหัสผู้สนับสนุน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string spon_code { get; set; }
        public string ref_doc { get; set; }
        [Display(Name = "งบประมาณ")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        //[DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? support_budget { get; set; }
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
