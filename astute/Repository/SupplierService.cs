using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        #endregion

        #region Ctor
        public SupplierService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
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
            var length = value_Config.Length > 0 ? new SqlParameter("@Length", value_Config.Length) : new SqlParameter("@Length", DBNull.Value);
            var width = value_Config.Width > 0 ? new SqlParameter("@Width", value_Config.Width) : new SqlParameter("@Width", DBNull.Value);
            var depth_Per = value_Config.Depth_Per > 0 ? new SqlParameter("@Depth_Per", value_Config.Depth_Per) : new SqlParameter("@Depth_Per", DBNull.Value);
            var table_Per = value_Config.Table_Per > 0 ? new SqlParameter("@Table_Per", value_Config.Table_Per) : new SqlParameter("@Table_Per", DBNull.Value);
            var crown_Angle = value_Config.Crown_Angle > 0 ? new SqlParameter("@Crown_Angle", value_Config.Crown_Angle) : new SqlParameter("@Crown_Angle", DBNull.Value);
            var crown_Height = value_Config.Crown_Height > 0 ? new SqlParameter("@Crown_Height", value_Config.Crown_Angle) : new SqlParameter("@Crown_Height", DBNull.Value);
            var pavilion_Angle = value_Config.Pavilion_Angle > 0 ? new SqlParameter("@Pavilion_Angle", value_Config.Pavilion_Angle) : new SqlParameter("@Pavilion_Angle", DBNull.Value);
            var pavilion_Height = value_Config.Pavilion_Height > 0 ? new SqlParameter("@Pavilion_Height", value_Config.Pavilion_Height) : new SqlParameter("@Pavilion_Height", DBNull.Value);
            var girdle_Per = value_Config.Girdle_Per > 0 ? new SqlParameter("@Girdle_Per", value_Config.Girdle_Per) : new SqlParameter("@Girdle_Per", DBNull.Value);
            var lr_Half = value_Config.Lr_Half > 0 ? new SqlParameter("@Lr_Half", value_Config.Lr_Half) : new SqlParameter("@Lr_Half", DBNull.Value);
            var star_Ln = value_Config.Star_Ln > 0 ? new SqlParameter("@Star_Ln", value_Config.Star_Ln) : new SqlParameter("@Star_Ln", DBNull.Value);
            var shape_Group = !string.IsNullOrEmpty(value_Config.Shape_Group) ? new SqlParameter("@Shape_Group", value_Config.Shape_Group) : new SqlParameter("@Shape_Group", DBNull.Value);
            var shape = !string.IsNullOrEmpty(value_Config.Shape) ? new SqlParameter("@Shape", value_Config.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var trans_Date = !string.IsNullOrEmpty(value_Config.Trans_Date) ? new SqlParameter("@Trans_Date", value_Config.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Value_Config_Insert_Update @ValueMap_ID, @Length, @Width, @Depth_Per, @Table_Per, @Crown_Angle, @Crown_Height, @Pavilion_Angle, 
                        @Pavilion_Height, @Girdle_Per, @Lr_Half, @Star_Ln, @Shape_Group, @Shape, @Trans_Date", valueMap_ID, length, width, depth_Per, table_Per, crown_Angle, crown_Height,
                        pavilion_Angle, pavilion_Height, girdle_Per, lr_Half, star_Ln, shape_Group, shape, trans_Date));

            return result;
        }
        public async Task<int> Delete_Value_Config(int valueMap_ID)
        {
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
