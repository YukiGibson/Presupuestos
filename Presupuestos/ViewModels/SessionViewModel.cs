using Presupuestos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presupuestos.ViewModels
{
    public class SessionViewModel
    {
        public SessionViewModel()
        {
            this.MessageType = new Dictionary<string, string>();
            this.Projections = new List<DetailPipeline>();
        }
        public ushort DocumentNumber { get; set; }
        public ProjectionViewModel Projection { get; set; }
        public virtual List<DetailPipeline> Projections { get; set; }
        public Dictionary<string, string> MessageType { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public DateTime? LastUpdate { get; set; }

        //[Display(Name = "Fecha de Fin")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yy}", ApplyFormatInEditMode = true)]
        //public DateTime EndDate { get; set; }
    }
}