using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class Exchange_Rate_Service : IExchange_Rate_Service
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        #endregion

        #region Ctor
        public Exchange_Rate_Service(AstuteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<int> Insert_Update_Exchange_Rate(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Struct_Exchange_Rate", SqlDbType.Structured)
            {
                TypeName = "dbo.Exchange_Rate_Data_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Exchange_Rate_Master_Insert_Update @Struct_Exchange_Rate", parameter);

            return result;
        }
        public async Task<IList<Exchange_Rate_Master>> Get_Exchange_Rate(int exchange_Id)
        {
            var _exchange_Id = exchange_Id > 0 ? new SqlParameter("@Exchange_Id", exchange_Id) : new SqlParameter("@Exchange_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Exchange_Rate_Master
                            .FromSqlRaw(@"exec Exchange_Rate_Master_Select @Exchange_Id", _exchange_Id).ToListAsync());

            return result;
        }
        public async Task Insert_Exchange_Rate_Trace(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblExchange_Rate_Master_Trace", SqlDbType.Structured)
            {
                TypeName = "dbo.Exchange_Rate_Master_Trace_Table_Type",
                Value = dataTable
            };

            await _dbContext.Database.ExecuteSqlRawAsync("EXEC Exchange_Rate_Trace_Insert @tblExchange_Rate_Master_Trace", parameter);
        }
        #endregion
    }
}
