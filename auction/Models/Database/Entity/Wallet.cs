using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auction.Models.Database.Entity;

public class Wallet
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }
    [Required]
    public ApplicationUser User { get; set; }
}