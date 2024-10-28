using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMSeccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CapacidadEstudiantes { get; set; }

        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }

    }
}