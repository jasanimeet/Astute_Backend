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
    public class First_Voucher_No : IFirst_Voucher_No
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public First_Voucher_No(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public async Task<List<Dictionary<string, object>>> Get_First_Voucher_No_Master(int Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("First_Voucher_No_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(Id > 0 ? new SqlParameter("@Trans_Id", Id) : new SqlParameter("@Trans_Id", DBNull.Value));

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
        public async Task<(string, bool, bool, int)> Create_Update_First_Voucher_No_Master(DataTable dataTable, int? user_Id)
        {
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var parameter = new SqlParameter("@First_Voucher_No_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.First_Voucher_No_Table_Type",
                Value = dataTable
            };

            var is_Exists = new SqlParameter("@Is_Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var is_Prefix_Exists = new SqlParameter("@Is_Prefix_Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"EXEC First_Voucher_No_Insert_Update @First_Voucher_No_Table_Type, @User_Id, @Is_Exists OUT,@Is_Prefix_Exists OUT", parameter, _user_Id, is_Exists, is_Prefix_Exists));

            var _is_Exists = (bool)is_Exists.Value;
            var _is_Prefix_Exists = (bool)is_Prefix_Exists.Value;
            if (_is_Exists || _is_Prefix_Exists)
                return ("exist", _is_Exists, _is_Prefix_Exists, result);

            return ("success", _is_Exists, _is_Prefix_Exists, result);
        }
        public async Task<int> Delete_First_Voucher_No_Master(string Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"First_Voucher_No_Delete {Id}"));
        }

        #endregion
    }
}
