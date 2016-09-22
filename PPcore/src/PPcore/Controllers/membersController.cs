using System.Linq;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using PPcore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PPcore.Services;
using PPcore.Helpers;
using System.Data.SqlClient;

namespace PPcore.Controllers
{
    public class membersController : Controller
    {
        private PalangPanyaDBContext _context;
        private SecurityDBContext _scontext;
        private readonly IEmailSender _emailSender;
        private IConfiguration _configuration;
        private IHostingEnvironment _env;

        private void prepareViewBag()
        {
            ViewBag.images_upload = _configuration.GetSection("Paths").GetSection("images_upload").Value;
            ViewBag.images_member = _configuration.GetSection("Paths").GetSection("images_member").Value;

            //ViewBag.sex = new SelectList(new[] { new { Value = "F", Text = "Female" }, new { Value = "M", Text = "Male" }, new { Value = "A", Text = "Alternative" } }, "Value", "Text", "F");
            ViewBag.sex = new SelectList(new[] { new { Value = "F", Text = "หญิง" }, new { Value = "M", Text = "ชาย" } }, "Value", "Text", "F");
            ViewBag.mem_title = new SelectList(new[] { new { Value = "0", Text = "" }, new { Value = "นาย", Text = "นาย" }, new { Value = "นาง", Text = "นาง" }, new { Value = "นางสาว", Text = "นางสาว" } }, "Value", "Text", "0");

            //ViewBag.cid_type = new SelectList(new[] { new { Value = "C", Text = "บัตรประชาชน" }, new { Value = "H", Text = "สำเนาทะเบียนบ้าน" }, new { Value = "P", Text = "Passport" } }, "Value", "Text", "F");
            //ViewBag.cid_type = new SelectList(new[] { new { Value = "C", Text = "บัตรประชาชน" }, new { Value = "P", Text = "Passport" } }, "Value", "Text", "F");
            ViewBag.marry_status = new SelectList(new[] { new { Value = "0", Text = "" }, new { Value = "N", Text = "โสด" }, new { Value = "Y", Text = "สมรส" } }, "Value", "Text", "0");
            ViewBag.zone = new SelectList(new[] { new { Value = "M", Text = "กลาง" }, new { Value = "N", Text = "เหนือ" }, new { Value = "E", Text = "ตะวันออก" }, new { Value = "W", Text = "ตะวันตก" }, new { Value = "S", Text = "ใต้" }, new { Value = "L", Text = "ตะวันออกเฉียงเหนือ" } }, "Value", "Text");

            //ViewBag.mem_group = new SelectList(_context.mem_group.OrderBy(g => g.mem_group_code), "mem_group_code", "mem_group_desc", "0  ");
            var mg = _context.mem_group.OrderBy(g => g.mem_group_code).Select(x => new { Value = x.mem_group_code, Text = x.mem_group_desc }).ToList();
            mg.Insert(0, (new { Value = "0", Text = "" }));
            ViewBag.mem_group = new SelectList(mg.AsEnumerable(), "Value", "Text", "0");

            //ViewBag.mem_type = new SelectList(_context.mem_type.OrderBy(t => t.mem_group_code).OrderBy(t => t.mem_type_code), "mem_type_code", "mem_type_desc", "3  ");

            //ViewBag.mem_level = new SelectList(_context.mem_level.OrderBy(t => t.mlevel_code), "mlevel_code", "mlevel_desc", "0  ");
            var ml = _context.mem_level.OrderBy(tt => tt.mlevel_code).Select(tt => new { Value = tt.mlevel_code, Text = tt.mlevel_desc }).ToList();
            ml.Insert(0, (new { Value = "0", Text = "" }));
            ViewBag.mem_level = new SelectList(ml.AsEnumerable(), "Value", "Text", "0");

            //ViewBag.mem_status = new SelectList(_context.mem_status.OrderBy(s => s.mstatus_code), "mstatus_code", "mstatus_desc", "0  ");
            var ms = _context.mem_status.OrderBy(ss => ss.mstatus_code).Select(ss => new { Value = ss.mstatus_code, Text = ss.mstatus_desc }).ToList();
            ms.Insert(0, (new { Value = "0", Text = "" }));
            ViewBag.mem_status = new SelectList(ms.AsEnumerable(), "Value", "Text", "0");


            ViewBag.income = new SelectList(new[] { new { Value = "", Text = "" }, new { Value = "A", Text = "น้อยกว่า 10,000 บาท" }, new { Value = "B", Text = "10,000 ถึง 20,000 บาท" }, new { Value = "C", Text = "20,000 ถึง 30,000 บาท" }, new { Value = "D", Text = "มากกว่า 30,000 บาท" } }, "Value", "Text", "");

            ViewBag.ini_country = new SelectList(_context.ini_country.OrderBy(c => c.country_code), "country_code", "country_desc", "66");

            //ViewBag.ini_province = new SelectList(_context.ini_province.OrderBy(p => p.province_code), "province_code", "pro_desc", "0       ");
            var ip = _context.ini_province.OrderBy(p => p.province_code).Select(p => new { Value = p.province_code, Text = p.pro_desc }).ToList();
            ip.Insert(0, (new { Value = "0", Text = "" }));
            ViewBag.ini_province = new SelectList(ip.AsEnumerable(), "Value", "Text", "0");

            ViewBag.IsCreate = 0; ViewBag.IsEdit = 0; ViewBag.IsDetails = 0; ViewBag.IsDetailsPersonal = 0;
            ViewBag.IsDisabled = false; ViewBag.IsMember = false;
        }

        public membersController(PalangPanyaDBContext context, SecurityDBContext scontext, IEmailSender emailSender, IConfiguration configuration, IHostingEnvironment env)
        {
            _context = context;
            _scontext = scontext;
            _emailSender = emailSender;
            _configuration = configuration;
            _env = env;
        }

        public void SendEmail(string email, string username, string password)
        {
            var title = "พลังปัญญา";
            var body = "ชื่อผู้ใช้งาน: " + username + "\nรหัสผ่าน: " + password;
            _emailSender.SendEmailAsync(email, title, body);
        }

