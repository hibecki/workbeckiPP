using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.course_instructor
{
    public class instructorViewModel
    {
        public Models.course_instructor course_instructor { get; set; }

        [Display(Name = "ชื่อวิทยากร")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string instructor_desc { get; set; }

        [Display(Name = "ตัวแทน/ผู้ติดต่อ")]
        public string contactor { get; set; }

        [Display(Name = "ข้อมูลในการติดต่อ")]
        public string contactor_detail { get; set; }
    }
}
