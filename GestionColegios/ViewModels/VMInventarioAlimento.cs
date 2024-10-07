using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestionColegios.Models;

namespace GestionColegios.ViewModels
{
    public class VMInventarioAlimento
    {
        public List<InventarioAlimento> InventarioAlimentos { get; set; }
        public InventarioAlimento InventarioAlimento { get; set; }

    }
}