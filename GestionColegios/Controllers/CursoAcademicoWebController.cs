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
        // GET: CursoAcademicoWeb
        public ActionResult Index()
        {
            var cursosAcademicos = db.CursosAcademicos.Include(c => c.Maestro).Include(c => c.Año).Include(c => c.Seccion).Include(c => c.AñoAcademico).ToList();

            var viewModel = new VMCursoAcademico.VMCursosAcademicos
            {
                Cursos = cursosAcademicos,  // Asegúrate de tener esta propiedad en el ViewModel
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
            return View(cursoAcademico);
        }

        // GET: CursoAcademicoWeb/Create
        public ActionResult Create()
        {
            var viewModel = new VMCursoAcademico.VMCursosAcademicos
            {
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList(),
                 CursoAcademico = new CursoAcademico()
            };

            return View(viewModel);
        }

        // POST: CursoAcademicoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: CursoAcademicoWeb/Create
        // POST: CursoAcademicoWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCursoAcademico.VMCursosAcademicos viewModel)
        {
            if (ModelState.IsValid)
            {
                db.CursosAcademicos.Add(viewModel.CursoAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si hay un error, volver a cargar los select lists
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

            var viewModel = new VMCursoAcademico.VMCursosAcademicos
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
        public ActionResult Edit(VMCursoAcademico.VMCursosAcademicos viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.CursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si hay un error, volver a cargar los select lists
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
            return View(cursoAcademico);
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
