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
    public ObjectStorageController(ILogger<HomeController> logger, ObjectStorageService  minio,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _minio = minio;
        _env = env;
    }
    [HttpGet]
    public async Task<IActionResult> GetImage(string imageName, bool isProfile = false)
    {
        try
        {
            var stream = await _minio.GetFileAsync(imageName);
            var file = File(stream, "image/*");
            if (file == null)
            {
                if(isProfile)
                    return ReturnDefaultImage(DefaultProfile);
                
                return ReturnDefaultImage(DefaultProduct);
            }
                
            return file;
        }
        catch (Exception ex)
        {
            return ReturnDefaultImage(DefaultProduct);
        }
    }
    
    private IActionResult ReturnDefaultImage(string imageName)
    {
        var path = _env.WebRootPath ;
        var imagePath = Path.Combine(path, "Image", imageName);
        return PhysicalFile(imagePath, "image/*");
    }
}