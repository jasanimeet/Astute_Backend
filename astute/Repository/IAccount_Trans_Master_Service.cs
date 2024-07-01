using astute.Models;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IAccount_Trans_Master_Service
    {
        Task<IList<DropdownModel>> Get_Account_Master_Active_Select(string? trans_Type, string? rec_Type, int account_Id);
        Task<List<Dictionary<string, object>>> Get_Currency_Master_Exchange_Rate_Select();
        Task<List<Dictionary<string, object>>> Get_Account_Trans_Master(int account_Trans_Id, int account_Trans_Detail_Id,string trans_Type);
        Task<List<Dictionary<string, object>>> Get_Account_Trans_Master_Purchase(int account_Trans_Id, int account_Trans_Detail_Id,string trans_Type, int? Year_Id);
        Task<(string, int)> Create_Update_Account_Trans_Master(DataTable dataTable, int account_Trans_Id, string trans_Type, string? invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id);
        Task<(string, int)> Create_Update_Account_Trans_Master_Purchase(DataTable dataTable, DataTable dataTable_Terms, DataTable dataTable_Expense, int account_Trans_Id, string trans_Type, string invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id, string remarks, DateTime? invoice_Date, TimeSpan? invoice_Time, int supplier_Id);
        Task<int> Delete_Account_Trans_Master(int id);
        Task<int> Delete_Account_Trans_Master_Purchase(int id);
    }
}
