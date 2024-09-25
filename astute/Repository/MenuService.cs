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
    public partial class MenuService : IMenuService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public MenuService(AstuteDbContext dbContext,
           IConfiguration configuration,
           IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Utilities
        private async Task Insert_Menu_Trace(Menu_Mas menu_Mas, string recordType)
        {           
            var menuName = !string.IsNullOrEmpty(menu_Mas.Menu_Name) ? new SqlParameter("@Menu_Name", menu_Mas.Menu_Name) : new SqlParameter("@Menu_Name", DBNull.Value);
            var caption = !string.IsNullOrEmpty(menu_Mas.Caption) ? new SqlParameter("@Caption", menu_Mas.Caption) : new SqlParameter("@Caption", DBNull.Value);
            var parentId = menu_Mas.Parent_Id > 0 ? new SqlParameter("@Parent_Id", menu_Mas.Parent_Id) : new SqlParameter("@Parent_Id", DBNull.Value);
            var menuType = !string.IsNullOrEmpty(menu_Mas.Menu_type) ? new SqlParameter("@Menu_type", menu_Mas.Menu_type) : new SqlParameter("@Menu_type", DBNull.Value);
            var shortKey = !string.IsNullOrEmpty(menu_Mas.Short_Key) ? new SqlParameter("@Short_Key", menu_Mas.Short_Key) : new SqlParameter("@Short_Key", DBNull.Value);
            var orderNo = menu_Mas.Order_No > 0 ? new SqlParameter("@Order_No", menu_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", menu_Mas.Status);
            var modulePath = !string.IsNullOrEmpty(menu_Mas.Module_Path) ? new SqlParameter("@Module_Path", menu_Mas.Module_Path) : new SqlParameter("@Module_Path", DBNull.Value);

            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Menu_Mas_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @RecordType, @Menu_Name, @Caption, @Parent_Id, @Menu_type, @Short_Key, @Order_No,
                            @Status, @Module_Path", empId, ipaddress, date, time, record_Type, menuName, caption, parentId, menuType, shortKey, orderNo, status, modulePath));
        }

        #endregion
        #region Methods
        public async Task<int> InsertMenu(Menu_Mas menu_Mas)
        {
            var menuId = new SqlParameter("@Menu_Id", menu_Mas.Menu_Id);
            var menuName = !string.IsNullOrEmpty(menu_Mas.Menu_Name) ? new SqlParameter("@Menu_Name", menu_Mas.Menu_Name) : new SqlParameter("@Menu_Name", DBNull.Value);
            var caption = !string.IsNullOrEmpty(menu_Mas.Caption) ? new SqlParameter("@Caption", menu_Mas.Caption) : new SqlParameter("@Caption", DBNull.Value);
            var parentId = menu_Mas.Parent_Id > 0 ? new SqlParameter("@Parent_Id", menu_Mas.Parent_Id) : new SqlParameter("@Parent_Id", DBNull.Value);
            var menuType = !string.IsNullOrEmpty(menu_Mas.Menu_type) ? new SqlParameter("@Menu_type", menu_Mas.Menu_type) : new SqlParameter("@Menu_type", DBNull.Value);
            var shortKey = !string.IsNullOrEmpty(menu_Mas.Short_Key) ? new SqlParameter("@Short_Key", menu_Mas.Short_Key) : new SqlParameter("@Short_Key", DBNull.Value);
            var orderNo = menu_Mas.Order_No > 0 ? new SqlParameter("@Order_No", menu_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", menu_Mas.Status);
            var modulePath = !string.IsNullOrEmpty(menu_Mas.Module_Path) ? new SqlParameter("@Module_Path", menu_Mas.Module_Path) : new SqlParameter("@Module_Path", DBNull.Value);
            var recoredType = new SqlParameter("@recordType", "Insert");
            var isExistParameter = new SqlParameter("@IsExistsMenu", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Menu_Mas_Insert_Update @Menu_Id, @Menu_Name, @Caption, @Parent_Id, @Menu_type, @Short_Key, @Order_No,
                            @Status, @Module_Path, @recordType, @IsExistsMenu OUT", menuId, menuName, caption, parentId, menuType, shortKey, orderNo, status, modulePath, recoredType, isExistParameter));

            var isExist = (bool)isExistParameter.Value;
            if (isExist)
                return 2;


            string record_Type = string.Empty;
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    record_Type = "Insert";
            //    await Insert_Menu_Trace(menu_Mas, record_Type);
            //}
            return result;
        }
        public async Task<int> UpdateMenu(Menu_Mas menu_Mas)
        {
            var menuId = new SqlParameter("@Menu_Id", menu_Mas.Menu_Id);
            var menuName = !string.IsNullOrEmpty(menu_Mas.Menu_Name) ? new SqlParameter("@Menu_Name", menu_Mas.Menu_Name) : new SqlParameter("@Menu_Name", DBNull.Value);
            var caption = !string.IsNullOrEmpty(menu_Mas.Caption) ? new SqlParameter("@Caption", menu_Mas.Caption) : new SqlParameter("@Caption", DBNull.Value);
            var parentId = menu_Mas.Parent_Id > 0 ? new SqlParameter("@Parent_Id", menu_Mas.Parent_Id) : new SqlParameter("@Parent_Id", DBNull.Value);
            var menuType = !string.IsNullOrEmpty(menu_Mas.Menu_type) ? new SqlParameter("@Menu_type", menu_Mas.Menu_type) : new SqlParameter("@Menu_type", DBNull.Value);
            var shortKey = !string.IsNullOrEmpty(menu_Mas.Short_Key) ? new SqlParameter("@Short_Key", menu_Mas.Short_Key) : new SqlParameter("@Short_Key", DBNull.Value);
            var orderNo = menu_Mas.Order_No > 0 ? new SqlParameter("@Order_No", menu_Mas.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var status = new SqlParameter("@Status", menu_Mas.Status);
            var modulePath = !string.IsNullOrEmpty(menu_Mas.Module_Path) ? new SqlParameter("@Module_Path", menu_Mas.Module_Path) : new SqlParameter("@Module_Path", DBNull.Value);
            var recoredType = new SqlParameter("@recordType", "Update");
            var isExistParameter = new SqlParameter("@IsExistsMenu", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Menu_Mas_Insert_Update @Menu_Id, @Menu_Name, @Caption, @Parent_Id, @Menu_type, @Short_Key, @Order_No,
                            @Status, @Module_Path, @recordType, @IsExistsMenu OUT", menuId, menuName, caption, parentId, menuType, shortKey, orderNo, status, modulePath, recoredType, isExistParameter));

            var isExist = (bool)isExistParameter.Value;
            if (isExist)
                return 2;

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    await Insert_Menu_Trace(menu_Mas, "Update");
            //}
            return result;
        }
        public async Task<int> DeleteMenu(int menuId)
        {
            var isReferencedParameter = new SqlParameter("@IsExists", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var _menuId = menuId > 0 ? new SqlParameter("@Menu_Id", menuId) : new SqlParameter("@Menu_Id", DBNull.Value);

            //    var result_menu = await Task.Run(() => _dbContext.Menu_Mas
            //                    .FromSqlRaw(@"exec Menu_Mas_Select @Menu_Id", _menuId).AsEnumerable()
            //                    .FirstOrDefault());

            //    if (result_menu != null)
            //    {
            //        await Insert_Menu_Trace(result_menu, "Delete");
            //    }
            //}

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Menu_Mas_Delete @Menu_Id, @IsExists OUT", new SqlParameter("@Menu_Id", menuId),
                                isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return 2;

            return result;
        }
        public async Task<IList<Menu_Mas>> GetMenu(int menuId)
        {
            var MenuId = menuId > 0 ? new SqlParameter("@Menu_Id", menuId) : new SqlParameter("@Menu_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Menu_Mas
                            .FromSqlRaw(@"exec Menu_Mas_Select @Menu_Id", MenuId).ToListAsync());

            return result;
        }
        public async Task<IList<MenuMasterModel>> Get_all_menus(int employeeId)
        {
            var allMenus = await _dbContext.Menu_Mas.ToListAsync();
            var rights = employeeId > 0 ? await _dbContext.Emp_rights.Where(x => x.Employee_Id == employeeId).ToListAsync() : new List<Emp_rights>();

            var menus = new List<MenuMasterModel>();

            menus = await Get_all_sub_menus(allMenus, rights, 0, employeeId);

            return menus;
        }

        public async Task<List<MenuMasterModel>> Get_all_sub_menus(List<Menu_Mas> allMenus, List<Emp_rights> rights, int parentId, int employeeId)
        {
            var menuList = new List<MenuMasterModel>();

            foreach (var item in allMenus.Where(x => x.Parent_Id == parentId))
            {
                var right_Model = await Set_Menu_Rights(item.Menu_Id, rights);

                var menuModel = new MenuMasterModel
                {
                    Menu_Id = item.Menu_Id,
                    Menu_Name = item.Menu_Name,
                    Caption = item.Caption,
                    Parent_Id = item.Parent_Id ?? 0,
                    Menu_type = item.Menu_type,
                    Short_Key = item.Short_Key,
                    Order_No = item.Order_No,
                    Status = item.Status,
                    Module_Path = item.Module_Path,
                    Menu_Rights = right_Model,
                    SubMenu = new List<MenuMasterModel>()
                };

                menuModel.SubMenu = await Get_all_sub_menus(allMenus, rights, item.Menu_Id, employeeId);
                menuList.Add(menuModel);
            }

            return menuList;
        }

        public async Task<Menu_Rights_Model> Set_Menu_Rights(int menuId, IList<Emp_rights> rights)
        {
            var right = rights.FirstOrDefault(x => x.Menu_Id == menuId);
            return right != null ? new Menu_Rights_Model
            {
                Insert_Allow = right.Insert_Allow,
                Update_Allow = right.Update_Allow,
                Delete_Allow = right.Delete_Allow,
                Excel_Allow = right.Excel_Allow,
                Print_Allow = right.Print_Allow,
                Query_Allow = right.Query_Allow
            } : new Menu_Rights_Model();
        }

        public async Task<int> Get_Menu_Max_Order_No()
        {
            var result = await _dbContext.Menu_Mas.Select(x => x.Order_No).MaxAsync();
            if (result > 0)
            {
                var maxValue = checked((int)result + 1);
                return maxValue;
            }
            return 1;
        }
    }
    #endregion
}
