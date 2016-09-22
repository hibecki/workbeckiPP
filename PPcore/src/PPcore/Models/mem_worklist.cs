using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class mem_worklist
    {
        [Display(Name = "ลำดับที่")]
        public int rec_no { get; set; }
        public string member_code { get; set; }
        [Display(Name = "ชื่อสถานที่ทำงาน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string company_name_th { get; set; }
        [Display(Name = "ชื่อสถานที่ทำงาน (ภาษาอังกฤษ)")]
        public string company_name_eng { get; set; }
        [Display(Name = "ตำแหน่งงาน")]
        public string position_name_th { get; set; }
        [Display(Name = "ตำแหน่งงาน (ภาษาอังกฤษ)")]
        public string position_name_eng { get; set; }
        //[Display(Name = "ปีที่ทำงาน")]
        [Display(Name = "ปีที่เริ่มทำงาน-ปีที่สิ้นสุด")]
        public string work_year { get; set; }
        [Display(Name = "ปีที่เริ่มทำงาน-ปีที่สิ้นสุด")]
        public string workYear
        {
            get
            {
                if (String.IsNullOrEmpty(work_year) || (work_year.Trim() == "-"))
                {
                    return "";
                } else {
                    string[] wy = work_year.Split('-');
                    if (String.IsNullOrEmpty(wy[0]))
                    {
                        return wy[1];
                    } else if (String.IsNullOrEmpty(wy[1]))
                    {
                        return wy[0];
                    } else
                    {
                        return work_year;
                    }

                } 
            }
        }

        [Display(Name = "ที่อยู่สถานที่ทำงาน")]
        public string office_address { get; set; }
        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
        public Guid id { get; set; }
        [HiddenInput]
        public byte[] rowversion { get; set; }
    }
}
