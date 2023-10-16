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
    public partial class BGMService : IBGMService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public BGMService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_BGM_Trace(BGM_Master bGM_Mas, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var bgm = new SqlParameter("@Bgm", bGM_Mas.BGM);
            var shade = bGM_Mas.Shade > 0 ? new SqlParameter("@Shade", bGM_Mas.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var milky = bGM_Mas.Milky > 0 ? new SqlParameter("@Milky", bGM_Mas.Milky) : new SqlParameter("@Milky", DBNull.Value);
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
            var shade = bGM_Mas.Shade > 0 ? new SqlParameter("@Shade", bGM_Mas.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var milky = bGM_Mas.Milky > 0 ? new SqlParameter("@Milky", bGM_Mas.Milky) : new SqlParameter("@Milky", DBNull.Value);
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

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_BGM_Trace(bGM_Mas, "Insert");
            }

            return result;
        }
        public async Task<int> UpdateBGM(BGM_Master bGM_Mas)
        {
            var bgmId = new SqlParameter("@Bgm_Id", bGM_Mas.Bgm_Id);
            var bgm = new SqlParameter("@Bgm", bGM_Mas.BGM);
            var shade = bGM_Mas.Shade > 0 ? new SqlParameter("@Shade", bGM_Mas.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var milky = bGM_Mas.Milky > 0 ? new SqlParameter("@Milky", bGM_Mas.Milky) : new SqlParameter("@Milky", DBNull.Value);
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

            if (CoreService.Enable_Trace_Records(_configuration))
            {
                await Insert_BGM_Trace(bGM_Mas, "Update");
            }

            return result;
        }
        public async Task<int> DeleteBGM(int bgm_Id)
        {
            if (CoreService.Enable_Trace_Records(_configuration))
            {
                var bGM_Mas = await Task.Run(() => _dbContext.BGM_Master
                            .FromSqlRaw(@"exec BGM_Mas_Select @BgmId, @Shade, @Milky", bgm_Id, new SqlParameter("@Shade", DBNull.Value), new SqlParameter("@Milky", DBNull.Value))
                            .AsEnumerable()
                            .FirstOrDefault());
                if(bGM_Mas != null)
                {
                    await Insert_BGM_Trace(bGM_Mas, "Delete");
                }
            }
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"BGM_Mas_Delete {bgm_Id}"));
        }
        public async Task<IList<BGM_Master>> GetBgm(int bgm_Id, int shade, int milky)
        {
            var _bgm_Id = bgm_Id > 0 ? new SqlParameter("@BgmId", bgm_Id) : new SqlParameter("@BgmId", DBNull.Value);
            var Shade = shade > 0 ? new SqlParameter("@Shade", shade) : new SqlParameter("@Shade", DBNull.Value);
            var Milky = shade > 0 ? new SqlParameter("@Milky", milky) : new SqlParameter("@Milky", DBNull.Value);

            var result = await Task.Run(() => _dbContext.BGM_Master
                            .FromSqlRaw(@"exec BGM_Mas_Select @BgmId, @Shade, @Milky", _bgm_Id, Shade, Milky).ToListAsync());

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
        #endregion
    }
}
