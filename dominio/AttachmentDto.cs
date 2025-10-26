using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class AttachmentDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; } // "application/vnd.ms-excel", "application/pdf"
        public byte[] Content { get; set; }
    }

}
