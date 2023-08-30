namespace auction.Models.ViewModels;

public class UserBidsViewModel
{
    public string ProductName { get; set; }
    public int ProductId { get; set; }
    public decimal BidAmount { get; set; }
    public decimal InitialAmount { get; set; }
    public string BidStatus { get; set; }
    public DateTime EndDate { get; set; }
}