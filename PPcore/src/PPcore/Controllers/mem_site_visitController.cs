using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PPcore.Controllers
{
    public class mem_site_visitController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_site_visitController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_site_visit
        public IActionResult Index(string memberId, string v)
        {
            List<ViewModels.mem_site_visit.mem_site_visitViewModel> mem_site_visitViewModels = new List<ViewModels.mem_site_visit.mem_site_visitViewModel>();
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_site_visits = _context.mem_site_visit.Where(m => m.member_code == member.member_code).OrderBy(m => m.rec_no).ToList();
            foreach (var msv in mem_site_visits)
            {
                var mem_site_visitViewModel = new ViewModels.mem_site_visit.mem_site_visitViewModel();
                mem_site_visitViewModel.mem_site_visit = msv;
                mem_site_visitViewModel.country_desc = _context.ini_country.Single(i => i.country_code == msv.country_code).country_desc;
                mem_site_visitViewModels.Add(mem_site_visitViewModel);
            }
            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            ViewBag.country_code = new SelectList(_context.ini_country, "country_code", "country_desc");
            return View(mem_site_visitViewModels);
        }

        // GET: mem_site_visit/Details/5
        public IActionResult Details(string id)
        {
            if (id != null)
            {
                mem_site_visit msv = _context.mem_site_visit.Single(m => m.id == new Guid(id));
                if (msv != null)
                {
                    return Json(new { id = msv.id, rec_no = msv.rec_no, site_visit_desc = msv.site_visit_desc, country_code = msv.country_code });
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

        // POST: mem_site_visit/Create
        [HttpPost]
        public IActionResult Create(string memberId, string site_visit_desc, int country_code)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            try
            { 
                _context.Database.ExecuteSqlCommand("INSERT INTO mem_site_visit (rec_no,member_code,country_code,site_visit_desc,x_status) VALUES (0,'"+member.member_code+"','"+ country_code + "',N'"+ site_visit_desc + "','Y')");
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

        // GET: mem_site_visit/Edit/5
        public IActionResult Edit(string id)
        {
            return Details(id);
        }

        // POST: mem_site_visit/Edit
        [HttpPost]
        public IActionResult Edit(string memberId, string id, int rec_no, string site_visit_desc, int country_code)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_site_visit = _context.mem_site_visit.Single(m => m.id == new Guid(id));
            mem_site_visit.country_code = country_code;
            mem_site_visit.site_visit_desc = site_visit_desc;
            _context.Update(mem_site_visit);

            _context.SaveChanges();

            return Json(new { result = "success" });
        }
    }
}
