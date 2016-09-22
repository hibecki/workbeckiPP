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
    public class ini_districtController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public ini_districtController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: ini_district/SelectBy/5
        [HttpGet]
        public IActionResult SelectBy(string provinceCode)
        {
            if (provinceCode == null)
            {
                return NotFound();
            }
            
            var districts = _context.ini_district.Where(d => d.province_code == provinceCode).OrderBy(d => d.dist_desc).ToList();
            if (districts == null)
            {
                return NotFound();
            }

            var selectOptions = "";
            //if (!String.IsNullOrEmpty(selectOptions))
            //{
                foreach (var district in districts)
                {
                    selectOptions += "<option value='" + district.district_code + "'>" + district.dist_desc + "</option>";
                }
            //}

            return Content(selectOptions);
        }

        // GET: ini_district
        public async Task<IActionResult> Index()
        {
            return View(await _context.ini_district.ToListAsync());
        }

        // GET: ini_district/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_district = await _context.ini_district.SingleOrDefaultAsync(m => m.district_code == id);
            if (ini_district == null)
            {
                return NotFound();
            }

            return View(ini_district);
        }

        // GET: ini_district/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ini_district/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("district_code,province_code,country_code,area_part,dist_desc,id,rowversion,x_log,x_note,x_status")] ini_district ini_district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ini_district);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ini_district);
        }

        // GET: ini_district/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_district = await _context.ini_district.SingleOrDefaultAsync(m => m.district_code == id);
            if (ini_district == null)
            {
                return NotFound();
            }
            return View(ini_district);
        }

        // POST: ini_district/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("district_code,province_code,country_code,area_part,dist_desc,id,rowversion,x_log,x_note,x_status")] ini_district ini_district)
        {
            if (id != ini_district.district_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ini_district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ini_districtExists(ini_district.district_code))
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
            return View(ini_district);
        }

        // GET: ini_district/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_district = await _context.ini_district.SingleOrDefaultAsync(m => m.district_code == id);
            if (ini_district == null)
            {
                return NotFound();
            }

            return View(ini_district);
        }

        // POST: ini_district/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ini_district = await _context.ini_district.SingleOrDefaultAsync(m => m.district_code == id);
            _context.ini_district.Remove(ini_district);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ini_districtExists(string id)
        {
            return _context.ini_district.Any(e => e.district_code == id);
        }
    }
}
