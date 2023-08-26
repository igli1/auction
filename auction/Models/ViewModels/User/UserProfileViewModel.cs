namespace auction.Models.ViewModels;

public class UserProfileViewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile Image { get; set; }
}