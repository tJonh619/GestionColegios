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
    public class EficienciaFisicaEstudianteWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: EficienciaFisicaEstudianteWeb
        public ActionResult Index()
        {
            var eficienciasFisicasEstudiantes = db.EficienciasFisicasEstudiantes.Include(e => e.EficienciaFisica).Include(e => e.Estudiante);
            return View(eficienciasFisicasEstudiantes.ToList());
        }

        // GET: EficienciaFisicaEstudianteWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisicaEstudiante eficienciaFisicaEstudiante = db.EficienciasFisicasEstudiantes.Find(id);
            if (eficienciaFisicaEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(eficienciaFisicaEstudiante);
        }

        // GET: EficienciaFisicaEstudianteWeb/Create
        public ActionResult Create()
        {
            ViewBag.EficienciaFisicaId = new SelectList(db.EficienciasFisicas, "Id", "Id");
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante");
            return View();
        }

        // POST: EficienciaFisicaEstudianteWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NombresApellidos,Edad,Genero,Peso_lb,Talla_cm,Velocidad_seg,Resistencia_min_seg,Salto_cm,Pechadas_repet,Abdominales_repet,FechaModificacion,Activo,EficienciaFisicaId,EstudianteId")] EficienciaFisicaEstudiante eficienciaFisicaEstudiante)
        {
            if (ModelState.IsValid)
            {
                db.EficienciasFisicasEstudiantes.Add(eficienciaFisicaEstudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EficienciaFisicaId = new SelectList(db.EficienciasFisicas, "Id", "Id", eficienciaFisicaEstudiante.EficienciaFisicaId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", eficienciaFisicaEstudiante.EstudianteId);
            return View(eficienciaFisicaEstudiante);
        }

        // GET: EficienciaFisicaEstudianteWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisicaEstudiante eficienciaFisicaEstudiante = db.EficienciasFisicasEstudiantes.Find(id);
            if (eficienciaFisicaEstudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.EficienciaFisicaId = new SelectList(db.EficienciasFisicas, "Id", "Id", eficienciaFisicaEstudiante.EficienciaFisicaId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", eficienciaFisicaEstudiante.EstudianteId);
            return View(eficienciaFisicaEstudiante);
        }

        // POST: EficienciaFisicaEstudianteWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombresApellidos,Edad,Genero,Peso_lb,Talla_cm,Velocidad_seg,Resistencia_min_seg,Salto_cm,Pechadas_repet,Abdominales_repet,FechaModificacion,Activo,EficienciaFisicaId,EstudianteId")] EficienciaFisicaEstudiante eficienciaFisicaEstudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eficienciaFisicaEstudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EficienciaFisicaId = new SelectList(db.EficienciasFisicas, "Id", "Id", eficienciaFisicaEstudiante.EficienciaFisicaId);
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", eficienciaFisicaEstudiante.EstudianteId);
            return View(eficienciaFisicaEstudiante);
        }

        // GET: EficienciaFisicaEstudianteWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisicaEstudiante eficienciaFisicaEstudiante = db.EficienciasFisicasEstudiantes.Find(id);
            if (eficienciaFisicaEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(eficienciaFisicaEstudiante);
        }

        // POST: EficienciaFisicaEstudianteWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EficienciaFisicaEstudiante eficienciaFisicaEstudiante = db.EficienciasFisicasEstudiantes.Find(id);
            db.EficienciasFisicasEstudiantes.Remove(eficienciaFisicaEstudiante);
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
