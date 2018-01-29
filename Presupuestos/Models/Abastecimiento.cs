using Presupuestos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presupuestos.Models
{
    public class Abastecimiento
    {
        public Abastecimiento()
        {
            sales = new List<ProjectionViewModel>();
            totals = new ProjectionViewModel();
        }
        public string client { get; set; }
        public int rowspan { get; set; }
        public List<ProjectionViewModel> sales { get; set; }
        public ProjectionViewModel totals { get; set; }
    }
}