        // GET: members
        public IActionResult Index()
        {
            //var pp = _context.member.Where(ppp => (ppp.mem_role_id == new Guid("17822a90-1029-454a-b4c7-f631c9ca6c7d")) && (ppp.x_status != "N"));
            //ViewBag.countRecords = pp.Count();
            //return View(pp.OrderByDescending(m => m.rowversion).ToList());

            //Get only member role
            var pp = _context.member.Where(ppp => (ppp.mem_role_id == new Guid("17822a90-1029-454a-b4c7-f631c9ca6c7d")) && (ppp.x_status != "N"))
                .Select(ppp => new { ppp.id, ppp.member_code, ppp.title, ppp.fname, ppp.lname, ppp.birthdate, ppp.age, ppp.cid_card, ppp.texta_address, ppp.textb_address, ppp.textc_address, ppp.tel, ppp.mobile, ppp.email, ppp.facebook, ppp.line, ppp.rowversion})
                .OrderByDescending(ppp => ppp.rowversion).ToList();
            ViewBag.countRecords = pp.Count();

            List<PPcore.ViewModels.member.MemberIndexViewModel> pl = new List<PPcore.ViewModels.member.MemberIndexViewModel>();
            foreach (var p in pp)
            {
                PPcore.ViewModels.member.MemberIndexViewModel pv = new PPcore.ViewModels.member.MemberIndexViewModel();
                pv.age = p.age;
                pv.birthdate = p.birthdate;
                pv.cid_card = p.cid_card;
                pv.email = p.email;
                pv.facebook = p.facebook;
                pv.fname = p.fname;
                pv.id = p.id;
                pv.line = p.line;
                pv.lname = p.lname;
                pv.member_code = p.member_code;
                pv.mobile = p.mobile;
                pv.rowversion = p.rowversion;
                pv.tel = p.tel;
                pv.texta_address = p.texta_address;
                pv.textb_address = p.textb_address;
                pv.textc_address = p.textc_address;
                pv.title = p.title;
                
                pl.Add(pv);
            }
            return View(pl);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            member member = _context.member.Single(m => m.id == new Guid(id));
            if (member == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(member.mem_photo))
            {
                //pic_image m = _context.pic_image.Single(i => i.image_code == member.mem_photo);
                //ViewBag.memPhoto = m.image_name;
                ViewBag.memPhoto = member.mem_photo;
            }

            if (!String.IsNullOrEmpty(member.cid_card_pic))
            {
                //pic_image c = _context.pic_image.Single(i => i.image_code == member.cid_card_pic);
                //ViewBag.cidCardPhoto =c.image_name;
                ViewBag.cidCardPhoto = member.cid_card_pic;
            }
            ViewBag.memberId = id;
            ViewBag.TabNoData = "";
            
            //For dropdown province; we need to manually assign it, then prepare viewbag for javascript
            ViewBag.zipCode = member.zip_code;
            ViewBag.subdistrictCode = member.subdistrict_code;
            ViewBag.districtCode = member.district_code;
            //ViewBag.provinceCode = member.province_code;
            ViewBag.incomeCode = member.income;

            prepareViewBag();
            ViewBag.IsDetails = 1;
            ViewBag.IsDisabled = true;
            return View(member);
        }

