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
    public class CursoAcademicoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CursoAcademicoWeb
        public ActionResult Index()
        {
            var cursosAcademicos = db.CursosAcademicos
                .Include(c => c.Maestro)
                .Include(c => c.Seccion)
                .Include(c => c.AñoAcademico)
                .Include(c => c.Año)
                .ToList();

            var viewModel = new VMCursoAcademico
            {
                CursoAcademicos = cursosAcademicos,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View(viewModel);
        }

        // GET: CursoAcademicoWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View(viewModel);
        }

        // GET: CursoAcademicoWeb/Create
        public ActionResult Create()
        {
            // Set ViewBag.EsEdicion to false if creating
            ViewBag.EsEdicion = false;

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = new CursoAcademico(),  // Asegúrate de inicializar CursoAcademico
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };
            return View(viewModel);

        }

        // POST: CursoAcademicoWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCursoAcademico viewModel)
        {
            if (ModelState.IsValid)
            {
                var cursoAcademico = viewModel.CursoAcademico;
                db.CursosAcademicos.Add(cursoAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Maestros = db.Maestros.ToList();
            viewModel.Secciones = db.Secciones.ToList();
            viewModel.AñosAcademicos = db.AñosAcademicos.ToList();
            viewModel.Años = db.Años.ToList();

            return View(viewModel);
        }

        // GET: CursoAcademicoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            // Set ViewBag.EsEdicion to true if editing
            ViewBag.EsEdicion = true;

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View(viewModel);
        }

        // POST: CursoAcademicoWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCursoAcademico viewModel)
        {
            if (ModelState.IsValid)
            {
                var cursoAcademico = viewModel.CursoAcademico;
                db.Entry(cursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Maestros = db.Maestros.ToList();
            viewModel.Secciones = db.Secciones.ToList();
            viewModel.AñosAcademicos = db.AñosAcademicos.ToList();
            viewModel.Años = db.Años.ToList();

            return View(viewModel);
        }

        // GET: CursoAcademicoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View(viewModel);
        }

        // POST: CursoAcademicoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            db.CursosAcademicos.Remove(cursoAcademico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Función para actualizar los campos cuando se edita un curso
        public ActionResult UpdateCursoAcademico(int id, CursoAcademico cursoAcademico)
        {
            if (id != cursoAcademico.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cursoAcademico);
        }

        // Función para activar la edición
        public ActionResult ActivateEdit(int id)
        {
            var cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            ViewBag.EsEdicion = true;
            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View("Edit", viewModel);
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
