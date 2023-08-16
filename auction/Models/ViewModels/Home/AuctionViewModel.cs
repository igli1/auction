namespace auction.Models.ViewModels;

public class AuctionViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string SellerName { get; set; }
    public decimal TopBid { get; set; }
    public string TimeRemaining { get; set; }
    public bool IsCurrentUserProductOwner { get; set; }
}