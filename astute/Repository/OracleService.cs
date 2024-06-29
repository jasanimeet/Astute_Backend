using astute.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static astute.Models.Employee_Master;

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

            DataTable dataTable = await _dbOracleAccess.CallSP("party_info_transfer", paramList);

            DataTable newDataTable = new DataTable();

            newDataTable.Columns.Add("CUSTOMER", typeof(string));
            newDataTable.Columns.Add("SUPPLIER", typeof(string));
            newDataTable.Columns.Add("LAB", typeof(string));
            newDataTable.Columns.Add("PARTY_CODE", typeof(string));
            newDataTable.Columns.Add("PARTY_NAME", typeof(string));
            newDataTable.Columns.Add("ADDRESS_1", typeof(string));
            newDataTable.Columns.Add("ADDRESS_2", typeof(string));
            newDataTable.Columns.Add("ADDRESS_3", typeof(string));
            newDataTable.Columns.Add("CITY", typeof(string));
            newDataTable.Columns.Add("PIN_CODE", typeof(string));
            newDataTable.Columns.Add("MOBILE_NO", typeof(string));
            newDataTable.Columns.Add("MOBILE_NO_1", typeof(string));
            newDataTable.Columns.Add("PHONE_NO", typeof(string));
            newDataTable.Columns.Add("PHONE_NO_1", typeof(string));
            newDataTable.Columns.Add("FAX_NO", typeof(string));
            newDataTable.Columns.Add("EMAIL", typeof(string));
            newDataTable.Columns.Add("EMAIL_1", typeof(string));
            newDataTable.Columns.Add("WEBSITE", typeof(string));
            newDataTable.Columns.Add("INVOICE_GROUP", typeof(string));
            newDataTable.Columns.Add("LEAVE_DATE", typeof(DateTime));
            newDataTable.Columns.Add("ASS1_CODE", typeof(int));
            newDataTable.Columns.Add("ASS1_PER", typeof(float));
            newDataTable.Columns.Add("ASS2_CODE", typeof(int));
            newDataTable.Columns.Add("ASS2_PER", typeof(float));
            newDataTable.Columns.Add("ASS3_CODE", typeof(int));
            newDataTable.Columns.Add("TRANS_DATE", typeof(DateTime));

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow newRow = newDataTable.NewRow();

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

                newDataTable.Rows.Add(newRow);
            }

            var parameter = new SqlParameter("@tableInq", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Fortune_Party_Master_Table_Type]",
                Value = newDataTable
            };

            _dbContext.Database.SetCommandTimeout(1800);
            var result = await _dbContext.Database.ExecuteSqlRawAsync(
                @"EXEC [Fortune_Party_Master_Ora_Insert_Update] @tableInq", parameter);

            return result;
        }

        public async Task<int> Order_Data_Transfer_Oracle(IList<Order_Processing_Complete_Detail> order_Processing_Complete_Details, Employee_Fortune_Master employee_Fortune_Master)
        {

            int LabId = 0, UserId = 0, Assist_UserId = 0, vuser_code = 0, vparty_code = 0;
            string vEntry_type = "W", vparty_name = "", lab_trans_status = "";

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

                if (!string.IsNullOrEmpty(lab_trans_status) && order_Processing_Complete_Details != null && order_Processing_Complete_Details.Count > 0)
                {
                    foreach (var item in order_Processing_Complete_Details)
                    {
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
                        param1.Value = (!string.IsNullOrEmpty(item.CostDisc) ? Convert.ToDouble(item.CostDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSOURCE_PARTY", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.Company) ? Convert.ToString(item.Company) : DBNull.Value;
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
                        param1.Value = (!string.IsNullOrEmpty(item.CostAmount) ? Convert.ToDouble(item.CostAmount) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vSUPP_FINAL_DISC", OracleDbType.Double);
                        param1.Value = (!string.IsNullOrEmpty(item.CostDisc) ? Convert.ToDouble(item.CostDisc) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_PER", OracleDbType.Double);
                        param1.Value = (item.GirdlePer > 0 ? Convert.ToDouble(item.GirdlePer) : 0);
                        paramList.Add(param1);

                        param1 = new OracleParameter("vGIRDLE_TYPE", OracleDbType.NVarchar2);
                        param1.Value = DBNull.Value;
                        paramList.Add(param1);

                        param1 = new OracleParameter("vCOMMENTS", OracleDbType.NVarchar2);
                        param1.Value = !string.IsNullOrEmpty(item.SupplierComments) ? Convert.ToString(item.SupplierComments) : DBNull.Value;
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
                        param1.Value = !string.IsNullOrEmpty(item.Luster) ? Convert.ToString(item.Luster) : DBNull.Value;
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
                        param1.Value = !string.IsNullOrEmpty(item.Remarks) ? Convert.ToString(item.Remarks) : DBNull.Value;
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

            }

            return 1;
        }

        #endregion

    }
}
