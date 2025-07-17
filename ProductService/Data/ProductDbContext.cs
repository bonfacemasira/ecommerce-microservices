using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.StockQuantity)
                    .IsRequired();

                entity.Property(e => e.Category)
                    .HasMaxLength(50);

                entity.Property(e => e.Brand)
                    .HasMaxLength(50);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(200);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()");                // Index for better query performance
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Brand);
                entity.HasIndex(e => e.IsActive);
            });

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "iPhone 15 Pro",
                    Description = "Latest iPhone with advanced features",
                    Price = 999.99m,
                    StockQuantity = 50,
                    Category = "Electronics",
                    Brand = "Apple",
                    ImageUrl = "https://example.com/iphone15pro.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Samsung Galaxy S24",
                    Description = "Premium Android smartphone",
                    Price = 799.99m,
                    StockQuantity = 30,
                    Category = "Electronics",
                    Brand = "Samsung",
                    ImageUrl = "https://example.com/galaxys24.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "MacBook Pro 16\"",
                    Description = "Professional laptop for developers",
                    Price = 2499.99m,
                    StockQuantity = 20,
                    Category = "Computers",
                    Brand = "Apple",
                    ImageUrl = "https://example.com/macbookpro16.jpg",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
