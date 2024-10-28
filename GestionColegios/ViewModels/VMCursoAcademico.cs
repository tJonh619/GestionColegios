using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionColegios.Models;

namespace GestionColegios.ViewModels
{
    public class VMCursoAcademico
    {
        public class VMCursosAcademicos
        {
            public List<CursoAcademico> Cursos { get; set; }
            public CursoAcademico CursoAcademico { get; set; }

            public List<Maestro> Maestros { get; set; }

            public Seccion Seccion { get; set; }
            public List<Seccion> Secciones { get; set; }

            public AñoAcademico AñoAcademico { get; set; }

            public List<AñoAcademico> AñosAcademicos { get; set; }

            public Año Año { get; set; }

            public List<Año> Años { get; set; }

            public VMCursosAcademicos()
            {
                Cursos = new List<CursoAcademico>();
                CursoAcademico = new CursoAcademico();
                Maestros = new List<Maestro>();
                Secciones = new List<Seccion>();
                AñosAcademicos = new List<AñoAcademico>();
                Años = new List<Año>();
            }

     
        }
    }
}