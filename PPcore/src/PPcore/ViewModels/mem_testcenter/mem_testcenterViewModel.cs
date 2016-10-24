using System;
using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.mem_testcenter
{
    public class mem_testcenterViewModel
    {
        public Models.mem_testcenter mem_testcenter { get; set; }

        [Display(Name = "สร้างโดย")]
        public string CreatedByUserName { get; set; }

        [Display(Name = "วันที่สร้าง")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "สถานะ")]
        public string Status { get; set; }
    }
}
