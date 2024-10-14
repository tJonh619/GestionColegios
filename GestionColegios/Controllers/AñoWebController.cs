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
    public class AñoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: AñoWeb
        public ActionResult Index()
        {
            return View(db.Años.ToList());
        }

        // GET: AñoWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Año año = db.Años.Find(id);
            if (año == null)
            {
                return HttpNotFound();
            }
            return View(año);
        }

        // GET: AñoWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AñoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] Año año)
        {
            if (ModelState.IsValid)
            {
                db.Años.Add(año);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(año);
        }

        // GET: AñoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Año año = db.Años.Find(id);
            if (año == null)
            {
                return HttpNotFound();
            }
            return View(año);
        }

        // POST: AñoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Año año)
        {
            if (ModelState.IsValid)
            {
                db.Entry(año).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(año);
        }

        // GET: AñoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Año año = db.Años.Find(id);
            if (año == null)
            {
                return HttpNotFound();
            }
            return View(año);
        }

        // POST: AñoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Año año = db.Años.Find(id);
            db.Años.Remove(año);
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
