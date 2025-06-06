using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Hubs
{
    public class PrintHub : Hub<IPrintHub>
    {
        public async Task HelloCommand(string message)
        {
            await Clients.All.HelloCommand(message);
        }
        public async Task SendPrintCommand(string clientId, Dictionary<string, object> result)
        {
            await Clients.All.SendPrintCommand(clientId, result);
            //await Clients.All.SendOffersToUser(message);
        }
    }
}