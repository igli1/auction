using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class Bid
{
    [Key]
    public int Id { get; set; }
    [Required]
    [ForeignKey("Bidder")]
    public string BidderId { get; set; }
    [Required]
    public ApplicationUser Bidder { get; set; }
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Required]
    public Product Product { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime BidTime { get; set; } = DateTime.UtcNow;
    public Transaction Transaction { get; set; }
}