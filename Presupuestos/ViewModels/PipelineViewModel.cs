using System;
using System.Collections.Generic;
using Presupuestos.Models;
using System.Linq;
using System.Web;

namespace Presupuestos.ViewModels
{
    public class PipelineViewModel
    {
        public PipelineViewModel()
        {
            loadStatus = new Dictionary<string, string>();
            yearDropDown = new Dictionary<string, string>();
            monthDropDown = new Dictionary<string, string>();
            estimatedDropDown = new Dictionary<string, string>();
            executiveDropDown = new Dictionary<string, string>();
        }

        public int? id { get; set; }
        public short session { get; set; }
        public string status { get; set; }
        public Dictionary<string, string> loadStatus { get; set; }
        public List<DetailPipelineVentas> ventas { get; set; }
        public DetailPipelineVentas venta { get; set; }

        public List<Detail> detailList { get; set; }

        //Dropdown Detalles

        public string searchKeyword { get; set; }
        public Dictionary<string, string> searchDropDown { get; set; }

        //Analisis Meses
        public string year { get; set; }
        public Dictionary<string, string> yearDropDown { get; set; }
        public byte month { get; set; }
        public Dictionary<string, string> monthDropDown { get; set; }
        public string estimated { get; set; }
        public Dictionary<string, string> estimatedDropDown { get; set; }
        public string executive { get; set; }
        public Dictionary<string, string> executiveDropDown { get; set; }
    }
}