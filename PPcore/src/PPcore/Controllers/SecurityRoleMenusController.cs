using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using Microsoft.AspNetCore.Http;

namespace PPcore.Controllers
{
    public class SecurityRoleMenusController : Controller
    {
        private readonly SecurityDBContext _scontext;

        public SecurityRoleMenusController(SecurityDBContext scontext)
        {
            _scontext = scontext;    
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoleMenus(string roleId, string menus)
        {
            Guid rId = new Guid(roleId);
            var rms = _scontext.SecurityRoleMenus.Where(rr => rr.RoleId == rId).ToList();
            _scontext.RemoveRange(rms);
            await _scontext.SaveChangesAsync();


            var menuar = menus.Split('|');
            foreach (string menuId in menuar)
            {
                SecurityRoleMenus r = new SecurityRoleMenus();
                r.RoleId = rId;
                r.MenuId = int.Parse(menuId);
                r.EditedBy = new Guid(HttpContext.Session.GetString("memberId"));
                r.EditedDate = DateTime.Now;
                _scontext.Add(r);
            }
            await _scontext.SaveChangesAsync();

            return Json(new { result = "success" });
        }

    }
}
