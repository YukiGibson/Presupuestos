using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using PagedList;
using Presupuestos.Services;
using Presupuestos.Models;

namespace Presupuestos.ViewModels
{
    public class MainViewModel
    {
        public Dictionary<string, string> MessageType { get; set; }

        public ushort documentNumber { get; set; }

        public ProjectionViewModel Projection { get; set; }

        public List<Abastecimiento> Projections { get; set; }

        //Search dropdowns
        public string year { get; set; }
        public Dictionary<string, string> yearDropDown { get; set; }
        public byte month { get; set; }
        public Dictionary<string, string> monthDropDown { get; set; }
        public string executive { get; set; }
        public Dictionary<string, string> executiveDropDown { get; set; }

        public MainViewModel()
        {
            this.Projection = new ProjectionViewModel();
            this.yearDropDown = new Dictionary<string, string>();
            this.monthDropDown = new Dictionary<string, string>();
            this.executiveDropDown = new Dictionary<string, string>();
        }
    }
}