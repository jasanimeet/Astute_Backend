using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IPointerService
    {
        Task Insert_Pointer_Detail_Trace(DataTable dataTable);
        #region Pointer Master
        Task<(string, int)> Add_Update_Pointer(Pointer_Master pointer_Mas);
        Task<Pointer_Master> Get_Pointer_Details(int pointer_Id);
        Task<int> UpdatePointer(Pointer_Master pointer_Mas);
        Task<int> DeletePointer(int pointerId);
        Task<IList<Pointer_Master>> GetPointer(int pointerId);
        Task<int> PointerChangeStatus(int pointer_Id, bool status);
        Task<int> Get_Pointer_Master_Max_Order_No();
        Task<IList<Pointer_Master>> Get_Pointer_For_Stock_Generation(int pointerId, string stock_type, int company_Id, string shape_Id);
        #endregion

        #region Pointer Detail
        Task<int> InsertPointerDetail(DataTable dataTable);
        Task<IList<Pointer_Detail>> GetPointerDetail(int pointerDetId, int pointerId);
        Task<int> PointerDetailChangeStatus(int pointer_Det_Id, bool status);
        #endregion
    }
}
