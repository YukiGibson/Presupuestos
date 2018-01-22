using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;

namespace Presupuestos.Ventas
{
    public class Detail
    {
        public Detail()
        {
            sales = new List<DetailPipelineVentas>();
            totals = new DetailPipelineVentas();
        }
        public string client { get; set; }
        public int rowspan { get; set; }
        public List<DetailPipelineVentas> sales { get; set; }
        public DetailPipelineVentas totals { get; set; }
    }
}