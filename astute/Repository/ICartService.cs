using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICartService
    {
        Task<(string, int, string)> Create_Update_Cart(DataTable dataTable, int Id, int User_Id, string customer_Name, string remarks, int validity_Days);
        Task<(string, int, string)> Delete_Cart(string ids, int user_Id);
        Task<int> Approved_Or_Rejected_by_Management(Approval_Management approval_Management);
        Task<int> Approved_Management_Update_Status(Approval_Management_Status approval_Management);
        //Task<int> Create_Approved_Management(Approval_Management_Create_Update approval_Management);
        Task<(string, int, string)> Create_Approved_Management(DataTable dataTable, int Id, int user_Id, string remarks, string status);
        Task<(string, int, string)> Create_Update_Order_Processing(DataTable dataTable, int Id, int? user_Id, string customer_Name, string remarks, string status);
        Task<int> Order_Processing_Inactive(Order_Processing_Inactive order_processing);
        Task<List<Dictionary<string, object>>> Get_Order_Summary(int order_no);
        Task<int> Order_Processing_Update_Order_Status(string order_Status, string order_No);
    }
}
