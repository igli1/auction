using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace auction.Models.Database.Entity;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    [Required]
    public Wallet Wallet { get; set; }
    public List<Product> UserProducts  { get; set; }
    public List<Bid> UserBids { get; set; }
    public List<Transaction> SentTransactions { get; set; }
    public List<Transaction> ReceivedTransactions { get; set; }
}