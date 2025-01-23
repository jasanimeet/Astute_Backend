using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface  ILab_User_Login_Activity_Services
    {
        Task<List<Dictionary<string, object>>> Get_Lab_User_Login_Activity(string? From_Date, string? To_Date, string? Common_Search);
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_User_Activity(string? From_Date, string? To_Date, string? Common_Search);
        Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Lab_User_Activity(string? From_Date, string? To_Date);
    }
}
