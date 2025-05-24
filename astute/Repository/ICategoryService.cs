using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ICategoryService
    {
        Task<int> InsertCategory(Category_Master category_Master);
        Task<int> UpdateCategory(Category_Master category_Master);
        Task<int> DeleteCategory(int id);
        Task<IList<Category_Master>> GetCategory(int catId, int colId);

        #region Category Value
        Task<int> InsertCategoryValue(Category_Value category_Value);
        Task<int> InsertCategoryValuePricing(int Cat_Val_Id, int Cat_Id);
        Task<int> UpdateCategoryValue(Category_Value category_Value);
        Task<int> DeleteCategoryValue(int id);
        Task<Category_Value> GetCategoryValueByCatValId(int catValId);
        Task<IList<CategoryValueModel>> GetCategoryValuesByCatId(int catId);
        Task<Dictionary<string, List<Dictionary<string, object>>>> Get_All_Category_Values();
        Task<IList<CategoryValueModel>> Get_Active_Category_Values(int catId);
        Task<int> ChangeStatus(int cat_val_Id, bool status);
        Task<DataTable> GetCategororyValues(int catId);
        Task<int> Get_Category_Value_Max_Order_No(int cat_Id);
        Task<List<Dictionary<string, object>>> Get_BGM();
        #endregion

        #region Column Master
        Task<List<Dictionary<string, object>>> Get_Column_Master();
        #endregion

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
    }
}
