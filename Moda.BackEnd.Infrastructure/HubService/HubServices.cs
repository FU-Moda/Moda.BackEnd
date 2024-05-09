using Microsoft.AspNetCore.SignalR;
using Moda.BackEnd.Application.IHubService;
using Moda.BackEnd.Infrastructure.ServerHub;

namespace Moda.BackEnd.Infrastructure.HubService;

public class HubServices : IHubServices
{
    private readonly IHubContext<NotificationHub> _signalRHub;
    public HubServices(IHubContext<NotificationHub> signalRHub)
    {
        _signalRHub = signalRHub;
    }
    public async Task SendAsync(string method)
    {
        await _signalRHub.Clients.All.SendAsync(method);
    }
}