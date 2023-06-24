using System.ComponentModel.DataAnnotations;

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
    public DateTime EndDate { get; set; }
}