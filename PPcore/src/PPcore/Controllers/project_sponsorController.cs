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
    public class project_sponsorController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public project_sponsorController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        public IActionResult Index()
        {
            ViewBag.countRecords = _context.project_sponsor.Count();
            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_sponsor = await _context.project_sponsor.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (project_sponsor == null)
            {
                return NotFound();
            }
            return View(project_sponsor);
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var ps = _context.project_sponsor.OrderBy(m => m.spon_code);
            return View(ps.ToList());
        }

        // GET: project_sponsor/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("spon_code,confirm_date,contactor,contactor_detail,id,ref_doc,spon_desc,x_log,x_note,x_status")] project_sponsor project_sponsor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project_sponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project_sponsor);
        }

        // GET: project_sponsor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_sponsor = await _context.project_sponsor.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (project_sponsor == null)
            {
                return NotFound();
            }
            var sum_budget = _context.project_supporter.Where(s => s.spon_code == project_sponsor.spon_code).Sum(s => s.support_budget);
            ViewBag.sum_budget = String.Format("{0:C0}", sum_budget);
            return View(project_sponsor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("spon_code,confirm_date,contactor,contactor_detail,id,ref_doc,spon_desc,x_log,x_note,x_status")] project_sponsor project_sponsor)
        {
            if (new Guid(id) != project_sponsor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project_sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!project_sponsorExists(project_sponsor.spon_code))
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
            return View(project_sponsor);
        }

        // GET: project_sponsor/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project_sponsor = await _context.project_sponsor.SingleOrDefaultAsync(m => m.spon_code == id);
            if (project_sponsor == null)
            {
                return NotFound();
            }

            return View(project_sponsor);
        }

        // POST: project_sponsor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var project_sponsor = await _context.project_sponsor.SingleOrDefaultAsync(m => m.spon_code == id);
            _context.project_sponsor.Remove(project_sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool project_sponsorExists(string id)
        {
            return _context.project_sponsor.Any(e => e.spon_code == id);
        }
    }
}
