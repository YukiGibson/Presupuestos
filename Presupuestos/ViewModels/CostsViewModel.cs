using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presupuestos.ViewModels
{
    public class CostsViewModel
    {
        public short id { get; set; }
        public string monthName { get; set; }

        public byte monthValue { get; set; }

        public short yearValue { get; set; }

        public CostsViewModel(short Id, string MonthName, byte MonthValue, short YearValue)
        {
            id = Id;
            monthName = MonthName;
            monthValue = MonthValue;
            yearValue = YearValue;
        }
        public CostsViewModel() { } // Default Empty ctor
    }
}