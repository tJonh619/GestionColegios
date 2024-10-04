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
    public class InventarioAlimentoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: InventarioAlimentoWeb
        public ActionResult Index()
        {
            return View(db.InventariosAlimentos.ToList());
        }

        // GET: InventarioAlimentoWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventarioAlimento inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento == null)
            {
                return HttpNotFound();
            }
            return View(inventarioAlimento);
        }

        // GET: InventarioAlimentoWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioAlimentoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,NombreAlimento,Stock,UnidadDeMedida,FechaReabastecimiento,FechaModificacion,Activo")] InventarioAlimento inventarioAlimento)
        {
            if (ModelState.IsValid)
            {
                db.InventariosAlimentos.Add(inventarioAlimento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventarioAlimento);
        }

        // GET: InventarioAlimentoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventarioAlimento inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento == null)
            {
                return HttpNotFound();
            }
            return View(inventarioAlimento);
        }

        // POST: InventarioAlimentoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,NombreAlimento,Stock,UnidadDeMedida,FechaReabastecimiento,FechaModificacion,Activo")] InventarioAlimento inventarioAlimento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventarioAlimento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventarioAlimento);
        }

        // GET: InventarioAlimentoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventarioAlimento inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento == null)
            {
                return HttpNotFound();
            }
            return View(inventarioAlimento);
        }

        // POST: InventarioAlimentoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventarioAlimento inventarioAlimento = db.InventariosAlimentos.Find(id);
            db.InventariosAlimentos.Remove(inventarioAlimento);
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
