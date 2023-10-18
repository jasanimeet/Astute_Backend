using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class EmpRightsService : IEmpRightsService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public EmpRightsService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> InsertUpdateEmpRights(Emp_rights_Model emp_Rights_Model)
        {
            int result = 0;
            if (emp_Rights_Model.Emp_Right_Post_Model != null && emp_Rights_Model.Emp_Right_Post_Model.Count > 0)
            {
                foreach (var right in emp_Rights_Model.Emp_Right_Post_Model)
                {
                    var employeeId = emp_Rights_Model.Employee_Id > 0 ? new SqlParameter("@Employee_Id", emp_Rights_Model.Employee_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
                    var menuId = right.Menu_Id > 0 ? new SqlParameter("@Menu_Id", right.Menu_Id) : new SqlParameter("@Menu_Id", DBNull.Value);
                    var insertRight = new SqlParameter("@Insert_Allow", right.Insert_Allow);
                    var updateRight = new SqlParameter("@Update_Allow", right.Update_Allow);
                    var deleteRight = new SqlParameter("@Delete_Allow", right.Delete_Allow);
                    var excelRight = new SqlParameter("@Excel_Allow", right.Excel_Allow);
                    var printRight = new SqlParameter("@Print_Allow", right.Print_Allow);
                    var queryRight = new SqlParameter("@Query_Allow", right.Query_Allow);

                    result = await Task.Run(() => _dbContext.Database
                                    .ExecuteSqlRawAsync(@"EXEC Emp_rights_Insert_Update @Menu_Id, @Employee_Id, @Insert_Allow, @Update_Allow, @Delete_Allow, @Excel_Allow,
                                    @Print_Allow, @Query_Allow", employeeId, menuId, insertRight, updateRight, deleteRight, excelRight, printRight, queryRight));
                }
                result = 1;
            }
            return result;
        }
        public async Task<int> Copy_Emp_Rights(int fromEmpId, int toEmpId)
        {
            int result = 0;
            var fromEmpRights = new List<Emp_rights>();
            var toEmpRights = new List<Emp_rights>();
            if (fromEmpId > 0)
            {
                var empId = new SqlParameter("@Employee_Id", fromEmpId);

                fromEmpRights = await Task.Run(() => _dbContext.Emp_rights
                                .FromSqlRaw(@"exec Emp_Rights_Select @Employee_Id", empId).ToListAsync());
            }
            if (toEmpId > 0)
            {
                var empId = new SqlParameter("@Employee_Id", toEmpId);

                await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Emp_Rights_Delete {empId}"));

                if(fromEmpRights != null && fromEmpRights.Count > 0)
                {
                    foreach (var right in fromEmpRights)
                    {
                        var employeeId = toEmpId > 0 ? new SqlParameter("@Employee_Id", toEmpId) : new SqlParameter("@Employee_Id", DBNull.Value);
                        var menuId = right.Menu_Id > 0 ? new SqlParameter("@Menu_Id", right.Menu_Id) : new SqlParameter("@Menu_Id", DBNull.Value);
                        var insertRight = new SqlParameter("@Insert_Allow", right.Insert_Allow);
                        var updateRight = new SqlParameter("@Update_Allow", right.Update_Allow);
                        var deleteRight = new SqlParameter("@Delete_Allow", right.Delete_Allow);
                        var excelRight = new SqlParameter("@Excel_Allow", right.Excel_Allow);
                        var printRight = new SqlParameter("@Print_Allow", right.Print_Allow);
                        var queryRight = new SqlParameter("@Query_Allow", right.Query_Allow);

                        result = await Task.Run(() => _dbContext.Database
                                        .ExecuteSqlRawAsync(@"EXEC Emp_rights_Insert_Update @Menu_Id, @Employee_Id, @Insert_Allow, @Update_Allow, @Delete_Allow, @Excel_Allow,
                                    @Print_Allow, @Query_Allow", employeeId, menuId, insertRight, updateRight, deleteRight, excelRight, printRight, queryRight));
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
