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
        public async Task<List<Dictionary<string, object>>> Get_Account_Trans_Master(int account_Trans_Id, int account_Trans_Detail_Id, string trans_Type)
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

        public async Task<(string, int)> Create_Update_Account_Trans_Master(DataTable dataTable, int account_Trans_Id, string trans_Type, string? invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id)
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

            var is_First_Voucher_Add = new SqlParameter("@Is_First_Voucher_Add", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };


            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [dbo].[Account_Trans_Master_Insert_Update] @Account_Trans_Detail_Table_Type, @Account_Trans_Id, @Trans_Type, @Invoice_No, @Currency_Id, @Company_Id, @Year_Id, @Account_Id, @Rate, @User_Id, @Is_First_Voucher_Add OUT",
                        parameter, _account_Trans_Id, _trans_Type, _invoice_No, _currency_Id, _company_Id, _year_Id, _account_Id, _rate, _user_Id, is_First_Voucher_Add));


            var _is_Exists = (bool)is_First_Voucher_Add.Value;
            if (_is_Exists == false)
                return ("not_exists", 409);

            return ("success", result);

        }

        public async Task<int> Delete_Account_Trans_Master(int Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Account_Trans_Master_Delete {Id}"));
        }

        public async Task<(string, int)> Create_Update_Account_Trans_Master_Purchase(DataTable dataTable, DataTable dataTable_Terms, DataTable dataTable_Expense, DataTable dataTable_InwardDetail, int account_Trans_Id, string trans_Type, string invoice_No, int currency_Id, int company_Id, int year_Id, int account_Id, decimal rate, int user_Id, string remarks, DateTime? invoice_Date, TimeSpan? invoice_Time, int supplier_Id)
        {
            var parameter = new SqlParameter("@Account_Trans_Detail_Table_Type_Purchase", SqlDbType.Structured)
            {
                TypeName = "dbo.Account_Trans_Detail_Table_Type_Purchase",
                Value = dataTable
            };

            var parameter_Terms = new SqlParameter("@Terms_Trans_Det_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Terms_Trans_Det_Table_Type",
                Value = dataTable_Terms
            };

            var parameter_Expense = new SqlParameter("@Expense_Trans_Det_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Expense_Trans_Det_Table_Type",
                Value = dataTable_Expense
            };

            var parameter_Inward_Detail = new SqlParameter("@Inward_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Inward_Detail_Table_Type",
                Value = dataTable_InwardDetail
            };

            var _account_Trans_Id = new SqlParameter("@Account_Trans_Id", account_Trans_Id);
            var _trans_Type = new SqlParameter("@Trans_Type", string.IsNullOrEmpty(trans_Type) ? (object)DBNull.Value : trans_Type);
            var _invoice_No = new SqlParameter("@Invoice_No", string.IsNullOrEmpty(invoice_No) ? (object)DBNull.Value : invoice_No);
            var _currency_Id = new SqlParameter("@Currency_Id", currency_Id);
            var _company_Id = new SqlParameter("@Company_Id", company_Id);
            var _year_Id = new SqlParameter("@Year_Id", year_Id);
            var _account_Id = new SqlParameter("@Account_Id", account_Id);
            var _rate = new SqlParameter("@Rate", rate);
            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var _remarks = new SqlParameter("@Remarks", string.IsNullOrEmpty(remarks) ? (object)DBNull.Value : remarks);
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id > 0 ? supplier_Id : (object)DBNull.Value);

            var _invoice_Date = new SqlParameter("@Invoice_Date", SqlDbType.Date)
            {
                Value = invoice_Date.HasValue ? (object)invoice_Date.Value.Date : DBNull.Value
            };

            var _invoice_Time = new SqlParameter("@Invoice_Time", SqlDbType.Time)
            {
                Value = invoice_Time.HasValue ? (object)invoice_Time : DBNull.Value
            };

            var is_First_Voucher_Add = new SqlParameter("@Is_First_Voucher_Add", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC [dbo].[Account_Trans_Master_Purchase_Insert_Update] 
                    @Account_Trans_Detail_Table_Type_Purchase, 
                    @Terms_Trans_Det_Table_Type, 
                    @Expense_Trans_Det_Table_Type, 
                    @Inward_Detail_Table_Type, 
                    @Account_Trans_Id, 
                    @Trans_Type, 
                    @Invoice_No, 
                    @Currency_Id, 
                    @Company_Id, 
                    @Year_Id, 
                    @Account_Id, 
                    @Rate, 
                    @User_Id, 
                    @Remarks, 
                    @Invoice_Date, 
                    @Invoice_Time, 
                    @Supplier_Id, 
                    @Is_First_Voucher_Add OUT",
                    parameter,
                    parameter_Terms,
                    parameter_Expense,
                    parameter_Inward_Detail,
                    _account_Trans_Id,
                    _trans_Type,
                    _invoice_No,
                    _currency_Id,
                    _company_Id,
                    _year_Id,
                    _account_Id,
                    _rate,
                    _user_Id,
                    _remarks,
                    _invoice_Date,
                    _invoice_Time,
                    _supplier_Id,
                    is_First_Voucher_Add);

                var _is_Exists = (bool)is_First_Voucher_Add.Value;
                if (!_is_Exists)
                    return ("not_exists", 409);

                return ("success", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Dictionary<string, object>>> Get_Account_Trans_Master_Purchase(int account_Trans_Id, int account_Trans_Detail_Id, string trans_Type, int? Year_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Trans_Master_Purchase_Select", connection))
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

        public async Task<int> Delete_Account_Trans_Master_Purchase(int Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Account_Trans_Master_Purchase_Delete {Id}"));
        }

        public async Task<Dictionary<string, object>> Get_Account_Trans_Purchase(int account_Trans_Id, string trans_Type, int? Year_Id)
        {
            var output = new Dictionary<string, object>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();

                // Fetch account_Trans_Master
                var account_Trans_Master_Result = await ExecuteStoredProcedure(connection, "Account_Trans_Master_Select_Purchase", account_Trans_Id, trans_Type, Year_Id);
                if (account_Trans_Master_Result != null)
                {
                    output["account_Trans_Master"] = account_Trans_Master_Result;
                }

                // Fetch account_Trans_Detail
                var account_Trans_Detail_Result = await ExecuteStoredProcedure(connection, "Account_Trans_Detail_Select_Purchase", account_Trans_Id, trans_Type, Year_Id);
                if (account_Trans_Detail_Result != null)
                {
                    output["account_Trans_Detail"] = account_Trans_Detail_Result;
                }

                // Fetch terms_Trans_Dets
                var terms_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Terms_Trans_Dets_Select_Purchase", account_Trans_Id, trans_Type, Year_Id);
                if (terms_Trans_Dets_Result != null)
                {
                    output["terms_Trans_Dets"] = terms_Trans_Dets_Result;
                }

                // Fetch expense_Trans_Dets
                var expense_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Expense_Trans_Dets_Select_Purchase", account_Trans_Id, trans_Type, Year_Id);
                if (expense_Trans_Dets_Result != null)
                {
                    output["expense_Trans_Dets"] = expense_Trans_Dets_Result;
                }

                // Fetch expense_Trans_Dets
                var inward_Detail_Result = await ExecuteStoredProcedure(connection, "Inward_Detail_Select_Purchase", account_Trans_Id, trans_Type, Year_Id);
                if (inward_Detail_Result != null)
                {
                    output["inward_Detail"] = inward_Detail_Result;
                }
            }

            return output;
        }

        private async Task<List<Dictionary<string, object>>> ExecuteStoredProcedure(SqlConnection connection, string storedProcedureName, int account_Trans_Id, string trans_Type, int? Year_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Account_Trans_Id", account_Trans_Id));
                command.Parameters.Add(!string.IsNullOrEmpty(trans_Type) ? new SqlParameter("@Trans_Type", trans_Type) : new SqlParameter("@Trans_Type", DBNull.Value));
                command.Parameters.Add(Year_Id > 0 ? new SqlParameter("@Year_Id", Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));

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

            return result.Count > 0 ? result : null;
        }
        public async Task<List<string>> Check_Inward_Detail_Stock_Id(string Stock_Id)
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Check_Inward_Detail_Stock_Id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Stock_Id) ? new SqlParameter("@Stock_Id", Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string Stock_Ids = string.Empty;

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnValue = reader.GetValue(i);

                                Stock_Ids = columnValue.ToString();
                            }

                            result.Add(Stock_Ids);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<string> Create_Stock_Id_Purchase(string CTS, string Shape)
        {
            try
            {
                var _cts = new SqlParameter("@Cts", string.IsNullOrEmpty(CTS) ? (object)DBNull.Value : CTS);
                var _shape = new SqlParameter("@Shape", string.IsNullOrEmpty(Shape) ? (object)DBNull.Value : Shape);

                var stock_Id = new SqlParameter("@Stock_Id", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC [dbo].[Stock_Number_Generation_Purchase] 
                    @Cts,
                    @Shape,
                    @Stock_Id OUT",
                    _cts,
                    _shape,
                    stock_Id);

                string stockIdValue = stock_Id.Value != DBNull.Value ? stock_Id.Value.ToString() : null;

                return stockIdValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }


        public async Task<IList<DropdownModel>> Get_Account_Master_SubGroupWise_Select(string? type)
        {
            var _type = !string.IsNullOrEmpty(type) ? new SqlParameter("@Type", type) : new SqlParameter("@Type", DBNull.Value);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec [dbo].[Account_Master_SubGroupWise_Select] @Type", _type).ToListAsync());

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Account_Master_TransTypeWise_Select(string? Trans_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Master_TransTypeWise_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Trans_Type) ? new SqlParameter("@Trans_Type", Trans_Type) : new SqlParameter("@Trans_Type", DBNull.Value));
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
        public async Task<List<Dictionary<string, object>>> Get_Account_Master_Active_Purchase_Select(int Party_Id, int Year_Id, string Trans_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Master_Active_Purchase_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(Party_Id > 0 ? new SqlParameter("@Party_Id", Party_Id) : new SqlParameter("@Party_Id", DBNull.Value));
                    command.Parameters.Add(Year_Id > 0 ? new SqlParameter("@Year_Id", Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(Trans_Type) ? new SqlParameter("@Trans_Type", Trans_Type) : new SqlParameter("@Trans_Type", DBNull.Value));
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
        public async Task<int> Create_Update_Cashbook_Account_Trans_Detail(DataTable dataTable, int? id, int? trans_Id, int? process_Id, int? company_Id, int? year_Id, DateTime? trans_Date, TimeSpan? trans_Time,
            int? by_Account, string by_Type, int? to_Account, string to_Type, int? currency_Id, float? ex_Rate, decimal? amount, decimal? amount_in_us, string remarks, string source_party, int user_Id)
        {
            var _terms_Invoice_Adjust_Table_Type = new SqlParameter("@Terms_Invoice_Adjust_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Terms_Invoice_Adjust_Table_Type",
                Value = dataTable
            };

            var _id = new SqlParameter("@Id", ((id ?? 0) > 0) ? id : DBNull.Value);
            var _company_Id = new SqlParameter("@Company_Id", (company_Id > 0) ? company_Id : DBNull.Value);
            var _year_Id = new SqlParameter("@Year_Id", (year_Id > 0) ? year_Id : DBNull.Value);
            var _by_Account = new SqlParameter("@By_Account", (by_Account > 0) ? by_Account : DBNull.Value);
            var _by_Type = new SqlParameter("@By_Type", by_Type);
            var _to_Account = new SqlParameter("@To_Account", (to_Account > 0) ? to_Account : DBNull.Value);
            var _to_Type = new SqlParameter("@To_Type", to_Type);
            var _currrency_id = new SqlParameter("@Currency_Id", (currency_Id > 0) ? currency_Id : DBNull.Value);
            var _ex_rate = new SqlParameter("@Ex_Rate", (ex_Rate > 0) ? ex_Rate : DBNull.Value);
            var _amount = new SqlParameter("@Amount", amount);
            var _remarks = new SqlParameter("@Remarks", string.IsNullOrEmpty(remarks) ? (object)DBNull.Value : remarks);
            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var _source_party = new SqlParameter("@Source_Party", source_party);

            var _trans_Date = new SqlParameter("@Trans_Date", SqlDbType.Date)
            {
                Value = trans_Date.HasValue ? (object)trans_Date.Value.Date : DBNull.Value
            };

            var _trans_Time = new SqlParameter("@Trans_Time", SqlDbType.Time)
            {
                Value = trans_Time.HasValue ? (object)trans_Time : DBNull.Value
            };

            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync(
                    @"EXEC [dbo].[Cashbook_Account_Trans_Detail_Insert_Update] 
                    @Terms_Invoice_Adjust_Table_Type,
                    @Id,
                    @Company_Id,
                    @Year_Id,
                    @Trans_Date,
                    @Trans_Time,
                    @By_Account,
                    @By_Type,
                    @To_Account,
                    @To_Type,
                    @Currency_Id,
                    @Ex_Rate,
                    @Amount,
                    @Remarks,
                    @Source_Party,
                    @User_Id",
                    _terms_Invoice_Adjust_Table_Type,
                    _id,
                    _company_Id,
                    _year_Id,
                    _trans_Date,
                    _trans_Time,
                    _by_Account,
                    _by_Type,
                    _to_Account,
                    _to_Type,
                    _currrency_id,
                    _ex_rate,
                    _amount,
                    _remarks,
                    _source_party,
                    _user_Id);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Dictionary<string, object>>> Get_Cashbook_Account_Trans_Select(int? id, int? year_id, int? company_id,int? process_id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Cashbook_Account_Trans_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(year_id > 0 ? new SqlParameter("@Year_Id", year_id) : new SqlParameter("@Year_Id", DBNull.Value));
                    command.Parameters.Add(company_id > 0 ? new SqlParameter("@Company_Id", company_id) : new SqlParameter("@Company_Id", DBNull.Value));
                    command.Parameters.Add(process_id > 0 ? new SqlParameter("@Process_Id", process_id) : new SqlParameter("@Process_Id", DBNull.Value));

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
        public async Task<List<Dictionary<string, object>>> Get_Cashbook_Account_Trans_Detail_Select(int id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Cashbook_Account_Trans_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));

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
        public async Task<int> Delete_Cashbook_Account_Trans(int Id, int User_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"[dbo].[Cashbook_Account_Trans_Detail_Delete] {Id}, {User_Id}"));
        }
        public async Task<IList<DropdownModel>> Get_Account_Master_Select(string? group, string? subGroup, int? mainCompany, string? purchaseExpense, string? salesExpense, bool? isParty, int? accountId)
        {
            var _account_Id = accountId > 0 ? new SqlParameter("@Account_Id", accountId) : new SqlParameter("@Account_Id", DBNull.Value);
            var _group = !string.IsNullOrEmpty(group) ? new SqlParameter("@Group", group) : new SqlParameter("@Group", DBNull.Value);
            var _sub_group = !string.IsNullOrEmpty(subGroup) ? new SqlParameter("@Sub_Group", subGroup) : new SqlParameter("@Sub_Group", DBNull.Value);
            var _main_company = accountId > 0 ? new SqlParameter("@Main_Company", mainCompany) : new SqlParameter("@Main_Company", DBNull.Value);
            var _purchase_expense = !string.IsNullOrEmpty(purchaseExpense) ? new SqlParameter("@Purchase_Expence", purchaseExpense) : new SqlParameter("@Purchase_Expence", DBNull.Value);
            var _sales_expense = !string.IsNullOrEmpty(salesExpense) ? new SqlParameter("@Sales_Expence", salesExpense) : new SqlParameter("@Sales_Expence", DBNull.Value);
            var _is_party = isParty.HasValue ? new SqlParameter("@Is_Party", isParty) : new SqlParameter("@Is_Party", DBNull.Value);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec [dbo].[Account_Master_By_Types_Select] @Account_Id,@Group,@Sub_Group,@Main_Company,@Purchase_Expence,@Sales_Expence,@Is_Party",
                            _account_Id, _group, _sub_group, _main_company, _purchase_expense, _sales_expense, _is_party).ToListAsync());

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Account_Trans_Detail_Ledger_Select(int? Account_Id, DateTime? fromDate, DateTime? toDate, int? Year_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("dbo.Account_Trans_Detail_Ledger_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(Account_Id > 0 ? new SqlParameter("@Account_Id", Account_Id) : new SqlParameter("@Account_Id", DBNull.Value));
                    var _from_Date = new SqlParameter("@From_Date", SqlDbType.Date)
                    {
                        Value = fromDate.HasValue ? (object)fromDate.Value.Date : DBNull.Value
                    };
                    command.Parameters.Add(_from_Date);
                    var _to_Date = new SqlParameter("@To_Date", SqlDbType.Date)
                    {
                        Value = toDate.HasValue ? (object)toDate.Value.Date : DBNull.Value
                    };
                    command.Parameters.Add(_to_Date);
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
        #endregion
    }
}
