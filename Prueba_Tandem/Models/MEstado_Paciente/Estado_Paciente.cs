using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MEstado_Paciente
{
    public class Estado_Paciente
    {
        [Key]
        public int Id { get; set; }
        public string Estado { get; set; }
    }
}
