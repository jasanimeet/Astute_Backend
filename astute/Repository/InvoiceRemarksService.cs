using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class InvoiceRemarksService : IInvoiceRemarksService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public InvoiceRemarksService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Invoice_Remarks_Trace(Invoice_Remarks invoice_Remarks, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var processId = invoice_Remarks.Process_Id > 0 ? new SqlParameter("@Process_Id", invoice_Remarks.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(invoice_Remarks.Remarks) ? new SqlParameter("@Remarks", invoice_Remarks.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var startDate = new SqlParameter("@Start_Date", invoice_Remarks.Start_Date);
            var orderNo = invoice_Remarks.Order_No > 0 ? new SqlParameter("@Order_No", invoice_Remarks.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = invoice_Remarks.Sort_No > 0 ? new SqlParameter("@Sort_No", invoice_Remarks.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", invoice_Remarks.Status);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Bank_Master_Trace_Insert @Employee_Id, @IP_Address,@Trace_Date, @Trace_Time, @Record_Type, @Process_Id, @Remarks, @Start_Date,
            @Order_No, @Sort_No, @Status", empId, ipaddress, date, time, record_Type, processId, remarks, startDate, orderNo, sortNo, status));
        }
        #endregion

        #region Methods
        public async Task<int> InsertInvoiceRemarks(Invoice_Remarks invoice_Remarks)
        {
            var processId = invoice_Remarks.Process_Id > 0 ? new SqlParameter("@Process_Id", invoice_Remarks.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(invoice_Remarks.Remarks) ? new SqlParameter("@Remarks", invoice_Remarks.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var startDate = new SqlParameter("@Start_Date", invoice_Remarks.Start_Date);
            var orderNo = invoice_Remarks.Order_No > 0 ? new SqlParameter("@Order_No", invoice_Remarks.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = invoice_Remarks.Sort_No > 0 ? new SqlParameter("@Sort_No", invoice_Remarks.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", invoice_Remarks.Status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", invoice_Remarks.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Invoice_Remarks_Insert_Update @Process_Id, @Remarks, @Start_Date, @Order_No, @Sort_No, @Status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                                processId, remarks, startDate, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Invoice_Remarks_Trace(invoice_Remarks, "Insert");
            //}

            return result;
        }
        public async Task<int> UpdateInvoiceRemarks(Invoice_Remarks invoice_Remarks)
        {
            var processId = invoice_Remarks.Process_Id > 0 ? new SqlParameter("@Process_Id", invoice_Remarks.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(invoice_Remarks.Remarks) ? new SqlParameter("@Remarks", invoice_Remarks.Remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var startDate = new SqlParameter("@Start_Date", invoice_Remarks.Start_Date);
            var orderNo = invoice_Remarks.Order_No > 0 ? new SqlParameter("@Order_No", invoice_Remarks.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = invoice_Remarks.Sort_No > 0 ? new SqlParameter("@Sort_No", invoice_Remarks.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", invoice_Remarks.Status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", invoice_Remarks.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Invoice_Remarks_Insert_Update @Process_Id, @Remarks, @Start_Date, @Order_No, @Sort_No, @Status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
                                processId, remarks, startDate, orderNo, sortNo, status, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Invoice_Remarks_Trace(invoice_Remarks, "Update");
            //}

            return result;
        }
        public async Task<int> DeleteInvoiceRemarks(int processId, DateTime startDate)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Process_Id", processId));
            parameter.Add(new SqlParameter("@Start_Date", startDate));

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var invoice_Remarks = await Task.Run(() => _dbContext.Invoice_Remarks
            //                    .FromSqlRaw(@"exec Invoice_Remarks_Select @Process_Id, @Start_Date", parameter.ToArray())
            //                    .AsEnumerable()
            //                    .FirstOrDefault());
            //    if (invoice_Remarks != null)
            //    {
            //        await Insert_Invoice_Remarks_Trace(invoice_Remarks, "Delete");
            //    }
            //}
            var result = await Task.Run(() => _dbContext.Database
                         .ExecuteSqlRawAsync(@"exec Invoice_Remarks_Delete @Process_Id, @Start_Date", parameter.ToArray()));

            return result;
        }
        public async Task<IList<Invoice_Remarks>> GetInvoiceRemarks(int processId, DateTime startDate)
        {
            var result = new List<Invoice_Remarks>();
            if (processId > 0 && !startDate.Equals(null))
            {
                var ProcessId = processId > 0 ? new SqlParameter("@Process_Id", processId) : new SqlParameter("@Process_Id", DBNull.Value);
                var StartDate = !startDate.Equals(null) ? new SqlParameter("@Start_Date", startDate) : new SqlParameter("@Start_Date", DateTime.MinValue);

                result = await Task.Run(() => _dbContext.Invoice_Remarks
                                .FromSqlRaw(@"exec Invoice_Remarks_Select @Process_Id, @Start_Date", ProcessId, StartDate).ToListAsync());
            }
            else
            {
                result = await Task.Run(() => _dbContext.Invoice_Remarks
                            .FromSqlRaw(@"exec Get_Invoice_Remarks").ToListAsync());
            }

            return result;
        }
        public async Task<int> InvoiceRemarksChangeStatus(int process_Id, bool status)
        {
            var processId = new SqlParameter("@Process_Id", process_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Invoice_Remarks_Update_Status @Process_Id, @Status", processId, Status));
            return result;
        }
        #endregion
    }
}
