using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_Tandem.Models.MPrueba;
using System.Web;
using Prueba_Tandem.Context;
using Prueba_Tandem.Models.MCita_Medica;
using System.IO;
using Prueba_Tandem.Models;

namespace Prueba_Tandem.Controllers
{
    public class PruebaController : Controller
    {
        public PruebaController(AppDBContext context)
        {
            Context = context;
        }
        public AppDBContext Context { get; }
        // GET: Prueba
        [Route("[controller]/[action]/{id}")]
        public ActionResult Index(int id)
        {
            Prueba PC = new Prueba()
            {
                Cita_Medica=id
            };
            return View(PC);
        }

        // GET: Prueba/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Prueba/Create
        public ActionResult Create()
        {
            return View();
        }

        //public  Upload(UploadModel upload)
        //{
        //    var filename = Path.Combine(Directory.GetCurrentDirectory(),"uploads",upload.MyFile.FileName);
        //    await upload.MyFile.CopyToAsync(new FileStream(filename,FileMode.Create));
        //}

        // POST: Prueba/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prueba P)
        {
            var filename = "";
            if (P.Archivo!=null)
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), "uploads", P.Archivo.FileName);
                P.FileName = filename;
            }
            try
            {
                await P.Archivo.CopyToAsync(new FileStream(filename,FileMode.Create));
                return RedirectToAction("Analizar", "Prueba",P);
            }
            catch(Exception ex)
            {
                ViewBag.mensaje = "Error Vuelva a intentarlo";
                return RedirectToAction("Index", "Cita_Medica");
            }
        }

        public IActionResult Analizar(Prueba P)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(P.FileName);
                if (lines.Length == 2)
                {
                    P.Helice1 = lines[0];
                    P.Helice2 = lines[1];
                }
                bool VirusFound = false;
                #region Variables para Virus
                int cont = 0;
                string vH = "";
                string AdnVirus = "T,T,C,G,G,A,G,T,A,A,C,A,C,G,C,C,T,A,T,A,G,G,C,G,T,G,T,T,A,C,T,C,C,G,A,A";
                string[] Virus = AdnVirus.Split(',');
                List<string> V = new List<string>(); ;
                #endregion
                #region Variables para Helices
                List<string> H = new List<string>();
                #endregion
                bool HelicesCorrectas = true;
                if ((!string.IsNullOrEmpty(P.Helice1)) && (!string.IsNullOrEmpty(P.Helice2)))
                {
                    string[] H1 = P.Helice1.Split(",");
                    string[] H2 = P.Helice2.Split(",");
                    if (H1.Count() == H2.Count())
                    {
                        for (int i = 0; i < H1.Count(); i++)
                        {
                            string UH = H1[i] + H2[i];
                            switch (UH)
                            {
                                case "AT":
                                    break;
                                case "TA":
                                    break;
                                case "AA":
                                    break;
                                case "TT":
                                    break;
                                case "GC":
                                    break;
                                case "CG":
                                    break;
                                case "CC":
                                    break;
                                case "GG":
                                    break;
                                default:
                                    HelicesCorrectas = false;
                                    break;
                            }
                            if (!HelicesCorrectas)
                            {
                                break;
                            }
                        }
                        if (HelicesCorrectas)
                        {
                            string H2B = "";
                            for (int i = H2.Count() - 1; i >= 0; i--)
                            {
                                H2B += H2[i] + ",";
                            }
                            H2B.TrimEnd(',');
                            P.Resultado = P.Helice1 + "," + H2B;
                            string[] HB = P.Resultado.Split(',');
                            for (int i = 0; i < Virus.Length; i++)
                            {
                                vH += Virus[i];
                                cont++;
                                if (cont % 6 == 0)
                                {
                                    V.Add(vH);
                                    vH = "";
                                }
                            }
                            vH = "";
                            cont = 0;
                            for (int i = 0; i < HB.Length; i++)
                            {
                                vH += HB[i];
                                cont++;
                                if (cont % 6 == 0)
                                {
                                    H.Add(vH);
                                    vH = "";
                                }
                            }
                            for (int i = 0; i < V.Count; i++)
                            {
                                if (VirusFound != true)
                                {
                                    for (int j = 0; j < H.Count; j++)
                                    {
                                        if (V[i] == H[j])
                                        {
                                            VirusFound = true;
                                        }
                                    }
                                }
                                else { break; }
                            }
                            #region Actualizacion del resultado de la prueba en cita medica
                            Cita_Medica CM = Context.cita_medica.Where(cm => cm.Id == P.Cita_Medica).FirstOrDefault();
                            CM.Virus = VirusFound;
                            Context.prueba.Add(P);
                            Context.SaveChanges();
                            Context.cita_medica.Update(CM);
                            Context.SaveChanges();
                            #endregion
                            return RedirectToAction("Index", "Cita_Medica");
                        }
                        else
                        {
                            ViewBag.mensaje = "Error Vuelva a intentarlo";
                            System.IO.File.Delete(P.FileName);
                            return RedirectToAction("Index", "Cita_Medica");
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(P.FileName);
                        ViewBag.mensaje = "Error Vuelva a intentarlo";
                        return RedirectToAction("Index", "Cita_Medica");
                    }
                }
                else
                {
                    ViewBag.mensaje = "Error Vuelva a intentarlo";
                    System.IO.File.Delete(P.FileName);
                    return RedirectToAction("Index", "Cita_Medica");
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(P.FileName);
                ViewBag.mensaje = "Error Vuelva a intentarlo";
                return RedirectToAction("Index", "Cita_Medica");
            }
        }

        // GET: Prueba/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Prueba/Edit/5
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

        // GET: Prueba/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Prueba/Delete/5
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