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

                if (fromEmpRights != null && fromEmpRights.Count > 0)
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

        public async Task<int> Insert_Update_Employee_Download_Share_Rights(Employee_Download_Share_Rights_Model employee_Download_Share_Rights_Model)
        {
            int result = 0;
            if (employee_Download_Share_Rights_Model.Employee_Download_Share_Rights_Post_Model != null && employee_Download_Share_Rights_Model.Employee_Download_Share_Rights_Post_Model.Count > 0)
            {
                foreach (var right in employee_Download_Share_Rights_Model.Employee_Download_Share_Rights_Post_Model)
                {
                    var employeeId = employee_Download_Share_Rights_Model.Employee_Id > 0 ? new SqlParameter("@Employee_Id", employee_Download_Share_Rights_Model.Employee_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
                    var menuId = right.Menu_Id > 0 ? new SqlParameter("@Menu_Id", right.Menu_Id) : new SqlParameter("@Menu_Id", DBNull.Value);
                    var D_Default_Excel = new SqlParameter("@D_Default_Excel", right.D_Default_Excel);
                    var D_Custom_Excel = new SqlParameter("@D_Custom_Excel", right.D_Custom_Excel);
                    var D_Image = new SqlParameter("@D_Image", right.D_Image);
                    var D_Video = new SqlParameter("@D_Video", right.D_Video);
                    var D_Certificate = new SqlParameter("@D_Certificate", right.D_Certificate);
                    var S_Default_Excel = new SqlParameter("@S_Default_Excel", right.S_Default_Excel);
                    var S_Custom_Excel = new SqlParameter("@S_Custom_Excel", right.S_Custom_Excel);
                    var S_Image = new SqlParameter("@S_Image", right.S_Image);
                    var S_Video = new SqlParameter("@S_Video", right.S_Video);
                    var S_Certificate = new SqlParameter("@S_Certificate", right.S_Certificate);

                    result = await Task.Run(() => _dbContext.Database
                                    .ExecuteSqlRawAsync(@"EXEC Employee_Download_Share_Rights_Insert_Update @Menu_Id, @Employee_Id, @D_Default_Excel, @D_Custom_Excel, @D_Image, @D_Video,
                                    @D_Certificate, @S_Default_Excel, @S_Custom_Excel, @S_Image, @S_Video, @S_Certificate", employeeId, menuId, D_Default_Excel, D_Custom_Excel, D_Image, D_Video, D_Certificate, S_Default_Excel, S_Custom_Excel, S_Image, S_Video, S_Certificate));
                }
                result = 1;
            }
            return result;
        }

        #endregion
    }
}
