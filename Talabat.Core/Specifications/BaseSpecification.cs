using System.Linq.Expressions;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.Core.Repsoitories
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecification(Expression<Func<T, bool>> Criteria)
        {
            this.Criteria = Criteria;
        }
        public BaseSpecification()
        {
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderBy)
        {
            this.OrderBy = OrderBy;
        }
        public void AddOrderByDescending(Expression<Func<T, object>> OrderByDescending)
        {
            this.OrderByDescending = OrderByDescending;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
