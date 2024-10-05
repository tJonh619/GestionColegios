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
    
    public partial class CursoAcademico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CursoAcademico()
        {
            this.EficienciaFisica = new HashSet<EficienciaFisica>();
            this.CursoAcademicoEstudiante = new HashSet<CursoAcademicoEstudiante>();
            this.ControlMerienda = new HashSet<ControlMerienda>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public int MaestroId { get; set; }
        public int SeccionId { get; set; }
        public int AñoAcademicoId { get; set; }
        public int AñoId { get; set; }
    
        public virtual Maestro Maestro { get; set; }
        public virtual Seccion Seccion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EficienciaFisica> EficienciaFisica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CursoAcademicoEstudiante> CursoAcademicoEstudiante { get; set; }
        public virtual AñoAcademico AñoAcademico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ControlMerienda> ControlMerienda { get; set; }
        public virtual Año Año { get; set; }
    }
}