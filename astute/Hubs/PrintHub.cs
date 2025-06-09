using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Hubs
{
    public class PrintHub : Hub<IPrintHub>
    {
        public async Task SendPrintCommand(int clientId, Dictionary<string, object> result)
        {
            await Clients.All.SendPrintCommand(clientId, result);
        }
    }
}