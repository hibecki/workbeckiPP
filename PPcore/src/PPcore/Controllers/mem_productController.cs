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
    public class mem_productController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public mem_productController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_product
        public IActionResult Index(string memberId, string v)
        {
            List<ViewModels.mem_product.mem_productViewModel> mem_productViewModels = new List<ViewModels.mem_product.mem_productViewModel>();
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_products = _context.mem_product.Where(m => m.member_code == member.member_code).OrderBy(m => m.rec_no).ToList();
            foreach (var mp in mem_products)
            {
                var mem_productViewModel = new ViewModels.mem_product.mem_productViewModel();
                mem_productViewModel.mem_product = mp;
                var product = _context.product.Single(p => p.product_code == mp.product_code);
                mem_productViewModel.product = product;
                mem_productViewModel.product_group_desc = _context.product_group.SingleOrDefault(p => p.product_group_code == product.product_group_code).product_group_desc;
                mem_productViewModel.product_type_desc = _context.product_type.Single(p => (p.product_type_code == product.product_type_code) && (p.product_group_code == product.product_group_code)).product_type_desc;
                mem_productViewModels.Add(mem_productViewModel);
            }
            ViewBag.memberId = memberId;
            //ViewBag.course_grade = new SelectList(_context.course_grade, "cgrade_code", "cgrade_desc");(x.Body.Scopes.Count > 5) && (x.Foo == "test")
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            return View(mem_productViewModels);
        }

        //AddProduct
        [HttpGet]
        public IActionResult Create(string memberId, string productId)
        {
            var m = _context.member.SingleOrDefault(mb => mb.id == new Guid(memberId));

            var mpro = _context.mem_product.Where(mpr => (mpr.member_code == m.member_code) && (mpr.id == new Guid(productId))).Count();

            if (mpro == 0)
            {
                var pd = _context.product.SingleOrDefault(pdt => pdt.id == new Guid(productId));
                var pgrp = _context.product_group.SingleOrDefault(pg => pg.product_group_code == pd.product_group_code);
                var ptyp = _context.product_type.SingleOrDefault(pt => (pt.product_type_code == pd.product_type_code) && (pt.product_group_code == pd.product_group_code));
                mem_product mp = new mem_product();
                mp.member_code = m.member_code;
                mp.product_code = pd.product_code;
                mp.x_status = "Y";

                _context.Add(mp);
                _context.SaveChanges();

                //var mpCount = _context.mem_product.Where(mj => (mj.member_code == m.member_code)).Count();
                var mpMax = _context.mem_product.Where(mj => (mj.member_code == m.member_code)).Max(mj => mj.rec_no);

                return Json(new { result = "success", rec_no = mpMax, mem_product_id = mp.id, product_group_desc = pgrp.product_group_desc, product_type_desc = ptyp.product_type_desc, product_desc = pd.product_desc });
            }
            else
            {
                return Json(new { result = "duplicate" });
            }
        }

        //Edit Product
        [HttpGet]
        public IActionResult Edit(string memberId, string productId, string mem_productId)
        {
            var m = _context.member.SingleOrDefault(mb => mb.id == new Guid(memberId));
            var pd = _context.product.SingleOrDefault(pr => pr.id == new Guid(productId));

            var mproCheck = _context.mem_product.Where(mpr => (mpr.member_code == m.member_code) && (mpr.product_code == pd.product_code)).Count();
            if (mproCheck == 0)
            {
                mem_product mp = new mem_product();
                mp.member_code = m.member_code;
                mp.product_code = pd.product_code;
                mp.x_status = "Y";
                _context.Add(mp);

                var mpro = _context.mem_product.SingleOrDefault(mpr => (mpr.member_code == m.member_code) && (mpr.id == new Guid(mem_productId)));
                var recno = mpro.rec_no;
                _context.Remove(mpro);

                _context.SaveChanges();

                var mpro2 = _context.mem_product.SingleOrDefault(mpr => (mpr.member_code == m.member_code) && (mpr.product_code == pd.product_code));
                mpro2.rec_no = recno;
                _context.Update(mpro2);
                _context.SaveChanges();

                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "duplicate" });
            }
        }
    }
}
