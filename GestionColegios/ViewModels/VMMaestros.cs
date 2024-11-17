using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMMaestros
    {
        public List<Maestro> Maestros { get; set; }
        public Maestro Maestro { get; set; }

        public List<Usuario> Usuarios { get; set; }

        public Usuario Usuario { get; set; }
    }
}