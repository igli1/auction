using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace auction.Models.ViewModels;

[Bind(Prefix = "Bid")]
public class BidViewModel
{
    [Required]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "Bid is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Bid must be a positive value.")]
    public decimal BidAmount { get; set; }
    [Required]
    public string UserId { get; set; }
}