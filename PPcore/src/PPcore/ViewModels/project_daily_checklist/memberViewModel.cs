using System.ComponentModel.DataAnnotations;


namespace PPcore.ViewModels.project_daily_checklist
{
    public class memberViewModel
    {
        public Models.member member { get; set; }
        [Display(Name = "เช็คชื่อเข้าอบรม")]
        public string attended { get; set; }
    }
}
