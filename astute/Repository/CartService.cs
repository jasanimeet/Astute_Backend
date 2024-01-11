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
        #region Cart
        public async Task<int> Insert_Cart_Review_Aproval_Management(Cart_Review_Approval_Management cart_Review_Approval_Management)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(cart_Review_Approval_Management.Supp_Stock_Ids) ? new SqlParameter("@Supp_Stock_Ids", cart_Review_Approval_Management.Supp_Stock_Ids) : new SqlParameter("@Supp_Stock_Ids", DBNull.Value);
            var user_Id = cart_Review_Approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", cart_Review_Approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(cart_Review_Approval_Management.Upload_Type) ? new SqlParameter("@Upload_Type", cart_Review_Approval_Management.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Review_Approval_Management_Insert_Update @Supp_Stock_Ids, @User_Id, @Upload_Type, @IsExist OUT", supp_Stock_Ids, user_Id, upload_Type, is_Exists));
            var _is_Exists = (bool)is_Exists.Value;
            if (_is_Exists)
                return 409;

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Cart_Review_Approval_Management(string upload_Type, string userIds)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Cart_Review_Approval_Management_Select", connection))
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
        public async Task<int> Delete_Cart_Review_Aproval_Management(string ids, int user_Id, string upload_Type)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(ids) ? new SqlParameter("@Ids", ids) : new SqlParameter("@Ids", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _upload_Type = !string.IsNullOrEmpty(upload_Type) ? new SqlParameter("@Upload_Type", upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Review_Approval_Management_Delete @Supp_Stock_Ids, @User_Id, @Upload_Type", supp_Stock_Ids, user_Id, upload_Type));

            return result;
        }
        #endregion

        #region Approval Management
        #endregion
        #endregion
    }
}
