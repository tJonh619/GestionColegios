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

            var viewModel = new VMAñosPeriodosSemestres
            {
                Periodo = periodo,
                Años = db.Años.ToList() // Asegúrate de llenar esta propiedad
            };

            return View(viewModel);
        }


        // POST: PeriodoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPeriodo(VMAñosPeriodosSemestres viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Periodo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Años = db.Años.ToList(); // Repetimos para rellenar la lista de años en caso de error
            return View(viewModel);
        }

        // GET: PeriodoWeb/Deactivate/5
        public ActionResult Desactivar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMAñosPeriodosSemestres
            {
                Periodo = periodo, // Asigna el objeto Periodo aquí
                                   // Puedes asignar otros datos si es necesario, por ejemplo:
                                   // Años = db.Años.ToList(),
                                   // Semestres = db.Semestres.ToList()
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("DeactivateConfirmedPeriodo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmedPeriodo(int id)
        {
            // Lógica para desactivar el periodo
            var periodo = db.Periodos.Find(id);
            if (periodo == null)
            {
                return HttpNotFound();
            }

            periodo.Activo = false; // Cambia a desactivado
            db.Entry(periodo).State = EntityState.Modified;
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

            // Crea una vista de modelo
            var viewModel = new VMAñosPeriodosSemestres
            {
                Año = año
            };

            return View(viewModel);
        }

        // POST: AñoWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: AñoWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAño(VMAñosPeriodosSemestres model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model.Año).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
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

            var viewModel = new VMAñosPeriodosSemestres
            {
                Año = año
            };

            return View(viewModel);
        }

        // POST: AñoWeb/Delete/5
        [HttpPost, ActionName("DeleteAño")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAño(int id)
        {
            Año año = db.Años.Find(id);
            if (año == null)
            {
                return HttpNotFound();
            }

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

            var semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMAñosPeriodosSemestres
            {
                Semestre = semestre,
               
            };

            return View(viewModel);
        }
        // POST: SemestreWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSemestre(VMAñosPeriodosSemestres viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Semestre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
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

            var viewModel = new VMAñosPeriodosSemestres
            {
                Semestre = semestre
            };

            return View(viewModel);
        }

        // POST: SemestreWeb/Delete/5
        [HttpPost, ActionName("DeleteSemestre")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedSemestre(int id)
        {
            Semestre semestre = db.Semestres.Find(id);
            if (semestre == null)
            {
                return HttpNotFound();
            }

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
