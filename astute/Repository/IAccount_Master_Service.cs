using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IAccount_Master_Service
    {
        Task<int> Create_Update_Account_Master(Account_Master account_Master);
        Task<List<Dictionary<string, object>>> Get_Account_Master(int account_Id);
        Task<int> Delete_Account_Master(int account_Id);
    }
}
