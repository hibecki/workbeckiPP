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
    public class SecurityMemberRolesController : Controller
    {
        private readonly SecurityDBContext _scontext;
        private readonly PalangPanyaDBContext _context;

        public SecurityMemberRolesController(SecurityDBContext scontext, PalangPanyaDBContext context)
        {
            _scontext = scontext;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Suspend(string id, string status)
        {
            var mr = _scontext.SecurityMemberRoles.SingleOrDefault(mrr => mrr.MemberId == new Guid(id));
            if (mr != null)
            {
                mr.x_status = status;
                mr.EditedDate = DateTime.Now;
                mr.EditedBy = new Guid(HttpContext.Session.GetString("memberId"));
                _scontext.Update(mr);
                await _scontext.SaveChangesAsync();
                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }
    }
}
