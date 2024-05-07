using MarketPlaceAPI.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MarketPlaceAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .ToTable("Users")
                .HasMany(u => u.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.Buyer)
                .HasForeignKey(t => t.BuyerId);


            modelBuilder.Entity<AppUser>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Transaction>()
                .ToTable("Transactions")
                .HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId);

            modelBuilder.Entity<Transaction>()
                 .HasOne(t => t.Buyer)
                 .WithMany(u => u.Transactions)
                 .HasForeignKey(t => t.BuyerId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}