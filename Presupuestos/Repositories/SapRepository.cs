using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Presupuestos.Models;
using Presupuestos.DAL;

namespace Presupuestos.Repositories
{
    public class SapRepository : IRepository<OITM>, IDisposable
    {
         private SapDataContext _sapDataContext;

        public SapRepository()
        {
            _sapDataContext = new SapDataContext();
        }

        public SapDataContext GetContext()
        {
            return _sapDataContext;
        }

        public void Create(OITM entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(OITM entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OITM> Read()
        {

            return _sapDataContext.OITM;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(OITM entity, string presupuesto)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            ((IDisposable)_sapDataContext).Dispose();
        }
    }
}