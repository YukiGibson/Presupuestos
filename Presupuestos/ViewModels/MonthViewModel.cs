using System;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.ViewModels
{
    public class MonthViewModel
    {
        public double id { get; set; }
        public int month { get; set; } // Month of each projection
        public int year { get; set; } // Year of each projection

        [DisplayFormat(DataFormatString = "{0:0,0.##}")]
        public double value { get; set; } // Projection month value
        public string monthName { get; set; } // The name of the month in Spanish
        public decimal? kg { get; set; }
    }
}