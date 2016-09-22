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
    public class product_typeController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        private void prepareViewBag()
        {
            ViewBag.x_status = new SelectList(new[] { new { Value = "Y", Text = "ใช้งาน" }, new { Value = "N", Text = "ยกเลิก" } }, "Value", "Text", "Y");
        }

        public product_typeController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        [HttpGet]
        public IActionResult SelectBy(string product_group_code)
        {
            if (product_group_code == null)
            {
                return NotFound();
            }
            var selectOptions = "";
            var productTypes = _context.product_type.Where(p => p.product_group_code == product_group_code).OrderBy(p => p.product_type_desc).ToList();
            if (productTypes == null)
            {
                return NotFound();
            }

            foreach (var productType in productTypes)
            {
                selectOptions += "<option value='" + productType.product_type_code + "'>" + productType.product_type_desc + "</option>";
            }

            return Content(selectOptions);
        }

        // GET: product_type
        public async Task<IActionResult> Index()
        {
            return View(await _context.product_type.ToListAsync());
        }

        // GET: product_type/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_type = await _context.product_type.SingleOrDefaultAsync(m => m.product_type_code == id);
            if (product_type == null)
            {
                return NotFound();
            }

            return View(product_type);
        }

        [HttpGet]
        public IActionResult DetailsAsTable(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_type = _context.product_type.Where(m => m.product_group_code == id).ToList();
            if (product_type == null)
            {
                return NotFound();
            }

            return View(product_type);
        }

        // GET: product_type/Create
        public IActionResult Create()
        {
            prepareViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("product_type_code,product_group_code,id,product_type_desc,rowversion,x_log,x_note,x_status")] product_type product_type)
        {
            _context.Add(product_type);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }

        // GET: product_type/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_type = await _context.product_type.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (product_type == null)
            {
                return NotFound();
            }
            prepareViewBag();
            return View(product_type);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("product_type_code,product_group_code,id,product_type_desc,rowversion,x_log,x_note,x_status")] product_type product_type)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product_type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { result = "fail" });
                }
                return Json(new { result = "success" });
            }
            return Json(new { result = "fail" });
        }

        // GET: product_type/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product_type = await _context.product_type.SingleOrDefaultAsync(m => m.product_type_code == id);
            if (product_type == null)
            {
                return NotFound();
            }

            return View(product_type);
        }

        // POST: product_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product_type = await _context.product_type.SingleOrDefaultAsync(m => m.product_type_code == id);
            _context.product_type.Remove(product_type);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool product_typeExists(string id)
        {
            return _context.product_type.Any(e => e.product_type_code == id);
        }
    }
}
