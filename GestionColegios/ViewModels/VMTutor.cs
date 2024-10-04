using GestionColegios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionColegios.ViewModels
{
    public class VMTutor
    {
        public List<Tutor> Tutores { get; set; }
        public Tutor Tutor { get; set; }
    }
}