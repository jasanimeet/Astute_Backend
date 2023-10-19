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
    public partial class TermsService : ITermsService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public TermsService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Terms_Master_Trace(Terms_Master terms_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var terms = new SqlParameter("@Terms", terms_Master.Terms);
            var termDays = new SqlParameter("@Term_Days", terms_Master.Term_Days);
            var orderNo = new SqlParameter("@Order_No", terms_Master.Order_No);
            var sortNo = new SqlParameter("@Sort_No", terms_Master.Sort_No);
            var status = new SqlParameter("@Status", terms_Master.Status);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Terms_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Terms, @Term_Days, @Order_No, @Sort_No, @Status",
            empId, ipaddress, date, time, record_Type, terms, termDays, orderNo, sortNo, status));
        }
        #endregion

        #region Methods
        public async Task<int> InsertTerms(Terms_Master terms_Mas)
        {
            var terms_Id = new SqlParameter("@terms_Id", terms_Mas.Terms_Id);
            var terms = new SqlParameter("@terms", terms_Mas.Terms);
            var termDays = new SqlParameter("@termDays", terms_Mas.Term_Days);
            var orderNo = new SqlParameter("@orderNo", terms_Mas.Order_No);
            var sortNo = new SqlParameter("@sortNo", terms_Mas.Sort_No);
            var status = new SqlParameter("@status", terms_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", terms_Mas.IsForceInsert);
            var isExistTerms = new SqlParameter("@IsExistTerms", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Terms_Mas_Insert_Update @terms_Id, @terms, @termDays, @orderNo, @sortNo, @status, @recordType, @IsExistTerms OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            terms_Id, terms, termDays, orderNo, sortNo, status, recordType, isExistTerms, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool termsIsExist = (bool)isExistTerms.Value;
            if (termsIsExist)
                return 2;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Terms_Master_Trace(terms_Mas, "Insert");
            }

            return result;
        }
        public async Task<int> UpdateTerms(Terms_Master terms_Mas)
        {
            var terms_Id = new SqlParameter("@terms_Id", terms_Mas.Terms_Id);
            var terms = new SqlParameter("@terms", terms_Mas.Terms);
            var termDays = new SqlParameter("@termDays", terms_Mas.Term_Days);
            var orderNo = new SqlParameter("@orderNo", terms_Mas.Order_No);
            var sortNo = new SqlParameter("@sortNo", terms_Mas.Sort_No);
            var status = new SqlParameter("@status", terms_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", terms_Mas.IsForceInsert);
            var isExistTerms = new SqlParameter("@IsExistTerms", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Terms_Mas_Insert_Update @terms_Id, @terms, @termDays, @orderNo, @sortNo, @status, @recordType, @IsExistTerms OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            terms_Id, terms, termDays, orderNo, sortNo, status, recordType, isExistTerms, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool termsIsExist = (bool)isExistTerms.Value;
            if (termsIsExist)
                return 2;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Terms_Master_Trace(terms_Mas, "Update");
            }

            return result;
        }
        public async Task<int> DeleteTerms(int terms_Id)
        {
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var _terms_Id = terms_Id > 0 ? new SqlParameter("@terms_Id", terms_Id) : new SqlParameter("@terms_Id", DBNull.Value);
                var result = await Task.Run(() => _dbContext.Terms_Master
                                .FromSqlRaw(@"exec Terms_Mas_Select @terms_Id", _terms_Id)
                                .AsEnumerable()
                                .FirstOrDefault());
                if (result != null)
                {
                    await Insert_Terms_Master_Trace(result, "Delete");
                }
            }

            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Terms_Mas_Delete {terms_Id}"));
        }
        public async Task<IList<Terms_Master>> GetTerms(int terms_Id)
        {
            var _terms_Id = terms_Id > 0 ? new SqlParameter("@terms_Id", terms_Id) : new SqlParameter("@terms_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Terms_Master
                            .FromSqlRaw(@"exec Terms_Mas_Select @terms_Id", _terms_Id)
                            .ToListAsync());
            return result;
        }
        public async Task<IList<Terms_Master>> Get_Active_Terms(int terms_Id)
        {
            var _terms_Id = terms_Id > 0 ? new SqlParameter("@terms_Id", terms_Id) : new SqlParameter("@terms_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Terms_Master
                            .FromSqlRaw(@"exec Terms_Mas_Active_Select @terms_Id", _terms_Id)
                            .ToListAsync());
            return result;
        }
        public async Task<int> TermsChangeStatus(int terms_Id, bool status)
        {
            var _terms_Id = new SqlParameter("@terms_Id", terms_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Terms_Master_Update_Status @terms_Id, @Status", _terms_Id, Status));
            return result;
        }
        #endregion
    }
}
