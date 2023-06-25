namespace auction.Models.ViewModels;

public class ProductDetailsViewModel
{
    public string UserId { get; set; }
    public bool IsCurrentUserProductOwner { get; set; }
    public bool IsCurrentUserHighestBidder { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductOwner { get; set; }
    public string DaysRemaining { get; set; }
    public string Description { get; set; }
    public string BidderName { get; set; }
    public decimal HighestBid { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal UserBalance { get; set; }
    public BidViewModel Bid { get; set; }
}