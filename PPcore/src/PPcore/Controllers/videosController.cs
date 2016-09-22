using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace PPcore.Controllers
{
    public class videosController : Controller
    {
        private readonly PalangPanyaDBContext _context;
        private IConfiguration _configuration;
        private IHostingEnvironment _env;

        public videosController(PalangPanyaDBContext context, IConfiguration configuration, IHostingEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        // GET: videos
        public async Task<IActionResult> Index()
        {
            var album = _context.album.Where(a => a.album_type == "V");
            ViewBag.countRecords = album.Count();
            return View(await album.OrderByDescending(m => m.album_date).ToListAsync());
        }

        // GET: albums/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.album.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (album == null)
            {
                return NotFound();
            }
            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            ViewBag.FormAction = "Details";
            ViewBag.album_code = album.album_code;
            ViewBag.appId = appId;
            return View(album);
        }

        // GET: videos/Create
        public IActionResult Create()
        {
            ViewBag.FormAction = "Create";
            ViewBag.album_code = DateTime.Now.ToString("yyMMddhhmmss");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("album_code,album_date,album_desc,album_name,created_by,id,rowversion,x_log,x_note,x_status")] album album)
        {
            album.album_type = "V"; //video
            album.x_status = "Y";
            album.created_by = "Administrator";

            _context.Add(album);
            _context.SaveChanges();

            var pImages = _context.pic_image.Where(p => (p.ref_doc_code == album.album_code) && (p.x_status == "N")).ToList();
            if (pImages != null)
            {
                foreach (var pImage in pImages)
                {
                    pImage.x_status = "Y";
                    _context.Update(pImage);
                }
                _context.SaveChanges();

                var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
                uploads = Path.Combine(uploads, album.album_code);

                DirectoryInfo di = new DirectoryInfo(uploads);
                if (di.Exists)
                {
                    var dest = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
                    dest = Path.Combine(dest, album.album_code);
                    Directory.Move(uploads, dest);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: videos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.album.SingleOrDefaultAsync(m => m.id == new Guid(id));
            if (album == null)
            {
                return NotFound();
            }

            clearImageUpload(album.album_code);

            ViewBag.FormAction = "Edit";
            ViewBag.album_code = album.album_code;
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("album_code,album_date,album_desc,album_name,album_type,created_by,id,rowversion,x_log,x_note,x_status")] album album)
        {
            try
            {
                _context.Update(album);
                _context.SaveChanges();

                var pImages = _context.pic_image.Where(p => (p.ref_doc_code == album.album_code) && (p.x_status == "N")).ToList();
                if (pImages != null)
                {
                    foreach (var pImage in pImages)
                    {
                        pImage.x_status = "Y";
                        _context.Update(pImage);
                    }
                    _context.SaveChanges();

                    var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
                    uploads = Path.Combine(uploads, album.album_code);

                    DirectoryInfo di = new DirectoryInfo(uploads);
                    if (di.Exists)
                    {
                        var dest = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
                        dest = Path.Combine(dest, album.album_code);
                        Directory.CreateDirectory(dest);
                        foreach (FileInfo file in di.GetFiles()) file.CopyTo(Path.Combine(dest, file.Name));
                        di.Delete(true);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!albumExists(album.album_code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteVideo(string albumCode, string imageCode)
        {
            pic_image pi = _context.pic_image.SingleOrDefault(p => p.image_code == imageCode);
            _context.Remove(pi);
            _context.SaveChanges();

            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, imageCode);
            System.IO.File.Delete(uploads);

            return Json(new { result = "success", imageCode = imageCode });
        }

        // POST: videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var album = await _context.album.SingleOrDefaultAsync(m => m.album_code == id);
            _context.album.Remove(album);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var album = await _context.album.SingleOrDefaultAsync(m => m.id == new Guid(id));
            //var albumImage = await _context.album_image
            _context.album.Remove(album);
            await _context.SaveChangesAsync();
            return Json(new { result = "success" });
        }

        private bool albumExists(string id)
        {
            return _context.album.Any(e => e.album_code == id);
        }

        [HttpPost]
        public IActionResult UploadAlbumVideo(ICollection<IFormFile> file, string albumCode)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            uploads = Path.Combine(uploads, albumCode);
            Directory.CreateDirectory(uploads);

            var fileName = ""; var fileExt = ""; var imageCode = "";
            foreach (var fi in file)
            {
                if (fi.Length > 0)
                {
                    fileName += Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(fi.ContentDisposition).FileName.Trim('"');
                    fileExt = Path.GetExtension(fileName);
                    fileName = Path.GetFileNameWithoutExtension(fileName);
                    fileName = fileName.Substring(0, (fileName.Length <= (50 - fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;

                    imageCode = "A" + DateTime.Now.ToString("yyMMddhhmmssfffffff") + fileExt;
                    using (var SourceStream = fi.OpenReadStream())
                    {
                        using (var fileStream = new FileStream(Path.Combine(uploads, imageCode), FileMode.Create))
                        {
                            SourceStream.CopyTo(fileStream);

                            pic_image pic_image = new pic_image();
                            pic_image.image_code = imageCode;
                            pic_image.x_status = "N";
                            pic_image.image_name = fileName;
                            pic_image.image_desc = fileName;
                            pic_image.ref_doc_type = "album";
                            pic_image.ref_doc_code = albumCode;
                            _context.pic_image.Add(pic_image);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            return Json(new { result = "success", uploads = uploads, fileName = fileName });
        }

        [HttpGet]
        public IActionResult ListAlbumVideo(string albumCode)
        {
            album album = _context.album.SingleOrDefault(m => m.album_code == albumCode);
            var abName = album.album_name;
            var abDesc = album.album_desc;

            var fiN = ""; var fiP = "";
            List<photo> ph = new List<photo>();
            var pImages = _context.pic_image.Where(p => (p.ref_doc_code == albumCode) && (p.x_status == "Y")).ToList();
            if (pImages != null)
            {
                foreach (var pImage in pImages)
                {
                    fiN = pImage.image_code;
                    fiP = Path.Combine(albumCode, fiN);
                    fiP = Path.Combine(_configuration.GetSection("Paths").GetSection("images_album").Value, fiP);
                    if (pImage.image_desc == null) { pImage.image_desc = ""; }
                    ph.Add(new photo { albumCode = albumCode, image_code = pImage.image_code, image_desc = pImage.image_desc, fileName = fiN, filePath = fiP, albumName = abName, albumDesc = abDesc });
                }
                string pjson = JsonConvert.SerializeObject(ph);
                return Json(pjson);
            }
            else
            {
                return Json("");
            }
        }

        [HttpGet]
        public IActionResult ChangeVideoDesc(string imageCode, string imageDesc)
        {
            pic_image pi = _context.pic_image.SingleOrDefault(p => p.image_code == imageCode);

            pi.image_desc = imageDesc;
            _context.Update(pi);

            _context.SaveChanges();
            return Json(new { result = "success", imageDesc = imageDesc });
        }

        [HttpGet]
        //public async Task<IActionResult> ShareVideo(string albumCode, string imageCode, string imageDesc, string fileName)
        public IActionResult ShareVideo(string albumCode, string imageCode, string imageDesc, string fileName)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, fileName);

            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;
            var accessToken = _configuration.GetSection("facebook").GetSection("AccessToken").Value;
            var pageId = _configuration.GetSection("facebook").GetSection("PageID").Value;

            //string resp = "";
            string resp2 = "";

            //using (HttpClient client = new HttpClient())
            //using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?access_token=" + accessToken))
            //using (HttpContent content = response.Content)
            //{
            //    resp = await content.ReadAsStringAsync();
            //}

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(imageDesc), "message");
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(uploads));
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("source")
                {
                    FileName = fileName
                };
                form.Add(fileContent);
                var result = client.PostAsync("/v2.6/" + pageId + "/videos?access_token=" + accessToken, form).Result;
                resp2 = result.Content.ReadAsStringAsync().Result;
            }
            return Json(resp2);
        }

        [HttpGet]
        public IActionResult ShareAlbum(string albumCode, string albumName)
        {
            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;
            var accessToken = _configuration.GetSection("facebook").GetSection("AccessToken").Value;
            var pageId = _configuration.GetSection("facebook").GetSection("PageID").Value;

            string resp = "";
            //string resp2 = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", albumName),
                });
                var result = client.PostAsync("/v2.6/" + pageId + "/albums?access_token=" + accessToken, content).Result;
                resp = result.Content.ReadAsStringAsync().Result;
            }
            return Content(resp);
        }
        [HttpGet]
        public IActionResult ShareVideosToAlbum(string albumId, string albumCode, string imageCode, string imageDesc)
        {
            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;
            var accessToken = _configuration.GetSection("facebook").GetSection("AccessToken").Value;
            var pageId = _configuration.GetSection("facebook").GetSection("PageID").Value;

            string resp = "";

            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, imageCode);

            string fiN; string fiP;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");

                fiN = imageCode;
                fiP = uploads;

                var form = new MultipartFormDataContent();
                form.Add(new StringContent(imageDesc), "message");
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(fiP));
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("source")
                {
                    FileName = imageCode
                };
                form.Add(fileContent);
                var result = client.PostAsync("/v2.6/" + albumId + "/videos?access_token=" + accessToken, form).Result;
                resp = result.Content.ReadAsStringAsync().Result;
            }
            return Json(new { albumCode = albumCode, imageCode = imageCode });
        }

        private void clearImageUpload(string albumCode)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_upload").Value);
            uploads = Path.Combine(uploads, albumCode);
            DirectoryInfo di = new DirectoryInfo(uploads);
            if (di.Exists) { di.Delete(true); }

            var pic_images = _context.pic_image.Where(p => (p.ref_doc_code == albumCode) && (p.x_status == "N")).ToList();
            foreach (var p in pic_images)
            {
                _context.pic_image.Remove(p);
                _context.SaveChanges();
            }
        }
    }
}
