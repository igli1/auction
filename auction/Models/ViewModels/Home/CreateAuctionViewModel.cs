using System.ComponentModel.DataAnnotations;
using auction.Helpers;

namespace auction.Models.ViewModels;

public class CreateAuctionViewModel
{
    [Required(ErrorMessage = "Product Name is required.")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Starting Bid is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Starting Bid must be a positive value.")]
    public decimal StartingBid { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "End Date is required.")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1).Date.AddHours(12);
    [MaxFileSize(5 * 1024 * 1024)] // 5MB
    public IFormFile? Image { get; set; }
}