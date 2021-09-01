using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models
{
    public class UploadModel
    {
        public IFormFile Clientes { get; set; }
        public IFormFile Consultorios { get; set; }
        [NotMapped]
        public string FilepathClientes { get; set; }
        [NotMapped]
        public string FilepathConsultorio { get; set; }
    }
}
