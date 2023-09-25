using astute.CoreServices;
using astute.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        #region Fields
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public ExcelExportController(ICommonService commonService,
            IConfiguration configuration)
        {
            _commonService = commonService;
            _configuration = configuration;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("exportstockexcel")]
        public async Task<IActionResult> ExportStockExcel()
        {
            DataTable dtSumm = new DataTable();

            dtSumm.Columns.Add("TOT_PAGE", typeof(Int32));
            dtSumm.Columns.Add("PAGE_SIZE", typeof(Int32));
            dtSumm.Columns.Add("TOT_PCS", typeof(Int32));
            dtSumm.Columns.Add("TOT_CTS", typeof(Decimal));
            dtSumm.Columns.Add("TOT_RAP_AMOUNT", typeof(Decimal));
            dtSumm.Columns.Add("TOT_NET_AMOUNT", typeof(Decimal));
            dtSumm.Columns.Add("AVG_PRICE_PER_CTS", typeof(Decimal));
            dtSumm.Columns.Add("AVG_SALES_DISC_PER", typeof(Decimal));

            DataTable dt = await _commonService.Get_Stock();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] dra = dt.Select("P_SEQ_NO IS NULL");
                if (dra.Length > 0)
                {
                    DataRow dr = dtSumm.NewRow();
                    dr["TOT_PAGE"] = dra[0]["TOTAL_PAGE"];
                    dr["PAGE_SIZE"] = dra[0]["PAGE_SIZE"];
                    dr["TOT_PCS"] = dra[0]["stone_ref_no"];
                    dr["TOT_CTS"] = dra[0]["CTS"];
                    dr["TOT_RAP_AMOUNT"] = Convert.ToDouble((dra[0]["RAP_AMOUNT"].ToString() != "" && dra[0]["RAP_AMOUNT"].ToString() != null ? dra[0]["RAP_AMOUNT"] : "0"));
                    dr["TOT_NET_AMOUNT"] = dra[0]["NET_AMOUNT"];
                    dr["AVG_PRICE_PER_CTS"] = dra[0]["PRICE_PER_CTS"];
                    dr["AVG_SALES_DISC_PER"] = Convert.ToDouble((dra[0]["SALES_DISC_PER"].ToString() != "" && dra[0]["SALES_DISC_PER"].ToString() != null ? dra[0]["SALES_DISC_PER"] : "0"));
                    dtSumm.Rows.Add(dr);
                }

                dt.DefaultView.RowFilter = "P_SEQ_NO IS NOT NULL";
                dt = dt.DefaultView.ToTable();
                dtSumm.TableName = "SummaryTable";

                string filename = "Sunrise_Diamonds_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CategoryExcel/");
                EpExcelExport.CreateExcel(dt.DefaultView.ToTable(), filePath, filePath + filename, colorType: "Fancy");
                var excelPath = _configuration["LocalPath"] + "Files/CategoryExcel/" + filename;
                return Ok(excelPath);
            }
            else
            {
                return Ok("No Stock found as per filter criteria !");
            }
        }
        #endregion
    }
}
