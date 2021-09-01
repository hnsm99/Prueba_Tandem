using Prueba_Tandem.Models.MCita_Medica;
using Prueba_Tandem.Models.MEstado_Paciente;
using Prueba_Tandem.Models.MTriage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Models.MPaciente
{
    public class PacienteIndex
    {
        public decimal Identificacion { get; set; }
        public string Nombre { get; set; }
        public decimal Edad { get; set; }
        public bool Sexo { get; set; }
        public string triages { get; set; }
        public int Triage { get; set; }
        public string Sintomas { get; set; }
        public string estado_Pacientes { get; set; }
        public int Estado { get; set; }
        public List<Cita_MedicaIndex> cita_medica { get; set; }
    }
}
