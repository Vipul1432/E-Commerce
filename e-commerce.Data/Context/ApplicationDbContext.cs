using e_commerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Data.Context
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            //seed category
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, Name = "Electronics" },
                new Category { CategoryId = 2, Name = "Clothing" },
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new List<Product>
            {
                new Product { ProductId = 10, Name = "Smartphone", Price = 499.99m, CategoryId = 1 },
                new Product { ProductId = 11, Name = "Laptop", Price = 899.99m, CategoryId = 1 },
                new Product { ProductId = 12, Name = "T-Shirt", Price = 19.99m, CategoryId = 2 },
                new Product { ProductId = 13, Name = "Jeans", Price = 19.99m, CategoryId = 2 },
            };

            modelBuilder.Entity<Product>().HasData(products);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
