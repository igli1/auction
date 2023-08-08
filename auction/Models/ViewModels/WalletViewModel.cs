namespace auction.Models.ViewModels;

public class WalletViewModel
{
    public decimal WalletValue { get; set; }
    public List<ProductsViewModel> ProductsSold { get; set; }
    public List<ProductsViewModel> ProductsBought { get; set; }
}