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
    
    public partial class Calificacion
    {
        public int Id { get; set; }
        public decimal NCuantitativa { get; set; }
        public string NCualitativa { get; set; }
        public bool Activo { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public int ParcialId { get; set; }
    
        public virtual Estudiante Estudiante { get; set; }
        public virtual Materia Materia { get; set; }
        public virtual Parcial Parcial { get; set; }
    }
}
