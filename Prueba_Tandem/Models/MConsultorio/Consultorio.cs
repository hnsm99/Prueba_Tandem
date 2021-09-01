using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MConsultorio
{
    public class Consultorio
    {
        [Key]
        [Required(ErrorMessage ="El codigo del consultorio es requerido.")]
        public decimal Id { get; set; }
        [Required(ErrorMessage = "El nombre del medico es requerido.")]
        public string Medico { get; set; }
        public bool Estado { get; set; }
    }
}
