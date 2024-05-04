using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbContext.ProductBrands.AddAsync(brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.ProductCategories.Any())
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        await dbContext.ProductCategories.AddAsync(category);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Products.AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var MethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);

                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var method in DeliveryMethods)
                    {
                        await dbContext.DeliveryMethods.AddAsync(method);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
