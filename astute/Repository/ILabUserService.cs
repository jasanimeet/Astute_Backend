using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ILabUserService
    {
        Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_User(int id, int party_Id, int user_Id);
        Task<int> Change_Active_Status(int id, bool active_Status);
        Task<Dictionary<string, object>> Get_Suspend_Day();
        Task<int> Delete_Lab_User(int id);
        Task<int> Create_Update_Suspend_Days(int id, int days);
    }
}
