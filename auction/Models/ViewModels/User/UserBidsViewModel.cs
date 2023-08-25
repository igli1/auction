namespace auction.Models.ViewModels;

public class UserBidsViewModel
{
    public string ProductName { get; set; }
    public string ProductId { get; set; }
    public decimal BidAmount { get; set; }
    public decimal InitialAmount { get; set; }
    public string ProductStatus { get; set; }
}