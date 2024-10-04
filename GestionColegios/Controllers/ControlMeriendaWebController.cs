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
    public class ControlMeriendaWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: ControlMeriendaWeb
        public ActionResult Index()
        {
            var controlesMeriendas = db.ControlesMeriendas.Include(c => c.CursoAcademico).Include(c => c.RAceite).Include(c => c.RArroz).Include(c => c.RCereal).Include(c => c.RFrijoles).Include(c => c.RMaiz);
            return View(controlesMeriendas.ToList());
        }

        // GET: ControlMeriendaWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlMerienda controlMerienda = db.ControlesMeriendas.Find(id);
            if (controlMerienda == null)
            {
                return HttpNotFound();
            }
            return View(controlMerienda);
        }

        // GET: ControlMeriendaWeb/Create
        public ActionResult Create()
        {
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre");
            ViewBag.InventarioAlimentoId2 = new SelectList(db.InventariosAlimentos, "Id", "Codigo");
            ViewBag.InventarioAlimentoId1 = new SelectList(db.InventariosAlimentos, "Id", "Codigo");
            ViewBag.InventarioAlimentoId3 = new SelectList(db.InventariosAlimentos, "Id", "Codigo");
            ViewBag.InventarioAlimentoId4 = new SelectList(db.InventariosAlimentos, "Id", "Codigo");
            ViewBag.InventarioAlimentoId5 = new SelectList(db.InventariosAlimentos, "Id", "Codigo");
            return View();
        }

        // POST: ControlMeriendaWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,FechaEntrega,Grado,AsistenciaEsperadaMujeres,AsistenciaEsperadaTotal,AsistenciaRealMujeres,AsistenciaRealTotal,EAceite,EArroz,ECereal,EFrijoles,EMaiz,NombreEstudiante,FirmaDocente,CedulaTutor,FirmaTutor,FechaModificacion,Activo,CursoAcademicoId,InventarioAlimentoId,InventarioAlimentoId2,InventarioAlimentoId1,InventarioAlimentoId3,InventarioAlimentoId4,InventarioAlimentoId5")] ControlMerienda controlMerienda)
        {
            if (ModelState.IsValid)
            {
                db.ControlesMeriendas.Add(controlMerienda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", controlMerienda.CursoAcademicoId);
            ViewBag.InventarioAlimentoId2 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId2);
            ViewBag.InventarioAlimentoId1 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId1);
            ViewBag.InventarioAlimentoId3 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId3);
            ViewBag.InventarioAlimentoId4 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId4);
            ViewBag.InventarioAlimentoId5 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId5);
            return View(controlMerienda);
        }

        // GET: ControlMeriendaWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlMerienda controlMerienda = db.ControlesMeriendas.Find(id);
            if (controlMerienda == null)
            {
                return HttpNotFound();
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", controlMerienda.CursoAcademicoId);
            ViewBag.InventarioAlimentoId2 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId2);
            ViewBag.InventarioAlimentoId1 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId1);
            ViewBag.InventarioAlimentoId3 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId3);
            ViewBag.InventarioAlimentoId4 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId4);
            ViewBag.InventarioAlimentoId5 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId5);
            return View(controlMerienda);
        }

        // POST: ControlMeriendaWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,FechaEntrega,Grado,AsistenciaEsperadaMujeres,AsistenciaEsperadaTotal,AsistenciaRealMujeres,AsistenciaRealTotal,EAceite,EArroz,ECereal,EFrijoles,EMaiz,NombreEstudiante,FirmaDocente,CedulaTutor,FirmaTutor,FechaModificacion,Activo,CursoAcademicoId,InventarioAlimentoId,InventarioAlimentoId2,InventarioAlimentoId1,InventarioAlimentoId3,InventarioAlimentoId4,InventarioAlimentoId5")] ControlMerienda controlMerienda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlMerienda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CursoAcademicoId = new SelectList(db.CursosAcademicos, "Id", "Nombre", controlMerienda.CursoAcademicoId);
            ViewBag.InventarioAlimentoId2 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId2);
            ViewBag.InventarioAlimentoId1 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId1);
            ViewBag.InventarioAlimentoId3 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId3);
            ViewBag.InventarioAlimentoId4 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId4);
            ViewBag.InventarioAlimentoId5 = new SelectList(db.InventariosAlimentos, "Id", "Codigo", controlMerienda.InventarioAlimentoId5);
            return View(controlMerienda);
        }

        // GET: ControlMeriendaWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlMerienda controlMerienda = db.ControlesMeriendas.Find(id);
            if (controlMerienda == null)
            {
                return HttpNotFound();
            }
            return View(controlMerienda);
        }

        // POST: ControlMeriendaWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ControlMerienda controlMerienda = db.ControlesMeriendas.Find(id);
            db.ControlesMeriendas.Remove(controlMerienda);
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
