using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using Presupuestos.DAL;
using System.Data.Entity;

namespace Presupuestos.Repositories
{
    public class ProjectionsRepository : IRepository<DetailPipelineProyeccione>, IDisposable
    {
        private ProjectionContext _projectionContext;

        public ProjectionsRepository()
        {
            _projectionContext = new ProjectionContext();
        }

        public bool ifExists(DetailPipelineProyeccione entity)
        {
            return _projectionContext.DetailPipelineProyecciones
                .Where(o => o.Año == entity.Año && o.Mes == entity.Mes && 
                o.Vendedor.Contains(entity.Vendedor)).Any();
        }

        public void Create(DetailPipelineProyeccione entity)
        {
            _projectionContext.Entry(entity).State = EntityState.Added;
        }

        public void Delete(DetailPipelineProyeccione entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DetailPipelineProyeccione> Read()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _projectionContext.SaveChanges();
        }

        public void Update(DetailPipelineProyeccione entity, string proyeccion)
        {
            DetailPipelineProyeccione forUpdate = _projectionContext.DetailPipelineProyecciones.Where(o => o.Año == entity.Año && o.Mes == entity.Mes &&
                o.Vendedor.Contains(entity.Vendedor)).FirstOrDefault();
            forUpdate.Proyeccion = entity.Proyeccion;
            
        }

        public void Dispose()
        {
            ((IDisposable)_projectionContext).Dispose();
        }
    }
}