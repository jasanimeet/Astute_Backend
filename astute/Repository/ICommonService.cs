using astute.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICommonService
    {
        #region Country Master
        Task<int> InsertCountry(Country_Master country_Mas);
        Task<int> UpdateCountry(Country_Master country_Mas);
        Task<int> DeleteCountry(int countryId);
        Task<IList<Country_Master>> GetCountry(int country_Id, string country, string isd_Code, string short_Code);
        Task<int> CountryChangeStatus(int country_Id, bool status);
        #endregion

        #region State Master
        Task<int> InsertState(State_Master state_Mas);
        Task<int> UpdateState(State_Master state_Mas);
        Task<int> DeleteState(int stateId);
        Task<IList<State_Master>> GetStates(int state_Id, string state, int country_Id);
        Task<int> StateChangeStatus(int state_Id, bool status);
        #endregion

        #region City Master
        Task<int> InsertCity(City_Master city_Mas);
        Task<int> UpdateCity(City_Master city_Mas);
        Task<int> DeleteCity(int cityId);
        Task<IList<City_Master>> GetCity(int cityId, string city, int stateId, int iPgNo, int iPgSize);
        Task<int> CityChangeStatus(int city_Id, bool status);
        #endregion

        #region Error Log
        Task<int> InsertErrorLog(string message, string method, string stackTrace);
        #endregion

        #region Years
        Task<IList<Year_Master>> GetYear(int yearId);
        Task<int> InsertYears(Year_Master year_Mas);
        Task<int> UpdateYears(Year_Master year_Mas);
        Task<int> DeleteYears(int yearId);
        #endregion

        #region Quote Mas
        Task<Quote_Mas_Model> GetRandomQuote();
        Task<IList<Quote_Master>> GetQuote(int quoteId);
        Task<int> InsertQuote(Quote_Master quote_Mas);
        Task<int> UpdateQuote(Quote_Master quote_Mas);
        Task<int> DeleteQuote(int quoteId);
        #endregion

        #region Layout Master
        Task<IList<Layout_Master>> GetLayout(int layoutId, int menuId, int employeeId);
        Task<IList<Layout_Detail>> GetLayout_Details(int layoutDetailId, int layoutId);
        Task<int> InsertLayoutMas(Layout_Master layout_Master);
        Task<int> DeleteLayout(int menuId, int employeeId);
        Task<int> DeleteLayoutDetail(int layout_Detail_Id);
        Task<int> UpdateLayoutStatus(int layoutDetailId, int layoutId);
        Task<Layout_Detail> DefaultLayout();

        //Task<int> UpdateLayoutMas(Layout_Master layout_Master);
        #endregion

        #region Loader Master
        Task<int> InsertLoader(Loader_Master loader_Master);
        Task<int> DeleteLoader(int loader_Id);
        Task<IList<Loader_Master>> GetLoader();
        Task<int> Add_Employee_Loader(Employee_Loader employee_Loader);
        Task<IList<Loader_Master>> Get_Employee_Loader(int employee_Id);
        Task<int> Set_Default_Loader(int employee_Id, int loader_Id, bool default_Loader);
        #endregion
        Task<DataTable> Get_Stock();
    }
}
