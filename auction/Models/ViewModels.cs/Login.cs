using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace auction.Models.ViewModels.cs;
[Bind(Prefix = "Login")]
public class Login
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters.")]
    public string UserName { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }
}