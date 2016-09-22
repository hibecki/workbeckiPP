using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;

namespace PPcore.Controllers
{
    public class project_supporterController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public project_supporterController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult Index(string id, string v)
        {
            var pj = _context.project.SingleOrDefault(p => p.id == new Guid(id));
            ViewBag.projectId = pj.id;
            ViewBag.projectCode = pj.project_code;
            ViewBag.countRecords = _context.project_supporter.Where(ps => ps.project_code == pj.project_code).Count();
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true; } else { ViewBag.IsDetails = false; }
            return View(new project_supporter());
        }

        // GET: project_supporter/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_supporter = await _context.project_supporter.SingleOrDefaultAsync(m => m.spon_code == id);
            if (project_supporter == null)
            {
                return NotFound();
            }

            return View(project_supporter);
        }

        public IActionResult DetailsAsTable(string id, string code)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project_supporter = _context.project_supporter.Where(m => m.project_code == code).ToList();
            if (project_supporter == null)
            {
                return NotFound();
            }
            List<ViewModels.project_supporter.project_sponsorViewModel> v = new List<ViewModels.project_supporter.project_sponsorViewModel>();
            foreach (project_supporter p in project_supporter)
            {
                var s = _context.project_sponsor.SingleOrDefault(m => m.spon_code == p.spon_code);
                ViewModels.project_supporter.project_sponsorViewModel ps = new ViewModels.project_supporter.project_sponsorViewModel();
                ps.project_supporter = p;
                ps.spon_desc = s.spon_desc;
                ps.confirm_date = s.confirm_date;
                v.Add(ps);
            }
            return View(v);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string sponsorId, string support_budget, string projectCode)
        {
            var psp = _context.project_sponsor.SingleOrDefault(pjs => pjs.id == new Guid(sponsorId));
            project_supporter ps = new project_supporter();
            ps.project_code = projectCode;
            ps.spon_code = psp.spon_code;
            ps.support_budget = Int32.Parse(support_budget);
            ps.contactor = psp.contactor;
            ps.contactor_detail = psp.contactor_detail;
            ps.x_status = psp.x_status;
            
            try
            {
                _context.Add(ps);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { result = "fail" });
            }
            return Json(new { result = "success" });
        }

        public IActionResult EditAsTable()
        {
            var p = _context.project_sponsor.OrderBy(ps => ps.spon_code).ToList();
            if (p == null)
            {
                return NotFound();
            }
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("spon_code,project_code,contactor,contactor_detail,id,ref_doc,support_budget,x_log,x_note,x_status")] project_supporter project_supporter)
        {
            if (id != project_supporter.spon_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project_supporter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                        return NotFound();

                }
                return RedirectToAction("Index");
            }
            return View(project_supporter);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var project_supporter = await _context.project_supporter.SingleOrDefaultAsync(m => m.id == new Guid(id));
            _context.project_supporter.Remove(project_supporter);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }
    }
}
