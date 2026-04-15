using Microservices.ProudctAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.ProudctAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId= 1,
                Name="Samosa",
                Price=15,
                Description= "Samosa Description ... ",
                CategoryName="Appetizer",
                ImageUrl = "https://placehold.co/600x400"

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Kachori",
                Price = 30,
                Description = "Kachori Description ... ",
                CategoryName = "Appetizer",
                ImageUrl = "https://placehold.co/600x400"

            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Dabeli",
                Price = 35,
                Description = "Dabeli Description ... ",
                CategoryName = "Dessert",
                ImageUrl = "https://placehold.co/600x400"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Pav Bhaji",
                Price = 40,
                Description = "Pav Bhaji Description ... ",
                CategoryName = "Dessert",
                ImageUrl = "https://placehold.co/600x400"

            });
        }
    }
}
