using auction.Models.Database.Entity;
using auction.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auction.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public UserController(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public IActionResult RegisterAndLogin()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        var rlVm = new RegisterLoginViewModel
        {
            Login = model
        };
        if (!ModelState.IsValid)
        {
            return View("RegisterAndLogin", rlVm);
        }
        var user = await _userManager.FindByNameAsync(model.UserName);
        
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User does not exist.");

            return View("RegisterAndLogin", rlVm);
        }
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

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

        var wallet = new Wallet
        {
            Balance = 1000
        };
        
        var applicationUser = new ApplicationUser() { UserName = model.UserName, 
            FirstName = model.FirstName.ToLower(), 
            LastName = model.LastName.ToLower(),
            Wallet = wallet
        };
        
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
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("RegisterAndLogin");
    }
}