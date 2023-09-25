using astute.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IBGMService
    {
        Task<int> InsertBGM(BGM_Master bGM_Mas);
        Task<int> UpdateBGM(BGM_Master bGM_Mas);
        Task<int> DeleteBGM(int bgm_Id);
        Task<IList<BGM_Master>> GetBgm(int bgm_Id, int shade, int milky);
        Task<int> BGMChangeStatus(int bgm_Id, bool status);
    }
}
