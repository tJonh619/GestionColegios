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

        // GET: CalificacionWeb/Create
        public ActionResult Create(int estudianteId)
        {
            var estudiante = db.Estudiantes.AsNoTracking().FirstOrDefault(e => e.Id == estudianteId);

            if (estudiante == null)
            {
                return HttpNotFound();
            }

            var vm = new VMCalificaciones
            {
                Estudiante = estudiante,
                Calificacion = new Calificacion
                {
                    EstudianteId = estudiante.Id
                },
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }


        // POST: CalificacionWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCalificaciones vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Calificacion.EstudianteId == 0)
                {
                    ModelState.AddModelError("", "El estudiante no se encontró o no fue seleccionado.");
                    CargarDatos(vm);
                    return View(vm);
                }

                // Iterar sobre las calificaciones y asignar el EstudianteId y MateriaId
                foreach (var calificacion in vm.Calificaciones)
                {
                    if (calificacion.NCuantitativa != 0 || !string.IsNullOrEmpty(calificacion.NCualitativa))
                    {
                        calificacion.EstudianteId = vm.Calificacion.EstudianteId;

                        // Verifica que la MateriaId sea válida
                        if (db.Materias.Any(m => m.Id == calificacion.MateriaId))
                        {
                            calificacion.Activo = true;
                            db.Calificaciones.Add(calificacion);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"La materia con ID {calificacion.MateriaId} no existe.");
                        }
                    }
                }

                // Guarda los cambios en la base de datos
                try
                {
                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error al guardar los datos: " + ex.InnerException?.Message);
                }
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
        // POST: CalificacionWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,NParcial1,NCualitativa1,NParcial2,NCualitativa2,NParcial3,NCualitativa3,NParcial4,NCualitativa4,Activo,EstudianteId,MateriaId")] Calificacion calificacion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Calificaciones.Add(calificacion);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.EstudianteId = new SelectList(db.Estudiantes, "Id", "Nombres", calificacion.EstudianteId);
        //    ViewBag.MateriaId = new SelectList(db.Materias, "Id", "Nombre", calificacion.MateriaId);
        //    return View(calificacion);
        //}

        // GET: CalificacionWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calificacion calificacion = db.Calificaciones.Find(id);
            if (calificacion == null)
            {
                return HttpNotFound();
            }

            var vm = new VMCalificaciones
            {
                Calificacion = calificacion,
                Estudiantes = db.Estudiantes.ToList(),
                Materias = db.Materias.ToList(),
                Parciales = db.Parciales.ToList()
            };

            return View(vm);
        }

        // POST: CalificacionWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCalificaciones vm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vm.Calificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            vm.Estudiantes = db.Estudiantes.ToList();
            vm.Materias = db.Materias.ToList();
            vm.Parciales = db.Parciales.ToList();
            return View(vm);
        }

        // GET: CalificacionWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calificacion calificacion = db.Calificaciones.Find(id);
            if (calificacion == null)
            {
                return HttpNotFound();
            }
            return View(calificacion);
        }

        // POST: CalificacionWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Calificacion calificacion = db.Calificaciones.Find(id);
            db.Calificaciones.Remove(calificacion);
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