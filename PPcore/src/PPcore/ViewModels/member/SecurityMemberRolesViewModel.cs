using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.ViewModels.member
{
    public class SecurityMemberRolesViewModel
    {
        public Guid memberId { get; set; }

        [Display(Name = "ชื่อผู้ใช้")]
        public string mem_username { get; set; }
        [Display(Name = "ชื่อที่แสดง")]
        public string displayname { get; set; }

        [Display(Name = "อีเมล")]
        public string email { get; set; }

        [Display(Name = "วันที่สร้าง")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "สร้างโดย")]
        public string CreatedByUserName { get; set; }
        public Guid CreatedBy { get; set; }

        [Display(Name = "วันที่แก้ไข")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime EditedDate { get; set; }

        [Display(Name = "แก้ไขโดย")]
        public string EditedByUserName { get; set; }
        public Guid EditedBy { get; set; }

        [Display(Name = "เข้าใช้ล่าสุด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? LoggedInDate { get; set; }
        [Display(Name = "ออกจากระบบล่าสุด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? LoggedOutDate { get; set; }

        [Display(Name = "สถานะ")]
        public string Status { get; set; }
    }
}
