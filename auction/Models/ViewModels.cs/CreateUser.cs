using System.ComponentModel.DataAnnotations;

namespace auction.Models.ViewModels.cs;

public class CreateUser
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
    public string UserName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords doesn't match.")]
    public string ConfirmPassword { get; set; }
}