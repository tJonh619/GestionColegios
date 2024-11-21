using GestionColegios.Models;
using GestionColegios.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace GestionColegios.Controllers
{
    public class HomeController : Controller
    {
        private BDColegioContainer db = new BDColegioContainer();


        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult Home()
        {
            var viewModel = new VMIndex { Maestros = db.Maestros.ToList(), Estudiantes = db.Estudiantes.ToList(), Tutores = db.Tutores.ToList(), Alimentos = db.InventariosAlimentos.ToList()};
            return PartialView(viewModel);
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}