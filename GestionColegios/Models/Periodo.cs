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
    
    public partial class Periodo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Periodo()
        {
            this.Matricula = new HashSet<Matricula>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Numero { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public bool Activo { get; set; }
        public int AñoId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }
        public virtual Año Año { get; set; }
    }
}
