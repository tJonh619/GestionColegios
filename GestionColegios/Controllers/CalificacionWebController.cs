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
        private readonly BDColegioContainer db = new BDColegioContainer();

        // GET: CalificacionWeb
        public ActionResult Index(int id)
        {
            ViewBag.idCurso = id;
            var vm = new VMCalificaciones
            {
                Calificaciones = db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Materia).ToList(),
                Estudiantes = db.Estudiantes.ToList(),
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList(),
                CursosAcademicos = db.CursosAcademicos.ToList()// Asegúrate de tener una entidad Parcial
            };
            return View(vm);
        }


        public ActionResult GestionAcademica()
        {
            var vm = new VMCalificaciones
            {
                Calificaciones = db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Materia).ToList(),
                Estudiantes = db.Estudiantes.ToList(),
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList(),
                CursosAcademicos = db.CursosAcademicos.ToList()// Asegúrate de tener una entidad Parcial
            };
            return View(vm);
        }

        public ActionResult EstudiantesCurso(int id)
        {
            ViewBag.idCurso = id;
            var vm = new VMCalificaciones
            {
                Calificaciones = db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Materia).ToList(),
                Estudiantes = db.Estudiantes.ToList(),
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList(),
                CursosAcademicos = db.CursosAcademicos.ToList()// Asegúrate de tener una entidad Parcial
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

            var errorMessage = "";
            foreach (var calificacion in vm.Calificaciones)
            {
                if (calificacion.NCuantitativa == 0)
                {
                    errorMessage = "Error: Notas duplicadas o campos vacíos.";
                    break; // Salir del bucle después de encontrar el primer error
                }

                // Validar duplicados en el mismo parcial para el mismo estudiante y materia
                var calificacionExistente = db.Calificaciones
                    .FirstOrDefault(c => c.EstudianteId == calificacion.EstudianteId
                                          && c.MateriaId == calificacion.MateriaId
                                          && c.ParcialId == calificacion.ParcialId);

                if (calificacionExistente != null && calificacionExistente.NCuantitativa != 0)
                {
                    errorMessage = $"La materia {calificacionExistente.Materia.Nombre} ya tiene una calificación registrada para este parcial.";
                    break;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Pasar el mensaje de error a TempData para usarlo en la siguiente vista
                TempData["ErrorMessage"] = errorMessage;

                // Redirigir a la vista SeleccionarParciales con el Id del estudiante
                return RedirectToAction("SeleccionarParciales", new { estudianteId = vm.Estudiante.Id });
            }

            if (ModelState.IsValid)
            {
                foreach (var calificacion in vm.Calificaciones)
                {
                    var calificacionExistente = db.Calificaciones
                        .FirstOrDefault(c => c.EstudianteId == calificacion.EstudianteId
                                              && c.MateriaId == calificacion.MateriaId
                                              && c.ParcialId == calificacion.ParcialId);

                    if (calificacionExistente != null && calificacionExistente.NCuantitativa != 0)
                    {
                        ModelState.AddModelError("Calificaciones", $"La materia {calificacionExistente.Materia.Nombre} ya tiene una calificación registrada para este parcial.");
                        continue;
                    }
                    else
                    {
                        if (calificacion.NCuantitativa > 0 || !string.IsNullOrEmpty(calificacion.NCualitativa))
                        {
                            calificacion.EstudianteId = vm.Estudiante.Id;
                            db.Calificaciones.Add(calificacion);
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    TempData["SuccessMessage"] = "Calificaciones registradas exitosamente.";
                    db.SaveChanges();
                    return RedirectToAction("Create", new { estudianteId = vm.Estudiante.Id });
                }

                CargarDatos(vm);
                return View(vm);
            }

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

        public ActionResult Edit(int estudianteId, int parcialId = 1)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener las calificaciones actualizadas del estudiante y el parcial
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    Estudiante = estudiante,
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

        public ActionResult Edit2(int estudianteId, int parcialId = 2)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener las calificaciones actualizadas del estudiante y el parcial
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    Estudiante = estudiante,
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

        public ActionResult Edit3(int estudianteId, int parcialId = 3)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener las calificaciones actualizadas del estudiante y el parcial
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    Estudiante = estudiante,
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

        public ActionResult Edit4(int estudianteId, int parcialId = 4)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener las calificaciones actualizadas del estudiante y el parcial
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                var materias = db.Materias.ToList();
                calificaciones = materias.Select(m => new Calificacion
                {
                    Estudiante = estudiante,
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
        public ActionResult Edit(int estudianteId, VMCalificaciones vm)
        {
            if (ModelState.IsValid)
            {
                foreach (var calificacion in vm.Calificaciones)
                {
                 
                    var calificacionExistente = db.Calificaciones
                        .FirstOrDefault(c => c.EstudianteId == estudianteId
                                              && c.MateriaId == calificacion.MateriaId
                                              && c.ParcialId == calificacion.ParcialId);

                    if (calificacionExistente != null)
                    {
                        calificacionExistente.NCuantitativa = calificacion.NCuantitativa;
                        calificacionExistente.NCualitativa = calificacion.NCualitativa;
                    }
                    else
                    {
                        calificacion.EstudianteId = estudianteId; // Asegúrate de asignarlo aquí
                        db.Calificaciones.Add(calificacion);
                    }
                }

                db.SaveChanges();

                TempData["SuccessMessage"] = "Las calificaciones se han actualizado correctamente.";

                return RedirectToAction("SeleccionarParciales", new { estudianteId = vm.Estudiante.Id });

            }

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