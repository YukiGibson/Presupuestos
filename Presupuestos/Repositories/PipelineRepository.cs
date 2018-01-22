using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Presupuestos.Models;
using Presupuestos.DAL;

namespace Presupuestos.Repositories
{
    public class PipelineRepository : IRepository<DetailPipelinePruebas>, IDisposable
    {
        private ProjectionContext _projectionContext;

        public ProjectionContext GetProjectionContext
        {
            get { return _projectionContext; }
        }

        public PipelineRepository()
        {
            this._projectionContext = new ProjectionContext();
        }

        public void Create(DetailPipelinePruebas entity)
        {
            _projectionContext.Entry(entity).State = EntityState.Added;
        }

        public void Delete(DetailPipelinePruebas entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DetailPipelinePruebas> Read()
        {
            return _projectionContext.DetailPipelinePruebas;
        }

        public void Save()
        {
            _projectionContext.SaveChanges();
        }

        public void Update(DetailPipelinePruebas entity, string presupuesto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ((IDisposable)_projectionContext).Dispose();
        }
    }
}