using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadingFiles.AppInfrastucture.Services.Interfaces
{
    public interface IFileService
    {
        public string GetUniqueFileName(string fileName);
    }
}
