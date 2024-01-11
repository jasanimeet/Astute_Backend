using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICartService
    {
        Task<int> Insert_Cart_Review_Aproval_Management(Cart_Review_Approval_Management cart_Review_Approval_Management);
        Task<List<Dictionary<string, object>>> Get_Cart_Review_Approval_Management(string upload_Type, string userIds);
        Task<int> Delete_Cart_Review_Aproval_Management(string ids, int user_Id, string upload_Type);
    }
}
