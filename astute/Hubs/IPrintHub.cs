using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Hubs {
    public interface IPrintHub
    {
        Task HelloCommand(string message);
        Task SendPrintCommand(string clientId, Dictionary<string, object> result);
    }
}