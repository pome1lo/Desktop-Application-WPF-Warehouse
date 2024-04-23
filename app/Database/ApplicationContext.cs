using app.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace app.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOPALX\\SQLEXPRESS;Database=CourseProject;User Id=sa;Password=sa;TrustServerCertificate=true;");
        }
          
        private static ApplicationContext? Instance;


        public static ApplicationContext GetContext()
        {
            if (Instance == null)
            {
                Instance = new ApplicationContext();
            }
            return Instance;
        } 
    }
}
