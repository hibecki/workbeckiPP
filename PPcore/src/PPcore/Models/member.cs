using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class member
    {
        [HiddenInput]
        public string mem_password { get; set; }
        [HiddenInput]
        public string mem_username { get; set; }
        [HiddenInput]
        public Guid mem_role_id { get; set; }

        [Display(Name = "รหัสสมาชิก")]
        //[Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "กรุณากรอกหมายเลข 13 หลัก")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "กรุณากรอกเฉพาะหมายเลข")]
        public string member_code { get; set; }
        [Display(Name = "สมาชิกต้นสังกัด")]
        public string parent_code { get; set; }

        [Display(Name = "ประเภทสมาชิก")]
        public string mem_type_code { get; set; }
        //[Display(Name = "กลุ่มของสมาชิก")]
        [Display(Name = "ประเภทสมาชิก")]
        public string mem_group_code { get; set; }

        [Display(Name = "ระดับสมาชิก")]
        public string mlevel_code { get; set; }
        [Display(Name = "สถานะสมาชิก")]
        public string mstatus_code { get; set; }

        [Display(Name = "คำนำหน้า")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string title { get; set; }

        [Display(Name = "ชื่อ")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string fname { get; set; }
        [Display(Name = "นามสกุล")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string lname { get; set; }

        [Display(Name = "อาชีพ")]
        public string occupation { get; set; }

        [Display(Name = "เพศ")]
        public string sex { get; set; }

        [Display(Name = "วัน/เดือน/ปีเกิด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "กรุณากรอก วัน/เดือน/ปีเกิด")]
        public DateTime? birthdate { get; set; }

        [Display(Name = "อายุปัจจุบัน")]
        public short? current_age { get; set; }
        [Display(Name = "อายุปัจจุบัน")]
        public int age { get
            {
                DateTime now = DateTime.Today;
                int year = 0;
                Int32.TryParse(String.Format("{0:yyyy}", birthdate), out year);
                int a = now.Year - year + 543;
                if (birthdate > now.AddYears(-a)) a--;
                return a;
            }
        }

        [Display(Name = "สถานภาพ")]
        public string marry_status { get; set; }




        [Display(Name = "สัญชาติ")]


        public string nationality { get; set; }
        [Display(Name = "รายได้ต่อเดือน")]
        public string income { get; set; }
        //[Display(Name = "เลขประจำตัวประชาชน")]
        //[Display(Name = "เลขบัตรประชาชน/พาสปอร์ต")]
        [Display(Name = "เลขบัตรประชาชน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        //[StringLength(13, MinimumLength = 13, ErrorMessage = "กรุณากรอกหมายเลข 13 หลัก")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "กรุณากรอกเฉพาะหมายเลข")]
        public string cid_card { get; set; }
        [Display(Name = "ศาสนา")]

        //[RegularExpression(@"^.{3,}$", ErrorMessage = "Minimum 3 characters required")]
        public string religion { get; set; }

        //[Display(Name = "ชื่อสถานที่")]
        [Display(Name = "ที่อยู่ปัจจุบัน")]
        public string place_name { get; set; }
        [Display(Name = "อาคาร")]
        public string building { get; set; }
        [Display(Name = "เลขที่ห้อง")]
        public string room { get; set; }
        [Display(Name = "บ้านเลขที่")]
        public string h_no { get; set; }
        [Display(Name = "ถนน")]
        public string street { get; set; }
        [Display(Name = "ตำบล / แขวง")]
        public string subdistrict_code { get; set; }
        [Display(Name = "จังหวัด")]
        public string province_code { get; set; }
        [Display(Name = "ภาค")]
        public string zone { get; set; }
        [Display(Name = "Latitude")]
        public decimal? latitude { get; set; }

        [Display(Name = "ชั้น")]
        public string floor { get; set; }
        [Display(Name = "หมู่บ้าน")]
        public string village { get; set; }
        [Display(Name = "หมู่ที่")]
        public string lot_no { get; set; }
        [Display(Name = "ตรอก / ซอย")]
        public string lane { get; set; }
        [Display(Name = "อำเภอ")]
        public string district_code { get; set; }
        [Display(Name = "รหัสไปรษณีย์")]
        public string zip_code { get; set; }
        [Display(Name = "ประเทศ")]
        public int? country_code { get; set; }
        [Display(Name = "Longitude")]
        public decimal? longitude { get; set; }

        [Display(Name = "ที่อยู่ 1")]
        public string texta_address { get; set; }
        [Display(Name = "ที่อยู่ 2")]
        public string textb_address { get; set; }
        [Display(Name = "ที่อยู่ 3")]
        public string textc_address { get; set; }
        [Display(Name = "ที่อยู่ในการจัดส่งเอกสาร")]
        public string mail_address { get; set; }

        [Display(Name = "หมายเลขโทรศัพท์มือถือ")]
        //[RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        public string mobile { get; set; }
        [Display(Name = "แฟกซ์")]
        //[RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        public string fax { get; set; }
        [Display(Name = "อีเมล")]
        //[RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "กรุณากรอกอีเมลล์แอดเดรส")]
        public string email { get; set; }

        [Display(Name = "หมายเลขโทรศัพท์บ้าน")]
        //[RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        public string tel { get; set; }

        [Display(Name = "เฟซบุ๊ค")]
        public string facebook { get; set; }
        [Display(Name = "ไลน์")]
        public string line { get; set; }

        [HiddenInput]
        [Display(Name = "รูปถ่ายสมาชิก")]
        public string mem_photo { get; set; }
        [HiddenInput]
        [Display(Name = "รูปของบัตรประชาชน")]
        public string cid_card_pic { get; set; }

        [HiddenInput]
        [Display(Name = "สถานที่สอบ")]
        public string mem_testcenter_code { get; set; }

        [HiddenInput]
        [Display(Name = "วันที่สมัคร")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? register_date { get; set; }

        public Guid id { get; set; }

        [HiddenInput]
        public byte[] rowversion { get; set; }

        public string x_log { get; set; }
        public string x_note { get; set; }
        public string x_status { get; set; }
    }
}
