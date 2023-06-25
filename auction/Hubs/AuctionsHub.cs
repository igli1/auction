using auction.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace auction.Hubs;

public class AuctionsHub : Hub
{
    public async Task UpdateAuctions(List<AuctionViewModel> updatedAuctions)
    {
        await Clients.All.SendAsync("ReceiveUpdatedAuctions", updatedAuctions);
    }
}