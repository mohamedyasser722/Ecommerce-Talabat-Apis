using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var Query = inputQuery;

            if (spec.Criteria != null)
                Query = Query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending is not null)
                Query = Query.OrderByDescending(spec.OrderByDescending);

            if (spec.IsPaginationEnabled)
                Query = Query.Skip(spec.Skip).Take(spec.Take);

            Query = spec.Includes.Aggregate(Query, (current, include) => current.Include(include));
            return Query;
        }

    }
}
