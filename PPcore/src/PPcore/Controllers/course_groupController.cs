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
    public class course_groupController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public course_groupController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: course_group
        public IActionResult Index()
        {
            ViewBag.countRecords = _context.course_group.Count();
            return View();
        }

        [HttpGet]
        public IActionResult DetailsAsTable()
        {
            var cg = _context.course_group.OrderBy(m => m.cgroup_code);
            return View(cg.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("cgroup_code,cgroup_desc,x_status")] course_group course_group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course_group);
                await _context.SaveChangesAsync();
            }
            return Json(new { result = "success" });
        }

        // GET: course_group/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course_group = await _context.course_group.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (course_group == null)
            {
                return NotFound();
            }
            return View(course_group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("cgroup_code,cgroup_desc,id,x_status")] course_group course_group)
        {
            if (course_group.id != new Guid(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    course_group cg = _context.course_group.SingleOrDefault(c => (c.id == new Guid(id)));
                    cg.cgroup_code = course_group.cgroup_code.Trim();
                    cg.cgroup_desc = course_group.cgroup_desc.Trim();
                    cg.x_status = course_group.x_status;
                    _context.Update(cg);

                    //_context.Update(course_group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!course_groupExists(course_group.id.ToString()))
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
            return View(course_group);
        }

        // GET: course_group/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course_group = await _context.course_group.SingleOrDefaultAsync(m => m.cgroup_code == id);
            if (course_group == null)
            {
                return NotFound();
            }

            return View(course_group);
        }

        // POST: course_group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var course_group = await _context.course_group.SingleOrDefaultAsync(m => m.cgroup_code == id);
            _context.course_group.Remove(course_group);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool course_groupExists(string id)
        {
            return _context.course_group.Any(e => e.id == new Guid(id));
        }
    }
}
