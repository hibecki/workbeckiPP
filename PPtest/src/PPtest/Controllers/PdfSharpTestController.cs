using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Drawing;

namespace PPtest.Controllers
{
    public class PdfSharpTestController : Controller
    {
        private IHostingEnvironment _env;

        public PdfSharpTestController(IHostingEnvironment env)
        {
            _env = env;
        }

        public ActionResult TestMigraDoc1()
        {

            Document document = new Document();

            Style style = document.Styles["Normal"];
            style.Font.Name = "Cordia New";
            style.Font.Size = 16;

            document.Info.Author = "PalangPanya";
            document.Info.Subject = "Member Information";
            document.Info.Keywords = "PalangPanya";

            Section section = document.AddSection();

            Paragraph paragraph = section.AddParagraph();

            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

            paragraph.AddFormattedText("Hello,ไทย World!", TextFormat.Bold);
            paragraph.AddFormattedText("ทดสอบการใช้ภาษา", TextFormat.Italic);

            var logoPath = Path.Combine(_env.WebRootPath, "images");
            logoPath = Path.Combine(logoPath, "logo_t.png");
            Image imgLogo = section.AddImage(logoPath);
            imgLogo.Height = "2cm";
            imgLogo.LockAspectRatio = true;

            var memberPicPath = Path.Combine(_env.WebRootPath, "images_member");
            memberPicPath = Path.Combine(memberPicPath, "_2_1365148227.jpg");
            //memberPicPath = Path.Combine(memberPicPath, "_677138-topic-ix-1.jpg");
            Image imgMember = section.AddImage(memberPicPath);
            imgMember.Height = "5cm";
            imgMember.LockAspectRatio = true;
            imgMember.RelativeVertical = RelativeVertical.Line;
            imgMember.RelativeHorizontal = RelativeHorizontal.Margin;
            imgMember.Top = ShapePosition.Top;
            imgMember.Left = ShapePosition.Right;
            imgMember.WrapFormat.Style = WrapStyle.Through;


            document.UseCmykColor = true;
            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            //const string filename = "HelloWorld.pdf";
            //pdfRenderer.PdfDocument.Save(filename);
            MemoryStream m = new MemoryStream();
            pdfRenderer.PdfDocument.Save(m,false);
            return new FileStreamResult(m, "application/pdf");
        }

        public ActionResult TestMigraDocMixPdfSharp()
        {
            PdfDocument doc = new PdfDocument();
            doc.Info.Title = "Member information";
            doc.Info.Author = "PalangPanya";
            doc.Info.Subject = "Member information";
            doc.Info.Keywords = "PalangPanya";

            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.MUH = PdfFontEncoding.Unicode;
            gfx.MFEH = PdfFontEmbedding.Default;
            XFont font = new XFont("Cordia New", 16, XFontStyle.Bold);
            gfx.DrawString("The following paragraph was rendered using ภาษาภ", font, XBrushes.Black,new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);


            XPen pen = new XPen(XColors.Navy, Math.PI);

    gfx.DrawRectangle(pen, 10, 0, 100, 60);
    gfx.DrawRectangle(XBrushes.DarkOrange, 130, 0, 100, 60);
    gfx.DrawRectangle(pen, XBrushes.DarkOrange, 10, 80, 100, 60);
    gfx.DrawRectangle(pen, XBrushes.DarkOrange, 150, 80, 60, 60);





                Document document = new Document();
                Section sec = document.AddSection();

                //Paragraph para = sec.AddParagraph();
                //para.Format.Alignment = ParagraphAlignment.Justify;
                //para.Format.Font.Name = "Times New Roman";
                //para.Format.Font.Size = 12;
                //para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
                //para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
                //para.AddText("Duisism odigna acipsum delesenisl ");
                //para.AddFormattedText("ullum in velenit", TextFormat.Bold);
                //para.AddText(" ipit iurero dolum zzriliquisis nit wis dolore vel et nonsequipit, velendigna " +
                //  "auguercilit lor se dipisl duismod tatem zzrit at laore magna feummod oloborting ea con vel " +
                //  "essit augiati onsequat luptat nos diatum vel ullum illummy nonsent nit ipis et nonsequis " +
                //  "niation utpat. Odolobor augait et non etueril landre min ut ulla feugiam commodo lortie ex " +
                //  "essent augait el ing eumsan hendre feugait prat augiatem amconul laoreet. ≤≥≈≠");
                //para.Format.Borders.Distance = "5pt";
                //para.Format.Borders.Color = Colors.Gold;





            Paragraph paragraph = sec.AddParagraph();

            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

            paragraph.AddFormattedText("Hello,ไทย World!", TextFormat.Bold);
            paragraph.AddFormattedText("ทดสอบการใช้ภาษา", TextFormat.Italic);

            var logoPath = Path.Combine(_env.WebRootPath, "images");
            logoPath = Path.Combine(logoPath, "logo_t.png");
            Image imgLogo = sec.AddImage(logoPath);
            imgLogo.Height = "2cm";
            imgLogo.LockAspectRatio = true;

            var memberPicPath = Path.Combine(_env.WebRootPath, "images_member");
            memberPicPath = Path.Combine(memberPicPath, "_2_1365148227.jpg");
            //memberPicPath = Path.Combine(memberPicPath, "_677138-topic-ix-1.jpg");
            Image imgMember = sec.AddImage(memberPicPath);
            imgMember.Height = "5cm";
            imgMember.LockAspectRatio = true;
            imgMember.RelativeVertical = RelativeVertical.Line;
            imgMember.RelativeHorizontal = RelativeHorizontal.Margin;
            imgMember.Top = ShapePosition.Top;
            imgMember.Left = ShapePosition.Right;
            imgMember.WrapFormat.Style = WrapStyle.Through;



            //document.UseCmykColor = true;
            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            

            //MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
            //MigraDoc.Rendering.DocumentRenderer docRenderer = pdfRenderer.DocumentRenderer;
            //docRenderer.PrepareDocument();

            //docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", paragraph);

            MemoryStream m = new MemoryStream();
            doc.Save(m, false);
            return new FileStreamResult(m, "application/pdf");
        }

