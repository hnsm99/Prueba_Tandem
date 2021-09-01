using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MPaciente
{
    public class Paciente
    {
        [Key]
        [Required(ErrorMessage ="La identificación es obligatoria.")]
        public decimal Identificacion { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La edad es obligatoria.")]
        public decimal Edad { get; set; }
        [Required(ErrorMessage = "El Sexo es obligatorio.")]
        public bool Sexo { get; set; }
        [Required(ErrorMessage = "El Triage es obligatorio.")]
        public int Triage { get; set; }
        [Required(ErrorMessage = "Los sintomas son obligatorios.")]
        public string Sintomas { get; set; }
        public int Estado { get; set; }
    }
}
