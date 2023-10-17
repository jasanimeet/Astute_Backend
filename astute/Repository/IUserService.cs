using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IUserService
    {
        Task<(string, int)> Add_Update_User(User_Registration user_Registration);
        Task<List<Dictionary<string, object>>> Get_User(int user_Id);
    }
}
