using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Baskets;
using Models.Catalog;
using Models.Orders;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CarMaker> Makers { get; set; }
        public DbSet<CarModel> Models { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        // public DbSet<CatalogItemCategory> CatalogItemCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<CatalogItemCategory>()
            //     .HasKey(cc => new { cc.CatalogItemId, cc.CategoryId });
            
            // builder.Entity<CatalogItemCategory>()
            //     .HasOne(cc => cc.CatalogItem)
            //     .WithMany(c => c.Categories)
            //     .HasForeignKey(cc => cc.CatalogItemId);
            
            // builder.Entity<CatalogItemCategory>()
            //     .HasOne(cc => cc.Category)
            //     .WithMany(c => c.CatalogItems)
            //     .HasForeignKey(cc => cc.CategoryId);

            builder.Entity<CatalogItem>()
                .HasMany(e => e.Comments)
                .WithOne(e => e.CatalogItem)
                .HasForeignKey(e => e.CatalogItemId)
                .IsRequired();

            builder.Entity<Category>()
                .HasMany(e => e.CatalogItems)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.Id)
                .IsRequired();
            
            builder.Entity<Comment>()
                .HasOne(c => c.CatalogItem)
                .WithMany(ci => ci.Comments)
                .HasForeignKey(c => c.CatalogItemId);

            builder.Entity<User>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>()
                .HasData(
                new Role { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                new Role { Id = 2, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = 3, Name = "User", NormalizedName = "User" }
                );
        }
    }
}
