using Microsoft.AspNetCore.SignalR;

namespace SignalRServer
{
    public class ClientHub : Hub
    {
        public async Task SendUpdate()
        {
            await Clients.All.SendAsync("ReceiveUpdate");
        }
    }
}
