using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPtest.Services;

namespace PPtest.Controllers
{
    public class EMailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EMailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> send()
        {
            await _emailSender.SendEmailAsync("nokflyingtest@gmail.com", "testing", "try to send");
            return View();
        }
    }
}