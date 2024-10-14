using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMAñosPeriodosSemestres
    {
        public List<Periodo> Periodos { get; set; }
        public Periodo Periodo { get; set; }
        public List<Semestre> Semestres { get; set; }
        public Semestre Semestre { get; set; }
        public List<Año> Años { get; set; }
        public Año Año { get; set; }
    }
}