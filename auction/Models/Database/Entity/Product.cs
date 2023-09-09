using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal StartingPrice { get; set; }
    [Required]
    public bool isDeleted { get; set; } = false;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }
    [StringLength(500)]
    public string Description { get; set; }
    [Required]
    [ForeignKey("Seller")]
    public string SellerId { get; set; }
    [Required]
    public ApplicationUser Seller { get; set; }
    public List<Bid> ProductBids { get; set; }
    public SoldItem SoldItem { get; set; }
    public string? Image { get; set; }
}