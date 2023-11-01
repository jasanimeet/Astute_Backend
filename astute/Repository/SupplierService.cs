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

        #region Supplier Column And Value
        public async Task<int> Insert_Update_Supplier_Value_Mapping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblSupplier_Value_Mapping", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Value_Mapping_Table_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @tblSupplier_Value_Mapping", parameter));

            return result;
        }
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
        public async Task<IList<Supplier_Value_Mapping>> Get_Supplier_Value_Mapping(int sup_Id, int col_Id)
        {
            var _sup_Id = sup_Id > 0 ? new SqlParameter("@Sup_Id", sup_Id) : new SqlParameter("@Sup_Id", DBNull.Value);
            var _col_Id = col_Id > 0 ? new SqlParameter("@Col_Id", col_Id) : new SqlParameter("@Col_Id", DBNull.Value);

            var categoryValue = await Task.Run(() => _dbContext.Supplier_Value_Mapping
                            .FromSqlRaw(@"exec Supplier_Value_Mapping_Select @Sup_Id, @Col_Id", _sup_Id, _col_Id).ToListAsync());

            return categoryValue;
        }
        public async Task<int> Add_Update_Supplier_Column_Mapping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@supplier_column", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Column_Mapping_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Column_Mapping_Insert_Update @supplier_column", parameter));
            return result;
        }
        public async Task<IList<Supplier_Column_Mapping>> Get_Supplier_Column_Mapping(int supp_Id, string map_Flag)
        {
            var _supp_Id = supp_Id > 0 ? new SqlParameter("@Supp_Id", supp_Id) : new SqlParameter("@Supp_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Supplier_Column_Mapping
                            .FromSqlRaw(@"exec Supplier_Column_Mapping_Select @Supp_Id, @Map_Flag", _supp_Id, _map_Flag)
                            .ToListAsync());
            return result;
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

        #region Supplier Pricing
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Pricing_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing_Id));
                    command.Parameters.Add(new SqlParameter("@Supplier_Id", supplier_Id));

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

            return result;
        }
        public async Task<int> Add_Update_Supplier_Pricing(Supplier_Pricing supplier_Pricing)
        {
            var supplier_Pricing_Id = new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing.Supplier_Pricing_Id);
            var supplier_Id = supplier_Pricing.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Pricing.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var shape = !string.IsNullOrEmpty(supplier_Pricing.Shape) ? new SqlParameter("@Shape", supplier_Pricing.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var cts = !string.IsNullOrEmpty(supplier_Pricing.Cts) ? new SqlParameter("@Cts", supplier_Pricing.Cts) : new SqlParameter("@Cts", DBNull.Value);
            var color = !string.IsNullOrEmpty(supplier_Pricing.Color) ? new SqlParameter("@Color", supplier_Pricing.Color) : new SqlParameter("@Color", DBNull.Value);
            var clarity = !string.IsNullOrEmpty(supplier_Pricing.Clarity) ? new SqlParameter("@Clarity", supplier_Pricing.Clarity) : new SqlParameter("@Clarity", DBNull.Value);
            var cut = !string.IsNullOrEmpty(supplier_Pricing.Cut) ? new SqlParameter("@Cut", supplier_Pricing.Cut) : new SqlParameter("@Cut", DBNull.Value);
            var polish = !string.IsNullOrEmpty(supplier_Pricing.Polish) ? new SqlParameter("@Polish", supplier_Pricing.Polish) : new SqlParameter("@Polish", DBNull.Value);
            var symm = !string.IsNullOrEmpty(supplier_Pricing.Symm) ? new SqlParameter("@Symm", supplier_Pricing.Symm) : new SqlParameter("@Symm", DBNull.Value);
            var flour = !string.IsNullOrEmpty(supplier_Pricing.Flour) ? new SqlParameter("@Flour", supplier_Pricing.Flour) : new SqlParameter("@Flour", DBNull.Value);
            var lab = !string.IsNullOrEmpty(supplier_Pricing.Lab) ? new SqlParameter("@Lab", supplier_Pricing.Lab) : new SqlParameter("@Lab", DBNull.Value);
            var shade = !string.IsNullOrEmpty(supplier_Pricing.Shade) ? new SqlParameter("@Shade", supplier_Pricing.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var luster = !string.IsNullOrEmpty(supplier_Pricing.Luster) ? new SqlParameter("@Luster", supplier_Pricing.Luster) : new SqlParameter("@Luster", DBNull.Value);
            var location = !string.IsNullOrEmpty(supplier_Pricing.Location) ? new SqlParameter("@Location", supplier_Pricing.Location) : new SqlParameter("@Location", DBNull.Value);
            var status = !string.IsNullOrEmpty(supplier_Pricing.Status) ? new SqlParameter("@Status", supplier_Pricing.Status) : new SqlParameter("@Status", DBNull.Value);
            var base_Disc_Per = !string.IsNullOrEmpty(supplier_Pricing.Base_Disc_Per) ? new SqlParameter("@Base_Disc_Per", supplier_Pricing.Base_Disc_Per) : new SqlParameter("@Base_Disc_Per", DBNull.Value);
            var base_Amt = !string.IsNullOrEmpty(supplier_Pricing.Base_Amt) ? new SqlParameter("@Base_Amt", supplier_Pricing.Base_Amt) : new SqlParameter("@Base_Amt", DBNull.Value);
            var final_Disc_Per = !string.IsNullOrEmpty(supplier_Pricing.Final_Disc_Per) ? new SqlParameter("@Final_Disc_Per", supplier_Pricing.Final_Disc_Per) : new SqlParameter("@Final_Disc_Per", DBNull.Value);
            var final_Amt = !string.IsNullOrEmpty(supplier_Pricing.Final_Amt) ? new SqlParameter("@Final_Amt", supplier_Pricing.Final_Amt) : new SqlParameter("@Final_Amt", DBNull.Value);
            var supplier_Filter_Type = !string.IsNullOrEmpty(supplier_Pricing.Supplier_Filter_Type) ? new SqlParameter("@Supplier_Filter_Type", supplier_Pricing.Supplier_Filter_Type) : new SqlParameter("@Supplier_Filter_Type", DBNull.Value);
            var calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.Calculation_Type) ? new SqlParameter("@Calculation_Type", supplier_Pricing.Calculation_Type) : new SqlParameter("@Calculation_Type", DBNull.Value);
            var sign = !string.IsNullOrEmpty(supplier_Pricing.Sign) ? new SqlParameter("@Sign", supplier_Pricing.Sign) : new SqlParameter("@Sign", DBNull.Value);
            var value_1 = supplier_Pricing.Value_1 > 0 ? new SqlParameter("@Value_1", supplier_Pricing.Value_1) : new SqlParameter("@Value_1", DBNull.Value);
            var value_2 = supplier_Pricing.Value_2 > 0 ? new SqlParameter("@Value_2", supplier_Pricing.Value_2) : new SqlParameter("@Value_2", DBNull.Value);
            var value_3 = supplier_Pricing.Value_3 > 0 ? new SqlParameter("@Value_3", supplier_Pricing.Value_3) : new SqlParameter("@Value_3", DBNull.Value);
            var value_4 = supplier_Pricing.Value_4 > 0 ? new SqlParameter("@Value_4", supplier_Pricing.Value_4) : new SqlParameter("@Value_4", DBNull.Value);
            var sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.SP_Calculation_Type) ? new SqlParameter("@SP_Calculation_Type", supplier_Pricing.SP_Calculation_Type) : new SqlParameter("@SP_Calculation_Type", DBNull.Value);
            var sp_sign = !string.IsNullOrEmpty(supplier_Pricing.SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Date) ? new SqlParameter("@SP_Start_Date", supplier_Pricing.SP_Start_Date) : new SqlParameter("@SP_Start_Date", DBNull.Value);
            var sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Time) ? new SqlParameter("@SP_Start_Time", supplier_Pricing.SP_Start_Time) : new SqlParameter("@SP_Start_Time", DBNull.Value);
            var sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Date) ? new SqlParameter("@SP_End_Date", supplier_Pricing.SP_End_Date) : new SqlParameter("@SP_End_Date", DBNull.Value);
            var sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Time) ? new SqlParameter("@SP_End_Time", supplier_Pricing.SP_End_Time) : new SqlParameter("@SP_End_Time", DBNull.Value);
            var sp_value_1 = supplier_Pricing.SP_Value_1 > 0 ? new SqlParameter("@SP_Value_1", supplier_Pricing.SP_Value_1) : new SqlParameter("@SP_Value_1", DBNull.Value);
            var sp_value_2 = supplier_Pricing.SP_Value_2 > 0 ? new SqlParameter("@SP_Value_2", supplier_Pricing.SP_Value_2) : new SqlParameter("@SP_Value_2", DBNull.Value);
            var sp_value_3 = supplier_Pricing.SP_Value_3 > 0 ? new SqlParameter("@SP_Value_3", supplier_Pricing.SP_Value_3) : new SqlParameter("@SP_Value_3", DBNull.Value);
            var sp_value_4 = supplier_Pricing.SP_Value_4 > 0 ? new SqlParameter("@SP_Value_4", supplier_Pricing.SP_Value_4) : new SqlParameter("@SP_Value_4", DBNull.Value);
            var ms_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_Calculation_Type) ? new SqlParameter("@MS_Calculation_Type", supplier_Pricing.MS_Calculation_Type) : new SqlParameter("@MS_Calculation_Type", DBNull.Value);
            var ms_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_Sign) ? new SqlParameter("@MS_Sign", supplier_Pricing.MS_Sign) : new SqlParameter("@MS_Sign", DBNull.Value);
            var ms_value_1 = supplier_Pricing.MS_Value_1 > 0 ? new SqlParameter("@MS_Value_1", supplier_Pricing.MS_Value_1) : new SqlParameter("@MS_Value_1", DBNull.Value);
            var ms_value_2 = supplier_Pricing.MS_Value_2 > 0 ? new SqlParameter("@MS_Value_2", supplier_Pricing.MS_Value_2) : new SqlParameter("@MS_Value_2", DBNull.Value);
            var ms_value_3 = supplier_Pricing.MS_Value_3 > 0 ? new SqlParameter("@MS_Value_3", supplier_Pricing.MS_Value_3) : new SqlParameter("@MS_Value_3", DBNull.Value);
            var ms_value_4 = supplier_Pricing.MS_Value_4 > 0 ? new SqlParameter("@MS_Value_4", supplier_Pricing.MS_Value_4) : new SqlParameter("@MS_Value_4", DBNull.Value);
            var ms_sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Calculation_Type) ? new SqlParameter("@MS_SP_Calculation_Type", supplier_Pricing.MS_SP_Calculation_Type) : new SqlParameter("@MS_SP_Calculation_Type", DBNull.Value);
            var ms_sp_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.MS_SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var ms_sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Date) ? new SqlParameter("@MS_SP_Start_Date", supplier_Pricing.MS_SP_Start_Date) : new SqlParameter("@MS_SP_Start_Date", DBNull.Value);
            var ms_sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Time) ? new SqlParameter("@MS_SP_Start_Time", supplier_Pricing.MS_SP_Start_Time) : new SqlParameter("@MS_SP_Start_Time", DBNull.Value);
            var ms_sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Date) ? new SqlParameter("@MS_SP_End_Date", supplier_Pricing.MS_SP_End_Date) : new SqlParameter("@MS_SP_End_Date", DBNull.Value);
            var ms_sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Time) ? new SqlParameter("@MS_SP_End_Time", supplier_Pricing.MS_SP_End_Time) : new SqlParameter("@MS_SP_End_Time", DBNull.Value);
            var ms_sp_value_1 = supplier_Pricing.MS_SP_Value_1 > 0 ? new SqlParameter("@MS_SP_Value_1", supplier_Pricing.MS_SP_Value_1) : new SqlParameter("@MS_SP_Value_1", DBNull.Value);
            var ms_sp_value_2 = supplier_Pricing.MS_SP_Value_2 > 0 ? new SqlParameter("@MS_SP_Value_2", supplier_Pricing.MS_SP_Value_2) : new SqlParameter("@MS_SP_Value_2", DBNull.Value);
            var ms_sp_value_3 = supplier_Pricing.MS_SP_Value_3 > 0 ? new SqlParameter("@MS_SP_Value_3", supplier_Pricing.MS_SP_Value_3) : new SqlParameter("@MS_SP_Value_3", DBNull.Value);
            var ms_sp_value_4 = supplier_Pricing.MS_SP_Value_4 > 0 ? new SqlParameter("@MS_SP_Value_4", supplier_Pricing.MS_SP_Value_4) : new SqlParameter("@MS_SP_Value_4", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Insert_Update @Supplier_Pricing_Id, @Supplier_Id, @Shape, @Cts, @Color, @Clarity, @Cut, @Polish, @Symm, @Flour, @Lab,
                        @Shade, @Luster, @Location, @Status, @Base_Disc_Per, @Base_Amt, @Final_Disc_Per, @Final_Amt, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4, @SP_Calculation_Type, @SP_Sign,
                        @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type, @MS_Sign, @MS_Value_1,
                        @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time, @MS_SP_Value_1,
                        @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4", supplier_Pricing_Id, supplier_Id, shape, cts, color, clarity, cut, polish, symm, flour, lab, shade, luster,
                        location, status, base_Disc_Per, base_Amt, final_Disc_Per, final_Amt, supplier_Filter_Type, calculation_Type, sign, value_1, value_2, value_3, value_4,
                        sp_calculation_Type, sp_sign, sp_start_date, sp_start_time, sp_end_time, sp_end_time, sp_value_1, sp_value_2, sp_value_3, sp_value_4, ms_calculation_Type,
                        ms_sign, ms_value_1, ms_value_2, ms_value_3, ms_value_4, ms_sp_calculation_Type, ms_sp_sign, ms_sp_start_date, ms_sp_start_time, ms_sp_end_date, ms_sp_end_time,
                        ms_sp_value_1, ms_sp_value_2, ms_sp_value_3, ms_sp_value_4));

            return result;
        }
        #endregion

        #region Supplier Stock
        public async Task<(string, int)> Stock_Data_Insert_Update(Stock_Data_Master stock_Data_Master)
        {
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Master.Stock_Data_Id);
            var supplier_Id = stock_Data_Master.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Data_Master.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var upload_Method = !string.IsNullOrEmpty(stock_Data_Master.Upload_Method) ? new SqlParameter("@Upload_Method", stock_Data_Master.Upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Data_Master_Insert_Update @Stock_Data_Id, @Supplier_Id, @Upload_Method, @Inserted_Id OUT", stock_Data_Id, supplier_Id, upload_Method, inserted_Id));

            int _insertedId = (int)inserted_Id.Value;
            if(_insertedId > 0)
            {
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<int> Stock_Data_Detail_Insert_Update(DataTable dataTable, int Stock_Data_Id)
        {
            var parameter = new SqlParameter("@Stock_data", SqlDbType.Structured)
            {
                TypeName = "dbo.[Stock_Data_Type]",
                Value = dataTable
            };
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", Stock_Data_Id);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec [Stock_Data_Details_Insert_Update] @Stock_data,@Stock_Data_Id", parameter, stock_Data_Id));

            return result;
        }
        public async Task<IList<Stock_Data_Column_Value>> Get_Stock_Data_Distinct_Column_Values(string column_Name)
        {
            var _column_Name = !string.IsNullOrEmpty(column_Name) ? new SqlParameter("@Column_Name", column_Name) : new SqlParameter("@Column_Name", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Stock_Data_Column_Value
                            .FromSqlRaw(@"exec Stock_Data_Distinct_Column_Value_Select @Column_Name", _column_Name).ToListAsync());

            return result;
        }
        #endregion
    }
}
