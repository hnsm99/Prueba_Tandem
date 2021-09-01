using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tandem.Context;
using Prueba_Tandem.Models.MCita_Medica;
using Prueba_Tandem.Models.MConsultorio;
using Prueba_Tandem.Models.MPaciente;
using Prueba_Tandem.Models.MPrueba;

namespace Prueba_Tandem.Controllers
{
    public class Cita_MedicaController : Controller
    {
        public Cita_MedicaController(AppDBContext context)
        {
            Context = context;
        }
        public AppDBContext Context { get; }
        // GET: Cita_Medica
        public ActionResult Index()
        {
            List<Cita_MedicaIndex> LstCMI = new List<Cita_MedicaIndex>();
            Cita_MedicaIndex CMI = new Cita_MedicaIndex();
            List<Cita_Medica> CM = Context.cita_medica.ToList();
            foreach (Cita_Medica item in CM)
            {
                CMI = new Cita_MedicaIndex
                {
                    Id=item.Id,
                    StrPaciente=Context.paciente.Where(p=>p.Identificacion.Equals(item.Paciente)).Select(p=>p.Nombre).FirstOrDefault(),
                    Paciente=item.Paciente,
                    strMedico=Context.consultorio.Where(c=>c.Id.Equals(item.Consultorio)).Select(c=>c.Medico).FirstOrDefault(),
                    Consultorio=item.Consultorio,
                    Diagnostico=item.Diagnostico,
                    Alta=item.Alta,
                    Virus=item.Virus,
                    Prueba=Context.prueba.Where(p=>p.Cita_Medica.Equals(item.Id)).FirstOrDefault()!=null?true:false,
                    Fecha_Prueba=item.Fecha_Prueba
                };
                LstCMI.Add(CMI);
            }
            return View(LstCMI);
        }

        // GET: Cita_Medica/Details/5
        public ActionResult Details(int id)
        {
            Cita_Medica CM = Context.cita_medica.Where(cm => cm.Id == id).FirstOrDefault();
            Cita_MedicaIndex CMI = new Cita_MedicaIndex()
            {
                Id = CM.Id,
                StrPaciente = Context.paciente.Where(p => p.Identificacion.Equals(CM.Paciente)).Select(p => p.Nombre).FirstOrDefault(),
                Paciente = CM.Paciente,
                strMedico = Context.consultorio.Where(c => c.Id.Equals(CM.Consultorio)).Select(c => c.Medico).FirstOrDefault(),
                Consultorio = CM.Consultorio,
                Diagnostico = CM.Diagnostico,
                Alta = CM.Alta,
                Virus = CM.Virus,
                Prueba = Context.prueba.Where(p => p.Cita_Medica.Equals(CM.Id)).FirstOrDefault() != null ? true : false,
                Fecha_Prueba = CM.Fecha_Prueba
            };
            return View(CMI);
        }

        // GET: Cita_Medica/Create
        [Route("[controller]/[action]/{id}")]
        public ActionResult Create(decimal id)
        {
            try
            {
                Consultorio C = Context.consultorio.Where(m => m.Id == id).FirstOrDefault();
                List<Paciente> P = Context.paciente.OrderByDescending(m => m.Triage).Where(m => m.Estado == 1).ToList();
                foreach (Paciente item in P)
                {
                    Cita_Medica CM = new Cita_Medica()
                    {
                        Paciente = item.Identificacion,
                        Consultorio = id,
                        Alta=false,
                        Virus=false,
                        
                    };
                    //Cita Medica
                    Context.cita_medica.Add(CM);
                    Context.SaveChanges();
                    //Paciente- Estado
                    item.Estado = 2;
                    Context.paciente.Update(item);
                    Context.SaveChanges();
                    //Consultorio-Estado
                    C.Estado = true;
                    Context.consultorio.Update(C);
                    Context.SaveChanges();
                    break;
                }
                return RedirectToAction("Index", "Consultorio");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Route("[controller]/[action]/{id}")]
        public ActionResult Alta(Cita_Medica CM)
        {
            try
            {
                Cita_Medica Cm = Context.cita_medica.Where(cm=>cm.Id==CM.Id).FirstOrDefault();
                Cm.Alta = true;
                Cm.Diagnostico = CM.Diagnostico;
                Context.cita_medica.Update(Cm);
                Context.SaveChanges();
                Paciente P = Context.paciente.Where(p => p.Identificacion == Cm.Paciente).FirstOrDefault();
                P.Estado = 3;
                Context.paciente.Update(P);
                Context.SaveChanges();
                Consultorio C = Context.consultorio.Where(c => c.Id == Cm.Consultorio).FirstOrDefault();
                C.Estado = false;
                Context.consultorio.Update(C);
                Context.SaveChanges();
                return RedirectToAction("Index","Paciente");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("[controller]/[action]/{id}")]
        public ActionResult SPrueba(int id)
        {
            return RedirectToAction("Index","Prueba",id);
        }

        public List<Cita_MedicaIndex> GetCita_MedicaIndex()
        {
            List<Cita_MedicaIndex> CT = new List<Cita_MedicaIndex>();
            Cita_MedicaIndex CMI = new Cita_MedicaIndex();
            List<Consultorio> C = Context.consultorio.ToList();
            List<Paciente> P = Context.paciente.ToList();
            List<Cita_Medica> CM = Context.cita_medica.ToList();
            foreach (Cita_Medica item in CM.Where(cm=>cm.Alta==false))
            {
                CMI = new Cita_MedicaIndex
                {
                    Id=item.Id,
                    Consultorio=item.Consultorio,
                    Paciente=item.Paciente,
                    strMedico=C.Where(m=>m.Id.Equals(item.Consultorio)).Select(m=>m.Medico).FirstOrDefault(),
                    StrPaciente=P.Where(m=>m.Identificacion.Equals(item.Paciente)).Select(m=>m.Nombre).FirstOrDefault()
                };
                CT.Add(CMI);
            }
            return CT;
        }
    }
}