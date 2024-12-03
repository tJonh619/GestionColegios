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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new VMUsuario
            {
                Usuarios = db.Usuarios.Include(u => u.Rol).Where(u => u.Activo==true).ToList(),
                Roles = db.Roles.Where(r => r.Activo == true).ToList(),
                Permisos = db.Permisos.Where(p => p.Activo == true).ToList(),
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                // Creación del usuario
                model.Usuario.CodigoUsuario = model.Usuario.NombreUsuario.Substring(0, 3).ToUpper() +
                                               new Random().Next(100, 1000).ToString();


                model.Usuario.Activo = true;
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            ViewBag.Edit = "usuario";
            return View("_AgregarUsuario", viewModel);
        }

        // POST: UsuarioWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMUsuario model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            ViewBag.Edit = "rol";
            return View("_AgregarRol", viewModel);
        }

        // POST: UsuarioWeb/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(VMUsuario model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            ViewBag.Edit = "permiso";
            return View("_AgregarPermiso", viewModel); // Usar la vista compartida
        }

        // POST: UsuarioWeb/EditPermiso/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermiso(VMUsuario model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
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
