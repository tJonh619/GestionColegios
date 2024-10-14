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
    public class SemestreWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: SemestreWeb
        public ActionResult Index()
        {
            return View(db.Semestres.ToList());
        }

        // GET: SemestreWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // GET: SemestreWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SemestreWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                db.Semestres.Add(semestre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(semestre);
        }

        // GET: SemestreWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // POST: SemestreWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semestre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semestre);
        }

        // GET: SemestreWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // POST: SemestreWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semestre semestre = db.Semestres.Find(id);
            db.Semestres.Remove(semestre);
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
