using astute.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IRapaportService
    {
        #region Rapaport Master
        Task<int> InsertRapaport(Rapaport_Master rapaport_Master);
        Task<int> UpdateRapaport(Rapaport_Master rapaport_Master);
        Task<int> DeleteRapaport(int rapId);
        Task<IList<Rapaport_Master>> GetRapaport(int rapId);
        #endregion

        #region Rapaport Detail
        Task<int> InsertRapaportDetail(DataTable dataTable);
        Task<int> UpdateRapaportDetail(Rapaport_Detail rapaport_Detail);
        Task<int> DeleteRapaportDetail(int rap_Det_Id);
        Task<IList<RapaportPriceModel>> GetRapaportDetail(string? shape, string? color, string? clarity, decimal? frmCts, decimal? toCts, decimal? rate, string? date);
        Task<IList<Rapaport_Color_Value>> Get_Rapaport_Color_Filter_Value();
        Task<IList<Rapaport_Clarity_Value>> Get_Rapaport_Clarity_Filter_Value();
        Task<IList<Rapaport_Date_Value>> Get_Rapaport_Date_Filter_Value();
        Task<IList<Bank_Dropdown_Model>> Get_Rapaport_Color();
        Task<IList<Shape_Value>> Get_Shape_Filter_Value();
        Task<IList<Diamond_Type_Value>> Get_Diamond_Type_Filter_Value();
        #endregion

        #region Rapaport User
        Task<int> InsertRapaportUser(Rapaport_User rapaport_User);
        Task<int> UpdateRapaportUser(Rapaport_User rapaport_User);
        Task<int> DeleteRapaportUser(string rap_User);
        Task<IList<Rapaport_User>> GetRapaportUser(string rap_User);
        #endregion
    }
}
