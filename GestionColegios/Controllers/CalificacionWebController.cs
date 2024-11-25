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
            // Verificar si el usuario está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                // Si no está autenticado, redirigir a la página de inicio de sesión
                return RedirectToAction("Login", "Account");
            }

            ViewBag.idCurso = id; // Almacenar el ID del curso en ViewBag para utilizarlo en la vista

            var curso = db.CursosAcademicos.Where(c => c.Id == id).FirstOrDefault(); // Buscar el curso académico correspondiente al ID proporcionado


            var Año = curso.AñoAcademico.Id; // Obtener el ID del año académico asociado al curso
            var matriculas = db.Matriculas.Where(m => m.AñoAcademicoId == Año && m.Periodos.AñoId == curso.AñoId).ToList(); // Buscar todas las matrículas que correspondan al año académico y periodo del curso


            // Obtener la lista de estudiantes asociados a esas matrículas
            var estudiantesMatriculados = matriculas.Select(m => m.Estudiante).ToList();

            // Crear un ViewModel y asignar los datos necesarios para la vista
            var vm = new VMCalificaciones
            {
                Estudiantes = db.Estudiantes.ToList(), // Lista de estudiantes seleccionados
                Matriculas = matriculas,
                Materias = db.Materias
                                      .Where(m => m.AñoAcademicoId == curso.AñoAcademicoId) // Filtrar materias según el curso académico
                                      .ToList(), // Convertir a lista para enviar a la vista
                Parciales = db.Parciales.ToList(), // Obtener todos los parciales
                CursosAcademicos = db.CursosAcademicos.ToList(), // Obtener todos los cursos académicos
                CursoAcademico = curso,
                AñoAcademicos = db.AñosAcademicos.ToList() // Obtener todos los años académicos
            };

            // Retornar la vista con el ViewModel generado
            return View(vm);
        }




        public ActionResult GestionAcademica()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Si no está autenticado, redirigir a la página de inicio de sesión
                return RedirectToAction("Login", "Account");
            }

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

            // Obtener la última matrícula del estudiante (la más reciente por fecha)
            var ultimaMatricula = db.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id)
                .OrderByDescending(m => m.FechaMatricula) // Asumiendo que Fecha es la propiedad que indica la fecha de matrícula
                .FirstOrDefault();

            if (ultimaMatricula == null)
            {
                return HttpNotFound(); // Si no hay matrícula, se retorna un error
            }

            // Obtener el año académico de la última matrícula
            var anioAcademico = ultimaMatricula.AñoAcademicoId;

            // Obtener las materias correspondientes a ese año académico
            var materiasPorAnioAcademico = db.Materias
                .Where(m => m.AñoAcademicoId == anioAcademico) // Asumiendo que Materia tiene la propiedad AnioAcademico
                .ToList();

            // Obtener las calificaciones para el estudiante y el parcial actual
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                calificaciones = materiasPorAnioAcademico.Select(m => new Calificacion
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
                Materias = materiasPorAnioAcademico, // Aquí solo pasamos las materias del año académico de la última matrícula
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

        public ActionResult Parcial2(int estudianteId, int parcialId = 2)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener la última matrícula del estudiante (la más reciente por fecha)
            var ultimaMatricula = db.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id)
                .OrderByDescending(m => m.FechaMatricula) // Asumiendo que Fecha es la propiedad que indica la fecha de matrícula
                .FirstOrDefault();

            if (ultimaMatricula == null)
            {
                return HttpNotFound(); // Si no hay matrícula, se retorna un error
            }

            // Obtener el año académico de la última matrícula
            var anioAcademico = ultimaMatricula.AñoAcademicoId;

            // Obtener las materias correspondientes a ese año académico
            var materiasPorAnioAcademico = db.Materias
                .Where(m => m.AñoAcademicoId == anioAcademico) // Asumiendo que Materia tiene la propiedad AnioAcademico
                .ToList();

            // Obtener las calificaciones para el estudiante y el parcial actual
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                calificaciones = materiasPorAnioAcademico.Select(m => new Calificacion
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
                Materias = materiasPorAnioAcademico, // Aquí solo pasamos las materias del año académico de la última matrícula
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }

        public ActionResult Parcial3(int estudianteId, int parcialId = 3)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener la última matrícula del estudiante (la más reciente por fecha)
            var ultimaMatricula = db.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id)
                .OrderByDescending(m => m.FechaMatricula) // Asumiendo que Fecha es la propiedad que indica la fecha de matrícula
                .FirstOrDefault();

            if (ultimaMatricula == null)
            {
                return HttpNotFound(); // Si no hay matrícula, se retorna un error
            }

            // Obtener el año académico de la última matrícula
            var anioAcademico = ultimaMatricula.AñoAcademicoId;

            // Obtener las materias correspondientes a ese año académico
            var materiasPorAnioAcademico = db.Materias
                .Where(m => m.AñoAcademicoId == anioAcademico) // Asumiendo que Materia tiene la propiedad AnioAcademico
                .ToList();

            // Obtener las calificaciones para el estudiante y el parcial actual
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                calificaciones = materiasPorAnioAcademico.Select(m => new Calificacion
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
                Materias = materiasPorAnioAcademico, // Aquí solo pasamos las materias del año académico de la última matrícula
                Calificaciones = calificaciones,
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }


        public ActionResult Parcial4(int estudianteId, int parcialId = 4)
        {
            var estudiante = db.Estudiantes.Find(estudianteId);
            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener la última matrícula del estudiante (la más reciente por fecha)
            var ultimaMatricula = db.Matriculas
                .Where(m => m.EstudianteId == estudiante.Id)
                .OrderByDescending(m => m.FechaMatricula) // Asumiendo que Fecha es la propiedad que indica la fecha de matrícula
                .FirstOrDefault();

            if (ultimaMatricula == null)
            {
                return HttpNotFound(); // Si no hay matrícula, se retorna un error
            }

            // Obtener el año académico de la última matrícula
            var anioAcademico = ultimaMatricula.AñoAcademicoId;

            // Obtener las materias correspondientes a ese año académico
            var materiasPorAnioAcademico = db.Materias
                .Where(m => m.AñoAcademicoId == anioAcademico) // Asumiendo que Materia tiene la propiedad AnioAcademico
                .ToList();

            // Obtener las calificaciones para el estudiante y el parcial actual
            var calificaciones = db.Calificaciones
                .Where(c => c.EstudianteId == estudiante.Id && c.ParcialId == parcialId)
                .ToList();

            // Si no existen calificaciones para ese parcial, inicializa una lista vacía para cada materia
            if (!calificaciones.Any())
            {
                calificaciones = materiasPorAnioAcademico.Select(m => new Calificacion
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
                Materias = materiasPorAnioAcademico, // Aquí solo pasamos las materias del año académico de la última matrícula
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

        [HttpPost]
        public ActionResult SeleccionarEstudiantes(int? anioAcademico, DateTime? fechaMatricula)
        {
            // Filtra los estudiantes según los parámetros recibidos
            var estudiantes = db.Estudiantes.AsQueryable();

            if (anioAcademico.HasValue)
            {
                estudiantes = estudiantes.Where(e => e.Matricula.Any(m => m.AñoAcademicoId == anioAcademico.Value));
            }

            if (fechaMatricula.HasValue)
            {
                estudiantes = estudiantes.Where(e => e.Matricula.Any(m => m.FechaMatricula >= fechaMatricula.Value));
            }

            // Devuelve solo el partial view con los estudiantes filtrados
            return PartialView("_TablaEstudianteModal", estudiantes.ToList());
        }



        [HttpPost]
        public ActionResult AgregarEstudiantes(List<int> estudiantesSeleccionados, int id)
        {
            if (estudiantesSeleccionados != null && estudiantesSeleccionados.Any())
            {
                bool estudiantesAgregados = false;

                foreach (var estudianteId in estudiantesSeleccionados)
                {
                    // Verifica si el estudiante ya está asociado al curso académico
                    var cursoAcademicoEstudianteExistente = db.CursosAcademicosEstudiantes
                                                              .FirstOrDefault(cae => cae.EstudianteId == estudianteId && cae.CursoAcademicoId == id);
                    if (cursoAcademicoEstudianteExistente != null)
                    {
                        // Si el estudiante ya está en ese curso académico, mostrar mensaje
                        TempData["ErrorMessage"] = $"El estudiante con ID {estudianteId} ya está asociado a este curso académico.";
                        return RedirectToAction("GestionAcademica");
                    }

                    // Lógica para asociar los estudiantes seleccionados con el curso académico
                    var estudiante = db.Estudiantes.Find(estudianteId);
                    if (estudiante != null)
                    {
                        var cursoAcademicoEstudiante = new CursoAcademicoEstudiante
                        {
                            EstudianteId = estudianteId,
                            CursoAcademicoId = id,
                            Estado = "Activo", // O el valor adecuado según tu lógica
                            FechaModificacion = DateTime.Now,
                            Activo = true
                        };

                        db.CursosAcademicosEstudiantes.Add(cursoAcademicoEstudiante);
                        estudiantesAgregados = true; // Indicar que al menos un estudiante fue agregado
                    }
                }

                // Solo guardar los cambios si algún estudiante fue agregado
                if (estudiantesAgregados)
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Los estudiantes se han agregado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se seleccionaron estudiantes válidos para agregar.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se seleccionaron estudiantes.";
            }

            return RedirectToAction("GestionAcademica");
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