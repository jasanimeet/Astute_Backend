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
    }
}
