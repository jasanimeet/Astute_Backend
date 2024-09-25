using astute.Models;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IEmpRightsService
    {
        Task<int> InsertUpdateEmpRights(Emp_rights_Model emp_Rights_Model);
        Task<int> Copy_Emp_Rights(int fromEmpId, int toEmpId);
    }
}
