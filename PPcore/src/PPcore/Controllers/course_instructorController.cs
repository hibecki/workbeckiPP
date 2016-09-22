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
    public class course_instructorController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public course_instructorController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult Index(string id, string v)
        {
            var c = _context.project_course.SingleOrDefault(m => m.id == new Guid(id));
            ViewBag.courseId = c.id;
            ViewBag.courseCode = c.course_code;
            ViewBag.countRecords = _context.course_instructor.Where(ci => ci.course_code == c.course_code).Count();
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true; } else { ViewBag.IsDetails = false; }
            return View(new course_instructor());
        }

        public IActionResult DetailsAsTable(string id, string code)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ci = _context.course_instructor.Where(m => m.course_code == code).ToList();
            if (ci == null)
            {
                return NotFound();
            }
            List<ViewModels.course_instructor.instructorViewModel> v = new List<ViewModels.course_instructor.instructorViewModel>();
            foreach (course_instructor c in ci)
            {
                ViewModels.course_instructor.instructorViewModel cv = new ViewModels.course_instructor.instructorViewModel();
                var i = _context.instructor.SingleOrDefault(m => m.instructor_code == c.instructor_code);
                cv.course_instructor = c;
                cv.instructor_desc = i.instructor_desc;
                cv.contactor = i.contactor;
                cv.contactor_detail = i.contactor_detail;
                v.Add(cv);
            }

            return View(v);
        }

        // GET: course_instructor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course_instructor = await _context.course_instructor.SingleOrDefaultAsync(m => m.instructor_code == id);
            if (course_instructor == null)
            {
                return NotFound();
            }

            return View(course_instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string instructorId, string instructor_cost, string courseCode)
        {
            var i = _context.instructor.SingleOrDefault(m => m.id == new Guid(instructorId));
            course_instructor ci = new course_instructor();
            ci.confirm_date = i.confirm_date;
            ci.course_code = courseCode;
            ci.instructor_code = i.instructor_code;
            ci.instructor_cost = Decimal.Parse(instructor_cost);
            ci.ref_doc = i.ref_doc;
            ci.x_status = i.x_status;
            try
            {
                _context.Add(ci);
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
            var p = _context.instructor.Where(m => m.x_status != "N").OrderBy(m => m.instructor_code).ToList();
            if (p == null)
            {
                return NotFound();
            }
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var ci = await _context.course_instructor.SingleOrDefaultAsync(m => m.id == new Guid(id));
            _context.course_instructor.Remove(ci);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }
    }
}
