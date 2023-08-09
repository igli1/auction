namespace auction.Models.ViewModels;

public class WalletViewModel
{
    public decimal WalletTotalValue { get; set; }
    public decimal OnHold { get; set; }
    public List<ProductsViewModel> ProductsSold { get; set; }
    public List<ProductsViewModel> ProductsBought { get; set; }
}