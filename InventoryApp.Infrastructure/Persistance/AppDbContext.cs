using InventoryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items => Set<Item>();
        public DbSet<Party> Parties => Set<Party>();
        public DbSet<PartyCategory> PartyCategories => Set<PartyCategory>();
        public DbSet<ItemCategory> ItemCategories => Set<ItemCategory>();
        public DbSet<ItemPrice> ItemPrices => Set<ItemPrice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CostPrice).HasPrecision(18, 2);
                entity.Property(e => e.SellingPrice).HasPrecision(18, 2);
                entity.Property(e => e.SKU).HasMaxLength(50);
              
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Items)
                      .HasForeignKey(e => e.ItemCategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Party>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PANNumber).HasMaxLength(9);
                entity.Property(e => e.VATNumber).HasMaxLength(20);
                entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
                entity.Property(e => e.OpeningBalance).HasPrecision(18, 2);
                entity.Property(e => e.CreditLimit).HasPrecision(18, 2);
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Parties)
                      .HasForeignKey(e => e.PartyCategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<PartyCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<ItemPrice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.HasOne(e => e.Item)
                      .WithMany(i => i.Prices)
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}