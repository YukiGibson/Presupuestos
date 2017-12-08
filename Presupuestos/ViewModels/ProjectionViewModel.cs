using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Presupuestos.ViewModels
{
    public class ProjectionViewModel
    {
        public int idLinea { get; set; }

        public long ID { get; set; }

        public bool Checked { get; set; }

        [Display(Name = "Ejecutivo")]
        [StringLength(40)]
        public string Ejecutivo { get; set; } // Vendedor

        [StringLength(60)]
        public string Cliente { get; set; } // Cliente

        [Display(Name = "Familia")]
        [StringLength(50)]
        public string Familia { get; set; } // Familia

        [Display(Name = "Producto")]
        [StringLength(100)]
        public string Producto { get; set; } // Producto

        [Display(Name = "Presupuesto")]
        [StringLength(12)]
        public string Presupuesto { get; set; } // Master Metrics

        [Display(Name = "ItemCodeSustrato")]
        [StringLength(30)]
        public string ItemCodeSustrato { get; set; }

        [Display(Name = "Sustrato")]
        [StringLength(20)]
        public string Sustrato { get; set; } // Sustrato

        public double? Gramaje { get; set; } // Gramaje

        [Display(Name = "Ancho Bobina")]
        public decimal? Ancho_Bobina { get; set; } // Ancho bobina

        [Display(Name = "Ancho Pliego")]
        public decimal? Ancho_Pliego { get; set; } // Ancho Pliego

        [Display(Name = "Largo Pliego")]
        public decimal? Largo_Pliego { get; set; } // Largo Pliego

        [Display(Name = "Paginas")]
        public int? Paginas { get; set; } // paginas

        [Display(Name = "Montaje")]
        public int? Montaje { get; set; } // Montaje

        public int? Pliegos { get; set; }  // Pliegos

        public virtual List<MonthViewModel> Month { get; set; }

        public string CardCode { get; set; }
    }
}