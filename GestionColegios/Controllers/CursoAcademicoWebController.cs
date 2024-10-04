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
    public class CursoAcademicoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CursoAcademicoWeb
        public ActionResult Index()
        {
            var cursosAcademicos = db.CursosAcademicos.Include(c => c.Maestro).Include(c => c.Seccion).Include(c => c.AñoAcademico);
            return View(cursosAcademicos.ToList());
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
            ViewBag.MaestroId = new SelectList(db.Maestros, "Id", "Codigo");
            ViewBag.SeccionId = new SelectList(db.Secciones, "Id", "Nombre");
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre");
            return View();
        }

        // POST: CursoAcademicoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Año,Nombre,FechaModificacion,Activo,MaestroId,SeccionId,AñoAcademicoId")] CursoAcademico cursoAcademico)
        {
            if (ModelState.IsValid)
            {
                db.CursosAcademicos.Add(cursoAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaestroId = new SelectList(db.Maestros, "Id", "Codigo", cursoAcademico.MaestroId);
            ViewBag.SeccionId = new SelectList(db.Secciones, "Id", "Nombre", cursoAcademico.SeccionId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", cursoAcademico.AñoAcademicoId);
            return View(cursoAcademico);
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
            ViewBag.MaestroId = new SelectList(db.Maestros, "Id", "Codigo", cursoAcademico.MaestroId);
            ViewBag.SeccionId = new SelectList(db.Secciones, "Id", "Nombre", cursoAcademico.SeccionId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", cursoAcademico.AñoAcademicoId);
            return View(cursoAcademico);
        }

        // POST: CursoAcademicoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Año,Nombre,FechaModificacion,Activo,MaestroId,SeccionId,AñoAcademicoId")] CursoAcademico cursoAcademico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaestroId = new SelectList(db.Maestros, "Id", "Codigo", cursoAcademico.MaestroId);
            ViewBag.SeccionId = new SelectList(db.Secciones, "Id", "Nombre", cursoAcademico.SeccionId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", cursoAcademico.AñoAcademicoId);
            return View(cursoAcademico);
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
