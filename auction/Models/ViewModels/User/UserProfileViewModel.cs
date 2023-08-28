using auction.Helpers;

namespace auction.Models.ViewModels;

public class UserProfileViewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [MaxFileSize(5 * 1024 * 1024)] // 5MB
    public IFormFile Image { get; set; }
}