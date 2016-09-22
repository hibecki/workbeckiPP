using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.project_course_register
{
    public class memberViewModel
    {
        public Models.member member { get; set; }
        [Display(Name = "ผลการอบรม")]
        public int course_grade { get; set; }
    }
}
