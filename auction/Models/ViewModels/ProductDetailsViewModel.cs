namespace auction.Models.ViewModels;

public class ProductDetailsViewModel
{
    public string ProductName { get; set; }
    public string ProductOwner { get; set; }
    public int DaysRemaining { get; set; }
    public string Description { get; set; }
    public string BidderName { get; set; }
    public decimal HighestBid { get; set; }
}