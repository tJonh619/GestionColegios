﻿using System;
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
    public class CursoAcademicoWebController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: CursoAcademicoWeb
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var cursosAcademicos = db.CursosAcademicos
                .Include(c => c.Maestro)
                .Include(c => c.Seccion)
                .Include(c => c.AñoAcademico)
                .Include(c => c.Año)
                .Where(c => c.Activo == true)
                .ToList();

            var viewModel = new VMCursoAcademico
            {
                CursoAcademicos = cursosAcademicos,
                CursoAcademico = new CursoAcademico(),
                Maestros = db.Maestros.Where(u => u.Activo == true).ToList(),
                Maestro = new Maestro(),
                Secciones = db.Secciones.Where(u => u.Activo == true).ToList(),
                Seccion = new Seccion(),
                AñosAcademicos = db.AñosAcademicos.Where(u => u.Activo == true).ToList(),
                AñoAcademico = new AñoAcademico(),
                Años = db.Años.ToList(),
                Año = new Año()
            };
            ViewBag.EsEdicion = false;
            return View(viewModel);
        }

      
        // GET: CursoAcademicoWeb/Create
        public ActionResult Create()
        {
            // Set ViewBag.EsEdicion to false if creating
            ViewBag.EsEdicion = false;

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = new CursoAcademico(),  // Asegúrate de inicializar CursoAcademico
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };
            ViewBag.EsEdicion = false;
            return View(viewModel);

        }

        // POST: CursoAcademicoWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCursoAcademico viewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var cursoAcademico = viewModel.CursoAcademico;
                cursoAcademico.FechaModificacion = DateTime.Now;
                cursoAcademico.Activo = true;
                db.CursosAcademicos.Add(cursoAcademico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("_AgregarCursos", viewModel);
        }

        // GET: CursoAcademicoWeb/Edit/5
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

            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            // Set ViewBag.EsEdicion to true if editing
            ViewBag.EsEdicion = true;

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.Where(u => u.Activo == true).ToList(),
                Secciones = db.Secciones.Where(u => u.Activo == true).ToList(),
                AñosAcademicos = db.AñosAcademicos.Where(u => u.Activo == true).ToList(),
                Años = db.Años.ToList()
            };

            return View("_AgregarCursos", viewModel);
        }

        // POST: CursoAcademicoWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCursoAcademico viewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                var cursoAcademico = viewModel.CursoAcademico;
                cursoAcademico.FechaModificacion = DateTime.Now;
                cursoAcademico.Activo = true;
                db.Entry(cursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Maestros = db.Maestros.ToList();
            viewModel.Secciones = db.Secciones.ToList();
            viewModel.AñosAcademicos = db.AñosAcademicos.ToList();
            viewModel.Años = db.Años.ToList();

            return View(viewModel);
        }

        // GET: CursoAcademicoWeb/Delete/5
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
            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMCursoAcademico
            {
                CursoAcademico = cursoAcademico,
                Maestros = db.Maestros.ToList(),
                Secciones = db.Secciones.ToList(),
                AñosAcademicos = db.AñosAcademicos.ToList(),
                Años = db.Años.ToList()
            };

            return View(viewModel);
        }

        // POST: CursoAcademicoWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            CursoAcademico cursoAcademico = db.CursosAcademicos.Find(id);
            if (cursoAcademico != null)
            {
                cursoAcademico.Activo = false;
                cursoAcademico.FechaModificacion = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Función para actualizar los campos cuando se edita un curso
        public ActionResult UpdateCursoAcademico(int id, CursoAcademico cursoAcademico)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id != cursoAcademico.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                db.Entry(cursoAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cursoAcademico);
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
