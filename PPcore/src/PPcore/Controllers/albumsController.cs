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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace PPcore.Controllers
{
    public class albumsController : Controller
    {
        private readonly PalangPanyaDBContext _context;
        private IConfiguration _configuration;
        private IHostingEnvironment _env;

        public albumsController(PalangPanyaDBContext context, IConfiguration configuration, IHostingEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        // GET: albums
        public async Task<IActionResult> Index()
        {
            var album = _context.album.Where(a => a.album_type == "P");
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

        // GET: albums/Create
        public IActionResult Create()
        {
            ViewBag.FormAction = "Create";
            ViewBag.album_code = DateTime.Now.ToString("yyMMddhhmmss");
            return View();
        }

        // POST: albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("album_code,album_desc,album_name,created_by,album_date,rowversion")] album album)
        {
            album.album_type = "P"; //pictures
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

        // GET: albums/Edit/5
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

        // POST: albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("album_code,album_desc,album_name,album_type,created_by,album_date,id,rowversion,x_log,x_note,x_status")] album album)
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

        // Get: albums/Delete/5
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
        public IActionResult UploadAlbumPhoto(ICollection<IFormFile> file, string albumCode)
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
                    fileName = fileName.Substring(0, (fileName.Length <= (50-fileExt.Length) ? fileName.Length : (50 - fileExt.Length))) + fileExt;
                    
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
        public IActionResult ListAlbumPhoto(string albumCode)
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
        public async Task<IActionResult> SharePhotoOld(string albumCode, string imageCode, string fileName)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, fileName);

            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;

            string resp = "";
            string resp2 = "";

            /**
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://graph.facebook.com/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("v2.6/me?fields=id,name");
                if (response.IsSuccessStatusCode)
                {
                    resp = await response.Content.ReadAsAsync<String>();
                }
            }
            **/

            //using (HttpResponseMessage response = await client.GetAsync("http://graph.facebook.com/v2.6/me?fields=id,name"))
            //https://graph.facebook.com/endpoint?key=value&amp;access_token=app_id|app_secret

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/oauth/access_token?client_id=" + appId + "&client_secret=" + appSecret + "&grant_type=client_credentials"))
            using (HttpContent content = response.Content)
            {
                resp = await content.ReadAsStringAsync();
            }
            //JToken res = JObject.Parse(resp);
            //FBUserID=126518931100981
            //var token = res.SelectToken("access_token"); //using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?fields=id,name&access_token=" + token))
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?" + resp))
            using (HttpContent content = response.Content)
            {
                resp2 = await content.ReadAsStringAsync();
            }

            //return Json(new { result = "success" });
            return Json(resp);
        }

        [HttpGet]
        public async Task<IActionResult> SharePhotoX(string access_token, string albumCode, string imageCode, string fileName)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, fileName);

            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;

            string resp = "";
            string resp2 = "";
            //using (HttpResponseMessage response = await client.GetAsync("http://graph.facebook.com/v2.6/me?fields=id,name"))
            //https://graph.facebook.com/endpoint?key=value&amp;access_token=app_id|app_secret

            //FBUserID=126518931100981
            //var token = res.SelectToken("access_token"); //using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?fields=id,name&access_token=" + token))
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?access_token=" + access_token))
            using (HttpContent content = response.Content)
            {
                resp = await content.ReadAsStringAsync();
            }

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://graph.facebook.com");
            //     var content = new FormUrlEncodedContent(new[]
            //   {
            //      new KeyValuePair<string, string>("message", fileName),
            //       //new KeyValuePair<string, string>("url", "https://cdn.theatlantic.com/assets/media/img/photo/2015/11/images-from-the-2016-sony-world-pho/s01_130921474920553591/main_1500.jpg")
            //       new KeyValuePair<string, string>("source", "https://cdn.theatlantic.com/assets/media/img/photo/2015/11/images-from-the-2016-sony-world-pho/s01_130921474920553591/main_1500.jpg")
            //   });
            //    var result = client.PostAsync("/v2.6/me/photos?access_token=" + access_token, content).Result;
            //    resp2 = result.Content.ReadAsStringAsync().Result;
            //}


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(fileName), "message");
                var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(uploads));
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("source")
                {
                    FileName = fileName
                };
                form.Add(fileContent);
                var result = client.PostAsync("/v2.6/me/photos?access_token=" + access_token, form).Result;
                resp2 = result.Content.ReadAsStringAsync().Result;
            }

            //return Json(new { result = "success" });
            return Json(resp2);
        }

        [HttpGet]
        public IActionResult ShareAlbumX(string access_token, string albumCode, string albumName)
        {
            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;

            string resp = "";
            string resp2 = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", albumName),
                });
                var result = client.PostAsync("/v2.6/me/albums?access_token=" + access_token, content).Result;
                resp = result.Content.ReadAsStringAsync().Result;
            }
            List<photo> p = new List<photo>();
            if (!String.IsNullOrEmpty(resp))
            {
                dynamic respJson = JsonConvert.DeserializeObject(resp);
                var albumId = respJson.id;


                album album = _context.album.SingleOrDefault(m => m.album_code == albumCode);
                var abName = album.album_name;
                var abDesc = album.album_desc;

                var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
                uploads = Path.Combine(uploads, albumCode);

                string[] fileEntries = Directory.GetFiles(uploads);
                string fiN; string fiP;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://graph.facebook.com");
                    foreach (string fileName in fileEntries)
                    {
                        fiN = Path.GetFileName(fileName);
                        fiP = Path.Combine(uploads, fiN);

                        var form = new MultipartFormDataContent();
                        form.Add(new StringContent(fileName), "message");
                        var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(fiP));
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("source")
                        {
                            FileName = fileName
                        };
                        form.Add(fileContent);
                        var result = client.PostAsync("/v2.6/" + albumId + "/photos?access_token=" + access_token, form).Result;
                        resp2 = result.Content.ReadAsStringAsync().Result;


                        p.Add(new photo { albumCode = albumCode, image_code = "", fileName = fiN, filePath = fiP, albumName = abName, albumDesc = abDesc });
                    }
                }



            }
            string pjson = JsonConvert.SerializeObject(p);
            return Json(pjson);
        }

        [HttpGet]
        public async Task<IActionResult> SharePhoto(string albumCode, string imageCode, string imageDesc, string fileName)
        {
            var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
            uploads = Path.Combine(uploads, albumCode);
            uploads = Path.Combine(uploads, fileName);

            var appId = _configuration.GetSection("facebook").GetSection("AppId").Value;
            var appSecret = _configuration.GetSection("facebook").GetSection("AppSecret").Value;
            var accessToken = _configuration.GetSection("facebook").GetSection("AccessToken").Value;
            var pageId = _configuration.GetSection("facebook").GetSection("PageID").Value;

            string resp = "";
            string resp2 = "";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v2.6/me?access_token=" + accessToken))
            using (HttpContent content = response.Content)
            {
                resp = await content.ReadAsStringAsync();
            }




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
                var result = client.PostAsync("/v2.6/" + pageId + "/photos?access_token=" + accessToken, form).Result;
                resp2 = result.Content.ReadAsStringAsync().Result;
            }

            //return Json(new { result = "success" });
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

            /**
            List<photo> p = new List<photo>();
            if (!String.IsNullOrEmpty(resp))
            {
                dynamic respJson = JsonConvert.DeserializeObject(resp);
                var albumId = respJson.id;


                album album = _context.album.SingleOrDefault(m => m.album_code == albumCode);
                var abName = album.album_name;
                var abDesc = album.album_desc;

                var uploads = Path.Combine(_env.WebRootPath, _configuration.GetSection("Paths").GetSection("images_album").Value);
                uploads = Path.Combine(uploads, albumCode);

                string[] fileEntries = Directory.GetFiles(uploads);
                string fiN; string fiP;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://graph.facebook.com");
                    foreach (string fileName in fileEntries)
                    {
                        fiN = Path.GetFileName(fileName);
                        fiP = Path.Combine(uploads, fiN);

                        var form = new MultipartFormDataContent();
                        form.Add(new StringContent(fileName), "message");
                        var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(fiP));
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("source")
                        {
                            FileName = fileName
                        };
                        form.Add(fileContent);
                        var result = client.PostAsync("/v2.6/" + albumId + "/photos?access_token=" + accessToken, form).Result;
                        resp2 = result.Content.ReadAsStringAsync().Result;


                        p.Add(new photo { albumCode = albumCode, image_code = "", fileName = fiN, filePath = fiP, albumName = abName, albumDesc = abDesc });
                    }
                }
            }
            string pjson = JsonConvert.SerializeObject(p);
            return Json(pjson);
            **/

        }
        [HttpGet]
        public IActionResult SharePhotoToAlbum(string albumId, string albumCode, string imageCode, string imageDesc)
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
                    var result = client.PostAsync("/v2.6/" + albumId + "/photos?access_token=" + accessToken, form).Result;
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

        [HttpGet]
        public IActionResult ChangePhotoDesc(string imageCode, string imageDesc)
        {
            pic_image pi = _context.pic_image.SingleOrDefault(p => p.image_code == imageCode);

            pi.image_desc = imageDesc;
            _context.Update(pi);
        
            _context.SaveChanges();
            return Json(new { result = "success", imageDesc = imageDesc });
        }

        [HttpGet]
        public IActionResult DeletePhoto(string albumCode, string imageCode)
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
    }


    public class photo
    {
        public string albumCode;
        public string image_code;
        public string image_desc;
        public string fileName;
        public string filePath;

        public string albumName;
        public string albumDesc;
    }
}
