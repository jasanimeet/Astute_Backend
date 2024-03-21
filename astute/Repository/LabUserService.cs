using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class LabUserService : ILabUserService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public LabUserService(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public async Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id)
        {
            var parameter = new SqlParameter("@tblLab_User", SqlDbType.Structured)
            {
                TypeName = "dbo.Lab_User_Table_Type",
                Value = dataTable
            };
            var _party_Id = new SqlParameter("@Party_Id", party_Id);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Lab_User_Insert_Update] @tblLab_User, @Party_Id",
                        parameter, _party_Id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_User(int id, int party_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_User_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value));
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
