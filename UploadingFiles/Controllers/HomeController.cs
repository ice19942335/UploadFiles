using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadingFiles.AppInfrastucture.Services.Interfaces;
using UploadingFiles.Models;

namespace UploadingFiles.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        // Constructor
        public HomeController(IWebHostEnvironment environment, IFileService fileService)
        {
            _environment = environment;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadFiles()
        {
            var images = Directory.EnumerateFiles(Path.Combine(_environment.WebRootPath, "Uploads\\Images"))
                .Select(path => Path.GetFileName(path));
            
            var viewModel = new UploadFilesViewModel
            {
                Images = images,
                WrongTypeFiles = new List<string>()
            };

            return View(viewModel);
        }

        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            List<string> wrongTypeFile = await _fileService.SaveFiles(files);

            if (wrongTypeFile.Count > 0)
            {
                var viewModel = new UploadFilesViewModel { Images = _fileService.GetImagesList(), WrongTypeFiles = wrongTypeFile };
                return View("UploadFiles", viewModel);
            }
            else
            {
                var viewModel = new UploadFilesViewModel { Images = _fileService.GetImagesList(), WrongTypeFiles = new List<string>() };
                return View("UploadFiles", viewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
