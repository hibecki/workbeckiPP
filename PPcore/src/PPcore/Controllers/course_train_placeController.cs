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
    public class course_train_placeController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public course_train_placeController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult Index(string id, string v)
        {
            var c = _context.project_course.SingleOrDefault(m => m.id == new Guid(id));
            ViewBag.courseId = c.id;
            ViewBag.courseCode = c.course_code;
            ViewBag.countRecords = _context.course_train_place.Where(ci => ci.course_code == c.course_code).Count();
            if (!String.IsNullOrEmpty(v)) { ViewBag.IsDetails = true; } else { ViewBag.IsDetails = false; }
            return View(new course_train_place());
        }

        public IActionResult DetailsAsTable(string id, string code)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ci = _context.course_train_place.Where(m => m.course_code == code).ToList();
            if (ci == null)
            {
                return NotFound();
            }
            List<ViewModels.course_train_place.train_placeViewModel> v = new List<ViewModels.course_train_place.train_placeViewModel>();
            foreach (course_train_place c in ci)
            {
                ViewModels.course_train_place.train_placeViewModel t = new ViewModels.course_train_place.train_placeViewModel();
                var tp = _context.train_place.SingleOrDefault(tpp => tpp.place_code == c.place_code);
                t.course_train_place = c;
                t.place_desc = tp.place_desc;
                t.contactor = tp.contactor;
                t.contactor_detail = tp.contactor_detail;
                v.Add(t);
            }
            return View(v);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string placeId, string place_cost, string courseCode)
        {
            var i = _context.train_place.SingleOrDefault(m => m.id == new Guid(placeId));
            course_train_place ci = new course_train_place();
            ci.confirm_date = i.confirm_date;
            ci.course_code = courseCode;
            ci.place_code = i.place_code;
            ci.place_cost = Decimal.Parse(place_cost);
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
            var p = _context.train_place.Where(m => m.x_status != "N").OrderBy(m => m.place_code).ToList();
            if (p == null)
            {
                return NotFound();
            }
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var ci = await _context.course_train_place.SingleOrDefaultAsync(m => m.id == new Guid(id));
            _context.course_train_place.Remove(ci);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }
    }
}
