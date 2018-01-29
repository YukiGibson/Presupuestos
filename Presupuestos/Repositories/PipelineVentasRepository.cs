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

        public bool IfExists(DetailPipelineVentas entity, short session)
        {
            bool exists = _projectionContext.DetailPipelineVentas.Where(p => p.OP.Contains(entity.OP)
            && p.Presupuesto.Contains(entity.Presupuesto) && p.TipoProducto.Contains(entity.TipoProducto)
            && p.Mes == entity.Mes && p.Año == entity.Año && p.Sesion == session).Any();
            return exists;
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

        public void UpdateColor(DetailPipelineVentas entity, string color)
        {
            DetailPipelineVentas updateVentas = _projectionContext.DetailPipelineVentas.Find(entity.ID);
            updateVentas.Color = color;
        }

        public void Update(DetailPipelineVentas entity, string session)
        {
            short _session = short.Parse(session);
            DetailPipelineVentas detail = _projectionContext.DetailPipelineVentas.Where(p => p.OP.Contains(entity.OP)
            && p.Presupuesto.Contains(entity.Presupuesto) && p.TipoProducto.Contains(entity.TipoProducto)
            && p.Mes == entity.Mes && p.Año == entity.Año && p.Sesion == _session).FirstOrDefault();

            detail.CantidadTotal = entity.CantidadTotal;
            detail.Cantidad = entity.Cantidad;
            detail.PorFacturar = entity.PorFacturar;
            detail.Costo = entity.Costo;
            detail.Rentabilidad = entity.Rentabilidad;
            detail.ProbabilidadVenta = entity.ProbabilidadVenta;
        }

        public void Dispose()
        {
            ((IDisposable)_projectionContext).Dispose();
        }
    }
}