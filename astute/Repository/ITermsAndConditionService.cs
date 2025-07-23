using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ITermsAndConditionService
    {
        Task<int> AddUpdateTermsAndCondition(TermsAndCondition termsAndCondition);
        Task<int> DeleteTermsAndCondition(int condition_Id);
        Task<IList<TermsAndCondition>> GetTermsAndCondition(int condition_Id, int process_Id);
        Task<int> TermsAndConditionChangeStatus(int condition_Id, bool status);
        Task<int> Get_TermsAndCondition_Max_Order_No();
        Task<List<Dictionary<string, object>>> Get_TermsAndCondition_By_Process(int process_Id,string trans_Date);
    }
}
