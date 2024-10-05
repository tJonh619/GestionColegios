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
    
    public partial class EficienciaFisica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EficienciaFisica()
        {
            this.EficienciaFisicaEstudiante = new HashSet<EficienciaFisicaEstudiante>();
        }
    
        public int Id { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public int CursoAcademicoId { get; set; }
        public int ConsolidadoEficienciaFisicaId { get; set; }
        public int SemestreId { get; set; }
        public int AñoId { get; set; }
    
        public virtual CursoAcademico CursoAcademico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EficienciaFisicaEstudiante> EficienciaFisicaEstudiante { get; set; }
        public virtual ConsolidadoEficienciaFisica ConsolidadoEficienciaFisica { get; set; }
        public virtual Semestre Semestre { get; set; }
        public virtual Año Año { get; set; }
    }
}