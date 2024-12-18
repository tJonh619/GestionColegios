﻿using System;
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
    public class MatriculasController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: Matriculas
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var matriculas = db.Matriculas.Include(m => m.Estudiante).Include(m => m.Tutor).Include(m => m.Periodos).Include(m => m.AñoAcademico);
            return View(matriculas.ToList());


        }

        // GET: Matriculas/Details/5
        public ActionResult Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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

            // Autogenerar el código de la matrícula
            string Codigo = "MATRICULA-" + DateTime.Now.Year + "-" + (db.Matriculas.Count() + 1).ToString("D4");

            // Prellenar los campos de estudiante y tutor
            ViewBag.PeriodosId = new SelectList(db.Periodos, "Id", "Nombre");
            ViewBag.AñoAcademicoId = new SelectList(db.AñosAcademicos, "Id", "Nombre");

            // Crea una nueva instancia de Matricula
            Matricula matricula = new Matricula
            {
                EstudianteId = estudiante.Id,
                TutorId = estudiante.Tutor.Id, // Usa el ID del tutor directamente
                Codigo = Codigo // Asignamos el código autogenerado
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
        public ActionResult Create([Bind(Include = "Id,Codigo,Descripcion,Continuidad,Traslado,Repitente,FechaMatricula,FechaModificacion,Activo,EstudianteId,TutorId,PeriodosId,AñoAcademicoId,Aprobado,Grado")] Matricula matricula)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                
                matricula.FechaMatricula = DateTime.Now;
                matricula.FechaModificacion = DateTime.Now;
                matricula.Activo = true;
                db.Matriculas.Add(matricula);
                db.SaveChanges();
                return RedirectToAction("Index","EstudianteWeb");
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
        public ActionResult Edit([Bind(Include = "Id,Codigo,Descripcion,Continuidad,Traslado,Repitente,FechaModificacion,Activo,EstudianteId,TutorId,PeriodosId,AñoAcademicoId,Aprobad,Grado")] Matricula matricula)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                // Mantener la FechaMatricula original del registro en la base de datos
                var matriculaOriginal = db.Matriculas.AsNoTracking().FirstOrDefault(m => m.Id == matricula.Id);
                if (matriculaOriginal != null)
                {
                    matricula.FechaMatricula = matriculaOriginal.FechaMatricula;
                }

                matricula.FechaModificacion = DateTime.Now;

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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Matricula matricula = db.Matriculas.Find(id);
            db.Matriculas.Remove(matricula);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Matriculas/Historial
        public ActionResult Historial(int? estudianteId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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

        public ActionResult ImprimirMatricula(int? AñoAcademicoId, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            // Obtener todas las matrículas, aplicando filtros si son proporcionados
            var matriculas = db.Matriculas.Include(m => m.Estudiante)
                                          .Include(m => m.Tutor)
                                          .Include(m => m.Periodos)
                                          .Include(m => m.AñoAcademico)
                                          .AsQueryable(); // Usamos IQueryable para permitir filtrado dinámico

            // Filtrar por Año Académico si se ha seleccionado
            if (AñoAcademicoId.HasValue)
            {
                matriculas = matriculas.Where(m => m.AñoAcademicoId == AñoAcademicoId.Value);
            }

            // Filtrar por Fecha de Inicio si se ha proporcionado
            if (FechaInicio.HasValue)
            {
                // Asegurarse de comparar solo la fecha, sin considerar las horas
                var inicioSinHora = FechaInicio.Value.Date;
                matriculas = matriculas.Where(m => m.FechaMatricula >= inicioSinHora);
            }

            // Filtrar por Fecha de Fin si se ha proporcionado
            if (FechaFin.HasValue)
            {
                // Asegurarse de comparar solo la fecha, sin considerar las horas
                var finSinHora = FechaFin.Value.Date.AddDays(1).AddTicks(-1); // Para incluir todo el día de la fecha final
                matriculas = matriculas.Where(m => m.FechaMatricula <= finSinHora);
            }

            // Obtener la lista de matrículas filtradas
            var matriculasList = matriculas.ToList();

            // Obtener el nombre del Año Académico seleccionado
            string añoAcademicoNombre = null;
            if (AñoAcademicoId.HasValue)
            {
                var añoAcademico = db.AñosAcademicos.FirstOrDefault(a => a.Id == AñoAcademicoId.Value);
                if (añoAcademico != null)
                {
                    añoAcademicoNombre = añoAcademico.Nombre; // Asumiendo que "Nombre" es la propiedad que contiene el nombre del año
                }
            }

            // Pasar los datos de los años académicos al ViewBag para el dropdown
            ViewBag.AñosAcademicos = db.AñosAcademicos.ToList();

            // Pasar el nombre del Año Académico seleccionado al ViewBag
            ViewBag.AñoAcademicoNombre = añoAcademicoNombre;

            // Pasar las matrículas filtradas al modelo para la vista
            var viewModel = new VMMatricula
            {
                Matriculas = matriculasList
            };

            return View(viewModel);
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
