using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IBankService
    {
        Task<int> InsertBank(Bank_Master bank_Mas);
        Task<int> UpdateBank(Bank_Master bank_Mas);
        Task<int> DeleteBank(int bankId);
        Task<IList<Bank_Master>> GetBank(int bankId);
        Task<IList<Bank_Master>> Get_Active_Bank(int bankId, string bank_Name);
        Task<int> BankChangeStatus(int bank_Id, bool status);
        Task<IList<Bank_Dropdown_Model>> Get_Bank_Distinct();
        Task<List<Dictionary<string, object>>> Get_Bank_Branch(string bank_Name);
    }
}
