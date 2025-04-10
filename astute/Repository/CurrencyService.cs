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
    public partial class CurrencyService : ICurrencyService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public CurrencyService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Currency_Master_Trace(Currency_Master currency_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var currency = new SqlParameter("@Currency", currency_Master.Currency);
            var currencyName = new SqlParameter("@Currency_Name", currency_Master.Currency_Name);
            var symbol = new SqlParameter("@Symbol", currency_Master.Symbol);
            var orderNo = new SqlParameter("@Order_No", currency_Master.Order_No);
            var sortNo = new SqlParameter("@Sort_No", currency_Master.Sort_No);
            var status = new SqlParameter("@status", currency_Master.status);

            await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Currency_Master_Trace_Insert @Employee_Id, @IP_Address,@Trace_Date, @Trace_Time, @Record_Type, @Currency, @Currency_Name, 
                                @Symbol, @Order_No, @Sort_No, @status", empId, ipaddress, date, time, record_Type, currency, currencyName, symbol, orderNo, sortNo, status));
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
            var isExistCurrency = new SqlParameter("@IsExistCurrency", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                               .ExecuteSqlRawAsync(@"exec Currency_Mas_Insert_Update @Currency_Id, @Currency, @Currency_Name, @Symbol, @Order_No, @Sort_No, @status, @recordType, @IsExistCurrency OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                               currency_Id, currency, currencyName, symbol, orderNo, sortNo, status, recordType, isExistCurrency, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool currencyIsExist = (bool)isExistCurrency.Value;
            if (currencyIsExist)
                return 4;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Currency_Master_Trace(currency_Mas, "Insert");
            //}

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
            var isExistCurrency = new SqlParameter("@IsExistCurrency", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                               .ExecuteSqlRawAsync(@"exec Currency_Mas_Insert_Update @Currency_Id, @Currency, @Currency_Name, @Symbol, @Order_No, @Sort_No, @status, @recordType, @IsExistCurrency OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                               currency_Id, currency, currencyName, symbol, orderNo, sortNo, status, recordType, isExistCurrency, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool currencyIsExist = (bool)isExistCurrency.Value;
            if (currencyIsExist)
                return 4;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Currency_Master_Trace(currency_Mas, "Update");
            //}

            return result;
        }
        public async Task<int> DeleteCurrency(int currency_Id)
        {
            var isReferencedParameter = new SqlParameter("@IsReferenced", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
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
        public async Task<IList<Currency_Master>> Get_Active_Currency(int currency_Id)
        {
            var param = currency_Id > 0 ? new SqlParameter("@Currency_Id", currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Currency_Master
                            .FromSqlRaw(@"exec Currency_Mas_Select_Active_Currency @Currency_Id", param)
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
        public async Task<int> Get_Currency_Max_Order_No()
        {
            var result = await _dbContext.Currency_Master.Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
        #endregion
    }
}
