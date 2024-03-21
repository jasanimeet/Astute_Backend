using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class LabUserService : ILabUserService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public LabUserService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> Create_Update_Lab_User(DataTable dataTable, int party_Id)
        {
            var parameter = new SqlParameter("@tblLab_User", SqlDbType.Structured)
            {
                TypeName = "dbo.Lab_User_Table_Type",
                Value = dataTable
            };
            var _party_Id = new SqlParameter("@Party_Id", party_Id);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Lab_User_Insert_Update] @tblLab_User, @Party_Id",
                        parameter, _party_Id));

            return result;
        }
        #endregion
    }
}
