﻿using astute.Models;
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
    public partial class Parcel_Master_Service : IParcel_Master_Service
    {

        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Parcel_Master_Service(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Parcel Master
        public async Task<int> Insert_Update_Parcel_Master(Parcel_Master parcel_Master)
        {
            var Parcel_Id = new SqlParameter("@Parcel_Id", parcel_Master.Parcel_Id);
            var Parcel_Name = new SqlParameter("@Parcel_Name", parcel_Master.Parcel_Name);
            var Cat_Val_Id = parcel_Master.Cat_Val_Id > 0 ? new SqlParameter("@Cat_Val_Id", parcel_Master.Cat_Val_Id) : new SqlParameter("@Cat_Val_Id", DBNull.Value);
            var Unit = new SqlParameter("@Unit", parcel_Master.Unit);
            var Pointer = parcel_Master.Pointer > 0 ? new SqlParameter("@Pointer", parcel_Master.Pointer) : new SqlParameter("@Pointer", DBNull.Value);
            var Shape = new SqlParameter("@Shape", parcel_Master.Shape);
            var Color = new SqlParameter("@Color", parcel_Master.Color);
            var Clarity = new SqlParameter("@Clarity", parcel_Master.Clarity);
            var Status = new SqlParameter("@Status", parcel_Master.Status);
            var isExistTerms = new SqlParameter("@IsExistTerms", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Parcel_Master_Insert_Update @Parcel_Id, @Parcel_Name, @Cat_Val_Id, @Unit, @Pointer, @Shape, @Color, @Clarity, @Status, @IsExistTerms OUT",
            Parcel_Id, Parcel_Name, Cat_Val_Id, Unit, Pointer, Shape, Color, Clarity, Status, isExistTerms));

            bool termsIsExist = (bool)isExistTerms.Value;
            if (termsIsExist)
                return 5;

            return result;
        }

        public async Task<int> Delete_Parcel_Master(int parcel_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Parcel_Master_Delete {parcel_Id}"));
        }

        public async Task<List<Dictionary<string, object>>> Get_Parcel_Master(int parcel_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Parcel_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parcel_Id > 0 ? new SqlParameter("@Parcel_Id", parcel_Id) : new SqlParameter("@Parcel_Id", DBNull.Value));

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

        public async Task<List<Dictionary<string, object>>> Get_Parcel_Master_By_Cat_Val_Id(int cat_Val_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Parcel_Master_Select_By_Cat_Val_Id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(cat_Val_Id > 0 ? new SqlParameter("@Cat_Val_Id", cat_Val_Id) : new SqlParameter("@Cat_Val_Id", DBNull.Value));

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

        #region Parcel Ref Master
        public async Task<int> Insert_Update_Parcel_Ref_Master(Parcel_Ref_Master parcel_Ref_Master, int user_Id)
        {
            var Parcel_Ref_Id = new SqlParameter("@Parcel_Ref_Id", parcel_Ref_Master.Parcel_Ref_Id);
            var Parcel_Name = new SqlParameter("@Parcel_Name", parcel_Ref_Master.Parcel_Name);
            var Parcel_Id = new SqlParameter("@Parcel_Id", parcel_Ref_Master.Parcel_Id);
            var Unit = new SqlParameter("@Unit", parcel_Ref_Master.Unit);
            var Status = new SqlParameter("@Status", parcel_Ref_Master.Status);
            var User_Id = new SqlParameter("@User_Id", user_Id);
            var isExistParcel = new SqlParameter("@IsExistParcel", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"exec Parcel_Ref_Master_Insert_Update @Parcel_Ref_Id, @Parcel_Name, @Parcel_Id, @Unit, @Status, @User_Id,@IsExistParcel OUT", Parcel_Ref_Id, Parcel_Name, Parcel_Id, Unit, Status, User_Id, isExistParcel));

            bool parcelIsExist = (bool)isExistParcel.Value;
            if (parcelIsExist)
                return 5;

            return result;
        }

        public async Task<int> Delete_Parcel_Ref_Master(int parcel_Ref_Id, int user_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Parcel_Ref_Master_Delete {parcel_Ref_Id}, {user_Id}"));
        }

        public async Task<List<Dictionary<string, object>>> Get_Parcel_Ref_Master(int parcel_Ref_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Parcel_Ref_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parcel_Ref_Id > 0 ? new SqlParameter("@Parcel_Ref_Id", parcel_Ref_Id) : new SqlParameter("@Parcel_Ref_Id", DBNull.Value));

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

        public async Task<List<Dictionary<string, object>>> Get_Parcel_Ref()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Parcel_Ref_Select", connection))
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

        #region Stock Allocation

        public async Task<int> Insert_Update_Stock_Allocation(DataTable dataTable)
        {
            var param = new SqlParameter("@Stock_Allocation_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Stock_Allocation_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Stock_Allocation_Insert_Update @Stock_Allocation_Type", param);

            return result;
        }

        public async Task<int> Delete_Stock_Allocation(int Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Stock_Allocation_Delete {Id}"));
        }

        public async Task<List<Dictionary<string, object>>> Get_Stock_Allocation(int ac_Grp_Code, int company_Id, int year_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Allocation_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(ac_Grp_Code > 0 ? new SqlParameter("@Ac_Grp_Code", ac_Grp_Code) : new SqlParameter("@Ac_Grp_Code", DBNull.Value));
                    command.Parameters.Add(company_Id > 0 ? new SqlParameter("@Company_Id", company_Id) : new SqlParameter("@Company_Id", DBNull.Value));
                    command.Parameters.Add(year_Id > 0 ? new SqlParameter("@Year_Id", year_Id) : new SqlParameter("@Year_Id", DBNull.Value));

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
