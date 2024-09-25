using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ITransactionService
    {
        Task<int> Create_Update_Transaction_Details(DataTable dataTable, int? Trans_Id, int Party_Code, int Due_Days, string Process, string Remarks);
        Task<List<string>> Check_Transaction_Detail_Stock_Id(string Stock_Id);
        Task<List<Dictionary<string, object>>> Get_Transaction_Details(DataTable dataTable, string Stock_id, string Id, string Sign, string value);
        Task<List<Dictionary<string, object>>> Get_Transaction_Detail();
    }
}
