using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tandem.Context;
using Prueba_Tandem.Models.MConsultorio;

namespace Prueba_Tandem.Controllers
{
    public class ConsultorioController : Controller
    {
        public ConsultorioController(AppDBContext context)
        {
            Context = context;
        }
        public AppDBContext Context { get; }
        // GET: Consultorio
        public ActionResult Index()
        {
            List<Consultorio> LstC = new List<Consultorio>();
            Consultorio C = new Consultorio();
            List<Consultorio> P = Context.consultorio.ToList();
            foreach (Consultorio item in P)
            {
                C = new Consultorio
                {
                     Id=item.Id,
                     Medico=item.Medico,
                     Estado=item.Estado
                };
                LstC.Add(C);
            }
            return View(LstC);
        }

        // GET: Consultorio/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Consultorio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultorio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Consultorio consultorioForm)
        {
            try
            {
                consultorioForm.Estado = false;
                if (ModelState.IsValid)
                {
                    Context.consultorio.Add(consultorioForm);
                    Context.SaveChanges();
                    TempData["Exito"] = "El consultorio se a creado correctamente";
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

        // GET: Consultorio/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Consultorio/Edit/5
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

        // GET: Consultorio/Delete/5
        public ActionResult Delete(decimal id)
        {
            try
            {
                Consultorio C = Context.consultorio.Where(c => c.Id == id).FirstOrDefault();
                Context.consultorio.Remove(C);
                Context.SaveChanges();
                return RedirectToAction("Index","Consultorio");
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "El consultorio no se puede eliminar, ya que tiene citas asignadas terminadas o en proceso.";
                return RedirectToAction("Index", "Consultorio");
            }
            
        }

        // POST: Consultorio/Delete/5
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