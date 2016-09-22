using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPcore.Controllers
{
    public class mem_healthController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_healthController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_health
        public IActionResult Index(string memberId, string v)
        {
            if (memberId == null)
            {
                return NotFound();
            }
            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            ViewBag.blood_group = new SelectList(new[]
                {
                    new SelectListItem { Text = "O", Value = "O", Selected = true },
                    new SelectListItem { Text = "A", Value = "A"},
                    new SelectListItem { Text = "B", Value = "B"},
                    new SelectListItem { Text = "AB", Value = "C"},
                }, "Value", "Text");
            
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            mem_health mem_health = _context.mem_health.SingleOrDefault(m => m.member_code == member.member_code);


            if (mem_health == null)
            {
                mem_health mh = new mem_health();
                mh.member_code = member.member_code;
                return View(mh);
            }
            else
            {
                return View(mem_health);
            }
        }

        // POST: mem_health/Edit/5
        [HttpPost]
        public IActionResult Edit(string memberId, string member_code, string medical_history, string blood_group, string hobby, string restrict_food, string special_food, string special_skill)
        {
            ViewBag.memberId = memberId;

            mem_health mem_health = _context.mem_health.SingleOrDefault(m => m.member_code == member_code);
            if (mem_health == null)
            {
                mem_health mh = new mem_health();
                mh.member_code = member_code;
                mh.medical_history = medical_history;
                mh.blood_group = blood_group;
                mh.hobby = hobby;
                mh.restrict_food = restrict_food;
                mh.special_food = special_food;
                mh.special_skill = special_skill;
                mh.x_status = "Y";
                _context.Add(mh);
            }
            else
            {
                mem_health mh = mem_health;
                mh.member_code = member_code;
                mh.medical_history = medical_history;
                mh.blood_group = blood_group;
                mh.hobby = hobby;
                mh.restrict_food = restrict_food;
                mh.special_food = special_food;
                mh.special_skill = special_skill;
                mh.x_status = "Y";
                _context.Update(mh);
            }

            _context.SaveChanges();
            return Json(new { result = "success" });
        }
    }
}
