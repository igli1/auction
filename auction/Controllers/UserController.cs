using System.Security.Claims;
using auction.Models.Database.Entity;
using auction.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(Login model)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        
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
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        
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
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            };
            return View("RegisterAndLogin", rlVm);
        }
        
        return RedirectToAction("RegistrationSuccess", "User", new { username = model.UserName });
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("RegisterAndLogin");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        var profile = user.Adapt<UserProfileViewModel>();
        return View(profile);
    }
    
    [Authorize]
    public async Task<IActionResult> Wallet()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        var profile = user.Adapt<UserProfileViewModel>();
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> Products()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        var profile = user.Adapt<UserProfileViewModel>();
        return View();
    }
    
    [Authorize]
    public async Task<IActionResult> Bids()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        var profile = user.Adapt<UserProfileViewModel>();
        return View();
    }

    public IActionResult RegistrationSuccess(string username)
    {
        ViewData["Username"] = username;
        return View();
    }
}