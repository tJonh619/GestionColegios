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
    public class EficienciaFisicaWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: EficienciaFisicaWeb
        public ActionResult Index()
        {
            var eficienciasFisicas = db.EficienciasFisicas.Include(e => e.CursoAcademico).Include(e => e.ConsolidadoEficienciaFisica);
            return View(eficienciasFisicas.ToList());
        }

        // GET: EficienciaFisicaWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisica eficienciaFisica = db.EficienciasFisicas.Find(id);
            if (eficienciaFisica == null)
            {
                return HttpNotFound();
            }
            return View(eficienciaFisica);
        }

        // GET: EficienciaFisicaWeb/Create
        public ActionResult Create()
        {
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre");
            ViewBag.ConsolidadoEficienciaFisicaId = new SelectList(db.ConsolidadosEficienciasFisicas, "Id", "Genero");
            return View();
        }

        // POST: EficienciaFisicaWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Año,Semestre,FechaModificacion,Activo,CursoAcademicoId,ConsolidadoEficienciaFisicaId")] EficienciaFisica eficienciaFisica)
        {
            if (ModelState.IsValid)
            {
                db.EficienciasFisicas.Add(eficienciaFisica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", eficienciaFisica.CursoAcademicoId);
            ViewBag.ConsolidadoEficienciaFisicaId = new SelectList(db.ConsolidadosEficienciasFisicas, "Id", "Genero", eficienciaFisica.ConsolidadoEficienciaFisicaId);
            return View(eficienciaFisica);
        }

        // GET: EficienciaFisicaWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisica eficienciaFisica = db.EficienciasFisicas.Find(id);
            if (eficienciaFisica == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", eficienciaFisica.CursoAcademicoId);
            ViewBag.ConsolidadoEficienciaFisicaId = new SelectList(db.ConsolidadosEficienciasFisicas, "Id", "Genero", eficienciaFisica.ConsolidadoEficienciaFisicaId);
            return View(eficienciaFisica);
        }

        // POST: EficienciaFisicaWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Año,Semestre,FechaModificacion,Activo,CursoAcademicoId,ConsolidadoEficienciaFisicaId")] EficienciaFisica eficienciaFisica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eficienciaFisica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", eficienciaFisica.CursoAcademicoId);
            ViewBag.ConsolidadoEficienciaFisicaId = new SelectList(db.ConsolidadosEficienciasFisicas, "Id", "Genero", eficienciaFisica.ConsolidadoEficienciaFisicaId);
            return View(eficienciaFisica);
        }

        // GET: EficienciaFisicaWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EficienciaFisica eficienciaFisica = db.EficienciasFisicas.Find(id);
            if (eficienciaFisica == null)
            {
                return HttpNotFound();
            }
            return View(eficienciaFisica);
        }

        // POST: EficienciaFisicaWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EficienciaFisica eficienciaFisica = db.EficienciasFisicas.Find(id);
            db.EficienciasFisicas.Remove(eficienciaFisica);
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
