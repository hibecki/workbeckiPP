using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace PPcore.Controllers
{
    public class productsController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public productsController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: product/DetailsAsTableList
        public IActionResult DetailsAsTableList(string memberId, string IsDetailsPersonal)
        {
            var roleId = HttpContext.Session.GetString("roleId");
            if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not member
            {
                ViewBag.IsMember = 0;
            }
            else //Is member
            {
                ViewBag.IsMember = 1;
            }
            ViewBag.product_group = new SelectList(_context.product_group, "product_group_code", "product_group_desc", "1");
            ViewBag.memberId = memberId;

            List<product> p = new List<product>(); 

            return View(p);
        }

        [HttpGet]
        public IActionResult DetailsAsJsonList(string product_group_code, string product_type_code, string pattern)
        {
            if ((product_type_code == null) || (product_group_code == null))
            {
                return NotFound();
            }
            //product_type_code = product_type_code.PadRight(3);
            //var sqlProduct = "SELECT * FROM product WHERE product_group_code = '"+ product_group_code + "'";
            //var products = _context.product.FromSql(sqlProduct).ToList();
            //if (pattern.Trim() == "") pattern = "%";
            //var products = _context.product.Where(pr => (pr.product_group_code == product_group_code) && (pr.product_type_code == product_type_code) && (pr.x_status == "Y")).OrderBy(pr => pr.rec_no).ToList();
            var cProd = _context.product.Where(pr => (pr.product_group_code == product_group_code) && (pr.product_type_code == product_type_code) && (pr.x_status == "Y"));
            if (pattern != null)
            {
                pattern = pattern.Trim();
                if (pattern != "")
                {
                    cProd = cProd.Where(pr => pr.product_desc.Contains(pattern));
                }
            }
            var products = cProd.OrderBy(pr => pr.rec_no).ToList();
            if (products == null)
            {
                return NotFound();
            }
            List<productItem> p = new List<productItem>();
            foreach (var product in products)
            {
                p.Add(new productItem { rec_no = product.rec_no, product_code = product.product_code, product_desc = product.product_desc, id = product.id.ToString() });
            }
            string pjson = JsonConvert.SerializeObject(p);
            return Json(pjson);
        }

        // GET: products/Create
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.memberId;
            product p = new product();
            return View(p);
        }

        [HttpPost]
        public IActionResult Create(string product_code, string product_desc, string product_group_code, string product_type_code)
        {
            product p = new product();
            p.product_code = product_code.Trim();
            p.product_desc = product_desc.Trim();
            p.product_group_code = product_group_code.Trim();
            p.product_type_code = product_type_code.Trim();
            p.x_status = "Y";
            _context.Add(p);
            try
            {
                _context.SaveChanges();
                //var cR = _context.product.Where(pr => (pr.product_group_code == product_group_code) && (pr.product_type_code == product_type_code)).Count();
                var mR = _context.product.Where(pr => (pr.product_group_code == product_group_code) && (pr.product_type_code == product_type_code)).Max(pr => pr.rec_no);

                return Json(new { result = "success", rec_no = mR, product_code = p.product_code, product_desc = p.product_desc });
            }
            catch (SqlException ex)
            {
                var errno = ex.Number; var msg = "";
                if (errno == 2627) //Violation of primary key. Handle Exception
                {
                    msg = "รหัสผลิตผลซ้ำ";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }
            catch (Exception ex)
            {
                var errno = ex.HResult; var msg = "";
                if (ex.InnerException.Message.IndexOf("PRIMARY KEY") != -1) {
                    msg = "รหัสผลิตผลซ้ำ";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }

            //_context.Database.ExecuteSqlCommand("INSERT INTO product (product_group_code,product_type_code,product_code,product_desc,x_status) VALUES ('" + product_group_code + "','" + product_type_code + "','" + product_code + "','" + product_desc + "','Y')");
            //var cP = _context.product.Count();


        }

        [HttpGet]
        public IActionResult Edit(string product_code)
        {
            product p = _context.product.SingleOrDefault(pr => (pr.product_code == product_code));
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(string product_code, string product_desc)
        {
            product p = _context.product.SingleOrDefault(pr => (pr.product_code == product_code));
            p.product_desc = product_desc.Trim();
            _context.Update(p);
            try
            {
                _context.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (SqlException ex)
            {
                var errno = ex.Number; var msg = "";
                if (errno == 2627) //Violation of primary key. Handle Exception
                {
                    msg = "รหัสผลิตผลซ้ำ";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }
            catch (Exception ex)
            {
                var errno = ex.HResult; var msg = "";
                if (ex.InnerException.Message.IndexOf("PRIMARY KEY") != -1)
                {
                    msg = "รหัสผลิตผลซ้ำ";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }
        }
        private class productItem
        {
            public int rec_no;
            public string product_code;
            public string product_desc;
            public string id;
        }
    }
}
