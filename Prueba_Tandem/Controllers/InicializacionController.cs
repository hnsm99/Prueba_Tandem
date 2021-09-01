using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tandem.Context;
using Prueba_Tandem.Models;
using Prueba_Tandem.Models.MConsultorio;
using Prueba_Tandem.Models.MPaciente;

namespace Prueba_Tandem.Controllers
{
    public class InicializacionController : Controller
    {
        public InicializacionController(AppDBContext context)
        {
            Context = context;
        }
        public AppDBContext Context { get; }
        // GET: Inicializacion
        public ActionResult Index()
        {
            return View();
        }
        // POST: Inicializacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadModel UP)
        {
            var filenameClientes = "";
            var filenameConsultorio = "";
            if (UP.Clientes != null && UP.Consultorios != null)
            {
                filenameClientes = Path.Combine(Directory.GetCurrentDirectory(), "uploads", UP.Clientes.FileName);
                filenameConsultorio = Path.Combine(Directory.GetCurrentDirectory(), "uploads", UP.Consultorios.FileName);
                UP.FilepathClientes = filenameClientes;
                UP.FilepathConsultorio = filenameConsultorio;
            }
            else
            {
                ViewBag.mensaje = "Error Vuelva a intentarlo";
                return RedirectToAction("Index", "Inicializacion");
            }
            try
            {
                using (var stream = System.IO.File.Create(filenameClientes))
                {
                    await UP.Clientes.CopyToAsync(stream);
                }
                using (var streamC = System.IO.File.Create(filenameConsultorio))
                {
                    await UP.Consultorios.CopyToAsync(streamC);
                }
                //await UP.Clientes.CopyToAsync(new FileStream(filenameClientes, FileMode.Create));
                //await UP.Consultorios.CopyToAsync(new FileStream(filenameConsultorio, FileMode.Create));
                Dispose();
                return RedirectToAction("Insert", "Inicializacion", UP);
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "Error Vuelva a intentarlo";
                return RedirectToAction("Index", "Inicializacion");
            }
        }

        // GET: Inicializacion/Create
        public ActionResult Insert(UploadModel UP)
        {
            try
            {
                #region Inicializacion de clientes
                Paciente P = new Paciente();
                string[] lines = System.IO.File.ReadAllLines(UP.FilepathClientes);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] paciente;
                    paciente = lines[i].Split('|');
                    if (paciente.Length == 6)
                    {
                        if (Context.paciente.Where(p => p.Identificacion == Decimal.Parse(paciente[0])).FirstOrDefault() == null)
                        {
                            P = new Paciente
                            {
                                Identificacion = Decimal.Parse(paciente[0]),
                                Nombre = paciente[1],
                                Edad = Decimal.Parse(paciente[2]),
                                Sexo = Boolean.Parse(paciente[3]),
                                Triage = Int32.Parse(paciente[4]),
                                Sintomas = paciente[5],
                                Estado = 1
                            };
                            Context.paciente.Add(P);
                            Context.SaveChanges();
                        }
                        else
                        {
                            ViewBag.mensaje = "Revise la informacion hay usuarios ya creados en el sistema.";
                            return RedirectToAction("InsertC", "Inicializacion",UP);
                        }
                    }
                    else
                    {
                        ViewBag.mensaje = "Rectifique el archivo subido, no cumple con los estandares";
                        return RedirectToAction("InsertC", "Inicializacion",UP);
                    }
                }
                #endregion
                return RedirectToAction("InsertC", "Inicializacion",UP);
            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error inicializando los pacientes revise la informacion y vuelva a intentarlo";
                return RedirectToAction("Index", "Inicializacion");
            }
        }

        public ActionResult InsertC(UploadModel UP)
        {
            try
            {
                #region Inicializacion Consultorios
                Consultorio C = new Consultorio();
                string[] linesC = System.IO.File.ReadAllLines(UP.FilepathConsultorio);
                for (int i = 0; i < linesC.Length; i++)
                {
                    string[] consultorio;
                    consultorio = linesC[i].Split('|');
                    if (consultorio.Length == 2)
                    {
                        if (Context.consultorio.Where(c => c.Id == Decimal.Parse(consultorio[0])).FirstOrDefault() == null)
                        {
                            C = new Consultorio
                            {
                                Id = Decimal.Parse(consultorio[0]),
                                Medico = consultorio[1],
                                Estado = false
                            };
                            Context.consultorio.Add(C);
                        }
                        else
                        {
                            ViewBag.mensaje = "Revise la informacion hay consultorios ya creados en el sistema.";
                            return RedirectToAction("Index", "Inicializacion");
                        }
                        Context.SaveChanges();
                    }
                    else
                    {
                        ViewBag.mensaje = "Rectifique el archivo subido, no cumple con los estandares";
                        return RedirectToAction("Index", "Inicializacion");
                    }
                }
                #endregion
                return RedirectToAction("Index", "Paciente");
            }
            catch (Exception)
            {
                ViewBag.mensaje = "Error inicializando los consultorios revise la informacion y vuelva a intentarlo";
                return RedirectToAction("Index", "Inicializacion");
            }
        }
    }
}