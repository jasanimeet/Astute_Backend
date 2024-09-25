using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class TransactionService :ITransactionService
    {

        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public TransactionService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IJWTAuthentication jWTAuthentication)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Method

        #region Hold

        public async Task<int> Create_Update_Transaction_Details(DataTable dataTable, int? Trans_Id,int Party_Code,int Due_Days,string Process,string Remarks)
        {
            var trans_id = new SqlParameter("@Trans_Id", Trans_Id);
            var party_code = Party_Code > 0 ? new SqlParameter("@Party_Code", Party_Code) : new SqlParameter("@Party_Code", DBNull.Value);
            var duedays = Due_Days > 0 ? new SqlParameter("@Due_Days", Due_Days) : new SqlParameter("@Due_Days", DBNull.Value);
            var process = !string.IsNullOrEmpty(Process) ? new SqlParameter("@Process", Process) : new SqlParameter("@Process", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(Remarks) ? new SqlParameter("@Remarks", Remarks) : new SqlParameter("@Remarks", DBNull.Value);

            var parameter = new SqlParameter("@Transaction_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Transaction_Detail_Table_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Transaction_Master_Insert_Update] @Trans_Id,@Party_Code,@Due_Days,@Process,@Remarks,@Transaction_Detail_Table_Type",
                        trans_id, party_code, duedays, process, remarks, parameter));


            return result;
        }
        public async Task<List<string>> Check_Transaction_Detail_Stock_Id(string Stock_Id)
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Check_Transaction_Detail_Stock_Id", connection))
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
        public async Task<List<Dictionary<string, object>>> Get_Transaction_Details(DataTable dataTable,string Stock_id,string Id,string Sign,string value)
        {
            var result = new List<Dictionary<string, object>>();

            var parameter = new SqlParameter("@Transaction_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Transaction_Detail_Table_Type",
                Value = dataTable
            };


            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[Get_Transaction_Details]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(!string.IsNullOrEmpty(Stock_id) ? new SqlParameter("@Stock_Id", Stock_id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(Id) ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(Sign) ? new SqlParameter("@Sign", Sign) : new SqlParameter("@Sign", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(value) ? new SqlParameter("@Value", value) : new SqlParameter("@Value", DBNull.Value));
                    command.CommandTimeout = 1800;
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

        public async Task<List<Dictionary<string, object>>> Get_Transaction_Detail()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Transaction_Detail_Select", connection))
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
        #endregion

        #endregion
    }
}
