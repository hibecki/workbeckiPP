using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.project_course_register
{
    public class memberRegisterViewModel
    {
        public Models.project_course project_course { get; set; }
        [Display(Name = "ผลการอบรม")]
        public bool IsRegistered { get; set; }
    }
}
