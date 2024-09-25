using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IHolidayService
    {
        Task<int> Insert_Update_Holiday(DataTable dataTable);
        Task<int> InsertHoliday(Holiday_Master holiday_Mas);
        Task<int> UpdateHoliday(Holiday_Master holiday_Mas);
        Task<int> DeleteHoliday(int holiday_Id);
        Task<IList<Holiday_Master>> GetHolidays(int holiday_Id);
        Task<IList<Holiday_Master>> Get_Holidays(string date);
        Task Insert_Holiday_Trace(DataTable dataTable);
    }
}
