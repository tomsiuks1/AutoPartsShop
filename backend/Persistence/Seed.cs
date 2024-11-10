
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Catalog;

namespace Persistence
{
    public class Seed
    {
      public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User { DisplayName = "Bob", UserName = "bob@gmail.com", Email = "bob@gmail.com" },
                    new User { DisplayName = "Thomas", UserName = "thomas@gmail.com", Email = "thomas@gmail.com" },
                    new User { DisplayName = "Rob", UserName = "rob@gmail.com", Email = "rob@gmail.com" },
                    new User { DisplayName = "Admin", UserName = "admin@gmail.com", Email = "admin@test.com" },
                    new User { DisplayName = "Tomas", UserName = "tomsiuks1@gmail.com", Email = "tomsiuks1@gmail.com" }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "test");

                    if (user.DisplayName == "Tomas" || user.DisplayName == "Admin")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "Member");
                    }
                }
            }

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Engines" },
                    new Category { Name = "Electronics" },
                    new Category { Name = "Timing Belts" },
                    new Category { Name = "Engine Parts" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var enginesCategory = context.Categories.First(c => c.Name == "Engines");
                var electronicsCategory = context.Categories.First(c => c.Name == "Electronics");
                var timingBeltsCategory = context.Categories.First(c => c.Name == "Timing Belts");
                var enginePartsCategory = context.Categories.First(c => c.Name == "Engine Parts");

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "V8 Engine",
                        Description = "High performance V8 engine.",
                        Price = 5000,
                        PictureUrl = "https://w7.pngwing.com/pngs/945/462/png-transparent-bmw-x6-m-car-v8-engine-car-engine-car-vehicle-transport-thumbnail.png",
                        CategoryId = enginesCategory.Id,
                        Type = enginesCategory.Name,
                        Brand = "BMW",
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 12
                    },
                    new Product
                    {
                        Name = "BMW 328 oil pump",
                        Description = "Latest model with advanced features.",
                        Price = 699,
                        PictureUrl = "https://cdn11.bigcommerce.com/s-i3kt5xjesl/images/stencil/640w/products/12222/32167/3144_11412245182__09681.1683632558.png",
                        CategoryId = enginePartsCategory.Id,
                        Brand = "BMW",
                        Type = enginePartsCategory.Name,
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 15
                    },
                    new Product
                    {
                        Name = "Timing Belt",
                        Description = "Durable timing belt for various engines.",
                        Price = 5900,
                        PictureUrl = "https://e7.pngegg.com/pngimages/931/170/png-clipart-car-timing-belt-motor-vehicle-service-camshaft-belt-automotive-car.png",
                        CategoryId = timingBeltsCategory.Id,
                        Brand = "Porche",
                        Type = timingBeltsCategory.Name,
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 99
                    },
                    new Product
                    {
                        Name = "Spark Plug",
                        Description = "High performance spark plug.",
                        Price = 899,
                        PictureUrl = "https://e7.pngegg.com/pngimages/813/386/png-clipart-spark-plug-ac-power-plugs-and-sockets-ngk-electrical-connector-ignition-system-spark-plug-miscellaneous-air-filter-thumbnail.png",
                        CategoryId = electronicsCategory.Id,
                        Brand = "NGK",
                        Type = electronicsCategory.Name,
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 10
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            if (!context.Comments.Any())
            {
                var bob = await userManager.FindByEmailAsync("bob@gmail.com");
                var thomas = await userManager.FindByEmailAsync("thomas@gmail.com");
                var rob = await userManager.FindByEmailAsync("rob@gmail.com");

                var smartphone = context.Products.First(ci => ci.Name == "Smartphone");
                var v8Engine = context.Products.First(ci => ci.Name == "V8 Engine");

                var comments = new List<Comment>
                {
                    new Comment
                    {
                        Content = "This smartphone is amazing!",
                        UserId = bob.Id,
                        ProductId = smartphone.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Comment
                    {
                        Content = "The V8 engine performs exceptionally well.",
                        UserId = thomas.Id,
                        ProductId = v8Engine.Id,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-10)
                    },
                    new Comment
                    {
                        Content = "Great value for the price.",
                        UserId = rob.Id,
                        ProductId = smartphone.Id,
                        CreatedAt = DateTime.UtcNow.AddHours(-1)
                    }
                };

                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();
            }
        }
    }
}
