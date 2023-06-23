using auction.Models.Database.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auction.Models.Database;

public class AuctionDbContext : IdentityDbContext<ApplicationUser>
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<SoldItem> SoldItem { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Bid> Bid { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>().ToTable("Wallets");
        modelBuilder.Entity<SoldItem>().ToTable("SoldItems");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Transaction>().ToTable("Transactions");
        modelBuilder.Entity<Bid>().ToTable("Bids");
        
        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.ApplicationUser)
            .WithOne(au => au.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SoldItem>()
            .HasOne(si => si.Buyer)
            .WithMany(au => au.SoldItems)
            .HasForeignKey(si => si.UserId)
            .OnDelete(DeleteBehavior.Restrict);;
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(si => si.Transaction)
            .WithOne(t => t.SoldItem)
            .HasForeignKey<SoldItem>(si => si.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(si => si.Product)
            .WithOne(p => p.SoldItem)
            .HasForeignKey<SoldItem>(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Wallet)
            .WithMany(w => w.Transactions)
            .HasForeignKey(w => w.WalletId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Product)
            .WithMany(p => p.Bids)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}