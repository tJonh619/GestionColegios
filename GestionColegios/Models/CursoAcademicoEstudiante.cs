//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestionColegios.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CursoAcademicoEstudiante
    {
        public int Id { get; set; }
        public decimal Promedio { get; set; }
        public bool Aprobado { get; set; }
        public string Estado { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public int CursoAcademicoId { get; set; }
        public int EstudianteId { get; set; }
    
        public virtual CursoAcademico CursoAcademico { get; set; }
        public virtual Estudiante Estudiante { get; set; }
    }
}
