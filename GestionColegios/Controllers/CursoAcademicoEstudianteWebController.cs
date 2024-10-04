using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionColegios.Models;

namespace GestionColegios.Controllers
{
    public class CursoAcademicoEstudianteWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CursoAcademicoEstudianteWeb
        public ActionResult Index()
        {
            var cursosAcademicosEstudiantes = db.CursosAcademicosEstudiantes.Include(c => c.CursoAcademico).Include(c => c.Estudiante);
            return View(cursosAcademicosEstudiantes.ToList());
        }

        // GET: CursoAcademicoEstudianteWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoAcademicoEstudiante cursoAcademicoEstudiante = db.CursosAcademicosEstudiantes.Find(id);
            if (cursoAcademicoEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(cursoAcademicoEstudiante);
        }

        // GET: CursoAcademicoEstudianteWeb/Create
        public ActionResult Create()
        {
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre");
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante");
            return View();
        }

        // POST: CursoAcademicoEstudianteWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Promedio,Aprobado,Estado,FechaModificacion,Activo,CursoAcademicoId,EstudianteId")] CursoAcademicoEstudiante cursoAcademicoEstudiante)
        {
            if (ModelState.IsValid)
            {
                db.CursosAcademicosEstudiantes.Add(cursoAcademicoEstudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", cursoAcademicoEstudiante.CursoAcademicoId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", cursoAcademicoEstudiante.EstudianteId);
            return View(cursoAcademicoEstudiante);
        }

        // GET: CursoAcademicoEstudianteWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoAcademicoEstudiante cursoAcademicoEstudiante = db.CursosAcademicosEstudiantes.Find(id);
            if (cursoAcademicoEstudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", cursoAcademicoEstudiante.CursoAcademicoId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", cursoAcademicoEstudiante.EstudianteId);
            return View(cursoAcademicoEstudiante);
        }

        // POST: CursoAcademicoEstudianteWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Promedio,Aprobado,Estado,FechaModificacion,Activo,CursoAcademicoId,EstudianteId")] CursoAcademicoEstudiante cursoAcademicoEstudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cursoAcademicoEstudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", cursoAcademicoEstudiante.CursoAcademicoId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", cursoAcademicoEstudiante.EstudianteId);
            return View(cursoAcademicoEstudiante);
        }

        // GET: CursoAcademicoEstudianteWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoAcademicoEstudiante cursoAcademicoEstudiante = db.CursosAcademicosEstudiantes.Find(id);
            if (cursoAcademicoEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(cursoAcademicoEstudiante);
        }

        // POST: CursoAcademicoEstudianteWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CursoAcademicoEstudiante cursoAcademicoEstudiante = db.CursosAcademicosEstudiantes.Find(id);
            db.CursosAcademicosEstudiantes.Remove(cursoAcademicoEstudiante);
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
