using auction.Models.Database.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auction.Models.Database;

public class AuctionDbContext : IdentityDbContext<ApplicationUser>
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }
}