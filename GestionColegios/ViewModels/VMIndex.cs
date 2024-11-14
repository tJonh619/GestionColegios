using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMIndex
    {
        public List<Estudiante> Estudiantes { get; set; }
        public List<Tutor> Tutores { get; set; }
        public List<Maestro> Maestros { get; set; }
        public List<InventarioAlimento> Alimentos{get; set; }

    }
}