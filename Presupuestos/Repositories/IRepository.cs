using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presupuestos.Repositories
{
    interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Read();
        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity, string presupuesto);
        void Save();

    }
}
