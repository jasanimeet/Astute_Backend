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
        public async Task<(string, int)> Create_Update_Cart(DataTable dataTable, int User_Id, string customer_Name, string remarks, int validity_Days)
        {
            var parameter = new SqlParameter("@Cart_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Cart_Table_Type",
                Value = dataTable
            };
            var user_Id = User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _customer_Name = !string.IsNullOrEmpty(customer_Name) ? new SqlParameter("@Customer_Name", customer_Name) : new SqlParameter("@Customer_Name", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(remarks) ? new SqlParameter("@Remarks", remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _validity_Days = validity_Days > 0 ? new SqlParameter("@Validity_Days", validity_Days) : new SqlParameter("@Validity_Days", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Cart_Insert_Update] @Cart_Table_Type, @User_Id, @Customer_Name, @Remarks, @Validity_Days, @IsExist OUT",
                        parameter, user_Id, _customer_Name, _remarks, _validity_Days, is_Exists));

            var _is_Exists = (bool)is_Exists.Value;
            if (_is_Exists)
                return ("exist",0);

            return ("success", result);
        }
        //public async Task<List<Dictionary<string, object>>> Get_Cart(string USER_ID)
        //{
        //    var result = new List<Dictionary<string, object>>();
        //    using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
        //    {
        //        using (var command = new SqlCommand("Cart_Select", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(!string.IsNullOrEmpty(USER_ID) ? new SqlParameter("@USER_ID", USER_ID) : new SqlParameter("@USER_ID", DBNull.Value));
        //            await connection.OpenAsync();

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var dict = new Dictionary<string, object>();

        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                    {
        //                        var columnName = reader.GetName(i);
        //                        var columnValue = reader.GetValue(i);

        //                        dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
        //                    }

        //                    result.Add(dict);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
        public async Task<int> Delete_Cart(string ids, int user_Id)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(ids) ? new SqlParameter("@Ids", ids) : new SqlParameter("@Ids", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Delete @Ids, @User_Id", supp_Stock_Ids, user_Id));

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
        public async Task<int> Create_Approved_Management(Approval_Management approval_Management)
        {
            var _supp_Stock_Id = !string.IsNullOrEmpty(approval_Management.Supp_Stock_Id) ? new SqlParameter("@Supp_Stock_Id", approval_Management.Supp_Stock_Id) : new SqlParameter("@Supp_Stock_Id", DBNull.Value);
            var _cart_Id = !string.IsNullOrEmpty(approval_Management.Cart_Id) ? new SqlParameter("@Cart_Id", approval_Management.Cart_Id) : new SqlParameter("@Cart_Id", DBNull.Value);
            var _user_Id = approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(approval_Management.Remarks) ? new SqlParameter("@Remarks", approval_Management.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _status = !string.IsNullOrEmpty(approval_Management.Status) ? new SqlParameter("@Status", approval_Management.Status) : new SqlParameter("@Status", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Approval_Management_Insert_Update] @Supp_Stock_Id, @Cart_Id, @User_Id, @Remarks ,@Status", _supp_Stock_Id, _cart_Id, _user_Id, _remarks , _status));

            return result;
        }
        public async Task<(string, int)> Create_Update_Order_Processing(DataTable dataTable, int? user_Id, string remarks, string status)
        {
            var parameter = new SqlParameter("@Order_Processing_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Order_Processing_Table_Type]",
                Value = dataTable
            };
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(remarks) ? new SqlParameter("@Remarks", remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _status = !string.IsNullOrEmpty(status) ? new SqlParameter("@Status", status) : new SqlParameter("@Status", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Order_Processing_Insert_Update] @Order_Processing_Table_Type, @User_Id, @Remarks ,@Status,@IsExist OUT", parameter, _user_Id, _remarks, _status, is_Exists));
            var _is_Exists = (bool)is_Exists.Value;
            if (_is_Exists)
                return ("exist", 0);
            return ("success", result);
        }
        #endregion
    }
}
