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
    
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateUser model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var applicationUser = new ApplicationUser() { UserName = model.UserName, FirstName = model.FirstName.ToLower(), LastName = model.LastName.ToLower()};
        
        var result = await _userManager.CreateAsync(applicationUser, model.Password);
        
        
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return RedirectToAction("Index", "Home");
    }
}