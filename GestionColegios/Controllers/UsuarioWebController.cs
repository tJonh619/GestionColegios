using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GestionColegios.Models;
using GestionColegios.ViewModels;

namespace GestionColegios.Controllers
{
    public class UsuarioWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: UsuarioWeb
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Rol).ToList();
            var roles = db.Roles.ToList();
            var permisos = db.Permisos.ToList();

            var viewModel = new VMUsuario
            {
                Usuarios = usuarios,
                Roles = roles,
                Permisos = permisos,
                Usuario = new Usuario() // Inicializa un nuevo usuario para el formulario
            };

            return View(viewModel); // Pasa el VMUsuario a la vista
        }

        // POST: UsuarioWeb/CreateUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsuario([Bind(Include = "Id,NombreUsuario,ClaveHash,CorreoRecuperacion,FechaModificacion,Activo,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaModificacion = DateTime.Now;
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        // GET: UsuarioWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roles = db.Roles.ToList();
            return View(usuario);
        }

        // POST: UsuarioWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreUsuario,ClaveHash,CorreoRecuperacion,FechaModificacion,Activo,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaModificacion = DateTime.Now;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: UsuarioWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: UsuarioWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: UsuarioWeb/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole([Bind(Include = "Id,Codigo,Nombre,Descripcion")] Rol rol) // Elimina FechaModificacion y Activo del Bind
        {
            if (ModelState.IsValid)
            {
                rol.FechaModificacion = DateTime.Now; // Establece la fecha de modificación
                rol.Activo = true; // Marca el rol como activo

                db.Roles.Add(rol); // Agrega el nuevo rol a la base de datos

                try
                {
                    db.SaveChanges(); // Guarda los cambios
                    return RedirectToAction("Index"); // Redirige a la lista de roles
                }
                catch (DbEntityValidationException ex)
                {
                    // Manejo de errores de validación
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            // Si ModelState no es válido o ocurre un error, vuelve a mostrar la vista con el modelo
            return View(rol); // Devuelve la vista con el modelo para mostrar los errores de validación
        }

        // GET: UsuarioWeb/EditRole/5
        public ActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Roles.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // POST: UsuarioWeb/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole([Bind(Include = "Id,Codigo,Descripcion,FechaModificacion,Activo")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                rol.FechaModificacion = DateTime.Now;
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        // GET: UsuarioWeb/DeleteRole/5
        public ActionResult DeleteRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Roles.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        // POST: UsuarioWeb/DeleteRole/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(int id)
        {
            Rol rol = db.Roles.Find(id);
            db.Roles.Remove(rol);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: UsuarioWeb/CreatePermiso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePermiso([Bind(Include = "Id,Nombre,Descripcion,FechaModificacion,Activo,RolId")] Permiso permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    permiso.FechaModificacion = DateTime.Now;
                    permiso.Activo = true;

                    // Validar que RolId sea válido
                    var rol = db.Roles.Find(permiso.RolId);
                    if (rol == null)
                    {
                        ModelState.AddModelError("RolId", "El rol seleccionado no es válido.");
                        return View(permiso);
                    }

                    db.Permisos.Add(permiso);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return View(permiso);  // Regresar la vista con los errores mostrados
            }

            return View(permiso);  // En caso de error, retornar la vista con los datos
        }

        // GET: UsuarioWeb/EditPermiso/5
        public ActionResult EditPermiso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permiso permiso = db.Permisos.Find(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        // POST: UsuarioWeb/EditPermiso/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermiso([Bind(Include = "Id,Descripcion,FechaModificacion,Activo")] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                permiso.FechaModificacion = DateTime.Now;
                db.Entry(permiso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permiso);
        }

        // GET: UsuarioWeb/DeletePermiso/5
        public ActionResult DeletePermiso(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permiso permiso = db.Permisos.Find(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        // POST: UsuarioWeb/DeletePermiso/5
        [HttpPost, ActionName("DeletePermiso")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePermisoConfirmed(int id)
        {
            Permiso permiso = db.Permisos.Find(id);
            db.Permisos.Remove(permiso);
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
