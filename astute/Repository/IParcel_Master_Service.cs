using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IParcel_Master_Service
    {
        Task<int> Insert_Update_Parcel_Master(Parcel_Master parcel_Master);
        Task<int> Delete_Parcel_Master(int parcel_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Master(int parcel_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Master_By_Cat_Val_Id(int cat_Val_Id);
        Task<int> Insert_Update_Parcel_Ref_Master(Parcel_Ref_Master parcel_Ref_Master, int user_Id);
        Task<int> Delete_Parcel_Ref_Master(int parcel_Ref_Id, int user_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Ref_Master(int parcel_Ref_Id);
        Task<List<Dictionary<string, object>>> Get_Parcel_Ref();
        Task<int> Insert_Update_Stock_Allocation(DataTable dataTable);
        Task<int> Delete_Stock_Allocation(int Id);
        Task<List<Dictionary<string, object>>> Get_Stock_Allocation(int ac_Grp_Code, int company_Id, int year_Id);
    }
}
