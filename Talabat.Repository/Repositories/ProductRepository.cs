using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(StoreContext context) : base(context)
        {
        }
    }
}
