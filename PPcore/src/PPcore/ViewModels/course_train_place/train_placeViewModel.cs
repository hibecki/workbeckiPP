using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.course_train_place
{
    public class train_placeViewModel
    {
        public Models.course_train_place course_train_place { get; set; }

        [Display(Name = "ชื่อสถานที่จัดอบรม")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string place_desc { get; set; }

        [Display(Name = "ตัวแทน/ผู้ติดต่อ")]
        public string contactor { get; set; }
        [Display(Name = "ข้อมูลในการติดต่อ")]
        public string contactor_detail { get; set; }
    }
}
