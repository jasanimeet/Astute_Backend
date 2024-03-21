using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ILabUserService
    {
        Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id);
    }
}
