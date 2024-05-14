using LutongBahayApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models;

namespace LutongBahayApp.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderFood> OrderFoods { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketFood> BasketFoods { get; set; }
        public DbSet<FoodPriceChange> FoodPriceChanges { get; set; }
        public DbSet<SoldFoodMetric> SoldFoodMetrics { get; set; }
        public DbSet<ReportIncident> ReportIncidents { get; set; }
        public DbSet<SoldMarketMetrics> SoldMarketMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderFood>().HasNoKey();
            modelBuilder.Entity<BasketFood>().HasNoKey();

            modelBuilder.Entity<OrderFood>()
                    .HasKey(pc => new { pc.FoodId, pc.OrderId });
            modelBuilder.Entity<OrderFood>()
                    .HasOne(p => p.Food)
                    .WithMany(pc => pc.OrderFood)
                    .HasForeignKey(p => p.FoodId);
            modelBuilder.Entity<OrderFood>()
                    .HasOne(p => p.Order)
                    .WithMany(pc => pc.OrderFood)
                    .HasForeignKey(c => c.OrderId);

            modelBuilder.Entity<BasketFood>()
                    .HasKey(pc => new { pc.FoodId, pc.BasketId });
            modelBuilder.Entity<BasketFood>()
                    .HasOne(p => p.Food)
                    .WithMany(pc => pc.BasketFood)
                    .HasForeignKey(p => p.FoodId);
            modelBuilder.Entity<BasketFood>()
                    .HasOne(p => p.Basket)
                    .WithMany(pc => pc.BasketFood)
                    .HasForeignKey(c => c.BasketId);
        }
    }
}
