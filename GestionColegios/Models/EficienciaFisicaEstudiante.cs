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
    
    public partial class EficienciaFisicaEstudiante
    {
        public int Id { get; set; }
        public string NombresApellidos { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public Nullable<decimal> Peso_lb { get; set; }
        public Nullable<int> Talla_cm { get; set; }
        public Nullable<decimal> Velocidad_seg { get; set; }
        public Nullable<decimal> Resistencia_min_seg { get; set; }
        public Nullable<int> Salto_cm { get; set; }
        public Nullable<int> Pechadas_repet { get; set; }
        public Nullable<int> Abdominales_repet { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public int EficienciaFisicaId { get; set; }
        public int EstudianteId { get; set; }
    
        public virtual EficienciaFisica EficienciaFisica { get; set; }
        public virtual Estudiante Estudiante { get; set; }
    }
}