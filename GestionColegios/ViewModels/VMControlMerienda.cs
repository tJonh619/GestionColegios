using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionColegios.Models;
using GestionColegios.ViewModels;

namespace GestionColegios.ViewModels
{
    public class VMControlMerienda
    {
        public List<Estudiante> Estudiantes { get; set; }
        public Estudiante Estudiante { get; set; }
        public List<CursoAcademico> CursosAcademicos { get; set; }
        public CursoAcademico cursoAcademico { get; set; }
        public List<ControlMerienda> ControlesMeriendas { get; set; }
        public ControlMerienda ControlMerienda { get; set; }

        public InventarioAlimento inventarioAlimento { get; set; }
        public List<InventarioAlimento> InventarioAlimentos { get; set; }

    }
}