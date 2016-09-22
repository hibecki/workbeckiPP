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
    public class ini_list_zipController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public ini_list_zipController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult SelectBy(string subdistrictCode)
        {
            if (subdistrictCode == null)
            {
                return NotFound();
            }
            var selectOptions = "";
            var zips = _context.ini_list_zip.Where(d => d.subdistrict_code == subdistrictCode).OrderBy(d => d.zip_code).ToList();
            if (zips == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var zip in zips)
                {
                    selectOptions = zip.zip_code;
                }
            }
            /**
            foreach (var zip in zips)
            {
                selectOptions += "<option value='" + zip.zip_code + "'>" + zip.zip_code + "</option>";
            }
            **/
            return Content(selectOptions);
        }

        // GET: ini_list_zip
        public async Task<IActionResult> Index()
        {
            return View(await _context.ini_list_zip.ToListAsync());
        }

        // GET: ini_list_zip/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_list_zip = await _context.ini_list_zip.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_list_zip == null)
            {
                return NotFound();
            }

            return View(ini_list_zip);
        }

        // GET: ini_list_zip/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ini_list_zip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("province_code,country_code,district_code,subdistrict_code,zip_code,id,rowversion,x_log,x_note,x_status")] ini_list_zip ini_list_zip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ini_list_zip);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ini_list_zip);
        }

        // GET: ini_list_zip/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_list_zip = await _context.ini_list_zip.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_list_zip == null)
            {
                return NotFound();
            }
            return View(ini_list_zip);
        }

        // POST: ini_list_zip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("province_code,country_code,district_code,subdistrict_code,zip_code,id,rowversion,x_log,x_note,x_status")] ini_list_zip ini_list_zip)
        {
            if (id != ini_list_zip.province_code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ini_list_zip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ini_list_zipExists(ini_list_zip.province_code))
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
            return View(ini_list_zip);
        }

        // GET: ini_list_zip/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ini_list_zip = await _context.ini_list_zip.SingleOrDefaultAsync(m => m.province_code == id);
            if (ini_list_zip == null)
            {
                return NotFound();
            }

            return View(ini_list_zip);
        }

        // POST: ini_list_zip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ini_list_zip = await _context.ini_list_zip.SingleOrDefaultAsync(m => m.province_code == id);
            _context.ini_list_zip.Remove(ini_list_zip);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ini_list_zipExists(string id)
        {
            return _context.ini_list_zip.Any(e => e.province_code == id);
        }
    }
}
