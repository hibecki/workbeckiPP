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
    public class ini_configController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public ini_configController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: ini_config
        public async Task<IActionResult> Index()
        {
            return View(await _context.ini_config.ToListAsync());
        }

        // GET: ini_config/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_config = await _context.ini_config.SingleOrDefaultAsync(m => m.client_code == id);
            if (ini_config == null)
            {
                return NotFound();
            }

            return View(ini_config);
        }

        // GET: ini_config/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ini_config/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("client_code,system,module,cnfig_item,id,rowversion,text_value,x_log,x_note,x_status")] ini_config ini_config)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ini_config);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ini_config);
        }

        // GET: ini_config/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_config = await _context.ini_config.SingleOrDefaultAsync(m => m.client_code == id);
            if (ini_config == null)
            {
                return NotFound();
            }
            return View(ini_config);
        }

        // POST: ini_config/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("client_code,system,module,cnfig_item,id,rowversion,text_value,x_log,x_note,x_status")] ini_config ini_config)
        {
            if (id != ini_config.client_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ini_config);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ini_configExists(ini_config.client_code))
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
            return View(ini_config);
        }

        // GET: ini_config/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_config = await _context.ini_config.SingleOrDefaultAsync(m => m.client_code == id);
            if (ini_config == null)
            {
                return NotFound();
            }

            return View(ini_config);
        }

        // POST: ini_config/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ini_config = await _context.ini_config.SingleOrDefaultAsync(m => m.client_code == id);
            _context.ini_config.Remove(ini_config);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ini_configExists(string id)
        {
            return _context.ini_config.Any(e => e.client_code == id);
        }
    }
}
