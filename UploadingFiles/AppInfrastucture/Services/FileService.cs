using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using UploadingFiles.AppInfrastucture.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace UploadingFiles.AppInfrastucture.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        // Constructor
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public IEnumerable<string> GetImagesList() => Directory.EnumerateFiles(Path.Combine(_environment.WebRootPath, "Uploads\\Images"))
                 .Select(path => Path.GetFileName(path));

        public async Task<List<string>> SaveFiles(List<IFormFile> files)
        {
            var imagesFolder = Path.Combine(_environment.WebRootPath, "Uploads\\Images");
            List<string> wrongTypeList = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var type = formFile.ContentType.Split("/")[1];
                    if (type == "jpeg" || type == "jpg" || type == "gif" || type == "png")
                    {
                        // full path to file in temp location
                        var filePath = Path.GetTempFileName();
                        var uniqueFileName = GetUniqueFileName($"image_{formFile.FileName}");
                        var filePathUploadsImages = Path.Combine(imagesFolder, uniqueFileName);

                        using var stream = new FileStream(filePathUploadsImages, FileMode.Create);
                        await formFile.CopyToAsync(stream);
                    }
                    else
                    {
                        wrongTypeList.Add(formFile.FileName);
                    }
                }
            }
            return wrongTypeList;
        }
    }
}
