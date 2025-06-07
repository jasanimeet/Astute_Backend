using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IProcessService
    {
        Task<int> InsertProcessMas(Process_Master process_Mas);
        Task<int> UpdateProcessMas(Process_Master process_Mas);
        Task<int> DeleteProcessMas(int proccessId);
        Task<IList<Process_Master>> GetProcess(int processId);
        Task<int> StateChangeStatus(int process_Id, bool status);
        Task<int> Get_Process_Max_Order_No();
        Task<IList<DropdownModel>> GetProcessByType(string process_Type);
    }
}
