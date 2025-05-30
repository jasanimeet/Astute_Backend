﻿using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class Account_Group_Service : IAccount_Group_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Account_Group_Service(AstuteDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        public async Task<(string, int)> Create_Update_Account_Group(Account_Group_Master account_Group_Master, int user_Id)
        {
            var ac_Group_Code = new SqlParameter("@AC_GRP_CODE", account_Group_Master.AC_GRP_CODE);
            var ac_Group_Name = !string.IsNullOrEmpty(account_Group_Master.AC_GRP_NAME) ? new SqlParameter("@AC_GRP_NAME", account_Group_Master.AC_GRP_NAME) : new SqlParameter("@AC_GRP_NAME", DBNull.Value);
            var comp_Code = account_Group_Master.COMP_CODE > 0 ? new SqlParameter("@COMP_CODE", account_Group_Master.COMP_CODE) : new SqlParameter("@COMP_CODE", DBNull.Value);
            var parent_Group = account_Group_Master.PARENT_GROUP > 0 ? new SqlParameter("@PARENT_GROUP", account_Group_Master.PARENT_GROUP) : new SqlParameter("@PARENT_GROUP", DBNull.Value);
            var trans_Type = !string.IsNullOrEmpty(account_Group_Master.TRANS_TYPE) ? new SqlParameter("@TRANS_TYPE", account_Group_Master.TRANS_TYPE) : new SqlParameter("@TRANS_TYPE", DBNull.Value);
            var main_Group = !string.IsNullOrEmpty(account_Group_Master.MAIN_GROUP) ? new SqlParameter("@MAIN_GROUP", account_Group_Master.MAIN_GROUP) : new SqlParameter("@MAIN_GROUP", DBNull.Value);
            var opposite_Group = account_Group_Master.OPPOSITE_GROUP > 0 ? new SqlParameter("@OPPOSITE_GROUP", account_Group_Master.OPPOSITE_GROUP) : new SqlParameter("@OPPOSITE_GROUP", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var is_Exist = new SqlParameter("@Is_Exist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };


            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Account_Group_Insert_Update @AC_GRP_CODE, @AC_GRP_NAME, @COMP_CODE, @PARENT_GROUP,
                            @TRANS_TYPE, @MAIN_GROUP, @OPPOSITE_GROUP, @User_Id, @Is_Exist OUT", ac_Group_Code, ac_Group_Name, comp_Code, parent_Group, trans_Type, main_Group, opposite_Group, _user_Id, is_Exist));
            bool _is_Exist = (bool)is_Exist.Value;
            if (_is_Exist)
                return ("exist", 409);

            return ("success", result);
        }
        public async Task<List<Dictionary<string, object>>> Get_Account_Group(int ac_Group_Code)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Group_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(ac_Group_Code > 0 ? new SqlParameter("@AC_GRP_CODE", ac_Group_Code) : new SqlParameter("@AC_GRP_CODE", DBNull.Value));

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
        public async Task<int> Delete_Account_Group(int ac_Group_Code)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Account_Group_Delete {ac_Group_Code}"));
        }
        public async Task<DataTable> Get_Account_Group_Excel()
        {
            DataTable dataTable = new DataTable();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Account_Group_Excel_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }
        public async Task<IList<DropdownModel>> Get_Perent_Account_Group()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Account_Perent_Group_Select")
                            .ToListAsync());
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Sub_Account_Group(int Id)
        {
            var _Id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Account_Sub_Group_Select @Id", _Id)
                            .ToListAsync());
            return result;
        }
        #endregion
    }
}
