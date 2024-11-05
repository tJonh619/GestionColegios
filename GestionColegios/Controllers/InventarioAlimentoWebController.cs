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
            var viewModel = new VMInventarioAlimento
            {
                InventarioAlimentos = db.InventariosAlimentos.Where(a => a.Activo).ToList(),
                InventarioAlimento = new InventarioAlimento()
            };

            ViewBag.EsEdicion = false;
            return View(viewModel);
        }

        // GET: InventarioAlimentoWeb/Create
        public ActionResult Create()
        {
            var viewModel = new VMInventarioAlimento
            {
                InventarioAlimento = new InventarioAlimento()
            };

            ViewBag.EsEdicion = false;
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" });
            return View("_AgregarAlimento", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMInventarioAlimento model)
        {
            if (ModelState.IsValid)
            {
                var alimentoExistente = db.InventariosAlimentos
                    .FirstOrDefault(a => a.NombreAlimento == model.InventarioAlimento.NombreAlimento);

                if (alimentoExistente != null)
                {
                    alimentoExistente.Stock += model.InventarioAlimento.Stock;
                    alimentoExistente.FechaModificacion = DateTime.Now;
                }
                else
                {
                    model.InventarioAlimento.Activo = true;
                    model.InventarioAlimento.FechaModificacion = DateTime.Now;
                    db.InventariosAlimentos.Add(model.InventarioAlimento);
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Alimento agregado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al agregar el alimento. Verifique los datos.";
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" });
            return View("_AgregarAlimento", model);
        }

        // GET: InventarioAlimentoWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMInventarioAlimento
            {
                InventarioAlimento = inventarioAlimento
            };

            ViewBag.EsEdicion = true;
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" }, inventarioAlimento.UnidadDeMedida);
            return View("_AgregarAlimento", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMInventarioAlimento model)
        {
            if (ModelState.IsValid)
            {
                var inventarioAlimento = db.InventariosAlimentos.Find(model.InventarioAlimento.Id);
                if (inventarioAlimento == null)
                {
                    return HttpNotFound();
                }

                inventarioAlimento.NombreAlimento = model.InventarioAlimento.NombreAlimento;
                inventarioAlimento.Stock = model.InventarioAlimento.Stock;
                inventarioAlimento.UnidadDeMedida = model.InventarioAlimento.UnidadDeMedida;
                inventarioAlimento.FechaModificacion = DateTime.Now;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Alimento actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el alimento.";
            ViewBag.UnidadesDeMedida = new SelectList(new List<string> { "libras", "cuartas" }, model.InventarioAlimento.UnidadDeMedida);
            return View("_AgregarAlimento", model);
        }

        // GET: InventarioAlimentoWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMInventarioAlimento
            {
                InventarioAlimento = inventarioAlimento
            };

            return View(viewModel);
        }

        // POST: InventarioAlimentoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var inventarioAlimento = db.InventariosAlimentos.Find(id);
            if (inventarioAlimento != null)
            {
                inventarioAlimento.Activo = false;
                inventarioAlimento.FechaModificacion = DateTime.Now;
                db.Entry(inventarioAlimento).State = EntityState.Modified;
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Alimento desactivado correctamente.";
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