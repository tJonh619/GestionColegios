using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionColegios.Models;

namespace GestionColegios.ViewModels
{
    public class VMCalificaciones
    {
        public List<Calificacion> Calificaciones { get; set; } = new List<Calificacion>(); // Asegúrate de que la lista esté inicializada
        public Calificacion Calificacion { get; set; }

        public List<Estudiante> Estudiantes { get; set; }
        public Estudiante Estudiante { get; set; }
        public List<Materia> Materias { get; set; } = new List<Materia>(); // Inicialización
        public Materia Materia { get; set; }

        public List<Parcial> Parciales { get; set; }
        public Parcial Parcial { get; set; }


        public List<AñoAcademico> AñoAcademicos { get; set; }

        public AñoAcademico AñoAcademico { get; set; }

    }
}