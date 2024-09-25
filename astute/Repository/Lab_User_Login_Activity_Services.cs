using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class Lab_User_Login_Activity_Services :ILab_User_Login_Activity_Services
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public Lab_User_Login_Activity_Services(AstuteDbContext dbContext,
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

        #region Methods
        public async Task<List<Dictionary<string, object>>> Get_Lab_User_Login_Activity(string? From_Date, string? To_Date, string? Common_Search)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Get_Lab_User_Login_Activity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(From_Date) ? new SqlParameter("@From_Date", From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(To_Date) ? new SqlParameter("@To_Date", To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(Common_Search) ? new SqlParameter("@Common_Search", Common_Search) : new SqlParameter("@Common_Search", DBNull.Value));
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
                                dict[columnName] = columnValue == DBNull.Value ? "" : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_User_Activity(string? From_Date, string? To_Date, string? Common_Search)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Get_Supplier_Stock_User_Activity", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(From_Date) ? new SqlParameter("@From_Date", From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(To_Date) ? new SqlParameter("@To_Date", To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(Common_Search) ? new SqlParameter("@Common_Search", Common_Search) : new SqlParameter("@Common_Search", DBNull.Value));
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
                                dict[columnName] = columnValue == DBNull.Value ? "" : columnValue;
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
