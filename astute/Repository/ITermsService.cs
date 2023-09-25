using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ITermsService
    {
        Task<int> InsertTerms(Terms_Master terms_Mas);
        Task<int> UpdateTerms(Terms_Master terms_Mas);
        Task<int> DeleteTerms(int terms_Id);
        Task<IList<Terms_Master>> GetTerms(int terms_Id);
        Task<int> TermsChangeStatus(int terms_Id, bool status);
    }
}
