using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Hubs
{
    public interface IPrintHub
    {
        Task SendPrintCommand(int clientId, Dictionary<string, object> result);
    }
}