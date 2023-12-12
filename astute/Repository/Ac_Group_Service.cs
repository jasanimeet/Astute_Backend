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
    public partial class Ac_Group_Service : IAc_Group_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public Ac_Group_Service(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        #endregion

        #region Utilities
        private async Task Insert_Ac_Group_Trace(Ac_Group_Master ac_Group_Master, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var ac_Group_Name = new SqlParameter("@Ac_Group_Name", ac_Group_Master.Ac_Group_Name);
            var trans_Type = new SqlParameter("@Trans_Type", ac_Group_Master.Trans_Type);
            var basic_Group = new SqlParameter("@Basic_Group", ac_Group_Master.Basic_Group);
            var status = new SqlParameter("@Status", ac_Group_Master.Status);

            await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Ac_Group_Master_Trace_Insert @Employee_Id, @IP_Address,@Trace_Date, @Trace_Time, @Record_Type, @Ac_Group_Name, @Trans_Type,
                                @Basic_Group, @Status", empId, ipaddress, date, time, record_Type, ac_Group_Name, trans_Type, basic_Group, status));
        }

        public async Task Insert_Ac_Group_Detail_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblAc_Group_Detail_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Ac_Group_Detail_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Ac_Group_Detail_Trace_Insert @tblAc_Group_Detail_Trace", parameter);
        }
        #endregion

        #region Methods
        public virtual async Task<(string, int)> Add_Update_Ac_Group(Ac_Group_Master ac_Group_Master)
        {
            var ac_Group_Id = new SqlParameter("@Ac_Group_Id", ac_Group_Master.Ac_Group_Id);
            var ac_Group_Name = new SqlParameter("@Ac_Group_Name", ac_Group_Master.Ac_Group_Name);
            var trans_Type = new SqlParameter("@Trans_Type", ac_Group_Master.Trans_Type);
            var basic_Group = new SqlParameter("@Basic_Group", ac_Group_Master.Basic_Group);
            var status = new SqlParameter("@Status", ac_Group_Master.Status);
            var insertedId = new SqlParameter("@InsertedId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Ac_Group_Master_Insert_Update @Ac_Group_Id, @Ac_Group_Name, @Trans_Type, @Basic_Group, @Status, @InsertedId OUT", ac_Group_Id, ac_Group_Name,
                        trans_Type, basic_Group, status, insertedId));

            if (result > 0)
            {
                int _insertedId = (int)insertedId.Value;
                //if (CoreService.Enable_Trace_Records(_configuration))
                //{
                //    if (ac_Group_Master.Ac_Group_Id > 0)
                //        await Insert_Ac_Group_Trace(ac_Group_Master, "Update");
                //    else
                //        await Insert_Ac_Group_Trace(ac_Group_Master, "Insert");
                //}
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public virtual async Task<IList<Ac_Group_Master>> Get_Ac_Group(int ac_Group_Id)
        {
            var _ac_Group_Id = ac_Group_Id > 0 ? new SqlParameter("@Ac_Group_Id", ac_Group_Id) : new SqlParameter("@Ac_Group_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Ac_Group_Master
                            .FromSqlRaw(@"exec Ac_Group_Master_Select @Ac_Group_Id", _ac_Group_Id).ToListAsync());

            return result;
        }
        public virtual async Task<IList<Ac_Group_Master>> Get_Active_Ac_Group(int ac_Group_Id)
        {
            var _ac_Group_Id = ac_Group_Id > 0 ? new SqlParameter("@Ac_Group_Id", ac_Group_Id) : new SqlParameter("@Ac_Group_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Ac_Group_Master
                            .FromSqlRaw(@"exec Ac_Group_Master_Active_Select @Ac_Group_Id", _ac_Group_Id).ToListAsync());

            return result;
        }
        public virtual async Task<int> Add_Update_Ac_Group_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblAc_Group_Det", SqlDbType.Structured)
            {
                TypeName = "dbo.Ac_Group_Detail_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Ac_Group_Detail_Insert_Update @tblAc_Group_Det", parameter);

            return result;
        }
        public virtual async Task<Ac_Group_Master> Get_Ac_Group_Detail(int ac_Group_Id)
        {
            var _ac_Group_Id = ac_Group_Id > 0 ? new SqlParameter("@Ac_Group_Id", ac_Group_Id) : new SqlParameter("@Ac_Group_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Ac_Group_Master
                            .FromSqlRaw(@"exec Ac_Group_Master_Select @Ac_Group_Id", _ac_Group_Id)
                            .AsEnumerable()
                            .FirstOrDefault());

            if (result != null)
            {
                if (ac_Group_Id > 0)
                {
                    var _ac_GroupId = ac_Group_Id > 0 ? new SqlParameter("@Ac_Group_Id", ac_Group_Id) : new SqlParameter("@Ac_Group_Id", DBNull.Value);
                    result.Ac_Group_Detail_List = await Task.Run(() => _dbContext.Ac_Group_Detail
                                                .FromSqlRaw(@"exec Ac_Group_Detail_Select @Ac_Group_Id", _ac_GroupId)
                                                .ToListAsync());
                }
            }
            return result;
        }
        public virtual async Task<int> Delete_Ac_Group(int ac_Group_Id)
        {
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var _ac_Group_Id = ac_Group_Id > 0 ? new SqlParameter("@Ac_Group_Id", ac_Group_Id) : new SqlParameter("@Ac_Group_Id", DBNull.Value);
            //    var result = await Task.Run(() => _dbContext.Ac_Group_Master
            //                    .FromSqlRaw(@"exec Ac_Group_Master_Select @Ac_Group_Id", _ac_Group_Id)
            //                    .AsEnumerable()
            //                    .FirstOrDefault());
            //    if(result != null)
            //    {
            //        await Insert_Ac_Group_Trace(result, "Delete");
            //    }
            //}

            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Ac_Group_Master_Delete {ac_Group_Id}"));
        }
        #endregion
    }
}
