namespace auction.Models.Database.Entity;

public class Bid
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; }
}