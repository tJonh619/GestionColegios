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
    public class MatriculasController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: Matriculas
        public ActionResult Index()
        {
            var matriculas = db.Matriculas.Include(m => m.Estudiante).Include(m => m.Tutor).Include(m => m.Periodos).Include(m => m.AñoAcademico);
            return View(matriculas.ToList());
        }

        // GET: Matriculas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }
        // GET: Matriculas/Create
        public ActionResult Create(int? estudianteId)
        {
            if (estudianteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Cargar el estudiante con el tutor relacionado explícitamente
            var estudiante = db.Estudiantes
                               .Include(e => e.Tutor) // Esto cargará el Tutor asociado al Estudiante
                               .FirstOrDefault(e => e.Id == estudianteId);

            if (estudiante == null)
            {
                return HttpNotFound("Estudiante no encontrado");
            }

            // Si no tiene tutor, devolver un mensaje adecuado
            if (estudiante.Tutor == null)
            {
                return HttpNotFound("El estudiante no tiene un tutor asignado");
            }

            // Prellenar los campos de estudiante y tutor
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre");
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre");

            // Crea una nueva instancia de Matricula
            Matricula matricula = new Matricula
            {
                EstudianteId = estudiante.Id,
                TutorId = estudiante.Tutor.Id // Usa el ID del tutor directamente
            };

            // Pasar el estudiante y el tutor a la vista
            ViewBag.Estudiante = estudiante;
            ViewBag.Tutor = estudiante.Tutor;

            return View(matricula);
        }


        // POST: Matriculas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Descripcion,Continuidad,Traslado,Repitente,FechaMatricula,FechaModificacion,Activo,EstudianteId,TutorId,PeriodosId,AñoAcademicoId,Aprobado")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                matricula.FechaMatricula = "Cambiar a datetime";
                matricula.FechaModificacion = DateTime.Now;
                matricula.Activo = true;
                db.Matriculas.Add(matricula);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", matricula.EstudianteId);
            ViewBag.TutorId = new SelectList(db.Tutores, "Id", "Nombres", matricula.TutorId);
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre", matricula.PeriodosId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", matricula.AñoAcademicoId);
            return View(matricula);
        }

        // GET: Matriculas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", matricula.EstudianteId);
            ViewBag.TutorId = new SelectList(db.Tutores, "Id", "Nombres", matricula.TutorId);
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre", matricula.PeriodosId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", matricula.AñoAcademicoId);
            return View(matricula);
        }

        // POST: Matriculas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Descripcion,Continuidad,Traslado,Repitente,FechaMatricula,FechaModificacion,Activo,EstudianteId,TutorId,PeriodosId,AñoAcademicoId,Aprobado")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matricula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "CodigoEstudiante", matricula.EstudianteId);
            ViewBag.TutorId = new SelectList(db.Tutores, "Id", "Nombres", matricula.TutorId);
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre", matricula.PeriodosId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", matricula.AñoAcademicoId);
            return View(matricula);
        }

        // GET: Matriculas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        // POST: Matriculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matricula matricula = db.Matriculas.Find(id);
            db.Matriculas.Remove(matricula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Matriculas/Historial
        public ActionResult Historial(int? estudianteId)
        {
            if (estudianteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Buscar las matrículas asociadas al estudiante
            var matriculas = db.Matriculas.Where(m => m.EstudianteId == estudianteId).ToList();

            if (matriculas == null || !matriculas.Any())
            {
                return HttpNotFound("No se encontraron matrículas para este estudiante.");
            }

            // Pasar las matrículas a la vista
            return View(matriculas);
        }

        // GET: Matriculas/Edit/5
        public ActionResult EditMatricula(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Buscar la matrícula por Id
            Matricula matricula = db.Matriculas.Find(id);
            if (matricula == null)
            {
                return HttpNotFound("Matrícula no encontrada.");
            }

            // Prellenar los campos de los dropdowns (Período, Año Académico, etc.)
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre", matricula.PeriodosId);
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre", matricula.AñoAcademicoId);

            return View("Create", matricula); // Usamos la misma vista de Create pero pasamos el modelo existente
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
