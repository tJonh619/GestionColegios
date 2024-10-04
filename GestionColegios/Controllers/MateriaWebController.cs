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
    public class MateriaWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: MateriaWeb
        public ActionResult Index()
        {
            var materias = db.Materias.Include(m => m.AñoAcademico);
            return View(materias.ToList());
        }

        // GET: MateriaWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materia materia = db.Materias.Find(id);
            if (materia == null)
            {
                return HttpNotFound();
            }
            return View(materia);
        }

        // GET: MateriaWeb/Create
        public ActionResult Create()
        {
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre");
            return View();
        }

        // POST: MateriaWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodigoMateria,Nombre,Descripcion,FechaModificacion,Activo,AñoAcademicoId")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                db.Materias.Add(materia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", materia.AñoAcademicoId);
            return View(materia);
        }

        // GET: MateriaWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materia materia = db.Materias.Find(id);
            if (materia == null)
            {
                return HttpNotFound();
            }
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", materia.AñoAcademicoId);
            return View(materia);
        }

        // POST: MateriaWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodigoMateria,Nombre,Descripcion,FechaModificacion,Activo,AñoAcademicoId")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", materia.AñoAcademicoId);
            return View(materia);
        }

        // GET: MateriaWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materia materia = db.Materias.Find(id);
            if (materia == null)
            {
                return HttpNotFound();
            }
            return View(materia);
        }

        // POST: MateriaWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Materia materia = db.Materias.Find(id);
            db.Materias.Remove(materia);
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
