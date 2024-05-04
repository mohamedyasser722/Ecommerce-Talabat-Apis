using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly StoreContext context;
        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        #region StaticQuery
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>(), spec);
        }

        public async Task Add(T entity) => await context.Set<T>().AddAsync(entity);

        public void Update(T entity) => context.Set<T>().Update(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);
    }
}
