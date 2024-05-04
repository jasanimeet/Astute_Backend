using astute.Models;
using Microsoft.IdentityModel.Protocols;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class Account_Group_Service : IAccount_Group_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public Account_Group_Service(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        //public async Task<int> Create_Update_Account_Group(Account_Group_Master account_Group_Master)
        //{
        //    var 
        //}
        #endregion
    }
}
