using Microsoft.EntityFrameworkCore;
using Prueba_Tandem.Models.MCita_Medica;
using Prueba_Tandem.Models.MConsultorio;
using Prueba_Tandem.Models.MEstado_Paciente;
using Prueba_Tandem.Models.MPaciente;
using Prueba_Tandem.Models.MPrueba;
using Prueba_Tandem.Models.MTriage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Tandem.Context
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Cita_Medica> cita_medica { get; set; }
        public DbSet<Consultorio> consultorio { get; set; }
        public DbSet<Paciente> paciente { get; set; }
        public DbSet<Prueba> prueba { get; set; }
        public DbSet<Triage> triage { get; set; }
        public DbSet<Estado_Paciente> estado_paciente { get; set; }
    }
}
