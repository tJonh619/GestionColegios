using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMUsuario
    {

        public Usuario Usuario { get; set; }
        public Rol Rol { get; set; }
        public Permiso Permiso  {get; set; }
        public List<Rol> Roles { get; set; } // Para mostrar una lista de roles
        public List<Usuario> Usuarios { get; set; } // Para listar usuarios
        public List<Permiso> Permisos { get; set; } // Para mostrar y asignar permisos a roles
        public List<int> SelectedPermisos { get; set; } // Para almacenar los permisos seleccionados
    }

}
