using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ITrans_Service
    {
        Task<List<Dictionary<string, object>>> Get_Trans_Master(int Id);
        Task<(string, bool, bool, int)> Create_Update_Trans_Master(DataTable dataTable, int? user_Id);
        Task<int> Delete_Trans_Master(string Id);
    }
}
