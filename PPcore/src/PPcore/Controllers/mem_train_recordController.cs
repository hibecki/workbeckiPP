using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPcore.Controllers
{
    public class mem_train_recordController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_train_recordController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_train_record
        public IActionResult Index(string memberId, string v)
        {
            //List<ViewModels.mem_train_record.mem_train_recordViewModel> mem_train_recordViewModels = new List<ViewModels.mem_train_record.mem_train_recordViewModel>();
            //var member = _context.member.Single(m => m.id == new Guid(memberId));
            //var mem_train_records = _context.mem_train_record.Where(m => m.member_code == member.member_code).OrderBy(m => m.course_code).ToList();
            //foreach (var mtr in mem_train_records)
            //{
            //    var mem_train_recordViewModel = new ViewModels.mem_train_record.mem_train_recordViewModel();
            //    mem_train_recordViewModel.mem_train_record = mtr;
            //    mem_train_recordViewModel.project_course = _context.project_course.Single(p => p.course_code == mtr.course_code);
            //    mem_train_recordViewModel.cgrade_desc = _context.course_grade.Single(c => c.cgrade_code == mtr.course_grade).cgrade_desc;
            //    mem_train_recordViewModels.Add(mem_train_recordViewModel);
            //}
            //ViewBag.memberId = memberId;
            //if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            //ViewBag.course_grade = new SelectList(_context.course_grade, "cgrade_code", "cgrade_desc");
            //return View(mem_train_recordViewModels);

            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_train_records = _context.mem_train_record.Where(m => m.member_code == member.member_code).OrderBy(m => m.course_code).ToList();
            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            ViewBag.course_grade = new SelectList(_context.course_grade, "cgrade_code", "cgrade_desc");
            return View(mem_train_records);
        }

        // GET: mem_train_record/Details/5
        public IActionResult Details(string id)
        {
            if (id != null)
            {
                mem_train_record mtr = _context.mem_train_record.Single(m => m.id == new Guid(id));
                if (mtr != null)
                {
                    //project_course pc = _context.project_course.Single(p => p.course_code == mtr.course_code);
                    //return Json(new { id = mtr.id, course_code = mtr.course_code, course_desc = pc.course_desc, course_grade = mtr.course_grade });

                    return Json(new { id = mtr.id, course_code = mtr.course_code, course_desc = mtr.course_desc, course_grade = mtr.course_grade });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: mem_train_record/Create
        [HttpPost]
        public IActionResult Create(string memberId, string course_code, string course_desc, string course_grade)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));

            var mem_train_record = new mem_train_record();
            mem_train_record.member_code = member.member_code;
            mem_train_record.course_code = DateTime.Now.ToString("yyMMddhhmmssfffffff");
            mem_train_record.course_desc = course_desc;
            mem_train_record.course_grade = course_grade;
            mem_train_record.x_status = "Y";

            //Temp create project, this need to review business logic
            //var project_course = new project_course();
            //project_course.course_code = mem_train_record.course_code;
            //project_course.course_desc = course_desc;
            //project_course.x_status = "Y";

            _context.mem_train_record.Add(mem_train_record);
            //_context.project_course.Add(project_course);
            _context.SaveChanges();

            return Json(new { result = "success" });
            //return RedirectToAction("Index","mem_train_record",new {memberId = memberId});
        }

        // GET: mem_train_record/Edit/5
        public IActionResult Edit(string id)
        {
            return Details(id);
        }

        // POST: mem_train_record/Edit
        [HttpPost]
        public IActionResult Edit(string memberId, string id, string course_code, string course_desc, string course_grade)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_train_record = _context.mem_train_record.Single(m => m.id == new Guid(id));
            //mem_train_record.course_code = course_code;
            mem_train_record.course_desc = course_desc;
            mem_train_record.course_grade = course_grade;
            _context.Update(mem_train_record);

            //Temp create project, this need to review business logic
            //var project_course = _context.project_course.Single(p => p.course_code == mem_train_record.course_code);
            //project_course.course_desc = course_desc;
            //_context.project_course.Update(project_course);

            _context.SaveChanges();

            return Json(new { result = "success" });
        }
    }
}
