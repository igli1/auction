namespace auction.Models.ViewModels;

public class ProductsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int? NumberOfBids { get; set; }
    public DateTime Date { get; set; }
}