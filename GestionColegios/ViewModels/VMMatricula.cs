using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionColegios.Models;

namespace GestionColegios.ViewModels
{
    public class VMMatricula
    {

        // Lista de matrículas
        public List<Matricula> Matriculas { get; set; }

        // Una matrícula específica para operaciones como editar
        public Matricula Matricula { get; set; }

        // Listas para los dropdowns
        public List<Estudiante> Estudiantes { get; set; }
        public List<Tutor> Tutores { get; set; }
        public List<Periodo> Periodos { get; set; }
        public List<AñoAcademico> AñosAcademicos { get; set; }
    }
}