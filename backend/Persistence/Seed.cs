﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Catalog;
using Persistence.CatalogItems;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User{DisplayName = "Bob", UserName = "bob@gmail.com",  Email = "bob@gmail.com"},
                    new User{DisplayName = "Thomas", UserName = "thomas@gmail.com",  Email = "thomas@gmail.com"},
                    new User{DisplayName = "Rob", UserName = "rob@gmail.com",  Email = "rob@gmail.com"},
                    new User{DisplayName = "Admin", UserName = "admin@gmail.com", Email = "admin@test.com" },
                    new User{DisplayName = "Tomas", UserName = "tomsiuks1@gmail.com", Email = "tomsiuks1@gmail.com" }
                };

                foreach(var user in users)
                {
                    await userManager.CreateAsync(user, "test");

                    if(user.DisplayName == "Tomas" || user.DisplayName == "Admin")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "Member");
                    }
                }
            }

            var makers = CatalogMaker.GetMakers();
            if (!context.Makers.Any())
            {
                await context.Makers.AddRangeAsync(makers);
            }

            if (!context.Models.Any())
            {
                var carModels = new List<CarModel>
                {
                    new CarModel { Id = Guid.NewGuid(), MakeId = makers.First(m => m.Name == "BMW").Id, Name = "320" },
                    new CarModel { Id = Guid.NewGuid(), MakeId = makers.First(m => m.Name == "BMW").Id, Name = "325" },
                    new CarModel { Id = Guid.NewGuid(), MakeId = makers.First(m => m.Name == "BMW").Id, Name = "335" },
                    new CarModel { Id = Guid.NewGuid(), MakeId = makers.First(m => m.Name == "Audi").Id, Name = "A4" },
                };

                await context.Models.AddRangeAsync(carModels);
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

        if (!context.CatalogItems.Any())
        {
            var catalogItems = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Name = "BMW 320 engine",
                    Description = "Lorem ipsum dolor sit amet...",
                    Price = 20000,
                    PictureUrl = "https://example.com/bmw320.png",
                    Brand = "BMW",
                    Type = "Engine",
                    QuantityInStock = 100,
                    Category = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Engines") 
                },
            };

            await context.CatalogItems.AddRangeAsync(catalogItems);
            await context.SaveChangesAsync();
        }

        if (!context.Comments.Any())
        {
            var catalogItem = await context.CatalogItems.FirstOrDefaultAsync(ci => ci.Name == "BMW 320 engine");

            if (catalogItem != null)
            {
                var comments = new List<Comment>
                {
                    new Comment { Content = "Great engine!", DisplayName = "User1", CatalogItemId = catalogItem.Id },
                    new Comment { Content = "Very efficient!", DisplayName = "User2", CatalogItemId = catalogItem.Id }
                };

                await context.Comments.AddRangeAsync(comments);
                await context.SaveChangesAsync();
            }
        }
            await context.SaveChangesAsync();
        }
    }
}
