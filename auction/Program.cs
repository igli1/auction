using auction.Configuration;
using auction.Helpers;
using auction.Hubs;
using auction.Models.Database;
using auction.Models.Database.Entity;
using auction.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog(builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<AuctionDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password settings.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = false;

        options.User.AllowedUserNameCharacters =
            UserNameHelper.AllowedUserNameCharacters;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<AuctionDbContext>()
    .AddUserValidator<CustomUserValidator>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/RegisterAndLogin";
    options.AccessDeniedPath = "/Home/Index";
});

builder.Services.AddSignalR();

builder.Services.AddHostedService<DailyTaskService>();
builder.Services.Configure<MinioConfiguration>(builder.Configuration.GetSection(MinioConfiguration.SettingsSection));
builder.Services.AddSingleton<ObjectStorageService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

 // Fallback
app.MapFallbackToController("User/{*wildcard}", "Profile", "User");
app.MapFallbackToController("{*wildcard}", "Index","Home");

app.MapHub<AuctionsHub>("/auctionsHub");

app.Run();