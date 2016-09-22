using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace PPtest.Controllers
{
    public class HomeController : Controller
    {
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

        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult DetailsPdfSrc()
        {
            return View();
        }

        public IActionResult iTextFontList()
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 50, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;


            int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            StringBuilder sb = new StringBuilder();
            foreach (string fontname in FontFactory.RegisteredFonts)
            {
                sb.Append(fontname + "\n");
            }

            //Font arial = FontFactory.GetFont("Arial", 28, Color.GRAY);
            //Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLDITALIC, new Color(125, 88, 15));
            //Font palatino = FontFactory.GetFont(
            // "palatino linotype italique",
            //  BaseFont.CP1252,
            //  BaseFont.EMBEDDED,
            //  10,
            //  Font.ITALIC,
            //  Color.GREEN
            //  );
            //Font smallfont = FontFactory.GetFont("Arial", 7);
            //Font x = FontFactory.GetFont("nina fett");
            //x.Size = 10;
            //x.SetStyle("Italic");
            //x.SetColor(100, 50, 200);

            document.Open();

            document.Add(new Paragraph("All Fonts:\n" + sb.ToString()));

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");


        }

        public async Task<IActionResult> iTextDetailsHtmlToPdf()
        {
            var htmlString = "";
            var urlString = Request.Scheme + "://" + Request.Host + Url.Action("DetailsPdfSrc", "Home");

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(urlString))
            using (HttpContent content = response.Content)
            {
                htmlString = await content.ReadAsStringAsync();
            }

            ////Create a byte array that will eventually hold our final PDF
            //Byte[] bytes;

            ////Boilerplate iTextSharp setup here
            ////Create a stream that we can write to, in this case a MemoryStream
            //MemoryStream ms;
            //using (ms = new MemoryStream())
            //{

            //    //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
            //    using (var doc = new Document())
            //    {

            //        //Create a writer that's bound to our PDF abstraction and our stream
            //        using (var writer = PdfWriter.GetInstance(doc, ms))
            //        {

            //            //Open the document for writing
            //            doc.Open();

            //            //Our sample HTML and CSS
            //            var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
            //            var example_css = @".headline{font-size:200%}";

            //            /**************************************************
            //             * Example #1                                     *
            //             *                                                *
            //             * Use the built-in HTMLWorker to parse the HTML. *
            //             * Only inline CSS is supported.                  *
            //             * ************************************************/

            //            //Create a new HTMLWorker bound to our document
            //            //using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
            //            //{

            //            //    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
            //            //    using (var sr = new StringReader(example_html))
            //            //    {

            //            //        //Parse the HTML
            //            //        htmlWorker.Parse(sr);
            //            //    }
            //            //}

            //            /**************************************************
            //             * Example #2                                     *
            //             *                                                *
            //             * Use the XMLWorker to parse the HTML.           *
            //             * Only inline CSS and absolutely linked          *
            //             * CSS is supported                               *
            //             * ************************************************/

            //            //XMLWorker also reads from a TextReader and not directly from a string
            //            using (var srHtml = new StringReader(example_html))
            //            {

            //                //Parse the HTML
            //                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
            //            }

            //            /**************************************************
            //             * Example #3                                     *
            //             *                                                *
            //             * Use the XMLWorker to parse HTML and CSS        *
            //             * ************************************************/

            //            //In order to read CSS as a string we need to switch to a different constructor
            //            //that takes Streams instead of TextReaders.
            //            //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
            //            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
            //            {
            //                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
            //                {

            //                    //Parse the HTML
            //                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
            //                }
            //            }


            //            doc.Close();
            //        }
            //    }

            //    //After all of the PDF "stuff" above is done and closed but **before** we
            //    //close the MemoryStream, grab all of the active bytes from the stream
            //    bytes = ms.ToArray();
            //}

            //Now we just need to do something with those bytes.
            //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
            //You could also write the bytes to a database in a varbinary() column (but please don't) or you
            //could pass them to another function for further PDF processing.

            //var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            //System.IO.File.WriteAllBytes(testFile, bytes);

            //MemoryStream workStream = new MemoryStream();
            //Document document = new Document(PageSize.A4, 25, 25, 50, 30);
            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            //byte[] byteInfo = workStream.ToArray();
            //workStream.Write(byteInfo, 0, byteInfo.Length);
            //workStream.Position = 0;

            //return new FileStreamResult(workStream, "application/pdf");

            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        //var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
                        var example_css = @".headline{font-size:200%}";
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlString)))
                            {
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }


            MemoryStream m = new MemoryStream(bytes);
            return new FileStreamResult(m, "application/pdf");

            //return Content(htmlString);
        }

        public IActionResult iTextDetailsPdfSample()
        {
            var htmlString = "";
            var urlString = Request.Scheme + "://" + Request.Host + Url.Action("DetailsPdfSrc", "Home");



            //Create a byte array that will eventually hold our final PDF
            Byte[] bytes;

            //Boilerplate iTextSharp setup here
            //Create a stream that we can write to, in this case a MemoryStream
            using (var ms = new MemoryStream())
            {

                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new Document())
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {

                        //Open the document for writing
                        doc.Open();

                        //Our sample HTML and CSS
                        var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
                        var example_css = @".headline{font-size:200%}";

                        /**************************************************
                         * Example #1                                     *
                         *                                                *
                         * Use the built-in HTMLWorker to parse the HTML. *
                         * Only inline CSS is supported.                  *
                         * ************************************************/

                        //Create a new HTMLWorker bound to our document
                        //using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        //{

                        //    //HTMLWorker doesn't read a string directly but instead needs a TextReader (which StringReader subclasses)
                        //    using (var sr = new StringReader(example_html))
                        //    {

                        //        //Parse the HTML
                        //        htmlWorker.Parse(sr);
                        //    }
                        //}

                        /**************************************************
                         * Example #2                                     *
                         *                                                *
                         * Use the XMLWorker to parse the HTML.           *
                         * Only inline CSS and absolutely linked          *
                         * CSS is supported                               *
                         * ************************************************/

                        //XMLWorker also reads from a TextReader and not directly from a string
                        using (var srHtml = new StringReader(example_html))
                        {

                            //Parse the HTML
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        }

                        /**************************************************
                         * Example #3                                     *
                         *                                                *
                         * Use the XMLWorker to parse HTML and CSS        *
                         * ************************************************/

                        //In order to read CSS as a string we need to switch to a different constructor
                        //that takes Streams instead of TextReaders.
                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {

                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }


                        doc.Close();
                    }
                }

                //After all of the PDF "stuff" above is done and closed but **before** we
                //close the MemoryStream, grab all of the active bytes from the stream
                bytes = ms.ToArray();
            }

            //Now we just need to do something with those bytes.
            //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
            //You could also write the bytes to a database in a varbinary() column (but please don't) or you
            //could pass them to another function for further PDF processing.
            var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            System.IO.File.WriteAllBytes(testFile, bytes);





            return Content(htmlString);
        }

        public IActionResult iTextDetailsPdf()
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 50, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            //PdfWriter p = PdfWriter.GetInstance(document, workStream);
            var logoPath = Path.Combine(_env.WebRootPath, "images");
            logoPath = Path.Combine(logoPath, "logo_t.png");
            Image logo = Image.GetInstance(logoPath);
            logo.ScalePercent(50);
            //png.ScaleToFit(140f, 120f);
            //png.SpacingBefore = 10f;
            //png.SpacingAfter = 1f;
            logo.Alignment = Element.ALIGN_CENTER;

            var memberPicPath = Path.Combine(_env.WebRootPath, "images_member");
            memberPicPath = Path.Combine(memberPicPath, "_2_1365148227.jpg");
            //memberPicPath = Path.Combine(memberPicPath, "_677138-topic-ix-1.jpg");

            Image memberPic = Image.GetInstance(memberPicPath);
            memberPic.ScaleToFit(120f, 120f);
            memberPic.Border = Rectangle.BOX;
            memberPic.BorderColor = BaseColor.DARK_GRAY;
            memberPic.BorderWidth = 1f;
            //var w = memberPic.ScaledWidth / 2;
            memberPic.SetAbsolutePosition(document.PageSize.Width - 30f - memberPic.ScaledWidth, document.PageSize.Height - 130f);
            //png.ScaleToFit(140f, 120f);
            //png.SpacingBefore = 10f;
            //png.SpacingAfter = 1f;
            memberPic.Alignment = Element.ALIGN_RIGHT;

            Rectangle rect = new Rectangle(30, 36, 565, 706);
            rect.Border = Rectangle.BOX;
            rect.BorderColor = BaseColor.DARK_GRAY;
            rect.BorderWidth = 1f;


            document.Open();
            document.Add(rect);
            document.Add(logo); document.Add(memberPic);


            document.Add(new Paragraph("ข้อมูลตามบัตรประชาชน"));
            document.Add(new Paragraph(DateTime.Now.ToString()));

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");


        }

        public IActionResult iTextDetailsPdfTest()
        {
            //BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/Fonts/THSarabunNew.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //Font defaultFont = new Font(bf, 12);
            //Font header = new Font(bf, 14, Font.BOLD);
            //Font headTB = new Font(bf, 12, Font.BOLD);

            //Font arial = FontFactory.GetFont("Arial", 28, Color.GRAY);
            //Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLDITALIC, new Color(125, 88, 15));
            //Font palatino = FontFactory.GetFont(
            // "palatino linotype italique",
            //  BaseFont.CP1252,
            //  BaseFont.EMBEDDED,
            //  10,
            //  Font.ITALIC,
            //  Color.GREEN
            //  );
            //Font smallfont = FontFactory.GetFont("Arial", 7);
            //Font x = FontFactory.GetFont("nina fett");
            //x.Size = 10;
            //x.SetStyle("Italic");
            //x.SetColor(100, 50, 200);

            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 50, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            //PdfWriter p = PdfWriter.GetInstance(document, workStream);
            var logoPath = Path.Combine(_env.WebRootPath, "images");
            logoPath = Path.Combine(logoPath, "logo_t.png");
            Image logo = Image.GetInstance(logoPath);
            logo.ScalePercent(50);
            //png.ScaleToFit(140f, 120f);
            //png.SpacingBefore = 10f;
            //png.SpacingAfter = 1f;
            logo.Alignment = Element.ALIGN_CENTER;

            var memberPicPath = Path.Combine(_env.WebRootPath, "images_member");
            memberPicPath = Path.Combine(memberPicPath, "_2_1365148227.jpg");
            //memberPicPath = Path.Combine(memberPicPath, "_677138-topic-ix-1.jpg");

            Image memberPic = Image.GetInstance(memberPicPath);
            memberPic.ScaleToFit(120f, 120f);
            memberPic.Border = Rectangle.BOX;
            memberPic.BorderColor = BaseColor.DARK_GRAY;
            memberPic.BorderWidth = 1f;
            //var w = memberPic.ScaledWidth / 2;
            memberPic.SetAbsolutePosition(document.PageSize.Width - 30f - memberPic.ScaledWidth, document.PageSize.Height - 130f);
            //png.ScaleToFit(140f, 120f);
            //png.SpacingBefore = 10f;
            //png.SpacingAfter = 1f;
            memberPic.Alignment = Element.ALIGN_RIGHT;

            Rectangle rect = new Rectangle(30, 36, 565, 706);
            rect.Border = Rectangle.BOX;
            rect.BorderColor = BaseColor.DARK_GRAY;
            rect.BorderWidth = 1f;


            //int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            //StringBuilder sb = new StringBuilder();
            //foreach (string fontname in FontFactory.RegisteredFonts)
            //{
            //    sb.Append(fontname + "\n");
            //}

            //Font cn = FontFactory.GetFont("CordiaNew");
            //cn.Size = 16;
            //cn.SetStyle("Italic");
            //cn.SetColor(100, 50, 200);
            //FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");

            var fontPath = Path.Combine(_env.WebRootPath, "fonts/THSarabunNew.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font cn = new Font(bf, 16);
            Font cnb = new Font(bf, 16, Font.BOLD);
            Font cni = new Font(bf, 16, Font.ITALIC);

            //FontFactory.RegisterDirectory(Environment.GetEnvironmentVariable("SystemRoot")+"\\Fonts");

            //Font cn = FontFactory.GetFont("cordianew", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 16);
            //Font cnb = FontFactory.GetFont("cordianew-bold",BaseFont.IDENTITY_H,BaseFont.EMBEDDED,16);
            //Font cni = FontFactory.GetFont("cordianew-italic", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 16);
            //Font cnbi = FontFactory.GetFont("cordianew-bolditalic", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 16);




            document.Open();
            document.Add(rect);
            document.Add(logo); document.Add(memberPic);

            var fname = "รุ่งนภา"; var lname = "ใจสว่าง"; var sex = "หญิง"; var nationality = "ไทย";
            var birthdate = "10 มกราคม พ.ศ.2524"; var current_age = "35";
            var cid_card = "1409900XXXXXX"; var marry_status = "โสด"; var religion = "พุทธ";
            var tel = "04-281-XXXX"; var mobile = "08-2110-XXXX";
            var fax = "04-281-XXXX"; var email = "roongnapa2016@gmail.co.th";
            var social_app_data = "Line ID: roongnapa2016, Facebook: roongnapa2016";
            var texta_address = "55/261 โครงการสุโขทัย อเวนิว 99 ถ.บอนด์สตรีท";
            var textb_address = "ต.บางพูด อ.ปากเกร็ด จ.นนทบุรี 11120";
            //var textc_address = "";

            List<listTraining> train = new List<listTraining>();
            train.Add(new listTraining { rec_no = "1", code = "10110", desc = "การแปรรูปผลิตภัณฑ์", grade = "Good" });

            List<listInfo> visit = new List<listInfo>();
            visit.Add(new listInfo { rec_no = "1", desc = "โครงการดูงานการส่งออกสินค้าต่างประเทศ ที่ประเทศมาเลเซีย" });

            List<listInfo> social = new List<listInfo>();
            social.Add(new listInfo { rec_no = "1", desc = "ปลูกป่าที่จังหวัดเชียงใหม่" });
            social.Add(new listInfo { rec_no = "2", desc = "ให้ความรู้เกี่ยวกับการเกษตรแบบเศรษฐกิจพอเพียงให้กับศูนย์ส่งเสริมการเกษตร จังหวัดน่าน" });

            List<listInfo> reward = new List<listInfo>();
            reward.Add(new listInfo { rec_no = "1", desc = "รางวัลโครงการผู้นำตัวอย่างเศรษฐกิจพอเพียง ปี พ.ศ.2558" });

            List<listInfo> education = new List<listInfo>();
            education.Add(new listInfo { rec_no = "1", desc = "ระดับมัธยมศึกษา โรงเรียนหอการค้า สาขาพาณิชยกรรม" });
            education.Add(new listInfo { rec_no = "2", desc = "ระดับอุดมศึกษา มหาวิทยาลัยสุโขทัยธรรมมาธิราช สาขาการเกษตร" });

            var medical_history = "หอบหืด"; var blood_group = "โอ"; var restrict_food = "อาหารทะเล"; 
            var hobby = "เพาะพันธุ์กล้วยไม้"; var special_skill = "เพาะพันธุ์กล้วยไม้หายาก";

            List<listWork> work = new List<listWork>();
            work.Add(new listWork { rec_no = "1", company = "บริษัท เกษตรพัฒนา (Kasadpattana Co.,Ltd.)", position = "วิทยากร (Lecturer)", year = "พ.ศ.2559", address = "55/621 โครงการสุโขทัย อเวนิว 99 ถ.บอนด์สตรีท ต.บางพูด อ.ปากเกร็ด จ.นนทบุรี 11120" });


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
            //PdfPCell cell = new PdfPCell(new Phrase("ข้อมูลตามบัตรประชาชน", cnb));
            //cell.Colspan = 3;
            //cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell);

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
            memberInfoRow3.SetWidths(new float[] { 78f, 77f, 40f, 25f, 25f, 50f });
            memberInfoRow3.AddCell(new PdfPCell(new Phrase("หมายเลขบัตรประชาชน", cnb)) { Border = Rectangle.NO_BORDER });
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
            memberInfoRow5.AddCell(new PdfPCell(new Phrase("แฟ็กส์", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase(fax, cni)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase("อีเมล", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow5.AddCell(new PdfPCell(new Phrase(email, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow5); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 6
            table.AddCell("");
            PdfPTable memberInfoRow6 = new PdfPTable(2);
            memberInfoRow6.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow6.TotalWidth = 400f;
            memberInfoRow6.LockedWidth = true;
            memberInfoRow6.DefaultCell.VerticalAlignment = 1;
            memberInfoRow6.SetWidths(new float[] { 30f, 285f });
            memberInfoRow6.AddCell(new PdfPCell(new Phrase("Social", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow6.AddCell(new PdfPCell(new Phrase(social_app_data, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow6); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 7
            table.AddCell("");
            PdfPTable memberInfoRow7 = new PdfPTable(2);
            memberInfoRow7.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow7.TotalWidth = 400f;
            memberInfoRow7.LockedWidth = true;
            memberInfoRow7.DefaultCell.VerticalAlignment = 1;
            memberInfoRow7.SetWidths(new float[] { 30f, 285f });
            memberInfoRow7.AddCell(new PdfPCell(new Phrase("ที่อยู่", cnb)) { Border = Rectangle.NO_BORDER });
            memberInfoRow7.AddCell(new PdfPCell(new Phrase(texta_address, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow7); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);
            //Member Info Row 8
            table.AddCell("");
            PdfPTable memberInfoRow8 = new PdfPTable(2);
            memberInfoRow8.DefaultCell.Border = Rectangle.NO_BORDER;
            memberInfoRow8.TotalWidth = 400f;
            memberInfoRow8.LockedWidth = true;
            memberInfoRow8.DefaultCell.VerticalAlignment = 1;
            memberInfoRow8.SetWidths(new float[] { 30f, 285f });
            memberInfoRow8.AddCell("");
            memberInfoRow8.AddCell(new PdfPCell(new Phrase(textb_address, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(memberInfoRow8); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

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
                    PdfPTable rowz = new PdfPTable(7);
                    //rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.TotalWidth = 400f; rowz.LockedWidth = true;
                    rowz.SetWidths(new float[] { 8f, 12f, 30f, 22f, 70f, 27f, 40f });
                    rowz.AddCell(new PdfPCell(new Phrase(train[0].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("รหัส", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(train[0].code, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("หลักสูตร", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(train[0].desc, cni)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ระดับเกรด", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(train[0].grade, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
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
                    PdfPTable rowz = new PdfPTable(2);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 8f, 180f });
                    rowz.AddCell(new PdfPCell(new Phrase(visit[0].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(visit[0].desc, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
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
            if (social.Count > 0) {
                for (var i = 0; i < social.Count; i++) {
                    PdfPTable rowz = new PdfPTable(2);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 8f, 180f });
                    rowz.AddCell(new PdfPCell(new Phrase(social[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(social[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
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
                    PdfPTable rowz = new PdfPTable(2);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 8f, 180f });
                    rowz.AddCell(new PdfPCell(new Phrase(reward[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(reward[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
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
                    PdfPTable rowz = new PdfPTable(2);
                    rowz.DefaultCell.Border = Rectangle.NO_BORDER;
                    rowz.TotalWidth = 400f;
                    rowz.LockedWidth = true;
                    rowz.DefaultCell.VerticalAlignment = 1;
                    rowz.SetWidths(new float[] { 8f, 180f });
                    rowz.AddCell(new PdfPCell(new Phrase(education[i].rec_no + ".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(education[i].desc, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
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
            healthInfoRow2.AddCell(new PdfPCell(new Phrase("งานอดิเรก", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow2.AddCell(new PdfPCell(new Phrase(hobby, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow2); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
            table.AddCell(cell);

            //Health Info Row 3
            table.AddCell("");
            PdfPTable healthInfoRow3 = new PdfPTable(4);
            healthInfoRow3.DefaultCell.Border = Rectangle.NO_BORDER;
            healthInfoRow3.TotalWidth = 400f;
            healthInfoRow3.LockedWidth = true;
            healthInfoRow3.DefaultCell.VerticalAlignment = 1;
            healthInfoRow3.SetWidths(new float[] { 30f, 50f, 45f, 70f });
            healthInfoRow3.AddCell(new PdfPCell(new Phrase("อาหารที่แพ้", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow3.AddCell(new PdfPCell(new Phrase(restrict_food, cni)) { Border = Rectangle.NO_BORDER });
            healthInfoRow3.AddCell(new PdfPCell(new Phrase("ความสามารถพิเศษ", cnb)) { Border = Rectangle.NO_BORDER });
            healthInfoRow3.AddCell(new PdfPCell(new Phrase(special_skill, cni)) { Border = Rectangle.NO_BORDER });
            cell = new PdfPCell(healthInfoRow3); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
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
                    rowz.SetWidths(new float[] { 8f,40f, 150f });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].rec_no+".", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ชื่อสถานที่ทำงาน", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].company, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);

                    if(i == 0)
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
                    rowz.SetWidths(new float[] { 8f,20f,90f, 25f,40f });
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
                    rowz.SetWidths(new float[] { 10f, 18f, 200f});
                    rowz.AddCell(new PdfPCell(new Phrase(" ", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase("ที่อยู่", cnb)) { Border = Rectangle.NO_BORDER });
                    rowz.AddCell(new PdfPCell(new Phrase(work[i].address, cni)) { Border = Rectangle.NO_BORDER });
                    cell = new PdfPCell(rowz); cell.HorizontalAlignment = 0; cell.Border = Rectangle.NO_BORDER; cell.PaddingLeft = 30f;
                    table.AddCell(cell);
                    table.AddCell("");
                }
            }





            document.Add(table);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");


        }
    }
}
