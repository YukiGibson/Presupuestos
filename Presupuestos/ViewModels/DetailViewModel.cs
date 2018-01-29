using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using System.ComponentModel.DataAnnotations;

namespace Presupuestos.ViewModels
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            estimadosDrop = new Dictionary<string, string>();
            monthDrop = new Dictionary<string, string>();
            yearDrop = new Dictionary<string, string>();
            executiveDrop = new Dictionary<string, string>();
            loadStatus = new Dictionary<string, string>();
        }

        public short session { get; set; }
        public string status { get; set; }
        public Dictionary<string, string> loadStatus { get; set; }
        public DetalleEjecutivo totalDetail { get; set; }

        //TODO change executiveList to 
        public List<DetalleEjecutivo> executiveList { get; set; }

        [Display(Name ="Estimado")]
        public string estimado { get; set; }
        public Dictionary<string, string> estimadosDrop { get; set; }

        [Display(Name = "Mes")]
        public string month { get; set; }
        public Dictionary<string, string> monthDrop { get; set; }

        [Display(Name = "Año")]
        public string year { get; set; }
        public Dictionary<string, string> yearDrop { get; set; }

        [Display(Name = "Ejecutivo")]
        public string executive { get; set; }
        public Dictionary<string, string> executiveDrop { get; set; }
    }
}