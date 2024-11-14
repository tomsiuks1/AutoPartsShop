
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
                    new Category {
                        Id = Guid.Parse("7ad5058f-5b0c-493f-8ba7-ce6d01247c55"),
                        Name = "Engines",
                        Products = new List<Product>()
                    },
                    new Category { 
                        Id = Guid.Parse("81fb4196-6036-4045-b7b3-e367407850a8"),
                        Name = "Electronics",
                        Products = new List<Product>()
                    },
                    new Category { 
                        Id = Guid.Parse("df07f1c2-2cdd-4c63-910c-0761c42cc36c"),
                        Name = "Timing Belts",
                        Products = new List<Product>()
                    },
                    new Category { 
                        Id = Guid.Parse("7b19cfaf-b0cf-4160-bf30-6d0dfbd32dba"),
                        Name = "Engine Parts",
                        Products = new List < Product >()
                    }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var bob = await userManager.FindByEmailAsync("bob@gmail.com");
                var rob = await userManager.FindByEmailAsync("rob@gmail.com");

                var enginesCategory = context.Categories.First(c => c.Name == "Engines");
                var electronicsCategory = context.Categories.First(c => c.Name == "Electronics");
                var timingBeltsCategory = context.Categories.First(c => c.Name == "Timing Belts");
                var enginePartsCategory = context.Categories.First(c => c.Name == "Engine Parts");

                var products = new List<Product>
                {
                    new Product
                    {
                        Id = Guid.Parse("83743a08-ad0e-466d-864f-85735440b68a"),
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
                        Id = Guid.Parse("b57983bf-707d-4477-96fa-18f66c16d73e"),
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
                        Id = Guid.Parse("42b523bb-6c90-45f2-acca-be5d5b6065d9"),
                        Name = "Timing Belt",
                        Description = "Durable timing belt for various engines.",
                        Price = 5900,
                        PictureUrl = "https://e7.pngegg.com/pngimages/931/170/png-clipart-car-timing-belt-motor-vehicle-service-camshaft-belt-automotive-car.png",
                        CategoryId = timingBeltsCategory.Id,
                        Brand = "Porche",
                        Type = timingBeltsCategory.Name,
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 99,
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Id = Guid.Parse("c01fa4c0-7af4-4f44-a026-00cd8c3300d8"),
                                DisplayName = bob.Email,
                                Content = "This timing belt is amazing!",
                                UserId = userManager.Users.First(u => u.DisplayName == "Bob").Id,
                                CreatedAt = DateTime.UtcNow,
                                ProductId = Guid.Parse("42b523bb-6c90-45f2-acca-be5d5b6065d9")
                            },
                            new Comment
                            {
                                Id = Guid.Parse("90ac1822-7481-4dae-bf93-50714cd4a44b"),
                                DisplayName = bob.Email,
                                Content = "This timing belt is bad!",
                                UserId = userManager.Users.First(u => u.DisplayName == "Bob").Id,
                                CreatedAt = DateTime.UtcNow,
                                ProductId = Guid.Parse("42b523bb-6c90-45f2-acca-be5d5b6065d9")
                            }
                        }
                    },
                    new Product
                    {
                        Id = Guid.Parse("618bb071-320e-4a38-a4a7-03a57e928efc"),
                        Name = "Spark Plug",
                        Description = "High performance spark plug.",
                        Price = 899,
                        PictureUrl = "https://e7.pngegg.com/pngimages/813/386/png-clipart-spark-plug-ac-power-plugs-and-sockets-ngk-electrical-connector-ignition-system-spark-plug-miscellaneous-air-filter-thumbnail.png",
                        CategoryId = electronicsCategory.Id,
                        Brand = "NGK",
                        Type = electronicsCategory.Name,
                        CreatedAt = DateTime.UtcNow,
                        QuantityInStock = 10,
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Id = Guid.Parse("4ccb411a-681b-4ef6-8a58-5c7f745ada60"),
                                Content = "Great value for the price.",
                                UserId = rob.Id,
                                ProductId = Guid.Parse("618bb071-320e-4a38-a4a7-03a57e928efc"),
                                CreatedAt = DateTime.UtcNow.AddHours(-1)
                            }
                        }
                    }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            //var product = await context.Products.FirstOrDefaultAsync(ci => ci.Id == Guid.Parse("42b523bb-6c90-45f2-acca-be5d5b6065d9"));

            if (!context.Comments.Any())
            {
                var thomas = await userManager.FindByEmailAsync("thomas@gmail.com");

                var smartphone = context.Products.First(ci => ci.Name == "Smartphone");
                var v8Engine = context.Products.First(ci => ci.Name == "V8 Engine");

                var comments = new List<Comment>
                {
                    new Comment
                    {   
                        Id = Guid.Parse("2987cf66-a031-4333-b8b2-739788a38459"),
                        Content = "The V8 engine performs exceptionally well.",
                        UserId = thomas.Id,
                        ProductId = v8Engine.Id,
                        CreatedAt = DateTime.UtcNow.AddMinutes(-10)
                    }
                };

                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();
            }
        }
    }
}
