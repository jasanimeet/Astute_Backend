using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IBGMService
    {
        Task<int> InsertBGM(BGM_Master bGM_Mas);
        Task<(string, int)> Add_Update_Bgm_Master(BGM_Master bGM_Master);
        Task<int> UpdateBGM(BGM_Master bGM_Mas);
        Task<int> DeleteBGM(int bgm_Id);
        Task<IList<BGM_Master>> GetBgm(int bgm_Id);
        Task<int> BGMChangeStatus(int bgm_Id, bool status);
        Task<int> Insert_BGM_Detail(DataTable dataTable);
        Task<BGM_Master> Get_Bgm_Detail(int bgm_Id);
        Task<int> BGM_Detail_Change_Status(int id, bool status);
    }
}
