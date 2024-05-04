using Talabat.Core.Entities;
using Talabat.Core.Repsoitories;

namespace Talabat.Core.Specifications
{
    public class ProductSpecifications : BaseSpecification<Product>
    {

        public ProductSpecifications(int id) : base(P => P.Id == id) //GetById
        {
            Includes.Add(P => P.ProductCategory);
            Includes.Add(P => P.Brand);
        }
        public ProductSpecifications(ProductSpecParams productSpec) : base(P =>
             (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
             (!productSpec.BrandId.HasValue || P.BrandId == productSpec.BrandId) &&
             (!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId))//Get
        {
            Includes.Add(P => P.ProductCategory);
            Includes.Add(P => P.Brand);

            if (string.IsNullOrEmpty(productSpec.Sort) == false)
            {
                switch (productSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }

            }

            ApplyPagination(productSpec.PageSize * (productSpec.PagIndex - 1), productSpec.PageSize);
        }
    }
}
