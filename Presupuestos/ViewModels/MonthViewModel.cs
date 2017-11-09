using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.ViewModels
{
    public class MonthViewModel
    {
        public int month { get; set; } // Month of each projection
        public int year { get; set; } // Year of each projection
        [Display(Name = "Proyección ")]
        public string value { get; set; } // Projection month value
    }
}