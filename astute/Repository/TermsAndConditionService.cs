using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class TermsAndConditionService : ITermsAndConditionService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public TermsAndConditionService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> AddUpdateTermsAndCondition(TermsAndCondition termsAndCondition)
        {
            var condition_Id = new SqlParameter("@Condition_Id", termsAndCondition.Condition_Id);
            var conditions = !string.IsNullOrEmpty(termsAndCondition.Conditions) ? new SqlParameter("@Conditions", termsAndCondition.Conditions) : new SqlParameter("@Conditions", DBNull.Value);
            var process_Id = termsAndCondition.Process_Id > 0 ? new SqlParameter("@Process_Id", termsAndCondition.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var start_Date = !termsAndCondition.Start_Date.Equals(null) ? new SqlParameter("@Start_Date", termsAndCondition.Start_Date) : new SqlParameter("@Start_Date", DBNull.Value);
            var order_No = termsAndCondition.Order_No > 0 ? new SqlParameter("@Order_No", termsAndCondition.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sort_No = termsAndCondition.Sort_No > 0 ? new SqlParameter("@Sort_No", termsAndCondition.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", termsAndCondition.Status);
            var isForce_Insert = new SqlParameter("@IsForceInsert", termsAndCondition.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC TermsAndCondition_Insert_Update @Condition_Id, @Conditions, @Process_Id, @Start_Date, @Order_No, @Sort_No, @Status, 
                        @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert", condition_Id, conditions, process_Id, start_Date, order_No, sort_No, status, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> DeleteTermsAndCondition(int condition_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"TermsAndCondition_Delete {condition_Id}"));
        }
        public async Task<IList<TermsAndCondition>> GetTermsAndCondition(int condition_Id, int process_Id)
        {
            var conditionId = condition_Id > 0 ? new SqlParameter("@Condition_Id", condition_Id) : new SqlParameter("@Condition_Id", DBNull.Value);
            var processId = process_Id > 0 ? new SqlParameter("@Process_Id", process_Id) : new SqlParameter("@Process_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.TermsAndCondition
                            .FromSqlRaw(@"exec TermsAndCondition_Select @Condition_Id, @Process_Id", conditionId, processId).ToListAsync());

            return result;
        }
        public async Task<int> TermsAndConditionChangeStatus(int condition_Id, bool status)
        {
            var conditionId = new SqlParameter("@Condition_Id", condition_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC TermsAndCondition_Update_Status @Condition_Id, @Status", conditionId, Status));
            return result;
        }
        #endregion
    }
}
