using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MTriage
{
    public class Triage
    {
        [Key]
        public int Id { get; set; }
        public string Nombre_Urgencia { get; set; }
    }
}
