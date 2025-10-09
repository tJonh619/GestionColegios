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
    
    public partial class ControlEntrada
    {
        public int Id { get; set; }
        public string FechaAbastecimiento { get; set; }
        public string NombreReceptor { get; set; }
        public string EAceite { get; set; }
        public string EArroz { get; set; }
        public string ECereal { get; set; }
        public string EFrijoles { get; set; }
        public string EMaiz { get; set; }
        public string Activo { get; set; }
        public int AceiteId { get; set; }
        public int ArrozId { get; set; }
        public int CerealId { get; set; }
        public int FrijolesId { get; set; }
        public int MaizId { get; set; }
    
        public virtual InventarioAlimento RAceite { get; set; }
        public virtual InventarioAlimento RArroz { get; set; }
        public virtual InventarioAlimento RCereal { get; set; }
        public virtual InventarioAlimento RFrijoles { get; set; }
        public virtual InventarioAlimento RMaiz { get; set; }
    }
}
