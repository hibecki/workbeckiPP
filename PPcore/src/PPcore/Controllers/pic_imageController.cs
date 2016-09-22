using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using System.IO;
using System.Drawing;

namespace PPcore.Controllers
{
    public class pic_imageController : Controller
    {
        private readonly PalangPanyaDBContext _context;

        public pic_imageController(PalangPanyaDBContext context)
        {
            _context = context;    
        }

        // GET: pic_image/Details/5
        public async Task<IActionResult> image(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pic_image = await _context.pic_image.SingleOrDefaultAsync(m => m.image_code == id);
            if (pic_image == null)
            {
                return NotFound();
            }

            byte[] imageBytes = Convert.FromBase64String(pic_image.image_file);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            var fileExt = Path.GetExtension(pic_image.image_code);
            FileStreamResult result = new FileStreamResult(ms, "image/"+fileExt);
            result.FileDownloadName = pic_image.image_code;
            return result;
        }




    }
}
