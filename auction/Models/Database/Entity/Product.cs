namespace auction.Models.Database.Entity;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal StartingPrice { get; set; }
    public bool isDeleted { get; set; } = false;
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public List<Bid> Bids { get; set; }
    public SoldItem SoldItem { get; set; }
}