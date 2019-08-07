using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadingFiles.Models
{
    public class UploadFilesViewModel
    {
        public IEnumerable<string> Images { get; set; }
        public List<string> WrongTypeFiles { get; set; }
    }
}
