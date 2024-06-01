using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class OracleService : IOracleService
    {

        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly Oracle_DBAccess _dbOracleAccess;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public OracleService(AstuteDbContext dbContext,
            IConfiguration configuration,
            Oracle_DBAccess dbAccess)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _dbOracleAccess = dbAccess;
        }
        #endregion

        #region Methods
        public async Task<int> Get_Pur_Disc()
        {
            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dataTable = await _dbOracleAccess.CallSP("Get_Pur_Disc", paramList);

            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("SHAPE", typeof(string));
            newDataTable.Columns.Add("POINTER", typeof(string));
            newDataTable.Columns.Add("SUBPOINTER", typeof(string));
            newDataTable.Columns.Add("COLOR", typeof(string));
            newDataTable.Columns.Add("PURITY", typeof(string));
            newDataTable.Columns.Add("CUT", typeof(string));
            newDataTable.Columns.Add("FLS", typeof(string));
            newDataTable.Columns.Add("PCS", typeof(int));
            newDataTable.Columns.Add("DISC", typeof(float));
            newDataTable.Columns.Add("KTS_GRD", typeof(string));
            newDataTable.Columns.Add("STONE_CLARITY", typeof(string));
            newDataTable.Columns.Add("VSHADE", typeof(string));
            newDataTable.Columns.Add("LUSTER", typeof(string));
            newDataTable.Columns.Add("ANALIS_SALES_DAYS", typeof(string));
            newDataTable.Columns.Add("TYPE", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["SHAPE"] = (!string.IsNullOrEmpty(row["SHAPE"].ToString())) ? row["SHAPE"] : DBNull.Value;
                newRow["POINTER"] = DBNull.Value;
                newRow["SUBPOINTER"] = (!string.IsNullOrEmpty(row["SUB_POINTER"].ToString())) ? row["SUB_POINTER"] : DBNull.Value;
                newRow["COLOR"] = (!string.IsNullOrEmpty(row["PUR_COLOR"].ToString())) ? row["PUR_COLOR"] : DBNull.Value;
                newRow["PURITY"] = (!string.IsNullOrEmpty(row["PUR_PURITY"].ToString())) ? row["PUR_PURITY"] : DBNull.Value;
                newRow["CUT"] = (!string.IsNullOrEmpty(row["PUR_CUT"].ToString())) ? row["PUR_CUT"] : DBNull.Value;
                newRow["FLS"] = (!string.IsNullOrEmpty(row["PUR_FLS"].ToString())) ? row["PUR_FLS"] : DBNull.Value;
                newRow["PCS"] = (!string.IsNullOrEmpty(row["ANALIS_PUR_PCS"].ToString())) ? Convert.ToInt32(row["ANALIS_PUR_PCS"]) : DBNull.Value;
                newRow["DISC"] = (!string.IsNullOrEmpty(row["CUR_PUR_DISC"].ToString())) ? Convert.ToSingle(row["CUR_PUR_DISC"]) : DBNull.Value;
                newRow["KTS_GRD"] = (!string.IsNullOrEmpty(row["PUR_KTS_GRD"].ToString())) ? row["PUR_KTS_GRD"] : DBNull.Value;
                newRow["STONE_CLARITY"] = DBNull.Value;
                newRow["VSHADE"] = (!string.IsNullOrEmpty(row["PUR_VSHADE"].ToString())) ? row["PUR_VSHADE"] : DBNull.Value;
                newRow["LUSTER"] = (!string.IsNullOrEmpty(row["PUR_LUSTER"].ToString())) ? row["PUR_LUSTER"] : DBNull.Value;
                newRow["ANALIS_SALES_DAYS"] = DBNull.Value;
                newRow["TYPE"] = "P";

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@tableInq", SqlDbType.Structured)
            {
                TypeName = "dbo.Fortune_Discount_Table_Type",
                Value = newDataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC [Fortune_Discount_Ora_Insert_Update] @tableInq", parameter));
            return result;
        }

        public async Task<int> Get_Sal_Disc()
        {
            DateTime from_date = DateTime.Now.AddDays(-31);
            DateTime to_date = DateTime.Now;

            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("p_from_date", OracleDbType.Date);
            param1.Value = from_date;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_date", OracleDbType.Date);
            param1.Value = to_date;
            paramList.Add(param1);

            param1 = new OracleParameter("p_pointer_flag", OracleDbType.NVarchar2);
            param1.Value = "N";
            paramList.Add(param1);

            param1 = new OracleParameter("p_color_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_purity_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_cut_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_fls_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_shade_grp", OracleDbType.NVarchar2);
            param1.Value = "WHITE";
            paramList.Add(param1);

            param1 = new OracleParameter("p_shape", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_subpointer_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts", OracleDbType.NVarchar2);
            param1.Value = "N";
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts_word", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_length", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_length", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_width", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_width", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_depth", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_depth", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_table_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_table_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_depth_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_depth_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_clarity_grade", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts_grd_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_stone_clarity_flag", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_shade_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_luster_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_pre_sold_flag", OracleDbType.NVarchar2);
            param1.Value = "B";
            paramList.Add(param1);

            DataTable dataTable = await _dbOracleAccess.CallSP("get_sal_disc_new", paramList);

            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("SHAPE", typeof(string));
            newDataTable.Columns.Add("POINTER", typeof(string));
            newDataTable.Columns.Add("SUBPOINTER", typeof(string));
            newDataTable.Columns.Add("COLOR", typeof(string));
            newDataTable.Columns.Add("PURITY", typeof(string));
            newDataTable.Columns.Add("CUT", typeof(string));
            newDataTable.Columns.Add("FLS", typeof(string));
            newDataTable.Columns.Add("PCS", typeof(int));
            newDataTable.Columns.Add("DISC", typeof(float));
            newDataTable.Columns.Add("KTS_GRD", typeof(string));
            newDataTable.Columns.Add("STONE_CLARITY", typeof(string));
            newDataTable.Columns.Add("VSHADE", typeof(string));
            newDataTable.Columns.Add("LUSTER", typeof(string));
            newDataTable.Columns.Add("ANALIS_SALES_DAYS", typeof(string));
            newDataTable.Columns.Add("TYPE", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["SHAPE"] = (!string.IsNullOrEmpty(row["SHAPE"].ToString())) ? row["SHAPE"] : DBNull.Value;
                newRow["POINTER"] = (!string.IsNullOrEmpty(row["SALE_POINTER"].ToString())) ? row["SALE_POINTER"] : DBNull.Value;
                newRow["SUBPOINTER"] = (!string.IsNullOrEmpty(row["SALE_SUBPOINTER"].ToString())) ? row["SALE_SUBPOINTER"] : DBNull.Value;
                newRow["COLOR"] = (!string.IsNullOrEmpty(row["SALE_COLOR"].ToString())) ? row["SALE_COLOR"] : DBNull.Value;
                newRow["PURITY"] = (!string.IsNullOrEmpty(row["SALE_PURITY"].ToString())) ? row["SALE_PURITY"] : DBNull.Value;
                newRow["CUT"] = (!string.IsNullOrEmpty(row["SALE_CUT"].ToString())) ? row["SALE_CUT"] : DBNull.Value;
                newRow["FLS"] = (!string.IsNullOrEmpty(row["SALE_FLS"].ToString())) ? row["SALE_FLS"] : DBNull.Value;
                newRow["PCS"] = (!string.IsNullOrEmpty(row["ANALIS_SALES_PCS"].ToString())) ? Convert.ToInt32(row["ANALIS_SALES_PCS"]) : DBNull.Value;
                newRow["DISC"] = (!string.IsNullOrEmpty(row["SALE_DISC"].ToString())) ? Convert.ToSingle(row["SALE_DISC"]) : DBNull.Value;
                newRow["KTS_GRD"] = (!string.IsNullOrEmpty(row["SALE_KTS_GRD"].ToString())) ? row["SALE_KTS_GRD"] : DBNull.Value;
                newRow["STONE_CLARITY"] = (!string.IsNullOrEmpty(row["SALE_STONE_CLARITY"].ToString())) ? row["SALE_STONE_CLARITY"] : DBNull.Value;
                newRow["VSHADE"] = (!string.IsNullOrEmpty(row["SALE_VSHADE"].ToString())) ? row["SALE_VSHADE"] : DBNull.Value;
                newRow["LUSTER"] = (!string.IsNullOrEmpty(row["SALE_LUSTER"].ToString())) ? row["SALE_LUSTER"] : DBNull.Value;
                newRow["ANALIS_SALES_DAYS"] = (!string.IsNullOrEmpty(row["ANALIS_SALES_DAYS"].ToString())) ? row["ANALIS_SALES_DAYS"] : DBNull.Value;
                newRow["TYPE"] = "SD";

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@tableInq", SqlDbType.Structured)
            {
                TypeName = "dbo.Fortune_Discount_Table_Type",
                Value = newDataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC [Fortune_Discount_Ora_Insert_Update] @tableInq", parameter));
            return result;
        }

        public async Task<int> Get_Stock_Kts()
        {
            DateTime date = DateTime.Now;

            Oracle_DBAccess oracleDbAccess = new Oracle_DBAccess(_configuration);
            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("p_for_comp", OracleDbType.Int32);
            param1.Value = 1;
            paramList.Add(param1);

            param1 = new OracleParameter("p_for_date", OracleDbType.Date);
            param1.Value = date;
            paramList.Add(param1);

            param1 = new OracleParameter("p_pointer_flag", OracleDbType.NVarchar2);
            param1.Value = "N";
            paramList.Add(param1);

            param1 = new OracleParameter("p_color_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_purity_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_cut_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_fls_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_shade_grp", OracleDbType.NVarchar2);
            param1.Value = "WHITE";
            paramList.Add(param1);

            param1 = new OracleParameter("p_shape", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_sub_pointer", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts", OracleDbType.NVarchar2);
            param1.Value = "N";
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts_word", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_length", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_length", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_width", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_width", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_depth", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_depth", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_table_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_table_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_from_depth_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_to_depth_per", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_clarity_grade", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            param1 = new OracleParameter("p_luster_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_shade_flag", OracleDbType.NVarchar2);
            param1.Value = "Y";
            paramList.Add(param1);

            param1 = new OracleParameter("p_all_luster", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_kts_grade_flag", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("p_stone_clarity_flag", OracleDbType.NVarchar2);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            DataTable dataTable = await _dbOracleAccess.CallSP("get_stock_disc", paramList);
            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("SHAPE", typeof(string));
            newDataTable.Columns.Add("POINTER", typeof(string));
            newDataTable.Columns.Add("SUBPOINTER", typeof(string));
            newDataTable.Columns.Add("COLOR", typeof(string));
            newDataTable.Columns.Add("PURITY", typeof(string));
            newDataTable.Columns.Add("CUT", typeof(string));
            newDataTable.Columns.Add("FLS", typeof(string));
            newDataTable.Columns.Add("PCS", typeof(int));
            newDataTable.Columns.Add("DISC", typeof(float));
            newDataTable.Columns.Add("KTS_GRD", typeof(string));
            newDataTable.Columns.Add("STONE_CLARITY", typeof(string));
            newDataTable.Columns.Add("VSHADE", typeof(string));
            newDataTable.Columns.Add("LUSTER", typeof(string));
            newDataTable.Columns.Add("ANALIS_SALES_DAYS", typeof(string));
            newDataTable.Columns.Add("TYPE", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["SHAPE"] = (!string.IsNullOrEmpty(row["SHAPE"].ToString())) ? row["SHAPE"] : DBNull.Value;
                newRow["POINTER"] = (!string.IsNullOrEmpty(row["STOCK_POINTER"].ToString())) ? row["STOCK_POINTER"] : DBNull.Value;
                newRow["SUBPOINTER"] = (!string.IsNullOrEmpty(row["STOCK_SUBPOINTER"].ToString())) ? row["STOCK_SUBPOINTER"] : DBNull.Value;
                newRow["COLOR"] = (!string.IsNullOrEmpty(row["STOCK_COLOR"].ToString())) ? row["STOCK_COLOR"] : DBNull.Value;
                newRow["PURITY"] = (!string.IsNullOrEmpty(row["STOCK_PURITY"].ToString())) ? row["STOCK_PURITY"] : DBNull.Value;
                newRow["CUT"] = (!string.IsNullOrEmpty(row["STOCK_CUT"].ToString())) ? row["STOCK_CUT"] : DBNull.Value;
                newRow["FLS"] = (!string.IsNullOrEmpty(row["STOCK_FLS"].ToString())) ? row["STOCK_FLS"] : DBNull.Value;
                newRow["PCS"] = (!string.IsNullOrEmpty(row["ANALIS_STOCK_PCS"].ToString())) ? Convert.ToInt32(row["ANALIS_STOCK_PCS"]) : DBNull.Value;
                newRow["DISC"] = (!string.IsNullOrEmpty(row["OFFER_DISC_PER"].ToString())) ? Convert.ToSingle(row["OFFER_DISC_PER"]) : DBNull.Value;
                newRow["KTS_GRD"] = (!string.IsNullOrEmpty(row["STOCK_KTS_GRD"].ToString())) ? row["STOCK_KTS_GRD"] : DBNull.Value;
                newRow["STONE_CLARITY"] = (!string.IsNullOrEmpty(row["STOCK_STONE_CLARITY"].ToString())) ? row["STOCK_STONE_CLARITY"] : DBNull.Value;
                newRow["VSHADE"] = (!string.IsNullOrEmpty(row["STOCK_VSHADE"].ToString())) ? row["STOCK_VSHADE"] : DBNull.Value;
                newRow["LUSTER"] = (!string.IsNullOrEmpty(row["STOCK_LUSTER"].ToString())) ? row["STOCK_LUSTER"] : DBNull.Value;
                newRow["ANALIS_SALES_DAYS"] = DBNull.Value;
                newRow["TYPE"] = "S";

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@tableInq", SqlDbType.Structured)
            {
                TypeName = "dbo.Fortune_Discount_Table_Type",
                Value = newDataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC [Fortune_Discount_Ora_Insert_Update] @tableInq", parameter));
            return result;
        }

        public async Task<(int, int, int)> Get_Fortune_Discount()
        {
            var Pur_Disc = await Get_Pur_Disc();
            var Sal_Disc = await Get_Sal_Disc();
            var Stock_Kts = await Get_Stock_Kts();
            return (Pur_Disc, Sal_Disc, Stock_Kts);
        }
        #endregion

    }
}
