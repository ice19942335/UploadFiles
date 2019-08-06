using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadingFiles.Models;

namespace UploadingFiles.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        // Constructor
        public HomeController(IWebHostEnvironment environment) => _environment = environment;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }


                    var uniqueFileName = GetUniqueFileName($"image_{formFile.FileName}");
                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    var filePath2 = Path.Combine(uploads, uniqueFileName);
                    formFile.CopyTo(new FileStream(filePath2, FileMode.Create));
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePaths });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
