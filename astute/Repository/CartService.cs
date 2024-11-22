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
        public async Task<(string, int, string)> Create_Update_Cart(DataTable dataTable, int Id, int User_Id, string customer_Name, string remarks, int validity_Days)
        {
            var parameter = new SqlParameter("@Cart_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Cart_Table_Type",
                Value = dataTable
            };

            var user_Id = User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);
            var _customer_Name = !string.IsNullOrEmpty(customer_Name) ? new SqlParameter("@Customer_Name", customer_Name) : new SqlParameter("@Customer_Name", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(remarks) ? new SqlParameter("@Remarks", remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _validity_Days = validity_Days > 0 ? new SqlParameter("@Validity_Days", validity_Days) : new SqlParameter("@Validity_Days", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var msg = new SqlParameter("@Msg", SqlDbType.NVarChar)
            {
                Size = -1,
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Cart_Insert_Update] @Cart_Table_Type, @Id, @User_Id, @Customer_Name, @Remarks, @Validity_Days, @IsExist OUT, @Msg OUT",
                        parameter,id, user_Id, _customer_Name, _remarks, _validity_Days, is_Exists, msg));

            var _is_Exists = (bool)is_Exists.Value;
            var _msg = (string)msg.Value;
            if (_is_Exists)
                return ("exist", 0, _msg);

            return ("success", result, _msg);
        }
        public async Task<(string, int, string)> Delete_Cart(string ids, int user_Id)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(ids) ? new SqlParameter("@Ids", ids) : new SqlParameter("@Ids", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var msg = new SqlParameter("@Msg", SqlDbType.NVarChar)
            {
                Size = -1,
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Delete @Ids, @User_Id, @IsExist OUT, @Msg OUT", supp_Stock_Ids, _user_Id, is_Exists, msg));

            var _is_Exists = (bool)is_Exists.Value;
            var _msg = (string)msg.Value;
            if (_is_Exists)
                return ("exist", 0, _msg);

            return ("success", result, _msg);
        }

        public async Task<int> Approved_Or_Rejected_by_Management(Approval_Management approval_Management)
        {
            var _ids = !string.IsNullOrEmpty(approval_Management.Ids) ? new SqlParameter("@Ids", approval_Management.Ids) : new SqlParameter("@Ids", DBNull.Value);
            var _is_Approved = new SqlParameter("@Is_Approved", approval_Management.Is_Approved);
            var _is_Rejected = new SqlParameter("@Is_Rejected", approval_Management.Is_Rejected);
            var _remarks = !string.IsNullOrEmpty(approval_Management.Remarks) ? new SqlParameter("@Remarks", approval_Management.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _user_Id = approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Approved_Or_Rejected_By_Management @Ids, @Is_Approved, @Is_Rejected, @Remarks, @User_Id", _ids, _is_Approved, _is_Rejected, _remarks, _user_Id));

            return result;
        }
        public async Task<int> Approved_Management_Update_Status(Approval_Management_Status approval_Management)
        {
            var _ids = !string.IsNullOrEmpty(approval_Management.Ids) ? new SqlParameter("@Ids", approval_Management.Ids) : new SqlParameter("@Ids", DBNull.Value);
            var _status = !string.IsNullOrEmpty(approval_Management.Status) ? new SqlParameter("@Status", approval_Management.Status) : new SqlParameter("@Status", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(approval_Management.Remarks) ? new SqlParameter("@Remarks", approval_Management.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _user_Id = approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Approved_Management_Update_Status @Ids, @Status,@Remarks, @User_Id", _ids, _status, _remarks, _user_Id));

            return result;
        }
        //public async Task<int> Create_Approved_Management(Approval_Management_Create_Update approval_Management)
        //{
        //    var _supp_Stock_Id = !string.IsNullOrEmpty(approval_Management.Supp_Stock_Id) ? new SqlParameter("@Supp_Stock_Id", approval_Management.Supp_Stock_Id) : new SqlParameter("@Supp_Stock_Id", DBNull.Value);
        //    var _cart_Id = !string.IsNullOrEmpty(approval_Management.Cart_Id) ? new SqlParameter("@Cart_Id", approval_Management.Cart_Id) : new SqlParameter("@Cart_Id", DBNull.Value);
        //    var _user_Id = approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
        //    var _remarks = !string.IsNullOrEmpty(approval_Management.Remarks) ? new SqlParameter("@Remarks", approval_Management.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
        //    var _status = !string.IsNullOrEmpty(approval_Management.Status) ? new SqlParameter("@Status", approval_Management.Status) : new SqlParameter("@Status", DBNull.Value);

        //    var result = await Task.Run(() => _dbContext.Database
        //                .ExecuteSqlRawAsync(@"EXEC [Approval_Management_Insert_Update] @Supp_Stock_Id, @Cart_Id, @User_Id, @Remarks ,@Status", _supp_Stock_Id, _cart_Id, _user_Id, _remarks, _status));

        //    return result;
        //}
        public async Task<(string,int, string)> Create_Approved_Management(DataTable dataTable, int Id, int user_Id, string remarks, string status)
        {
            var parameter = new SqlParameter("@tblApproval_Management", SqlDbType.Structured)
            {
                TypeName = "dbo.Approval_Management_Table_Type",
                Value = dataTable
            };
            var id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(remarks) ? new SqlParameter("@Remarks", remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _status = !string.IsNullOrEmpty(status) ? new SqlParameter("@Status", status) : new SqlParameter("@Status", DBNull.Value);
            var msg = new SqlParameter("@Msg", SqlDbType.NVarChar)
            {
                Size = -1,
                Direction = ParameterDirection.Output
            };
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Approval_Management_Insert_Update] @tblApproval_Management,@Id, @User_Id, @Remarks ,@Status,@IsExist OUT, @Msg OUT", parameter, id, _user_Id, _remarks, _status, is_Exists, msg));

            var _is_Exists = (bool)is_Exists.Value;
            var _msg = (string)msg.Value;
            if (_is_Exists)
                return ("exist", 0, _msg);

            return ("success", result, _msg);
        }
        public async Task<(string, int,string, int)> Create_Update_Order_Processing(DataTable dataTable, int Id, int? user_Id, string customer_Name, string remarks, string status, int? assist_By)
        {
            var parameter = new SqlParameter("@Order_Processing_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Order_Processing_Table_Type]",
                Value = dataTable
            };
            var id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _customer_Name = !string.IsNullOrEmpty(customer_Name) ? new SqlParameter("@Customer_Name", customer_Name) : new SqlParameter("@Customer_Name", DBNull.Value);
            var _remarks = !string.IsNullOrEmpty(remarks) ? new SqlParameter("@Remarks", remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var _status = !string.IsNullOrEmpty(status) ? new SqlParameter("@Status", status) : new SqlParameter("@Status", DBNull.Value);
            var _assist_By = assist_By > 0 ? new SqlParameter("@Assist_By", assist_By) : new SqlParameter("@Assist_By", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var msg = new SqlParameter("@Msg", SqlDbType.NVarChar)
            {
                Size = -1,
                Direction = ParameterDirection.Output
            };
            var Order_No = new SqlParameter("@Order_No", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Order_Processing_Insert_Update] @Order_Processing_Table_Type,@Id, @User_Id, @Customer_Name, @Remarks ,@Status, @Assist_By,@IsExist OUT,@Msg OUT, @Order_No OUT", parameter, id, _user_Id, _customer_Name, _remarks, _status, _assist_By, is_Exists,msg, Order_No));
            var _is_Exists = (bool)is_Exists.Value;
            var _msg = (string)msg.Value;
            var order_no = (int)Order_No.Value;
            if (_is_Exists)
                return ("exist", 0, _msg, order_no);

            return ("success", result, _msg, order_no);
        }
        public async Task<int> Order_Processing_Inactive(Order_Processing_Inactive order_processing)
        {
            var _Ids = !string.IsNullOrEmpty(order_processing.Ids) ? new SqlParameter("@Ids", order_processing.Ids) : new SqlParameter("@Ids", DBNull.Value);
            var _user_Id = order_processing.User_Id > 0 ? new SqlParameter("@User_Id", order_processing.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _is_Inactive = new SqlParameter("@Is_Inactive", order_processing.Is_Inactive);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Order_Processing_Inactive @Ids, @Is_Inactive, @User_Id", _Ids, _is_Inactive, _user_Id));

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Order_Summary(string order_no)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Summary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Order_No", order_no));
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
        public async Task<int> Order_Processing_Update_Order_Status(string order_Status, string order_No)
        {
            var _status = !string.IsNullOrEmpty(order_Status) ? new SqlParameter("@Status", order_Status) : new SqlParameter("@Status", DBNull.Value);
            var _order_No = !string.IsNullOrEmpty(order_No) ? new SqlParameter("@Order_No", order_No) : new SqlParameter("@Order_No", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Order_Prossesing_Summary_Update_Order_Status @Status,@Order_No", _status, _order_No));

            return result;
        }
        #endregion
    }
}
