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
    public class train_placeController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public train_placeController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: train_place
        public IActionResult Index()
        {
            ViewBag.countRecords = _context.train_place.Count();
            return View();
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var tp = _context.train_place.OrderBy(m => m.place_code);
            return View(tp.ToList());
        }

        // GET: train_place/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train_place = await _context.train_place.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (train_place == null)
            {
                return NotFound();
            }
            return View(train_place);
        }

        // GET: train_place/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("place_code,confirm_date,contactor,contactor_detail,id,place_desc,ref_doc,x_log,x_note,x_status")] train_place train_place)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train_place);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(train_place);
        }

        // GET: train_place/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train_place = await _context.train_place.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (train_place == null)
            {
                return NotFound();
            }
            return View(train_place);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("place_code,confirm_date,contactor,contactor_detail,id,place_desc,ref_doc,x_log,x_note,x_status")] train_place train_place)
        {
            if (new Guid(id) != train_place.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train_place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!train_placeExists(train_place.place_code))
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
            return View(train_place);
        }

        // GET: train_place/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train_place = await _context.train_place.SingleOrDefaultAsync(m => m.place_code == id);
            if (train_place == null)
            {
                return NotFound();
            }

            return View(train_place);
        }

        // POST: train_place/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var train_place = await _context.train_place.SingleOrDefaultAsync(m => m.place_code == id);
            _context.train_place.Remove(train_place);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool train_placeExists(string id)
        {
            return _context.train_place.Any(e => e.place_code == id);
        }
    }
}
