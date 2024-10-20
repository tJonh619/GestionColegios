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
    public class EstudianteWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: EstudianteWeb
        public ActionResult Index()
        {
            var viewModel = new VMEstudiantes
            {
                Estudiantes = db.Estudiantes
                                .Include(e => e.Tutor)
                                .Where(e => e.Activo) // Filtrar solo estudiantes activos
                                .ToList(),
                Estudiante = new Estudiante(),
                Tutor = new Tutor() // Agregamos un nuevo tutor aquí
            };
            return View(viewModel);
        }

        // Método para listar los estudiantes
        public ActionResult Listar()
        {
            var estudiantes = db.Estudiantes
                                .Include(e => e.Tutor)
                                .Where(e => e.Activo) // Filtrar solo estudiantes activos
                                .ToList();
            return PartialView("_ListaEstudiantes", estudiantes); // Asegúrate de que la vista parcial existe y se llama _ListaEstudiantes.cshtml
        }

        // GET: EstudianteWeb/Create
        public ActionResult Create()
        {
            var viewModel = new VMEstudiantes
            {
                Estudiante = new Estudiante(),
                Tutor = new Tutor() // Inicializa un nuevo Tutor
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMEstudiantes model)
        {
            if (ModelState.IsValid)
            {
                // Guarda el tutor primero
                if (model.Tutor != null)
                {
                    model.Tutor.FechaModificacion = DateTime.Now;
                    model.Tutor.Activo = true;
                    db.Tutores.Add(model.Tutor);
                    db.SaveChanges(); // Guarda el tutor y obtiene su ID
                    TempData["SuccessMessage"] = "Tutor guardado correctamente.";
                }

                // Asigna el ID del tutor al estudiante
                model.Estudiante.TutorId = model.Tutor.Id;

                // Genera el código del estudiante
                string codigo = model.Estudiante.Nombres.Substring(0, 2).ToUpper() + model.Estudiante.Apellidos.Substring(0, 2).ToUpper() + new Random().Next(100, 1000).ToString();
                model.Estudiante.CodigoEstudiante = codigo;
                
            // Cambia esto según tu lógica
                model.Estudiante.FechaModificacion = DateTime.Now;
                model.Estudiante.Activo = true;

                // Guarda el estudiante
                db.Estudiantes.Add(model.Estudiante);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Estudiantes guardado correctamente.";

                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Error al guardar Estudiante y tutor. Intente de nuevo.";
            return View(model); // Si hay un error, retorna el modelo para mostrar errores
        }

        // GET: EstudianteWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estudiante estudiante = db.Estudiantes.Include(e => e.Tutor).SingleOrDefault(e => e.Id == id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMEstudiantes
            {
                Estudiante = estudiante,
                Tutor = estudiante.Tutor // Asumiendo que tienes una propiedad de Tutor en Estudiante
            };
            return View(viewModel);
        }

        // POST: EstudianteWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMEstudiantes model)
        {
            if (ModelState.IsValid)
            {
                // Actualiza el estudiante
                var estudiante = db.Estudiantes.Include(e => e.Tutor).SingleOrDefault(e => e.Id == model.Estudiante.Id);
                if (estudiante == null)
                {
                    return HttpNotFound();
                }

                // Actualiza los datos del estudiante
                estudiante.CodigoEstudiante = model.Estudiante.CodigoEstudiante;
                estudiante.Nombres = model.Estudiante.Nombres;
                estudiante.Apellidos = model.Estudiante.Apellidos;
                estudiante.FechaNacimiento = model.Estudiante.FechaNacimiento;
                estudiante.Edad = model.Estudiante.Edad;
                estudiante.Sexo = model.Estudiante.Sexo;
                estudiante.Direccion = model.Estudiante.Direccion;
                estudiante.FechaModificacion = DateTime.Now; // Actualiza la fecha de modificación
                estudiante.Activo = model.Estudiante.Activo;

                // Actualiza el tutor solo si ya existe en la base de datos
                if (model.Tutor != null && model.Tutor.Id > 0)
                {
                    var tutorExistente = db.Tutores.Find(model.Tutor.Id);
                    if (tutorExistente != null)
                    {
                        tutorExistente.Nombres = model.Tutor.Nombres;
                        tutorExistente.Apellidos = model.Tutor.Apellidos;
                        tutorExistente.Cedula = model.Tutor.Cedula;
                        tutorExistente.RelacionConElEstudiante = model.Tutor.RelacionConElEstudiante;
                        tutorExistente.Direccion = model.Tutor.Direccion;
                        tutorExistente.Celular = model.Tutor.Celular;
                        tutorExistente.FechaModificacion = DateTime.Now; // Actualiza la fecha de modificación
                    }
                    else
                    {
                        ModelState.AddModelError("Tutor", "El tutor no existe.");
                        return View(model);
                    }
                }

                // Guarda los cambios
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: EstudianteWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Asegúrate de que estás utilizando Include para cargar el Tutor
            Estudiante estudiante = db.Estudiantes
                                       .Include(e => e.Tutor) // Esto carga el tutor
                                       .SingleOrDefault(e => e.Id == id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Crear el ViewModel y asignar el estudiante y el tutor
            var viewModel = new VMEstudiantes
            {
                Estudiante = estudiante,
                Tutor = estudiante.Tutor // Asignar el tutor al ViewModel
            };

            // Pasar el ViewModel a la vista
            return View(viewModel);
        }
        // POST: EstudianteWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Buscar el estudiante
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante != null)
            {
                estudiante.Activo = false; // Desactivar estudiante
                estudiante.FechaModificacion = DateTime.Now; // Actualiza la fecha de modificación

                // Si el estudiante tiene un tutor, desactivarlo también
                if (estudiante.Tutor != null)
                {
                    Tutor tutor = db.Tutores.Find(estudiante.TutorId);
                    if (tutor != null)
                    {
                        tutor.Activo = false; // Desactivar tutor
                        tutor.FechaModificacion = DateTime.Now; // Actualiza la fecha de modificación
                    }
                }

                // Guardar cambios en la base de datos
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
