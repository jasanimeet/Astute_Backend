using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IMenuService
    {
        Task<int> InsertMenu(Menu_Mas menu_Mas);
        Task<int> UpdateMenu(Menu_Mas menu_Mas);
        Task<int> DeleteMenu(int menuId);
        Task<IList<Menu_Mas>> GetMenu(int menuId);
        Task<IList<MenuMasterModel>> Get_all_menus(int employeeId);
        Task<List<MenuMasterModel>> Get_all_sub_menus(List<Menu_Mas> allMenus, List<Emp_rights> rights, int parentId, int employeeId);
        Task<Menu_Rights_Model> Set_Menu_Rights(int menuId, IList<Emp_rights> rights);
        Task<int> Get_Menu_Max_Order_No();
        Task<IList<MenuDownloadShareMasterModel>> Get_All_Menus_Rights(int employeeId);
        Task<List<MenuDownloadShareMasterModel>> Get_All_Sub_Menus_Rights(List<Menu_Mas> allMenus, List<Employee_Download_Share_Rights> download_rights, List<Emp_rights> rights, int parentId, int employeeId);
        Task<Menu_Download_Share_Rights_Model> Set_Menu_Download_Share_Rights(int menuId, IList<Employee_Download_Share_Rights> rights);
    }
}
