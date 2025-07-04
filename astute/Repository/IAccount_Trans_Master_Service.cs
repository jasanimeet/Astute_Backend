using astute.Models;
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
        Task<List<Dictionary<string, object>>> Get_Account_Trans_Master(int account_Trans_Id, int account_Trans_Detail_Id, string trans_Type);
        Task<List<Dictionary<string, object>>> Get_Account_Trans_Master_Purchase(int account_Trans_Id, int account_Trans_Detail_Id, string trans_Type, int? Year_Id);
        Task<(string, int)> Create_Update_Account_Trans_Master(DataTable dataTable, int account_Trans_Id, string trans_Type, string? invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id);
        Task<(string, int)> Create_Update_Account_Trans_Master_Purchase(DataTable dataTable, DataTable dataTable_Terms, DataTable dataTable_Expense, DataTable dataTable_InwardDetail, int account_Trans_Id, string trans_Type, string invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id, string remarks, DateTime? invoice_Date, TimeSpan? invoice_Time, int supplier_Id);
        Task<int> Delete_Account_Trans_Master(int id);
        Task<int> Delete_Account_Trans_Master_Purchase(int id);
        Task<Dictionary<string, object>> Get_Account_Trans_Purchase(int account_Trans_Id, string trans_Type, int? Year_Id);
        Task<List<string>> Check_Inward_Detail_Stock_Id(string Stock_Id);
        Task<string> Create_Stock_Id_Purchase(string CTS, string Shape);
        Task<IList<DropdownModel>> Get_Account_Master_SubGroupWise_Select(string? Type);
        Task<IList<DropdownModel>> Get_Account_Master_TransTypeWise_Select(string? Trans_Type);
        Task<List<Dictionary<string, object>>> Get_Account_Master_Active_Purchase_Select(int Party_Id, int Year_Id);
        Task<int> Create_Update_Cashbook_Account_Trans_Detail(DataTable dataTable, int? id, int? trans_Id, int? process_Id, int? company_Id, int? year_Id, DateTime? trans_Date, TimeSpan? trans_Time,
            int? by_Account, string by_Type, int? to_Account, string to_Type, int? currency_Id, float? ex_Rate, decimal? amount, decimal? amount_in_us, string remarks, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Cashbook_Account_Trans_Select(int? id, int? year_id, int? company_id);
        Task<List<Dictionary<string, object>>> Get_Cashbook_Account_Trans_Detail_Select(int id);
        Task<int> Delete_Cashbook_Account_Trans(int id, int user_Id);
        Task<IList<DropdownModel>> Get_Account_Master_Select(string? group, string? subGroup, int? mainCompany, string? purchaseExpense, string? salesExpense, bool? isParty, int? accountId);
        Task<List<Dictionary<string, object>>> Get_Account_Trans_Detail_Ledger_Select(int? Account_Id, DateTime? fromDate, DateTime? toDate, int? Year_Id);
    }
}
