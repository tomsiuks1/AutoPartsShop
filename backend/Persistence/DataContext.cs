using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Baskets;
using Models.Catalog;
using Models.Orders;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User, Role, Guid>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasMany(c => c.CatalogItems)
                .WithOne(ci => ci.Category)
                .HasForeignKey(ci => ci.CategoryId);

            builder.Entity<CatalogItem>()
                .HasMany(ci => ci.Comments)
                .WithOne(c => c.CatalogItem)
                .HasForeignKey(c => c.CatalogItemId);

            builder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>()
                .HasData(
                new Role { Id = Guid.NewGuid(), Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User" }
                );
        }
    }
}
