using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionColegios.Models;
using GestionColegios.ViewModels;

namespace GestionColegios.Controllers
{
    public class CalificacionWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CalificacionWeb
        public ActionResult Index()
        {
            var vm = new VMCalificaciones
            {
                Calificaciones = db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Materia).ToList(),
                Estudiantes = db.Estudiantes.ToList(),
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList() // Asegúrate de tener una entidad Parcial
            };
            return View(vm);
        }

        public ActionResult Create(int estudianteId, int parcialId = 1)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener las calificaciones para el estudiante y el parcial actual
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    EstudianteId = estudiante.Id,
                    MateriaId = m.Id,
                    ParcialId = parcialId,
                    NCuantitativa = 0, // Inicializa con 0 o null, según lo que sea necesario
                    NCualitativa = null // Puede ser null si no hay calificación cualitativa
                }).ToList();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Materias = db.Materias.ToList(),
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCalificaciones vm)
        {
            if (vm.Calificaciones == null)
            {
                vm.Calificaciones = new List<Calificacion>();
            }

            // Validación de que todos los campos de notas cuantitativas estén llenos
            foreach (var calificacion in vm.Calificaciones)
            {
                if (calificacion.NCuantitativa == 0 || calificacion.NCuantitativa == null)
                {
                    ModelState.AddModelError("Calificaciones", "Error Campos vacios o nota duplicada");
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var calificacion in vm.Calificaciones)
                {
                    // Verificar si ya existe una calificación para el mismo estudiante, materia y parcial
                    var calificacionExistente = db.Calificaciones
                        .FirstOrDefault(c => c.EstudianteId == calificacion.EstudianteId
                                              && c.MateriaId == calificacion.MateriaId
                                              && c.ParcialId == calificacion.ParcialId);

                    if (calificacionExistente != null && calificacionExistente.NCuantitativa != 0 && calificacionExistente.NCuantitativa != null)
                    {
                        // Si ya existe, mostrar un mensaje de error y no guardar la calificación
                        ModelState.AddModelError("Calificaciones",
                            $"La materia {calificacionExistente.Materia.Nombre} ya tiene una calificación registrada para este parcial.");
                    }
                    else
                    {
                        // Si no existe, agregar la nueva calificación solo si tiene valores
                        if (calificacion.NCuantitativa > 0 || !string.IsNullOrEmpty(calificacion.NCualitativa))
                        {
                            calificacion.EstudianteId = vm.Estudiante.Id;
                            calificacion.MateriaId = calificacion.MateriaId;
                            calificacion.ParcialId = calificacion.ParcialId;

                            db.Calificaciones.Add(calificacion); // Agregar la calificación al contexto
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    db.SaveChanges(); // Guardar cambios en la base de datos
                    TempData["SuccessMessage"] = "Calificaciones registradas exitosamente.";
                    return RedirectToAction("Index"); // Redirigir a la lista de calificaciones
                }
            }

            // Si el ModelState no es válido, recargar los datos necesarios para que se muestren en la vista
            CargarDatos(vm);
            return View(vm);
        }




        private void CargarDatos(VMCalificaciones vm)
        {
            vm.Estudiantes = db.Estudiantes.ToList();
            vm.Materias = db.Materias.ToList();
            vm.Parciales = db.Parciales.ToList();
        }

        public ActionResult SeleccionarParciales(int estudianteId)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Parciales = db.Parciales.ToList() // Asegúrate de tener la lista de parciales
            };

            return View(vm); // Retorna el modelo adecuado a la vista
        }

        public ActionResult Parcial2(int estudianteId)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == 2)
                .ToList();

            // Si no existen calificaciones para Parcial 2, inicializa una lista en blanco para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    EstudianteId = estudiante.Id,
                    MateriaId = m.Id,
                    ParcialId = 3
                }).ToList();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Materias = db.Materias.ToList(),
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }

        public ActionResult Parcial3(int estudianteId)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == 3)
                .ToList();

            // Si no existen calificaciones para Parcial 3, inicializa una lista en blanco para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    EstudianteId = estudiante.Id,
                    MateriaId = m.Id,
                    ParcialId = 4
                }).ToList();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Materias = db.Materias.ToList(),
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }


        public ActionResult Parcial4(int estudianteId)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == 4)
                .ToList();

            // Si no existen calificaciones para Parcial 4, inicializa una lista en blanco para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    EstudianteId = estudiante.Id,
                    MateriaId = m.Id,
                    ParcialId = 5
                }).ToList();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Materias = db.Materias.ToList(),
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }

        public ActionResult VerCalificaciones(int estudianteId)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener todas las calificaciones del estudiante
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudianteId)
                .Include(c => c.Materia) // Asegúrate de que Materia está incluido para acceder a su nombre
                .ToList();

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Calificaciones = calificaciones,
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
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