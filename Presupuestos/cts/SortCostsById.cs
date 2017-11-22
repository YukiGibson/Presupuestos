using System.Collections.Generic;
using Presupuestos.ViewModels;

namespace Presupuestos.cts
{
    public class SortCostsById : IComparer<CostsViewModel>
    {
        public int Compare(CostsViewModel firstMonth, CostsViewModel secondMonth)
        {
            if (firstMonth.id > secondMonth.id)
                return 1;
            if (firstMonth.id < secondMonth.id)
                return -1;
            else
                return 0;
        }
    }
}