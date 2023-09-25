using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IExchange_Rate_Service
    {
        Task<int> Insert_Update_Exchange_Rate(DataTable dataTable);
        Task<IList<Exchange_Rate_Master>> Get_Exchange_Rate(int exchange_Id);
    }
}
