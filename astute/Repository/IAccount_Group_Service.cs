using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IAccount_Group_Service
    {
        Task<(string, int)> Create_Update_Account_Group(Account_Group_Master account_Group_Master);
        Task<List<Dictionary<string, object>>> Get_Account_Group(int ac_Group_Code);
        Task<int> Delete_Account_Group(int ac_Group_Code);
        Task<DataTable> Get_Account_Group_Excel();
        Task<IList<DropdownModel>> Get_Perent_Account_Group();
        Task<IList<DropdownModel>> Get_Sub_Account_Group(int Id);
    }
}
