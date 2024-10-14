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
    public class PeriodoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: PeriodoWeb
        public ActionResult Index()
        {



            var viewModel = new VMAñosPeriodosSemestres
            {
                Años = db.Años.ToList(),
                Año = new Año(),
                Periodos = db.Periodos.ToList(),
                Periodo = new Periodo(),
                Semestres = db.Semestres.ToList(),
                Semestre = new Semestre()
            };
            return View(viewModel);
        }
        // GET: PeriodoWeb/Details/5
        public ActionResult DetailsPeriodo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            return View(periodo);
        }

        // GET: PeriodoWeb/Create
        public ActionResult CreatePeriodo()
        {
            ViewBag.AñoId = new SelectList(db.Años, "Id", "Id");
            return View();
        }

        // POST: PeriodoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Numero,FechaModificacion,Activo,AñoId")] Periodo periodo)
        {
            if (ModelState.IsValid)
            {
                db.Periodos.Add(periodo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AñoId = new SelectList(db.Años, "Id", "Id", periodo.AñoId);
            return View(periodo);
        }

        // GET: PeriodoWeb/Edit/5
        public ActionResult EditPeriodo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AñoId = new SelectList(db.Años, "Id", "Id", periodo.AñoId);
            return View(periodo);
        }

        // POST: PeriodoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPeriodo([Bind(Include = "Id,Nombre,Numero,FechaModificacion,Activo,AñoId")] Periodo periodo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(periodo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AñoId = new SelectList(db.Años, "Id", "Id", periodo.AñoId);
            return View(periodo);
        }

        // GET: PeriodoWeb/Delete/5
        public ActionResult DeletePeriodo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Periodo periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }
            return View(periodo);
        }

        // POST: PeriodoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedPeriodo(int id)
        {
            Periodo periodo = db.Periodos.Find(id);
            db.Periodos.Remove(periodo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        public ActionResult DetailsAño(int? id)
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
        public ActionResult CreateAño()
        {
            return View();
        }

        // POST: AñoWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAño([Bind(Include = "Id,Nombre")] Año año)
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
        public ActionResult EditAño(int? id)
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
        public ActionResult EditAño([Bind(Include = "Id,Nombre")] Año año)
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
        public ActionResult DeleteAño(int? id)
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
        public ActionResult DeleteConfirmedAño(int id)
        {
            Año año = db.Años.Find(id);
            db.Años.Remove(año);
            db.SaveChanges();
            return RedirectToAction("Index");
        }





        public ActionResult DetailsSemestre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // GET: SemestreWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SemestreWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSemestre([Bind(Include = "Id,Nombre")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                db.Semestres.Add(semestre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(semestre);
        }

        // GET: SemestreWeb/Edit/5
        public ActionResult EditSemestre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // POST: SemestreWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSemestre([Bind(Include = "Id,Nombre")] Semestre semestre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semestre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semestre);
        }

        // GET: SemestreWeb/Delete/5
        public ActionResult DeleteSemestre(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }
            return View(semestre);
        }

        // POST: SemestreWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedSemestre(int id)
        {
            Semestre semestre = db.Semestres.Find(id);
            db.Semestres.Remove(semestre);
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
