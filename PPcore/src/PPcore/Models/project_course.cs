using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class project_course
    {
        [Display(Name = "รหัสหลักสูตร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string course_code { get; set; }
        [Display(Name = "รหัสโครงการ")]
        public string project_code { get; set; }
        [Display(Name = "ประเภทหลักสูตร")]
        public string ctype_code { get; set; }
        [Display(Name = "กลุ่มหลักสูตร")]
        public string cgroup_code { get; set; }
        [Display(Name = "ชื่อหลักสูตรอบรม")] //ชื่อ/คำอธบายหลักสูตร
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string course_desc { get; set; }
        
        [Display(Name = "วันที่จัดตั้งหลักสูตรอบรม")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? course_date { get; set; }
        [Display(Name = "วันที่อนุมัติหลักสูตรอบรม")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? course_approve_date { get; set; }
        [Display(Name = "วันที่อบรม")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? course_begin { get; set; }
        [Display(Name = "วันที่สิ้นสุด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? course_end { get; set; }
        [Display(Name = "เอกสารอ้างอิง")]
        public string ref_doc { get; set; }
        
        [Display(Name = "งบประมาณ")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        //[DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? budget { get; set; }
        
        [Display(Name = "ค่าใช้จ่ายต่อคน")]
        public decimal? charge_head { get; set; }
        [Display(Name = "ค่าใช้จ่ายที่โครงการออกให้")]
        public decimal? support_head { get; set; }
        
        [Display(Name = "ผู้รับผิดชอบ")]
        public string project_manager { get; set; }
        [Display(Name = "จำนวนเป้าหมายผู้เข้าอบรม")]
        public int? target_member_join { get; set; }

        [Display(Name = "จำนวนผู้ลงทะเบียนเข้าอบรม")]
        public int? active_member_join { get; set; }


        [Display(Name = "จำนวนผู้ที่สอบผ่าน")]
        public int? passed_member { get; set; }

        [Display(Name = "เกณฑ์การประเมิน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public int passed_score { get; set; }

        [Display(Name = "สถานะ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        [HiddenInput]
        public Guid id { get; set; }
    }
}
