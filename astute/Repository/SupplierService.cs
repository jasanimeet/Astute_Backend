using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    }
}
