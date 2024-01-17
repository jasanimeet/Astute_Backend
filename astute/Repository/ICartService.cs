using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICartService
    {
        Task<int> Insert_Cart(DataTable dataTable, int User_Id);
        Task<List<Dictionary<string, object>>> Get_Cart(string userIds);
        Task<int> Delete_Cart(string ids, int user_Id);
        Task<int> Approved_Or_Rejected_by_Management(string ids, bool is_Approved, bool is_Rejected, int user_Id);
    }
}
