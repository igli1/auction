using Microsoft.AspNetCore.Identity;

namespace auction.Models.Database.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}