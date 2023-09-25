using astute.CoreServices;
using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class RapaportService : IRapaportService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public RapaportService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        #region Rapaport Master
        public async Task<int> InsertRapaport(Rapaport_Master rapaport_Master)
        {
            var rap_Id = new SqlParameter("@Rap_Id", rapaport_Master.Rap_Id);
            var inserted_Date = new SqlParameter("@Insert_Date", rapaport_Master.Insert_Date);
            var record_Type = new SqlParameter("@recordType", "Insert");
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Rapaport_Master_Insert_Update @Rap_Id, @Insert_Date, @recordType, @InsertedId OUT",
                                rap_Id, inserted_Date, record_Type, insertedId));

            var inserted_Layout_Id = (int)insertedId.Value;

            return inserted_Layout_Id;
        }
        public async Task<int> UpdateRapaport(Rapaport_Master rapaport_Master)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Rap_Id", rapaport_Master.Rap_Id));
            parameter.Add(new SqlParameter("@Insert_Date", rapaport_Master.Insert_Date));
            parameter.Add(new SqlParameter("@recordType", "Update"));

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Rapaport_Master_Insert_Update @Rap_Id, @Insert_Date, @recordType", parameter.ToArray()));
            return result;
        }
        public async Task<int> DeleteRapaport(int rapId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Rapaport_Master_Delete {rapId}"));
        }
        public async Task<IList<Rapaport_Master>> GetRapaport(int rapId)
        {
            var rap_Id = rapId > 0 ? new SqlParameter("@Rap_Id", rapId) : new SqlParameter("@Rap_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Rapaport_Master
                            .FromSqlRaw(@"exec Rapaport_Master_Select @Rap_Id", rap_Id).ToListAsync());

            return result;
        }
        #endregion

        #region Rapaport Detail
        public async Task<int> InsertRapaportDetail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@rapaport_Details", SqlDbType.Structured)
            {
                TypeName = "dbo.RapaportDetailDataType",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Rapaport_Detail_Insert_Update @rapaport_Details", parameter);
            return result;
        }
        public async Task<int> UpdateRapaportDetail(Rapaport_Detail rapaport_Detail)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Rap_Id", rapaport_Detail.Rap_Id));
            parameter.Add(new SqlParameter("@Rap_Det_Id", rapaport_Detail.Rap_Det_Id));
            parameter.Add(new SqlParameter("@From_Cts", rapaport_Detail.From_Cts));
            parameter.Add(new SqlParameter("@To_Cts", rapaport_Detail.To_Cts));
            parameter.Add(new SqlParameter("@Color_Id", rapaport_Detail.Color_Id));
            parameter.Add(new SqlParameter("@Clarity_Id", rapaport_Detail.Clarity_Id));
            parameter.Add(new SqlParameter("@Rate", rapaport_Detail.Rate));
            parameter.Add(new SqlParameter("@recordType", "Update"));

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Rapaport_Detail_Insert_Update @Rap_Id, @Rap_Det_Id, @From_Cts, @To_Cts, @Color_Id, @Clarity_Id,
                                @Rate, @recordType", parameter.ToArray()));
            return result;

        }
        public async Task<int> DeleteRapaportDetail(int rap_Det_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete_Rapaport_Detail {rap_Det_Id}"));
        }
        public async Task<IList<RapaportPriceModel>> GetRapaportDetail(string? shape, string? color, string? clarity, decimal? frmCts, decimal? toCts, decimal? rate, string? date)
        {
            var _shape = !string.IsNullOrEmpty(shape) ? new SqlParameter("@Shape", shape) : new SqlParameter("@Shape", DBNull.Value);
            var _color = !string.IsNullOrEmpty(color) ? new SqlParameter("@Color", color) : new SqlParameter("@Color", DBNull.Value);
            var _clarity = !string.IsNullOrEmpty(clarity) ? new SqlParameter("@Clarity", clarity) : new SqlParameter("@Clarity", DBNull.Value);
            var _frmCts = frmCts > 0 ? new SqlParameter("@FromCts", frmCts) : new SqlParameter("@FromCts", DBNull.Value);
            var _toCts = toCts > 0 ? new SqlParameter("@ToCts", toCts) : new SqlParameter("@ToCts", DBNull.Value);
            var _rate = rate > 0 ? new SqlParameter("@Rate", rate) : new SqlParameter("@Rate", DBNull.Value);
            var _date = !string.IsNullOrEmpty(date) ? new SqlParameter("@Date", date) : new SqlParameter("@Date", DBNull.Value);

            var result = await _dbContext.RapaportPriceModel
                        .FromSqlRaw($"EXEC Rapaport_Detail_Select @Shape, @Color, @Clarity, @FromCts, @ToCts, @Rate, @Date", _shape, _color, _clarity, _frmCts,
                        _toCts, _rate, _date).ToListAsync();

            return result;
        }
        public async Task<IList<Rapaport_Color_Value>> Get_Rapaport_Color_Filter_Value()
        {
            var result = await _dbContext.Rapaport_Color_Value.FromSqlRaw(@"EXEC Category_Color_Value").ToListAsync();
            return result;
        }
        public async Task<IList<Rapaport_Clarity_Value>> Get_Rapaport_Clarity_Filter_Value()
        {   
            var result = await _dbContext.Rapaport_Clarity_Value.FromSqlRaw(@"EXEC Category_Clarity_Value").ToListAsync();
            return result;
        }
        public async Task<IList<Rapaport_Date_Value>> Get_Rapaport_Date_Filter_Value()
        {   
            var result = await _dbContext.Rapaport_Date_Value.FromSqlRaw(@"EXEC Rapaport_Date_Select").ToListAsync();
            return result;
        }
        public async Task<IList<Bank_Dropdown_Model>> Get_Rapaport_Color()
        {
            var result = await _dbContext.Bank_Dropdown_Model.FromSqlRaw(@"EXEC Rapaport_Color_Select").ToListAsync();
            return result;
        }
        #endregion

        #region Rapaport User
        public async Task<int> InsertRapaportUser(Rapaport_User rapaport_User)
        {
            //var encrypt_Password = CoreService.Encrypt(rapaport_User.Rap_Password);
            var rap_User = !string.IsNullOrEmpty(rapaport_User.Rap_User) ? new SqlParameter("@Rap_User", rapaport_User.Rap_User) : new SqlParameter("@Rap_User", DBNull.Value);
            var rap_Password = !string.IsNullOrEmpty(rapaport_User.Rap_Password) ? new SqlParameter("@Rap_Password", rapaport_User.Rap_Password) : new SqlParameter("@Rap_User", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");


            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Rapaport_User_Insert_Update @Rap_User, @Rap_Password, @recordType", rap_User, rap_Password, recordType));
            return result;
        }
        public async Task<int> UpdateRapaportUser(Rapaport_User rapaport_User)
        {
            //var encrypt_Password = CoreService.Encrypt(rapaport_User.Rap_Password);
            var rap_User = !string.IsNullOrEmpty(rapaport_User.Rap_User) ? new SqlParameter("@Rap_User", rapaport_User.Rap_User) : new SqlParameter("@Rap_User", DBNull.Value);
            var rap_Password = !string.IsNullOrEmpty(rapaport_User.Rap_Password) ? new SqlParameter("@Rap_Password", rapaport_User.Rap_Password) : new SqlParameter("@Rap_User", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");


            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Rapaport_User_Insert_Update @Rap_User, @Rap_Password, @recordType", rap_User, rap_Password, recordType));
            return result;
        }
        public async Task<int> DeleteRapaportUser(string rap_User)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Rapaport_User_Delete {rap_User}"));
        }
        public async Task<IList<Rapaport_User>> GetRapaportUser(string rap_User)
        {
            var rapUser = !string.IsNullOrWhiteSpace(rap_User) ? new SqlParameter("@Rap_User", rap_User) : new SqlParameter("@Rap_User", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Rapaport_User
                            .FromSqlRaw(@"exec Rapaport_User_Select @Rap_User", rapUser).ToListAsync());

            return result;
        }
        #endregion
        #endregion

    }
}
