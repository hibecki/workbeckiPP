using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PPcore.Services;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PPcore.Controllers
{
    public class newsController : Controller
    {
        private readonly PalangPanyaDBContext _context;
        private IConfiguration _configuration;
        private IHostingEnvironment _env;
        private readonly IEmailSender _emailSender;

        public newsController(PalangPanyaDBContext context, IConfiguration configuration, IHostingEnvironment env, IEmailSender emailSender)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult EMailAllMembers()
        {
            ViewBag.emailCode = "email" + DateTime.Now.ToString("yyMMddhhmmss");
            return View();
        }

        [HttpPost]
        public IActionResult EMailAllMembersSend(string emailCode, string topic, string content)
        {
            var title = topic;
            var body = content;
            List<string> attachFiles = new List<string>();
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            uploads = Path.Combine(uploads, emailCode);
            DirectoryInfo di = new DirectoryInfo(uploads);
            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles()) attachFiles.Add(Path.Combine(uploads, file.Name));
            }
            RegexUtilities util = new RegexUtilities();
            var ms = _context.member.Where(mss => (mss.x_status != "N") && (mss.email != null)).ToList();
            if (ms != null)
            {
                foreach (member m in ms)
                {
                    char[] delimiterChars = { ' ', ',', ';' };
                    string[] emails = m.email.Split(delimiterChars);
                    foreach (string email in emails)
                    {
                        if (util.IsValidEmail(email)) _emailSender.SendEmailWithAttachment(email, title, body, attachFiles);
                    }
                }
            }
            return Json(new { result = "success" });
        }

        [HttpPost]
        public IActionResult UploadFiles(ICollection<IFormFile> file, string emailCode)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            uploads = Path.Combine(uploads, emailCode);
            Directory.CreateDirectory(uploads);

            var fileName = "";
            foreach (var fi in file)
            {
                if (fi.Length > 0)
                {
                    fileName = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(fi.ContentDisposition).FileName.Trim('"');
                    using (var SourceStream = fi.OpenReadStream())
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            SourceStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            return Json(new { result = "success", uploads = uploads, fileName = fileName });
        }
    }

    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
