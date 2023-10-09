using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface ISupplierService
    {
        Task<int> InsertSupplierValueMapping(Supplier_Value_Mapping supplier_Value);
        Task<int> UpdateSupplierValueMapping(Supplier_Value_Mapping supplier_Value);
        Task<int> DeleteSupplierValueMapping(int supId);
        Task<IList<Supplier_Value_Mapping>> Get_Supplier_Value_Mapping(int sup_Id, int cat_val_Id);

        #region Value Config
        Task<int> Add_Update_Value_Config(Value_Config value_Config);
        Task<int> Delete_Value_Config(int valueMap_ID);
        Task<IList<Value_Config>> Get_Value_Config(int valueMap_ID);
        #endregion
    }
}
