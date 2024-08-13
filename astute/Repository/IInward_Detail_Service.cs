using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IInward_Detail_Service
    {
        #region Import Master
        Task<List<Dictionary<string, object>>> Get_Import_Master();
        Task<int> Insert_Import_Master(Import_Master import_Master, int user_Id);
        Task<int> Update_Import_Master(Import_Master import_Master);
        Task<int> Delete_Import_Master(int id);
        #endregion 

        #region Import Detail
        Task<List<Dictionary<string, object>>> Get_Import_Detail();
        Task<int> Insert_Import_Detail(Import_Detail import_Detail);
        Task<int> Update_Import_Detail(Import_Detail import_Detail);
        Task<int> Delete_Import_Detail(int id);
        #endregion
        #region Import Excel
        Task<List<Dictionary<string, object>>> Get_Import_Excel();
        Task<int> Insert_Update_Import_Excel(DataTable dataTable);
        Task<int> Delete_Import_Excel(int id);
        Task<List<Dictionary<string, object>>> Get_Import_Master_Detail(int import_Id);
        Task<List<Dictionary<string, object>>> Get_Import_Master_Detail_Purchase(int import_Id);
        #endregion

        #region Inward Detail
        Task<List<Dictionary<string, object>>> Get_Inward_Detail(string? Stock_ID, string? ID);

        #endregion
    }
}
