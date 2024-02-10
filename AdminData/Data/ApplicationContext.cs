using AdminData.Models.Admin;
using AdminData.Models.Category;
using AdminData.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Web_Admin.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            { }

        public DbSet<Admin_db> Auth_user {  get; set; } 
        public DbSet<Category_db> Category {  get; set; }

        public DbSet<Product_db> Product_set { get; set; }



        
        // Other DbSet properties...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your model here...

            modelBuilder.Entity<Admin_db>(entity =>
            {
                // Map your Auth_user properties...
                entity.Property(e => e.ResetToken).HasMaxLength(255);
                entity.Property(e => e.ResetTokenExpiration).HasColumnType("datetime");
            });

            // Additional configurations...

            base.OnModelCreating(modelBuilder);
        }
    }
    }


