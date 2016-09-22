using System;
using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.project_supporter
{
    public class project_sponsorViewModel
    {
        public Models.project_supporter project_supporter { get; set; }

        [Display(Name = "ชื่อผู้สนับสนุน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string spon_desc { get; set; }
        [Display(Name = "วันที่เริ่มสนับสนุน")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? confirm_date { get; set; }
    }
}
