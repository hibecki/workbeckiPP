using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class SecurityMemberRoles
    {
        public Guid MemberId { get; set; }

        [Display(Name = "สร้างโดย")]
        public Guid CreatedBy { get; set; }

        [Display(Name = "วันที่สร้าง")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "แก้ไขโดย")]
        public Guid EditedBy { get; set; }

        [Display(Name = "วันที่แก้ไข")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime EditedDate { get; set; }

        [Display(Name = "วันที่เข้าใช้ล่าสุด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? LoggedInDate { get; set; }
        [Display(Name = "วันที่ออกระบบล่าสุด")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime? LoggedOutDate { get; set; }

        public string x_status { get; set; }
        public string x_note { get; set; }
        public string x_log { get; set; }
    }
}
