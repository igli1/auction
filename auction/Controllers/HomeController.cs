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

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuctionDbContext _context;

    public HomeController(ILogger<HomeController> logger, AuctionDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var products = await _context.Product
            .Include(n => n.Seller)
            .Include(p => p.ProductBids)
            .Where(p => !p.isDeleted && p.EndDate >= DateTime.UtcNow)
            .Select(p => new AuctionViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                SellerName = p.Seller.UserName,
                TimeRemaining = (p.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
                IsCurrentUserProductOwner = p.SellerId == userId
            })
            .OrderBy(p => p.TimeRemaining)
            .ToListAsync();
        
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
        
        var auction = new AuctionIndexViewModel
        {
            Auctions = products,
            WalletValue = wallet?.Balance ?? 0
        };
        
        return View(auction);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult NewAuction()
    {
        return View();
    }
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
            SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Description = model.Description,
            EndDate = model.EndDate
        };
        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> ProductDetails(int Id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
        var product = await _context.Product
            .Include(p => p.Seller)
            .Include(p => p.ProductBids)
            .Where(p=> p.Id == Id && !p.isDeleted)
            .Select(p => new ProductDetailsViewModel
        {
            UserId = userId,
            UserBalance = wallet.Balance,
            ProductId = p.Id,
            ProductName   = p.Name,
            IsCurrentUserProductOwner = p.Seller.Id == userId,
            ProductOwner = p.Seller.UserName,
            DaysRemaining = (p.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
            Description = p.Description,
            StartingPrice = p.StartingPrice,
        }).FirstOrDefaultAsync();
        
        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> NewBid(BidViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("ProductDetails", new { id = model.ProductId });
        }
        
        var wallet = await _context.Wallet.
            Include(w => w.User)
            .ThenInclude(u => u.UserBids)
            .FirstOrDefaultAsync(w => w.UserId == model.Userid);
        
        var productValid = await _context.Product.AnyAsync(p => p.Id == model.ProductId && !p.isDeleted);
        
        var activeBidsTotal = wallet.User.UserBids?.Where(b => b.IsWinning == false).Sum(b => b.Amount);
        var availableamount = wallet.Balance - activeBidsTotal;
        
        if (wallet == null || !productValid || (availableamount < model.BidAmount))
        {
            return BadRequest();
        }
        
        var bid = new Bid
        {
            BidderId = model.Userid,
            ProductId = model.ProductId,
        };

        await _context.Bid.AddAsync(bid);
        _context.Wallet.Update(wallet);

        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}