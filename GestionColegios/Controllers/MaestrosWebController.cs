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
    public class MaestrosWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: MaestrosWeb
        public ActionResult Index()
        {
            var viewModel = new VMMaestros { Maestros = db.Maestros.ToList(), Maestro = new Maestro() };
            return View(viewModel);
        }

        // GET: MaestrosWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // GET: MaestrosWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaestrosWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Cedula,Nombres,Apellidos,Sexo,Celular,Direccion,Especialidad,FechaContratacion,HorarioTrabajo,Nivel,FechaModificacion,Activo")] Maestro maestro)
        {
            if (ModelState.IsValid)
            {
                //se genera un codigo basandose en las primeras dos letras del primer nombre y el primer apellido con tres numeros aleatorios
                string Codigo = maestro.Nombres.Substring(0, 3).ToUpper() + maestro.Apellidos.Substring(0, 2).ToUpper() + new Random().Next(100, 1000).ToString();
                maestro.Codigo = Codigo;
                maestro.FechaModificacion = DateTime.Now;
                maestro.Activo = true;
                db.Maestros.Add(maestro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maestro);
        }

        // GET: MaestrosWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // POST: MaestrosWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Cedula,Nombres,Apellidos,Sexo,Celular,Direccion,Especialidad,FechaContratacion,HorarioTrabajo,Nivel,FechaModificacion,Activo")] Maestro maestro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maestro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maestro);
        }

        // GET: MaestrosWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // POST: MaestrosWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maestro maestro = db.Maestros.Find(id);
            db.Maestros.Remove(maestro);
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
