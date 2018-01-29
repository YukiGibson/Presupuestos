using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Presupuestos.Models;
using Presupuestos.DAL;

namespace Presupuestos.Repositories
{
    public class PipelineRepository : IRepository<DetailPipeline>, IDisposable
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

        public void Create(DetailPipeline entity)
        {
            _projectionContext.Entry(entity).State = EntityState.Added;
        }

        public void Delete(DetailPipeline entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DetailPipeline> Read()
        {
            return _projectionContext.DetailPipeline;
        }

        public void Save()
        {
            _projectionContext.SaveChanges();
        }

        public void Update(DetailPipeline entity, string presupuesto)
        {
            var newEntity = _projectionContext.DetailPipeline.Find(entity.ID);
            newEntity = entity;
        }

        public void Dispose()
        {
            ((IDisposable)_projectionContext).Dispose();
        }
    }
}