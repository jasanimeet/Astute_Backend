using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICurrencyService
    {
        Task<int> InsertCurrency(Currency_Master currency_Mas);
        Task<int> UpdateCurrency(Currency_Master currency_Mas);
        Task<int> DeleteCurrency(int currency_Id);
        Task<IList<Currency_Master>> GetCurrency(int currency_Id);
        Task<int> CurrencyChangeStatus(int currency_Id, bool status);
    }
}
