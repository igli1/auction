using Microsoft.AspNetCore.Identity;

namespace auction.Models.Database.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Wallet Wallet { get; set; }
    public List<SoldItem> SoldItems { get; set; }
    public List<Product> Products { get; set; }
}