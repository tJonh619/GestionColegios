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
    public class MateriaWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: MateriaWeb
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new VMMateria
            {
                Materias = db.Materias.ToList(),
                Materia = new Materia(),
                AñoAcademico = new AñoAcademico(),
                AñosAcademicos = db.AñosAcademicos.ToList()

            };
            ViewBag.EsEdicion = false;
            return View(viewModel);
        }

        // GET: MateriaWeb/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new VMMateria
            {
                Materia = new Materia(),
                Materias = db.Materias.ToList(),
                AñoAcademico = new AñoAcademico(),
                AñosAcademicos = db.AñosAcademicos.ToList()
            };

            ViewBag.EsEdicion = false;
            return View("_Create", viewModel);
        }

        // POST: MateriaWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMMateria model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                model.Materia.Activo = true;
                model.Materia.FechaModificacion = DateTime.Now;
                db.Materias.Add(model.Materia);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Materia creada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear la materia. Verifique los datos.";
            return View("_Create", model);
        }

        // GET: MateriaWeb/Edit/5
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

            var materia = db.Materias.Find(id);
            if (materia == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMMateria
            {
                Materia = materia,
                Materias = db.Materias.ToList(),
                AñoAcademico = new AñoAcademico(),
                AñosAcademicos = db.AñosAcademicos.ToList()
            };

            ViewBag.EsEdicion = true;
            return View("_Create", viewModel);
        }

        // POST: MateriaWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMMateria model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var materia = db.Materias.Find(model.Materia.Id);
                if (materia == null)
                {
                    return HttpNotFound();
                }

                materia.CodigoMateria = model.Materia.CodigoMateria;
                materia.Nombre = model.Materia.Nombre;
                materia.Descripcion = model.Materia.Descripcion;
                materia.AñoAcademicoId = model.Materia.AñoAcademicoId;
                materia.FechaModificacion = DateTime.Now;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Materia actualizada correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar la materia.";
            return View("_Create", model);
        }

        // GET: MateriaWeb/Delete/5
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

            var materia = db.Materias.Find(id);
            if (materia == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMMateria { Materia = materia };
            return View(viewModel);
        }

        // POST: MateriaWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var materia = db.Materias.Find(id);
            if (materia != null)
            {
                materia.Activo = false;
                materia.FechaModificacion = DateTime.Now;
                db.Entry(materia).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Materia desactivada correctamente.";
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
