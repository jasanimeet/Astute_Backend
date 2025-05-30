﻿using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
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
    public partial class CategoryService : ICategoryService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public CategoryService(AstuteDbContext dbContext,
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

        #region Utilities
        private async Task Insert_Category_Master_Trace(Category_Master category_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
            var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

            var columnName = new SqlParameter("@ColumnName", category_Master.Column_Name);
            var displayName = new SqlParameter("@DisplayName", category_Master.Display_Name);
            var status = new SqlParameter("@Status", category_Master.Status);
            var colId = new SqlParameter("@ColId", category_Master.Col_Id);

            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(user_Id ?? 0, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"EXEC Category_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @RecordType, @ColumnName, @DisplayName, @Status, @ColId"
            , empId, ipaddress, date, time, record_Type, columnName, displayName, status, colId));

        }

        private async Task Insert_Category_Value_Trace(Category_Value category_Value, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
            var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

            var catName = new SqlParameter("@CatName", category_Value.Cat_Name);
            var groupName = !string.IsNullOrEmpty(category_Value.Group_Name) ? new SqlParameter("@GroupName", category_Value.Group_Name) : new SqlParameter("@GroupName", DBNull.Value);
            var rapaportName = !string.IsNullOrEmpty(category_Value.Rapaport_Name) ? new SqlParameter("@RapaportName", category_Value.Rapaport_Name) : new SqlParameter("@RapaportName", DBNull.Value);
            var rapnetname = !string.IsNullOrEmpty(category_Value.Rapnet_name) ? new SqlParameter("@Rapnetname", category_Value.Rapnet_name) : new SqlParameter("@Rapnetname", DBNull.Value);
            var synonyms = !string.IsNullOrEmpty(category_Value.Synonyms) ? new SqlParameter("@Synonyms", category_Value.Synonyms) : new SqlParameter("@Synonyms", DBNull.Value);
            var orderNo = new SqlParameter("@OrderNo", category_Value.Order_No);
            var sortNo = new SqlParameter("@SortNo", category_Value.Sort_No);
            var status = new SqlParameter("@Status", category_Value.Status);
            var icon_Url = !string.IsNullOrEmpty(category_Value.Icon_Url) ? new SqlParameter("@IconUrl", category_Value.Icon_Url) : new SqlParameter("@IconUrl", DBNull.Value);
            var catId = category_Value.Cat_Id > 0 ? new SqlParameter("@CatId", category_Value.Cat_Id) : new SqlParameter("@CatId", DBNull.Value);
            var displayName = !string.IsNullOrEmpty(category_Value.Display_Name) ? new SqlParameter("@DisplayName", category_Value.Display_Name) : new SqlParameter("@DisplayName", DBNull.Value);
            var shortName = !string.IsNullOrEmpty(category_Value.Short_Name) ? new SqlParameter("@ShortName", category_Value.Short_Name) : new SqlParameter("@ShortName", DBNull.Value);

            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(user_Id ?? 0, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);
            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Category_Value_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @RecordType, @CatName, @GroupName, @RapaportName,
            @Rapnetname, @Synonyms, @OrderNo, @SortNo, @Status, @IconUrl, @CatId, @DisplayName, @ShortName",
            empId, ipaddress, date, time, record_Type, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName));
        }
        #endregion

        #region Methods
        #region Category Master
        public async Task<int> InsertCategory(Category_Master category_Master)
        {
            var catId = new SqlParameter("@CatId", category_Master.Cat_Id);
            var columnName = new SqlParameter("@ColumnName", category_Master.Column_Name);
            var displayName = new SqlParameter("@DisplayName", category_Master.Display_Name);
            var status = new SqlParameter("@Status", category_Master.Status);
            var colId = new SqlParameter("@ColId", category_Master.Col_Id);
            var recordType = new SqlParameter("@RecordType", "Insert");
            var isReferencedParameter = new SqlParameter("@IsExistsCat", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Master_Insert_Update @CatId, @ColumnName, @DisplayName, @Status, @ColId, @RecordType, @IsExistsCat OUT",
            catId, columnName, displayName, status, colId, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Category_Master_Trace(category_Master, "Insert");
            //}
            return result;
        }
        public async Task<int> UpdateCategory(Category_Master category_Master)
        {
            var catId = new SqlParameter("@CatId", category_Master.Cat_Id);
            var columnName = new SqlParameter("@ColumnName", category_Master.Column_Name);
            var displayName = new SqlParameter("@DisplayName", category_Master.Display_Name);
            var status = new SqlParameter("@Status", category_Master.Status);
            var colId = new SqlParameter("@ColId", category_Master.Col_Id);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isReferencedParameter = new SqlParameter("@IsExistsCat", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Master_Insert_Update @CatId, @ColumnName, @DisplayName, @Status, @ColId, @RecordType, @IsExistsCat OUT",
            catId, columnName, displayName, status, colId, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Category_Master_Trace(category_Master, "Update");
            //}
            return result;
        }
        public async Task<int> DeleteCategory(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var _id = id > 0 ? new SqlParameter("@CatId", id) : new SqlParameter("@CatId", DBNull.Value);

            //    var result_cat = await Task.Run(() => _dbContext.Category_Master
            //                    .FromSqlRaw(@"exec Category_Master_Select @CatId", _id).AsEnumerable()
            //                    .FirstOrDefault());

            //    if (result_cat != null)
            //    {
            //        await Insert_Category_Master_Trace(result_cat, "Delete");
            //    }
            //}
            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Category_Master_Delete @CatId, @IsReferenced OUT",
                                        new SqlParameter("@CatId", id),
                                        isReferencedParameter);
            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        public async Task<IList<Category_Master>> GetCategory(int catId, int colId)
        {
            var CatId = catId > 0 ? new SqlParameter("@catId", catId) : new SqlParameter("@catId", DBNull.Value);
            var ColId = colId > 0 ? new SqlParameter("@colId", colId) : new SqlParameter("@colId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Category_Master
                            .FromSqlRaw(@"exec Category_Master_Select @catId, @colId", CatId, ColId).ToListAsync());

            return result;
        }
        #endregion

        #region Category Values
        public async Task<int> InsertCategoryValue(Category_Value category_Value)
        {
            var catvalId = new SqlParameter("@CatvalId", category_Value.Cat_val_Id);
            var catName = new SqlParameter("@CatName", category_Value.Cat_Name);
            var groupName = !string.IsNullOrEmpty(category_Value.Group_Name) ? new SqlParameter("@GroupName", category_Value.Group_Name) : new SqlParameter("@GroupName", DBNull.Value);
            var rapaportName = !string.IsNullOrEmpty(category_Value.Rapaport_Name) ? new SqlParameter("@RapaportName", category_Value.Rapaport_Name) : new SqlParameter("@RapaportName", DBNull.Value);
            var rapnetname = !string.IsNullOrEmpty(category_Value.Rapnet_name) ? new SqlParameter("@Rapnetname", category_Value.Rapnet_name) : new SqlParameter("@Rapnetname", DBNull.Value);
            var synonyms = !string.IsNullOrEmpty(category_Value.Synonyms) ? new SqlParameter("@Synonyms", category_Value.Synonyms) : new SqlParameter("@Synonyms", DBNull.Value);
            var orderNo = category_Value.Order_No > 0 ? new SqlParameter("@OrderNo", category_Value.Order_No) : new SqlParameter("@OrderNo", DBNull.Value);
            var sortNo = category_Value.Sort_No > 0 ? new SqlParameter("@SortNo", category_Value.Sort_No) : new SqlParameter("@SortNo", DBNull.Value);
            var status = new SqlParameter("@Status", category_Value.Status);
            var icon_Url = !string.IsNullOrEmpty(category_Value.Icon_Url) ? new SqlParameter("@Icon_Url", category_Value.Icon_Url) : new SqlParameter("@Icon_Url", DBNull.Value);
            var catId = category_Value.Cat_Id > 0 ? new SqlParameter("@CatId", category_Value.Cat_Id) : new SqlParameter("@CatId", DBNull.Value);
            var displayName = !string.IsNullOrEmpty(category_Value.Display_Name) ? new SqlParameter("@DisplayName", category_Value.Display_Name) : new SqlParameter("@DisplayName", DBNull.Value);
            var shortName = !string.IsNullOrEmpty(category_Value.Short_Name) ? new SqlParameter("@ShortName", category_Value.Short_Name) : new SqlParameter("@ShortName", DBNull.Value);
            var webName = !string.IsNullOrEmpty(category_Value.Web_Name) ? new SqlParameter("@WebName", category_Value.Web_Name) : new SqlParameter("@WebName", DBNull.Value);
            var recordType = new SqlParameter("@RecordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", category_Value.IsForceInsert);
            var isExistCatVal = new SqlParameter("@IsExistsCatVal", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var isExistOrderNo = new SqlParameter("@IsExistsOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var isExistSortNo = new SqlParameter("@IsExistsSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Value_Insert_Update @CatvalId, @CatName, @GroupName, @RapaportName, @Rapnetname,
            @Synonyms, @OrderNo, @SortNo, @Status, @Icon_Url, @CatId, @DisplayName, @ShortName, @WebName, @RecordType, @IsExistsCatVal OUT, @IsExistsOrderNo OUT, @IsExistsSortNo OUT, @IsForceInsert,@Inserted_Id OUT",
            catvalId, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName, webName, recordType,
            isExistCatVal, isExistOrderNo, isExistSortNo, isForce_Insert, inserted_Id));

            bool isExist = (bool)isExistCatVal.Value;
            if (isExist)
                return 2;

            bool isExistOrder_No = (bool)isExistOrderNo.Value;
            if (isExistOrder_No)
                return 3;

            bool isExistSort_No = (bool)isExistSortNo.Value;
            if (isExistSort_No)
                return 4;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Category_Value_Trace(category_Value, "Insert");
            }

            int inserted_id = (int)inserted_Id.Value;
            return inserted_id;
        }
        public async Task<int> InsertCategoryValuePricing(int Cat_Val_Id, int Cat_Id)
        {
            var catvalId = new SqlParameter("@CatvalId", Cat_Val_Id);
            var catId = new SqlParameter("@CatId", Cat_Id);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Supplier_Pricing_Category_Value_Insert_Update @CatvalId, @CatId", catvalId, catId));

            return result;
        }
        public async Task<int> UpdateCategoryValue(Category_Value category_Value)
        {
            var catvalId = new SqlParameter("@CatvalId", category_Value.Cat_val_Id);
            var catName = new SqlParameter("@CatName", category_Value.Cat_Name);
            var groupName = !string.IsNullOrEmpty(category_Value.Group_Name) ? new SqlParameter("@GroupName", category_Value.Group_Name) : new SqlParameter("@GroupName", DBNull.Value);
            var rapaportName = !string.IsNullOrEmpty(category_Value.Rapaport_Name) ? new SqlParameter("@RapaportName", category_Value.Rapaport_Name) : new SqlParameter("@RapaportName", DBNull.Value);
            var rapnetname = !string.IsNullOrEmpty(category_Value.Rapnet_name) ? new SqlParameter("@Rapnetname", category_Value.Rapnet_name) : new SqlParameter("@Rapnetname", DBNull.Value);
            var synonyms = !string.IsNullOrEmpty(category_Value.Synonyms) ? new SqlParameter("@Synonyms", category_Value.Synonyms) : new SqlParameter("@Synonyms", DBNull.Value);
            var orderNo = category_Value.Order_No > 0 ? new SqlParameter("@OrderNo", category_Value.Order_No) : new SqlParameter("@OrderNo", DBNull.Value);
            var sortNo = category_Value.Sort_No > 0 ? new SqlParameter("@SortNo", category_Value.Sort_No) : new SqlParameter("@SortNo", DBNull.Value);
            var status = new SqlParameter("@Status", category_Value.Status);
            var icon_Url = !string.IsNullOrEmpty(category_Value.Icon_Url) ? new SqlParameter("@Icon_Url", category_Value.Icon_Url) : new SqlParameter("@Icon_Url", DBNull.Value);
            var catId = category_Value.Cat_Id > 0 ? new SqlParameter("@CatId", category_Value.Cat_Id) : new SqlParameter("@CatId", DBNull.Value);
            var displayName = !string.IsNullOrEmpty(category_Value.Display_Name) ? new SqlParameter("@DisplayName", category_Value.Display_Name) : new SqlParameter("@DisplayName", DBNull.Value);
            var shortName = !string.IsNullOrEmpty(category_Value.Short_Name) ? new SqlParameter("@ShortName", category_Value.Short_Name) : new SqlParameter("@ShortName", DBNull.Value);
            var webName = !string.IsNullOrEmpty(category_Value.Web_Name) ? new SqlParameter("@WebName", category_Value.Web_Name) : new SqlParameter("@WebName", DBNull.Value);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", category_Value.IsForceInsert);
            var isExistCatVal = new SqlParameter("@IsExistsCatVal", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var isExistOrderNo = new SqlParameter("@IsExistsOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var isExistSortNo = new SqlParameter("@IsExistsSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Value_Insert_Update @CatvalId, @CatName, @GroupName, @RapaportName, @Rapnetname,
            @Synonyms, @OrderNo, @SortNo, @Status, @Icon_Url, @CatId, @DisplayName, @ShortName, @WebName, @RecordType, @IsExistsCatVal OUT, @IsExistsOrderNo OUT, @IsExistsSortNo OUT, @IsForceInsert,@Inserted_Id OUT",
            catvalId, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName, webName, recordType,
            isExistCatVal, isExistOrderNo, isExistSortNo, isForce_Insert, inserted_Id));

            bool isExist = (bool)isExistCatVal.Value;
            if (isExist)
                return 2;

            bool isExistOrder_No = (bool)isExistOrderNo.Value;
            if (isExistOrder_No)
                return 3;

            bool isExistSort_No = (bool)isExistSortNo.Value;
            if (isExistSort_No)
                return 4;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Category_Value_Trace(category_Value, "Update");
            }

            return result;
        }
        public async Task<int> DeleteCategoryValue(int id)
        {
            var cat_val_Id = new SqlParameter("@catValId", id);
            var isExists_Party_Type = new SqlParameter("@IsExists_Party_Type", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExists_Employee_Type = new SqlParameter("@IsExists_Employee_Type", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var categoryValue = await Task.Run(() => _dbContext.Category_Value
                                .FromSqlRaw(@"exec GetCategoryValueByCatValId @catValId", cat_val_Id)
                                .AsEnumerable()
                                .FirstOrDefault());
                if (categoryValue != null)
                {
                    await Insert_Category_Value_Trace(categoryValue, "Delete");
                }
            }
            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Category_Value_Delete @catvalId, @IsExists_Party_Type OUT, @IsExists_Employee_Type OUT", cat_val_Id, isExists_Party_Type, isExists_Employee_Type));

            bool isExistPartyType = (bool)isExists_Party_Type.Value;
            if (isExistPartyType)
                return 547;

            bool isExistEmployeeType = (bool)isExists_Employee_Type.Value;
            if (isExistEmployeeType)
                return 547;

            return result;
        }
        public async Task<Category_Value> GetCategoryValueByCatValId(int catValId)
        {
            var model = new Category_Value();
            var param = new SqlParameter("@catValId", catValId);

            var categoryValue = await Task.Run(() => _dbContext.Category_Value
                            .FromSqlRaw(@"exec GetCategoryValueByCatValId @catValId", param).ToListAsync());

            if (categoryValue != null && categoryValue.Count > 0)
            {
                foreach (var item in categoryValue)
                {
                    item.Preview_Icon = !string.IsNullOrEmpty(item.Icon_Url) ? _configuration["BaseUrl"] + CoreCommonFilePath.CategoryIcomFilePath + item.Icon_Url : null;
                }
            }

            return categoryValue.FirstOrDefault();
        }
        public async Task<Dictionary<string, List<Dictionary<string, object>>>> Get_All_Category_Values()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Category_Value_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (row[col] == DBNull.Value)
                            {
                                dict[col.ColumnName] = null;
                            }
                            else
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        result.Add(dict);
                    }
                }
            }

            var groupedResult = result.GroupBy(item => item["Column_Name"].ToString()).ToDictionary(group => group.Key, group => group.ToList());

            return groupedResult;
        }
        public async Task<IList<CategoryValueModel>> GetCategoryValuesByCatId(int catId)
        {
            var model = new List<CategoryValueModel>();
            var param = new SqlParameter("@catId", catId);

            var categoryValue = await Task.Run(() => _dbContext.Category_Value
                            .FromSqlRaw(@"exec GetCategoryValuesByCatID @catId", param).ToListAsync());
            if (categoryValue != null && categoryValue.Count > 0)
            {
                foreach (var x in categoryValue)
                {
                    model.Add(new CategoryValueModel()
                    {
                        Cat_val_Id = x.Cat_val_Id,
                        Cat_Name = x.Cat_Name.Replace("_", " "),
                        Group_Name = x.Group_Name,
                        Rapaport_Name = x.Rapaport_Name,
                        Rapnet_name = x.Rapnet_name,
                        Synonyms = x.Synonyms,
                        Order_No = x.Order_No ?? 0,
                        Sort_No = x.Sort_No ?? 0,
                        Status = x.Status ?? false,
                        Icon_Url = !string.IsNullOrEmpty(x.Icon_Url) ? _configuration["BaseUrl"] + CoreCommonFilePath.CategoryIcomFilePath + x.Icon_Url : null,
                        Cat_Id = catId,
                        Display_Name = x.Display_Name,
                        Short_Name = x.Short_Name,
                        Web_Name = x.Web_Name
                    });
                }
            }
            return model;
        }
        public async Task<IList<CategoryValueModel>> Get_Active_Category_Values(int catId)
        {
            var model = new List<CategoryValueModel>();
            var param = catId > 0 ? new SqlParameter("@catId", catId) : new SqlParameter("@catId", DBNull.Value);

            var categoryValue = await Task.Run(() => _dbContext.Category_Value
                            .FromSqlRaw(@"exec Category_Values_Active_Select @catId", param).ToListAsync());
            if (categoryValue != null && categoryValue.Count > 0)
            {
                foreach (var x in categoryValue)
                {
                    model.Add(new CategoryValueModel()
                    {
                        Cat_val_Id = x.Cat_val_Id,
                        Cat_Name = x.Cat_Name,
                        Group_Name = x.Group_Name,
                        Rapaport_Name = x.Rapaport_Name,
                        Rapnet_name = x.Rapnet_name,
                        Synonyms = x.Synonyms,
                        Order_No = x.Order_No ?? 0,
                        Sort_No = x.Sort_No ?? 0,
                        Status = x.Status ?? false,
                        Icon_Url = !string.IsNullOrEmpty(x.Icon_Url) ? _configuration["BaseUrl"] + CoreCommonFilePath.CategoryIcomFilePath + x.Icon_Url : null,
                        Cat_Id = catId,
                        Display_Name = x.Display_Name,
                        Short_Name = x.Short_Name
                    });
                }
            }
            return model;
        }
        public async Task<int> ChangeStatus(int cat_val_Id, bool status)
        {
            var catvalId = new SqlParameter("@Cat_Val_Id", cat_val_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Category_Value_Update_Status @Cat_Val_Id, @Status", catvalId, Status));
            return result;
        }
        public async Task<DataTable> GetCategororyValues(int catId)
        {
            using var con = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString());
            using var cmd = new SqlCommand("GetCategoryValuesByCatID", con);
            var param = new SqlParameter("@catId", catId);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@catId", param);

            con.Open();

            using var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            using var ds = new DataSet();
            da.Fill(ds);

            return ds.Tables[ds.Tables.Count - 1];
        }
        public async Task<int> Get_Category_Value_Max_Order_No(int cat_Id)
        {
            var result = await _dbContext.Category_Value.Where(x => x.Cat_Id == cat_Id).Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
        public async Task<List<Dictionary<string, object>>> Get_BGM()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("BGM_Select", connection))
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

        #region Column Master
        public async Task<List<Dictionary<string, object>>> Get_Column_Master()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Column_Master_Select", connection))
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

        #region Import Master
        public async Task<List<Dictionary<string, object>>> Get_Import_Master()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Master_Select", connection))
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

        public async Task<int> Insert_Import_Master(Import_Master import_Master, int user_Id)
        {
            var importid = new SqlParameter("@Import_Id", import_Master.Import_Id);
            var formatname = new SqlParameter("@Format_Name", import_Master.Format_Name);
            var type = new SqlParameter("@Type", import_Master.Type);
            var userId = new SqlParameter("@UserId", user_Id);
            var isReferencedParameter = new SqlParameter("@ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Master_Insert_Update @Import_Id, @Format_Name, @Type, @UserId, @ID OUT",
            importid, formatname, type, userId, isReferencedParameter));

            var Id = (int)isReferencedParameter.Value;

            return Id;
        }

        public async Task<int> Update_Import_Master(Import_Master import_Master)
        {
            var importid = new SqlParameter("@Import_Id", import_Master.Import_Id);
            var formatname = new SqlParameter("@Format_Name", import_Master.Format_Name);
            var type = new SqlParameter("@Type", import_Master.Type);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Master_Insert_Update @Import_Id, @Format_Name, @Type, @RecordType, @IsExist OUT",
            importid, formatname, type, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Delete_Import_Master(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Master_Delete @Import_Id, @IsReferenced OUT",
                                        new SqlParameter("@Import_Id", id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        #endregion

        #region Import Detail
        public async Task<List<Dictionary<string, object>>> Get_Import_Detail()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Detail_Select", connection))
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

        public async Task<int> Insert_Import_Detail(Import_Detail import_Detail)
        {
            var importdetid = new SqlParameter("@Import_Det_Id", import_Detail.Import_Det_Id);
            var importid = new SqlParameter("@Import_Id", import_Detail.Import_Id);
            var columnname = new SqlParameter("@Column_Name", import_Detail.Column_Name);
            var excelcolumnno = new SqlParameter("@Excel_Column_No", import_Detail.Excel_Column_No);
            var required = new SqlParameter("@Required", import_Detail.Required);
            var recordType = new SqlParameter("@RecordType", "Insert");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Detail_Insert_Update @Import_Det_Id, @Import_Id, @Column_Name, @Excel_Column_No, @Required, @RecordType, @IsExist OUT",
            importdetid, importid, columnname, excelcolumnno, required, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Update_Import_Detail(Import_Detail import_Detail)
        {
            var importdetid = new SqlParameter("@Import_Det_Id", import_Detail.Import_Det_Id);
            var importid = new SqlParameter("@Import_Id", import_Detail.Import_Id);
            var columnname = new SqlParameter("@Column_Name", import_Detail.Column_Name);
            var excelcolumnno = new SqlParameter("@Excel_Column_No", import_Detail.Excel_Column_No);
            var required = new SqlParameter("@Required", import_Detail.Required);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isReferencedParameter = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Import_Detail_Insert_Update @Import_Det_Id, @Import_Id, @Column_Name, @Excel_Column_No, @Required, @RecordType, @IsExist OUT",
            importdetid, importid, columnname, excelcolumnno, required, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                return 2;

            return result;
        }

        public async Task<int> Delete_Import_Detail(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Detail_Delete @Import_Det_Id, @IsReferenced OUT",
                                        new SqlParameter("@Import_Det_Id", id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Import_Excel()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Import_Excel_Select", connection))
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

        public async Task<List<Dictionary<string, object>>> Get_Import_Master_Detail(int import_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Import_Master_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Import_Id", import_Id > 0 ? import_Id : (object)DBNull.Value));

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

        public async Task<int> Insert_Update_Import_Excel(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Import_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Import_Detail_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Import_Excel_Insert_Update @Import_Detail", parameter));

            return result;
        }

        public async Task<int> Delete_Import_Excel(int id)
        {
            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Import_Master_Detail_Delete @Import_Id",
                                        new SqlParameter("@Import_Id", id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Import_Master_Detail_Purchase(int import_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("Import_Master_Detail_Select_Purchase", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Import_Id", import_Id > 0 ? import_Id : (object)DBNull.Value));

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
        #endregion
    }
}
