using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IOracleService
    {
        Task<int> Get_Fortune_Purchase_Disc();
        Task<int> Get_Fortune_Sale_Disc();
        Task<int> Get_Fortune_Stock_Disc();
        Task<int> Get_Fortune_Sale_Disc_Kts();
        Task<int> Get_Fortune_Stock_Disc_Kts();
        Task<(int, int, int, int, int)> Get_Fortune_Discount();
        Task<int> Get_Fortune_Party();
        Task<int> Get_Fortune_Party_Master();
    }
}
