using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public ActionResult CreateUsuario([Bind(Include = "Id,CodigoUsuario,NombreUsuario,ClaveHash,CorreoRecuperacion,FechaModificacion,Activo,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaModificacion = DateTime.Now;
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Usuario guardado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al guardar Usuario. Intente de nuevo.";
            // Si ocurre un error, vuelve a cargar la lista de roles
            var model = new VMUsuario
            {
                Roles = db.Roles.ToList(),
                Usuario = usuario
            };
        
            return View(model);
          
        }
        // GET: UsuarioWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            // Validar si el ID es nulo
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Buscar el usuario en la base de datos
            Usuario usuario = db.Usuarios.Include(u => u.Rol).SingleOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            // Crear el ViewModel
            var viewModel = new VMUsuario
            {
                Usuario = usuario,
                Roles = db.Roles.ToList() // Obtener la lista de roles
            };

            // Retornar la vista de edición con el ViewModel
            return View(viewModel);
        }

        // POST: UsuarioWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                // Buscar el usuario en la base de datos
                var usuario = db.Usuarios.Find(viewModel.Usuario.Id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                // Actualizar los datos del usuario
                usuario.NombreUsuario = viewModel.Usuario.NombreUsuario;
                usuario.ClaveHash = viewModel.Usuario.ClaveHash;
                usuario.CorreoRecuperacion = viewModel.Usuario.CorreoRecuperacion;
                usuario.RolId = viewModel.Usuario.RolId;
                usuario.Activo = viewModel.Usuario.Activo;
                usuario.FechaModificacion = DateTime.Now;

                // Marcar el estado del usuario como modificado
                db.Entry(usuario).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                // Redirigir a la acción Index
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, volver a cargar la lista de roles en caso de error
            viewModel.Roles = db.Roles.ToList();
            return View(viewModel);
        }

        // GET: UsuarioWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Encuentra el usuario
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            // Crear el ViewModel y asignar el usuario
            var viewModel = new VMUsuario
            {
                Usuario = usuario
            };

            // Pasa el ViewModel a la vista
            return View(viewModel);
        }

        // POST: UsuarioWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);

            if (usuario != null)
            {
                // Marcar el usuario como inactivo
                usuario.Activo = false;
                usuario.FechaModificacion = DateTime.Now; // Registrar la fecha de modificación
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }

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
                    TempData["SuccessMessage"] = "Rol guardado correctamente.";
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
            TempData["ErrorMessage"] = "Error al guardar Rol. Intente de nuevo.";
            return View(rol); // Devuelve la vista con el modelo para mostrar los errores de validación
        }

        // GET: UsuarioWeb/EditRole/5
        public ActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Busca el rol en la base de datos usando el ID
            Rol rol = db.Roles.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }

            // Retorna la vista EditRole en la carpeta UsuarioWeb
            return View(rol);
        }

        // POST: UsuarioWeb/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole([Bind(Include = "Id,Codigo,Nombre,Descripcion,FechaModificacion,Activo")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                // Actualiza la fecha de modificación y marca el rol como modificado
                rol.FechaModificacion = DateTime.Now;
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();

                // Redirige al Index después de guardar
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, retorna la misma vista con los errores
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
            return View(rol); // Asegúrate de que estás usando la vista correcta
        }

        // POST: UsuarioWeb/DeleteRole/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(int id)
        {
            Rol rol = db.Roles.Find(id);
            if (rol != null)
            {
                rol.Activo = false; // Marcar el rol como inactivo
                rol.FechaModificacion = DateTime.Now; // Registrar la fecha de modificación
                db.Entry(rol).State = EntityState.Modified; // Marcar el estado como modificado
                db.SaveChanges();
            }
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
                    TempData["SuccessMessage"] = "Permiso guardado correctamente.";
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
            TempData["ErrorMessage"] = "Error al guardar Permiso. Intente de nuevo.";
            return View(permiso);  // En caso de error, retornar la vista con los datos
        }

        // GET: UsuarioWeb/EditPermiso/5
        public ActionResult EditPermiso(int? id)
        {
            // Validar si el ID es nulo
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Buscar el permiso en la base de datos
            Permiso permiso = db.Permisos.Find(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }

            // Si necesitas listar los roles disponibles, podrías hacerlo aquí
            ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre", permiso.RolId);

            // Retornar la vista de edición con el modelo del permiso
            return View(permiso);
        }

        // POST: UsuarioWeb/EditPermiso/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermiso([Bind(Include = "Id,Nombre,Descripcion,FechaModificacion,Activo,RolId")] Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                // Actualizar la fecha de modificación
                permiso.FechaModificacion = DateTime.Now;

                // Marcar el estado del permiso como modificado
                db.Entry(permiso).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                // Redirigir a la acción Index
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, retornar la misma vista con los errores
            return View(permiso);
        }

        // GET: UsuarioWeb/DeletePermission/5
        public ActionResult DeletePermission(int? id)
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
            return View(permiso); // Asegúrate de que estás usando la vista correcta
        }

        // POST: UsuarioWeb/DeletePermission/5
        [HttpPost, ActionName("DeletePermission")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePermissionConfirmed(int id)
        {
            Permiso permiso = db.Permisos.Find(id);
            if (permiso != null)
            {
                permiso.Activo = false; // Marcar el permiso como inactivo
                permiso.FechaModificacion = DateTime.Now; // Registrar la fecha de modificación
                db.Entry(permiso).State = EntityState.Modified; // Marcar el estado como modificado
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario model)
        {
            var usuario = db.Usuarios.SingleOrDefault(u => u.NombreUsuario == model.NombreUsuario && u.ClaveHash == model.ClaveHash);
            if (usuario != null)
            {
                Session["Usuario"] = usuario.NombreUsuario;
                Session["Rol"] = usuario.Rol.Nombre; // Guarda el rol en la sesión
                return RedirectToAction("Index", "Home"); // Redirige después de iniciar sesión
            }
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Limpia la sesión al cerrar sesión
            return RedirectToAction("Login", "UsuarioWeb");
        }

        // GET: UsuarioWeb/OlvidasteContrasena
        public ActionResult OlvidasteContrasena()
        {
            var model = new Usuario(); // Inicializa el modelo de Usuario
            return View(model); // Muestra la vista para ingresar el nombre de usuario
        }

        // POST: UsuarioWeb/OlvidasteContrasena
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OlvidasteContrasena(Usuario model)
        {
            if (ModelState.IsValid)
            {
                // Busca el usuario por el nombre de usuario ingresado
                var usuario = db.Usuarios.FirstOrDefault(u => u.NombreUsuario == model.NombreUsuario);

                if (usuario != null)
                {
                    // Redirige a la vista para cambiar la contraseña con el id del usuario
                    return RedirectToAction("CambiarContrasena", new { usuarioId = usuario.Id });
                }
                else
                {
                    ModelState.AddModelError("", "No se encontró un usuario con ese nombre.");
                }
            }

            return View(model); // Si hay errores, se muestra la misma vista
        }

        // GET: UsuarioWeb/CambiarContrasena
        public ActionResult CambiarContrasena(int usuarioId)
        {
            var usuario = db.Usuarios.Find(usuarioId);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            // Enviamos el usuario encontrado a la vista
            return View(usuario);
        }

        // POST: UsuarioWeb/CambiarContrasena
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.Find(model.Id);

                if (usuario != null)
                {
                    // Actualizamos la contraseña del usuario
                    usuario.ClaveHash = model.ClaveHash; // Asegúrate de encriptarla si es necesario
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Tu contraseña ha sido cambiada exitosamente.";
                    return RedirectToAction("Login");
                }
            }

            return View(model); // Si hay errores, se muestra la misma vista
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
