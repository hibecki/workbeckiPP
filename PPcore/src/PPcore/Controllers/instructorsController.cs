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
    public class instructorsController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public instructorsController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        public IActionResult Index()
        {
            ViewBag.countRecords = _context.instructor.Count();
            return View();
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var i = _context.instructor.OrderBy(m => m.instructor_code);
            return View(i.ToList());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructor.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("instructor_code,confirm_date,contactor,contactor_detail,id,instructor_desc,ref_doc,x_log,x_note,x_status")] instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: instructors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructor.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("instructor_code,confirm_date,contactor,contactor_detail,id,instructor_desc,ref_doc,x_log,x_note,x_status")] instructor instructor)
        {
            if (new Guid(id) != instructor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!instructorExists(instructor.instructor_code))
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
            return View(instructor);
        }

        // GET: instructors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructor.SingleOrDefaultAsync(m => m.instructor_code == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var instructor = await _context.instructor.SingleOrDefaultAsync(m => m.instructor_code == id);
            _context.instructor.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool instructorExists(string id)
        {
            return _context.instructor.Any(e => e.instructor_code == id);
        }
    }
}
