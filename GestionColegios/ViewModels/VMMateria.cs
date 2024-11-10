using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMMateria
    {
        public List<Materia> Materias { get; set; }
        public Materia Materia { get; set; }
        public List<AñoAcademico> AñosAcademicos { get; set; }
        public AñoAcademico AñoAcademico { get; set; }
    }
}