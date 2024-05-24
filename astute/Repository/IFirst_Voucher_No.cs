using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IFirst_Voucher_No
    {
        Task<List<Dictionary<string, object>>> Get_First_Voucher_No_Master(int Id);
        Task<(string, bool, bool, int)> Create_Update_First_Voucher_No_Master(DataTable dataTable, int? user_Id);
        Task<int> Delete_First_Voucher_No_Master(string Id);
    }
}
