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
    public class ConsolidadoEficienciaFisicaWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: ConsolidadoEficienciaFisicaWeb
        public ActionResult Index()
        {
            return View(db.ConsolidadosEficienciasFisicas.ToList());
        }

        // GET: ConsolidadoEficienciaFisicaWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsolidadoEficienciaFisica consolidadoEficienciaFisica = db.ConsolidadosEficienciasFisicas.Find(id);
            if (consolidadoEficienciaFisica == null)
            {
                return HttpNotFound();
            }
            return View(consolidadoEficienciaFisica);
        }

        // GET: ConsolidadoEficienciaFisicaWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConsolidadoEficienciaFisicaWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Año,Semestre,Edad,Genero,CantidadAlumnos,PromedioPeso,PromedioTalla,PromedioVelocidad,PromedioResistencia,PromedioSalto,PromedioPechadas,PromedioAbdominales,Observaciones,FechaConsolidado,FechaModificacion,Activo")] ConsolidadoEficienciaFisica consolidadoEficienciaFisica)
        {
            if (ModelState.IsValid)
            {
                db.ConsolidadosEficienciasFisicas.Add(consolidadoEficienciaFisica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consolidadoEficienciaFisica);
        }

        // GET: ConsolidadoEficienciaFisicaWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsolidadoEficienciaFisica consolidadoEficienciaFisica = db.ConsolidadosEficienciasFisicas.Find(id);
            if (consolidadoEficienciaFisica == null)
            {
                return HttpNotFound();
            }
            return View(consolidadoEficienciaFisica);
        }

        // POST: ConsolidadoEficienciaFisicaWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Año,Semestre,Edad,Genero,CantidadAlumnos,PromedioPeso,PromedioTalla,PromedioVelocidad,PromedioResistencia,PromedioSalto,PromedioPechadas,PromedioAbdominales,Observaciones,FechaConsolidado,FechaModificacion,Activo")] ConsolidadoEficienciaFisica consolidadoEficienciaFisica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consolidadoEficienciaFisica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consolidadoEficienciaFisica);
        }

        // GET: ConsolidadoEficienciaFisicaWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsolidadoEficienciaFisica consolidadoEficienciaFisica = db.ConsolidadosEficienciasFisicas.Find(id);
            if (consolidadoEficienciaFisica == null)
            {
                return HttpNotFound();
            }
            return View(consolidadoEficienciaFisica);
        }

        // POST: ConsolidadoEficienciaFisicaWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConsolidadoEficienciaFisica consolidadoEficienciaFisica = db.ConsolidadosEficienciasFisicas.Find(id);
            db.ConsolidadosEficienciasFisicas.Remove(consolidadoEficienciaFisica);
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