        private string formatTelNumber(string tel)
        {
            var result = "";
            if (tel != null)
            {
                //    if ((tel.Count() == 9) && (tel.Substring(0, 2) == "02"))
                //    {
                //        result = "02-" + tel.Substring(2, 3) + "-" + new String('X', tel.Substring(5).Count());
                //    }
                //    else if ((tel.Count() == 10) && (tel.Substring(0, 2) == "08"))
                //    {
                //        result = "08-" + tel.Substring(2, 4) + "-" + new String('X', tel.Substring(6).Count());
                //    }
                //    else if(tel.Count() == 9)
                //    {
                //        result = tel.Substring(0, 3) + "-" + tel.Substring(3, 3) + "-" + new String('X', tel.Substring(6).Count());
                //    }
                //    else
                //    {
                //        result = tel;
                //    }
                result = tel.Trim();
            }
            else
            {
                result = "";
            }
            return result;
        }
        // GET: members/DetailsPdf/5
        public FileStreamResult DetailsPdf(String id)
        {
            // Find who executes this
            var userId = HttpContext.Session.GetString("memberId");
            member muser = _context.member.SingleOrDefault(mb => mb.id == new Guid(userId));

            // Who will be printed
            member m = _context.member.SingleOrDefault(mb => mb.id == new Guid(id));

            var fname = m.title + " " + m.fname; var lname = m.lname;
            var sex = m.sex == "F"? "หญิง" : "ชาย";
            var nationality = m.nationality;

            var birthdate = String.Format("{0:d MMMM yyyy}",m.birthdate);
            //DateTime today = DateTime.Today;
            //int year = 0;
            //Int32.TryParse(String.Format("{0:yyyy}", m.birthdate),out year);
            //int age = today.Year - year + 543;
            //if (m.birthdate > today.AddYears(-age)) age--;
            //var current_age = age.ToString();
            var current_age = m.age.ToString();

            //var cid_card = m.cid_card.Substring(0,7) + "XXXXXX";
            var cid_card = m.cid_card;

            var marry_status = m.marry_status == "N"? "โสด" :"สมรส";

            var religion = m.religion;
            var tel = formatTelNumber(m.tel);
            var mobile = formatTelNumber(m.mobile);
            var fax = formatTelNumber(m.fax);
            var email = m.email;
            var facebook = m.facebook;
            var lineId = m.line;
            var text_address = m.texta_address + " " + m.textb_address + " " + m.textc_address;
            text_address = text_address.Trim();
            var count = 0;

            List<listTraining> train = new List<listTraining>();
            List<mem_train_record> mtrs = _context.mem_train_record.Where(mtr => (mtr.member_code == m.member_code) && (mtr.x_status == "Y")).OrderBy(mtr => mtr.course_code).ToList();
            foreach (mem_train_record mtr in mtrs)
            {
                //Remove this because change business logic => mem_train_record is not related to project_course
                //project_course pc = _context.project_course.SingleOrDefault(p => p.course_code == mtr.course_code);

                //Remove this because it was removed from screen
                //course_grade cg = _context.course_grade.SingleOrDefault(c => c.cgrade_code == mtr.course_grade);

                count++;
                //train.Add(new listTraining { rec_no = count.ToString(), code = mtr.course_code, desc = pc.course_desc, grade = cg.cgrade_desc });
                train.Add(new listTraining { rec_no = count.ToString(), code = mtr.course_code, desc = mtr.course_desc, grade = "" });
            }

            List<listInfo> visit = new List<listInfo>();
            List<mem_site_visit> msvs = _context.mem_site_visit.Where(msv => (msv.member_code == m.member_code) && (msv.x_status == "Y")).OrderBy(msv => msv.rec_no).ToList();
            foreach (mem_site_visit msv in msvs)
            {
                ini_country ic = _context.ini_country.SingleOrDefault(c => c.country_code == msv.country_code);
                visit.Add(new listInfo { rec_no = msv.rec_no.ToString(), desc = msv.site_visit_desc + " ที่ประเทศ " + ic.country_desc});
            }
            

            List<listInfo> social = new List<listInfo>();
            List<mem_social> mss = _context.mem_social.Where(ms => (ms.member_code == m.member_code) && (ms.x_status =="Y")).OrderBy(ms => ms.rec_no).ToList();
            foreach (mem_social ms in mss)
            {
                social.Add(new listInfo { rec_no = ms.rec_no.ToString(), desc = ms.social_desc });
            }

            List<listInfo> reward = new List<listInfo>();
            List<mem_reward> mrs = _context.mem_reward.Where(mr => (mr.member_code == m.member_code) && (mr.x_status == "Y")).OrderBy(mr => mr.rec_no).ToList();
            foreach (mem_reward mr in mrs)
            {
                reward.Add(new listInfo { rec_no = mr.rec_no.ToString(), desc = mr.reward_desc });
            }

            List<listInfo> education = new List<listInfo>();
            List<mem_education> mes = _context.mem_education.Where(me => (me.member_code == m.member_code) && (me.x_status == "Y")).OrderBy(me => me.rec_no).ToList();
            foreach (mem_education me in mes)
            {
                var level = String.IsNullOrEmpty(me.degree) ? "" : "ระดับ " + me.degree + " ";
                var fct = String.IsNullOrEmpty(me.faculty) ? "" : " สาขา/วิชาเอก " + me.faculty;
                education.Add(new listInfo { rec_no = me.rec_no.ToString(), desc = level + me.colledge_name + fct });
            }

            mem_health mh = _context.mem_health.SingleOrDefault(mhr => mhr.member_code == m.member_code);
            var medical_history = ""; var blood_group = ""; var restrict_food = "";
            var hobby = ""; var special_skill = ""; var special_food = "";
            if (mh != null)
            {
                medical_history = mh.medical_history;
                blood_group = mh.blood_group;
                if (blood_group!=null) { blood_group = (blood_group != "C") ? blood_group : "AB"; }
                restrict_food = mh.restrict_food;
                hobby = mh.hobby; special_skill = mh.special_skill; special_food = mh.special_food;
            }

            List<listWork> work = new List<listWork>();
            List<mem_worklist> mwls = _context.mem_worklist.Where(mwl => (mwl.member_code == m.member_code) && (mwl.x_status == "Y")).OrderBy(mwl => mwl.rec_no).ToList();
            foreach(mem_worklist mwl in mwls)
            {
                var comeng = String.IsNullOrEmpty(mwl.company_name_eng) ? "" : " (" + mwl.company_name_eng + ")";
                var poseng = String.IsNullOrEmpty(mwl.position_name_eng) ? "" : " ("+ mwl.position_name_eng +")";
                work.Add(new listWork { rec_no = mwl.rec_no.ToString(), company = mwl.company_name_th + comeng, position = mwl.position_name_th + poseng, year = mwl.work_year, address = mwl.office_address });
            }

            //List<listProduct> product = new List<listProduct>();
            //List<mem_product> mpds = _context.mem_product.Where(mp => (mp.member_code == m.member_code) && (mp.x_status == "Y")).OrderBy(mp => mp.rec_no).ToList();
            //foreach (mem_product mpd in mpds)
            //{
            //    product prod = _context.product.SingleOrDefault(p => p.product_code == mpd.product_code);
            //    product_group prodG = _context.product_group.SingleOrDefault(p => p.product_group_code == prod.product_group_code);
            //    product_type prodT = _context.product_type.SingleOrDefault(p => p.product_type_code == prod.product_type_code);
            //    product.Add(new listProduct { rec_no = mpd.rec_no.ToString(), productGroup = prodG.product_group_desc, productType = prodT.product_type_desc, productDesc = prod.product_desc});
            //}

            List<listInfo> product = new List<listInfo>();
            List<mem_product> mpds = _context.mem_product.Where(mp => (mp.member_code == m.member_code) && (mp.x_status == "Y")).OrderBy(mp => mp.rec_no).ToList();
            foreach (mem_product mpd in mpds)
            {
                product prod = _context.product.SingleOrDefault(p => p.product_code == mpd.product_code);
                product_group prodG = _context.product_group.SingleOrDefault(p => p.product_group_code == prod.product_group_code);
                product_type prodT = _context.product_type.SingleOrDefault(p => (p.product_type_code == prod.product_type_code) && (p.product_group_code == prod.product_group_code));
                product.Add(new listInfo { rec_no = mpd.rec_no.ToString(), desc = "กลุ่มผลิตผล " + prodG.product_group_desc + " ประเภทผลิตผล " + prodT.product_type_desc + " ผลิตผล " + prod.product_desc });
            }

            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 50, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            var logoPath = Path.Combine(_env.WebRootPath, "images");
            logoPath = Path.Combine(logoPath, "logonew.jpg");
            Image logo = Image.GetInstance(logoPath);
            //logo.ScalePercent(30);
            logo.ScalePercent(13);
            logo.Alignment = Element.ALIGN_CENTER;

            Image memberPic;
            if (m.mem_photo != null)
            {
                pic_image pii = _context.pic_image.SingleOrDefault(piii => piii.image_code == m.mem_photo);
                byte[] imageBytes = Convert.FromBase64String(pii.image_file);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                memberPic = Image.GetInstance(ms);
            }
            else
            {
                var memberPicPath = Path.Combine(_env.WebRootPath, "images/dummy_person.jpg");
                memberPic = Image.GetInstance(memberPicPath);
            }

            memberPic.ScaleToFit(120f, 120f);
            memberPic.Border = Rectangle.BOX;
            memberPic.BorderColor = BaseColor.DARK_GRAY;
            memberPic.BorderWidth = 1f;
            memberPic.SetAbsolutePosition(document.PageSize.Width - 30f - memberPic.ScaledWidth, document.PageSize.Height - 130f);
            memberPic.Alignment = Element.ALIGN_RIGHT;

            //Rectangle rect = new Rectangle(30, 36, 565, 706);
            Rectangle rect = new Rectangle(30, 30, 565, 706);
            rect.Border = Rectangle.BOX;
            rect.BorderColor = BaseColor.DARK_GRAY;
            rect.BorderWidth = 1f;

            var fontPath = Path.Combine(_env.WebRootPath, "fonts/THSarabunNew.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font cn = new Font(bf, 14);
            Font cnb = new Font(bf, 14, Font.BOLD);
            Font cni = new Font(bf, 14, Font.ITALIC);
            Font cng = new Font(bf, 14, Font.NORMAL, BaseColor.GRAY);

            document.Open();
            document.Add(rect);
            document.Add(logo); document.Add(memberPic);

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            //table.TotalWidth = document.PageSize.Width;
            table.TotalWidth = 535f;
            table.LockedWidth = true;
            table.DefaultCell.VerticalAlignment = 1;
            table.DefaultCell.PaddingLeft = 15f;
            //table.DefaultCell.PaddingTop = 30f;
            float[] widths = new float[] { 135f, 400f };
            table.SetWidths(widths);
            table.SpacingBefore = 50f;

            //new paragraph Member Info
            PdfPCell cell = new PdfPCell(new Phrase("ข้อมูลตามบัตรประชาชน", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            //Member Info Row 1
            table.AddCell(cell);
            PdfPTable memberInfoRow1 = new PdfPTable(6);
            memberInfoRow1.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow1.TotalWidth = 400f;
            memberInfoRow1.LockedWidth = true;
            memberInfoRow1.DefaultCell.VerticalAlignment = 1;
            memberInfoRow1.SetWidths(new float[] { 15f, 135f, 20f, 30f, 30f, 55f });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase("ชื่อ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase(fname + " " + lname, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase("เพศ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase(sex, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase("สัญชาติ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow1.AddCell(new PdfPCell(new Phrase(nationality, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow1); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 2
            table.AddCell("");
            PdfPTable memberInfoRow2 = new PdfPTable(5);
            memberInfoRow2.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow2.TotalWidth = 400f;
            memberInfoRow2.LockedWidth = true;
            memberInfoRow2.DefaultCell.VerticalAlignment = 1;
            memberInfoRow2.SetWidths(new float[] { 60f, 95f, 20f, 15f, 105f });
            memberInfoRow2.AddCell(new PdfPCell(new Phrase("วัน/เดือน/ปี เกิด", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow2.AddCell(new PdfPCell(new Phrase(birthdate, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow2.AddCell(new PdfPCell(new Phrase("อายุ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow2.AddCell(new PdfPCell(new Phrase(current_age, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow2.AddCell(new PdfPCell(new Phrase("ปี", cnb)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow2); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 3
            table.AddCell("");
            PdfPTable memberInfoRow3 = new PdfPTable(6);
            memberInfoRow3.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow3.TotalWidth = 400f;
            memberInfoRow3.LockedWidth = true;
            memberInfoRow3.DefaultCell.VerticalAlignment = 1;
            //memberInfoRow3.SetWidths(new float[] { 78f, 77f, 40f, 25f, 25f, 50f });
            //memberInfoRow3.SetWidths(new float[] { 85f, 70f, 40f, 25f, 25f, 50f });
            memberInfoRow3.SetWidths(new float[] { 60f, 95f, 40f, 25f, 25f, 50f });
            //memberInfoRow3.AddCell(new PdfPCell(new Phrase("หมายเลขบัตรประชาชน", cnb)) { Border = Rectangle.NO_BORDER });
            //memberInfoRow3.AddCell(new PdfPCell(new Phrase("เลขบัตรประชาชน/พาสปอร์ต", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase("เลขบัตรประชาชน", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase(cid_card, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase("สถานภาพ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase(marry_status, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase("ศาสนา", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase(religion, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow3); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 4
            table.AddCell("");
            PdfPTable memberInfoRow4 = new PdfPTable(4);
            memberInfoRow4.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow4.TotalWidth = 400f;
            memberInfoRow4.LockedWidth = true;
            memberInfoRow4.DefaultCell.VerticalAlignment = 1;
            memberInfoRow4.SetWidths(new float[] { 83f, 77f, 80f, 65f });
            memberInfoRow4.AddCell(new PdfPCell(new Phrase("หมายเลขโทรศัพท์มือถือ", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow4.AddCell(new PdfPCell(new Phrase(mobile, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow4.AddCell(new PdfPCell(new Phrase("หมายเลขโทรศัพท์บ้าน", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow4.AddCell(new PdfPCell(new Phrase(tel, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow4); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 5
            table.AddCell("");
            PdfPTable memberInfoRow5 = new PdfPTable(4);
            memberInfoRow5.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow5.TotalWidth = 400f;
            memberInfoRow5.LockedWidth = true;
            memberInfoRow5.DefaultCell.VerticalAlignment = 1;
            memberInfoRow5.SetWidths(new float[] { 25f, 135f, 23f, 122f });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase("แฟกซ์", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase(fax, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase("อีเมล", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase(email, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow5); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            //Member Info Row 6

            table.AddCell("");
            PdfPTable memberInfoRow61 = new PdfPTable(2);
            memberInfoRow61.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow61.TotalWidth = 400f;
            memberInfoRow61.LockedWidth = true;
            memberInfoRow61.DefaultCell.VerticalAlignment = 1;
            memberInfoRow61.SetWidths(new float[] { 50f, 350f });
            memberInfoRow61.AddCell(new PdfPCell(new Phrase("Facebook:", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow61.AddCell(new PdfPCell(new Phrase(facebook, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow61); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            table.AddCell("");
            PdfPTable memberInfoRow62 = new PdfPTable(2);
            memberInfoRow62.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow62.TotalWidth = 400f;
            memberInfoRow62.LockedWidth = true;
            memberInfoRow62.DefaultCell.VerticalAlignment = 1;
            memberInfoRow62.SetWidths(new float[] { 30f, 285f });
            memberInfoRow62.AddCell(new PdfPCell(new Phrase("Line ID:", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow62.AddCell(new PdfPCell(new Phrase(lineId, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow62); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);


            //Member Info Row 7
            table.AddCell("");
            PdfPTable memberInfoRow7 = new PdfPTable(2);
            memberInfoRow7.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow7.TotalWidth = 400f;
            memberInfoRow7.LockedWidth = true;
            memberInfoRow7.DefaultCell.VerticalAlignment = 1;
            memberInfoRow7.SetWidths(new float[] { 20f, 350f });
            memberInfoRow7.AddCell(new PdfPCell(new Phrase("ที่อยู่", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow7.AddCell(new PdfPCell(new Phrase(text_address, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow7); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 8
            //table.AddCell("");
            //PdfPTable memberInfoRow8 = new PdfPTable(2);
            //memberInfoRow8.DefaultCell.Border = Rectangle.NO_BORDER;
            //memberInfoRow8.TotalWidth = 400f;
            //memberInfoRow8.LockedWidth = true;
            //memberInfoRow8.DefaultCell.VerticalAlignment = 1;
            //memberInfoRow8.SetWidths(new float[] { 30f, 285f });
            //memberInfoRow8.AddCell("");
            //memberInfoRow8.AddCell(new PdfPCell(new Phrase(textb_address, cni)) { Border = Rectangle.NO_BORDER });
            //cell = new PdfPCell(memberInfoRow8); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            //table.AddCell(cell);

            //Line
            table.AddCell(" ");
            //Line
            PdfPTable line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph Training Info
            cell = new PdfPCell(new Phrase("ประวัติการฝึกอบรม", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (train.Count > 0)
            {
                for (var i = 0; i < train.Count; i++)
                {
                    //PdfPTable rowz = new PdfPTable(7);
                    PdfPTable rowz = new PdfPTable(3);
                    //rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    //rowz.SetWidths(new float[] { 8f, 12f, 30f, 22f, 70f, 27f, 40f });
                    rowz.SetWidths(new float[] { 8f, 22f, 179f });
                    rowz.AddCell(new PdfPCell(new Phrase(train[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });

                    //rowz.AddCell(new PdfPCell(new Phrase("รหัส", cnb)) { Border = Rectangle.NO_BORDER });
                    //rowz.AddCell(new PdfPCell(new Phrase(train[i].code, cni)) { Border = Rectangle.NO_BORDER });

                    rowz.AddCell(new PdfPCell(new Phrase("หลักสูตร", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(train[i].desc, cni)) { Border = Rectangle.NO_BORDER });

                    //Remove grade because business logic change to not have grade in mem_train_record
                    //rowz.AddCell(new PdfPCell(new Phrase("ระดับเกรด", cnb)) { Border = Rectangle.NO_BORDER });
                    //rowz.AddCell(new PdfPCell(new Phrase(train[i].grade, cni)) { Border = Rectangle.NO_BORDER });

                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }

            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph
            cell = new PdfPCell(new Phrase("ประวัติการดูงาน", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (visit.Count > 0)
            {
                for (var i = 0; i < visit.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    //rowz.SetWidths(new float[] { 8f, 180f });  rowz.SetWidths(new float[] { 15f, 355f });
                    rowz.SetWidths(new float[] { 15f, 340f, 10f });
                    rowz.AddCell(new PdfPCell(new Phrase(visit[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(visit[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }

            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph Social Info
            cell = new PdfPCell(new Phrase("ประสบการณ์ช่วยเหลือสังคม", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (social.Count > 0)
            {
                for (var i = 0; i < social.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 15f, 340f, 10f });
                    rowz.AddCell(new PdfPCell(new Phrase(social[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(social[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }


            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph
            cell = new PdfPCell(new Phrase("รางวัลเชิดชูเกียรติ", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (reward.Count > 0)
            {
                for (var i = 0; i < reward.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 15f, 340f, 10f });
                    rowz.AddCell(new PdfPCell(new Phrase(reward[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(reward[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }

            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph
            cell = new PdfPCell(new Phrase("การศึกษา", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (education.Count > 0)
            {
                for (var i = 0; i < education.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 15f, 340f, 10f });
                    rowz.AddCell(new PdfPCell(new Phrase(education[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(education[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }

            //Line
            //table.AddCell(" ");
            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph Health Info
            cell = new PdfPCell(new Phrase("ข้อมูลสุขภาพ", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;

            //Health Info Row 1
            table.AddCell(cell);
            PdfPTable healthInfoRow1 = new PdfPTable(2);
            healthInfoRow1.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow1.TotalWidth = 400f;
            healthInfoRow1.LockedWidth = true;
            healthInfoRow1.DefaultCell.VerticalAlignment = 1;
            healthInfoRow1.SetWidths(new float[] { 30f, 165f });
            healthInfoRow1.AddCell(new PdfPCell(new Phrase("โรคประจำตัว", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow1.AddCell(new PdfPCell(new Phrase(medical_history, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow1); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            //Health Info Row 2
            table.AddCell("");
            PdfPTable healthInfoRow2 = new PdfPTable(4);
            healthInfoRow2.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow2.TotalWidth = 400f;
            healthInfoRow2.LockedWidth = true;
            healthInfoRow2.DefaultCell.VerticalAlignment = 1;
            healthInfoRow2.SetWidths(new float[] { 30f, 50f, 45f, 70f });
            healthInfoRow2.AddCell(new PdfPCell(new Phrase("หมู่โลหิต", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow2.AddCell(new PdfPCell(new Phrase(blood_group, cni)) { Border = Rectangle.NO_BORDER });
            healthInfoRow2.AddCell(new PdfPCell(new Phrase("งานอดิเรก/กีฬาที่ชอบ", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow2.AddCell(new PdfPCell(new Phrase(hobby, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow2); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            //Health Info Row 3
            table.AddCell("");
            PdfPTable healthInfoRow31 = new PdfPTable(2);
            healthInfoRow31.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow31.TotalWidth = 400f;
            healthInfoRow31.LockedWidth = true;
            healthInfoRow31.DefaultCell.VerticalAlignment = 1;
            healthInfoRow31.SetWidths(new float[] { 80f, 290f });
            healthInfoRow31.AddCell(new PdfPCell(new Phrase("อาหารที่แพ้/ยาที่แพ้", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow31.AddCell(new PdfPCell(new Phrase(restrict_food, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow31); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            table.AddCell("");
            PdfPTable healthInfoRow32 = new PdfPTable(2);
            healthInfoRow32.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow32.TotalWidth = 400f;
            healthInfoRow32.LockedWidth = true;
            healthInfoRow32.DefaultCell.VerticalAlignment = 1;
            healthInfoRow32.SetWidths(new float[] { 80f, 290f });
            healthInfoRow32.AddCell(new PdfPCell(new Phrase("ความสามารถพิเศษ", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow32.AddCell(new PdfPCell(new Phrase(special_skill, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow32); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            table.AddCell("");
            PdfPTable healthInfoRow33 = new PdfPTable(2);
            healthInfoRow33.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow33.TotalWidth = 400f;
            healthInfoRow33.LockedWidth = true;
            healthInfoRow33.DefaultCell.VerticalAlignment = 1;
            healthInfoRow33.SetWidths(new float[] { 80f, 290f });
            healthInfoRow33.AddCell(new PdfPCell(new Phrase("ประเภทอาหารพิเศษ", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow33.AddCell(new PdfPCell(new Phrase(special_food, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow33); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);


            //Line
            table.AddCell(" "); //table.AddCell(" ");
            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph
            cell = new PdfPCell(new Phrase("ข้อมูลสถานที่ทำงาน", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (work.Count > 0)
            {
                for (var i = 0; i < work.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER; rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    rowz.SetWidths(new float[] { 8f, 40f, 150f });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ชื่อสถานที่ทำงาน", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].company, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);

                    if (i == 0)
                    {
                        cell = new PdfPCell(new Phrase("และประวัติการทำงาน", cnb));
                        cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
                        table.AddCell(cell);
                    }
                    else
                    {
                        table.AddCell("");
                    }

                    rowz = new PdfPTable(5);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER; rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    rowz.SetWidths(new float[] { 8f, 20f, 90f, 25f, 40f });
                    rowz.AddCell("  ");
                    rowz.AddCell(new PdfPCell(new Phrase("ตำแหน่ง", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].position, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ปีที่ทำงาน", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].year, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");

                    rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER; rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    rowz.SetWidths(new float[] { 10f, 18f, 200f });
                    rowz.AddCell(new PdfPCell(new Phrase(" ", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ที่อยู่", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].address, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }


            //Line
            line = new PdfPTable(3);
            line.DefaultCell.Border = Rectangle.NO_BORDER;
            line.DefaultCell.FixedHeight = 1f;
            line.SpacingBefore = 10f;
            line.TotalWidth = 400f;
            line.LockedWidth = true;
            line.SetWidths(new float[] { 5f, 295f, 15f });
            line.AddCell("");
            cell = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, FixedHeight = 1f };
            cell.BorderWidthTop = 1f;
            line.AddCell(cell);
            line.AddCell("");
            table.AddCell(line);

            //new paragraph
            cell = new PdfPCell(new Phrase("ผลิตผลในครัวเรือน", cnb));
            cell.HorizontalAlignment = 2; cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
            if (product.Count > 0)
            {
                for (var i = 0; i < product.Count; i++)
                {
                    PdfPTable rowz = new PdfPTable(3);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER; rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    rowz.SetWidths(new float[] { 15f, 340f, 10f });
                    rowz.AddCell(new PdfPCell(new Phrase(product[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(product[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }
            else
            {
                cell = new PdfPCell(new Phrase(" ", cni)) { Border = Rectangle.NO_BORDER }; cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                table.AddCell(cell);
                table.AddCell("");
            }


            document.Add(table);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            PdfReader prr = new PdfReader(workStream);
            byte[] byteRes;
            using (MemoryStream ms2 = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(prr, ms2, '\0', true))
                {
                    int n = prr.NumberOfPages; PdfPTable ptable; PdfContentByte cb;
                    for (int i = 1; i <= n; i++)
                    {
                        ptable = new PdfPTable(2);
                        ptable.TotalWidth = 530f;
                        ptable.LockedWidth = true;
                        ptable.DefaultCell.FixedHeight = 20;
                        ptable.DefaultCell.Border = Rectangle.NO_BORDER;
                        ptable.DefaultCell.VerticalAlignment = 1;
                        ptable.SetWidths(new float[] { 300f, 230f });
                        var printdate = String.Format("{0:d MMMM yyyy}", DateTime.Now);
                        ptable.AddCell(new PdfPCell(new Phrase("Printed by: "+ muser.fname + " " + muser.lname + " ("+muser.mem_username+") Printed date: " + printdate, cng)) { Border = Rectangle.NO_BORDER });
                        ptable.AddCell(new PdfPCell(new Phrase(string.Format("Page {0} of {1}", i, n), cng)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = 2 });
                        cb = stamper.GetOverContent(i);
                        ptable.WriteSelectedRows(0, -1, 34, 33, cb);
                        if (i > 1)
                        {
                            Rectangle recta = new Rectangle(30, 30, 565, 806);
                            recta.Border = Rectangle.BOX;
                            recta.BorderColor = BaseColor.DARK_GRAY;
                            recta.BorderWidth = 1f;
                            cb.Rectangle(recta);
                            cb.Stroke();
                        }
                    }
                }
                byteRes = ms2.ToArray();
            }

            MemoryStream mems = new MemoryStream(byteRes);

            return new FileStreamResult(mems, "application/pdf");
        }

        // GET: members/Create
        public IActionResult Create()
        {
            prepareViewBag();
            ViewBag.IsCreate = 1;
            return View();
        }

        // POST: members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("member_code,birthdate,building,cid_card,cid_card_pic,cid_type,country_code,current_age,district_code,email,fax,floor,fname,h_no,lane,latitude,lname,longitude,lot_no,marry_status,mem_group_code,mem_photo,mem_type_code,mlevel_code,mobile,mstatus_code,nationality,parent_code,place_name,province_code,religion,room,rowversion,sex,social_app_data,street,subdistrict_code,tel,texta_address,textb_address,textc_address,village,x_log,x_note,x_status,zip_code,zone")] member member)
        public IActionResult Create(member member)
        {
            //if (ModelState.IsValid)
            //{
                member.x_status = "Y";
                if (member.title == "0") { member.title = null; }
                if (member.marry_status == "0") { member.marry_status = null; }
                if (member.mem_group_code == "0") { member.mem_group_code = null; }
                if (member.mlevel_code == "0") { member.mlevel_code = null; }
                if (member.mstatus_code == "0") { member.mstatus_code = null; }
                if (member.province_code == "0") { member.province_code = null; }
                if ((!string.IsNullOrEmpty(member.mem_photo)) && (member.mem_photo.Substring(0, 1) != "M"))
                {
                    var fileName = member.mem_photo.Substring(9);
                    var fileExt = Path.GetExtension(fileName);

                    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), member.mem_photo);
                    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                    //System.IO.File.Copy(s, d, true);
                    //System.IO.File.Delete(s);

                    pic_image m = new pic_image();
                    m.image_code = "M" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                    m.x_status = "Y";
                    m.image_name = fileName;
                    string base64String = "";
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                    {
                        using (MemoryStream mem = new MemoryStream())
                        {
                            image.Save(mem, image.RawFormat);
                            byte[] imageBytes = mem.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
                    m.image_file = base64String;
                    m.ref_doc_type = "member";
                    m.ref_doc_code = member.member_code;
                    fileName = m.image_code;
                    _context.pic_image.Add(m);
                    _context.SaveChanges();

                    System.IO.File.Delete(s);
                    clearImageUpload();

                    member.mem_photo = m.image_code;
                }
                if ((!string.IsNullOrEmpty(member.cid_card_pic)) && (member.cid_card_pic.Substring(0, 1) != "C"))
                {
                    var fileName = member.cid_card_pic.Substring(9);
                    var fileExt = Path.GetExtension(fileName);

                    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), member.cid_card_pic);
                    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                    //System.IO.File.Copy(s, d, true);
                    //System.IO.File.Delete(s);

                    pic_image pic_image = new pic_image();
                    pic_image.image_code = "C" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                    pic_image.x_status = "Y";
                    pic_image.image_name = fileName;
                    string base64String = "";
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                    {
                        using (MemoryStream mem = new MemoryStream())
                        {
                            image.Save(mem, image.RawFormat);
                            byte[] imageBytes = mem.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
                    pic_image.image_file = base64String;
                    pic_image.ref_doc_type = "cidcard";
                    pic_image.ref_doc_code = member.member_code;
                    fileName = pic_image.image_code;
                    _context.pic_image.Add(pic_image);
                    _context.SaveChanges();

                    System.IO.File.Delete(s);
                    clearImageUpload();

                    member.cid_card_pic = pic_image.image_code;
                }
                member.mem_username = member.cid_card;
                string password = member.cid_card.Substring(member.cid_card.Length - 4);
                member.mem_password = Utils.EncodeMd5(password);
                member.mem_role_id = new Guid("17822a90-1029-454a-b4c7-f631c9ca6c7d"); //Role member

                //var user = new ApplicationUser { UserName = member.cid_card, Email = member.email };
                //_userManager.CreateAsync(user, password);
                //_userManager.AddToRoleAsync(user, "Members");
               // System.Security.Claims.Claim cl = new System.Security.Claims.Claim("fullName", member.fname + " " + member.lname);
                //_userManager.AddClaimAsync(user, cl);
                //_signInManager.SignInAsync(user, isPersistent: false);
                SendEmail(member.email, member.cid_card, password);

                //var result = await _userManager.CreateAsync(user, password);

                //if (result.Succeeded)
                //{
                //    await _userManager.AddToRoleAsync(user, "Members");
                //    System.Security.Claims.Claim cl = new System.Security.Claims.Claim("fullName", fname + " " + lname);
                //    await _userManager.AddClaimAsync(user, cl);
                //    await _signInManager.SignInAsync(user, isPersistent: false);
                //    _logger.LogInformation(3, "User created a new account with password.");
                //    SendEmail(email, cid_card, password);
                //}
                //else
                //{
                //    return Json(new { result = "fail", error_code = -1, error_message = "userManaget cannot create user!" });
                //}

                _context.member.Add(member);
                _context.SaveChanges();

                var sessionMemberId = HttpContext.Session.GetString("memberId");
                SecurityMemberRoles smr = new SecurityMemberRoles();
                smr.MemberId = member.id;
                smr.CreatedDate = DateTime.Now;
                smr.CreatedBy = new Guid(sessionMemberId);
                smr.EditedDate = DateTime.Now;
                smr.EditedBy = new Guid(sessionMemberId);
                smr.x_status = "Y";
                _scontext.SecurityMemberRoles.Add(smr);
                _scontext.SaveChanges();

                return RedirectToAction("Index");
            //}

            //prepareViewBag();
            //return View(member);
        }

        // GET: members/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            member member = _context.member.Single(m => m.id == new Guid(id));
            if (member == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(member.mem_photo))
            {
                //pic_image m = _context.pic_image.Single(i => i.image_code == member.mem_photo);
                //ViewBag.memPhoto = m.image_name;
                ViewBag.memPhoto = member.mem_photo;
            }

            if (!String.IsNullOrEmpty(member.cid_card_pic))
            {
                //pic_image c = _context.pic_image.Single(i => i.image_code == member.cid_card_pic);
                //ViewBag.cidCardPhoto =c.image_name;
                ViewBag.cidCardPhoto = member.cid_card_pic;
            }

            ViewBag.memberId = id;

            //For dropdown province; we need to manually assign it, then prepare viewbag for javascript
            ViewBag.zipCode = member.zip_code;
            ViewBag.subdistrictCode = member.subdistrict_code;
            ViewBag.districtCode = member.district_code;
            //ViewBag.provinceCode = member.province_code;
            ViewBag.incomeCode = member.income;


            prepareViewBag();ViewBag.IsEdit = 1;
            var roleId = HttpContext.Session.GetString("roleId");
            if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not member
            {
                ViewBag.IsMember = false;
            }
            else //Is member
            {
                ViewBag.IsMember = true;
            }
            clearImageUpload();
            return View(member);
        }

        // POST: members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("income,title,occupation,mail_address,facebook,line,mem_username,mem_password,mem_role_id,member_code,birthdate,building,cid_card,cid_card_pic,cid_type,country_code,current_age,district_code,email,fax,floor,fname,h_no,id,lane,latitude,lname,longitude,lot_no,marry_status,mem_group_code,mem_photo,mem_type_code,mlevel_code,mobile,mstatus_code,nationality,parent_code,place_name,province_code,religion,room,rowversion,sex,street,subdistrict_code,tel,texta_address,textb_address,textc_address,village,x_log,x_note,x_status,zip_code,zone")] member member)
        {
            if (ModelState.IsValid)
            {
                var mo = _context.member.SingleOrDefault(moo => moo.id == new Guid(id));
                var mem_photo_old = mo.mem_photo;
                var cid_card_pic_old = mo.cid_card_pic;
                _context.Entry(mo).State = EntityState.Detached;

                member.x_status = "Y";
                if (member.title == "0") { member.title = null; }
                if (member.marry_status == "0") { member.marry_status = null; }
                if (member.mem_group_code == "0") { member.mem_group_code = null; }
                if (member.mlevel_code == "0") { member.mlevel_code = null; }
                if (member.mstatus_code == "0") { member.mstatus_code = null; }
                if (member.province_code == "0") { member.province_code = null; }
                if ((!string.IsNullOrEmpty(member.mem_photo)) && (member.mem_photo.Substring(0,1) != "M"))
                {
                    var fileName = member.mem_photo.Substring(9);
                    var fileExt = Path.GetExtension(fileName);

                    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), member.mem_photo);
                    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                    //System.IO.File.Copy(s, d, true);
                    //System.IO.File.Delete(s);

                    pic_image m = new pic_image();
                    m.image_code = "M" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                    m.x_status = "Y";
                    m.image_name = fileName;

                    string base64String = "";
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                    {
                        using (MemoryStream mem = new MemoryStream())
                        {
                            image.Save(mem, image.RawFormat);
                            byte[] imageBytes = mem.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
                    m.image_file = base64String;

                    m.ref_doc_type = "member";
                    m.ref_doc_code = member.member_code;
                    fileName = m.image_code;
                    _context.pic_image.Add(m);
                    _context.SaveChanges();

                    System.IO.File.Delete(s);
                    clearImageUpload();

                    var pmold = _context.pic_image.SingleOrDefault(pp => pp.image_code == mem_photo_old);
                    if (pmold != null)
                    {
                        _context.pic_image.Remove(pmold);
                        _context.SaveChanges();
                    }

                    member.mem_photo = m.image_code;
                }
                if ((!string.IsNullOrEmpty(member.cid_card_pic)) && (member.cid_card_pic.Substring(0, 1) != "C"))
                {
                    var fileName = member.cid_card_pic.Substring(9);
                    var fileExt = Path.GetExtension(fileName);

                    var s = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value), member.cid_card_pic);
                    //var d = Path.Combine(Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_member").Value), fileName);
                    //System.IO.File.Copy(s, d, true);
                    //System.IO.File.Delete(s);

                    pic_image pic_image = new pic_image();
                    pic_image.image_code = "C" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                    pic_image.x_status = "Y";
                    pic_image.image_name = fileName;

                    string base64String = "";
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(s))
                    {
                        using (MemoryStream mem = new MemoryStream())
                        {
                            image.Save(mem, image.RawFormat);
                            byte[] imageBytes = mem.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
                    pic_image.image_file = base64String;

                    pic_image.ref_doc_type = "cidcard";
                    pic_image.ref_doc_code = member.member_code;
                    fileName = pic_image.image_code;
                    _context.pic_image.Add(pic_image);
                    _context.SaveChanges();

                    System.IO.File.Delete(s);
                    clearImageUpload();

                    var pmold = _context.pic_image.SingleOrDefault(pp => pp.image_code == cid_card_pic_old);
                    if (pmold != null)
                    {
                        _context.pic_image.Remove(pmold);
                        _context.SaveChanges();
                    }

                    member.cid_card_pic = pic_image.image_code;
                }

                //member.mem_username = "";
                //member.mem_password = Utils.EncodeMd5(password);
                //member.mem_role_id = new Guid("17822a90-1029-454a-b4c7-f631c9ca6c7d"); //Role member

                _context.Update(member);
                _context.SaveChanges();
                var roleId = HttpContext.Session.GetString("roleId");
                if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not member
                {
                    return RedirectToAction("Index");
                }
                else //Is member
                {
                    return RedirectToAction("DetailsPersonal","members");
                }

            }
            prepareViewBag();
            return View(member);
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

        private void clearImageUpload()
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);

            string[] dirs = Directory.GetFiles(uploads);
            System.Diagnostics.Debug.WriteLine("The number of files starting with c is {0}.", dirs.Length);
            foreach (string dir in dirs)
            {
                var f = dir;
                System.Diagnostics.Debug.WriteLine(dir);
            }
        }

        [HttpGet]
        public IActionResult ListTabNoData(string memberId)
        {
            var memberCode = _context.member.SingleOrDefault(m => m.id == new Guid(memberId)).member_code;
            List<tab> t = new List<tab>();
            if (_context.mem_train_record.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_train_record", value = 0 });
            }
            if (_context.mem_site_visit.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_site_visit", value = 0 });
            }
            if (_context.mem_social.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_social", value = 0 });
            }
            if (_context.mem_reward.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_reward", value = 0 });
            }
            if (_context.mem_education.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_education", value = 0 });
            }
            if (_context.mem_health.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_health", value = 0 });
            }
            if (_context.mem_worklist.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_worklist", value = 0 });
            }
            if (_context.mem_product.Where(mt => mt.member_code == memberCode).Count() == 0)
            {
                t.Add(new tab { name = "mem_product", value = 0 });
            }
            return Json(JsonConvert.SerializeObject(t));
        }

        [HttpGet]
        public IActionResult SecurityDetailsAsTable(string roleId)
        {
            var ms = _context.member.Where(mss => mss.mem_role_id == new Guid(roleId))
                .Select(mss => new { mss.id, mss.mem_username, mss.fname, mss.lname, mss.email })
                .OrderBy(mss => mss.mem_username).ToArray();
            List<PPcore.ViewModels.member.SecurityMemberRolesViewModel> mvs = new List<ViewModels.member.SecurityMemberRolesViewModel>();
            foreach (var mem in ms)
            {
                PPcore.ViewModels.member.SecurityMemberRolesViewModel mv = new PPcore.ViewModels.member.SecurityMemberRolesViewModel();
                mv.memberId = mem.id;
                mv.mem_username = mem.mem_username;
                mv.displayname = (mem.fname + " " + mem.lname).Trim();
                mv.email = mem.email;

                //var smr = _scontext.SecurityMemberRoles.SingleOrDefault(smrr => smrr.MemberId == mem.id);
                var smr = _scontext.SecurityMemberRoles.Where(smrr => smrr.MemberId == mem.id).Select(smrr => new { smrr.LoggedInDate, smrr.LoggedOutDate, smrr.CreatedDate, smrr.CreatedBy, smrr. EditedDate, smrr.EditedBy, smrr.x_status} ).FirstOrDefault();

                mv.LoggedInDate = smr.LoggedInDate;
                mv.LoggedOutDate = smr.LoggedOutDate;
                mv.CreatedDate = smr.CreatedDate;
                mv.CreatedBy = smr.CreatedBy;
                mv.EditedDate = smr.EditedDate;
                mv.EditedBy = smr.EditedBy;
                mv.Status = smr.x_status;


                //var mc = _context.member.SingleOrDefault(mcc => mcc.id == mv.CreatedBy);
                var mc = _context.member.Where(mcc => mcc.id == mv.CreatedBy).Select(mcc => new { mcc.mem_username }).FirstOrDefault();
                if (mc != null)
                {
                    //mv.CreatedByUserName = (mc.fname + " " + mc.lname).Trim();
                    mv.CreatedByUserName = mc.mem_username;
                }
                else { mv.CreatedByUserName = ""; }


                //var me = _context.member.SingleOrDefault(mee => mee.id == mv.EditedBy);
                var me = _context.member.Where(mee => mee.id == mv.EditedBy).Select(mee => new { mee.mem_username }).FirstOrDefault();
                if (me != null)
                {
                    //mv.EditedByUserName = (me.fname + " " + me.lname).Trim();
                    mv.EditedByUserName = me.mem_username;
                }
                else { mv.EditedByUserName = ""; }

                mvs.Add(mv);
            }
            return View(mvs);
        }

        [HttpPost]
        public IActionResult SecurityCreateUser(string roleId, string uname, string email, string upwd)
        {
            var m = _context.member.SingleOrDefault(mm => mm.mem_username == uname.ToUpper());
            if (m != null)
            {
                return Json(new { result = "dup" });
            }
            else
            {
                member nm = new member();
                nm.member_code = "U" + DateTime.Now.ToString("yyMMddHHmmssfffffff");
                nm.mem_username = uname.ToUpper();
                nm.fname = uname.ToUpper();
                upwd = Utils.GeneratePassword();
                nm.mem_password = Utils.EncodeMd5(upwd);
                nm.mem_role_id = new Guid(roleId);
                nm.email = email;
                _context.Add(nm);
                _context.SaveChanges();

                var memberId = HttpContext.Session.GetString("memberId");
                SecurityMemberRoles nmr = new SecurityMemberRoles();
                nmr.MemberId = nm.id;
                nmr.x_status = "Y";
                nmr.CreatedBy = new Guid(memberId);
                nmr.CreatedDate = DateTime.Now;
                nmr.EditedBy = new Guid(memberId);
                nmr.EditedDate = DateTime.Now;
                _scontext.Add(nmr);
                _scontext.SaveChanges();

                SendEmail(email,uname,upwd);

                return Json(new { result = "success" });
            }

        }

        public IActionResult DetailsPersonal()
        {
            var id = HttpContext.Session.GetString("memberId");
            if (id == null)
            {
                return NotFound();
            }

            member member = _context.member.Single(m => m.id == new Guid(id));
            if (member == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(member.mem_photo))
            {
                //pic_image m = _context.pic_image.Single(i => i.image_code == member.mem_photo);
                //ViewBag.memPhoto = m.image_name;
                ViewBag.memPhoto = member.mem_photo;
            }

            if (!String.IsNullOrEmpty(member.cid_card_pic))
            {
                //pic_image c = _context.pic_image.Single(i => i.image_code == member.cid_card_pic);
                //ViewBag.cidCardPhoto =c.image_name;
                ViewBag.cidCardPhoto = member.cid_card_pic;
            }
            ViewBag.memberId = id;
            ViewBag.TabNoData = "";

            //For dropdown province; we need to manually assign it, then prepare viewbag for javascript
            ViewBag.zipCode = member.zip_code;
            ViewBag.subdistrictCode = member.subdistrict_code;
            ViewBag.districtCode = member.district_code;
            //ViewBag.provinceCode = member.province_code;
            ViewBag.incomeCode = member.income;

            prepareViewBag();
            ViewBag.IsDetailsPersonal = 1;
            ViewBag.IsDisabled = true;
            return View(member);
        }

        private class listTraining
        {
            public string rec_no { get; set; }
            public string code { get; set; }
            public string desc { get; set; }
            public string grade { get; set; }
        }
        private class listInfo
        {
            public string rec_no { get; set; }
            public string desc { get; set; }
        }
        private class listWork
        {
            public string rec_no { get; set; }
            public string company { get; set; }
            public string position { get; set; }
            public string year { get; set; }
            public string address { get; set; }
        }
        private class listProduct
        {
            public string rec_no { get; set; }
            public string productGroup { get; set; }
            public string productType { get; set; }
            public string productDesc { get; set; }
        }
    }

    //Json for javascript to check which tabs are no data; see ListTabNoData action in this class
    public class tab
    {
        public string name;
        public int value;
    }
}
