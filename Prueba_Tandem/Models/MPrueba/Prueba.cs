using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MPrueba
{
    public class Prueba
    {
        [Key]
        public int Id { get; set; }
        public int Cita_Medica { get; set; }
        
        [Required(ErrorMessage ="El resultado es obligatorio")]
        public string Resultado { get; set; }
        [NotMapped]
        public string Helice1 { get; set; }
        [NotMapped]
        public string Helice2 { get; set; }
        [NotMapped]
        public IFormFile Archivo { get; set; }
        [NotMapped]
        public string FileName { get; set; }
    }
}
