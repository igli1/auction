using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Timestamp]
    public DateTime Timestamp { get; set; }
    [Required]
    [ForeignKey("FromUser")]
    public string FromUserId { get; set; }
    
    [Required]
    public ApplicationUser FromUser { get; set; }
    [Required]
    [ForeignKey("ToUser")]
    public string ToUserId { get; set; }

    [Required]
    public ApplicationUser ToUser { get; set; }
}