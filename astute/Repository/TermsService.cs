using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class TermsService : ITermsService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public TermsService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
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

            return result;
        }
        public async Task<int> DeleteTerms(int terms_Id)
        {
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
