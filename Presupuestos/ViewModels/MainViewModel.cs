using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using PagedList;

namespace Presupuestos.ViewModels
{
    public class MainViewModel
    {
        public int? Page { get; set; }

        public Dictionary<string, string> MessageType { get; set; }

        public string SearchBudget { get; set; }

        public string SearchExecutive { get; set; }

        public string SortBy { get; set; }

        public ushort documentNumber { get; set; }

        public Dictionary<string, string> Sorts { get; set; }

        public ProjectionViewModel Projection { get; set; }

        public List<ProjectionViewModel> Projections { get; set; }

        public IPagedList PagingMetaData { get; set; }

        public SortedSet<CostsViewModel> orderedCosts { get; set; }

        public MainViewModel()
        {
            this.Projection = new ProjectionViewModel();
            this.PagingMetaData = new List<ProjectionViewModel>().ToPagedList(1, 1);
        }
    }
}