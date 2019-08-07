using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UploadingFiles.AppInfrastucture.Services.Interfaces
{
    public interface IFileService
    {
        public string GetUniqueFileName(string fileName);
        public IEnumerable<string> GetImagesList();
        public void SaveFiles(List<IFormFile> files);
    }
}
