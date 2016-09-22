using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace PPcore.Controllers
{
    public class mem_worklistController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_worklistController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_worklist
        public IActionResult Index(string memberId, string v)
        {
            List<Models.mem_worklist> mem_worklists = new List<Models.mem_worklist>();
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            mem_worklists = _context.mem_worklist.Where(m => m.member_code == member.member_code).OrderBy(m => m.rec_no).ToList();

            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            return View(mem_worklists);
        }

        // GET: mem_worklist/Details/5
        public IActionResult Details(string id)
        {
            if (id != null)
            {
                mem_worklist mw = _context.mem_worklist.Single(m => m.id == new Guid(id));
                if (mw != null)
                {
                    return Json(new { id = mw.id, rec_no = mw.rec_no, company_name_th = mw.company_name_th, company_name_eng = mw.company_name_eng, position_name_th = mw.position_name_th, position_name_eng = mw.position_name_eng, work_year = mw.work_year, office_address = mw.office_address });
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

        // POST: mem_worklist/Create
        [HttpPost]
        public IActionResult Create(string memberId, string company_name_th, string company_name_eng, string position_name_th, string position_name_eng, string work_year, string office_address)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));

            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO mem_worklist (member_code,company_name_th,company_name_eng,position_name_th,position_name_eng,work_year,office_address,x_status) VALUES ('" + member.member_code + "',N'" + company_name_th + "',N'" + company_name_eng + "',N'" + position_name_th + "',N'" + position_name_eng + "',N'" + work_year + "',N'" + office_address + "','Y')");
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

        // GET: mem_worklist/Edit/5
        public IActionResult Edit(string id)
        {
            return Details(id);
        }

        // POST: mem_worklist/Edit/5
        [HttpPost]
        public IActionResult Edit(string memberId, string id, int rec_no, string company_name_th, string company_name_eng, string position_name_th, string position_name_eng, string work_year, string office_address)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_worklist = _context.mem_worklist.Single(m => m.id == new Guid(id));
            mem_worklist.member_code = member.member_code;
            //mem_worklist.rec_no = rec_no;
            mem_worklist.company_name_th = company_name_th;
            mem_worklist.company_name_eng = company_name_eng;
            mem_worklist.position_name_th = position_name_th;
            mem_worklist.position_name_eng = position_name_eng;
            mem_worklist.work_year = work_year;
            mem_worklist.office_address = office_address;
            mem_worklist.x_status = "Y";
            _context.Update(mem_worklist);

            _context.SaveChanges();

            return Json(new { result = "success" });
        }
    }
}
