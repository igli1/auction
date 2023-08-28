using System.Security.Claims;
using auction.Models.Database;
using auction.Models.Database.Entity;
using auction.Models.ViewModels;
using auction.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auction.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AuctionDbContext _context;
    private readonly ObjectStorageService  _minio;
    private readonly IWebHostEnvironment _env;
    public UserController(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        AuctionDbContext context,
    ObjectStorageService minio,
        IWebHostEnvironment env)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _minio = minio;
        _env = env;
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
        
        var applicationUser = new ApplicationUser() { 
            UserName = model.UserName, 
            FirstName = model.FirstName.ToLower(), 
            LastName = model.LastName.ToLower(),
            Email = model.Email.ToLower(),
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
    [HttpPost]
    public async Task<IActionResult> Profile([FromForm] UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }
        
        
        string imageName = "";
        bool insertion = false;
        if (model.Image != null)
        {
            try
            {
                imageName = Guid.NewGuid().ToString("N") + model.Image.FileName;
                insertion =await _minio.UploadFileAsync(imageName,model.Image.OpenReadStream() );
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            } 
        }

        if (insertion)
        {
            user.ProfilePicture = imageName;
            _context.Update(user);
            await _context.SaveChangesAsync();
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

        var response = _context.SoldItem
            .Include(si =>si.Transaction)
            .Where(si =>si.SellerId == userId || si.Buyerid == userId)
            .GroupBy(si => 1)
            .Select(g => new WalletViewModel
            {
                ProductsSold = g.Where(si => si.SellerId == userId).Select(si => new ProductsViewModel
                {
                    Id = si.Product.Id,
                    Name = si.Product.Name,
                    Price = si.Transaction.Bid.Amount,
                    Date = si.Transaction.TransactionTime
                }).ToList(),
                ProductsBought = g.Where(si => si.Buyerid == userId).Select(si => new ProductsViewModel
                {
                    Id = si.Product.Id,
                    Name = si.Product.Name,
                    Price = si.Transaction.Bid.Amount,
                    Date = si.Transaction.TransactionTime
                }).ToList()
            }).FirstOrDefault();

        var productsOnSale = await _context.Product
            .Include(p => p.SoldItem)
            .Include(p =>p.ProductBids)
            .Where(p => p.SellerId == userId && p.SoldItem == null && p.isDeleted == false).Select(p => new ProductsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Date = p.EndDate,
                Price = p.ProductBids.Any() ? p.ProductBids.Max(b => b.Amount) : p.StartingPrice,
                NumberOfBids = p.ProductBids.Count
            }).ToListAsync();

        
        
        if (response == null)
        {
            response = new WalletViewModel
            {
                ProductsSold = new List<ProductsViewModel>(),
                ProductsBought = new List<ProductsViewModel>()
            };
        }
        
        if (productsOnSale == null)
        {
            productsOnSale = new List<ProductsViewModel>();
        }
        
        response.ProductsOnSale = productsOnSale;
        response.WalletTotalValue = wallet.Balance;
        response.OnHold = onHold;
        
        return View(response);
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
    
    [HttpGet]
    public async Task<IActionResult> GetImage(string imageName)
    {
        try
        {
            var stream = await _minio.GetFileAsync(imageName);
            var file = File(stream, "image/*"); 
            if(file == null)
                return ReturnDefaultImage();
            return file;
        }
        catch (Exception ex)
        {
            return ReturnDefaultImage();
        }
    }
    private IActionResult ReturnDefaultImage()
    {
        var path = _env.WebRootPath ;
        var imagePath = Path.Combine(path, "Image", "Profile.webp");
        return PhysicalFile(imagePath, "image/webp");
    }
}