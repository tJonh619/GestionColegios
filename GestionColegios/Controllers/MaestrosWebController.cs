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
    public class MaestrosWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: MaestrosWeb
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new VMMaestros { Maestros = db.Maestros.ToList(), Maestro = new Maestro(), Usuarios = db.Usuarios.ToList() };
            ViewBag.EsEdicion = false;
            return View(viewModel);
        }


        // GET: MaestrosWeb/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new VMMaestros
            {
                Maestro = new Maestro(),
                Usuarios = db.Usuarios.ToList() // Aquí cargamos la lista de usuarios

            };
            ViewBag.EsEdicion = false;
            return View("_Create_Edit", viewModel);
        }

        // POST: MaestrosWeb/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: MaestrosWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMMaestros model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                if (model.Maestro.Id == 0)
                {
                    // Creación del maestro
                    model.Maestro.Codigo = model.Maestro.Nombres.Substring(0, 3).ToUpper() +
                                           model.Maestro.Apellidos.Substring(0, 2).ToUpper() +
                                           new Random().Next(100, 1000).ToString();
                    model.Maestro.FechaModificacion = DateTime.Now;
                    model.Maestro.Activo = true;
                    db.Maestros.Add(model.Maestro);
                }
                else
                {
                    // Edición de maestro existente
                    var maestro = db.Maestros.SingleOrDefault(m => m.Id == model.Maestro.Id);
                    if (maestro == null) return HttpNotFound();

                    maestro.Cedula = model.Maestro.Cedula;
                    maestro.Nombres = model.Maestro.Nombres;
                    maestro.Apellidos = model.Maestro.Apellidos;
                    maestro.Sexo = model.Maestro.Sexo;
                    maestro.Celular = model.Maestro.Celular;
                    maestro.Direccion = model.Maestro.Direccion;
                    maestro.Especialidad = model.Maestro.Especialidad;
                    maestro.FechaContratacion = model.Maestro.FechaContratacion;
                    maestro.HorarioTrabajo = model.Maestro.HorarioTrabajo;
                    maestro.Nivel = model.Maestro.Nivel;
                    maestro.UsuarioId = model.Maestro.UsuarioId;  // Actualiza el UsuarioId
                    maestro.FechaModificacion = DateTime.Now;
                    maestro.Activo = model.Maestro.Activo;
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Maestro guardado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al guardar el maestro. Intente de nuevo.";
            model.Usuarios = db.Usuarios.ToList(); // Asegurarse de volver a pasar la lista de usuarios
            return View("_Create_Edit", model); // Retorna a la misma vista en caso de error
        }
        // GET: MaestrosWeb/Edit/5
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

            // Busca el maestro con el ID especificado
            Maestro maestro = db.Maestros.SingleOrDefault(m => m.Id == id);
            if (maestro == null)
            {
                return HttpNotFound();
            }

            // Crea el ViewModel para pasar los datos a la vista
            var viewModel = new VMMaestros
            {
                Maestro = maestro, // Maestro ya existente
                Usuarios = db.Usuarios.ToList() // Lista de usuarios
            };


            ViewBag.EsEdicion = true;
            return View("_Create_Edit", viewModel);
        }

        // POST: MaestrosWeb/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMMaestros model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                // Busca el maestro
                var maestro = db.Maestros.SingleOrDefault(m => m.Id == model.Maestro.Id);
                if (maestro == null)
                {
                    return HttpNotFound();
                }

                // Actualiza los datos del maestro
                maestro.Cedula = model.Maestro.Cedula;
                maestro.Nombres = model.Maestro.Nombres;
                maestro.Apellidos = model.Maestro.Apellidos;
                maestro.Sexo = model.Maestro.Sexo;
                maestro.Celular = model.Maestro.Celular;
                maestro.Direccion = model.Maestro.Direccion;
                maestro.Especialidad = model.Maestro.Especialidad;
                maestro.FechaContratacion = model.Maestro.FechaContratacion;
                maestro.HorarioTrabajo = model.Maestro.HorarioTrabajo;
                maestro.Nivel = model.Maestro.Nivel;
                maestro.FechaModificacion = DateTime.Now;
                maestro.Activo = model.Maestro.Activo;
                maestro.UsuarioId = model.Maestro.UsuarioId;  // Actualiza el UsuarioId

                // Marca el maestro como modificado y guarda los cambios
                db.Entry(maestro).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Maestro actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el maestro. Intente de nuevo.";
            model.Usuarios = db.Usuarios.ToList(); // Asegúrate de volver a cargar la lista de usuarios
            return View(model);
        }

        // GET: MaestrosWeb/Delete/5
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
            Maestro maestro = db.Maestros.Find(id);
            if (maestro == null)
            {
                return HttpNotFound();
            }
            return View(maestro);
        }

        // POST: MaestrosWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Maestro maestro = db.Maestros.Find(id);
            db.Maestros.Remove(maestro);
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
