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
    public class ini_subdistrictController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public ini_subdistrictController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult SelectBy(string districtCode)
        {
            if (districtCode == null)
            {
                return NotFound();
            }
            var selectOptions = "";
            var subdistricts = _context.ini_subdistrict.Where(d => d.district_code == districtCode).OrderBy(d => d.dist_desc).ToList();
            if (subdistricts == null)
            {
                return NotFound();
            }

            foreach (var subdistrict in subdistricts)
            {
                selectOptions += "<option value='" + subdistrict.subdistrict_code + "'>" + subdistrict.dist_desc + "</option>";
            }

            return Content(selectOptions);
        }

        // GET: ini_subdistrict
        public async Task<IActionResult> Index()
        {
            return View(await _context.ini_subdistrict.ToListAsync());
        }

        // GET: ini_subdistrict/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_subdistrict = await _context.ini_subdistrict.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_subdistrict == null)
            {
                return NotFound();
            }

            return View(ini_subdistrict);
        }

        // GET: ini_subdistrict/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ini_subdistrict/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("province_code,country_code,district_code,subdistrict_code,area_part,dist_desc,id,rowversion,x_log,x_note,x_status")] ini_subdistrict ini_subdistrict)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ini_subdistrict);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ini_subdistrict);
        }

        // GET: ini_subdistrict/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_subdistrict = await _context.ini_subdistrict.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_subdistrict == null)
            {
                return NotFound();
            }
            return View(ini_subdistrict);
        }

        // POST: ini_subdistrict/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("province_code,country_code,district_code,subdistrict_code,area_part,dist_desc,id,rowversion,x_log,x_note,x_status")] ini_subdistrict ini_subdistrict)
        {
            if (id != ini_subdistrict.province_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ini_subdistrict);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ini_subdistrictExists(ini_subdistrict.province_code))
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
            return View(ini_subdistrict);
        }

        // GET: ini_subdistrict/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_subdistrict = await _context.ini_subdistrict.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_subdistrict == null)
            {
                return NotFound();
            }

            return View(ini_subdistrict);
        }

        // POST: ini_subdistrict/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ini_subdistrict = await _context.ini_subdistrict.SingleOrDefaultAsync(m => m.province_code == id);
            _context.ini_subdistrict.Remove(ini_subdistrict);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ini_subdistrictExists(string id)
        {
            return _context.ini_subdistrict.Any(e => e.province_code == id);
        }
    }
}
