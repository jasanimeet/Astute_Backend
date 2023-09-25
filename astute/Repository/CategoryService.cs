using astute.CoreModel;
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
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class CategoryService : ICategoryService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public CategoryService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Category_Trace(Category_Value category_Value, string recordType)
        {
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

            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);
            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Category_Value_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @RecordType, @CatName, @GroupName, @RapaportName,
            @Rapnetname, @Synonyms, @OrderNo, @SortNo, @Status, @IconUrl, @CatId, @DisplayName, @ShortName",
            empId, ipaddress, date, time, recordType, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName));
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
            var isReferencedParameter = new SqlParameter("@IsExistsCat", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Master_Insert_Update @CatId, @ColumnName, @DisplayName, @Status, @ColId, @RecordType, @IsExistsCat OUT",
            catId, columnName, displayName, status, colId, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                result = 2;

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
            var isReferencedParameter = new SqlParameter("@IsExistsCat", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Master_Insert_Update @CatId, @ColumnName, @DisplayName, @Status, @ColId, @RecordType, @IsExistsCat OUT",
            catId, columnName, displayName, status, colId, recordType, isReferencedParameter));

            var isExist = (bool)isReferencedParameter.Value;
            if (isExist)
                result = 2;

            return result;
        }
        public async Task<int> DeleteCategory(int id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Category_Master_Delete @CatId, @IsReferenced OUT",
                                        new SqlParameter("@CatId", id),
                                        isReferencedParameter);
            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                result = 2;

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
            var orderNo = new SqlParameter("@OrderNo", category_Value.Order_No);
            var sortNo = new SqlParameter("@SortNo", category_Value.Sort_No);
            var status = new SqlParameter("@Status", category_Value.Status);
            var icon_Url = !string.IsNullOrEmpty(category_Value.Icon_Url) ? new SqlParameter("@Icon_Url", category_Value.Icon_Url) : new SqlParameter("@Icon_Url", DBNull.Value);
            var catId = category_Value.Cat_Id > 0 ? new SqlParameter("@CatId", category_Value.Cat_Id) : new SqlParameter("@CatId", DBNull.Value);
            var displayName = !string.IsNullOrEmpty(category_Value.Display_Name) ? new SqlParameter("@DisplayName", category_Value.Display_Name) : new SqlParameter("@DisplayName", DBNull.Value);
            var shortName = !string.IsNullOrEmpty(category_Value.Short_Name) ? new SqlParameter("@ShortName", category_Value.Short_Name) : new SqlParameter("@ShortName", DBNull.Value);
            var recordType = new SqlParameter("@RecordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", category_Value.IsForceInsert);
            var isExistCatVal = new SqlParameter("@IsExistsCatVal", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var isExistOrderNo = new SqlParameter("@IsExistsOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var isExistSortNo = new SqlParameter("@IsExistsSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Value_Insert_Update @CatvalId, @CatName, @GroupName, @RapaportName, @Rapnetname,
            @Synonyms, @OrderNo, @SortNo, @Status, @Icon_Url, @CatId, @DisplayName, @ShortName, @RecordType, @IsExistsCatVal OUT, @IsExistsOrderNo OUT, @IsExistsSortNo OUT, @IsForceInsert",
            catvalId, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName, recordType,
            isExistCatVal, isExistOrderNo, isExistSortNo, isForce_Insert));

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
                await Insert_Category_Trace(category_Value, "Insert");
            }

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
            var orderNo = new SqlParameter("@OrderNo", category_Value.Order_No);
            var sortNo = new SqlParameter("@SortNo", category_Value.Sort_No);
            var status = new SqlParameter("@Status", category_Value.Status);
            var icon_Url = !string.IsNullOrEmpty(category_Value.Icon_Url) ? new SqlParameter("@Icon_Url", category_Value.Icon_Url) : new SqlParameter("@Icon_Url", DBNull.Value);
            var catId = category_Value.Cat_Id > 0 ? new SqlParameter("@CatId", category_Value.Cat_Id) : new SqlParameter("@CatId", DBNull.Value);
            var displayName = !string.IsNullOrEmpty(category_Value.Display_Name) ? new SqlParameter("@DisplayName", category_Value.Display_Name) : new SqlParameter("@DisplayName", DBNull.Value);
            var shortName = !string.IsNullOrEmpty(category_Value.Short_Name) ? new SqlParameter("@ShortName", category_Value.Short_Name) : new SqlParameter("@ShortName", DBNull.Value);
            var recordType = new SqlParameter("@RecordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", category_Value.IsForceInsert);
            var isExistCatVal = new SqlParameter("@IsExistsCatVal", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var isExistOrderNo = new SqlParameter("@IsExistsOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var isExistSortNo = new SqlParameter("@IsExistsSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Category_Value_Insert_Update @CatvalId, @CatName, @GroupName, @RapaportName, @Rapnetname,
            @Synonyms, @OrderNo, @SortNo, @Status, @Icon_Url, @CatId, @DisplayName, @ShortName, @RecordType, @IsExistsCatVal OUT, @IsExistsOrderNo OUT, @IsExistsSortNo OUT, @IsForceInsert",
            catvalId, catName, groupName, rapaportName, rapnetname, synonyms, orderNo, sortNo, status, icon_Url, catId, displayName, shortName, recordType,
            isExistCatVal, isExistOrderNo, isExistSortNo, isForce_Insert));

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
                await Insert_Category_Trace(category_Value, "Update");
            }

            return result;
        }
        public async Task<int> DeleteCategoryValue(int id)
        {
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var param = new SqlParameter("@catValId", id);

                var categoryValue = await Task.Run(() => _dbContext.Category_Value
                                .FromSqlRaw(@"exec GetCategoryValueByCatValId @catValId", param)
                                .AsEnumerable()
                                .FirstOrDefault());
                if (categoryValue != null)
                {
                    await Insert_Category_Trace(categoryValue, "Delete");
                }
            }
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Category_Value_Delete {id}"));
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
                        Cat_Name = x.Cat_Name,
                        Group_Name = x.Group_Name,
                        Rapaport_Name = x.Rapaport_Name,
                        Rapnet_name = x.Rapnet_name,
                        Synonyms = x.Synonyms,
                        Order_No = x.Order_No,
                        Sort_No = x.Sort_No,
                        Status = x.Status,
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
        #endregion
        #endregion
    }
}
