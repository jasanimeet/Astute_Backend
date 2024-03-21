using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ILabUserService
    {
        Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_User(int id, int party_Id);
    }
}
