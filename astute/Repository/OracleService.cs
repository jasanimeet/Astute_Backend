using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public async Task<int> Get_Fortune_Purchase_Disc()
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

        public async Task<int> Get_Fortune_Sale_Disc()
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
            param1.Value = DBNull.Value;
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

        public async Task<int> Get_Fortune_Stock_Disc()
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

        public async Task<int> Get_Fortune_Sale_Disc_Kts()
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
                newRow["TYPE"] = "SDK";

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

        public async Task<int> Get_Fortune_Stock_Disc_Kts()
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
            param1.Value = "Y";
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
                newRow["TYPE"] = "SK";

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

        public async Task<(int, int, int, int, int)> Get_Fortune_Discount()
        {
            var Fortune_Purchase_Disc = await Get_Fortune_Purchase_Disc();
            var Fortune_Sale_Disc = await Get_Fortune_Sale_Disc();
            var Fortune_Stock_Disc = await Get_Fortune_Stock_Disc();
            var Fortune_Sale_Disc_Kts = await Get_Fortune_Sale_Disc_Kts();
            var Fortune_Stock_Disc_Kts = await Get_Fortune_Stock_Disc_Kts();

            return (Fortune_Purchase_Disc, Fortune_Sale_Disc, Fortune_Stock_Disc, Fortune_Sale_Disc_Kts, Fortune_Stock_Disc_Kts);
        }

        public async Task<int> Get_Fortune_Party()
        {
            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dataTable = await _dbOracleAccess.CallSP("party_info_transfer", paramList);

            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("PARTY_CODE", typeof(string));
            newDataTable.Columns.Add("PARTY_NAME", typeof(string));
            newDataTable.Columns.Add("ADDRESS_1", typeof(string));
            newDataTable.Columns.Add("ADDRESS_2", typeof(string));
            newDataTable.Columns.Add("ADDRESS_3", typeof(string));
            newDataTable.Columns.Add("CITY", typeof(string));
            newDataTable.Columns.Add("STATE", typeof(string));
            newDataTable.Columns.Add("COUNTRY", typeof(string));
            newDataTable.Columns.Add("PIN_CODE", typeof(string));
            newDataTable.Columns.Add("PHONE_NO", typeof(string));
            newDataTable.Columns.Add("FAX_NO", typeof(string));
            newDataTable.Columns.Add("EMAIL", typeof(string));
            newDataTable.Columns.Add("WEBSITE", typeof(string));
            newDataTable.Columns.Add("JOIN_DATE", typeof(DateTime));
            newDataTable.Columns.Add("LEAVE_DATE", typeof(DateTime));
            newDataTable.Columns.Add("CUSTOMER", typeof(string));
            newDataTable.Columns.Add("SUPPLIER", typeof(string));
            newDataTable.Columns.Add("BROKER", typeof(string));
            newDataTable.Columns.Add("LAB", typeof(string));
            newDataTable.Columns.Add("KYC", typeof(string));
            newDataTable.Columns.Add("MOBILE_NO", typeof(string));
            newDataTable.Columns.Add("MOBILE_NO_1", typeof(string));
            newDataTable.Columns.Add("PHONE_NO_1", typeof(string));
            newDataTable.Columns.Add("EMAIL_1", typeof(string));
            newDataTable.Columns.Add("INVOICE_GROUP", typeof(string));
            newDataTable.Columns.Add("ASS1_CODE", typeof(string));
            newDataTable.Columns.Add("ASS1_PER", typeof(int));
            newDataTable.Columns.Add("ASS2_CODE", typeof(string));
            newDataTable.Columns.Add("ASS2_PER", typeof(int));
            newDataTable.Columns.Add("ASS3_CODE", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["PARTY_CODE"] = (!string.IsNullOrEmpty(row["PARTY_CODE"].ToString())) ? row["PARTY_CODE"] : DBNull.Value;
                newRow["PARTY_NAME"] = (!string.IsNullOrEmpty(row["PARTY_NAME"].ToString())) ? row["PARTY_NAME"] : DBNull.Value;
                newRow["ADDRESS_1"] = (!string.IsNullOrEmpty(row["ADDRESS_1"].ToString())) ? row["ADDRESS_1"] : DBNull.Value;
                newRow["ADDRESS_2"] = (!string.IsNullOrEmpty(row["ADDRESS_2"].ToString())) ? row["ADDRESS_2"] : DBNull.Value;
                newRow["ADDRESS_3"] = (!string.IsNullOrEmpty(row["ADDRESS_3"].ToString())) ? row["ADDRESS_3"] : DBNull.Value;
                newRow["CITY"] = (!string.IsNullOrEmpty(row["CITY"].ToString())) ? row["CITY"] : DBNull.Value;
                newRow["STATE"] = (!string.IsNullOrEmpty(row["STATE"].ToString())) ? row["STATE"] : DBNull.Value;
                newRow["COUNTRY"] = (!string.IsNullOrEmpty(row["COUNTRY"].ToString())) ? row["COUNTRY"] : DBNull.Value;
                newRow["PIN_CODE"] = (!string.IsNullOrEmpty(row["PIN_CODE"].ToString())) ? row["PIN_CODE"] : DBNull.Value;
                newRow["PHONE_NO"] = (!string.IsNullOrEmpty(row["PHONE_NO"].ToString())) ? row["PHONE_NO"] : DBNull.Value;
                newRow["FAX_NO"] = (!string.IsNullOrEmpty(row["FAX_NO"].ToString())) ? row["FAX_NO"] : DBNull.Value;
                newRow["EMAIL"] = (!string.IsNullOrEmpty(row["EMAIL"].ToString())) ? row["EMAIL"] : DBNull.Value;
                newRow["WEBSITE"] = (!string.IsNullOrEmpty(row["WEBSITE"].ToString())) ? row["WEBSITE"] : DBNull.Value;
                newRow["JOIN_DATE"] = (!string.IsNullOrEmpty(row["JOIN_DATE"].ToString())) ? Convert.ToDateTime(row["JOIN_DATE"]) : DBNull.Value;
                newRow["LEAVE_DATE"] = (!string.IsNullOrEmpty(row["LEAVE_DATE"].ToString())) ? Convert.ToDateTime(row["LEAVE_DATE"]) : DBNull.Value;
                newRow["CUSTOMER"] = (!string.IsNullOrEmpty(row["CUSTOMER"].ToString())) ? row["CUSTOMER"] : DBNull.Value;
                newRow["SUPPLIER"] = (!string.IsNullOrEmpty(row["SUPPLIER"].ToString())) ? row["SUPPLIER"] : DBNull.Value;
                newRow["BROKER"] = (!string.IsNullOrEmpty(row["BROKER"].ToString())) ? row["BROKER"] : DBNull.Value;
                newRow["LAB"] = (!string.IsNullOrEmpty(row["LAB"].ToString())) ? row["LAB"] : DBNull.Value;
                newRow["KYC"] = (!string.IsNullOrEmpty(row["KYC"].ToString())) ? row["KYC"] : DBNull.Value;
                newRow["MOBILE_NO"] = (!string.IsNullOrEmpty(row["MOBILE_NO"].ToString())) ? row["MOBILE_NO"] : DBNull.Value;
                newRow["MOBILE_NO_1"] = (!string.IsNullOrEmpty(row["MOBILE_NO_1"].ToString())) ? row["MOBILE_NO_1"] : DBNull.Value;
                newRow["PHONE_NO_1"] = (!string.IsNullOrEmpty(row["PHONE_NO_1"].ToString())) ? row["PHONE_NO_1"] : DBNull.Value;
                newRow["EMAIL_1"] = (!string.IsNullOrEmpty(row["EMAIL_1"].ToString())) ? row["EMAIL_1"] : DBNull.Value;
                newRow["INVOICE_GROUP"] = (!string.IsNullOrEmpty(row["INVOICE_GROUP"].ToString())) ? row["INVOICE_GROUP"] : DBNull.Value;
                newRow["ASS1_CODE"] = (!string.IsNullOrEmpty(row["ASS1_CODE"].ToString())) ? row["ASS1_CODE"] : DBNull.Value;
                newRow["ASS1_PER"] = (!string.IsNullOrEmpty(row["ASS1_PER"].ToString())) ? Convert.ToInt32(row["ASS1_PER"]) : DBNull.Value;
                newRow["ASS2_CODE"] = (!string.IsNullOrEmpty(row["ASS2_CODE"].ToString())) ? row["ASS2_CODE"] : DBNull.Value;
                newRow["ASS2_PER"] = (!string.IsNullOrEmpty(row["ASS2_PER"].ToString())) ? Convert.ToInt32(row["ASS2_PER"]) : DBNull.Value;
                newRow["ASS3_CODE"] = (!string.IsNullOrEmpty(row["ASS3_CODE"].ToString())) ? row["ASS3_CODE"] : DBNull.Value;

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@tableInq", SqlDbType.Structured)
            {
                TypeName = "dbo.Fortune_Party_Table_Type",
                Value = newDataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC [Fortune_Party_Ora_Insert_Update] @tableInq", parameter));
            return result;
        }

        public async Task<int> Get_Fortune_Party_Master()
        {
            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dataTable_Party_Info = await _dbOracleAccess.CallSP("party_info_transfer", paramList);            

            DataTable newDataTable_Party_Info = new DataTable();

            newDataTable_Party_Info.Columns.Add("CUSTOMER", typeof(string));
            newDataTable_Party_Info.Columns.Add("SUPPLIER", typeof(string));
            newDataTable_Party_Info.Columns.Add("LAB", typeof(string));
            newDataTable_Party_Info.Columns.Add("PARTY_CODE", typeof(string));
            newDataTable_Party_Info.Columns.Add("PARTY_NAME", typeof(string));
            newDataTable_Party_Info.Columns.Add("ADDRESS_1", typeof(string));
            newDataTable_Party_Info.Columns.Add("ADDRESS_2", typeof(string));
            newDataTable_Party_Info.Columns.Add("ADDRESS_3", typeof(string));
            newDataTable_Party_Info.Columns.Add("CITY", typeof(string));
            newDataTable_Party_Info.Columns.Add("PIN_CODE", typeof(string));
            newDataTable_Party_Info.Columns.Add("MOBILE_NO", typeof(string));
            newDataTable_Party_Info.Columns.Add("MOBILE_NO_1", typeof(string));
            newDataTable_Party_Info.Columns.Add("PHONE_NO", typeof(string));
            newDataTable_Party_Info.Columns.Add("PHONE_NO_1", typeof(string));
            newDataTable_Party_Info.Columns.Add("FAX_NO", typeof(string));
            newDataTable_Party_Info.Columns.Add("EMAIL", typeof(string));
            newDataTable_Party_Info.Columns.Add("EMAIL_1", typeof(string));
            newDataTable_Party_Info.Columns.Add("WEBSITE", typeof(string));
            newDataTable_Party_Info.Columns.Add("INVOICE_GROUP", typeof(string));
            newDataTable_Party_Info.Columns.Add("LEAVE_DATE", typeof(DateTime));
            newDataTable_Party_Info.Columns.Add("ASS1_CODE", typeof(int));
            newDataTable_Party_Info.Columns.Add("ASS1_PER", typeof(float));
            newDataTable_Party_Info.Columns.Add("ASS2_CODE", typeof(int));
            newDataTable_Party_Info.Columns.Add("ASS2_PER", typeof(float));
            newDataTable_Party_Info.Columns.Add("ASS3_CODE", typeof(int));
            newDataTable_Party_Info.Columns.Add("TRANS_DATE", typeof(DateTime));
            newDataTable_Party_Info.Columns.Add("ALIAS_NAME", typeof(string));
            newDataTable_Party_Info.Columns.Add("SKYPE_ID", typeof(string));
            newDataTable_Party_Info.Columns.Add("WECHATID", typeof(string));

            foreach (DataRow row in dataTable_Party_Info.Rows)
            {
                DataRow newRow = newDataTable_Party_Info.NewRow();

                newRow["CUSTOMER"] = DBNull.Value.Equals(row["CUSTOMER"]) ? DBNull.Value : row["CUSTOMER"].ToString();
                newRow["SUPPLIER"] = DBNull.Value.Equals(row["SUPPLIER"]) ? DBNull.Value : row["SUPPLIER"].ToString();
                newRow["LAB"] = DBNull.Value.Equals(row["LAB"]) ? DBNull.Value : row["LAB"].ToString();
                newRow["PARTY_CODE"] = DBNull.Value.Equals(row["PARTY_CODE"]) ? DBNull.Value : row["PARTY_CODE"].ToString();
                newRow["PARTY_NAME"] = DBNull.Value.Equals(row["PARTY_NAME"]) ? DBNull.Value : row["PARTY_NAME"].ToString();
                newRow["ADDRESS_1"] = DBNull.Value.Equals(row["ADDRESS_1"]) ? DBNull.Value : row["ADDRESS_1"].ToString();
                newRow["ADDRESS_2"] = DBNull.Value.Equals(row["ADDRESS_2"]) ? DBNull.Value : row["ADDRESS_2"].ToString();
                newRow["ADDRESS_3"] = DBNull.Value.Equals(row["ADDRESS_3"]) ? DBNull.Value : row["ADDRESS_3"].ToString();
                newRow["CITY"] = DBNull.Value.Equals(row["CITY"]) ? DBNull.Value : row["CITY"].ToString();
                newRow["PIN_CODE"] = DBNull.Value.Equals(row["PIN_CODE"]) ? DBNull.Value : row["PIN_CODE"].ToString();
                newRow["MOBILE_NO"] = DBNull.Value.Equals(row["MOBILE_NO"]) ? DBNull.Value : row["MOBILE_NO"].ToString();
                newRow["MOBILE_NO_1"] = DBNull.Value.Equals(row["MOBILE_NO_1"]) ? DBNull.Value : row["MOBILE_NO_1"].ToString();
                newRow["PHONE_NO"] = DBNull.Value.Equals(row["PHONE_NO"]) ? DBNull.Value : row["PHONE_NO"].ToString();
                newRow["PHONE_NO_1"] = DBNull.Value.Equals(row["PHONE_NO_1"]) ? DBNull.Value : row["PHONE_NO_1"].ToString();
                newRow["FAX_NO"] = DBNull.Value.Equals(row["FAX_NO"]) ? DBNull.Value : row["FAX_NO"].ToString();
                newRow["EMAIL"] = DBNull.Value.Equals(row["EMAIL"]) ? DBNull.Value : row["EMAIL"].ToString();
                newRow["EMAIL_1"] = DBNull.Value.Equals(row["EMAIL_1"]) ? DBNull.Value : row["EMAIL_1"].ToString();
                newRow["WEBSITE"] = DBNull.Value.Equals(row["WEBSITE"]) ? DBNull.Value : row["WEBSITE"].ToString();
                newRow["INVOICE_GROUP"] = DBNull.Value.Equals(row["INVOICE_GROUP"]) ? DBNull.Value : row["INVOICE_GROUP"].ToString();
                newRow["LEAVE_DATE"] = DBNull.Value.Equals(row["LEAVE_DATE"]) || string.IsNullOrEmpty(row["LEAVE_DATE"].ToString()) ? DBNull.Value : (object)Convert.ToDateTime(row["LEAVE_DATE"]);
                newRow["TRANS_DATE"] = DBNull.Value.Equals(row["TRANS_DATE"]) || string.IsNullOrEmpty(row["TRANS_DATE"].ToString()) ? DBNull.Value : (object)Convert.ToDateTime(row["TRANS_DATE"]);
                newRow["ASS1_CODE"] = DBNull.Value.Equals(row["ASS1_CODE"]) || string.IsNullOrEmpty(row["ASS1_CODE"].ToString()) ? DBNull.Value : (object)Convert.ToInt32(row["ASS1_CODE"]);
                newRow["ASS1_PER"] = DBNull.Value.Equals(row["ASS1_PER"]) || string.IsNullOrEmpty(row["ASS1_PER"].ToString()) ? DBNull.Value : (object)Convert.ToSingle(row["ASS1_PER"]);
                newRow["ASS2_CODE"] = DBNull.Value.Equals(row["ASS2_CODE"]) || string.IsNullOrEmpty(row["ASS2_CODE"].ToString()) ? DBNull.Value : (object)Convert.ToInt32(row["ASS2_CODE"]);
                newRow["ASS2_PER"] = DBNull.Value.Equals(row["ASS2_PER"]) || string.IsNullOrEmpty(row["ASS2_PER"].ToString()) ? DBNull.Value : (object)Convert.ToSingle(row["ASS2_PER"]);
                newRow["ASS3_CODE"] = DBNull.Value.Equals(row["ASS3_CODE"]) ? DBNull.Value : row["ASS3_CODE"].ToString();
                newRow["ALIAS_NAME"] = (!string.IsNullOrEmpty(row["SHORT_NAME"].ToString())) ? row["SHORT_NAME"] : DBNull.Value;
                newRow["SKYPE_ID"] = (!string.IsNullOrEmpty(row["SKYPE_ID"].ToString())) ? row["SKYPE_ID"] : DBNull.Value;
                newRow["WECHATID"] = (!string.IsNullOrEmpty(row["WECHATID"].ToString())) ? row["WECHATID"] : DBNull.Value;

                newDataTable_Party_Info.Rows.Add(newRow);
            }

            DataTable dataTable_Party_Contact = await _dbOracleAccess.CallSP("party_contact_transfer", paramList);

            DataTable newDataTable_Party_Contact = new DataTable();

            newDataTable_Party_Contact.Columns.Add("PARTY_CODE", typeof(string));
            newDataTable_Party_Contact.Columns.Add("FIRST_NAME", typeof(string));
            newDataTable_Party_Contact.Columns.Add("DESIGNATION_ID", typeof(string));
            newDataTable_Party_Contact.Columns.Add("MOBILE_NO", typeof(string));
            newDataTable_Party_Contact.Columns.Add("PHONE_NO", typeof(string));
            newDataTable_Party_Contact.Columns.Add("EMAIL", typeof(string));
            newDataTable_Party_Contact.Columns.Add("SRNO", typeof(int));

            foreach (DataRow row in dataTable_Party_Contact.Rows)
            {
                DataRow newRow = newDataTable_Party_Contact.NewRow();
                
                newRow["PARTY_CODE"] = DBNull.Value.Equals(row["PARTY_CODE"]) ? DBNull.Value : row["PARTY_CODE"].ToString();
                newRow["FIRST_NAME"] = DBNull.Value.Equals(row["NAME"]) ? DBNull.Value : row["NAME"].ToString();
                newRow["DESIGNATION_ID"] = DBNull.Value.Equals(row["DESG"]) ? DBNull.Value : row["DESG"].ToString();
                newRow["MOBILE_NO"] = DBNull.Value.Equals(row["MOBILE_NO"]) ? DBNull.Value : row["MOBILE_NO"].ToString();
                newRow["PHONE_NO"] = DBNull.Value.Equals(row["PHONE_NO"]) ? DBNull.Value : row["PHONE_NO"].ToString();
                newRow["EMAIL"] = DBNull.Value.Equals(row["EMAIL"]) ? DBNull.Value : row["EMAIL"].ToString();
                newRow["SRNO"] = DBNull.Value.Equals(row["SRNO"]) || string.IsNullOrEmpty(row["SRNO"].ToString()) ? DBNull.Value : (object)Convert.ToInt32(row["SRNO"]);

                newDataTable_Party_Contact.Rows.Add(newRow);
            }

            var parameter_Fortune_Party_Master_Table_Type = new SqlParameter("@Fortune_Party_Master_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Fortune_Party_Master_Table_Type]",
                Value = newDataTable_Party_Info
            };
            
            var parameter_Fortune_Party_Contact_Table_Type = new SqlParameter("@Fortune_Party_Contact_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Fortune_Party_Contact_Table_Type]",
                Value = newDataTable_Party_Contact
            };

            _dbContext.Database.SetCommandTimeout(1800);
            var result = await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC [Fortune_Party_Master_Ora_Insert_Update] @Fortune_Party_Master_Table_Type, @Fortune_Party_Contact_Table_Type", 
                parameter_Fortune_Party_Master_Table_Type, 
                parameter_Fortune_Party_Contact_Table_Type);

            return result;
        }

        public async Task<int> Order_Data_Transfer_Oracle(IList<Order_Processing_Complete_Detail> order_Processing_Complete_Details, Employee_Fortune_Order_Master employee_Fortune_Master, string Summary_QC_Remarks, string vEntry_type, string type)
        {
            string lab_trans_status = "";

            if (employee_Fortune_Master != null)
            {
                List<OracleParameter> paramList = new List<OracleParameter>();

                OracleParameter param1 = new OracleParameter("vuser_code", OracleDbType.Int32);
                param1.Value = employee_Fortune_Master.Fortune_Id;
                paramList.Add(param1);

                param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
                param1.Direction = ParameterDirection.Output;
                paramList.Add(param1);

                param1 = new OracleParameter("vEntry_type", OracleDbType.NVarchar2);
                param1.Value = vEntry_type;
                paramList.Add(param1);

                param1 = new OracleParameter("vparty_name", OracleDbType.NVarchar2);
                param1.Value = order_Processing_Complete_Details[0].Customer;
                paramList.Add(param1);

                param1 = new OracleParameter("vparty_code", OracleDbType.Int32);
                param1.Value = !string.IsNullOrEmpty(order_Processing_Complete_Details[0].Party_code) ? Convert.ToInt32(order_Processing_Complete_Details[0].Party_code) : 0;
                paramList.Add(param1);

                DataTable mas_dt = await _dbOracleAccess.CallSP("web_trans.lab_trans", paramList);

                if (mas_dt != null && mas_dt.Rows.Count > 0)
                {
                    lab_trans_status = Convert.ToString(mas_dt.Rows[0]["STATUS"]);
                }

                List<int> Order_Ids_List = new List<int>();

                if (!string.IsNullOrEmpty(lab_trans_status) && order_Processing_Complete_Details != null && order_Processing_Complete_Details.Count > 0)
                {
                    foreach (var item in order_Processing_Complete_Details)
                    {
                        Order_Ids_List.Add(item.Id);
                        paramList = new List<OracleParameter>();

                        param1 = new OracleParameter("vtrans_id", OracleDbType.Int32);
                        param1.Value = lab_trans_status;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vREF_NO", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.StockId) ? Convert.ToString(item.StockId) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vLAB", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Lab) ? Convert.ToString(item.Lab) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSHAPE", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Shape) ? Convert.ToString(item.Shape) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCTS", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.Cts) ? Convert.ToDouble(item.Cts) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCOLOR", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Color) ? Convert.ToString(item.Color) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPURITY", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Clarity) ? Convert.ToString(item.Clarity) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCUT", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Cut) ? Convert.ToString(item.Cut) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPOLISH", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Polish) ? Convert.ToString(item.Polish) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSYMM", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Symm) ? Convert.ToString(item.Symm) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vFLS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.FlsIntensity) ? Convert.ToString(item.FlsIntensity) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPP_OFFER_PER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.BaseDisc) ? Convert.ToDouble(item.BaseDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vOFFER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CurrentCostDisc) ? Convert.ToDouble(item.CurrentCostDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSOURCE_PARTY", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Customer) ? Convert.ToString(item.Customer) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vcerti_no", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.CertificateNo) ? Convert.ToString(item.CertificateNo) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPP_BASE_VALUE", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.BaseAmount) ? Convert.ToDouble(item.BaseAmount) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vRAP_PRICE", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.RapRate) ? Convert.ToDouble(item.RapRate) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vRAP_VALUE", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.RapAmount) ? Convert.ToDouble(item.RapAmount) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vLENGTH", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.Length) ? Convert.ToDouble(item.Length) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vWIDTH", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.Width) ? Convert.ToDouble(item.Width) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vDEPTH", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.Depth) ? Convert.ToDouble(item.Depth) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vDEPTH_PER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.DepthPer) ? Convert.ToDouble(item.DepthPer) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_ANGEL", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CrownAngle) ? Convert.ToDouble(item.CrownAngle) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_HEIGHT", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CrownHeight) ? Convert.ToDouble(item.CrownHeight) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPAV_ANGEL", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.PavilionAngle) ? Convert.ToDouble(item.PavilionAngle) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPAV_HEIGHT", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.PavilionHeight) ? Convert.ToDouble(item.PavilionHeight) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vculet", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Culet) ? Convert.ToString(item.Culet) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSYMBOL", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.KeyToSymbol) ? Convert.ToString(item.KeyToSymbol) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vTABLE_PER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.TablePer) ? Convert.ToDouble(item.TablePer) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_NATTS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.SideBlack) ? Convert.ToString(item.SideBlack) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_INCLUSION", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.SideWhite) ? Convert.ToString(item.SideWhite) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vTABLE_NATTS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.TableBlack) ? Convert.ToString(item.TableBlack) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vTABLE_INCLUSION", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.TableWhite) ? Convert.ToString(item.TableWhite) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vFINAL_VALUE", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.OfferAmount) ? Convert.ToDouble(item.OfferAmount) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vFINAL_DISC_PER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.OfferDisc) ? Convert.ToDouble(item.OfferDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vIMG_PATH", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.ImageLink) ? Convert.ToString(item.ImageLink) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vVDO_PATH", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.VideoLink) ? Convert.ToString(item.VideoLink) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vDNA_PATH", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Dna) ? Convert.ToString(item.Dna) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPARTY_STONE_NO", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.SupplierNo) ? Convert.ToString(item.SupplierNo) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPPLIER_NAME", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Company) ? Convert.ToString(item.Company) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPP_FINAL_VALUE", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CurrentCostAmount) ? Convert.ToDouble(item.CurrentCostAmount) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPP_FINAL_DISC", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CurrentCostDisc) ? Convert.ToDouble(item.CurrentCostDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_PER", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.GirdlePer) ? Convert.ToDouble(item.GirdlePer.LongCount()) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_TYPE", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCOMMENTS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.GiaComments) ? Convert.ToString(item.GiaComments) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSTR_LN", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vLR_HALF", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSHADE", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Shade) ? Convert.ToString(item.Shade) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vluster", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Milky) ? Convert.ToString(item.Milky) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vLASER_INCLUSION", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.LaserInscription) ? Convert.ToString(item.LaserInscription) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vcer_path", OracleDbType.NVarchar2);
                        //param1.Value = !string.IsNullOrEmpty(item.CertificateLink) ? Convert.ToString(item.CertificateLink) : DBNull.Value;
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vTABLE_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.TableOpen) ? Convert.ToString(item.TableOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.GirdleOpen) ? Convert.ToString(item.GirdleOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.CrownOpen) ? Convert.ToString(item.CrownOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPAV_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.PavilionOpen) ? Convert.ToString(item.PavilionOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vBGM", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Bgm) ? Convert.ToString(item.Bgm) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSTATUS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Status) ? Convert.ToString(item.Status) : DBNull.Value;

                        paramList.Add(param1);

                        param1 = new OracleParameter("vQC_REQUIRE", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(Summary_QC_Remarks) ? Convert.ToString(Summary_QC_Remarks) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vorigin", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
                        param1.Direction = ParameterDirection.Output;
                        paramList.Add(param1);

                        var det_dt = await _dbOracleAccess.CallSP("web_trans.lab_trans_det", paramList);

                    }
                }
                if (type == "O")
                {
                    
                    string Order_Ids = string.Join(",",Order_Ids_List);

                    var _order_Ids = new SqlParameter("@Order_Ids", !string.IsNullOrEmpty(Order_Ids) ? Order_Ids : DBNull.Value);
                    var _trans_Id = new SqlParameter("@Trans_Id", !string.IsNullOrEmpty(lab_trans_status) ? Convert.ToInt32(lab_trans_status) : DBNull.Value);


                    var result = await Task.Run(() => _dbContext.Database
                           .ExecuteSqlRawAsync(@"EXEC [Order_Processing_Update_Tras_Id] @Order_Ids,@Trans_Id", _order_Ids,_trans_Id));
                }
                else if (type == "L") 
                {
                    string Order_Ids = string.Join(",", Order_Ids_List.Distinct());

                    var _order_Ids = new SqlParameter("@Order_Ids", !string.IsNullOrEmpty(Order_Ids) ? Order_Ids : DBNull.Value);
                    var _trans_Id = new SqlParameter("@Trans_Id", !string.IsNullOrEmpty(lab_trans_status) ? Convert.ToInt32(lab_trans_status) : DBNull.Value);


                    var result = await Task.Run(() => _dbContext.Database
                           .ExecuteSqlRawAsync(@"EXEC [Lab_Entry_Update_Fortune_Trans_Id] @Order_Ids,@Trans_Id", _order_Ids, _trans_Id));
                }
            }

            return 1;
        }

        public async Task<int> Lab_Entry_Notification()
        {
            int r = 0;

            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vtrans_ID", OracleDbType.Int32);
            param1.Value = DBNull.Value;
            paramList.Add(param1);

            param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dt = await _dbOracleAccess.CallSP_Timeout("web_trans.lab_entry_notification", paramList);

            if (dt != null)
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Trans_Id", typeof(string));
                dataTable.Columns.Add("Stock_Id", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));

                foreach (DataRow item in dt.Rows)
                {
                    dataTable.Rows.Add(item["TRANS_ID"], item["REF_NO"], item["STATUS"]);
                }

                var parameter = new SqlParameter("@Order_Procesing_Table_Type", SqlDbType.Structured)
                {
                    TypeName = "[dbo].[Order_Procesing_Status_Update_Oracle_Table_Type]",
                    Value = dataTable
                };

                var result = await Task.Run(() => _dbContext.Database
                       .ExecuteSqlRawAsync(@"EXEC [Order_Procesing_Status_Update_Oracle] @Order_Procesing_Table_Type", parameter));

                r = result;
            }

            return r;
        }
                
        public async Task<int> Order_Data_Detail_Transfer_Oracle(IList<Order_Processing_Complete_Fortune_Detail> order_Processing_Complete_Fortune_Details)
        {
            if (order_Processing_Complete_Fortune_Details == null || order_Processing_Complete_Fortune_Details.Count == 0)
                return 0;

            var groupedBySupplier = order_Processing_Complete_Fortune_Details
                .GroupBy(item => item.Supplier_code)
                .ToList();

            List<int> allOrderIds = new List<int>();

            foreach (var group in groupedBySupplier)
            {
                string lab_trans_status = string.Empty;

                List<OracleParameter> paramList = new List<OracleParameter>();

                var supplier = group.First();
                OracleParameter param1 = new OracleParameter("supp_code", OracleDbType.Int32);
                param1.Value = supplier.Supplier_code;
                paramList.Add(param1);

                param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
                param1.Direction = ParameterDirection.Output;
                paramList.Add(param1);

                DataTable mas_dt = await _dbOracleAccess.CallSP("web_trans.sun_pur_mas", paramList);

                if (mas_dt != null && mas_dt.Rows.Count > 0)
                {
                    lab_trans_status = Convert.ToString(mas_dt.Rows[0][":B1"]);
                }

                if (!string.IsNullOrEmpty(lab_trans_status))
                {
                    List<int> Order_Ids_List = new List<int>();

                    foreach (var item in group)
                    {
                        Order_Ids_List.Add(item.Id);

                        paramList = new List<OracleParameter>();

                        param1 = new OracleParameter("trans_id", OracleDbType.Int32);
                        param1.Value = lab_trans_status;
                        paramList.Add(param1);

                        param1 = new OracleParameter("supplier_stock_id", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.SuppStockId) ? Convert.ToString(item.SuppStockId) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("Certi_No", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.CertificateNo) ? Convert.ToString(item.CertificateNo) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("STATUS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Status) ? Convert.ToString(item.Status.ToUpper()) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("Buyer_code", OracleDbType.Int32);
                        param1.Value = item.BuyerCode;
                        paramList.Add(param1);

                        Decimal base_amt = Convert.ToDecimal(item.BaseAmount);

                        param1 = new OracleParameter("base_amt", OracleDbType.Decimal);
                        param1.Value = !string.IsNullOrEmpty(item.BaseAmount) ? base_amt : DBNull.Value;
                        paramList.Add(param1);

                        Decimal cost_amt = Convert.ToDecimal(item.CostAmount);

                        param1 = new OracleParameter("cost_amt", OracleDbType.Decimal);
                        param1.Value = !string.IsNullOrEmpty(item.CostAmount) ? cost_amt : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSHADE", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Shade) ? Convert.ToString(item.Shade) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vluster", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Milky) ? Convert.ToString(item.Milky) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vTABLE_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.TableOpen) ? Convert.ToString(item.TableOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.GirdleOpen) ? Convert.ToString(item.GirdleOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCROWN_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.CrownOpen) ? Convert.ToString(item.CrownOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vPAV_OPEN", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.PavilionOpen) ? Convert.ToString(item.PavilionOpen) : DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
                        param1.Direction = ParameterDirection.Output;
                        paramList.Add(param1);

                        DataTable det_dt = await _dbOracleAccess.CallSP("web_trans.sun_pur_det", paramList);
                    }

                    allOrderIds.AddRange(Order_Ids_List);

                    string Order_Ids = string.Join(",", Order_Ids_List);

                    var _order_Ids = new SqlParameter("@Order_Ids", !string.IsNullOrEmpty(Order_Ids) ? Order_Ids : DBNull.Value);
                    var _trans_Id = new SqlParameter("@Trans_Id", !string.IsNullOrEmpty(lab_trans_status) ? Convert.ToInt32(lab_trans_status) : DBNull.Value);

                    await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [Order_Processing_Update_Tras_Id] @Order_Ids,@Trans_Id", _order_Ids, _trans_Id));
                }
            }

            return 1;
        }
      
        public async Task<int> Sun_Pur_Notification()
        {
            int r = 0;

            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dt = await _dbOracleAccess.CallSP_Timeout("web_trans.sun_pur_notification", paramList);

            if (dt != null)
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Trans_Id", typeof(string));
                dataTable.Columns.Add("Stock_Id", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));

                foreach (DataRow item in dt.Rows)
                {
                    dataTable.Rows.Add(item["TRANS_ID"], item["SUPP_REF_NO"], item["STATUS"]);
                }

                var parameter = new SqlParameter("@Order_Procesing_Table_Type", SqlDbType.Structured)
                {
                    TypeName = "[dbo].[Order_Procesing_Status_Update_Oracle_Table_Type]",
                    Value = dataTable
                };

                var result = await Task.Run(() => _dbContext.Database
                       .ExecuteSqlRawAsync(@"EXEC [Order_Procesing_Status_Update_Sun_Pur_Oracle] @Order_Procesing_Table_Type", parameter));

                r = result;
            }

            return r;
        }
        
        public async Task<int> Get_Fortune_Overseas_Data()
        {
            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dataTable = await _dbOracleAccess.CallSP_Timeout("web_trans.get_live_data_overseas", paramList);

            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("Stock_Id", typeof(string));
            newDataTable.Columns.Add("Shape", typeof(string));
            newDataTable.Columns.Add("Cert_No", typeof(string));
            newDataTable.Columns.Add("Pointer_Name", typeof(string));
            newDataTable.Columns.Add("Color", typeof(string));
            newDataTable.Columns.Add("Clarity", typeof(string));
            newDataTable.Columns.Add("Cts", typeof(float));
            newDataTable.Columns.Add("Rap_Rate", typeof(float));
            newDataTable.Columns.Add("Rap_Amt", typeof(float));
            newDataTable.Columns.Add("Base_Disc", typeof(float));
            newDataTable.Columns.Add("Base_Amt", typeof(float));
            newDataTable.Columns.Add("Cost_Disc", typeof(float));
            newDataTable.Columns.Add("Cost_Amt", typeof(float));
            newDataTable.Columns.Add("Offer_Disc", typeof(float));
            newDataTable.Columns.Add("Offer_Amt", typeof(float));
            newDataTable.Columns.Add("Cut", typeof(string));
            newDataTable.Columns.Add("Polish", typeof(string));
            newDataTable.Columns.Add("Symm", typeof(string));
            newDataTable.Columns.Add("Flour_Intensity", typeof(string));
            newDataTable.Columns.Add("Length", typeof(float));
            newDataTable.Columns.Add("Width", typeof(float));
            newDataTable.Columns.Add("Depth", typeof(float));
            newDataTable.Columns.Add("Depth_Per", typeof(float));
            newDataTable.Columns.Add("Table_Per", typeof(float));
            newDataTable.Columns.Add("Lab", typeof(string));
            newDataTable.Columns.Add("Crown_Angle", typeof(float));
            newDataTable.Columns.Add("Crown_Height", typeof(float));
            newDataTable.Columns.Add("Pavillion_Angle", typeof(float));
            newDataTable.Columns.Add("Pavillion_Height", typeof(float));
            newDataTable.Columns.Add("Table_Black", typeof(string));
            newDataTable.Columns.Add("Crown_Black", typeof(string));
            newDataTable.Columns.Add("Table_White", typeof(string));
            newDataTable.Columns.Add("Crown_White", typeof(string));
            newDataTable.Columns.Add("Culet", typeof(string));
            newDataTable.Columns.Add("Key_to_Symbol", typeof(string));
            newDataTable.Columns.Add("Additional_Comment", typeof(string));
            newDataTable.Columns.Add("Laser_Insc", typeof(string));
            newDataTable.Columns.Add("Girdle_Per", typeof(float));
            newDataTable.Columns.Add("Girdle_Type", typeof(string));
            newDataTable.Columns.Add("Image", typeof(string));
            newDataTable.Columns.Add("Video", typeof(string));
            newDataTable.Columns.Add("Supplier_Ref_No", typeof(string));
            newDataTable.Columns.Add("Supplier_Name", typeof(string));
            newDataTable.Columns.Add("Location", typeof(string));
            newDataTable.Columns.Add("Cert_Link", typeof(string));
            newDataTable.Columns.Add("BGM", typeof(string));
            newDataTable.Columns.Add("Table_Open", typeof(string));
            newDataTable.Columns.Add("Crown_Open", typeof(string));
            newDataTable.Columns.Add("Pav_Open", typeof(string));
            newDataTable.Columns.Add("Girdle_Open", typeof(string));
            newDataTable.Columns.Add("Cert_Type", typeof(string));
            newDataTable.Columns.Add("Party_Code", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

                newRow["Stock_Id"] = (!string.IsNullOrEmpty(row["Stock_Id"].ToString())) ? row["Stock_Id"] : DBNull.Value;
                newRow["Shape"] = (!string.IsNullOrEmpty(row["Shape"].ToString())) ? row["Shape"] : DBNull.Value;
                newRow["Cert_No"] = (!string.IsNullOrEmpty(row["Cert_No"].ToString())) ? row["Cert_No"] : DBNull.Value;
                newRow["Pointer_Name"] = (!string.IsNullOrEmpty(row["Pointer_Name"].ToString())) ? row["Pointer_Name"] : DBNull.Value;
                newRow["Color"] = (!string.IsNullOrEmpty(row["Color"].ToString())) ? row["Color"] : DBNull.Value;
                newRow["Clarity"] = (!string.IsNullOrEmpty(row["Clarity"].ToString())) ? row["Clarity"] : DBNull.Value;
                newRow["Cts"] = row["Cts"] != DBNull.Value ? row["Cts"] : DBNull.Value;
                newRow["Rap_Rate"] = row["Rap_Rate"] != DBNull.Value ? row["Rap_Rate"] : DBNull.Value;
                newRow["Rap_Amt"] = row["Rap_Amt"] != DBNull.Value ? row["Rap_Amt"] : DBNull.Value;
                newRow["Base_Disc"] = row["Base_Disc"] != DBNull.Value ? row["Base_Disc"] : DBNull.Value;
                newRow["Base_Amt"] = row["Base_Amt"] != DBNull.Value ? row["Base_Amt"] : DBNull.Value;
                newRow["Cost_Disc"] = row["Cost_Disc"] != DBNull.Value ? row["Cost_Disc"] : DBNull.Value;
                newRow["Cost_Amt"] = row["Cost_Amt"] != DBNull.Value ? row["Cost_Amt"] : DBNull.Value;
                newRow["Offer_Disc"] = row["Offer_Disc"] != DBNull.Value ? row["Offer_Disc"] : DBNull.Value;
                newRow["Offer_Amt"] = row["Offer_Amt"] != DBNull.Value ? row["Offer_Amt"] : DBNull.Value;
                newRow["Cut"] = (!string.IsNullOrEmpty(row["Cut"].ToString())) ? row["Cut"] : DBNull.Value;
                newRow["Polish"] = (!string.IsNullOrEmpty(row["Polish"].ToString())) ? row["Polish"] : DBNull.Value;
                newRow["Symm"] = (!string.IsNullOrEmpty(row["Symm"].ToString())) ? row["Symm"] : DBNull.Value;
                newRow["Flour_Intensity"] = (!string.IsNullOrEmpty(row["Flour_Intensity"].ToString())) ? row["Flour_Intensity"] : DBNull.Value;
                newRow["Length"] = row["Length"] != DBNull.Value ? row["Length"] : DBNull.Value;
                newRow["Width"] = row["Width"] != DBNull.Value ? row["Width"] : DBNull.Value;
                newRow["Depth"] = row["Depth"] != DBNull.Value ? row["Depth"] : DBNull.Value;
                newRow["Depth_Per"] = row["Depth_Per"] != DBNull.Value ? row["Depth_Per"] : DBNull.Value;
                newRow["Table_Per"] = row["Table_Per"] != DBNull.Value ? row["Table_Per"] : DBNull.Value;
                newRow["Lab"] = (!string.IsNullOrEmpty(row["Lab"].ToString())) ? row["Lab"] : DBNull.Value;
                newRow["Crown_Angle"] = row["Crown_Angle"] != DBNull.Value ? row["Crown_Angle"] : DBNull.Value;
                newRow["Crown_Height"] = row["Crown_Height"] != DBNull.Value ? row["Crown_Height"] : DBNull.Value;
                newRow["Pavillion_Angle"] = row["Pavillion_Angle"] != DBNull.Value ? row["Pavillion_Angle"] : DBNull.Value;
                newRow["Pavillion_Height"] = row["Pavillion_Height"] != DBNull.Value ? row["Pavillion_Height"] : DBNull.Value;
                newRow["Table_Black"] = (!string.IsNullOrEmpty(row["Table_Black"].ToString())) ? row["Table_Black"] : DBNull.Value;
                newRow["Crown_Black"] = (!string.IsNullOrEmpty(row["Crown_Black"].ToString())) ? row["Crown_Black"] : DBNull.Value;
                newRow["Table_White"] = (!string.IsNullOrEmpty(row["Table_White"].ToString())) ? row["Table_White"] : DBNull.Value;
                newRow["Crown_White"] = (!string.IsNullOrEmpty(row["Crown_White"].ToString())) ? row["Crown_White"] : DBNull.Value;
                newRow["Culet"] = (!string.IsNullOrEmpty(row["Culet"].ToString())) ? row["Culet"] : DBNull.Value;
                newRow["Key_to_Symbol"] = (!string.IsNullOrEmpty(row["Key_to_Symbol"].ToString())) ? row["Key_to_Symbol"] : DBNull.Value;
                newRow["Additional_Comment"] = (!string.IsNullOrEmpty(row["Additional_Comment"].ToString())) ? row["Additional_Comment"] : DBNull.Value;
                newRow["Laser_Insc"] = (!string.IsNullOrEmpty(row["Laser_Insc"].ToString())) ? row["Laser_Insc"] : DBNull.Value;
                newRow["Girdle_Per"] = row["Girdle_Per"] != DBNull.Value ? row["Girdle_Per"] : DBNull.Value;
                newRow["Girdle_Type"] = (!string.IsNullOrEmpty(row["Girdle_Type"].ToString())) ? row["Girdle_Type"] : DBNull.Value;
                newRow["Image"] = (!string.IsNullOrEmpty(row["Image"].ToString())) ? row["Image"] : DBNull.Value;
                newRow["Video"] = (!string.IsNullOrEmpty(row["Video"].ToString())) ? row["Video"] : DBNull.Value;
                newRow["Supplier_Ref_No"] = (!string.IsNullOrEmpty(row["Supplier_Ref_No"].ToString())) ? row["Supplier_Ref_No"] : DBNull.Value;
                newRow["Supplier_Name"] = (!string.IsNullOrEmpty(row["Supplier_Name"].ToString())) ? row["Supplier_Name"] : DBNull.Value;
                newRow["Location"] = (!string.IsNullOrEmpty(row["Location"].ToString())) ? row["Location"] : DBNull.Value;
                newRow["Cert_Link"] = (!string.IsNullOrEmpty(row["Cert_Link"].ToString())) ? row["Cert_Link"] : DBNull.Value;
                newRow["BGM"] = (!string.IsNullOrEmpty(row["BGM"].ToString())) ? row["BGM"] : DBNull.Value;
                newRow["Table_Open"] = (!string.IsNullOrEmpty(row["Table_Open"].ToString())) ? row["Table_Open"] : DBNull.Value;
                newRow["Crown_Open"] = (!string.IsNullOrEmpty(row["Crown_Open"].ToString())) ? row["Crown_Open"] : DBNull.Value;
                newRow["Pav_Open"] = (!string.IsNullOrEmpty(row["Pav_Open"].ToString())) ? row["Pav_Open"] : DBNull.Value;
                newRow["Girdle_Open"] = (!string.IsNullOrEmpty(row["Girdle_Open"].ToString())) ? row["Girdle_Open"] : DBNull.Value;
                newRow["Cert_Type"] = (!string.IsNullOrEmpty(row["Cert_Type"].ToString())) ? row["Cert_Type"] : DBNull.Value;
                newRow["Party_Code"] = row["Party_Code"] != DBNull.Value ? row["Party_Code"] : DBNull.Value;

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@Fortune_Overseas_Stock_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Fortune_Overseas_Stock_Type",
                Value = newDataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC [Fortune_Overseas_Stock_Ora_Insert_Update] @Fortune_Overseas_Stock_Type", parameter));
            return result;
        }
        
        public async Task<int> Get_Fortune_Sunrise_Data()
        {
            var sqlCommand = @"exec [Fortune_Sunrise_Stock_Ora_Insert_Update]";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;

                    command.CommandTimeout = 3600;

                    await _dbContext.Database.OpenConnectionAsync();

                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows;
                }
            });

            return result;
        }

        public async Task<int> Get_Lab_Entry_Live_Data_Fortune()
        {
            int r = 0;

            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dt = await _dbOracleAccess.CallSP_Timeout("web_trans.get_live_data_fortune", paramList);

            if (dt != null)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("REF_NO", typeof(string));
                dataTable.Columns.Add("RF_ID", typeof(string));
                dataTable.Columns.Add("SUNRISE_STATUS", typeof(string));
                dataTable.Columns.Add("FORTUNE_SOURCE_PARTY", typeof(string));
                dataTable.Columns.Add("UPCOMING_FLAG", typeof(string));
                dataTable.Columns.Add("CLOSE_DATE", typeof(DateTime));
                dataTable.Columns.Add("DELIVERY_STATUS", typeof(string));

                foreach (DataRow item in dt.Rows)
                {
                    dataTable.Rows.Add(item["REF_NO"], item["RF_ID"], item["SUNRISE_STATUS"], item["SOURCE_PARTY"], item["UPCOMING_FLAG"], item["CLOSE_DATE"], item["DELIVERY_STATUS"]);
                }

                var parameter = new SqlParameter("@Lab_Entry_Live_Data_Fortune_Table_Type", SqlDbType.Structured)
                {
                    TypeName = "[dbo].[Lab_Entry_Live_Data_Fortune_Table_Type]",
                    Value = dataTable
                };

                var result = await Task.Run(() => _dbContext.Database
                       .ExecuteSqlRawAsync(@"EXEC [Lab_Entry_Live_Data_Fortune_Update_Oracle] @Lab_Entry_Live_Data_Fortune_Table_Type", parameter));

                r = result;
            }

            return r;
        }

        public async Task<DataTable> Get_Media_Inward()
        {
            int r = 0;

            List<OracleParameter> paramList = new List<OracleParameter>();

            OracleParameter param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
            param1.Direction = ParameterDirection.Output;
            paramList.Add(param1);

            DataTable dt = await _dbOracleAccess.CallSP_Timeout("web_trans.get_media_inward", paramList);

            return dt;
        }


        public async Task<int> Get_Media_Upload(Purchase_Media_Upload_Model purchase_Media_Upload_Model)
        {
            if (purchase_Media_Upload_Model != null)
            {
                List<OracleParameter> paramList = new List<OracleParameter>();

                OracleParameter param1 = new OracleParameter("vREF_NO", OracleDbType.NVarchar2);
                param1.Value = !string.IsNullOrEmpty(purchase_Media_Upload_Model.Sunrise_Stock_Id) ? Convert.ToString(purchase_Media_Upload_Model.Sunrise_Stock_Id) : DBNull.Value;
                paramList.Add(param1);

                param1 = new OracleParameter("vSUPP_IMG_LINK", OracleDbType.NVarchar2);
                param1.Value = !string.IsNullOrEmpty(purchase_Media_Upload_Model.NewImageUrl) ? Convert.ToString(purchase_Media_Upload_Model.NewImageUrl) : DBNull.Value;
                paramList.Add(param1);
                
                param1 = new OracleParameter("vSUPP_VDO_LINK", OracleDbType.NVarchar2);
                param1.Value = !string.IsNullOrEmpty(purchase_Media_Upload_Model.NewVideoUrl) ? Convert.ToString(purchase_Media_Upload_Model.NewVideoUrl) : DBNull.Value;
                paramList.Add(param1);

                param1 = new OracleParameter("vrec", OracleDbType.RefCursor);
                param1.Direction = ParameterDirection.Output;
                paramList.Add(param1);

                DataTable dt = await _dbOracleAccess.CallSP("web_trans.get_media_upload", paramList);
            }

            return 1;
        }


        #endregion
    }
}
