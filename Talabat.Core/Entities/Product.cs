
namespace Talabat.Core.Entities
{
   
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public int BrandId { get; set; } //Foreign Key Column => ProductBrand
        public ProductBrand Brand { get; set; } //Navigational Property [One] 

        public int CategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }
}
