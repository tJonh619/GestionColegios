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
            var viewModel = new VMUsuario
            {
                Rol = new Rol(),
                Roles = db.Roles.ToList()
            };
            ViewBag.EsEdicion = false;
            return View("_AgregarRol", viewModel); // Vista reutilizable para crear/editar
        }

        // POST: UsuarioWeb/CreateRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                model.Rol.Codigo =  $"{model.Rol.Nombre.Substring(0, 4).ToUpper()}{new Random().Next(100, 999)}";
                model.Rol.FechaModificacion = DateTime.Now;
                model.Rol.Activo = true;

                db.Roles.Add(model.Rol);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Rol guardado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
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
            model.Roles = db.Roles.ToList(); // Recarga la lista de roles en caso de error
            return View("_AgregarRol", model);
        }

        // GET: UsuarioWeb/EditRole/5
        public ActionResult EditRole(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var rol = db.Roles.Find(id);
            if (rol == null)
                return HttpNotFound();

            var viewModel = new VMUsuario
            {
                Rol = rol,
                Roles = db.Roles.ToList()
            };
            ViewBag.EsEdicion = true;
            return View("_AgregarRol", viewModel);
        }

        // POST: UsuarioWeb/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                var rolExistente = db.Roles.Find(model.Rol.Id);
                if (rolExistente == null)
                    return HttpNotFound();

 
                rolExistente.Nombre = model.Rol.Nombre;
                rolExistente.Descripcion = model.Rol.Descripcion;
                rolExistente.FechaModificacion = DateTime.Now;
                rolExistente.Activo = model.Rol.Activo;

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
            model.Roles = db.Roles.ToList(); // Recarga la lista de roles en caso de error
            return View("_AgregarRol", model);
        }

        // GET: UsuarioWeb/DeleteRole/5
        public ActionResult DeleteRole(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var rol = db.Roles.Find(id);
            if (rol == null)
                return HttpNotFound();

            var viewModel = new VMUsuario
            {
                Rol = rol
            };
            return View(viewModel);
        }

        // POST: UsuarioWeb/DeleteRole/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(int id)
        {
            var rol = db.Roles.Find(id);
            if (rol != null)
            {
                rol.Activo = false;
                rol.FechaModificacion = DateTime.Now;
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }



        // GET: UsuarioWeb/CreatePermiso
        public ActionResult CreatePermiso()
        {
            var viewModel = new VMUsuario
            {

                Permiso = new Permiso(),
                Permisos = db.Permisos.ToList(),
                Roles = db.Roles.ToList() // Cargar roles disponibles
            };
            ViewBag.EsEdicion = false;
            return View("_AgregarPermiso", viewModel); // Vista compartida para crear/editar permisos
        }

        // POST: UsuarioWeb/CreatePermiso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePermiso(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                model.Permiso.FechaModificacion = DateTime.Now;
                model.Permiso.Activo = true;
                db.Permisos.Add(model.Permiso);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Permiso guardado correctamente.";
                return RedirectToAction("Index");
            }

            model.Permisos = db.Permisos.ToList(); // Recargar permisos si falla la validación
            TempData["ErrorMessage"] = "Error al guardar el permiso. Intente de nuevo.";
            return View("_AgregarPermiso", model);
        }

        // GET: UsuarioWeb/EditPermiso/5
        public ActionResult EditPermiso(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var permiso = db.Permisos.Find(id);
            if (permiso == null)
                return HttpNotFound();

            var viewModel = new VMUsuario
            {
                Permiso = permiso,
                Roles = db.Roles.ToList() // Cargar roles disponibles
            };
            ViewBag.EsEdicion = true;
            return View("_AgregarPermiso", viewModel); // Usar la vista compartida
        }

        // POST: UsuarioWeb/EditPermiso/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermiso(VMUsuario model)
        {
            if (ModelState.IsValid)
            {
                var permisoExistente = db.Permisos.Find(model.Permiso.Id);
                if (permisoExistente == null)
                    return HttpNotFound();

                // Actualizar campos específicos del permiso
                permisoExistente.Nombre = model.Permiso.Nombre;
                permisoExistente.Descripcion = model.Permiso.Descripcion;
                permisoExistente.FechaModificacion = DateTime.Now;
                permisoExistente.Activo = model.Permiso.Activo;

                

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

            model.Roles = db.Roles.ToList(); // Recargar roles si falla la validación
            TempData["ErrorMessage"] = "Error al actualizar el permiso. Intente de nuevo.";
            return View("_AgregarPermiso", model);
        }

        // GET: UsuarioWeb/DeletePermiso/5
        public ActionResult DeletePermiso(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var permiso = db.Permisos.Find(id);
            if (permiso == null)
                return HttpNotFound();

            var viewModel = new VMUsuario
            {
                Permiso = permiso
            };
            return View(viewModel); // Vista adecuada para confirmar eliminación
        }

        // POST: UsuarioWeb/DeletePermiso/5
        [HttpPost, ActionName("DeletePermiso")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePermisoConfirmed(int id)
        {
            var permiso = db.Permisos.Find(id);
            if (permiso != null)
            {
                permiso.Activo = false; // Marcar como inactivo
                permiso.FechaModificacion = DateTime.Now;
                db.Entry(permiso).State = EntityState.Modified;
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
