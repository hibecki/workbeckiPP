using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using Microsoft.AspNetCore.Http;

namespace PPcore.Controllers
{
    public class mem_testcenterController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        private void prepareViewBag()
        {
            ViewBag.x_status = new SelectList(new[] { new { Value = "Y", Text = "ใช้งาน" }, new { Value = "N", Text = "ยกเลิก" } }, "Value", "Text", "Y");
        }

        public mem_testcenterController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        public async Task<IActionResult> Index()
        {
            prepareViewBag();
            return View(await _context.mem_testcenter.ToListAsync());
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var mtc = _context.mem_testcenter.OrderBy(mtcc => mtcc.mem_testcenter_desc).ToList();

            List<PPcore.ViewModels.mem_testcenter.mem_testcenterViewModel> mtcvs = new List<PPcore.ViewModels.mem_testcenter.mem_testcenterViewModel>();
            foreach (mem_testcenter m in mtc)
            {
                PPcore.ViewModels.mem_testcenter.mem_testcenterViewModel mtcv = new PPcore.ViewModels.mem_testcenter.mem_testcenterViewModel();
                mtcv.mem_testcenter = m;
                var mc = _context.member.Where(mcc => mcc.id == m.CreatedBy).Select(mcc => new { mcc.mem_username }).FirstOrDefault();
                if (mc != null)
                {
                    mtcv.CreatedByUserName = mc.mem_username;
                }
                else { mtcv.CreatedByUserName = ""; }
                mtcv.CreatedDate = m.CreatedDate;
                mtcvs.Add(mtcv);
            }
            return View(mtcvs);
        }

        // GET: mem_testcenter/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("id,mem_testcenter_desc,rowversion,x_status")] mem_testcenter mem_testcenter)
        {
            if (ModelState.IsValid)
            {
                mem_testcenter.mem_testcenter_desc = mem_testcenter.mem_testcenter_desc.Trim();
                mem_testcenter.id = Guid.NewGuid();
                mem_testcenter.CreatedBy = new Guid(HttpContext.Session.GetString("memberId"));
                mem_testcenter.CreatedDate = DateTime.Now;
                mem_testcenter.x_status = mem_testcenter.x_status.Trim();
                mem_testcenter.mem_testcenter_code = DateTime.Now.ToString("yyMMddhhmmss");

                _context.Add(mem_testcenter);
                await _context.SaveChangesAsync();
            }
            return Json(new { result = "success" });
        }

        // GET: mem_testcenter/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mem_testcenter = await _context.mem_testcenter.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (mem_testcenter == null)
            {
                return NotFound();
            }
            return View(mem_testcenter);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("id,mem_testcenter_code,mem_testcenter_desc,rowversion,x_status,CreatedBy,CreatedDate")] mem_testcenter mem_testcenter)
        {
            if (ModelState.IsValid)
            {
                mem_testcenter.mem_testcenter_desc = mem_testcenter.mem_testcenter_desc.Trim();
                mem_testcenter.x_status = mem_testcenter.x_status.Trim();

                _context.Update(mem_testcenter);
                await _context.SaveChangesAsync();
            }
            return Json(new { result = "success" });
        }
    }
}
