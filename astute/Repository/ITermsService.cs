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
        Task<IList<Terms_Master>> Get_Active_Terms(int terms_Id);
        Task<int> TermsChangeStatus(int terms_Id, bool status);
        Task<int> Insert_Terms_Trans_Det(Terms_Trans_Det terms_Trans_Det);
        Task<int> Update_Terms_Trans_Det(Terms_Trans_Det terms_Trans_Det);
        Task<int> Delete_Terms_Trans_Det(int terms_Id);
    }
}
