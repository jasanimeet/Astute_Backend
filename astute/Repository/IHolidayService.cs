using astute.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IHolidayService
    {
        Task<int> InsertHoliday(Holiday_Master holiday_Mas);
        Task<int> UpdateHoliday(Holiday_Master holiday_Mas);
        Task<int> DeleteHoliday(int holiday_Id);
        Task<IList<Holiday_Master>> GetHolidays(int holiday_Id);
    }
}
