using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class HolidayService : IHolidayService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public HolidayService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> Insert_Update_Holiday(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblHoliday", SqlDbType.Structured)
            {
                TypeName = "dbo.Holiday_Master_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Holiday_Mas_Insert_Update @tblHoliday", parameter);
            return result;
        }
        public async Task<int> InsertHoliday(Holiday_Master holiday_Mas)
        {
            var date = !holiday_Mas.Date.Equals(null) ? new SqlParameter("@Date", holiday_Mas.Date) : new SqlParameter("@Date", DBNull.Value);
            var start_Date = !string.IsNullOrEmpty(holiday_Mas.Start_Time) ? new SqlParameter("@Start_Time", holiday_Mas.Start_Time) : new SqlParameter("@Start_Time", DBNull.Value);
            var end_Date = !string.IsNullOrEmpty(holiday_Mas.End_Time) ? new SqlParameter("@End_Time", holiday_Mas.End_Time) : new SqlParameter("@End_Time", DBNull.Value);
            var holiday_Flag = new SqlParameter("@Holiday_Flag", holiday_Mas.Holiday_Flag);
            var description = !string.IsNullOrEmpty(holiday_Mas.Description) ? new SqlParameter("@Description", holiday_Mas.Description) : new SqlParameter("@Description", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Holiday_Mas_Insert_Update @Date, @Start_Time, @End_Time, @Holiday_Flag, @Description, @recordType", date, start_Date, end_Date, holiday_Flag, description, recordType));

            return result;
        }
        public async Task<int> UpdateHoliday(Holiday_Master holiday_Mas)
        {
            var date = !holiday_Mas.Date.Equals(null) ? new SqlParameter("@Date", holiday_Mas.Date) : new SqlParameter("@Date", DBNull.Value);
            var start_Date = !string.IsNullOrEmpty(holiday_Mas.Start_Time) ? new SqlParameter("@Start_Time", holiday_Mas.Start_Time) : new SqlParameter("@Start_Time", DBNull.Value);
            var end_Date = !string.IsNullOrEmpty(holiday_Mas.End_Time) ? new SqlParameter("@End_Time", holiday_Mas.End_Time) : new SqlParameter("@End_Time", DBNull.Value);
            var holiday_Flag = new SqlParameter("@Holiday_Flag", holiday_Mas.Holiday_Flag);
            var description = !string.IsNullOrEmpty(holiday_Mas.Description) ? new SqlParameter("@Description", holiday_Mas.Description) : new SqlParameter("@Description", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Holiday_Mas_Insert_Update @Date, @Start_Time, @End_Time, @Holiday_Flag, @Description, @recordType", date, start_Date, end_Date, holiday_Flag, description, recordType));

            return result;
        }
        public async Task<int> DeleteHoliday(int holiday_Id)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Holiday_Id", holiday_Id));

            var result = await Task.Run(() => _dbContext.Database
                         .ExecuteSqlRawAsync(@"exec Holiday_Mas_Delete @Holiday_Id", parameter.ToArray()));

            return result;
        }
        public async Task<IList<Holiday_Master>> GetHolidays(int holiday_Id)
        {
            var _holiday_Id = holiday_Id > 0 ? new SqlParameter("@Holiday_Id", holiday_Id) : new SqlParameter("@Holiday_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Holiday_Master
                            .FromSqlRaw(@"EXEC Holiday_Mas_Select @Holiday_Id", _holiday_Id).ToListAsync());
            return result;
        }
        public async Task<IList<Holiday_Master>> Get_Holidays(string date)
        {
            var _date = !string.IsNullOrEmpty(date) ? new SqlParameter("@Date", date) : new SqlParameter("@Date", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Holiday_Master
                            .FromSqlRaw(@"EXEC Holiday_Mas_Select @Date", _date).ToListAsync());
            return result;
        }
        public async Task Insert_Holiday_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblHoliday_Master_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Holiday_Master_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Holiday_Master_Trace_Insert @tblHoliday_Master_Trace", parameter);
        }
        #endregion
    }
}
