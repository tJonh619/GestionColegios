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
        private readonly BDColegioContainer db = new BDColegioContainer();

        // GET: UsuarioWeb
        public ActionResult Index()
        {
            var viewModel = new VMUsuario
            {
                Usuarios = db.Usuarios.Include(u => u.Rol).ToList(),
                Roles = db.Roles.ToList(),
                Permisos = db.Permisos.ToList(),
                Usuario = new Usuario(), // Nuevo usuario para el formulario
                Rol = new Rol(),
                Permiso = new Permiso()
            };
            ViewBag.EsEdicion = false; // Indicador para la vista
            return View(viewModel);
        }

        // GET: UsuarioWeb/Create
        public ActionResult Create()
        {
            var viewModel = new VMUsuario
            {
                Usuario = new Usuario(),
                Roles = db.Roles.ToList()
            };
            ViewBag.EsEdicion = false;
            return View("_AgregarUsuario", viewModel); // Vista reutilizable para crear/editar
        }

        // POST: UsuarioWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                model.Usuario.FechaModificacion = DateTime.Now;
                db.Usuarios.Add(model.Usuario);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Usuario creado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al crear el usuario. Intente de nuevo.";
            model.Roles = db.Roles.ToList(); // Recarga los roles si ocurre error
            return View("_AgregarUsuario", model);
        }

        // GET: UsuarioWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var usuario = db.Usuarios.Include(u => u.Rol).SingleOrDefault(u => u.Id == id);
            if (usuario == null)
                return HttpNotFound();

            var viewModel = new VMUsuario
            {
                Usuario = usuario,
                Roles = db.Roles.ToList()
            };
            ViewBag.EsEdicion = true;
            return View("_AgregarUsuario", viewModel);
        }

        // POST: UsuarioWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.Find(model.Usuario.Id);
                if (usuario == null)
                    return HttpNotFound();

                // Actualización de datos del usuario
                usuario.NombreUsuario = model.Usuario.NombreUsuario;
                usuario.ClaveHash = model.Usuario.ClaveHash;
                usuario.CorreoRecuperacion = model.Usuario.CorreoRecuperacion;
                usuario.RolId = model.Usuario.RolId;
                usuario.Activo = model.Usuario.Activo;
                usuario.FechaModificacion = DateTime.Now;

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Usuario actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error al actualizar el usuario. Intente de nuevo.";
            model.Roles = db.Roles.ToList(); // Recarga roles si falla la validación
            return View("_AgregarUsuario", model);
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
        public ActionResult CreateRole()
        {
            var rol = new Rol();
            ViewBag.EsEdicion = false;
            return View("_AgregarRol", rol); // Vista compartida para crear/editar roles
        }

        // POST: UsuarioWeb/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(Rol rol)
        {
            if (ModelState.IsValid)
            {
                rol.FechaModificacion = DateTime.Now;
                rol.Activo = true;

                db.Roles.Add(rol);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Rol guardado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    // Manejo de errores de validación detallado
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var error in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado: " + ex.Message;
                }
            }

            TempData["ErrorMessage"] = "Error al guardar el rol. Intente de nuevo.";
            return View("_AgregarRol", rol); // Devuelve la vista con el modelo para mostrar errores
        }

        // GET: UsuarioWeb/EditRole/5
        public ActionResult EditRole(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var rol = db.Roles.Find(id);
            if (rol == null)
                return HttpNotFound();

            ViewBag.EsEdicion = true;
            return View("_AgregarRol", rol); // Vista compartida para crear/editar roles
        }

        // POST: UsuarioWeb/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(Rol rol)
        {
            if (ModelState.IsValid)
            {
                var rolExistente = db.Roles.Find(rol.Id);
                if (rolExistente == null)
                    return HttpNotFound();

                // Actualiza los campos necesarios
                rolExistente.Codigo = rol.Codigo;
                rolExistente.Nombre = rol.Nombre;
                rolExistente.Descripcion = rol.Descripcion;
                rolExistente.FechaModificacion = DateTime.Now;
                rolExistente.Activo = rol.Activo;

                db.Entry(rolExistente).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Rol actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al actualizar el rol: " + ex.Message;
                }
            }

            TempData["ErrorMessage"] = "Error al actualizar el rol. Intente de nuevo.";
            return View("_AgregarRol", rol); // Vista con los errores mostrados
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
        public ActionResult CreatePermiso()
        {
            ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre"); // Cargar los roles disponibles
            var permiso = new Permiso();
            return View("_AgregarPermiso", permiso); // Usar una vista compartida para crear/editar permisos
        }

        // POST: UsuarioWeb/CreatePermiso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePermiso(Permiso permiso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    permiso.FechaModificacion = DateTime.Now;
                    permiso.Activo = true;

                    // Validar la existencia del rol seleccionado
                    if (!db.Roles.Any(r => r.Id == permiso.RolId))
                    {
                        ModelState.AddModelError("RolId", "El rol seleccionado no es válido.");
                        ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre", permiso.RolId);
                        return View("_AgregarPermiso", permiso);
                    }

                    db.Permisos.Add(permiso);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Permiso guardado correctamente.";
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Manejo de errores de validación
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado: " + ex.Message;
            }

            ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre", permiso.RolId);
            TempData["ErrorMessage"] = "Error al guardar el permiso. Intente de nuevo.";
            return View("_AgregarPermiso", permiso);
        }

        // GET: UsuarioWeb/EditPermiso/5
        public ActionResult EditPermiso(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var permiso = db.Permisos.Find(id);
            if (permiso == null)
                return HttpNotFound();

            ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre", permiso.RolId);
            return View("_AgregarPermiso", permiso); // Usar la vista compartida
        }

        // POST: UsuarioWeb/EditPermiso/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermiso(Permiso permiso)
        {
            if (ModelState.IsValid)
            {
                var permisoExistente = db.Permisos.Find(permiso.Id);
                if (permisoExistente == null)
                    return HttpNotFound();

                // Actualizar campos específicos
                permisoExistente.Nombre = permiso.Nombre;
                permisoExistente.Descripcion = permiso.Descripcion;
                permisoExistente.FechaModificacion = DateTime.Now;
                permisoExistente.Activo = permiso.Activo;
                permisoExistente.RolId = permiso.RolId;

                db.Entry(permisoExistente).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Permiso actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al actualizar el permiso: " + ex.Message;
                }
            }

            ViewBag.RolId = new SelectList(db.Roles, "Id", "Nombre", permiso.RolId);
            TempData["ErrorMessage"] = "Error al actualizar el permiso. Intente de nuevo.";
            return View("_Create_EditPermiso", permiso);
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
