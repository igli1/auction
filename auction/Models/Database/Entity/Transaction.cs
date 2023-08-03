using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Bid")]
    public int BidId { get; set; }
    [Required]
    public Bid Bid { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
    [Required]
    [ForeignKey("Buyer")]
    public string BuyerId { get; set; }
    
    [Required]
    public ApplicationUser Buyer { get; set; }
    [Required]
    [ForeignKey("Seller")]
    public string SellerId { get; set; }

    [Required]
    public ApplicationUser Seller { get; set; }

    public SoldItem SoldItem { get; set; }
}