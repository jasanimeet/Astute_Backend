using astute.Models;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICartService
    {
        Task<int> Insert_Cart_Review_Aproval_Management(Cart_Review_Approval_Management cart_Review_Approval_Management);
    }
}
