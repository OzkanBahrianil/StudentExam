using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrate
{
   public class ExtendedLog : ZNetCS.AspNetCore.Logging.EntityFrameworkCore.Log
    {
        public string Browser { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string User { get; set; }
    }
}
