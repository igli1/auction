using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class SoldItem
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Seller")]
    public string SellerId { get; set; }
    [Required]
    public ApplicationUser Seller { get; set; }
    [Required]
    [ForeignKey("Buyer")]
    public string Buyerid { get; set; }
    [Required]
    public ApplicationUser Buyer { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    [Required]
    public Product Product { get; set; }
    [Required]
    [ForeignKey("Transaction")]
    public int TransactionId { get; set; }
    [Required]
    public Transaction Transaction { get; set; }
}