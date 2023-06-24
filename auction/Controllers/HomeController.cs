using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using auction.Models;
using auction.Models.Database;
using auction.Models.Database.Entity;
using auction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace auction.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuctionDbContext _context;

    public HomeController(ILogger<HomeController> logger, AuctionDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var products = await _context.Product
            .Include(n => n.User)
            .Include(p => p.Bids)
            .Where(p => !p.isDeleted && p.EndDate >= DateTime.UtcNow)
            .Select(p => new AuctionViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                SellerName = p.User.UserName,
                TimeRemaining = (p.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
                IsCurrentUserProductOwner = p.UserId == userId
            })
            .ToListAsync();
        
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
        
        var auction = new AuctionIndexViewModel
        {
            Auctions = products,
            WalletValue = wallet?.Balance ?? 0
        };
        
        return View(auction);
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
    public async Task<IActionResult> NewAuction(CreateAuctionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var product = new Product
        {
            Name = model.ProductName,
            StartingPrice = model.StartingBid,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            EndDate = model.EndDate
        };
        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [Authorize]
    public async Task<IActionResult> ProductDetails(int Id)
    {
        var product = await _context.Product
            .Include(p => p.User)
            .Include(p => p.Bids)
            .ThenInclude(b => b.Transaction)
            .ThenInclude(t => t.Wallet)
            .ThenInclude(w => w.ApplicationUser)
            .Where(p=> p.Id == Id && !p.isDeleted)
            .Select(p => new ProductDetailsViewModel
        {
         ProductName   = p.Name,
         ProductOwner = p.User.FirstName + " " + p.User.LastName,
         DaysRemaining = (p.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
         Description = p.Description,
         HighestBid = p.Bids != null && p.Bids.Any(b => b.Transaction != null) ? 
             p.Bids.Where(b => b.Transaction != null).Max(b => b.Transaction.Amount) : 0,
         BidderName = p.Bids != null && p.Bids.Any(b => b.Transaction != null && b.Transaction.Wallet != null && b.Transaction.Wallet.ApplicationUser != null)
             ? p.Bids.Where(b => b.Transaction != null)
                   .OrderByDescending(b => b.Transaction.Amount)
                   .FirstOrDefault().Transaction.Wallet.ApplicationUser.UserName : ""
        }).FirstOrDefaultAsync();
        
        return View(product);
    }
}