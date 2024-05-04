using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Interfaces
{
    public interface ISpecification<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; set; }
        Expression<Func<T, object>> OrderBy { get; set; }
        Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
