using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class BGMService : IBGMService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public BGMService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IJWTAuthentication jWTAuthentication)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Utilities
        private async Task Insert_BGM_Trace(BGM_Master bGM_Mas, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
            var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(user_Id ?? 0, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var bgm = new SqlParameter("@Bgm", bGM_Mas.BGM);
            var shade = new SqlParameter("@Shade", DBNull.Value);
            var milky = new SqlParameter("@Milky", DBNull.Value);
            var sortNo = bGM_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bGM_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var orderNo = bGM_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bGM_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", bGM_Mas.Status);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC BGM_Master_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Bgm, @Shade, @Milky, @Sort_No, @Order_No, @Status",
            empId, ipaddress, date, time, record_Type, bgm, shade, milky, sortNo, orderNo, status));
        }
        #endregion

        #region Methods
        public async Task<int> InsertBGM(BGM_Master bGM_Mas)
        {
            var bgmId = new SqlParameter("@Bgm_Id", bGM_Mas.Bgm_Id);
            var bgm = new SqlParameter("@Bgm", bGM_Mas.BGM);
            var shade = new SqlParameter("@Shade", DBNull.Value);
            var milky = new SqlParameter("@Milky", DBNull.Value);
            var sortNo = bGM_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bGM_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var orderNo = bGM_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bGM_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", bGM_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Insert");

            var isExistShade_Milky = new SqlParameter("@IsExistShade_Milky", System.Data.SqlDbType.Bit)
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
            .ExecuteSqlRawAsync(@"EXEC BGM_Mas_Insert_Update @Bgm_Id, @Bgm, @Shade, @Milky, @Sort_No, @Order_No, @Status, @recordType, @IsExistShade_Milky OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT",
            bgmId, bgm, shade, milky, sortNo, orderNo, status, recordType, isExistShade_Milky, isExistOrderNo, isExistSortNo));

            bool Shade_MilkyIsExist = (bool)isExistShade_Milky.Value;
            if (Shade_MilkyIsExist)
                return 5;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_BGM_Trace(bGM_Mas, "Insert");
            //}

            return result;
        }
        public async Task<(string, int)> Add_Update_Bgm_Master(BGM_Master bGM_Master)
        {
            var bgmId = new SqlParameter("@Bgm_Id", bGM_Master.Bgm_Id);
            var bgm = new SqlParameter("@Bgm", bGM_Master.BGM);
            var sortNo = bGM_Master.Sort_No > 0 ? new SqlParameter("@Sort_No", bGM_Master.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var orderNo = bGM_Master.Order_No > 0 ? new SqlParameter("@Order_No", bGM_Master.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", bGM_Master.Status);
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
            .ExecuteSqlRawAsync(@"EXEC BGM_Mas_Insert_Update @Bgm_Id, @Bgm, @Sort_No, @Order_No, @Status, @IsExistOrderNo OUT, @IsExistSortNo OUT, @InsertedId OUT",
            bgmId, bgm, sortNo, orderNo, status, isExistOrderNo, isExistSortNo, insertedId));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return ("_error_order_no", 0);

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return ("_error_sort_no", 0);

            if (result > 0)
            {
                int _insertedId = (int)insertedId.Value;
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<int> UpdateBGM(BGM_Master bGM_Mas)
        {
            var bgmId = new SqlParameter("@Bgm_Id", bGM_Mas.Bgm_Id);
            var bgm = new SqlParameter("@Bgm", bGM_Mas.BGM);
            var shade = new SqlParameter("@Shade", DBNull.Value);
            var milky = new SqlParameter("@Milky", DBNull.Value);
            var sortNo = bGM_Mas.Sort_No > 0 ? new SqlParameter("@Sort_No", bGM_Mas.Sort_No) : new SqlParameter("@Sort_No", DBNull.Value);
            var orderNo = bGM_Mas.Order_No > 0 ? new SqlParameter("@Order_No", bGM_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", bGM_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Update");

            var isExistShade_Milky = new SqlParameter("@IsExistShade_Milky", System.Data.SqlDbType.Bit)
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
            .ExecuteSqlRawAsync(@"EXEC BGM_Mas_Insert_Update @Bgm_Id, @Bgm, @Shade, @Milky, @Sort_No, @Order_No, @Status, @recordType, @IsExistShade_Milky OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT",
            bgmId, bgm, shade, milky, sortNo, orderNo, status, recordType, isExistShade_Milky, isExistOrderNo, isExistSortNo));

            bool Shade_MilkyIsExist = (bool)isExistShade_Milky.Value;
            if (Shade_MilkyIsExist)
                return 5;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_BGM_Trace(bGM_Mas, "Update");
            //}

            return result;
        }
        public async Task<int> DeleteBGM(int bgm_Id)
        {
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var bGM_Mas = await Task.Run(() => _dbContext.BGM_Master
            //                .FromSqlRaw(@"exec BGM_Mas_Select @BgmId, @Shade, @Milky", bgm_Id, new SqlParameter("@Shade", DBNull.Value), new SqlParameter("@Milky", DBNull.Value))
            //                .AsEnumerable()
            //                .FirstOrDefault());
            //    if(bGM_Mas != null)
            //    {
            //        await Insert_BGM_Trace(bGM_Mas, "Delete");
            //    }
            //}
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"BGM_Mas_Delete {bgm_Id}"));
        }
        public async Task<IList<BGM_Master>> GetBgm(int bgm_Id)
        {
            var _bgm_Id = bgm_Id > 0 ? new SqlParameter("@BgmId", bgm_Id) : new SqlParameter("@BgmId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.BGM_Master
                            .FromSqlRaw(@"exec BGM_Mas_Select @BgmId", _bgm_Id).ToListAsync());

            return result;
        }
        public async Task<int> BGMChangeStatus(int bgm_Id, bool status)
        {
            var bgmid = new SqlParameter("@Bgm_Id", bgm_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC BGM_Master_Update_Status @Bgm_Id, @Status", bgmid, Status));
            return result;
        }
        public async Task<int> Insert_BGM_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblBGM_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.BGM_Detail_Table_Type",
                Value = dataTable
            };
            //var isExistShade_Milky = new SqlParameter("@IsExistShade_Milky", System.Data.SqlDbType.Bit)
            //{
            //    Direction = System.Data.ParameterDirection.Output
            //};

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC BGM_Detail_Insert_Update @tblBGM_Detail", parameter);

            //bool Shade_MilkyIsExist = (bool)isExistShade_Milky.Value;
            //if (Shade_MilkyIsExist)
            //    return 409;

            return result;
        }
        public async Task<BGM_Master> Get_Bgm_Detail(int bgm_Id)
        {
            var _bgm_Id = bgm_Id > 0 ? new SqlParameter("@BgmId", bgm_Id) : new SqlParameter("@BgmId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.BGM_Master
                .FromSqlRaw(@"exec BGM_Mas_Select @BgmId", _bgm_Id)
                .AsEnumerable()
                .FirstOrDefault());

            if (result != null)
            {
                if (bgm_Id > 0)
                {
                    var id = new SqlParameter("@Id", DBNull.Value);
                    var _bgm_Id1 = bgm_Id > 0 ? new SqlParameter("@BgmId", bgm_Id) : new SqlParameter("@BgmId", DBNull.Value);

                    result.BGM_Detail_List = await Task.Run(() => _dbContext.BGM_Detail
                            .FromSqlRaw(@"exec BGM_Detail_Select @Id, @BgmId", id, _bgm_Id1).ToList());
                }
            }

            return result;
        }
        public async Task<int> BGM_Detail_Change_Status(int id, bool status)
        {
            var _id = new SqlParameter("@Id", id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC BGM_Detail_Update_Status @Id, @Status", _id, Status));
            return result;
        }
        public async Task<int> Get_BGM_Master_Max_Order_No()
        {
            var result = await _dbContext.BGM_Master.Select(x => x.Order_No).MaxAsync();
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
