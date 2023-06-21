using auction.Models.Database.Entity;
using auction.Models.ViewModels.cs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auction.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public IActionResult RegisterAndLogin()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(Login model)
    {
        if (!ModelState.IsValid)
        {
            var rlVm = new RegisterLoginViewModel
            {
                Login = model
            };
            return View("RegisterAndLogin", rlVm);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if (!ModelState.IsValid)
        {
            var rlVm = new RegisterLoginViewModel
            {
                Register = model
            };
            return View("RegisterAndLogin", rlVm);
        }

        var applicationUser = new ApplicationUser() { UserName = model.UserName, FirstName = model.FirstName.ToLower(), LastName = model.LastName.ToLower()};
        
        var result = await _userManager.CreateAsync(applicationUser, model.Password);

        if (!result.Succeeded)
        {
            var rlVm = new RegisterLoginViewModel
            {
                Register = model
            };
            return View("RegisterAndLogin", rlVm);
        }
        
        return RedirectToAction("Index", "Home");
    }
}