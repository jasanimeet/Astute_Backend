using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IRegistrationService
    {
        Task<List<Dictionary<string, object>>> Get_Registration_Master(int id, int user_Id);
        Task<int> Change_Active_Status(int id, bool active_Status);
    }
}
