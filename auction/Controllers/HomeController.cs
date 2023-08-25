using System.Diagnostics;
using System.Security.Claims;
using auction.Hubs;
using Microsoft.AspNetCore.Mvc;
using auction.Models;
using auction.Models.Database;
using auction.Models.Database.Entity;
using auction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace auction.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuctionDbContext _context;
    private readonly IHubContext<AuctionsHub> _auctionHubContext;

    public HomeController(ILogger<HomeController> logger, 
        AuctionDbContext context,
        IHubContext<AuctionsHub> auctionHubContext)
    {
        _logger = logger;
        _context = context;
        _auctionHubContext = auctionHubContext;
    }
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await Auctions(userId);
        
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);

        var bids = await _context.Bid
            .Where(b => b.BidderId == userId && _context.Bid
                .Where(b2 => b2.ProductId == b.ProductId && b2.Product.isDeleted == false)
                .Max(b2 => b2.Amount) == b.Amount)
            .ToListAsync();
        decimal onHold = 0;

        foreach (var bid in bids)
        {
            onHold += bid.Amount;
        }
        
        var auction = new AuctionIndexViewModel
        {
            Auctions = products,
            WalletValue = wallet?.Balance - onHold ?? 0
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

        if (model.EndDate < DateTime.Today.AddDays(1))
        {
            ModelState.AddModelError("EndDate", "Minimum End Date should be a day after today.");
            return View(model);
        }
        
        if (ModelState.IsValid)
        {
            model.EndDate = new DateTime(model.EndDate.Year, model.EndDate.Month, model.EndDate.Day, 12, 0, 0);
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
        
        await UpdateAuctions();
        
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> ProductDetails(int Id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
        
        var bids = await _context.Bid
            .Where(b => b.BidderId == userId && _context.Bid
                .Where(b2 => b2.ProductId == b.ProductId)
                .Max(b2 => b2.Amount) == b.Amount)
            .ToListAsync();
        decimal onHold = 0;

        foreach (var bid in bids)
        {
            onHold += bid.Amount;
        }
        
        var product = await _context.Product
            .Include(p => p.Seller)
            .Include(p => p.ProductBids)
            .ThenInclude(b => b.Bidder)
            .Where(p=> p.Id == Id && !p.isDeleted)
            .Select(p => new 
            {
                Product = p,
                HighestBid = p.ProductBids.OrderByDescending(b => b.Amount).FirstOrDefault()
            })
            .Select(x => new ProductDetailsViewModel
            {
                UserId = userId,
                UserBalance = wallet.Balance - onHold,
                ProductId = x.Product.Id,
                ProductName = x.Product.Name,
                IsCurrentUserProductOwner = x.Product.Seller.Id == userId,
                ProductOwner = x.Product.Seller.UserName,
                DaysRemaining = (x.Product.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
                Description = x.Product.Description,
                StartingPrice = x.Product.StartingPrice,
                IsCurrentUserHighestBidder = x.HighestBid !=null ? x.HighestBid.Bidder.Id == userId : false,
                HighestBid = x.HighestBid != null ? x.HighestBid.Amount : 0,
                BidderName = x.HighestBid != null ? x.HighestBid.Bidder.UserName : ""
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
        
        var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == model.UserId);

        var bids = await _context.Bid
            .Where(b => b.BidderId == model.UserId && _context.Bid
                .Where(b2 => b2.ProductId == b.ProductId && b2.Product.isDeleted == false && b2.Transaction == null)
                .Max(b2 => b2.Amount) == b.Amount)
            .ToListAsync();
        decimal onHold = 0;

        foreach (var bid in bids)
        {
            onHold += bid.Amount;
        }
        
        var productValid = await _context.Product.AnyAsync(p => p.Id == model.ProductId && !p.isDeleted);
        
        var availableamount = wallet.Balance - onHold;
        
        if (wallet == null || !productValid || (availableamount < model.BidAmount))
        {
            return BadRequest();
        }
        
        var prductBid = new Bid
        {
            BidderId = model.UserId,
            ProductId = model.ProductId,
            Amount = model.BidAmount
        };

        await _context.Bid.AddAsync(prductBid);

        await _context.SaveChangesAsync();
        
        await UpdateAuctions();
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> DeleteProduct(int Id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == Id && p.SellerId == userId);
        
        if (product == null)
        {
            return NotFound();
        }

        product.isDeleted = true;
        
        _context.Product.Update(product);
        await _context.SaveChangesAsync();
        
        await UpdateAuctions();
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateAuctions()
    {
        await _auctionHubContext.Clients.All.SendAsync("ReceiveUpdatedAuctions");
        return Ok();
    }
    [HttpGet]
    public async Task<JsonResult> GetAuctions()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var products = await Auctions(userId);
        return new JsonResult(products);
    }
    public async Task<List<AuctionViewModel>> Auctions(string userId)
    {
        var products = await _context.Product
            .Include(n => n.Seller)
            .Include(p => p.ProductBids)
            .Where(p => !p.isDeleted && p.EndDate >= DateTime.UtcNow)
            .OrderBy(p => p.EndDate)
            .Select(p => new AuctionViewModel
            {
                ProductId = p.Id,
                ProductName = p.Name,
                SellerName = p.Seller.UserName,
                TimeRemaining = (p.EndDate - DateTime.UtcNow).TotalDays.ToString("0"),
                IsCurrentUserProductOwner = p.SellerId == userId,
                TopBid = p.ProductBids.Max(b => (decimal?)b.Amount) ?? 0
            })
            .ToListAsync();
        
        return products;
    }
}