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
    public class ControlMeriendasController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();

        // GET: ControlMeriendas
        public ActionResult Index()
        {
            var controlesMeriendas = db.ControlesMeriendas
                .Include(c => c.CursoAcademico)
                .Include(c => c.RAceite)
                .Include(c => c.RArroz)
                .Include(c => c.RCereal)
                .Include(c => c.RFrijoles)
                .Include(c => c.RMaiz)
                .Include(c => c.Estudiante);

            var viewModel = new VMControlMerienda
            {
                ControlesMeriendas = controlesMeriendas.ToList(),
                ControlMerienda = new ControlMerienda(),
                CursosAcademicos = db.CursosAcademicos.ToList(),
                cursoAcademico = new CursoAcademico(),
                Estudiantes = db.Estudiantes.ToList(),
                InventarioAlimentos = db.InventariosAlimentos.ToList(),
                Estudiante = new Estudiante()
            };

            return View(viewModel);
        }

        // GET: ControlMeriendas/Create
        public ActionResult Create()
        {
            var viewModel = new VMControlMerienda
            {
                CursosAcademicos = db.CursosAcademicos.ToList(),
                Estudiantes = db.Estudiantes.ToList(),
                InventarioAlimentos = db.InventariosAlimentos.ToList()
            };

            return View(viewModel);
        }

        // POST: ControlMeriendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMControlMerienda viewModel, string[] Codigo, DateTime[] FechaEntrega, int[] AsistenciaEsperadaMujeres, int[] AsistenciaEsperadaTotal, int[] AsistenciaRealMujeres, int[] AsistenciaRealTotal, decimal[] SAceite, decimal[] SArroz, decimal[] SCereal, decimal[] SFrijoles, decimal[] SMaiz, string[] FirmaDocente, string[] CedulaTutor, string[] FirmaTutor, int[] CursoAcademicoId, int[] EstudianteId)
        {
            if (ModelState.IsValid)
            {
                if (Codigo != null && Codigo.Length > 0)
                {
                    var aceite = db.InventariosAlimentos.SingleOrDefault(a => a.NombreAlimento == "Aceite");
                    var arroz = db.InventariosAlimentos.SingleOrDefault(a => a.NombreAlimento == "Arroz");
                    var cereal = db.InventariosAlimentos.SingleOrDefault(a => a.NombreAlimento == "Cereal");
                    var frijoles = db.InventariosAlimentos.SingleOrDefault(a => a.NombreAlimento == "Frijoles");
                    var maiz = db.InventariosAlimentos.SingleOrDefault(a => a.NombreAlimento == "Maíz");

                    for (int i = 0; i < Codigo.Length; i++)
                    {
                        if (CursoAcademicoId[i] != 0 && EstudianteId[i] != 0) // Validar que no sean nulos
                        {
                            var controlMerienda = new ControlMerienda
                            {
                                Codigo = Codigo[i],
                                FechaEntrega = FechaEntrega[i],
                                AsistenciaEsperadaMujeres = AsistenciaEsperadaMujeres[i],
                                AsistenciaEsperadaTotal = AsistenciaEsperadaTotal[i],
                                AsistenciaRealMujeres = AsistenciaRealMujeres[i],
                                AsistenciaRealTotal = AsistenciaRealTotal[i],
                                SAceite = SAceite[i],
                                SArroz = SArroz[i],
                                SCereal = SCereal[i],
                                SFrijoles = SFrijoles[i],
                                SMaiz = SMaiz[i],
                                FirmaDocente = FirmaDocente[i],
                                CedulaTutor = CedulaTutor[i],
                                FirmaTutor = FirmaTutor[i],
                                CursoAcademicoId = CursoAcademicoId[i],
                                EstudianteId = EstudianteId[i],
                                AceiteId = aceite.Id,
                                ArrozId = arroz.Id,
                                CerealId = cereal.Id,
                                FrijolesId = frijoles.Id,
                                MaizId = maiz.Id,
                                FechaModificacion = DateTime.Now,
                                Activo = true
                            };

                            // Actualiza el inventario
                            if (aceite != null) aceite.Stock -= SAceite[i];
                            if (arroz != null) arroz.Stock -= SArroz[i];
                            if (cereal != null) cereal.Stock -= SCereal[i];
                            if (frijoles != null) frijoles.Stock -= SFrijoles[i];
                            if (maiz != null) maiz.Stock -= SMaiz[i];

                            db.ControlesMeriendas.Add(controlMerienda);
                        }
                    }

                    // Guarda los cambios en la base de datos
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "No hay registros para guardar.");
                }
            }

            viewModel.CursosAcademicos = db.CursosAcademicos.ToList();
            viewModel.Estudiantes = db.Estudiantes.ToList();
            return View(viewModel);
        }

        // GET: ControlMeriendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlMerienda controlMerienda = db.ControlesMeriendas.Find(id);
            if (controlMerienda == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VMControlMerienda
            {
                ControlMerienda = controlMerienda,
                CursosAcademicos = db.CursosAcademicos.ToList(),
                Estudiantes = db.Estudiantes.ToList()
            };

            return View(viewModel);
        }

        // POST: ControlMeriendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VMControlMerienda viewModel, string[] Codigo, DateTime[] FechaEntrega, int[] AsistenciaEsperadaMujeres, int[] AsistenciaEsperadaTotal, int[] AsistenciaRealMujeres, int[] AsistenciaRealTotal, decimal[] SAceite, decimal[] SArroz, decimal[] SCereal, decimal[] SFrijoles, decimal[] SMaiz, string[] FirmaDocente, string[] CedulaTutor, string[] FirmaTutor, int[] CursoAcademicoId, int[] EstudianteId)
        {
            if (ModelState.IsValid)
            {
                var controlMerienda = db.ControlesMeriendas.Find(id);
                if (controlMerienda != null)
                {
                    if (CursoAcademicoId[0] != 0 && EstudianteId[0] != 0) // Validar que no sean nulos
                    {
                        controlMerienda.Codigo = Codigo[0];
                        controlMerienda.FechaEntrega = FechaEntrega[0];
                        controlMerienda.AsistenciaEsperadaMujeres = AsistenciaEsperadaMujeres[0];
                        controlMerienda.AsistenciaEsperadaTotal = AsistenciaEsperadaTotal[0];
                        controlMerienda.AsistenciaRealMujeres = AsistenciaRealMujeres[0];
                        controlMerienda.AsistenciaRealTotal = AsistenciaRealTotal[0];
                        controlMerienda.SAceite = 3;
                        controlMerienda.SArroz = 1;
                        controlMerienda.SCereal = 4;
                        controlMerienda.SFrijoles = 2;
                        controlMerienda.SMaiz = 5;
                        controlMerienda.FirmaDocente = FirmaDocente[0];
                        controlMerienda.CedulaTutor = CedulaTutor[0];
                        controlMerienda.FirmaTutor = FirmaTutor[0];
                        controlMerienda.CursoAcademicoId = CursoAcademicoId[0];
                        controlMerienda.EstudianteId = EstudianteId[0];
                        controlMerienda.FechaModificacion = DateTime.Now;

                        db.Entry(controlMerienda).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Debe seleccionar un curso académico y un estudiante válidos.");
                    }
                }
            }

            viewModel.CursosAcademicos = db.CursosAcademicos.ToList();
            viewModel.Estudiantes = db.Estudiantes.ToList();
            return View(viewModel);
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
