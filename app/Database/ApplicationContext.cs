using app.Database.Repositories.MSSQL;
using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductFromBasket> ProductsFromBasket { get; set; } = null!;
        public DbSet<ProductFromOrder> ProductsFromOrder { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOPALX\\SQLEXPRESS;Database=CourseProject;User Id=sa;Password=sa;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductFromBasket>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(u => u.Id).ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);
        } 
    }
}
