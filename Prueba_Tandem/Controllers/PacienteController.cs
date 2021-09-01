using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prueba_Tandem.Context;
using Prueba_Tandem.Models.MCita_Medica;
using Prueba_Tandem.Models.MEstado_Paciente;
using Prueba_Tandem.Models.MPaciente;
using Prueba_Tandem.Models.MTriage;

namespace Prueba_Tandem.Controllers
{
    public class PacienteController : Controller
    {
        public PacienteController(AppDBContext context)
        {
            Context = context;
        }
        public AppDBContext Context { get; }
        // GET: Paciente
        public ActionResult Index()
        {
            Cita_MedicaController CMC = new Cita_MedicaController(Context);
            List<PacienteIndex> LstPI = new List<PacienteIndex>();
            PacienteIndex PI = new PacienteIndex();
            List<Triage> Tr = Context.triage.ToList();
            List<Estado_Paciente> EP = Context.estado_paciente.ToList();
            List<Paciente> P = Context.paciente.ToList();
            List <Cita_MedicaIndex> CMI= CMC.GetCita_MedicaIndex();
            foreach (Paciente item in P)
            {
                PI = new PacienteIndex
                {
                    Identificacion = item.Identificacion,
                    Nombre = item.Nombre,
                    Edad = item.Edad,
                    Estado = item.Estado,
                    Sexo = item.Sexo,
                    estado_Pacientes = EP.Where(m => m.Id.Equals(item.Estado)).Select(m => m.Estado).FirstOrDefault(),
                    Sintomas = item.Sintomas,
                    Triage = item.Triage,
                    triages = Tr.Where(m => m.Id.Equals(item.Triage)).Select(m => m.Nombre_Urgencia).FirstOrDefault(),
                    cita_medica=CMI
                };
                LstPI.Add(PI);
            }
            return View(LstPI);
        }

        // GET: Paciente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Paciente/Create
        public ActionResult Create()
        {
            List<Triage> tr = Context.triage.ToList();
            List<SelectListItem> items = tr.ConvertAll(d =>
             {
                 return new SelectListItem()
                 {
                     Text = d.Nombre_Urgencia,
                     Value = d.Id.ToString()
                 };
             });
            ViewBag.items = items;
            return View();
        }

        // POST: Paciente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Paciente pacienteform)
        {
            try
            {
                //pacienteform.Sexo = pacienteform.Sexo == "0" ? true : false;
                if (ModelState.IsValid)
                {
                    pacienteform.Estado = 1;
                    Context.paciente.Add(pacienteform);
                    Context.SaveChanges();
                    TempData["Exito"] = "El paciente se a creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Paciente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Paciente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Paciente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Paciente/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}