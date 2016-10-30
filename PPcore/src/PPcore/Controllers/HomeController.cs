using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using PPcore.Helpers;
using System.IO;
using PPcore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPcore.Controllers
{
    public class HomeController : Controller
    {
        private PalangPanyaDBContext _context;
        private SecurityDBContext _scontext;
        private readonly IEmailSender _emailSender;
        private IConfiguration _configuration;
        private IHostingEnvironment _env;

        public HomeController(PalangPanyaDBContext context, SecurityDBContext scontext, IEmailSender emailSender, IConfiguration configuration, IHostingEnvironment env)
        {
            _context = context;
            _scontext = scontext;
            _emailSender = emailSender;
            _configuration = configuration;
            _env = env;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(HomeController.Login), "Home");
            //return RedirectToAction(nameof(membersController.Index), "members");
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string uname, string upwd)
        {
            upwd = Utils.EncodeMd5(upwd.Trim());

            var m =  _context.member.SingleOrDefault(mm => (mm.mem_username == uname.Trim()) && (mm.mem_password == upwd));
            if (m != null)
            {
                var smr = await _scontext.SecurityMemberRoles.SingleOrDefaultAsync(smrr => (smrr.MemberId == m.id) && (smrr.x_status != "N"));
                if (smr != null)
                {
                    HttpContext.Session.SetString("memberId", m.id.ToString());
                    HttpContext.Session.SetString("roleId", m.mem_role_id.ToString());
                    HttpContext.Session.SetString("username", m.mem_username);
                    HttpContext.Session.SetString("displayname", (m.fname + " " + m.lname).Trim());

                    var memberId = HttpContext.Session.GetString("memberId");
                    var roleId = HttpContext.Session.GetString("roleId");
                    var displayName = HttpContext.Session.GetString("displayname");

                    var rms = _scontext.SecurityRoleMenus.Where(rmss => rmss.RoleId == m.mem_role_id).OrderByDescending(rmss => rmss.MenuId).ToList();
                    if (rms != null)
                    {
                        string menuHtml = "<ul class='nav navbar-top-links navbar-left'>_menuleft_</ul><ul class='nav navbar-top-links navbar-right'>_menuright_</ul>";
                        string menuLeft = ""; string menuRight = "";
                        string link = ""; string menuTemp = ""; int prevLevel = 0;
                        foreach (SecurityRoleMenus rm in rms)
                        {
                            SecurityMenus menu = await _scontext.SecurityMenus.SingleOrDefaultAsync(me => me.MenuId == rm.MenuId);
                            if (menu != null)
                            {
                                if (menu.HaveChild == 1)
                                {
                                    menuTemp = "<li class='dropdown'><a class='dropdown-toggle' data-toggle='dropdown' href='#'>" + menu.MenuDisplay + "</a><ul class='dropdown-menu'>" + menuTemp + "</ul></li>";
                                    if (menu.Level == 1)
                                    {
                                        if (menu.IsRightAlign != 1)
                                        {
                                            menuLeft = menuTemp + menuLeft;
                                        }
                                        else
                                        {
                                            menuRight = menuTemp + menuRight;
                                        }
                                        menuTemp = "";
                                    }
                                }
                                else
                                {
                                    if (menu.Level != prevLevel)
                                    {
                                        if (menu.IsRightAlign != 1)
                                        {
                                            menuLeft = menuTemp + menuLeft;
                                        }
                                        else
                                        {
                                            menuRight = menuTemp + menuRight;
                                        }
                                        menuTemp = "";
                                    }

                                    if (menu.MenuUrl != null)
                                    {
                                        link = menu.MenuUrl;
                                    }
                                    else
                                    {
                                        link = Url.Action(menu.MenuAction, menu.MenuController);
                                    }
                                    if (menu.MenuName != "-")
                                    {
                                        if (menu.Level != 1)
                                        {
                                            menuTemp = "<li><a href='" + link + "'>" + menu.MenuDisplay.Replace(@"""", @"\""") + "</a></li>" + menuTemp;
                                        }
                                        else
                                        {
                                            menuTemp = "<li><a class='dropdown-toggle' href='" + link + "'>" + menu.MenuDisplay.Replace(@"""", @"\""") + "</a></li>" + menuTemp;
                                        }
                                        
                                    }
                                    else
                                    {
                                        menuTemp = "<li class='divider'></li>" + menuTemp;
                                    }
                                    if (menu.Level == 1)
                                    {
                                        if (menu.IsRightAlign != 1)
                                        {
                                            menuLeft = menuTemp + menuLeft;
                                        }
                                        else
                                        {
                                            menuRight = menuTemp + menuRight;
                                        }
                                        menuTemp = "";
                                    }
                                }
                                prevLevel = menu.Level;
                            }
                        }
                        menuRight = menuRight.Replace("_displayname_", "<span id='_displayname_'>" + displayName + "</span>");
                        menuHtml = menuHtml.Replace("_menuright_", menuRight);
                        menuHtml = menuHtml.Replace("_menuleft_", menuLeft);
                        HttpContext.Session.SetString("mainmenu", menuHtml);
                    }

                    smr.LoggedInDate = DateTime.Now;
                    _scontext.Update(smr);
                    await _scontext.SaveChangesAsync();

                    var returnUrl = "";
                    //if (roleId != "c5a644a2-97b0-40e5-aa4d-e2afe4cdf428") //Not Administrators role
                    //{
                    //    if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not Member
                    //    {
                    //        //returnUrl = Url.Action("Index", "members");
                    //        returnUrl = Url.Action("DetailsPersonal", "members");
                    //    }
                    //    else //Is Member
                    //    {
                    //        returnUrl = Url.Action("DetailsPersonal", "members");
                    //    }
                    //}
                    //else //Have Administrators role
                    //{
                    //    returnUrl = Url.Action("ManageMembers", "Security");
                    //}
                    returnUrl = Url.Action("Home", "Security");
                    return Json(new { result = "success", url = returnUrl });
                }
                else
                {
                    return Json(new { result = "fail" });
                }
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }

        public IActionResult RegisterMember()
        {
            var mtc = _context.mem_testcenter.Where(mtcc => mtcc.x_status != "N").OrderBy(mtcc => mtcc.mem_testcenter_desc).Select(mtcc => new { Value = mtcc.mem_testcenter_code, Text = mtcc.mem_testcenter_desc }).ToList();
            mtc.Insert(0, (new { Value = "0", Text = "--- สนามสอบ ---" }));
            ViewBag.mem_testcenter_code = new SelectList(mtc.AsEnumerable(), "Value", "Text", "0");
            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> Create(string birthdate, string cid_card, string email, string fname, string lname, string mobile, string mem_photo, string cid_card_pic)
        //public IActionResult RegisterMember(string birthdate, string cid_card, string email, string fname, string lname, string mobile, string mem_photo, string cid_card_pic)
        public IActionResult RegisterMember(string birthdate, string cid_card, string email, string fname, string lname, string mobile, string mem_testcenter_code)
        {
            DateTime bd = Convert.ToDateTime(birthdate);
            //birthdate = (bd.Year).ToString() + bd.Month.ToString() + bd.Day.ToString();
            birthdate = (bd.Year).ToString() + bd.ToString("MMdd");
            string password = cid_card.Substring(cid_card.Length - 4);
            string passwordMD5 = Utils.EncodeMd5(password);
            try
            {
                //if ((!string.IsNullOrEmpty(mem_photo)) && (mem_photo.Substring(0, 1) != "M"))
                //{
                //    var fileName = mem_photo.Substring(9);
                //    var fileExt = Path.GetExtension(fileName);

                //    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), mem_photo);
                //    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                //    //System.IO.File.Copy(s, d, true);
                //    //System.IO.File.Delete(s);

                //    pic_image m = new pic_image();
                //    m.image_code = "M" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                //    m.x_status = "Y";
                //    m.image_name = fileName;
                //    string base64String = "";
                //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                //    {
                //        using (MemoryStream mem = new MemoryStream())
                //        {
                //            image.Save(mem, image.RawFormat);
                //            byte[] imageBytes = mem.ToArray();
                //            base64String = Convert.ToBase64String(imageBytes);
                //        }
                //    }
                //    m.image_file = base64String;

                //    m.ref_doc_type = "member";
                //    m.ref_doc_code = cid_card; //member_code;
                //    fileName = m.image_code;
                //    _context.pic_image.Add(m);
                //    _context.SaveChanges();

                //    System.IO.File.Delete(s);
                //    //clearImageUpload();

                //    mem_photo = m.image_code;
                //}
                //if ((!string.IsNullOrEmpty(cid_card_pic)) && (cid_card_pic.Substring(0, 1) != "C"))
                //{
                //    var fileName = cid_card_pic.Substring(9);
                //    var fileExt = Path.GetExtension(fileName);

                //    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), cid_card_pic);
                //    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                //    //System.IO.File.Copy(s, d, true);
                //    //System.IO.File.Delete(s);

                //    pic_image pic_image = new pic_image();
                //    pic_image.image_code = "C" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                //    pic_image.x_status = "Y";
                //    pic_image.image_name = fileName;
                //    string base64String = "";
                //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                //    {
                //        using (MemoryStream mem = new MemoryStream())
                //        {
                //            image.Save(mem, image.RawFormat);
                //            byte[] imageBytes = mem.ToArray();
                //            base64String = Convert.ToBase64String(imageBytes);
                //        }
                //    }
                //    pic_image.image_file = base64String;
                //    pic_image.ref_doc_type = "cidcard";
                //    pic_image.ref_doc_code = cid_card; //member_code;
                //    fileName = pic_image.image_code;
                //    _context.pic_image.Add(pic_image);
                //    _context.SaveChanges();

                //    System.IO.File.Delete(s);
                //    //clearImageUpload();

                //    cid_card_pic = pic_image.image_code;
                //}

                mem_testcenter_code = (mem_testcenter_code == "0") ? "NULL" : "'" + mem_testcenter_code + "'";

                //_context.Database.ExecuteSqlCommand("INSERT INTO member (member_code,cid_card,birthdate,fname,lname,mobile,email,x_status,mem_username,mem_password,mem_role_id,mem_photo,cid_card_pic) VALUES ('" + cid_card + "','" + cid_card + "','" + birthdate + "',N'" + fname + "',N'" + lname + "','" + mobile + "','" + email + "','Y','" + cid_card + "','" + passwordMD5 + "','17822a90-1029-454a-b4c7-f631c9ca6c7d','" + mem_photo + "','" + cid_card_pic + "')");
                _context.Database.ExecuteSqlCommand("INSERT INTO member (member_code,cid_card,birthdate,fname,lname,mobile,email,x_status,mem_username,mem_password,mem_role_id,mem_testcenter_code,register_date) VALUES ('" + cid_card + "','" + cid_card + "','" + birthdate + "',N'" + fname + "',N'" + lname + "','" + mobile + "','" + email + "','Y','" + cid_card + "','" + passwordMD5 + "','17822a90-1029-454a-b4c7-f631c9ca6c7d', " + mem_testcenter_code + ",GETDATE())");

                var mb = _context.member.SingleOrDefault(mm => mm.member_code == cid_card);
                SecurityMemberRoles smr = new SecurityMemberRoles();
                smr.MemberId = mb.id;
                smr.CreatedDate = DateTime.Now;
                smr.CreatedBy = mb.id;
                smr.EditedDate = DateTime.Now;
                smr.EditedBy = mb.id;
                smr.x_status = "Y";
                _scontext.Add(smr);
                _scontext.SaveChanges();

                SendEmail(email, cid_card, password);
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

        [HttpPost]
        public async Task<IActionResult> uploadMemPhoto(ICollection<IFormFile> fileMemPhoto)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            var filePrefix = DateTime.Now.ToString("ddhhmmss") + "_";
            var fileName = ""; var fileExt = "";
            foreach (var file in fileMemPhoto)
            {
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    fileExt = Path.GetExtension(fileName);

                    //fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;
                    //fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length)));

                    fileName = fileName.Substring(0, fileName.Length - fileExt.Length);
                    fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;

                    using (var SourceStream = file.OpenReadStream())
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, filePrefix + fileName), FileMode.Create))
                        {
                            await SourceStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            return Json(new { result = "success", uploads = uploads, fileName = filePrefix + fileName });
        }


        [HttpPost]
        public async Task<IActionResult> uploadCidCardPhoto(ICollection<IFormFile> fileCidCardPhoto)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            var filePrefix = DateTime.Now.ToString("ddhhmmss") + "_";
            var fileName = ""; var fileExt = "";

            foreach (var file in fileCidCardPhoto)
            {
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    fileExt = Path.GetExtension(fileName);
                    //fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;
                    //fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length)));

                    fileName = fileName.Substring(0, fileName.Length - fileExt.Length);
                    fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;

                    using (var SourceStream = file.OpenReadStream())
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, filePrefix + fileName), FileMode.Create))
                        {
                            await SourceStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            return Json(new { result = "success", uploads = uploads, fileName = filePrefix + fileName });
        }


        [HttpPost]
        public IActionResult ForgetPwd(string uname, string upwd)
        {
            member mb = _context.member.SingleOrDefault(m => m.mem_username == uname.ToUpper());
            if (mb != null)
            {
                var newpwd = Utils.GeneratePassword();
                mb.mem_password = Utils.EncodeMd5(newpwd);
                _context.Update(mb);
                _context.SaveChanges();

                var mr = _scontext.SecurityMemberRoles.SingleOrDefault(mrr => mrr.MemberId == mb.id);
                if (mr != null)
                {
                    mr.EditedBy = mr.MemberId;
                    mr.EditedDate = DateTime.Now;
                    _scontext.Update(mr);
                    _scontext.SaveChanges();
                }

                SendEmail(mb.email, uname, newpwd);

                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }

        [HttpPost]
        public IActionResult ResetPwd(string id)
        {
            member mb = _context.member.SingleOrDefault(m => m.id == new Guid(id));
            if (mb != null)
            {
                var newpwd = Utils.GeneratePassword();
                mb.mem_password = Utils.EncodeMd5(newpwd);
                _context.Update(mb);
                _context.SaveChanges();

                var mr = _scontext.SecurityMemberRoles.SingleOrDefault(mrr => mrr.MemberId == mb.id);
                if (mr != null)
                {
                    var memberId = HttpContext.Session.GetString("memberId");
                    mr.EditedBy = new Guid(memberId);
                    mr.EditedDate = DateTime.Now;
                    _scontext.Update(mr);
                    _scontext.SaveChanges();
                }

                SendEmail(mb.email, mb.mem_username, newpwd);

                return Json(new { result = "success" });
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }


        private void SendEmail(string email, string username, string password)
        {
            var title = "พลังปัญญา";
            var body = "ชื่อผู้ใช้งาน: " + username + "\nรหัสผ่าน: " + password;
            _emailSender.SendEmailAsync(email, title, body);
        }
    }
}
