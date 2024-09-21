using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Core.Entities;

namespace Infrastructure.Data;

public class SeedContext()
{
    

    public static async Task SeedData(StoreContext context)
    {

        if (!context.Products.Any()){
         var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
           
            var product = JsonSerializer.Deserialize<List<Product>>(productsData);
             if (product == null) return;
             context.AddRange(product);
             await context.SaveChangesAsync();

        }

    }
}