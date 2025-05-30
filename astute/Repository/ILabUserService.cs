﻿using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ILabUserService
    {
        Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_User(int id, int party_Id, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Lab_User_Company(int user_Id);
        Task<int> Change_Active_Status(int id, bool active_Status);
        Task<Dictionary<string, object>> Get_Suspend_Day();
        Task<(int, string)> Delete_Lab_User(int id, bool check_Primary_User);
        Task<int> Create_Update_Suspend_Days(int id, int days);
        Task<List<Dictionary<string, object>>> Get_Customer_Lab_User(int party_Id);
        Task<int> Job_Transfer_User_Pricing();
        Task<int> Job_Transfer_Supplier_Pricing_Cal(int party_Id);
        Task<int> Job_Transfer_Auto_Supplier_Stock();
        Task<int> Job_Transfer_Auto_Category_Value();
        Task<int> Job_Transfer_Auto_Stock_Pricing(string? Upload_From);
        Task<int> Job_Transfer_Auto_Party_Url_Format();
        Task<int> Job_Transfer_Auto_Party_Account_Master();
    }
}
