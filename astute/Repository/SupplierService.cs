using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class SupplierService : ISupplierService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public SupplierService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Value_Config_Trace(Value_Config value_Config, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var length_From = value_Config.Length_From > 0 ? new SqlParameter("@Length_From", value_Config.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = value_Config.Length_To > 0 ? new SqlParameter("@Length_To", value_Config.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = value_Config.Width_From > 0 ? new SqlParameter("@Width_From", value_Config.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = value_Config.Width_To > 0 ? new SqlParameter("@Width_To", value_Config.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = value_Config.Depth_From > 0 ? new SqlParameter("@Depth_From", value_Config.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = value_Config.Depth_To > 0 ? new SqlParameter("@Depth_To", value_Config.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = value_Config.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", value_Config.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = value_Config.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", value_Config.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = value_Config.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", value_Config.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = value_Config.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", value_Config.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = value_Config.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", value_Config.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = value_Config.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", value_Config.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = value_Config.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", value_Config.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = value_Config.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", value_Config.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = value_Config.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", value_Config.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = value_Config.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", value_Config.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = value_Config.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", value_Config.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = value_Config.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", value_Config.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = value_Config.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", value_Config.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = value_Config.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", value_Config.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var lr_Half_From = value_Config.Lr_Half_From > 0 ? new SqlParameter("@Lr_Half_From", value_Config.Lr_Half_From) : new SqlParameter("@Lr_Half_From", DBNull.Value);
            var lr_Half_To = value_Config.Lr_Half_To > 0 ? new SqlParameter("@Lr_Half_To", value_Config.Lr_Half_To) : new SqlParameter("@Lr_Half_To", DBNull.Value);
            var star_Ln_From = value_Config.Star_Ln_From > 0 ? new SqlParameter("@Star_Ln_From", value_Config.Star_Ln_From) : new SqlParameter("@Star_Ln_From", DBNull.Value);
            var star_Ln_To = value_Config.Star_Ln_To > 0 ? new SqlParameter("@Star_Ln_To", value_Config.Star_Ln_To) : new SqlParameter("@Star_Ln_To", DBNull.Value);
            var shape_Group = !string.IsNullOrEmpty(value_Config.Shape_Group) ? new SqlParameter("@Shape_Group", value_Config.Shape_Group) : new SqlParameter("@Shape_Group", DBNull.Value);
            var shape = !string.IsNullOrEmpty(value_Config.Shape) ? new SqlParameter("@Shape", value_Config.Shape) : new SqlParameter("@Shape", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Value_Config_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From, @Depth_Per_To, @Table_Per_From,
                        @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Lr_Half_From, @Lr_Half_To, @Star_Ln_From, @Star_Ln_To, @Shape_Group, @Shape", empId, ipaddress, date, time, record_Type, length_From, length_To,
            width_From, width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
            pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, lr_Half_From, lr_Half_To, star_Ln_From, star_Ln_To, shape_Group, shape));
        }
        #endregion

        #region Methods
        public async Task<int> InsertSupplierValueMapping(Supplier_Value_Mapping supplier_Value)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@SupId", supplier_Value.Sup_Id));
            parameter.Add(new SqlParameter("@SuppCatname", supplier_Value.Supp_Cat_Name));
            parameter.Add(new SqlParameter("@CatValId", supplier_Value.Cat_val_Id));
            parameter.Add(new SqlParameter("@Status", supplier_Value.Status));
            parameter.Add(new SqlParameter("@RecordType", "Insert"));

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @SupId, @SuppCatname, @CatValId, @Status, @RecordType", parameter.ToArray()));

            return result;
        }
        public async Task<int> UpdateSupplierValueMapping(Supplier_Value_Mapping supplier_Value)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@SupId", supplier_Value.Sup_Id));
            parameter.Add(new SqlParameter("@SuppCatname", supplier_Value.Supp_Cat_Name));
            parameter.Add(new SqlParameter("@CatValId", supplier_Value.Cat_val_Id));
            parameter.Add(new SqlParameter("@Status", supplier_Value.Status));
            parameter.Add(new SqlParameter("@RecordType", "Update"));

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @SupId, @SuppCatname, @CatValId, @Status, @RecordType", parameter.ToArray()));

            return result;
        }
        public async Task<int> DeleteSupplierValueMapping(int supId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Supplier_Value_Delete {supId}"));
        }
        public async Task<IList<Supplier_Value_Mapping>> Get_Supplier_Value_Mapping(int sup_Id, int cat_val_Id)
        {
            var _sup_Id = sup_Id > 0 ? new SqlParameter("@Sup_Id", sup_Id) : new SqlParameter("@Sup_Id", DBNull.Value);
            var _cal_val_Id = cat_val_Id > 0 ? new SqlParameter("@Cal_val_Id", cat_val_Id) : new SqlParameter("@Cal_val_Id", DBNull.Value);

            var categoryValue = await Task.Run(() => _dbContext.Supplier_Value_Mapping
                            .FromSqlRaw(@"exec Supplier_Value_Mapping_Select @Sup_Id, @Cal_val_Id", _sup_Id, _cal_val_Id).ToListAsync());

            return categoryValue;
        }
        #endregion

        #region Value Config
        public async Task<int> Add_Update_Value_Config(Value_Config value_Config)
        {
            var valueMap_ID = new SqlParameter("@ValueMap_ID", value_Config.ValueMap_ID);
            var length_From = value_Config.Length_From > 0 ? new SqlParameter("@Length_From", value_Config.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = value_Config.Length_To > 0 ? new SqlParameter("@Length_To", value_Config.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = value_Config.Width_From > 0 ? new SqlParameter("@Width_From", value_Config.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = value_Config.Width_To > 0 ? new SqlParameter("@Width_To", value_Config.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = value_Config.Depth_From > 0 ? new SqlParameter("@Depth_From", value_Config.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = value_Config.Depth_To > 0 ? new SqlParameter("@Depth_To", value_Config.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = value_Config.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", value_Config.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = value_Config.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", value_Config.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = value_Config.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", value_Config.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = value_Config.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", value_Config.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = value_Config.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", value_Config.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = value_Config.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", value_Config.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = value_Config.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", value_Config.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = value_Config.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", value_Config.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = value_Config.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", value_Config.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = value_Config.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", value_Config.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = value_Config.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", value_Config.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = value_Config.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", value_Config.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = value_Config.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", value_Config.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = value_Config.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", value_Config.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var lr_Half_From = value_Config.Lr_Half_From > 0 ? new SqlParameter("@Lr_Half_From", value_Config.Lr_Half_From) : new SqlParameter("@Lr_Half_From", DBNull.Value);
            var lr_Half_To = value_Config.Lr_Half_To > 0 ? new SqlParameter("@Lr_Half_To", value_Config.Lr_Half_To) : new SqlParameter("@Lr_Half_To", DBNull.Value);
            var star_Ln_From = value_Config.Star_Ln_From > 0 ? new SqlParameter("@Star_Ln_From", value_Config.Star_Ln_From) : new SqlParameter("@Star_Ln_From", DBNull.Value);
            var star_Ln_To = value_Config.Star_Ln_To > 0 ? new SqlParameter("@Star_Ln_To", value_Config.Star_Ln_To) : new SqlParameter("@Star_Ln_To", DBNull.Value);
            var shape_Group = !string.IsNullOrEmpty(value_Config.Shape_Group) ? new SqlParameter("@Shape_Group", value_Config.Shape_Group) : new SqlParameter("@Shape_Group", DBNull.Value);
            var shape = !string.IsNullOrEmpty(value_Config.Shape) ? new SqlParameter("@Shape", value_Config.Shape) : new SqlParameter("@Shape", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Value_Config_Insert_Update @ValueMap_ID, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From, @Depth_Per_To, @Table_Per_From,
                        @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Lr_Half_From, @Lr_Half_To, @Star_Ln_From, @Star_Ln_To, @Shape_Group, @Shape", valueMap_ID, length_From, length_To,
                        width_From, width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
                        pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, lr_Half_From, lr_Half_To, star_Ln_From, star_Ln_To, shape_Group, shape));

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                if (value_Config.ValueMap_ID > 0)
                    await Insert_Value_Config_Trace(value_Config, "Update");
                else
                    await Insert_Value_Config_Trace(value_Config, "Insert");
            }

            return result;
        }
        public async Task<int> Delete_Value_Config(int valueMap_ID)
        {
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var _valueMap_ID = valueMap_ID > 0 ? new SqlParameter("@ValueMap_ID", valueMap_ID) : new SqlParameter("@ValueMap_ID", DBNull.Value);

                var result = await Task.Run(() => _dbContext.Value_Config
                                .FromSqlRaw(@"exec Value_Config_Select @ValueMap_ID", _valueMap_ID)
                                .AsEnumerable()
                                .FirstOrDefault());
                if(result != null)
                {
                    await Insert_Value_Config_Trace(result, "Delete");
                }
            }
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Value_Config_Delete {valueMap_ID}"));
        }
        public async Task<IList<Value_Config>> Get_Value_Config(int valueMap_ID)
        {
            var _valueMap_ID = valueMap_ID > 0 ? new SqlParameter("@ValueMap_ID", valueMap_ID) : new SqlParameter("@ValueMap_ID", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Value_Config
                            .FromSqlRaw(@"exec Value_Config_Select @ValueMap_ID", _valueMap_ID).ToListAsync());

            return result;
        }
        #endregion
    }
}
