using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.ViewModels.member
{
    public class MemberIndexViewModel
    {
        public Guid id { get; set; }

        [Display(Name = "รหัสสมาชิก")]
        public string member_code { get; set; }

        [Display(Name = "คำนำหน้า")]
        public string title { get; set; }

        [Display(Name = "ชื่อ")]
        public string fname { get; set; }
        [Display(Name = "นามสกุล")]
        public string lname { get; set; }

        [Display(Name = "วัน/เดือน/ปีเกิด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? birthdate { get; set; }

        //[Display(Name = "อายุปัจจุบัน")]
        //public short? current_age { get; set; }
        [Display(Name = "อายุปัจจุบัน")]
        public int age
        {
            get
            {
                DateTime now = DateTime.Today;
                int year = 0;
                Int32.TryParse(String.Format("{0:yyyy}", birthdate), out year);
                int a = now.Year - year + 543;
                if (birthdate > now.AddYears(-a)) a--;
                return a;
            }

            set { }
        }

        [Display(Name = "เลขบัตรประชาชน")]
        public string cid_card { get; set; }

        [Display(Name = "ที่อยู่ 1")]
        public string texta_address { get; set; }
        [Display(Name = "ที่อยู่ 2")]
        public string textb_address { get; set; }
        [Display(Name = "ที่อยู่ 3")]
        public string textc_address { get; set; }

        [Display(Name = "หมายเลขโทรศัพท์บ้าน")]
        public string tel { get; set; }
        [Display(Name = "หมายเลขโทรศัพท์มือถือ")]
        public string mobile { get; set; }
        [Display(Name = "อีเมล")]
        public string email { get; set; }

        [Display(Name = "เฟซบุ๊ค")]
        public string facebook { get; set; }
        [Display(Name = "ไลน์")]
        public string line { get; set; }

        public byte[] rowversion { get; set; }
    }
}
