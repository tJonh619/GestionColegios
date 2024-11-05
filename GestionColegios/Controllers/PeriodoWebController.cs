using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                Periodos = db.Periodos.ToList(),
                Semestres = db.Semestres.ToList(),
                Año = new Año(),
                Periodo = new Periodo(),
                Semestre = new Semestre()
            };
            ViewBag.EsEdicion = false;
            return View(viewModel);
        }

        // CRUD para Años

        // GET: PeriodoWeb/CreateAño
        public ActionResult CreateAño()
        {
            var viewModel = new VMAñosPeriodosSemestres { Año = new Año() };
            ViewBag.EsEdicion = false;
            return View("_AñosCreate", viewModel);
        }

        // POST: PeriodoWeb/CreateAño
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAño(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Años.Add(model.Año);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Año creado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear el Año. Intente de nuevo.";
            ViewBag.EsEdicion = false;
            return View("_AñosCreate", model);
        }

        // GET: PeriodoWeb/EditAño/5
        public ActionResult EditAño(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var año = db.Años.Find(id);
            if (año == null) return HttpNotFound();

            var viewModel = new VMAñosPeriodosSemestres { Año = año };
            ViewBag.EsEdicion = true;
            return View("_AñosCreate", viewModel);
        }

        // POST: PeriodoWeb/EditAño/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAño(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Año).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Año actualizado correctamente.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Error al actualizar el Año. Intente de nuevo.";
            ViewBag.EsEdicion = true;
            return View("_AñosCreate", model);
        }

        // CRUD para Periodos

        // GET: PeriodoWeb/CreatePeriodo
        public ActionResult CreatePeriodo()
        {
            var viewModel = new VMAñosPeriodosSemestres
            {
                Años = db.Años.ToList(), // Aquí inicializas Años
                Periodo = new Periodo()
            };
            ViewBag.EsEdicion = false;
            return View("_PeriodosCreate", viewModel);
        }

        // POST: PeriodoWeb/CreatePeriodo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePeriodo(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Periodos.Add(model.Periodo);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Periodo creado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear el Periodo. Intente de nuevo.";
            ViewBag.EsEdicion = false;
            return View("_PeriodosCreate", model);
        }

        // GET: PeriodoWeb/EditPeriodo/5
        // GET: PeriodoWeb/EditPeriodo/5
        public ActionResult EditPeriodo(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var periodo = db.Periodos.Find(id);
            if (periodo == null) return HttpNotFound();

            // Cargar la lista de años para que esté disponible en el ViewModel
            var viewModel = new VMAñosPeriodosSemestres
            {
                Periodo = periodo,
                Años = db.Años.ToList() // Cargar los Años desde la base de datos
            };

            ViewBag.EsEdicion = true;
            return View("_PeriodosCreate", viewModel);
        }


        // POST: PeriodoWeb/EditPeriodo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPeriodo(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Periodo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Periodo actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el Periodo. Intente de nuevo.";
            ViewBag.EsEdicion = true;
            return View("_PeriodosCreate", model);
        }

        // CRUD para Semestres

        // GET: PeriodoWeb/CreateSemestre
        public ActionResult CreateSemestre()
        {
            var viewModel = new VMAñosPeriodosSemestres { Semestre = new Semestre() };
            ViewBag.EsEdicion = false;
            return View("_SemestresCreate", viewModel);
        }

        // POST: PeriodoWeb/CreateSemestre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSemestre(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Semestres.Add(model.Semestre);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Semestre creado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear el Semestre. Intente de nuevo.";
            ViewBag.EsEdicion = false;
            return View("_SemestresCreate", model);
        }

        // GET: PeriodoWeb/EditSemestre/5
        public ActionResult EditSemestre(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var semestre = db.Semestres.Find(id);
            if (semestre == null) return HttpNotFound();

            var viewModel = new VMAñosPeriodosSemestres { Semestre = semestre };
            ViewBag.EsEdicion = true;
            return View("_SemestresCreate", viewModel);
        }

        // POST: PeriodoWeb/EditSemestre/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSemestre(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Semestre).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Semestre actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el Semestre. Intente de nuevo.";
            ViewBag.EsEdicion = true;
            return View("_SemestresCreate", model);
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
