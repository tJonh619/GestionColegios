using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionColegios.Models;
using GestionColegios.ViewModels;

namespace GestionColegios.Controllers
{
    public class AñoAcademicoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: AñoAcademicoWeb
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var viewModel = new VMAñosAcademicos
                {
                    AñosAcademicos = db.AñosAcademicos.Where(a => a.Activo).ToList(),
                    AñoAcademico = new AñoAcademico()
                };
                ViewBag.EsEdicion = false;
                return View(viewModel);
            }
            
        }

        // GET: AñoAcademicoWeb/Create
        public ActionResult Create()
        {
            var viewModel = new VMAñosAcademicos
            {
                AñoAcademico = new AñoAcademico()
            };
            ViewBag.EsEdicion = false;
            return View("_CrearEditarAñoAcademico", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMAñosAcademicos model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                model.AñoAcademico.Activo = true;
                db.AñosAcademicos.Add(model.AñoAcademico);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Año académico creado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear el año académico. Intente nuevamente.";
            return View("_CrearEditarAñoAcademico", model);
        }

        // GET: AñoAcademicoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var añoAcademico = db.AñosAcademicos.SingleOrDefault(a => a.Id == id);
            if (añoAcademico == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMAñosAcademicos
            {
                AñoAcademico = añoAcademico
            };
            ViewBag.EsEdicion = true;
            return View("_CrearEditarAñoAcademico", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMAñosAcademicos model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var añoAcademico = db.AñosAcademicos.SingleOrDefault(a => a.Id == model.AñoAcademico.Id);
                if (añoAcademico == null)
                {
                    return HttpNotFound();
                }

                añoAcademico.Nombre = model.AñoAcademico.Nombre;
                añoAcademico.Descripcion = model.AñoAcademico.Descripcion;
                añoAcademico.Nivel = model.AñoAcademico.Nivel;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Año académico actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el año académico. Intente de nuevo.";
            return View("_CrearEditarAñoAcademico", model);
        }

        // GET: AñoAcademicoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var añoAcademico = db.AñosAcademicos.SingleOrDefault(a => a.Id == id);
            if (añoAcademico == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMAñosAcademicos
            {
                AñoAcademico = añoAcademico
            };

            return View(viewModel);
        }

        // POST: AñoAcademicoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var añoAcademico = db.AñosAcademicos.Find(id);
            if (añoAcademico != null)
            {
                añoAcademico.Activo = false;
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Año académico desactivado correctamente.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
