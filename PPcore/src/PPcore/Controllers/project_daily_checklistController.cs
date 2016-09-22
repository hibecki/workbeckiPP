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
    public class project_daily_checklistController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public project_daily_checklistController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        public IActionResult Index()
        {
            ViewBag.countRecords = _context.project_course.Where(m => m.x_status != "N").Count();
            return View();
        }

        [HttpGet]
        public IActionResult DetailsAsTableCourse()
        {
            var p = _context.project_course.Where(m => m.x_status != "N").OrderBy(m => m.course_code);
            return View(p.ToList());
        }

        public async Task<IActionResult> DetailsCourse(string id)
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
            ViewBag.ctype_code = project_course.ctype_code;
            if ((project_course.course_end == null) || (project_course.course_end <= project_course.course_begin))
            {
                ViewBag.course_day = new SelectList(new[] { new { Value = "1", Text = "1" } }, "Value", "Text", "1");
            }
            else
            {
                TimeSpan ts = (DateTime)project_course.course_end - (DateTime)project_course.course_begin;
                List<SelectListItem> items = new List<SelectListItem>();
                for (int i = 1; i <= (ts.Days + 1);i++)
                {
                    items.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
                ViewBag.course_day = new SelectList(items, "Value", "Text", "1");
            }
            ViewBag.cgroup_code = new SelectList(_context.course_group.Where(cg => cg.x_status != "N").OrderBy(cg => cg.cgroup_code), "cgroup_code", "cgroup_desc", 1);
            ViewBag.active_member_join = _context.project_course_register.Where(pcr => pcr.course_code == project_course.course_code).Count();
            ViewBag.passed_member = _context.project_course_register.Where(p => (p.course_code == project_course.course_code) && (p.course_grade >= project_course.passed_score)).Count();
            return View(project_course);
        }

        [HttpGet]
        public IActionResult DetailsAsTableMember(string course_code, int course_day)
        {
            var ps = _context.project_course_register.Where(pp => pp.course_code == course_code).OrderBy(pp => pp.member_code).ToList();
            var c = _context.project_course.SingleOrDefault(cc => cc.course_code == course_code);
            var cdate = ((DateTime)c.course_begin).AddDays(course_day - 1);
            List<ViewModels.project_daily_checklist.memberViewModel> pms = new List<ViewModels.project_daily_checklist.memberViewModel>();

            foreach (project_course_register p in ps)
            {
                var m = _context.member.SingleOrDefault(mm => mm.member_code == p.member_code);
                var pc = _context.project_daily_checklist.SingleOrDefault(pcc => (pcc.member_code == m.member_code) && (pcc.course_code == course_code) && (pcc.course_date == cdate));
                ViewModels.project_daily_checklist.memberViewModel pd = new ViewModels.project_daily_checklist.memberViewModel();
                pd.member = m;
                if (pc == null) { pd.attended = "N"; } else { pd.attended = "Y"; }
                pms.Add(pd);
            }
            return View(pms);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string action, string member_code, string course_code, DateTime course_begin, int course_day)
        {
            var cdate = course_begin.AddDays(course_day-1);

            try
            {

                if (action.Equals("add"))
                {
                    var c = new project_daily_checklist();
                    c.member_code = member_code;
                    c.course_code = course_code;
                    c.course_date = cdate;
                    c.x_status = "Y";
                    _context.Add(c);
                }
                else if (action.Equals("del"))
                {
                    var c = _context.project_daily_checklist.SingleOrDefault(d => (d.member_code == member_code) && (d.course_code == course_code) && (d.course_date == cdate));
                    _context.Remove(c);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { result = "fail" });
            }
            return Json(new { result = "success" });
        }
    }
}
