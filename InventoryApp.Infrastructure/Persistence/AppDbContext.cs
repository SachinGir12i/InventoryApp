using InventoryApp.Domain.Entities;
using InventoryApp.Domain.Enums;
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
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            // Purchase config
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PurchaseNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SubTotal).HasPrecision(18, 2);
                entity.Property(e => e.VATAmount).HasPrecision(18, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.PaidAmount).HasPrecision(18, 2);
                entity.Property(e => e.DueAmount).HasPrecision(18, 2);

                // Purchase belongs to one Supplier (Party)
                entity.HasOne(e => e.Supplier)
                      .WithMany()
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PurchaseItem config
            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(18, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.LineTotal).HasPrecision(18, 2);

                // PurchaseItem belongs to one Purchase
                entity.HasOne(e => e.Purchase)
                      .WithMany(p => p.PurchaseItems)
                      .HasForeignKey(e => e.PurchaseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // PurchaseItem references one Item
                entity.HasOne(e => e.Item)
                      .WithMany()
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PartyCategory>().HasData(
               new PartyCategory { Id = 1, Name = "Customer", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
               new PartyCategory { Id = 2, Name = "Supplier", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
               new PartyCategory { Id = 3, Name = "Other", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
            modelBuilder.Entity<Party>().HasData(
                new Party
                {
                    Id = 1,
                    Name = "Default Supplier",
                    Type = PartyType.Supplier,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    PartyCategoryId = 2 // Make sure this matches the seeded "Supplier" PartyCategory
                    // Set other required properties as needed, e.g. OpeningBalance, CurrentBalance, etc.
                }
            );
            // Sale config
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SaleNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SubTotal).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
                entity.Property(e => e.VATAmount).HasPrecision(18, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.ReceivedAmount).HasPrecision(18, 2);
                entity.Property(e => e.DueAmount).HasPrecision(18, 2);

                // Sale belongs to one Customer (Party)
                entity.HasOne(e => e.Customer)
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // SaleItem config
            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(18, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.LineDiscount).HasPrecision(18, 2);
                entity.Property(e => e.LineTotal).HasPrecision(18, 2);

                // SaleItem belongs to one Sale
                entity.HasOne(e => e.Sale)
                      .WithMany(s => s.SaleItems)
                      .HasForeignKey(e => e.SaleId)
                      .OnDelete(DeleteBehavior.Cascade);

                // SaleItem references one Item
                entity.HasOne(e => e.Item)
                      .WithMany()
                      .HasForeignKey(e => e.ItemId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}