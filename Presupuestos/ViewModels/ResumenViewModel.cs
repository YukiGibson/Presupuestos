using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presupuestos.ViewModels
{
    public class ResumenViewModel
    {
        public ResumenViewModel()
        {
            this.annoDropDrown = new Dictionary<string, string>();
            this.mesDropDown = new Dictionary<string, string>();
            this.requestStatus = new Dictionary<string, string>();
            this.familiaDropDown = new Dictionary<string, string>();
        }

        public List<ClienteKilosGrafico> graficoKilos { get; set; }

        public List<ClienteConsumosMesesGrafico> graficoConsumos { get; set; }

        public Dictionary<string, string> requestStatus { get; set; }

        public Dictionary<string, string> mesDropDown { get; set; }

        public Dictionary<string, string> annoDropDrown { get; set; }

        public Dictionary<string, string> familiaDropDown { get; set; }

        [Display(Name="Mes")]
        public int mes { get; set; }
        [Display(Name="Año")]
        public int anno { get; set; }
        [Display(Name="Familia")]
        public string familia { get; set; }
    }

    public class ClienteKilosGrafico
    {
        public string cliente { get; set; }
        public decimal? kilogramos { get; set; }
    }

    public class ClienteConsumosMesesGrafico
    {
        public string cliente { get; set; }
        public int? mes { get; set; }
        public int? anno { get; set; }
        public decimal? consumos { get; set; }
        public string familia { get; set; }
    }
}