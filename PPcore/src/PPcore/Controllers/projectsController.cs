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
    public class projectsController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public projectsController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        public IActionResult Index()
        {
            ViewBag.countRecords = _context.project.Count();
            return View();
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var ps = _context.project.OrderBy(m => m.project_code).ToList();
            foreach (project p in ps)
            {
                int countJoin = 0; int countPassed = 0;
                var pcs = _context.project_course.Where(pss => pss.project_code == p.project_code).ToList();
                foreach (project_course pc in pcs)
                {
                    countJoin += _context.project_course_register.Where(pcr => pcr.course_code == pc.course_code).Count();
                    countPassed += _context.project_course_register.Where(pcrr => (pcrr.course_code == pc.course_code) && (pcrr.course_grade >= pc.passed_score)).Count();
                }
                p.active_member_join = countJoin;
                p.passed_member = countPassed;
            }
            return View(ps);
        }

        [HttpGet]
        public IActionResult DetailsAsTableBySponsor(string spon_code)
        {
            var pss = _context.project_supporter.Where(mps => mps.spon_code == spon_code).ToList();
            List<ViewModels.project.project_supporterViewModel> pjs = new List<ViewModels.project.project_supporterViewModel>();
            foreach (project_supporter ps in pss)
            {
                var p = _context.project.SingleOrDefault(m => m.project_code == ps.project_code);
                ViewModels.project.project_supporterViewModel v = new ViewModels.project.project_supporterViewModel();
                v.project = p;
                v.support_budget = ps.support_budget;
                pjs.Add(v);
            }
            return View("DetailsAsTableBySponsor", pjs.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("project_code,active_member_join,budget,id,passed_member,project_approve_date,project_date,project_desc,project_manager,ref_doc,target_member_join,x_log,x_note,x_status")] project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(string id, string v)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.project.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (project == null)
            {
                return NotFound();
            }
            int countJoin = 0; int countPassed = 0;
            var ps = _context.project_course.Where(pss => pss.project_code == project.project_code).ToList();
            foreach (project_course p in ps)
            {
                countJoin += _context.project_course_register.Where(pcr => pcr.course_code == p.course_code).Count();
                countPassed += _context.project_course_register.Where(pcrr => (pcrr.course_code == p.course_code) && (pcrr.course_grade >= p.passed_score)).Count();
            }
            ViewBag.active_member_join = countJoin;
            ViewBag.passed_member = countPassed;
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true;  } else { ViewBag.IsDetails = false; }
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("project_code,active_member_join,budget,id,passed_member,project_approve_date,project_date,project_desc,project_manager,ref_doc,target_member_join,x_log,x_note,x_status")] project project)
        {
            if (new Guid(id) != project.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!projectExists(project.project_code))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: projects/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.project.SingleOrDefaultAsync(m => m.project_code == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var project = await _context.project.SingleOrDefaultAsync(m => m.project_code == id);
            _context.project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool projectExists(string id)
        {
            return _context.project.Any(e => e.project_code == id);
        }
    }
}
