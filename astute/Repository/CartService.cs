using astute.Models;

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
        #endregion

        #region Review
        #endregion

        #region Approval Management
        #endregion
        #endregion
    }
}
