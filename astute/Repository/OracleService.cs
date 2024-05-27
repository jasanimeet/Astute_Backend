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

                newRow["SHAPE"] = row["SHAPE"];
                newRow["POINTER"] = DBNull.Value;
                newRow["SUBPOINTER"] = row["SUB_POINTER"];
                newRow["COLOR"] = row["PUR_COLOR"];
                newRow["PURITY"] = row["PUR_PURITY"];
                newRow["CUT"] = row["PUR_CUT"];
                newRow["FLS"] = row["PUR_FLS"];
                newRow["PCS"] = Convert.ToInt32(row["ANALIS_PUR_PCS"]);
                newRow["DISC"] = Convert.ToSingle(row["CUR_PUR_DISC"]);
                newRow["KTS_GRD"] = row["PUR_KTS_GRD"];
                newRow["STONE_CLARITY"] = DBNull.Value;
                newRow["VSHADE"] = row["PUR_VSHADE"];
                newRow["LUSTER"] = row["PUR_LUSTER"];
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

                newRow["SHAPE"] = row["SHAPE"];
                newRow["POINTER"] = row["SALE_POINTER"];
                newRow["SUBPOINTER"] = row["SALE_SUBPOINTER"];
                newRow["COLOR"] = row["SALE_COLOR"];
                newRow["PURITY"] = row["SALE_PURITY"];
                newRow["CUT"] = row["SALE_CUT"];
                newRow["FLS"] = row["SALE_FLS"];
                newRow["PCS"] = Convert.ToInt32(row["ANALIS_SALES_PCS"]);
                newRow["DISC"] = Convert.ToSingle(row["SALE_DISC"]);
                newRow["KTS_GRD"] = row["SALE_KTS_GRD"];
                newRow["STONE_CLARITY"] = row["SALE_STONE_CLARITY"];
                newRow["VSHADE"] = row["SALE_VSHADE"];
                newRow["LUSTER"] = row["SALE_LUSTER"];
                newRow["ANALIS_SALES_DAYS"] = row["ANALIS_SALES_DAYS"];
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

                newRow["SHAPE"] = row["SHAPE"];
                newRow["POINTER"] = row["STOCK_POINTER"];
                newRow["SUBPOINTER"] = row["STOCK_SUBPOINTER"];
                newRow["COLOR"] = row["STOCK_COLOR"];
                newRow["PURITY"] = row["STOCK_PURITY"];
                newRow["CUT"] = row["STOCK_CUT"];
                newRow["FLS"] = row["STOCK_FLS"];
                newRow["PCS"] = Convert.ToInt32(row["ANALIS_STOCK_PCS"]);
                newRow["DISC"] = Convert.ToSingle(row["OFFER_DISC_PER"]);
                newRow["KTS_GRD"] = row["STOCK_KTS_GRD"];
                newRow["STONE_CLARITY"] = row["STOCK_STONE_CLARITY"];
                newRow["VSHADE"] = row["STOCK_VSHADE"];
                newRow["LUSTER"] = row["STOCK_LUSTER"];
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
