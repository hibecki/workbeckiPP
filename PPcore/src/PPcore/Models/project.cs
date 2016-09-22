using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class project
    {
        [Display(Name = "รหัสโครงการ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string project_code { get; set; }
        [Display(Name = "ชื่อโครงการ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string project_desc { get; set; }
        [Display(Name = "วันที่จัดตั้งโครงการ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? project_date { get; set; }
        [Display(Name = "วันที่อนุมัติโครงการ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? project_approve_date { get; set; }
        [Display(Name = "เอกสารอ้างอิง")]
        public string ref_doc { get; set; }
        [Display(Name = "งบประมาณ")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        //[DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? budget { get; set; }
        [Display(Name = "ผู้รับผิดชอบโครงการ")]
        public string project_manager { get; set; }
        [Display(Name = "จำนวนเป้าหมายผู้เข้าร่วมโครงการ")]
        public int? target_member_join { get; set; }
        [Display(Name = "จำนวนผู้ลงทะเบียนเข้าร่วมโครงการ")]
        public int? active_member_join { get; set; }
        [Display(Name = "จำนวนผู้ที่สอบผ่าน")]
        public int? passed_member { get; set; }
        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
    }
}
