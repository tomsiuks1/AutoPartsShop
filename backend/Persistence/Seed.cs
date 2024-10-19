
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Catalog;

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
                var engineCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Engines");

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
                        CategoryId = engineCategory!.Id
                    }
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
                        new Comment
                        {
                            Content = "Great engine!",
                            DisplayName = "User1",
                            CatalogItemId = catalogItem.Id
                        },
                        new Comment
                        {
                            Content = "Very efficient!",
                            DisplayName = "User2",
                            CatalogItemId = catalogItem.Id
                        }
                    };

                    await context.Comments.AddRangeAsync(comments);
                    await context.SaveChangesAsync();
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
