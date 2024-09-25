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
    public partial class PointerService : IPointerService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public PointerService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Pointer_Trace(Pointer_Master pointer_Mas, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var pointerName = new SqlParameter("@Pointer_Name", pointer_Mas.Pointer_Name);
            var fromCts = new SqlParameter("@From_Cts", pointer_Mas.From_Cts);
            var toCts = new SqlParameter("@To_Cts", pointer_Mas.To_Cts);
            var pointerType = new SqlParameter("@Pointer_Type", pointer_Mas.Pointer_Type);
            var orderNo = pointer_Mas.Order_No > 0 ? new SqlParameter("@Order_No", pointer_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = pointer_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", pointer_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", pointer_Mas.Status);

            await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"exec Pointer_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Pointer_Name, 
                        @From_Cts, @To_Cts, @Pointer_Type, @Order_No, @Sort_No, @Status", empId, ipaddress, date, time, record_Type, pointerName, fromCts, toCts, 
                        pointerType, orderNo, sortNo, status));
        }
        public async Task Insert_Pointer_Detail_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Pointer_Detail_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Pointer_Detail_Trace_Data_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Pointer_Detail_Trace_Insert @Struct_Pointer_Detail_Trace", parameter);
        }
        #endregion

        #region Methods
        #region Pointer Master
        public async Task<(string, int)> Add_Update_Pointer(Pointer_Master pointer_Mas)
        {   
            var pointerId = new SqlParameter("@Pointer_Id", pointer_Mas.Pointer_Id);
            var pointerName = new SqlParameter("@Pointer_Name", pointer_Mas.Pointer_Name);
            var fromCts = new SqlParameter("@From_Cts", pointer_Mas.From_Cts);
            var toCts = new SqlParameter("@To_Cts", pointer_Mas.To_Cts);
            var pointerType = new SqlParameter("@Pointer_Type", pointer_Mas.Pointer_Type);
            var orderNo = pointer_Mas.Order_No > 0 ? new SqlParameter("@Order_No", pointer_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sortNo = pointer_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", pointer_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var status = new SqlParameter("@Status", pointer_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isExistPointer_Name = new SqlParameter("@IsExistPointer_Name", System.Data.SqlDbType.Bit)
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
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"exec Pointer_Mas_Insert_Update @Pointer_Id, @Pointer_Name, @From_Cts, @To_Cts, @Pointer_Type, @Order_No, @Sort_No, 
                        @Status, @recordType, @IsExistPointer_Name OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT", pointerId, pointerName, fromCts, toCts, pointerType, orderNo, sortNo,
                        status, recordType, isExistPointer_Name, isExistOrderNo, isExistSortNo, insertedId));

            bool pointer_NameIsExist = (bool)isExistPointer_Name.Value;
            if (pointer_NameIsExist)
                return ("_error_pointer_name", 0);

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return ("_error_order_no", 0);

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return ("_error_sort_no", 0);

            if (result > 0)
            {
                string record_Type = string.Empty;
                int _insertedId = (int)insertedId.Value;
                //if (CoreService.Enable_Trace_Records(_configuration))
                //{
                //    if (pointer_Mas.Pointer_Id == 0)
                //        record_Type = "Insert";
                //    else
                //        record_Type = "Update";
                //    await Insert_Pointer_Trace(pointer_Mas, record_Type);
                //}
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<Pointer_Master> Get_Pointer_Details(int pointer_Id)
        {
            var _pointer_Id = pointer_Id > 0 ? new SqlParameter("@Pointer_Id", pointer_Id) : new SqlParameter("@Pointer_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Pointer_Master
                            .FromSqlRaw(@"exec Pointer_Mas_Select @Pointer_Id", _pointer_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            if(result != null)
            {
                if(pointer_Id > 0)
                {
                    result.Pointer_Detail_List = await GetPointerDetail(0, pointer_Id);
                }
            }
            return result;
        }
        public async Task<int> UpdatePointer(Pointer_Master pointer_Mas)
        {
            var pointerId = new SqlParameter("@Pointer_Id", pointer_Mas.Pointer_Id);
            var pointerName = new SqlParameter("@Pointer_Name", pointer_Mas.Pointer_Name);
            var fromCts = new SqlParameter("@From_Cts", pointer_Mas.From_Cts);
            var toCts = new SqlParameter("@To_Cts", pointer_Mas.To_Cts);
            var pointerType = new SqlParameter("@Pointer_Type", pointer_Mas.Pointer_Type);
            var orderNo = new SqlParameter("@Order_No", pointer_Mas.Order_No);
            var sortNo = new SqlParameter("@Sort_No", pointer_Mas.Sort_No);
            var status = new SqlParameter("@Status", pointer_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"exec Pointer_Mas_Insert_Update @Pointer_Id, @Pointer_Name, @From_Cts, @To_Cts, @Pointer_Type, @Order_No, @Sort_No, 
                        @Status, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert", pointerId, pointerName, fromCts, toCts, pointerType, orderNo, sortNo,
                        status, recordType, isExistOrderNo, isExistSortNo));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> DeletePointer(int pointerId)
        {
            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Pointer_Master_Delete @PointerId", new SqlParameter("@PointerId", pointerId));

            return result;
        }
        public async Task<IList<Pointer_Master>> GetPointer(int pointerId)
        {   
            var pointer_Id = pointerId > 0 ? new SqlParameter("@Pointer_Id", pointerId) : new SqlParameter("@Pointer_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Pointer_Master
                            .FromSqlRaw(@"exec Pointer_Mas_Select @Pointer_Id", pointer_Id)
                            .ToListAsync());
            return result;
        }
        public async Task<int> PointerChangeStatus(int pointer_Id, bool status)
        {
            var pointerId = new SqlParameter("@Pointer_Id", pointer_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Pointer_Master_Update_Status @Pointer_Id, @Status", pointerId, Status));
            return result;
        }
        public async Task<int> Get_Pointer_Master_Max_Order_No()
        {
            var result = await _dbContext.Pointer_Master.Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
        public async Task<IList<Pointer_Master>> Get_Pointer_For_Stock_Generation(int pointerId, string stock_type, int company_Id, string shape_Ids)
        {
            var pointer_Id = new SqlParameter("@Pointer_Id", DBNull.Value);
            var _stock_type = !string.IsNullOrEmpty(stock_type) ? new SqlParameter("@Stock_Type", stock_type) : new SqlParameter("@Stock_Type", DBNull.Value);
            var _company_Id = company_Id > 0 ? new SqlParameter("@Company_Id", company_Id) : new SqlParameter("@Company_Id", DBNull.Value);
            var _shape_Ids = !string.IsNullOrEmpty(shape_Ids) ? new SqlParameter("@Shape_Ids", shape_Ids) : new SqlParameter("@Shape_Ids", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Pointer_Master
                            .FromSqlRaw(@"exec Get_Pointer_For_Stock_Generation @Pointer_Id, @Stock_Type, @Company_Id, @Shape_Ids", pointer_Id, _stock_type, _company_Id, shape_Ids)
                            .ToListAsync());
            return result;
        }
        #endregion

        #region Pointer Detail
        public async Task<int> InsertPointerDetail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@pointer_Det", SqlDbType.Structured)
            {
                TypeName = "dbo.Pointer_Detail_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Pointer_Det_Insert_Update @pointer_Det", parameter);
            return result;
        }
        public async Task<IList<Pointer_Detail>> GetPointerDetail(int pointerDetId, int pointerId)
        {
            var pointer_Det_Id = pointerDetId > 0 ? new SqlParameter("@Pointer_Det_Id", pointerDetId) : new SqlParameter("@Pointer_Det_Id", DBNull.Value);
            var pointer_Id = pointerId > 0 ? new SqlParameter("@Pointer_id", pointerId) : new SqlParameter("@Pointer_id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Pointer_Detail
                            .FromSqlRaw(@"exec Pointer_Det_Select @Pointer_Det_Id, @Pointer_id", pointer_Det_Id, pointer_Id).ToList());
            return result;
        }
        public async Task<int> PointerDetailChangeStatus(int pointer_Det_Id, bool status)
        {
            var pointerDetId = new SqlParameter("@Pointer_Det_Id", pointer_Det_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Pointer_Detail_Update_Status @Pointer_Det_Id, @Status", pointerDetId, Status));
            return result;
        }
        #endregion
        #endregion
    }
}
