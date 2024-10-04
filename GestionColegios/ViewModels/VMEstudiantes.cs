using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMEstudiantes
    {
        public List<Estudiante> Estudiantes { get; set; }
        public Estudiante Estudiante { get; set; }

        public List<Tutor> Tutores { get; set; }
        public Tutor Tutor { get; set; }
    }
}