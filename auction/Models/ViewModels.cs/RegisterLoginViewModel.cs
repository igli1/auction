using System.ComponentModel.DataAnnotations;

namespace auction.Models.ViewModels.cs;

public class RegisterLoginViewModel
{
    public Login Login { get; set; }
    public Register Register { get; set; }
}
