using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace PPcore.Controllers
{
    public class mem_rewardController : Controller
    {
        private PalangPanyaDBContext _context;

        public mem_rewardController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: mem_reward
        public IActionResult Index(string memberId, string v)
        {
            List<Models.mem_reward> mem_rewards = new List<Models.mem_reward>();
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            mem_rewards = _context.mem_reward.Where(m => m.member_code == member.member_code).OrderBy(m => m.rec_no).ToList();

            ViewBag.memberId = memberId;
            if (!String.IsNullOrEmpty(v)) { ViewBag.isViewOnly = 1; } else { ViewBag.isViewOnly = 0; }
            return View(mem_rewards);
        }

        // GET: mem_reward/Details/5
        public IActionResult Details(string id)
        {
            if (id != null)
            {
                mem_reward mr = _context.mem_reward.Single(m => m.id == new Guid(id));
                if (mr != null)
                {
                    return Json(new { id = mr.id, rec_no = mr.rec_no, reward_desc = mr.reward_desc });
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: mem_reward/Create
        [HttpPost]
        public IActionResult Create(string memberId, string reward_desc)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));

            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO mem_reward (rec_no,member_code,reward_desc,x_status) VALUES (0,'" + member.member_code + "',N'" + reward_desc + "','Y')");
            }
            catch (SqlException ex)
            {
                var errno = ex.Number; var msg = "";
                if (errno == 2627) //Violation of primary key. Handle Exception
                {
                    msg = "duplicate";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }
            catch (Exception ex)
            {
                var errno = ex.HResult; var msg = "";
                if (ex.InnerException.Message.IndexOf("PRIMARY KEY") != -1)
                {
                    msg = "duplicate";
                }
                return Json(new { result = "fail", error_code = errno, error_message = msg });
            }

            return Json(new { result = "success" });
        }

        // GET: mem_reward/Edit/5
        public IActionResult Edit(string id)
        {
            return Details(id);
        }

        // POST: mem_reward/Edit/5
        [HttpPost]
        public IActionResult Edit(string memberId, string id, int rec_no, string reward_desc)
        {
            var member = _context.member.Single(m => m.id == new Guid(memberId));
            var mem_reward = _context.mem_reward.Single(m => m.id == new Guid(id));
            mem_reward.reward_desc = reward_desc;
            _context.Update(mem_reward);

            _context.SaveChanges();

            return Json(new { result = "success" });
        }
    }
}
