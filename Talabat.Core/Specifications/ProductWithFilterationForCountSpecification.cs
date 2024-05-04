using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repsoitories;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification :BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams productSpec) : base(P =>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
            (!productSpec.BrandId.HasValue || P.BrandId == productSpec.BrandId) &&
            (!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId))//Get
        {
          
        }
    }
}
