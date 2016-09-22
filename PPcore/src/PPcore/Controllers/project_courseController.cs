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
    public class project_courseController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public project_courseController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult Index(string id, string v)
        {
            var pj = _context.project.SingleOrDefault(p => p.id == new Guid(id));
            ViewBag.projectId = pj.id;
            ViewBag.projectCode = pj.project_code;
            ViewBag.countRecords = _context.project_course.Where(pc => pc.project_code == pj.project_code).Count();
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true; } else { ViewBag.IsDetails = false; }
            return View(new project_course());
        }

        public IActionResult DetailsAsTable(string id, string code)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pcs = _context.project_course.Where(m => m.project_code == code).OrderBy(m => m.course_code).ToList();
            if (pcs == null)
            {
                return NotFound();
            }

            foreach (project_course pc in pcs)
            {
                pc.passed_member = _context.project_course_register.Where(pcr => (pcr.course_code == pc.course_code) && (pcr.course_grade >= pc.passed_score)).Count();
            }
            return View(pcs);
        }

        // GET: project_course/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_course = await _context.project_course.SingleOrDefaultAsync(m => m.course_code == id);
            if (project_course == null)
            {
                return NotFound();
            }

            return View(project_course);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string courseId, string projectCode)
        {
            var c = _context.project_course.SingleOrDefault(m => m.id == new Guid(courseId));
            //project_course pc = new project_course();
            //pc.project_code = projectCode;
            //pc.active_member_join = c.active_member_join;
            //pc.budget = c.budget;
            //pc.cgroup_code = c.cgroup_code;
            //pc.charge_head = c.charge_head;
            //pc.course_approve_date = c.course_approve_date;
            //pc.course_begin = c.course_begin;
            //pc.course_code = c.course_code;
            //pc.course_date = c.course_date;
            //pc.course_desc = c.course_desc;
            //pc.course_end = c.course_end;
            //pc.ctype_code = c.ctype_code;
            //pc.passed_member = c.passed_member;
            //pc.project_manager = c.project_manager;
            //pc.ref_doc = c.ref_doc;
            //pc.support_head = c.support_head;
            //pc.target_member_join = c.target_member_join;
            //pc.x_status = c.x_status;
            c.project_code = projectCode;
            try
            {
                //_context.Add(pc);
                _context.Update(c);
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
            var p = _context.project_course.Where(m => m.project_code == null).OrderBy(m => m.course_code).ToList();
            if (p == null)
            {
                return NotFound();
            }
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("course_code,active_member_join,budget,cgroup_code,charge_head,course_approve_date,course_begin,course_date,course_desc,course_end,ctype_code,id,passed_member,project_code,project_manager,ref_doc,support_head,target_member_join,x_log,x_note,x_status")] project_course project_course)
        {
            if (new Guid(id) != project_course.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project_course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction("Index");
            }
            return View(project_course);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var pc = await _context.project_course.SingleOrDefaultAsync(m => m.id == new Guid(id));
            _context.project_course.Remove(pc);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }

        // ------ Courses ----------

        private void prepareViewBag()
        {
            ViewBag.cgroup_code = new SelectList(_context.course_group.Where(cg => cg.x_status != "N").OrderBy(cg => cg.cgroup_code), "cgroup_code", "cgroup_desc", 1);
        }

        public IActionResult CourseIndex()
        {
            ViewBag.countRecords = _context.project_course.Count();
            return View();
        }

        [HttpGet]
        public IActionResult CourseDetailsAsTable()
        {
            var pcs = _context.project_course.OrderBy(m => m.course_code).ToList();
            foreach (project_course pc in pcs)
            {
                pc.passed_member = _context.project_course_register.Where(pcr => (pcr.course_code == pc.course_code) && (pcr.course_grade >= pc.passed_score)).Count();
            }
            return View(pcs);
        }

        public IActionResult CourseCreate()
        {
            prepareViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CourseCreate([Bind("course_code,active_member_join,budget,cgroup_code,charge_head,course_approve_date,course_begin,course_date,course_desc,course_end,ctype_code,id,passed_member,project_code,project_manager,ref_doc,support_head,target_member_join,x_log,x_note,x_status")] project_course project_course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project_course);
                await _context.SaveChangesAsync();
                return RedirectToAction("CourseIndex");
            }
            return View(project_course);
        }

        public async Task<IActionResult> CourseEdit(string id, string v)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_course = await _context.project_course.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (project_course == null)
            {
                return NotFound();
            }
            prepareViewBag();
            ViewBag.ctype_code = project_course.ctype_code;
            ViewBag.active_member_join = _context.project_course_register.Where(pcr => pcr.course_code == project_course.course_code).Count();
            ViewBag.passed_member = _context.project_course_register.Where(p => (p.course_code == project_course.course_code) && (p.course_grade >= project_course.passed_score)).Count();
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true; } else { ViewBag.IsDetails = false; }
            return View(project_course);
        }

        [HttpPost]
        public async Task<IActionResult> CourseEdit(string id, [Bind("passed_score,course_code,active_member_join,budget,cgroup_code,charge_head,course_approve_date,course_begin,course_date,course_desc,course_end,ctype_code,id,passed_member,project_code,project_manager,ref_doc,support_head,target_member_join,x_log,x_note,x_status")] project_course project_course)
        {
            if (new Guid(id) != project_course.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project_course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction("CourseIndex");
            }
            return View(project_course);
        }
    }
}
