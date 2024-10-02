
using Microsoft.AspNetCore.Identity;
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

            if (!context.CatalogItems.Any())
            {
                var catalogItems = new List<CatalogItem>
                {
                        new CatalogItem
                        {
                            Name = "BMW 320 engine",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 20000,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/478/2387857.png",
                            Brand = "BMW",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW 328 engine",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 21000,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/478/2387857.png",
                            Brand = "BMW",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW 330 engine",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 25000,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/478/2387857.png",
                            Brand = "BMW",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW 335 engine",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 30000,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/478/2387857.png",
                            Brand = "BMW",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Audi A4",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 25000,
                            PictureUrl = "https://s19531.pcdn.co/wp-content/uploads/2021/09/2012_Golf_GTI-Large-3315-copy.jpg",
                            Brand = "Audi",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Audi A6",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 25000,
                            PictureUrl = "https://s19531.pcdn.co/wp-content/uploads/2021/09/2012_Golf_GTI-Large-3315-copy.jpg",
                            Brand = "Audi",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Audi A8",
                            Description =
                                "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 25000,
                            PictureUrl = "https://s19531.pcdn.co/wp-content/uploads/2021/09/2012_Golf_GTI-Large-3315-copy.jpg",
                            Brand = "Audi",
                            Type = "Engine",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Porche 911 Engine Part",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 100,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/479/2392872.png",
                            Brand = "Porche",
                            Type = "Engine Part",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Porche 911 Engine Part 2",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 300,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/479/2392872.png",
                            Brand = "Porche",
                            Type = "Engine Part",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Porche 911 Engine Part 3",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 100,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/479/2392872.png",
                            Brand = "Porche",
                            Type = "Engine Part",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW 530d Timing Belt",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 1500,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/480/2398857.png",
                            Brand = "Continental",
                            Type = "Timing Belt",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW 420d Timing Belt",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 1500,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/480/2398857.png",
                            Brand = "Continental",
                            Type = "Timing Belt",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "Porche Cayman Timing Belt",
                            Description =
                                "Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                            Price = 1500,
                            PictureUrl = "https://www.bmwautodalys.lt/diagrams/s3000002/480/2398857.png",
                            Brand = "Continental",
                            Type = "Timing Belt",
                            QuantityInStock = 100
                        },
                        new CatalogItem
                        {
                            Name = "BMW M4",
                            Description =
                                "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                            Price = 19999,
                            PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRwT6rZOIerhgI1MwefF0rpmX7nLRbKJ65OSnSaTvJ-hw&s",
                            Brand = "BMW",
                            Type = "Timing Chain",
                            QuantityInStock = 1000
                        },
                        new CatalogItem
                        {
                            Name = "BMW M3",
                            Description =
                                "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                            Price = 1999,
                            PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRwT6rZOIerhgI1MwefF0rpmX7nLRbKJ65OSnSaTvJ-hw&s",
                            Brand = "BMW",
                            Type = "Timing Chain",
                            QuantityInStock = 1000
                        },
                        new CatalogItem
                        {
                            Name = "BMW 435i",
                            Description =
                                "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                            Price = 20999,
                            PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRwT6rZOIerhgI1MwefF0rpmX7nLRbKJ65OSnSaTvJ-hw&s",
                            Brand = "BMW",
                            Type = "Timing Chain",
                            QuantityInStock = 1000
                        },
                };

                await context.CatalogItems.AddRangeAsync(catalogItems);
            }

            await context.SaveChangesAsync();
        }
    }
}
