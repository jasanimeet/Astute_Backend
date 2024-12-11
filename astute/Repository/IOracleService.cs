using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static astute.Models.Employee_Master;

namespace astute.Repository
{
    public partial interface IOracleService
    {
        Task<int> Get_Fortune_Purchase_Disc();
        Task<int> Get_Fortune_Sale_Disc();
        Task<int> Get_Fortune_Stock_Disc();
        Task<int> Get_Fortune_Sale_Disc_Kts();
        Task<int> Get_Fortune_Stock_Disc_Kts();
        Task<(int, int, int, int, int)> Get_Fortune_Discount();
        Task<int> Get_Fortune_Party();
        Task<int> Get_Fortune_Party_Master();
        Task<int> Order_Data_Transfer_Oracle(IList<Order_Processing_Complete_Detail> order_Processing_Complete_Details, Employee_Fortune_Order_Master employee_Fortune_Master, string Summary_QC_Remarks);
        Task<int> Lab_Entry_Notification();
        Task<int> Order_Data_Detail_Transfer_Oracle(IList<Order_Processing_Complete_Fortune_Detail> order_Processing_Complete_Fortune_Details);
    }
}
