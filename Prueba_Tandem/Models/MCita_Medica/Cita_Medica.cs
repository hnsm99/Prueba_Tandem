using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MCita_Medica

{
    public class Cita_Medica
    {
        [Key]
        public int Id { get; set; }
        public decimal Paciente { get; set; }
        public decimal Consultorio { get; set; }
        public string Diagnostico { get; set; }
        public bool Alta { get; set; }
        public bool Virus { get; set; }
        public DateTime Fecha_Prueba { get; set; }


    }
}
