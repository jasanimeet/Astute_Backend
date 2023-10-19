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
    public partial class ProcessService : IProcessService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public ProcessService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Process_Master_Trace(Process_Master process_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var processName = new SqlParameter("@Process_Name", process_Master.Process_Name);
            var processType = new SqlParameter("@Process_Type", process_Master.Process_Type);
            var orderNo = new SqlParameter("@Order_No", process_Master.Order_No);
            var sortNo = new SqlParameter("@Sort_No", process_Master.Sort_No);
            var status = new SqlParameter("@status", process_Master.status);

            await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Process_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Process_Name, @Process_Type, @Order_No, @Sort_No, @status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                                empId, ipaddress, date, time, record_Type, processName, processType, orderNo, sortNo, status));
        }
        #endregion

        #region Methods
        public async Task<int> InsertProcessMas(Process_Master process_Mas)
        {
            var processId = new SqlParameter("@Process_Id", process_Mas.Process_Id);
            var processName = new SqlParameter("@Process_Name", process_Mas.Process_Name);
            var processType = new SqlParameter("@Process_Type", process_Mas.Process_Type);
            var orderNo = new SqlParameter("@Order_No", process_Mas.Order_No);
            var sortNo = new SqlParameter("@Sort_No", process_Mas.Sort_No);
            var status = new SqlParameter("@status", process_Mas.status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", process_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"exec Process_Mas_Insert_Update @Process_Id, @Process_Name, @Process_Type, @Order_No, @Sort_No, @status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                                processId, processName, processType, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Process_Master_Trace(process_Mas, "Insert");
            }

            return result;
        }
        public async Task<int> UpdateProcessMas(Process_Master process_Mas)
        {
            var processId = new SqlParameter("@Process_Id", process_Mas.Process_Id);
            var processName = new SqlParameter("@Process_Name", process_Mas.Process_Name);
            var processType = new SqlParameter("@Process_Type", process_Mas.Process_Type);
            var orderNo = new SqlParameter("@Order_No", process_Mas.Order_No);
            var sortNo = new SqlParameter("@Sort_No", process_Mas.Sort_No);
            var status = new SqlParameter("@status", process_Mas.status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", process_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"exec Process_Mas_Insert_Update @Process_Id, @Process_Name, @Process_Type, @Order_No, @Sort_No, @status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                                processId, processName, processType, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_Process_Master_Trace(process_Mas, "Update");
            }

            return result;
        }
        public async Task<int> DeleteProcessMas(int proccessId)
        {
            var process_Id = new SqlParameter("@Process_Id", proccessId);
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var process_Master = await Task.Run(() => _dbContext.Process_Master
                                .FromSqlRaw(@"exec Process_Mas_Select @Process_Id", process_Id)
                                .AsEnumerable()
                                .FirstOrDefault());
                if(process_Master != null)
                {
                    await Insert_Process_Master_Trace(process_Master, "Delete");
                }
            }

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Process_Mas_Delete @Process_Id", process_Id);

            return result;
        }
        public async Task<IList<Process_Master>> GetProcess(int processId)
        {
            var param = processId > 0 ? new SqlParameter("@Process_Id", processId) : new SqlParameter("@Process_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Process_Master
                            .FromSqlRaw(@"exec Process_Mas_Select @Process_Id", param)
                            .ToListAsync());
            return result;
        }
        public async Task<int> StateChangeStatus(int process_Id, bool status)
        {
            var processId = new SqlParameter("@Process_Id", process_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Process_Master_Update_Status @Process_Id, @Status", processId, Status));
            return result;
        }
        #endregion
    }
}
