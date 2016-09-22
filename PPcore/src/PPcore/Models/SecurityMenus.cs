using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPcore.Models
{
    public partial class SecurityMenus
    {
        public int MenuId { get; set; }
        public int Level { get; set; }
        public int HaveChild { get; set; }
        public int IsRightAlign { get; set; }
        public string MenuName { get; set; }
        public string MenuDisplay { get; set; }
        public string MenuController { get; set; }
        public string MenuAction { get; set; }
        public string MenuUrl { get; set; }
    }
}
