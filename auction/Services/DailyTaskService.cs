using auction.Models.Database;
using auction.Models.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace auction.Services;

public class DailyTaskService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DailyTaskService> _logger;
    
    public DailyTaskService(IServiceScopeFactory scopeFactory, ILogger<DailyTaskService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await MyTask();
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(1);
            var delay = nextRun - now;
            
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, stoppingToken);
                await MyTask();
            }
        }
    }

    private async Task MyTask()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            try
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<AuctionDbContext>();

                var products = await _context.Product
                    .Include(p => p.Seller)
                    .ThenInclude(b => b.Wallet)
                    .Include(p => p.ProductBids)
                    .ThenInclude(pb => pb.Bidder)
                    .ThenInclude(b => b.Wallet)
                    .Where(p => !p.isDeleted &&
                                p.EndDate <= DateTime.UtcNow &&
                                p.ProductBids.Any() &&
                                p.SoldItem == null)
                    .ToListAsync();

                var soldItems = new List<SoldItem>();
                var bidders = new List<Wallet>();
                var sellers = new List<Wallet>();

                foreach (var product in products)
                {
                    var highestBid = product.ProductBids.OrderByDescending(b => b.Amount).FirstOrDefault();

                    var soldItem = new SoldItem
                    {
                        SellerId = product.SellerId,
                        Buyerid = highestBid.BidderId,
                        BidId = highestBid.Id,
                        Amount = highestBid.Amount,
                        ProductId = product.Id
                    };
                    soldItems.Add(soldItem);

                    var biddersWallet = highestBid.Bidder.Wallet;
                    biddersWallet.Balance -= highestBid.Amount;
                    bidders.Add(biddersWallet);

                    var sellersWallet = product.Seller.Wallet;
                    sellersWallet.Balance += highestBid.Amount;
                    sellers.Add(sellersWallet);

                }

                await _context.AddRangeAsync(soldItems);
                _context.UpdateRange(bidders);
                _context.UpdateRange(sellers);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing Daily Task.");
            }
        }
    }
}