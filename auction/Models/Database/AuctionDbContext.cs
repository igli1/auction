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
    public DbSet<SoldItem> SoldItem { get; set; }
    public DbSet<Bid> Bid { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>().ToTable("Wallets");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Transaction>().ToTable("Transactions");
        modelBuilder.Entity<Bid>().ToTable("Bids");
        modelBuilder.Entity<SoldItem>().ToTable("SoldItems");
        
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
            .HasOne(t => t.Buyer)
            .WithMany(u => u.SentTransactions)
            .HasForeignKey(t => t.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Seller)
            .WithMany(u => u.ReceivedTransactions)
            .HasForeignKey(t => t.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Bid)
            .WithOne(b => b.Transaction)
            .HasForeignKey<Transaction>(t => t.BidId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(t => t.Seller)
            .WithMany(u => u.Seller)
            .HasForeignKey(t => t.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(t => t.Buyer)
            .WithMany(u => u.Buyer)
            .HasForeignKey(t => t.Buyerid)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(w => w.Product)
            .WithOne(au => au.SoldItem)
            .HasForeignKey<SoldItem>(w => w.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SoldItem>()
            .HasOne(w => w.Transaction)
            .WithOne(au => au.SoldItem)
            .HasForeignKey<SoldItem>(w => w.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}