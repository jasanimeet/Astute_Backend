using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IOracleService
    {
        Task<int> Get_Pur_Disc();
        Task<int> Get_Sal_Disc();
        Task<int> Get_Stock_Kts();
        Task<(int, int, int)> Get_Fortune_Discount();
        Task<int> Get_Fortune_Party();
        Task<int> Get_Fortune_Party_Master();
    }
}
