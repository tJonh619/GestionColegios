using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionColegios.Models;
using GestionColegios.ViewModels;

namespace GestionColegios.Controllers
{
    public class SeccionWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: SeccionWeb
        public ActionResult Index()
        {
            return View(db.Secciones.ToList());
        }

        // GET: SeccionWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seccion seccion = db.Secciones.Find(id);
            if (seccion == null)
            {
                return HttpNotFound();
            }
            return View(seccion);
        }

        // GET: SeccionWeb/Create
        public ActionResult Create()
        {
            ViewBag.EsEdicion = false;
            return View(new VMSeccion());
        }

        // POST: SeccionWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMSeccion vmSeccion)
        {
            if (ModelState.IsValid)
            {
                var seccion = new Seccion
                {
                    Nombre = vmSeccion.Nombre,
                    CapacidadEstudiantes = vmSeccion.CapacidadEstudiantes,
                    Activo = vmSeccion.Activo,
                    FechaModificacion = DateTime.Now // Asegúrate de setear la fecha
                };

                db.Secciones.Add(seccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vmSeccion);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Seccion seccion = db.Secciones.Find(id);
            if (seccion == null)
            {
                return HttpNotFound();
            }

            var vmSeccion = new VMSeccion
            {
                Id = seccion.Id,
                Nombre = seccion.Nombre,
                CapacidadEstudiantes = seccion.CapacidadEstudiantes,
                Activo = seccion.Activo,
                
            };
            ViewBag.EsEdicion = true;
            return View(vmSeccion);
        }

        // POST: SeccionWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMSeccion vmSeccion)
        {
            if (ModelState.IsValid)
            {
                var seccion = db.Secciones.Find(vmSeccion.Id);
                if (seccion == null)
                {
                    return HttpNotFound();
                }

                seccion.Nombre = vmSeccion.Nombre;
                seccion.CapacidadEstudiantes = vmSeccion.CapacidadEstudiantes;
                seccion.Activo = vmSeccion.Activo;
                seccion.FechaModificacion = DateTime.Now; // Asegúrate de setear la fecha

                db.Entry(seccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vmSeccion);
        }

        // POST: SeccionWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seccion seccion = db.Secciones.Find(id);
            db.Secciones.Remove(seccion);
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
