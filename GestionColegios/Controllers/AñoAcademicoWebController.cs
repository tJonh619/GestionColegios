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
    public class AñoAcademicoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: AñoAcademicoWeb
        public ActionResult Index()
        {
            return View(db.AñosAcademicos.ToList());
        }

        // GET: AñoAcademicoWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AñoAcademico añoAcademico = db.AñosAcademicos.Find(id);
            if (añoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(añoAcademico);
        }

        // GET: AñoAcademicoWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AñoAcademicoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Descripcion,Nivel,Activo")] AñoAcademico añoAcademico)
        {
            if (ModelState.IsValid)
            {
                db.AñosAcademicos.Add(añoAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(añoAcademico);
        }

        // GET: AñoAcademicoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AñoAcademico añoAcademico = db.AñosAcademicos.Find(id);
            if (añoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(añoAcademico);
        }

        // POST: AñoAcademicoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion,Nivel,Activo")] AñoAcademico añoAcademico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(añoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(añoAcademico);
        }

        // GET: AñoAcademicoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AñoAcademico añoAcademico = db.AñosAcademicos.Find(id);
            if (añoAcademico == null)
            {
                return HttpNotFound();
            }
            return View(añoAcademico);
        }

        // POST: AñoAcademicoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AñoAcademico añoAcademico = db.AñosAcademicos.Find(id);
            db.AñosAcademicos.Remove(añoAcademico);
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
