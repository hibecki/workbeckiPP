using System.ComponentModel.DataAnnotations;

namespace PPcore.ViewModels.mem_train_record
{
    public class mem_train_recordViewModel
    {
        /**
        //from mem_train_record model
        [Display(Name = "รหัสหลักสูตรอบรม")]
        public string course_code { get; set; }
        public string member_code { get; set; }
        [Display(Name = "ระดับเกรด")]
        public string course_grade { get; set; }

        //from project_course model
        [Display(Name = "ชื่อ หลักสูตรอบรม")]
        public string course_desc { get; set; }
        **/

        public Models.mem_train_record mem_train_record { get; set; }
        public Models.project_course project_course { get; set; }
        [Display(Name = "ระดับเกรด")]
        public string cgrade_desc { get; set; }
    }
}
