namespace auction.Models.ViewModels;

public class AuctionIndexViewModel
{
    public List<AuctionViewModel> Auctions { get; set; }
    public decimal WalletValue { get; set; }
}