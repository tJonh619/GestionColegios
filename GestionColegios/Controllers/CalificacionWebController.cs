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
    public class CalificacionWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CalificacionWeb
        public ActionResult Index()
        {
            var calificaciones = db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Materia);
            return View(calificaciones.ToList());
        }

        // GET: CalificacionWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calificacion calificacion = db.Calificaciones.Find(id);
            if (calificacion == null)
            {
                return HttpNotFound();
            }
            return View(calificacion);
        }

        // GET: CalificacionWeb/Create
        public ActionResult Create()
        {
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante");
            ViewBag.MateriaId = new SelectList(db.Materias, "Id", "CodigoMateria");
            return View();
        }

        // POST: CalificacionWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NParcial1,NCualitativa1,NParcial2,NCualitativa2,NParcial3,NCualitativa3,NParcial4,NCualitativa4,Activo,EstudianteId,MateriaId")] Calificacion calificacion)
        {
            if (ModelState.IsValid)
            {
                db.Calificaciones.Add(calificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", calificacion.EstudianteId);
            ViewBag.MateriaId = new SelectList(db.Materias, "Id", "CodigoMateria", calificacion.MateriaId);
            return View(calificacion);
        }

        // GET: CalificacionWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calificacion calificacion = db.Calificaciones.Find(id);
            if (calificacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", calificacion.EstudianteId);
            ViewBag.MateriaId = new SelectList(db.Materias, "Id", "CodigoMateria", calificacion.MateriaId);
            return View(calificacion);
        }

        // POST: CalificacionWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NParcial1,NCualitativa1,NParcial2,NCualitativa2,NParcial3,NCualitativa3,NParcial4,NCualitativa4,Activo,EstudianteId,MateriaId")] Calificacion calificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", calificacion.EstudianteId);
            ViewBag.MateriaId = new SelectList(db.Materias, "Id", "CodigoMateria", calificacion.MateriaId);
            return View(calificacion);
        }

        // GET: CalificacionWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calificacion calificacion = db.Calificaciones.Find(id);
            if (calificacion == null)
            {
                return HttpNotFound();
            }
            return View(calificacion);
        }

        // POST: CalificacionWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Calificacion calificacion = db.Calificaciones.Find(id);
            db.Calificaciones.Remove(calificacion);
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
