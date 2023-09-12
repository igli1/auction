using System.Security.Claims;
using auction.Models.Database;
using auction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auction.Controllers;

[Authorize]
public class ObjectStorageController : Controller
{
    private static string DefaultProduct = "product.jpg";
    private static string DefaultProfile = "Profile.webp";
    
    private readonly ObjectStorageService  _minio;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HomeController> _logger;
    private readonly AuctionDbContext _context;
    public ObjectStorageController(ILogger<HomeController> logger, ObjectStorageService  minio,
        IWebHostEnvironment env, AuctionDbContext context)
    {
        _logger = logger;
        _minio = minio;
        _env = env;
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetImage(string imageName, bool isProfile = false)
    {

        if (isProfile)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(i => i.Id == userId);
            imageName = user.ProfilePicture;
        }
        else if(imageName == null)
            imageName = String.Empty;
        
        var stream = await _minio.GetFileAsync(imageName, isProfile);
        var file = File(stream, "image/*");
                
        return file;

    }
}