namespace auction.Models.Database.Entity;

public class Wallet
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public List<Transaction>? Transactions { get; set; }
}