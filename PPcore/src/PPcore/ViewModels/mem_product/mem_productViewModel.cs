using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.ViewModels.mem_product
{
    public class mem_productViewModel
    {
        public Models.mem_product mem_product { get; set; }
        public Models.product product { get; set; }
        [Display(Name = "กลุ่มผลิตผล")]
        public string product_group_desc { get; set; }
        [Display(Name = "ประเภทผลิตผล")]
        public string product_type_desc { get; set; }
    }
}
