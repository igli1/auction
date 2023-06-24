using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using auction.Models;
using auction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace auction.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [Authorize]
    public IActionResult NewAuction()
    {
        return View();
    }
    [Authorize]
    [HttpPost]
    public IActionResult NewAuction(CreateAuctionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        return View();
    }
    [Authorize]
    public IActionResult ProductDetails(int Id)
    {
        return View();
    }
}