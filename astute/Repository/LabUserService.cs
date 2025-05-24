using astute.CoreServices;
using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public async Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id, int user_Id)
        {
            var parameter = new SqlParameter("@tblLab_User", SqlDbType.Structured)
            {
                TypeName = "dbo.Lab_User_Table_Type",
                Value = dataTable
            };
            var isExist_User_Name = new SqlParameter("@IsExist_User_Name", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExist_Primary_User = new SqlParameter("@IsExist_Primary_User", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var _party_Id = new SqlParameter("@Party_Id", party_Id);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Lab_User_Insert_Update] @tblLab_User, @Party_Id, @User_Id, @IsExist_User_Name OUT, @IsExist_Primary_User OUT",
                        parameter, _party_Id, _user_Id, isExist_User_Name, isExist_Primary_User));

            bool _isExistUserName = (bool)isExist_User_Name.Value;
            if (_isExistUserName)
                return 409;

            bool _isExist_Primary_User = (bool)isExist_Primary_User.Value;
            if (_isExist_Primary_User)
                return 410;

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Lab_User(int id, int party_Id, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_User_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
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

                                if (columnName == "Password")
                                    columnValue = CoreService.Decrypt(Convert.ToString(columnValue));

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Lab_User_Company(int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_User_Company_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
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

                                if (columnName == "Password")
                                    columnValue = CoreService.Decrypt(Convert.ToString(columnValue));

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Customer_Lab_User(int party_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Customer_Pricing_User_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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
        public async Task<int> Change_Active_Status(int id, bool active_Status)
        {
            var _Id = new SqlParameter("@Id", id);
            var _active_Status = new SqlParameter("@Active_Status", active_Status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Lab_User_Active_Status_Update @Id, @Active_Status", _Id, _active_Status));
            return result;
        }
        public async Task<Dictionary<string, object>> Get_Suspend_Day()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Suspend_Day_Select", connection))
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
            return result.FirstOrDefault();
        }
        public async Task<(int, string)> Delete_Lab_User(int id, bool check_Primary_User)
        {
            var _id = new SqlParameter("@Id", id);
            var _check_Primary_User = new SqlParameter("@Check_Primary_User", check_Primary_User);
            var is_Exist = new SqlParameter("@Is_Exist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var is_Sub_Exist = new SqlParameter("@Is_Sub_Exist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Lab_User_Delete] @Id, @Check_Primary_User, @Is_Exist OUT, @Is_Sub_Exist OUT",
                        _id, _check_Primary_User, is_Exist, is_Sub_Exist));
            var _is_Exist = (bool)is_Exist.Value;
            if (_is_Exist)
                return (409, "exist");

            var _is_Sub_Exist = (bool)is_Sub_Exist.Value;
            if (_is_Sub_Exist)
                return (409, "subExist");

            return (result, "success");

            //return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Lab_User_Delete {id}"));
        }
        public async Task<int> Create_Update_Suspend_Days(int id, int days)
        {
            var _id = new SqlParameter("@Id", id);
            var _days = new SqlParameter("@Days", days);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Suspend_Days_Insert_Update] @Id, @Days",
                        _id, _days));

            return result;
        }

        public async Task<int> Job_Transfer_User_Pricing()
        {

            var sqlCommand = @"exec [Job_Transfer_User_Pricing]";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = 1800;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            //var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC [Job_Transfer_User_Pricing]"));

            return result;
        }

        public async Task<int> Job_Transfer_Supplier_Pricing_Cal(int party_Id)
        {
            var _party_Id = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"EXEC [Job_Transfer_Supplier_Pricing_Cal] @Party_Id";
                command.Parameters.Add(_party_Id);

                command.CommandTimeout = 1800;

                await _dbContext.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }
        }

        public async Task<int> Job_Transfer_Auto_Supplier_Stock()
        {

            var sqlCommand = @"exec [Job_Transfer_Auto_Supplier_Stock]";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;

                    command.CommandTimeout = 3600;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }
        public async Task<int> Job_Transfer_Auto_Category_Value()
        {

            var sqlCommand = @"exec [Job_Transfer_Column_Master_Category_Master_Category_Value]";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;

                    command.CommandTimeout = 3600;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }

        public async Task<int> Job_Transfer_Auto_Stock_Pricing(string? Upload_From)
        {
            var _upload_From = !string.IsNullOrEmpty(Upload_From) ? new SqlParameter("@Upload_From", Upload_From) : new SqlParameter("@Upload_From", DBNull.Value);

            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"EXEC [Job_Transfer_Auto_Stock_Pricing] @Upload_From";
                command.Parameters.Add(_upload_From);

                command.CommandTimeout = 10800;

                await _dbContext.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }
        }

        public async Task<int> Job_Transfer_Auto_Party_Url_Format()
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"EXEC [Job_Transfer_Auto_Party_Url_Format]";

                command.CommandTimeout = 3600;

                await _dbContext.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }
        }

        public async Task<int> Job_Transfer_Auto_Party_Account_Master()
        {
            using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"EXEC [Job_Transfer_Party_Master_Account_Master]";

                command.CommandTimeout = 3600;

                await _dbContext.Database.OpenConnectionAsync();

                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
                finally
                {
                    _dbContext.Database.CloseConnection();
                }
            }
        }

        #endregion
    }
}
