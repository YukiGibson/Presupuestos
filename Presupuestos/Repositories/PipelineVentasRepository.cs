using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using Presupuestos.DAL;
using System.Data.Entity;

namespace Presupuestos.Repositories
{
    public class PipelineVentasRepository : IRepository<DetailPipelineVentas>, IDisposable
    {
        private ProjectionContext _projectionContext;

        public PipelineVentasRepository()
        {
            _projectionContext = new ProjectionContext();
        }

        public ProjectionContext GetContext()
        {
            return _projectionContext;
        }

        public void Create(DetailPipelineVentas entity)
        {
            _projectionContext.Entry(entity).State = EntityState.Added;
        }

        public void Delete(DetailPipelineVentas entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DetailPipelineVentas> Read()
        {
            int lastSession = _projectionContext.DetailPipelineVentas.Select(p => p.Sesion).Max();
            IQueryable<DetailPipelineVentas> existingList = _projectionContext.DetailPipelineVentas
            .Where(p => p.Sesion == lastSession);
            return existingList;
        }

        public void Save()
        {
            _projectionContext.SaveChanges();
        }

        public void Update(DetailPipelineVentas entity, string presupuesto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ((IDisposable)_projectionContext).Dispose();
        }
    }
}