using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public class Account_Trans_Master_Service : IAccount_Trans_Master_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Account_Trans_Master_Service(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods

        public async Task<IList<DropdownModel>> Get_Account_Master_Active_Select(string? trans_Type, string? rec_Type, int account_Id)
        {
            var _trans_Type = !string.IsNullOrEmpty(trans_Type) ? new SqlParameter("@Trans_Type", trans_Type) : new SqlParameter("@Trans_Type", DBNull.Value);
            var _rec_Type = !string.IsNullOrEmpty(rec_Type) ? new SqlParameter("@Rec_Type", rec_Type) : new SqlParameter("@Rec_Type", DBNull.Value);
            var _account_Id = account_Id > 0 ? new SqlParameter("@Account_Id", account_Id) : new SqlParameter("@Account_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Account_Master_Active_Select @Trans_Type,@Rec_Type,@Account_Id", _trans_Type, _rec_Type, _account_Id).ToListAsync());

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Currency_Master_Exchange_Rate_Select()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Currency_Master_Exchange_Rate_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

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
        public async Task<List<Dictionary<string, object>>> Get_Account_Trans_Master(int account_Trans_Id,int account_Trans_Detail_Id,string trans_Type, int? Year_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Trans_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(account_Trans_Id > 0 ? new SqlParameter("@Account_Trans_Id", account_Trans_Id) : new SqlParameter("@Account_Trans_Id", DBNull.Value));
                    command.Parameters.Add(account_Trans_Detail_Id > 0 ? new SqlParameter("@Account_Trans_Detail_Id", account_Trans_Detail_Id) : new SqlParameter("@Account_Trans_Detail_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(trans_Type) ? new SqlParameter("@Trans_Type", trans_Type) : new SqlParameter("@Trans_Type", DBNull.Value));
                    command.Parameters.Add(Year_Id > 0 ? new SqlParameter("@Year_Id", Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));

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
        public async Task<(string,int)> Create_Update_Account_Trans_Master(DataTable dataTable, int account_Trans_Id, string trans_Type, string? invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id, string remarks)
        {
            var parameter = new SqlParameter("@Account_Trans_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Account_Trans_Detail_Table_Type",
                Value = dataTable
            };

            var _account_Trans_Id = new SqlParameter("@Account_Trans_Id", account_Trans_Id);
            var _trans_Type = new SqlParameter("@Trans_Type", !string.IsNullOrEmpty(trans_Type) ? trans_Type : DBNull.Value);
            var _invoice_No = new SqlParameter("@Invoice_No", !string.IsNullOrEmpty(invoice_No) ? invoice_No : DBNull.Value);
            var _currency_Id = new SqlParameter("@Currency_Id", currency_Id);
            var _company_Id = new SqlParameter("@Company_Id", company_Id);
            var _year_Id = new SqlParameter("@Year_Id", year_Id);
            var _account_Id = new SqlParameter("@Account_Id", account_Id);
            var _rate = new SqlParameter("@Rate", rate);
            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var _remarks = new SqlParameter("@Remarks", !string.IsNullOrEmpty(remarks) ? remarks : DBNull.Value);

            var is_First_Voucher_Add = new SqlParameter("@Is_First_Voucher_Add", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };


            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [dbo].[Account_Trans_Master_Insert_Update] @Account_Trans_Detail_Table_Type, @Account_Trans_Id, @Trans_Type, @Invoice_No, @Currency_Id, @Company_Id, @Year_Id, @Account_Id, @Rate, @User_Id, @Remarks, @Is_First_Voucher_Add OUT",
                        parameter, _account_Trans_Id, _trans_Type, _invoice_No,_currency_Id,_company_Id,_year_Id,_account_Id,_rate,_user_Id, _remarks, is_First_Voucher_Add));


            var _is_Exists = (bool)is_First_Voucher_Add.Value;
            if (_is_Exists == false)
                return ("not_exists", 409);

            return ("success", result);

        }
        public async Task<int> Delete_Account_Trans_Master(int Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Account_Trans_Master_Delete {Id}"));
        }
        #endregion
    }
}
