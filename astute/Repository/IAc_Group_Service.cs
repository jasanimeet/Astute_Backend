using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IAc_Group_Service
    {
        Task<(string, int)> Add_Update_Ac_Group(Ac_Group_Master ac_Group_Master);
        Task<IList<Ac_Group_Master>> Get_Ac_Group(int ac_Group_Id);
        Task<IList<Ac_Group_Master>> Get_Active_Ac_Group(int ac_Group_Id);
        Task<int> Add_Update_Ac_Group_Detail(DataTable dataTable);
        Task<Ac_Group_Master> Get_Ac_Group_Detail(int ac_Group_Id);
        Task<int> Delete_Ac_Group(int ac_Group_Id);
        Task Insert_Ac_Group_Detail_Trace(DataTable dataTable);
    }
}
