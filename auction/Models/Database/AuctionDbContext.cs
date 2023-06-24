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
    public DbSet<Product> Product { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Bid> Bid { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>().ToTable("Wallets");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Transaction>().ToTable("Transactions");
        modelBuilder.Entity<Bid>().ToTable("Bids");
        
        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.User)
            .WithOne(au => au.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Bid>()
            .HasOne(b => b.Product)
            .WithMany(p => p.ProductBids)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Product>()
            .HasOne(b => b.Seller)
            .WithMany(u => u.UserProducts)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromUser)
            .WithMany(u => u.SentTransactions)
            .HasForeignKey(t => t.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToUser)
            .WithMany(u => u.ReceivedTransactions)
            .HasForeignKey(t => t.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}