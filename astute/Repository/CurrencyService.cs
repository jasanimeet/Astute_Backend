using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class CurrencyService : ICurrencyService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public CurrencyService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> InsertCurrency(Currency_Master currency_Mas)
        {
            var currency_Id = new SqlParameter("@Currency_Id", currency_Mas.Currency_Id);
            var currency = new SqlParameter("@Currency", currency_Mas.Currency);
            var currencyName = new SqlParameter("@Currency_Name", currency_Mas.Currency_Name);
            var symbol = new SqlParameter("@Symbol", currency_Mas.Symbol);
            var orderNo = new SqlParameter("@Order_No", currency_Mas.Order_No);
            var sortNo = new SqlParameter("@Sort_No", currency_Mas.Sort_No);
            var status = new SqlParameter("@status", currency_Mas.status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", currency_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                               .ExecuteSqlRawAsync(@"exec Currency_Mas_Insert_Update @Currency_Id, @Currency, @Currency_Name, @Symbol, @Order_No, @Sort_No, @status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                               currency_Id, currency, currencyName, symbol, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> UpdateCurrency(Currency_Master currency_Mas)
        {
            var currency_Id = new SqlParameter("@Currency_Id", currency_Mas.Currency_Id);
            var currency = new SqlParameter("@Currency", currency_Mas.Currency);
            var currencyName = new SqlParameter("@Currency_Name", currency_Mas.Currency_Name);
            var symbol = new SqlParameter("@Symbol", currency_Mas.Symbol);
            var orderNo = new SqlParameter("@Order_No", currency_Mas.Order_No);
            var sortNo = new SqlParameter("@Sort_No", currency_Mas.Sort_No);
            var status = new SqlParameter("@status", currency_Mas.status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", currency_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                               .ExecuteSqlRawAsync(@"exec Currency_Mas_Insert_Update @Currency_Id, @Currency, @Currency_Name, @Symbol, @Order_No, @Sort_No, @status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                               currency_Id, currency, currencyName, symbol, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> DeleteCurrency(int currency_Id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Currency_Mas_Delete @Currency_Id, @IsReferenced OUT", new SqlParameter("@Currency_Id", currency_Id),
                                isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        public async Task<IList<Currency_Master>> GetCurrency(int currency_Id)
        {
            var param = currency_Id > 0 ? new SqlParameter("@Currency_Id", currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Currency_Master
                            .FromSqlRaw(@"exec Currency_Mas_Select @Currency_Id", param)
                            .ToListAsync());
            return result;
        }
        public async Task<int> CurrencyChangeStatus(int currency_Id, bool status)
        {
            var Currency_Id = new SqlParameter("@Currency_Id", currency_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Currency_Master_Update_Status @Currency_Id, @Status", Currency_Id, Status));
            return result;
        }
        #endregion
    }
}
