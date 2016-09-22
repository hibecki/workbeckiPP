using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPcore.Controllers
{
    public class mem_educationController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_educationController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_education
        public IActionResult Index(string memberId, string v)
        {
            List<Models.mem_education> mem_educations = new List<Models.mem_education>();
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            mem_educations = _context.mem_education.Where(m => m.member_code == member.member_code).OrderBy(m => m.rec_no).ToList();
            ViewBag.mem_education_degree = new SelectList(new[] {
                new { Value = "340", Text = "ปริญญาเอก" },
                new { Value = "330", Text = "ปริญญาโท" },
                new { Value = "320", Text = "ปริญญาตรี" },
                new { Value = "310", Text = "อนุปริญญา" },
                new { Value = "240", Text = "ปวส" },
                new { Value = "230", Text = "ปวช"},
                new { Value = "220", Text = "มัธยมศึกษาตอนปลาย"},
                new { Value = "210", Text = "มัธยมศึกษาตอนต้น"},
                new { Value = "101", Text = "ประถมศึกษา"}
            }, "Value", "Text", "101");
            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            return View(mem_educations);
        }

        // GET: mem_education/Details/5
        public IActionResult Details(string id)
        {
            if (id != null)
            {
                mem_education me = _context.mem_education.Single(m => m.id == new Guid(id));
                if (me != null)
                {
                    return Json(new { id = me.id, rec_no = me.rec_no, colledge_name = me.colledge_name, degree = me.degree, faculty = me.faculty});
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

        // POST: mem_education/Create
        [HttpPost]
        public IActionResult Create(string memberId, string colledge_name, string degree, string faculty)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));

            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO mem_education (rec_no,member_code,degree,colledge_name,faculty,x_status) VALUES (0,'" + member.member_code + "',N'" + degree + "',N'" + colledge_name + "',N'" + faculty + "','Y')");
            }
            catch (SqlException ex)
            {
                var errno = ex.Number; var msg = "";
                if (errno == 2627) //Violation of primary key. Handle Exception
                {
                    msg = "duplicate";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }
            catch (Exception ex)
            {
                var errno = ex.HResult; var msg = "";
                if (ex.InnerException.Message.IndexOf("PRIMARY KEY") != -1)
                {
                    msg = "duplicate";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }

            return Json(new { result = "success" });
        }

        // GET: mem_education/Edit/5
        public IActionResult Edit(string id)
        {
            return Details(id);
        }

        // POST: mem_education/Edit/5
        [HttpPost]
        public IActionResult Edit(string memberId, string id, int rec_no, string colledge_name, string degree, string faculty)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_education = _context.mem_education.Single(m => m.id == new Guid(id));
            mem_education.colledge_name = colledge_name;
            mem_education.degree = degree;
            mem_education.faculty = faculty;
            _context.Update(mem_education);

            _context.SaveChanges();

            return Json(new { result = "success" });
        }
    }
}
