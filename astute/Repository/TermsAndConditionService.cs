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
    public partial class TermsAndConditionService : ITermsAndConditionService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public TermsAndConditionService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_TermsAndCondition_Trace(TermsAndCondition termsAndCondition, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var conditions = !string.IsNullOrEmpty(termsAndCondition.Conditions) ? new SqlParameter("@Conditions", termsAndCondition.Conditions) : new SqlParameter("@Conditions", DBNull.Value);
            var process_Id = termsAndCondition.Process_Id > 0 ? new SqlParameter("@Process_Id", termsAndCondition.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var start_Date = !string.IsNullOrEmpty(termsAndCondition.Start_Date) ? new SqlParameter("@Start_Date", termsAndCondition.Start_Date) : new SqlParameter("@Start_Date", DBNull.Value);
            var order_No = termsAndCondition.Order_No > 0 ? new SqlParameter("@Order_No", termsAndCondition.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sort_No = termsAndCondition.Sort_No > 0 ? new SqlParameter("@Sort_No", termsAndCondition.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", termsAndCondition.Status);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC TermsAndCondition_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Conditions, @Process_Id, @Start_Date, @Order_No,
            @Sort_No, @Status", empId, ipaddress, date, time, record_Type, conditions, process_Id, start_Date, order_No, sort_No, status));
        }
        #endregion

        #region Methods
        public async Task<int> AddUpdateTermsAndCondition(TermsAndCondition termsAndCondition)
        {
            var condition_Id = new SqlParameter("@Condition_Id", termsAndCondition.Condition_Id);
            var conditions = !string.IsNullOrEmpty(termsAndCondition.Conditions) ? new SqlParameter("@Conditions", termsAndCondition.Conditions) : new SqlParameter("@Conditions", DBNull.Value);
            var process_Id = termsAndCondition.Process_Id > 0 ? new SqlParameter("@Process_Id", termsAndCondition.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var start_Date = !string.IsNullOrEmpty(termsAndCondition.Start_Date) ? new SqlParameter("@Start_Date", termsAndCondition.Start_Date) : new SqlParameter("@Start_Date", DBNull.Value);
            var order_No = termsAndCondition.Order_No > 0 ? new SqlParameter("@Order_No", termsAndCondition.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sort_No = termsAndCondition.Sort_No > 0 ? new SqlParameter("@Sort_No", termsAndCondition.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", termsAndCondition.Status);
            var isForce_Insert = new SqlParameter("@IsForceInsert", termsAndCondition.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
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

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    if (termsAndCondition.Condition_Id > 0)
            //        await Insert_TermsAndCondition_Trace(termsAndCondition, "Update");
            //    else
            //        await Insert_TermsAndCondition_Trace(termsAndCondition, "Insert");

            //}

            return result;
        }
        public async Task<int> DeleteTermsAndCondition(int condition_Id)
        {
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var conditionId = condition_Id > 0 ? new SqlParameter("@Condition_Id", condition_Id) : new SqlParameter("@Condition_Id", DBNull.Value);
            //    var result = await Task.Run(() => _dbContext.TermsAndCondition
            //                .FromSqlRaw(@"exec TermsAndCondition_Select @Condition_Id, @Process_Id", conditionId, new SqlParameter("@Process_Id", DBNull.Value))
            //                .AsEnumerable()
            //                .FirstOrDefault());
            //    if (result != null)
            //    {
            //        await Insert_TermsAndCondition_Trace(result, "Delete");
            //    }
            //}
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
        public async Task<int> Get_TermsAndCondition_Max_Order_No()
        {
            var result = await _dbContext.TermsAndCondition.Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
        public async Task<List<Dictionary<string, object>>> Get_TermsAndCondition_By_Process(int process_Id, string trans_Date)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[TermsAndCondition_By_Process_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(process_Id > 0 ? new SqlParameter("@Process_Id", process_Id) : new SqlParameter("@Process_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(trans_Date) ? new SqlParameter("@Trans_Date", trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_TermsAndCondition_By_Trans_Id(int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[TermsAndCondition_By_Trans_Id_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(Trans_Id > 0 ? new SqlParameter("@Trans_Id", Trans_Id) : new SqlParameter("@Trans_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
