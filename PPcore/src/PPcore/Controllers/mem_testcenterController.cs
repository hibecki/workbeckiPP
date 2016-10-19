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
    public class mem_testcenterController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public mem_testcenterController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_testcenter
        public async Task<IActionResult> Index()
        {
            return View(await _context.mem_testcenter.ToListAsync());
        }

        // GET: mem_testcenter/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mem_testcenter = await _context.mem_testcenter.SingleOrDefaultAsync(m => m.mem_testcenter_code == id);
            if (mem_testcenter == null)
            {
                return NotFound();
            }

            return View(mem_testcenter);
        }

        // GET: mem_testcenter/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: mem_testcenter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("mem_testcenter_code,CreatedBy,CreatedDate,id,mem_testcenter_desc,rowversion,x_log,x_note,x_status")] mem_testcenter mem_testcenter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mem_testcenter);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mem_testcenter);
        }

        // GET: mem_testcenter/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mem_testcenter = await _context.mem_testcenter.SingleOrDefaultAsync(m => m.mem_testcenter_code == id);
            if (mem_testcenter == null)
            {
                return NotFound();
            }
            return View(mem_testcenter);
        }

        // POST: mem_testcenter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("mem_testcenter_code,CreatedBy,CreatedDate,id,mem_testcenter_desc,rowversion,x_log,x_note,x_status")] mem_testcenter mem_testcenter)
        {
            if (id != mem_testcenter.mem_testcenter_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mem_testcenter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mem_testcenterExists(mem_testcenter.mem_testcenter_code))
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
            return View(mem_testcenter);
        }

        // GET: mem_testcenter/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mem_testcenter = await _context.mem_testcenter.SingleOrDefaultAsync(m => m.mem_testcenter_code == id);
            if (mem_testcenter == null)
            {
                return NotFound();
            }

            return View(mem_testcenter);
        }

        // POST: mem_testcenter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mem_testcenter = await _context.mem_testcenter.SingleOrDefaultAsync(m => m.mem_testcenter_code == id);
            _context.mem_testcenter.Remove(mem_testcenter);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool mem_testcenterExists(string id)
        {
            return _context.mem_testcenter.Any(e => e.mem_testcenter_code == id);
        }
    }
}
