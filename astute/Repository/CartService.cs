using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class CartService : ICartService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public CartService(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        #region Cart
        public async Task<int> Insert_Cart_Review_Aproval_Management(Cart_Review_Approval_Management cart_Review_Approval_Management)
        {
            var supp_Stock_Ids = !string.IsNullOrEmpty(cart_Review_Approval_Management.Supp_Stock_Ids) ? new SqlParameter("@Supp_Stock_Ids", cart_Review_Approval_Management.Supp_Stock_Ids) : new SqlParameter("@Supp_Stock_Ids", DBNull.Value);
            var user_Id = cart_Review_Approval_Management.User_Id > 0 ? new SqlParameter("@User_Id", cart_Review_Approval_Management.User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(cart_Review_Approval_Management.Upload_Type) ? new SqlParameter("@Upload_Type", cart_Review_Approval_Management.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var is_Exists = new SqlParameter("@IsExist", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Cart_Review_Approval_Management_Insert_Update @Supp_Stock_Ids, @User_Id, @Upload_Type, @IsExist OUT", supp_Stock_Ids, user_Id, upload_Type, is_Exists));
            var _is_Exists = (bool)is_Exists.Value;
            if (_is_Exists)
                return 409;

            return result;
        }
        #endregion

        #region Review
        #endregion

        #region Approval Management
        #endregion
        #endregion
    }
}
