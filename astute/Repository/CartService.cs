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
    public partial class CartService : ICartService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public CartService(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public async Task<int> Insert_Cart(DataTable dataTable, int User_Id)
        {
            var parameter = new SqlParameter("@Cart_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Cart_Table_Type",
                Value = dataTable
            };
            var user_Id = User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Cart_Insert_Update] @Cart_Table_Type, @User_Id, @IsExist OUT", parameter, user_Id, is_Exists));
            var _is_Exists = (bool)is_Exists.Value;
            if (_is_Exists)
                return 409;


            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Cart(string upload_Type, string userIds)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Cart_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(upload_Type) ? new SqlParameter("@Upload_Type", upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(userIds) ? new SqlParameter("@User_Ids", userIds) : new SqlParameter("@User_Ids", DBNull.Value));
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
        public async Task<int> Delete_Cart(string ids, int user_Id)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(ids) ? new SqlParameter("@Ids", ids) : new SqlParameter("@Ids", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Delete @Supp_Stock_Ids, @User_Id", supp_Stock_Ids, user_Id));

            return result;
        }
        public async Task<int> Approved_Or_Rejected_by_Management(string ids, bool is_Approved, bool is_Rejected, int user_Id)
        {
            var _ids = !string.IsNullOrEmpty(ids) ? new SqlParameter("@Ids", ids) : new SqlParameter("@Ids", DBNull.Value);
            var _is_Approved = new SqlParameter("@Is_Approved", is_Approved);
            var _is_Rejected = new SqlParameter("@Is_Rejected", is_Rejected);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Approved_Or_Rejected_By_Management @Ids, @Is_Approved, @Is_Rejected, @User_Id", _ids, _is_Approved, _is_Rejected, _user_Id));

            return result;
        }
        #endregion
    }
}
