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
    public class product_groupController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        private void prepareViewBag()
        {
            ViewBag.x_status = new SelectList(new[] { new { Value = "Y", Text = "ใช้งาน" }, new { Value = "N", Text = "ยกเลิก" } }, "Value", "Text", "Y");
        }

        public product_groupController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: product_group
        public async Task<IActionResult> Index()
        {
            prepareViewBag();
            return View(await _context.product_group.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("product_group_code,id,product_group_desc,rowversion,x_log,x_note,x_status")] product_group product_group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product_group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product_group);
        }

        // GET: product_group/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_group = await _context.product_group.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (product_group == null)
            {
                return NotFound();
            }
            prepareViewBag();
            return View(product_group);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("product_group_code,id,product_group_desc,rowversion,x_log,x_note,x_status")] product_group product_group)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(product_group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!product_groupExists(product_group.product_group_code))
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
            return View(product_group);
        }

        // GET: product_group/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_group = await _context.product_group.SingleOrDefaultAsync(m => m.product_group_code == id);
            if (product_group == null)
            {
                return NotFound();
            }

            return View(product_group);
        }

        // POST: product_group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product_group = await _context.product_group.SingleOrDefaultAsync(m => m.product_group_code == id);
            _context.product_group.Remove(product_group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool product_groupExists(string id)
        {
            return _context.product_group.Any(e => e.product_group_code == id);
        }
    }
}