        public ActionResult TestPdfSharp1()
        {
            PdfDocument doc = new PdfDocument();
            doc.Info.Title = "Member information";
            doc.Info.Author = "PalangPanya";
            doc.Info.Subject = "Member information";
            doc.Info.Keywords = "PalangPanya";

            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.MUH = PdfFontEncoding.Unicode;
            gfx.MFEH = PdfFontEmbedding.Default;
            XFont font = new XFont("Cordia New", 16, XFontStyle.Bold);
            gfx.DrawString("The following paragraph was rendered using ภาษาภ", font, XBrushes.Black, new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);


            XPen pen = new XPen(XColors.Navy, Math.PI);

            gfx.DrawRectangle(pen, 10, 0, 100, 60);
            gfx.DrawRectangle(XBrushes.DarkOrange, 130, 0, 100, 60);
            gfx.DrawRectangle(pen, XBrushes.DarkOrange, 10, 80, 100, 60);
            gfx.DrawRectangle(pen, XBrushes.DarkOrange, 150, 80, 60, 60);





            Document document = new Document();
            Section sec = document.AddSection();

            Paragraph para = sec.AddParagraph();
            para.Format.Alignment = ParagraphAlignment.Justify;
            para.Format.Font.Name = "Times New Roman";
            para.Format.Font.Size = 12;
            para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
            para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.DarkGray;
            para.AddText("Duisism odigna acipsum delesenisl ");
            para.AddFormattedText("ullum in velenit", TextFormat.Bold);
            para.AddText(" ipit iurero dolum zzriliquisis nit wis dolore vel et nonsequipit, velendigna " +
              "auguercilit lor se dipisl duismod tatem zzrit at laore magna feummod oloborting ea con vel " +
              "essit augiati onsequat luptat nos diatum vel ullum illummy nonsent nit ipis et nonsequis " +
              "niation utpat. Odolobor augait et non etueril landre min ut ulla feugiam commodo lortie ex " +
              "essent augait el ing eumsan hendre feugait prat augiatem amconul laoreet. ≤≥≈≠");
            para.Format.Borders.Distance = "5pt";
            para.Format.Borders.Color = Colors.Gold;





            //Paragraph paragraph = sec.AddParagraph();

            //paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

            //paragraph.AddFormattedText("Hello,ไทย World!", TextFormat.Bold);
            //paragraph.AddFormattedText("ทดสอบการใช้ภาษา", TextFormat.Italic);

            //var logoPath = Path.Combine(_env.WebRootPath, "images");
            //logoPath = Path.Combine(logoPath, "logo_t.png");
            //Image imgLogo = sec.AddImage(logoPath);
            //imgLogo.Height = "2cm";
            //imgLogo.LockAspectRatio = true;

            //var memberPicPath = Path.Combine(_env.WebRootPath, "images_member");
            //memberPicPath = Path.Combine(memberPicPath, "_2_1365148227.jpg");
            ////memberPicPath = Path.Combine(memberPicPath, "_677138-topic-ix-1.jpg");
            //Image imgMember = sec.AddImage(memberPicPath);
            //imgMember.Height = "5cm";
            //imgMember.LockAspectRatio = true;
            //imgMember.RelativeVertical = RelativeVertical.Line;
            //imgMember.RelativeHorizontal = RelativeHorizontal.Margin;
            //imgMember.Top = ShapePosition.Top;
            //imgMember.Left = ShapePosition.Right;
            //imgMember.WrapFormat.Style = WrapStyle.Through;



            //document.UseCmykColor = true;
            //const bool unicode = true;
            //const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            //PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            //pdfRenderer.Document = document;
            //pdfRenderer.RenderDocument();


            //MigraDoc.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(document);
            //MigraDoc.Rendering.DocumentRenderer docRenderer = pdfRenderer.DocumentRenderer;
            //docRenderer.PrepareDocument();

            //docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", paragraph);

            MemoryStream m = new MemoryStream();
            doc.Save(m, false);
            return new FileStreamResult(m, "application/pdf");
        }


    }

}