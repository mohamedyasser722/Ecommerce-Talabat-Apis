using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _Repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
            _Repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(type))
                _Repositories.Add(type, new GenericRepository<TEntity>(_context));

            return _Repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();
    }
}
