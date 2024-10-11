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
        public IEnumerable<ControlMerienda> ControlMeriendas { get; set; }
        public IEnumerable<CursoAcademico> CursosAcademicos { get; set; } // Asegúrate de que esta clase exista
        public IEnumerable<Estudiante> Estudiantes { get; set; } // Asegúrate de que esta clase exista

        // Constructor (opcional, pero recomendable)
        public VMControlMerienda()
        {
            ControlMeriendas = new List<ControlMerienda>();
            CursosAcademicos = new List<CursoAcademico>();
            Estudiantes = new List<Estudiante>();
        }

    }
}