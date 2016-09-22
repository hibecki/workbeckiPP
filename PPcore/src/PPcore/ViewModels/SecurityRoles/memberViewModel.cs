using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.ViewModels.SecurityRoles
{
    public class memberViewModel
    {
        public Models.SecurityRoles SecurityRoles { get; set; }
        public int memberCount { get; set; }
        public string panelColorCSS { get; set; }
    }
}
