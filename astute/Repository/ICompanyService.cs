using astute.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICompanyService
    {
        #region Utilities
        Task Insert_Company_Document_Trace(DataTable dataTable);
        Task Insert_Company_Media_Trace(DataTable dataTable);
        Task Insert_Company_Bank_Trace(DataTable dataTable);
        #endregion

        #region Company Master
        Task<(string, int)> AddUpdateCompany(Company_Master company_Master);
        Task<Company_Master> Get_Company_Details_By_Id(int company_Id);
        Task<int> DeleteCompany(int companyId);
        Task<IList<Company_Master>> GetCompany(int companyId);
        Task<int> CompanyChangeStatus(int company_Id, bool status);
        Task<IList<DropdownModel>> Get_Active_Company();
        #endregion

        #region Company Document
        Task<int> InsertCompanyDocument(DataTable dataTable);
        #endregion

        #region Company Media
        Task<int> InsertCompanyMedia(DataTable dataTable);
        #endregion

        #region Company Bank
        Task<int> InsertCompanyBank(DataTable dataTable);
        #endregion
        
    }
}
