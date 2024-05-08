using astute.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace astute.Repository
{
    public class Account_Master_Service : IAccount_Master_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Account_Master_Service(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        public async Task<int> Create_Update_Account_Master(Account_Master account_Master)
        {
            var account_Id = account_Master.Account_Id >0 ? new SqlParameter("@Account_Id", account_Master.Account_Id) : new SqlParameter("@Account_Id", DBNull.Value);
            var account_Name = !string.IsNullOrEmpty(account_Master.Account_Name) ? new SqlParameter("@Account_Name", account_Master.Account_Name) : new SqlParameter("@Account_Name", DBNull.Value);
            var group = !string.IsNullOrEmpty(account_Master.Group) ? new SqlParameter("@Group", account_Master.Group) : new SqlParameter("@Group", DBNull.Value);
            var sub_Group = !string.IsNullOrEmpty(account_Master.Sub_Group) ? new SqlParameter("@Sub_Group", account_Master.Sub_Group) : new SqlParameter("@Sub_Group", DBNull.Value);
            var main_Company = !string.IsNullOrEmpty(account_Master.Main_Company) ? new SqlParameter("@Main_Company", account_Master.Main_Company) : new SqlParameter("@Main_Company", DBNull.Value);
            var purchase_Expence = !string.IsNullOrEmpty(account_Master.Purchase_Expence) ? new SqlParameter("@Purchase_Expence", account_Master.Purchase_Expence) : new SqlParameter("@Purchase_Expence", DBNull.Value);
            var sales_Expence = !string.IsNullOrEmpty(account_Master.Sales_Expence) ? new SqlParameter("@Sales_Expence", account_Master.Sales_Expence) : new SqlParameter("@Sales_Expence", DBNull.Value);
            var user_Id = account_Master.User_Id > 0  ? new SqlParameter("@User_Id", account_Master.User_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Account_Master_Insert_Update @Account_Id, @Account_Name, @Group, @Sub_Group, @Main_Company, @Purchase_Expence, @Sales_Expence, @User_Id",
                            account_Id, account_Name, group, sub_Group, main_Company, purchase_Expence, sales_Expence, user_Id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Account_Master(int account_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(account_Id > 0 ? new SqlParameter("@Account_Id", account_Id) : new SqlParameter("@Account_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Delete_Account_Master(int account_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Account_Master_Delete {account_Id}"));
        }
    }
}
