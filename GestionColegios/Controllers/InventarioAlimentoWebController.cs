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
    public class InventarioAlimentoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: InventarioAlimentoWeb
        public ActionResult Index()
        {
            // Obtén la lista de alimentos desde la base de datos
            var listaDeAlimentos = db.InventariosAlimentos.ToList();

            // Crea un ViewModel con la lista de alimentos
            var viewModel = new VMInventarioAlimento
            {
                InventarioAlimentos = listaDeAlimentos, // Asigna la lista de alimentos
                InventarioAlimento = new InventarioAlimento() // Inicializa un objeto vacío para el formulario de agregar
            };

            // Pasa el ViewModel a la vista
            return View(viewModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,NombreAlimento,Stock,UnidadDeMedida,FechaReabastecimiento,FechaModificacion,Activo")] InventarioAlimento inventarioAlimento)
        {
            if (ModelState.IsValid)
            {

                inventarioAlimento.FechaModificacion = DateTime.Now;
                inventarioAlimento.Activo = true;
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

            // Cargar la lista de unidades de medida
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" }, inventarioAlimento.UnidadDeMedida);

            return View(inventarioAlimento);
        }

        // POST: InventarioAlimentoWeb/Edit/5
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

            // Volver a cargar la lista de unidades de medida en caso de error
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" }, inventarioAlimento.UnidadDeMedida);

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

            if (inventarioAlimento != null)
            {
                // Marcar como inactivo en lugar de eliminar
                inventarioAlimento.Activo = false;
                db.Entry(inventarioAlimento).State = EntityState.Modified;
                db.SaveChanges();
            }

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
