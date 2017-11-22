using System;

namespace Presupuestos.ViewModels
{
    public class MonthViewModel
    {
        public double id { get; set; }
        public int month { get; set; } // Month of each projection
        public int year { get; set; } // Year of each projection
        public string value { get; set; } // Projection month value
        public string monthName { get; set; } // The name of the month in Spanish
        public DateTime date { get; set; } // Saves a Type Of DateTime that holds each month date
    }
}