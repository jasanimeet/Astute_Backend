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
        Task<int> UpdateCategoryValue(Category_Value category_Value);
        Task<int> DeleteCategoryValue(int id);
        Task<Category_Value> GetCategoryValueByCatValId(int catValId);
        Task<IList<CategoryValueModel>> GetCategoryValuesByCatId(int catId);
        Task<IList<CategoryValueModel>> Get_Active_Category_Values(int catId);
        Task<int> ChangeStatus(int cat_val_Id, bool status);
        Task<DataTable> GetCategororyValues(int catId);
        #endregion
    }
}
