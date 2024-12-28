using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Enitities;
using Talabat.Core.Enitities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static  class StoreContextSeed
    {
        //Seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {


            // Ensure the database context is not null
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            try
            {
                // Seed Product Brands
                await SeedDataAsync<ProductBrand>(dbContext, "../Talabat.Repository/Data/DataSeed/brands.json");

                // Seed Product Types
                await SeedDataAsync<ProductType>(dbContext, "../Talabat.Repository/Data/DataSeed/types.json");

                // Seed Products
                await SeedDataAsync<Product>(dbContext, "../Talabat.Repository/Data/DataSeed/products.json");
                await SeedDataAsync<DeliveryMethod>(dbContext, "../Talabat.Repository/Data/DataSeed/delivery.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during seeding: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedDataAsync<T>(StoreContext dbContext, string filePath) where T : class
        {
            // Check if data already exists
            if (await dbContext.Set<T>().AnyAsync()) return;

            // Read and deserialize JSON data
            var jsonData = await File.ReadAllTextAsync(filePath);
            var dataList = JsonSerializer.Deserialize<List<T>>(jsonData);

            if (dataList?.Count > 0)
            {
                await dbContext.Set<T>().AddRangeAsync(dataList); // Add all items in one call
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
