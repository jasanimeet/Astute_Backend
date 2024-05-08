using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Operators;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class PartyController : ControllerBase
    {
        #region Fields
        private readonly IPartyService _partyService;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISupplierService _supplierService;
        IExcelDataReader _excelDataReader;
        private readonly ICartService _cartService;
        private readonly IEmailSender _emailSender;
        private readonly IJWTAuthentication _jWTAuthentication;
        private readonly IEmployeeService _employeeService;
        private readonly ILabUserService _labUserService;
        private readonly IAccount_Group_Service _account_Group_Service;
        private readonly IAccount_Master_Service _account_Master_Service;
        #endregion

        #region Ctor
        public PartyController(IPartyService partyService,
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            ISupplierService supplierService,
            ICartService cartService,
            IEmailSender emailSender,
            IJWTAuthentication jWTAuthentication,
            IEmployeeService employeeService,
            ILabUserService labUserService,
            IAccount_Group_Service account_Group_Service,
            IAccount_Master_Service account_Master_Service)
        {
            _partyService = partyService;
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _supplierService = supplierService;
            _cartService = cartService;
            _emailSender = emailSender;
            _jWTAuthentication = jWTAuthentication;
            _employeeService = employeeService;
            _labUserService = labUserService;
            _account_Group_Service = account_Group_Service;
            _account_Master_Service = account_Master_Service;
        }
        #endregion

        #region Utilities
        private async Task<DataTable> Set_Column_In_Datatable(int? supplier_Id, DataTable dt_stock_data, IList<Stock_Data> stock_Datas)
        {
            var party_master = await _partyService.Get_Party_Details(supplier_Id ?? 0);
            dt_stock_data.Columns.Add("SUPPLIER_NO", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_NO", typeof(string));
            dt_stock_data.Columns.Add("LAB", typeof(string));
            dt_stock_data.Columns.Add("SHAPE", typeof(string));
            dt_stock_data.Columns.Add("CTS", typeof(string));
            dt_stock_data.Columns.Add("BASE_DISC", typeof(string));
            dt_stock_data.Columns.Add("BASE_RATE", typeof(string));
            dt_stock_data.Columns.Add("BASE_AMOUNT", typeof(string));
            dt_stock_data.Columns.Add("COLOR", typeof(string));
            dt_stock_data.Columns.Add("CLARITY", typeof(string));
            dt_stock_data.Columns.Add("CUT", typeof(string));
            dt_stock_data.Columns.Add("POLISH", typeof(string));
            dt_stock_data.Columns.Add("SYMM", typeof(string));
            dt_stock_data.Columns.Add("FLS_COLOR", typeof(string));
            dt_stock_data.Columns.Add("FLS_INTENSITY", typeof(string));
            dt_stock_data.Columns.Add("LENGTH", typeof(string));
            dt_stock_data.Columns.Add("WIDTH", typeof(string));
            dt_stock_data.Columns.Add("DEPTH", typeof(string));
            dt_stock_data.Columns.Add("MEASUREMENT", typeof(string));
            dt_stock_data.Columns.Add("DEPTH_PER", typeof(string));
            dt_stock_data.Columns.Add("TABLE_PER", typeof(string));
            dt_stock_data.Columns.Add("CULET", typeof(string));
            dt_stock_data.Columns.Add("SHADE", typeof(string));
            dt_stock_data.Columns.Add("LUSTER", typeof(string));
            dt_stock_data.Columns.Add("MILKY", typeof(string));
            dt_stock_data.Columns.Add("BGM", typeof(string));
            dt_stock_data.Columns.Add("LOCATION", typeof(string));
            dt_stock_data.Columns.Add("STATUS", typeof(string));
            dt_stock_data.Columns.Add("TABLE_BLACK", typeof(string));
            dt_stock_data.Columns.Add("SIDE_BLACK", typeof(string));
            dt_stock_data.Columns.Add("TABLE_WHITE", typeof(string));
            dt_stock_data.Columns.Add("SIDE_WHITE", typeof(string));
            dt_stock_data.Columns.Add("TABLE_OPEN", typeof(string));
            dt_stock_data.Columns.Add("CROWN_OPEN", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_OPEN", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_OPEN", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_FROM", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_TO", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_CONDITION", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_TYPE", typeof(string));
            dt_stock_data.Columns.Add("LASER_INSCRIPTION", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_DATE", typeof(string));
            dt_stock_data.Columns.Add("CROWN_ANGLE", typeof(string));
            dt_stock_data.Columns.Add("CROWN_HEIGHT", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_ANGLE", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_HEIGHT", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_PER", typeof(string));
            dt_stock_data.Columns.Add("LR_HALF", typeof(string));
            dt_stock_data.Columns.Add("STAR_LN", typeof(string));
            dt_stock_data.Columns.Add("CERT_TYPE", typeof(string));
            dt_stock_data.Columns.Add("FANCY_COLOR", typeof(string));
            dt_stock_data.Columns.Add("FANCY_INTENSITY", typeof(string));
            dt_stock_data.Columns.Add("FANCY_OVERTONE", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_LINK", typeof(string));
            dt_stock_data.Columns.Add("Image2", typeof(string));
            dt_stock_data.Columns.Add("VIDEO_LINK", typeof(string));
            dt_stock_data.Columns.Add("Video2", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_LINK", typeof(string));
            dt_stock_data.Columns.Add("DNA", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_HEART_LINK", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_ARROW_LINK", typeof(string));
            dt_stock_data.Columns.Add("H_A_LINK", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_TYPE_LINK", typeof(string));
            dt_stock_data.Columns.Add("KEY_TO_SYMBOL", typeof(string));
            dt_stock_data.Columns.Add("LAB_COMMENTS", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_COMMENTS", typeof(string));
            dt_stock_data.Columns.Add("ORIGIN", typeof(string));
            dt_stock_data.Columns.Add("BOW_TIE", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_PAVILION", typeof(string));
            dt_stock_data.Columns.Add("INTERNAL_GRAINING", typeof(string));
            dt_stock_data.Columns.Add("H_A", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_DISC", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_AMOUNT", typeof(string));
            dt_stock_data.Columns.Add("OFFER_DISC", typeof(string));
            dt_stock_data.Columns.Add("OFFER_VALUE", typeof(string));
            dt_stock_data.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
            dt_stock_data.Columns.Add("MAX_SLAB_BASE_VALUE", typeof(string));
            dt_stock_data.Columns.Add("EYE_CLEAN", typeof(string));
            dt_stock_data.Columns.Add("GOOD_TYPE", typeof(string));
            dt_stock_data.Columns.Add("Short_Code", typeof(string));
            if (stock_Datas != null && stock_Datas.Count > 0)
            {
                var lakhi_Party_Code = _configuration["Lakhi_Party_Code"];
                foreach (var item in stock_Datas)
                {
                    var table_white = string.Empty;
                    var table_Black = string.Empty;
                    var side_White = string.Empty;
                    var side_Black = string.Empty;
                    if (party_master != null && party_master.Party_Code == lakhi_Party_Code)
                    {
                        (table_white, _) = CoreService.Table_And_Side_White(item.TABLE_WHITE);
                        (_, side_White) = CoreService.Table_And_Side_White(item.SIDE_WHITE);

                        (table_Black, _) = CoreService.Table_And_Side_Black(item.TABLE_BLACK);
                        (_, side_Black) = CoreService.Table_And_Side_Black(item.SIDE_BLACK);
                    }
                    else
                    {
                        table_white = item.TABLE_WHITE;
                        table_Black = item.TABLE_BLACK;
                        side_White = item.SIDE_WHITE;
                        side_Black = item.SIDE_BLACK;
                    }
                    dt_stock_data.Rows.Add(
                        item.SUPPLIER_NO ?? null,
                        item.CERTIFICATE_NO ?? null,
                        item.LAB ?? null,
                        item.SHAPE ?? null,
                        item.CTS ?? null,
                        item.BASE_DISC != item.BASE_DISC && !string.IsNullOrEmpty(item.BASE_DISC) ? item.BASE_DISC.Replace("$", "") : null,
                        item.BASE_RATE != item.BASE_RATE && !string.IsNullOrEmpty(item.BASE_RATE) ? item.BASE_RATE.Replace("$", "") : null,
                        item.BASE_AMOUNT != item.BASE_AMOUNT && !string.IsNullOrEmpty(item.BASE_AMOUNT) ? item.BASE_AMOUNT.Replace("$", "") : null,
                        item.COLOR ?? null,
                        item.CLARITY ?? null,
                        item.CUT ?? null,
                        item.POLISH ?? null,
                        item.SYMM ?? null,
                        item.FLS_COLOR ?? null,
                        item.FLS_INTENSITY ?? null,
                        item.LENGTH != null && !string.IsNullOrEmpty(item.LENGTH.ToString()) ? Convert.ToString(item.LENGTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "length") : DBNull.Value,
                        item.WIDTH != null && !string.IsNullOrEmpty(item.WIDTH.ToString()) ? Convert.ToString(item.WIDTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "width") : DBNull.Value,
                        item.DEPTH != null && !string.IsNullOrEmpty(item.DEPTH.ToString()) ? Convert.ToString(item.DEPTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "depth") : DBNull.Value,
                        item.MEASUREMENT ?? null,
                        item.DEPTH_PER != item.DEPTH_PER && !string.IsNullOrEmpty(item.DEPTH_PER) ? item.DEPTH_PER.Replace("%", "") : null,
                        item.TABLE_PER != item.TABLE_PER && !string.IsNullOrEmpty(item.TABLE_PER) ? item.TABLE_PER.Replace("%", "") : null,
                        item.CULET ?? null,
                        item.SHADE ?? null,
                        item.LUSTER ?? null,
                        item.MILKY ?? null,
                        item.BGM ?? null,
                        item.LOCATION ?? null,
                        item.STATUS ?? null,
                        table_Black,
                        side_Black,
                        table_white,
                        side_White,
                        item.TABLE_OPEN ?? null,
                        item.CROWN_OPEN ?? null,
                        item.PAVILION_OPEN ?? null,
                        item.GIRDLE_OPEN ?? null,
                        !string.IsNullOrEmpty(item.GIRDLE_FROM) ? (item.GIRDLE_FROM.Contains("-") ? (item.GIRDLE_FROM.Split(" - ").Length == 1 ? item.GIRDLE_FROM.Split(" - ")[0] : item.GIRDLE_FROM) : (item.GIRDLE_FROM.ToUpper().Contains(" TO ") ? (item.GIRDLE_FROM.ToUpper().Split(" TO ").Length == 1 ? item.GIRDLE_FROM.ToUpper().Split(" TO ")[0] : item.GIRDLE_FROM) : item.GIRDLE_FROM)) : null,
                        !string.IsNullOrEmpty(item.GIRDLE_TO) ? (item.GIRDLE_TO.Contains("-") ? (item.GIRDLE_TO.Split(" - ").Length == 2 ? item.GIRDLE_TO.Split(" - ")[0] : item.GIRDLE_TO) : (item.GIRDLE_TO.ToUpper().Contains(" TO ") ? (item.GIRDLE_TO.ToUpper().Split(" TO ").Length == 2 ? item.GIRDLE_TO.ToUpper().Split(" TO ")[0] : item.GIRDLE_TO) : item.GIRDLE_TO)) : null,
                        item.GIRDLE_CONDITION ?? null,
                        item.GIRDLE_TYPE ?? null,
                        item.LASER_INSCRIPTION ?? null,
                        item.CERTIFICATE_DATE ?? null,
                        item.CROWN_ANGLE != item.CROWN_ANGLE && !string.IsNullOrEmpty(item.CROWN_ANGLE) ? item.CROWN_ANGLE.Replace("%", "") : null,
                        item.CROWN_HEIGHT != item.CROWN_HEIGHT && !string.IsNullOrEmpty(item.CROWN_HEIGHT) ? item.CROWN_HEIGHT.Replace("%", "") : null,
                        item.PAVILION_ANGLE != item.PAVILION_ANGLE && !string.IsNullOrEmpty(item.PAVILION_ANGLE) ? item.PAVILION_ANGLE.Replace("%", "") : null,
                        item.PAVILION_HEIGHT != item.PAVILION_HEIGHT && !string.IsNullOrEmpty(item.PAVILION_HEIGHT) ? item.PAVILION_HEIGHT.Replace("%", "") : null,
                        item.GIRDLE_PER != item.GIRDLE_PER && !string.IsNullOrEmpty(item.GIRDLE_PER) ? item.GIRDLE_PER.Replace("%", "") : null,
                        item.LR_HALF != item.LR_HALF && !string.IsNullOrEmpty(item.LR_HALF) ? item.LR_HALF.Replace("%", "") : null,
                        item.STAR_LN != item.STAR_LN && !string.IsNullOrEmpty(item.STAR_LN) ? item.STAR_LN.Replace("%", "") : null,
                        item.CERT_TYPE ?? null,
                        item.FANCY_COLOR ?? null,
                        item.FANCY_INTENSITY ?? null,
                        item.FANCY_OVERTONE ?? null,
                        item.IMAGE_LINK ?? null,
                        item.Image2 ?? null,
                        item.VIDEO_LINK ?? null,
                        item.Video2 ?? null,
                        item.CERTIFICATE_LINK ?? null,
                        item.DNA ?? null,
                        item.IMAGE_HEART_LINK ?? null,
                        item.IMAGE_ARROW_LINK ?? null,
                        item.H_A_LINK ?? null,
                        item.CERTIFICATE_TYPE_LINK ?? null,
                        item.KEY_TO_SYMBOL ?? null,
                        item.LAB_COMMENTS ?? null,
                        item.SUPPLIER_COMMENTS ?? null,
                        item.ORIGIN ?? null,
                        item.BOW_TIE ?? null,
                        item.EXTRA_FACET_TABLE ?? null,
                        item.EXTRA_FACET_CROWN ?? null,
                        item.EXTRA_FACET_PAVILION ?? null,
                        item.INTERNAL_GRAINING ?? null,
                        item.H_A ?? null,
                        item.SUPPLIER_DISC ?? null,
                        item.SUPPLIER_AMOUNT ?? null,
                        item.OFFER_DISC ?? null,
                        item.OFFER_VALUE ?? null,
                        item.MAX_SLAB_BASE_DISC ?? null,
                        item.MAX_SLAB_BASE_VALUE ?? null,
                        item.EYE_CLEAN ?? null,
                        item.GOOD_TYPE ?? null,
                        item.Short_Code ?? null);
                }
            }
            return dt_stock_data;
        }
        private async Task<DataTable> Set_Column_In_Datatable_Scheduler(DataTable dt_stock_data, IList<Stock_Data_Schedular> stock_Datas, int supplier_Id)
        {
            var party_master = await _partyService.Get_Party_Details(supplier_Id);
            dt_stock_data.Columns.Add("SUPPLIER_NO", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_NO", typeof(string));
            dt_stock_data.Columns.Add("LAB", typeof(string));
            dt_stock_data.Columns.Add("SHAPE", typeof(string));
            dt_stock_data.Columns.Add("CTS", typeof(string));
            dt_stock_data.Columns.Add("BASE_DISC", typeof(string));
            dt_stock_data.Columns.Add("BASE_RATE", typeof(string));
            dt_stock_data.Columns.Add("BASE_AMOUNT", typeof(string));
            dt_stock_data.Columns.Add("COLOR", typeof(string));
            dt_stock_data.Columns.Add("CLARITY", typeof(string));
            dt_stock_data.Columns.Add("CUT", typeof(string));
            dt_stock_data.Columns.Add("POLISH", typeof(string));
            dt_stock_data.Columns.Add("SYMM", typeof(string));
            dt_stock_data.Columns.Add("FLS_COLOR", typeof(string));
            dt_stock_data.Columns.Add("FLS_INTENSITY", typeof(string));
            dt_stock_data.Columns.Add("LENGTH", typeof(string));
            dt_stock_data.Columns.Add("WIDTH", typeof(string));
            dt_stock_data.Columns.Add("DEPTH", typeof(string));
            dt_stock_data.Columns.Add("MEASUREMENT", typeof(string));
            dt_stock_data.Columns.Add("DEPTH_PER", typeof(string));
            dt_stock_data.Columns.Add("TABLE_PER", typeof(string));
            dt_stock_data.Columns.Add("CULET", typeof(string));
            dt_stock_data.Columns.Add("SHADE", typeof(string));
            dt_stock_data.Columns.Add("LUSTER", typeof(string));
            dt_stock_data.Columns.Add("MILKY", typeof(string));
            dt_stock_data.Columns.Add("BGM", typeof(string));
            dt_stock_data.Columns.Add("LOCATION", typeof(string));
            dt_stock_data.Columns.Add("STATUS", typeof(string));
            dt_stock_data.Columns.Add("TABLE_BLACK", typeof(string));
            dt_stock_data.Columns.Add("SIDE_BLACK", typeof(string));
            dt_stock_data.Columns.Add("TABLE_WHITE", typeof(string));
            dt_stock_data.Columns.Add("SIDE_WHITE", typeof(string));
            dt_stock_data.Columns.Add("TABLE_OPEN", typeof(string));
            dt_stock_data.Columns.Add("CROWN_OPEN", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_OPEN", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_OPEN", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_FROM", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_TO", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_CONDITION", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_TYPE", typeof(string));
            dt_stock_data.Columns.Add("LASER_INSCRIPTION", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_DATE", typeof(string));
            dt_stock_data.Columns.Add("CROWN_ANGLE", typeof(string));
            dt_stock_data.Columns.Add("CROWN_HEIGHT", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_ANGLE", typeof(string));
            dt_stock_data.Columns.Add("PAVILION_HEIGHT", typeof(string));
            dt_stock_data.Columns.Add("GIRDLE_PER", typeof(string));
            dt_stock_data.Columns.Add("LR_HALF", typeof(string));
            dt_stock_data.Columns.Add("STAR_LN", typeof(string));
            dt_stock_data.Columns.Add("CERT_TYPE", typeof(string));
            dt_stock_data.Columns.Add("FANCY_COLOR", typeof(string));
            dt_stock_data.Columns.Add("FANCY_INTENSITY", typeof(string));
            dt_stock_data.Columns.Add("FANCY_OVERTONE", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_LINK", typeof(string));
            dt_stock_data.Columns.Add("Image2", typeof(string));
            dt_stock_data.Columns.Add("VIDEO_LINK", typeof(string));
            dt_stock_data.Columns.Add("Video2", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_LINK", typeof(string));
            dt_stock_data.Columns.Add("DNA", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_HEART_LINK", typeof(string));
            dt_stock_data.Columns.Add("IMAGE_ARROW_LINK", typeof(string));
            dt_stock_data.Columns.Add("H_A_LINK", typeof(string));
            dt_stock_data.Columns.Add("CERTIFICATE_TYPE_LINK", typeof(string));
            dt_stock_data.Columns.Add("KEY_TO_SYMBOL", typeof(string));
            dt_stock_data.Columns.Add("LAB_COMMENTS", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_COMMENTS", typeof(string));
            dt_stock_data.Columns.Add("ORIGIN", typeof(string));
            dt_stock_data.Columns.Add("BOW_TIE", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
            dt_stock_data.Columns.Add("EXTRA_FACET_PAVILION", typeof(string));
            dt_stock_data.Columns.Add("INTERNAL_GRAINING", typeof(string));
            dt_stock_data.Columns.Add("H_A", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_DISC", typeof(string));
            dt_stock_data.Columns.Add("SUPPLIER_AMOUNT", typeof(string));
            dt_stock_data.Columns.Add("OFFER_DISC", typeof(string));
            dt_stock_data.Columns.Add("OFFER_VALUE", typeof(string));
            dt_stock_data.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
            dt_stock_data.Columns.Add("MAX_SLAB_BASE_VALUE", typeof(string));
            dt_stock_data.Columns.Add("EYE_CLEAN", typeof(string));
            dt_stock_data.Columns.Add("GOOD_TYPE", typeof(string));
            dt_stock_data.Columns.Add("Short_Code", typeof(string));
            if (stock_Datas != null && stock_Datas.Count > 0)
            {
                var lakhi_Party_Code = _configuration["Lakhi_Party_Code"];
                foreach (var item in stock_Datas)
                {
                    var table_white = string.Empty;
                    var table_Black = string.Empty;
                    var side_White = string.Empty;
                    var side_Black = string.Empty;
                    if (party_master != null && party_master.Party_Code == lakhi_Party_Code)
                    {
                        (table_white, _) = CoreService.Table_And_Side_White(Convert.ToString(item.TABLE_WHITE));
                        (_, side_White) = CoreService.Table_And_Side_White(Convert.ToString(item.SIDE_WHITE));

                        (table_Black, _) = CoreService.Table_And_Side_White(Convert.ToString(item.TABLE_BLACK));
                        (_, side_Black) = CoreService.Table_And_Side_White(Convert.ToString(item.SIDE_BLACK));
                    }
                    else
                    {
                        table_white = Convert.ToString(item.TABLE_WHITE);
                        table_Black = Convert.ToString(item.TABLE_BLACK);
                        side_White = Convert.ToString(item.SIDE_WHITE);
                        side_Black = Convert.ToString(item.SIDE_BLACK);
                    }
                    dt_stock_data.Rows.Add(item.SUPPLIER_NO != null ? Convert.ToString(item.SUPPLIER_NO) : DBNull.Value,
                        item.CERTIFICATE_NO != null ? Convert.ToString(item.CERTIFICATE_NO) : DBNull.Value,
                        item.LAB != null ? Convert.ToString(item.LAB) : DBNull.Value,
                        item.SHAPE != null ? Convert.ToString(item.SHAPE) : DBNull.Value,
                        item.CTS != null ? Convert.ToString(item.CTS) : DBNull.Value,
                        item.BASE_DISC != null ? Convert.ToString(item.BASE_DISC).Replace("$", "") : DBNull.Value,
                        item.BASE_RATE != null ? Convert.ToString(item.BASE_RATE).Replace("$", "") : DBNull.Value,
                        item.BASE_AMOUNT != null ? Convert.ToString(item.BASE_AMOUNT).Replace("$", "") : DBNull.Value,
                        item.COLOR != null ? Convert.ToString(item.COLOR) : DBNull.Value,
                        item.CLARITY != null ? Convert.ToString(item.CLARITY) : DBNull.Value,
                        item.CUT != null ? Convert.ToString(item.CUT) : DBNull.Value,
                        item.POLISH != null ? Convert.ToString(item.POLISH) : DBNull.Value,
                        item.SYMM != null ? Convert.ToString(item.SYMM) : DBNull.Value,
                        item.FLS_COLOR != null ? Convert.ToString(item.FLS_COLOR) : DBNull.Value,
                        item.FLS_INTENSITY != null ? Convert.ToString(item.FLS_INTENSITY) : DBNull.Value,
                        item.LENGTH != null && !string.IsNullOrEmpty(item.LENGTH.ToString()) ? Convert.ToString(item.LENGTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "length") : DBNull.Value,
                        item.WIDTH != null && !string.IsNullOrEmpty(item.WIDTH.ToString()) ? Convert.ToString(item.WIDTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "width") : DBNull.Value,
                        item.DEPTH != null && !string.IsNullOrEmpty(item.DEPTH.ToString()) ? Convert.ToString(item.DEPTH) : item.MEASUREMENT != null ? Split_Supplier_Stock_Measurement(Convert.ToString(item.MEASUREMENT), "depth") : DBNull.Value,
                        item.MEASUREMENT != null ? Convert.ToString(item.MEASUREMENT) : DBNull.Value,
                        item.DEPTH_PER != null ? Convert.ToString(item.DEPTH_PER).Replace("%", "") : DBNull.Value,
                        item.TABLE_PER != null ? Convert.ToString(item.TABLE_PER).Replace("%", "") : DBNull.Value,
                        item.CULET != null ? Convert.ToString(item.CULET) : DBNull.Value,
                        item.SHADE != null ? Convert.ToString(item.SHADE) : DBNull.Value,
                        item.LUSTER != null ? Convert.ToString(item.LUSTER) : DBNull.Value,
                        item.MILKY != null ? Convert.ToString(item.MILKY) : DBNull.Value,
                        item.BGM != null ? Convert.ToString(item.BGM) : DBNull.Value,
                        item.LOCATION != null ? Convert.ToString(item.LOCATION) : DBNull.Value,
                        item.STATUS != null ? Convert.ToString(item.STATUS) : DBNull.Value,
                        table_Black,
                        side_Black,
                        table_white,
                        side_White,
                        item.TABLE_OPEN != null ? Convert.ToString(item.TABLE_OPEN) : DBNull.Value,
                        item.CROWN_OPEN != null ? Convert.ToString(item.CROWN_OPEN) : DBNull.Value,
                        item.PAVILION_OPEN != null ? Convert.ToString(item.PAVILION_OPEN) : DBNull.Value,
                        item.GIRDLE_OPEN != null ? Convert.ToString(item.GIRDLE_OPEN) : DBNull.Value,
                        item.GIRDLE_FROM != null ? !string.IsNullOrEmpty(Convert.ToString(item.GIRDLE_FROM)) ? (Convert.ToString(item.GIRDLE_FROM).Contains("-") ? (Convert.ToString(item.GIRDLE_FROM).Split(" - ").Length == 1 ? Convert.ToString(item.GIRDLE_FROM).Split(" - ")[0] : item.GIRDLE_FROM) : (Convert.ToString(item.GIRDLE_FROM).ToUpper().Contains(" TO ") ? (Convert.ToString(item.GIRDLE_FROM).Split(" TO ").Length == 1 ? Convert.ToString(item.GIRDLE_FROM).Split(" TO ")[0] : item.GIRDLE_FROM) : Convert.ToString(item.GIRDLE_FROM))) : null : DBNull.Value,
                        item.GIRDLE_TO != null ? !string.IsNullOrEmpty(Convert.ToString(item.GIRDLE_TO)) ? (Convert.ToString(item.GIRDLE_TO).Contains("-") ? (Convert.ToString(item.GIRDLE_TO).Split(" - ").Length == 2 ? Convert.ToString(item.GIRDLE_TO).Split(" - ")[1] : item.GIRDLE_TO) : (Convert.ToString(item.GIRDLE_TO).ToUpper().Contains(" TO ") ? (Convert.ToString(item.GIRDLE_TO).Split(" TO ").Length == 2 ? Convert.ToString(item.GIRDLE_TO).Split(" TO ")[1] : item.GIRDLE_TO) : Convert.ToString(item.GIRDLE_TO))) : null : DBNull.Value,
                        item.GIRDLE_CONDITION != null ? Convert.ToString(item.GIRDLE_CONDITION) : DBNull.Value,
                        item.GIRDLE_TYPE != null ? Convert.ToString(item.GIRDLE_TYPE) : DBNull.Value,
                        item.LASER_INSCRIPTION != null ? Convert.ToString(item.LASER_INSCRIPTION) : DBNull.Value,
                        item.CERTIFICATE_DATE != null ? Convert.ToString(item.CERTIFICATE_DATE) : DBNull.Value,
                        item.CROWN_ANGLE != null ? Convert.ToString(item.CROWN_ANGLE).Replace("%", "") : DBNull.Value,
                        item.CROWN_HEIGHT != null ? Convert.ToString(item.CROWN_HEIGHT).Replace("%", "") : DBNull.Value,
                        item.PAVILION_ANGLE != null ? Convert.ToString(item.PAVILION_ANGLE).Replace("%", "") : DBNull.Value,
                        item.PAVILION_HEIGHT != null ? Convert.ToString(item.PAVILION_HEIGHT).Replace("%", "") : DBNull.Value,
                        item.GIRDLE_PER != null ? Convert.ToString(item.GIRDLE_PER).Replace("%", "") : DBNull.Value,
                        item.LR_HALF != null ? Convert.ToString(item.LR_HALF).Replace("%", "") : DBNull.Value,
                        item.STAR_LN != null ? Convert.ToString(item.STAR_LN).Replace("%", "") : DBNull.Value,
                        item.CERT_TYPE != null ? Convert.ToString(item.CERT_TYPE) : DBNull.Value,
                        item.FANCY_COLOR != null ? Convert.ToString(item.FANCY_COLOR) : DBNull.Value,
                        item.FANCY_INTENSITY != null ? Convert.ToString(item.FANCY_INTENSITY) : DBNull.Value,
                        item.FANCY_OVERTONE != null ? Convert.ToString(item.FANCY_OVERTONE) : DBNull.Value,
                        item.IMAGE_LINK != null ? Convert.ToString(item.IMAGE_LINK).Length >= 20 ? Convert.ToString(item.IMAGE_LINK).Contains(",") ? Convert.ToString(item.IMAGE_LINK).Split(",")[0] : Convert.ToString(item.IMAGE_LINK) : DBNull.Value : DBNull.Value,
                        item.Image2 != null ? Convert.ToString(item.Image2) : DBNull.Value,
                        item.VIDEO_LINK != null ? Convert.ToString(item.VIDEO_LINK).Length >= 20 ? Convert.ToString(item.VIDEO_LINK) : DBNull.Value : DBNull.Value,
                        item.Video2 != null ? Convert.ToString(item.Video2) : DBNull.Value,
                        item.CERTIFICATE_LINK != null ? Convert.ToString(item.CERTIFICATE_LINK).Length >= 20 ? Convert.ToString(item.CERTIFICATE_LINK) : DBNull.Value : DBNull.Value,
                        item.DNA != null ? Convert.ToString(item.DNA) : DBNull.Value,
                        item.IMAGE_HEART_LINK != null ? Convert.ToString(item.IMAGE_HEART_LINK) : DBNull.Value,
                        item.IMAGE_ARROW_LINK != null ? Convert.ToString(item.IMAGE_ARROW_LINK) : DBNull.Value,
                        item.H_A_LINK != null ? Convert.ToString(item.H_A_LINK) : DBNull.Value,
                        item.CERTIFICATE_TYPE_LINK != null ? Convert.ToString(item.CERTIFICATE_TYPE_LINK) : DBNull.Value,
                        item.KEY_TO_SYMBOL != null ? Convert.ToString(item.KEY_TO_SYMBOL) : DBNull.Value,
                        item.LAB_COMMENTS != null ? Convert.ToString(item.LAB_COMMENTS) : DBNull.Value,
                        item.SUPPLIER_COMMENTS != null ? Convert.ToString(item.SUPPLIER_COMMENTS) : DBNull.Value,
                        item.ORIGIN != null ? Convert.ToString(item.ORIGIN) : DBNull.Value,
                        item.BOW_TIE != null ? Convert.ToString(item.BOW_TIE) : DBNull.Value,
                        item.EXTRA_FACET_TABLE != null ? Convert.ToString(item.EXTRA_FACET_TABLE) : DBNull.Value,
                        item.EXTRA_FACET_CROWN != null ? Convert.ToString(item.EXTRA_FACET_CROWN) : DBNull.Value,
                        item.EXTRA_FACET_PAVILION != null ? Convert.ToString(item.EXTRA_FACET_PAVILION) : DBNull.Value,
                        item.INTERNAL_GRAINING != null ? Convert.ToString(item.INTERNAL_GRAINING) : DBNull.Value,
                        item.H_A != null ? Convert.ToString(item.H_A) : DBNull.Value,
                        item.SUPPLIER_DISC != null ? Convert.ToString(item.SUPPLIER_DISC) : DBNull.Value,
                        item.SUPPLIER_AMOUNT != null ? Convert.ToString(item.SUPPLIER_AMOUNT) : DBNull.Value,
                        item.OFFER_DISC != null ? Convert.ToString(item.OFFER_DISC) : DBNull.Value,
                        item.OFFER_VALUE != null ? Convert.ToString(item.OFFER_VALUE) : DBNull.Value,
                        item.MAX_SLAB_BASE_DISC != null ? Convert.ToString(item.MAX_SLAB_BASE_DISC) : DBNull.Value,
                        item.MAX_SLAB_BASE_VALUE != null ? Convert.ToString(item.MAX_SLAB_BASE_VALUE) : DBNull.Value,
                        item.EYE_CLEAN != null ? Convert.ToString(item.EYE_CLEAN) : DBNull.Value,
                        item.GOOD_TYPE != null ? Convert.ToString(item.EYE_CLEAN) : DBNull.Value,
                        item.Short_Code != null ? Convert.ToString(item.Short_Code) : DBNull.Value);
                }
            }
            return dt_stock_data;
        }
        private DataTable Set_Supp_Stock_Column_In_Datatable(DataTable dt_stock_data, IList<Stock_Data> stock_Datas)
        {
            dt_stock_data.Columns.Add("Supplier_Ref_No", typeof(string));
            dt_stock_data.Columns.Add("Cert_No", typeof(string));
            dt_stock_data.Columns.Add("Lab", typeof(string));
            dt_stock_data.Columns.Add("Shape", typeof(string));
            dt_stock_data.Columns.Add("Cts", typeof(string));
            dt_stock_data.Columns.Add("Base_Disc", typeof(string));
            dt_stock_data.Columns.Add("Base_Rate", typeof(string));
            dt_stock_data.Columns.Add("Base_Amt", typeof(string));
            dt_stock_data.Columns.Add("Color", typeof(string));
            dt_stock_data.Columns.Add("Clarity", typeof(string));
            dt_stock_data.Columns.Add("Cut", typeof(string));
            dt_stock_data.Columns.Add("Polish", typeof(string));
            dt_stock_data.Columns.Add("Symm", typeof(string));
            dt_stock_data.Columns.Add("Flour", typeof(string));
            dt_stock_data.Columns.Add("Flour_Intensity", typeof(string));
            dt_stock_data.Columns.Add("Length", typeof(string));
            dt_stock_data.Columns.Add("Width", typeof(string));
            dt_stock_data.Columns.Add("Depth", typeof(string));
            dt_stock_data.Columns.Add("Measurement", typeof(string));
            dt_stock_data.Columns.Add("Depth_Per", typeof(string));
            dt_stock_data.Columns.Add("Table_Per", typeof(string));
            dt_stock_data.Columns.Add("Culet", typeof(string));
            dt_stock_data.Columns.Add("Shade", typeof(string));
            dt_stock_data.Columns.Add("Luster", typeof(string));
            dt_stock_data.Columns.Add("Milky", typeof(string));
            dt_stock_data.Columns.Add("BGM", typeof(string));
            dt_stock_data.Columns.Add("Location", typeof(string));
            dt_stock_data.Columns.Add("Status", typeof(string));
            dt_stock_data.Columns.Add("Table_Black", typeof(string));
            dt_stock_data.Columns.Add("Crown_Black", typeof(string));
            dt_stock_data.Columns.Add("Table_White", typeof(string));
            dt_stock_data.Columns.Add("Crown_White", typeof(string));
            dt_stock_data.Columns.Add("Table_Open", typeof(string));
            dt_stock_data.Columns.Add("Crown_Open", typeof(string));
            dt_stock_data.Columns.Add("Pav_Open", typeof(string));
            dt_stock_data.Columns.Add("Girdle_Open", typeof(string));
            dt_stock_data.Columns.Add("Girdle_From", typeof(string));
            dt_stock_data.Columns.Add("Girdle_To", typeof(string));
            dt_stock_data.Columns.Add("Girdle_Condition", typeof(string));
            dt_stock_data.Columns.Add("Girdle_Type", typeof(string));
            dt_stock_data.Columns.Add("Laser_Insc", typeof(string));
            dt_stock_data.Columns.Add("Cert_Date", typeof(string));
            dt_stock_data.Columns.Add("Crown_Angle", typeof(string));
            dt_stock_data.Columns.Add("Crown_Height", typeof(string));
            dt_stock_data.Columns.Add("Pavillion_Angle", typeof(string));
            dt_stock_data.Columns.Add("Pavillion_Height", typeof(string));
            dt_stock_data.Columns.Add("Girdle_Per", typeof(string));
            dt_stock_data.Columns.Add("LR_Half", typeof(string));
            dt_stock_data.Columns.Add("Str_Ln", typeof(string));
            dt_stock_data.Columns.Add("Cert_Type", typeof(string));
            dt_stock_data.Columns.Add("Fancy_Color", typeof(string));
            dt_stock_data.Columns.Add("Fancy_Intensity", typeof(string));
            dt_stock_data.Columns.Add("Fancy_Overtone", typeof(string));
            dt_stock_data.Columns.Add("Image", typeof(string));
            dt_stock_data.Columns.Add("Image2", typeof(string));
            dt_stock_data.Columns.Add("Video", typeof(string));
            dt_stock_data.Columns.Add("Video2", typeof(string));
            dt_stock_data.Columns.Add("Cert_Link", typeof(string));
            dt_stock_data.Columns.Add("DNA", typeof(string));
            dt_stock_data.Columns.Add("Heart_Link", typeof(string));
            dt_stock_data.Columns.Add("Arrow_Link", typeof(string));
            dt_stock_data.Columns.Add("H_A_Link", typeof(string));
            dt_stock_data.Columns.Add("Cert_Type_Link", typeof(string));
            dt_stock_data.Columns.Add("Key_to_Symbol", typeof(string));
            dt_stock_data.Columns.Add("Additional_Comment", typeof(string));
            dt_stock_data.Columns.Add("Supplier_Comment", typeof(string));
            dt_stock_data.Columns.Add("Rough_Origin", typeof(string));
            dt_stock_data.Columns.Add("Bow_Tie", typeof(string));
            dt_stock_data.Columns.Add("Extra_Facet_Table", typeof(string));
            dt_stock_data.Columns.Add("Extra_Facet_Crown", typeof(string));
            dt_stock_data.Columns.Add("Extra_Facet_Pav", typeof(string));
            dt_stock_data.Columns.Add("Internal_Graining", typeof(string));
            dt_stock_data.Columns.Add("H_A", typeof(string));
            dt_stock_data.Columns.Add("Cost_Disc", typeof(string));
            dt_stock_data.Columns.Add("Cost_Amt", typeof(string));
            dt_stock_data.Columns.Add("Offer_Disc", typeof(string));
            dt_stock_data.Columns.Add("Offer_Amt", typeof(string));
            dt_stock_data.Columns.Add("Max_Slab_Disc", typeof(string));
            dt_stock_data.Columns.Add("Max_Slab_Amt", typeof(string));
            dt_stock_data.Columns.Add("Eye_Clean", typeof(string));
            dt_stock_data.Columns.Add("Supp_Short_Name", typeof(string));
            foreach (var item in stock_Datas)
            {
                dt_stock_data.Rows.Add(item.SUPPLIER_NO, item.CERTIFICATE_NO, item.LAB, item.SHAPE, item.CTS, item.BASE_DISC, item.BASE_RATE, item.BASE_AMOUNT, item.COLOR,
                    item.CLARITY, item.CUT, item.POLISH, item.SYMM, item.FLS_COLOR, item.FLS_INTENSITY, item.LENGTH, item.WIDTH, item.DEPTH, item.MEASUREMENT, item.DEPTH_PER,
                    item.TABLE_PER, item.CULET, item.SHADE, item.LUSTER, item.MILKY, item.BGM, item.LOCATION, item.STATUS, item.TABLE_BLACK, item.SIDE_BLACK, item.TABLE_BLACK,
                    item.SIDE_WHITE, item.TABLE_OPEN, item.CROWN_OPEN, item.PAVILION_OPEN, item.GIRDLE_OPEN, item.GIRDLE_FROM, item.GIRDLE_TO, item.GIRDLE_CONDITION, item.GIRDLE_TYPE,
                    item.LASER_INSCRIPTION, item.CERTIFICATE_DATE, item.CROWN_ANGLE, item.CROWN_HEIGHT, item.PAVILION_ANGLE, item.PAVILION_HEIGHT, item.GIRDLE_PER, item.LR_HALF,
                    item.STAR_LN, item.CERT_TYPE, item.FANCY_COLOR, item.FANCY_INTENSITY, item.FANCY_OVERTONE, item.Image2, item.Image2, item.Video2, item.Video2, item.CERTIFICATE_LINK,
                    item.DNA, item.IMAGE_HEART_LINK, item.IMAGE_ARROW_LINK, item.H_A_LINK, item.CERTIFICATE_TYPE_LINK, item.KEY_TO_SYMBOL, item.LAB_COMMENTS, item.SUPPLIER_COMMENTS,
                    item.ORIGIN, item.BOW_TIE, item.EXTRA_FACET_TABLE, item.EXTRA_FACET_CROWN, item.EXTRA_FACET_PAVILION, item.INTERNAL_GRAINING, item.H_A, item.SUPPLIER_DISC, item.SUPPLIER_AMOUNT,
                    item.OFFER_DISC, item.OFFER_VALUE, item.MAX_SLAB_BASE_DISC, item.MAX_SLAB_BASE_VALUE, item.EYE_CLEAN, item.Short_Code);
            }
            return dt_stock_data;
        }
        private DataTable Set_Column_And_Rows_In_DataTable<T>(IList<T> items)
        {
            DataTable dt = new DataTable();

            if (items != null && items.Any())
            {
                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (var item in items)
                {
                    DataRow row = dt.NewRow();
                    foreach (var prop in properties)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
        private string Split_Supplier_Stock_Measurement(string expression, string dimension)
        {
            try
            {

                expression = expression.Replace("-", "*");
                // Define a regular expression pattern to capture numeric values
                string pattern = @"[-+]?\d*\.\d+|[-+]?\d+";

                // Use Regex.Matches to find all matches
                MatchCollection matches = Regex.Matches(expression, pattern);

                // Extract numeric values and store them in an array
                double[] values = matches.Cast<System.Text.RegularExpressions.Match>().Select(match => Convert.ToDouble(match.Value)).ToArray();

                // Check if any numeric values were found
                if (values.Length > 0)
                {
                    // Sort the values in descending order
                    System.Array.Sort(values, (a, b) => b.CompareTo(a));

                    // Determine the dimension to return
                    switch (dimension.ToLower())
                    {
                        case "length":
                            return values[0].ToString();
                        case "width":
                            return values.Length > 1 ? values[1].ToString() : "0"; // Consider 0 if no width value
                        case "depth":
                            return values.Length > 2 ? values[2].ToString() : "0"; // Consider 0 if no depth value
                        default:
                            throw new ArgumentException("Invalid dimension specified.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it if needed
                throw;
            }

            // Return 0 if no numeric values were found
            return "0";
        }
        private DataTable Set_GIA_Lab_Parameter_Column_In_Datatable(DataTable dataTable, IList<GIA_Lab_Parameter> gIA_Lab_Parameters)
        {
            dataTable.Columns.Add("Report_No", typeof(string));
            dataTable.Columns.Add("Report_Date", typeof(string));
            dataTable.Columns.Add("Length", typeof(float));
            dataTable.Columns.Add("Width", typeof(float));
            dataTable.Columns.Add("Depth", typeof(float));
            dataTable.Columns.Add("Color_Grade", typeof(string));
            dataTable.Columns.Add("Clarity_Grade", typeof(string));
            dataTable.Columns.Add("Cut_Grade", typeof(string));
            dataTable.Columns.Add("Polish_Grade", typeof(string));
            dataTable.Columns.Add("Symmetry_Grade", typeof(string));
            dataTable.Columns.Add("Fluorescence", typeof(string));
            dataTable.Columns.Add("Inscriptions", typeof(string));
            dataTable.Columns.Add("Key_To_Symbols", typeof(string));
            dataTable.Columns.Add("Report_Comments", typeof(string));
            dataTable.Columns.Add("Table_Pct", typeof(float));
            dataTable.Columns.Add("Depth_Pct", typeof(float));
            dataTable.Columns.Add("Crown_Angle", typeof(float));
            dataTable.Columns.Add("Crown_Height", typeof(float));
            dataTable.Columns.Add("Pavilion_Angle", typeof(float));
            dataTable.Columns.Add("Pavilion_Depth", typeof(float));
            dataTable.Columns.Add("Star_Length", typeof(float));
            dataTable.Columns.Add("Lower_Half", typeof(float));
            dataTable.Columns.Add("Shape_Code", typeof(string));
            dataTable.Columns.Add("Shape_Group", typeof(string));
            dataTable.Columns.Add("Carats", typeof(float));
            dataTable.Columns.Add("Clarity", typeof(string));
            dataTable.Columns.Add("Cut", typeof(string));
            dataTable.Columns.Add("Polish", typeof(string));
            dataTable.Columns.Add("Symmetry", typeof(string));
            dataTable.Columns.Add("Fluorescence_Intensity", typeof(string));
            dataTable.Columns.Add("Fluorescence_Color");
            dataTable.Columns.Add("Girdle_Condition", typeof(string));
            dataTable.Columns.Add("Girdle_Condition_Code", typeof(string));
            dataTable.Columns.Add("Girdle_Pct", typeof(float));
            dataTable.Columns.Add("Girdle_Size", typeof(string));
            dataTable.Columns.Add("Girdle_Size_Code", typeof(string));
            dataTable.Columns.Add("Culet_Code", typeof(string));
            dataTable.Columns.Add("Certificate_PDF", typeof(string));
            dataTable.Columns.Add("Plotting_Diagram", typeof(string));
            dataTable.Columns.Add("Proportions_Diagram", typeof(string));
            dataTable.Columns.Add("Digital_Card", typeof(string));

            if (gIA_Lab_Parameters != null && gIA_Lab_Parameters.Count > 0)
            {
                foreach (var item in gIA_Lab_Parameters)
                {
                    dataTable.Rows.Add(item.Report_No, item.Report_Date, item.Length, item.Width, item.Depth, item.Color_Grade, item.Clarity_Grade, item.Cut_Grade, item.Polish_Grade
                                        , item.Symmetry_Grade, item.Fluorescence, item.Inscriptions, item.Key_To_Symbols, item.Report_Comments, item.Table_Pct, item.Depth_Pct, item.Crown_Angle
                                        , item.Crown_Height, item.Pavilion_Angle, item.Pavilion_Depth, item.Star_Length, item.Lower_Half, item.Shape_Code, item.Shape_Group, item.Carats
                                        , item.Clarity, item.Cut, item.Polish, item.Symmetry, item.Fluorescence_Intensity, item.Fluorescence_Color, item.Girdle_Condition, item.Girdle_Condition_Code
                                        , item.Girdle_Pct, item.Girdle_Size, item.Girdle_Size_Code, item.Culet_Code, item.Certificate_PDF, item.Plotting_Diagram, item.Proportions_Diagram
                                        , item.Digital_Card);
                }
            }

            return dataTable;
        }

        #endregion

        #region Methods
        #region Party Master
        [HttpGet]
        [Route("getparty")]
        [Authorize]
        public async Task<IActionResult> GetParty(int party_Id, string party_Type)
        {
            try
            {
                var result = await _partyService.GetParty(party_Id, party_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetParty", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_raplicate")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Raplicate(int party_Id, string party_Type)
        {
            try
            {
                var result = await _partyService.GetParty_Raplicate(party_Id, party_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetParty_Raplicate", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_raplicate_08052024")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Raplicate_08052024(int party_Id, string party_Type, int page_Size, int Page_No)
        {
            try
            {
                var result = await _partyService.GetPartyReplicateFromCache(party_Id, party_Type, page_Size, Page_No);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetParty_Raplicate", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("getpartycustomer")]
        [Authorize]
        public async Task<IActionResult> GetPartyCustomer(int party_Id, string party_Type)
        {
            try
            {
                var result = await _partyService.GetPartyCustomer(party_Id, party_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetParty", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }

        }

        [HttpDelete]
        [Route("deleteparty")]
        [Authorize]
        public async Task<IActionResult> DeleteParty(int party_Id)
        {
            try
            {
                var (message, result) = await _partyService.DeleteParty(party_Id);
                if (message == "success" && result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.PartyMasterDeleted
                    });
                }
                else if (message == "_reference_found" && result == (int)HttpStatusCode.Conflict)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "Reference found in the Party Api/ Party File/ Party FTP, you can not delete this record."
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "parameter mismatched."
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteEmployee", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("change_status_party_master")]
        [Authorize]
        public async Task<IActionResult> Change_Status_Party_Master(int party_Id, bool status)
        {
            try
            {
                var result = await _partyService.Party_Master_Change_Status(party_Id, status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Change_Status_Party_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_search_result")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Search_Result(string common_Search)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _partyService.Get_Party_Search_Select(common_Search, user_Id ?? 0);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Search_Result", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Party Bank
        [HttpPut]
        [Route("changestatuspartybank")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusPartyBank(int account_Id, bool status)
        {
            try
            {
                var result = await _partyService.PartyBankChangeStatus(account_Id, status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusPartyBank", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Party Shipping
        [HttpPut]
        [Route("changestatuspartyshipping")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusPartyShipping(int shipId, bool status)
        {
            try
            {
                var result = await _partyService.PartyShippingChangeStatus(shipId, status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusPartyShipping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
        #endregion

        #region Party Details
        [HttpGet]
        [Route("get_party_details")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Details(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_Party_Details(party_Id);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Details", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_party_detils")]
        [Authorize]
        public async Task<IActionResult> Create_Party_Detils([FromForm] Party_Master party_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    var (message, party_Id) = await _partyService.Add_Update_Party(party_Master);
                    if (message == "success" && party_Id > 0)
                    {
                        // Party Contact Detail
                        if (party_Master.Party_Contact_List != null && party_Master.Party_Contact_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Contact_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Prefix", typeof(string));
                            dataTable.Columns.Add("First_Name", typeof(string));
                            dataTable.Columns.Add("Last_Name", typeof(string));
                            dataTable.Columns.Add("Designation_Id", typeof(int));
                            dataTable.Columns.Add("Phone_No", typeof(string));
                            dataTable.Columns.Add("Phone_No_Country_Code", typeof(string));
                            dataTable.Columns.Add("Mobile_No", typeof(string));
                            dataTable.Columns.Add("Mobile_No_Country_Code", typeof(string));
                            dataTable.Columns.Add("Email", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Contact_Name", typeof(string));
                            //    dataTable1.Columns.Add("Sex", typeof(string));
                            //    dataTable1.Columns.Add("Designation_Id", typeof(int));
                            //    dataTable1.Columns.Add("Mobile_No", typeof(string));
                            //    dataTable1.Columns.Add("Email", typeof(string));
                            //    dataTable1.Columns.Add("Birth_Date", typeof(string));
                            //}

                            foreach (var item in party_Master.Party_Contact_List)
                            {
                                dataTable.Rows.Add(item.Contact_Id, party_Id, item.Prefix, item.First_Name, item.Last_Name, item.Designation_Id, item.Phone_No, item.Phone_No_Country_Code, item.Mobile_No, item.Mobile_No_Country_Code, item.Email, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Contact_Name, item.Sex, item.Designation_Id, item.Mobile_No, item.Email, item.Birth_Date, false);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Contact_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyContact(dataTable);
                        }
                        // Party Assist
                        if (party_Master.Party_Assist_List != null && party_Master.Party_Assist_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Assist_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Diamond_Type", typeof(int));
                            dataTable.Columns.Add("Assist_1", typeof(int));
                            dataTable.Columns.Add("Per_1", typeof(float));
                            dataTable.Columns.Add("Assist_2", typeof(int));
                            dataTable.Columns.Add("Per_2", typeof(float));
                            dataTable.Columns.Add("Viewing_Rights_To", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));
                            dataTable.Columns.Add("Date", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Diamond_Type", typeof(int));
                            //    dataTable1.Columns.Add("Assist_1", typeof(int));
                            //    dataTable1.Columns.Add("Per_1", typeof(decimal));
                            //    dataTable1.Columns.Add("Assist_2", typeof(int));
                            //    dataTable1.Columns.Add("Per_2", typeof(decimal));
                            //    dataTable1.Columns.Add("Assist_3", typeof(int));
                            //}
                            foreach (var item in party_Master.Party_Assist_List)
                            {
                                var tot_per = (item.Per_1 + item.Per_2);
                                if (tot_per > 100)
                                {
                                    return Conflict(new
                                    {
                                        statusCode = HttpStatusCode.Conflict,
                                        message = "Less then or equal to 100% allow."
                                    });
                                }
                                else
                                {
                                    dataTable.Rows.Add(item.Assist_Id, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Viewing_Rights_To, item.QueryFlag, item.Date);
                                }
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Viewing_Rights_To);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Assist_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyAssist(dataTable);
                        }
                        // Party Bank Detail
                        if (party_Master.Party_Bank_List != null && party_Master.Party_Bank_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Account_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Bank_Id", typeof(int));
                            dataTable.Columns.Add("Account_No", typeof(string));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("Account_Type", typeof(int));
                            dataTable.Columns.Add("Default_Bank", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Bank_Id", typeof(int));
                            //    dataTable1.Columns.Add("Account_No", typeof(int));
                            //    dataTable1.Columns.Add("Status", typeof(bool));
                            //    dataTable1.Columns.Add("Account_Type", typeof(int));
                            //}

                            foreach (var item in party_Master.Party_Bank_List)
                            {
                                dataTable.Rows.Add(item.Account_Id, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type, item.Default_Bank, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Bank_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyBank(dataTable);
                        }
                        // Party KYC Document
                        if (party_Master.Party_Document_List != null && party_Master.Party_Document_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Document_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Document_Type", typeof(int));
                            dataTable.Columns.Add("Document_No", typeof(string));
                            dataTable.Columns.Add("Upload_Path", typeof(string));
                            dataTable.Columns.Add("Valid_From", typeof(string));
                            dataTable.Columns.Add("Valid_To", typeof(string));
                            dataTable.Columns.Add("Kyc_Grade", typeof(int));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Document_Type", typeof(int));
                            //    dataTable1.Columns.Add("Document_No", typeof(string));
                            //    dataTable1.Columns.Add("Upload_Path", typeof(int));
                            //    dataTable1.Columns.Add("Valid_From", typeof(DateTime));
                            //    dataTable1.Columns.Add("Valid_To", typeof(DateTime));
                            //    dataTable1.Columns.Add("Kyc_Grade", typeof(int));
                            //}

                            foreach (var item in party_Master.Party_Document_List)
                            {
                                if (item.Upload_Path_Name != null && item.Upload_Path_Name.Length > 0)
                                {
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/PartyDocuments");
                                    if (!(Directory.Exists(filePath)))
                                    {
                                        Directory.CreateDirectory(filePath);
                                    }
                                    string fileName = Path.GetFileNameWithoutExtension(item.Upload_Path_Name.FileName);
                                    string fileExt = Path.GetExtension(item.Upload_Path_Name.FileName);

                                    string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                                    using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                    {
                                        await item.Upload_Path_Name.CopyToAsync(fileStream);
                                    }
                                    item.Upload_Path = strFile;
                                }
                                dataTable.Rows.Add(item.Document_Id, party_Id, item.Document_Type, item.Document_No, item.Upload_Path, item.Valid_From, item.Valid_To, item.Kyc_Grade, item.QueryFlag);

                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Document_Type, item.Document_No, item.Upload_Path, item.Valid_From, item.Valid_To, item.Kyc_Grade);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Document_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyDocument(dataTable);
                        }
                        //Party Media
                        if (party_Master.Party_Media_List != null && party_Master.Party_Media_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Party_Media_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Cat_val_Id", typeof(int));
                            dataTable.Columns.Add("ID", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Party Media Log
                            DataTable dataTable1 = new DataTable();
                            dataTable1.Columns.Add("Employee_Id", typeof(int));
                            dataTable1.Columns.Add("IP_Address", typeof(string));
                            dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            dataTable1.Columns.Add("Record_Type", typeof(string));
                            dataTable1.Columns.Add("Party_Id", typeof(int));
                            dataTable1.Columns.Add("Cat_val_Id", typeof(int));
                            dataTable1.Columns.Add("ID", typeof(string));
                            #endregion

                            foreach (var item in party_Master.Party_Media_List)
                            {
                                dataTable.Rows.Add(item.Party_Media_Id, party_Id, item.Cat_val_Id, item.ID, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Cat_val_Id, item.ID);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Media_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyMedia(dataTable);
                        }
                        // Party Shipping Detail
                        if (party_Master.Party_Shipping_List != null && party_Master.Party_Shipping_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Ship_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Company_Name", typeof(string));
                            dataTable.Columns.Add("Address_1", typeof(string));
                            dataTable.Columns.Add("Address_2", typeof(string));
                            dataTable.Columns.Add("Address_3", typeof(string));
                            dataTable.Columns.Add("City_Id", typeof(int));
                            dataTable.Columns.Add("Mobile_No", typeof(string));
                            dataTable.Columns.Add("Mobile_No_Country_Code", typeof(string));
                            dataTable.Columns.Add("Phone_No", typeof(string));
                            dataTable.Columns.Add("Phone_No_Country_Code", typeof(string));
                            dataTable.Columns.Add("Contact_Person", typeof(string));
                            dataTable.Columns.Add("TIN_No", typeof(string));
                            dataTable.Columns.Add("Default_Address", typeof(bool));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("Is_Editable", typeof(bool));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Company_Name", typeof(string));
                            //    dataTable1.Columns.Add("Address_1", typeof(string));
                            //    dataTable1.Columns.Add("Address_2", typeof(string));
                            //    dataTable1.Columns.Add("Address_3", typeof(string));
                            //    dataTable1.Columns.Add("City_Id", typeof(int));
                            //    dataTable1.Columns.Add("Mobile_No", typeof(string));
                            //    dataTable1.Columns.Add("Phone_No", typeof(string));
                            //    dataTable1.Columns.Add("Contact_Person", typeof(string));
                            //    dataTable1.Columns.Add("Contact_Email", typeof(string));
                            //    dataTable1.Columns.Add("Default_Address", typeof(bool));
                            //    dataTable1.Columns.Add("Status", typeof(bool));
                            //}

                            foreach (var item in party_Master.Party_Shipping_List)
                            {
                                dataTable.Rows.Add(item.Ship_Id, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Mobile_No_Country_Code, item.Phone_No, item.Phone_No_Country_Code, item.Contact_Person, item.TIN_No, item.Default_Address, item.Status, item.Is_Editable, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Phone_No, item.Contact_Person, item.TIN_No, item.Default_Address, item.Status);
                                //}
                            }
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Shipping_Trace(dataTable1);
                            //}
                            await _partyService.AddUpdatePartyShipping(dataTable);
                        }
                        //Party Print Process
                        if (party_Master.Party_Print_Process_List != null && party_Master.Party_Print_Process_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Print_Process_Id", typeof(int));
                            dataTable.Columns.Add("Party_Id", typeof(int));
                            dataTable.Columns.Add("Start_Date", typeof(string));
                            dataTable.Columns.Add("Process_Type", typeof(string));
                            dataTable.Columns.Add("Default_Printing_Type", typeof(int));
                            dataTable.Columns.Add("Default_Currency", typeof(int));
                            dataTable.Columns.Add("Default_Bank", typeof(int));
                            dataTable.Columns.Add("Default_Payment_Terms", typeof(int));
                            dataTable.Columns.Add("Default_Remarks", typeof(int));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            #region Party Print Process
                            //DataTable dataTable1 = new DataTable();
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    dataTable1.Columns.Add("Employee_Id", typeof(int));
                            //    dataTable1.Columns.Add("IP_Address", typeof(string));
                            //    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                            //    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                            //    dataTable1.Columns.Add("Record_Type", typeof(string));
                            //    dataTable1.Columns.Add("Party_Id", typeof(int));
                            //    dataTable1.Columns.Add("Start_Date", typeof(string));
                            //    dataTable1.Columns.Add("Process_Type", typeof(string));
                            //    dataTable1.Columns.Add("Default_Printing_Type", typeof(int));
                            //    dataTable1.Columns.Add("Default_Currency", typeof(int));
                            //    dataTable1.Columns.Add("Default_Bank", typeof(int));
                            //    dataTable1.Columns.Add("Default_Payment_Terms", typeof(int));
                            //    dataTable1.Columns.Add("Default_Remarks", typeof(int));
                            //}
                            #endregion

                            foreach (var item in party_Master.Party_Print_Process_List)
                            {
                                dataTable.Rows.Add(item.Print_Process_Id, party_Id, item.Start_Date, item.Process_Type, item.Default_Printing_Type, item.Default_Currency, item.Default_Bank, item.Default_Payment_Terms, item.Default_Remarks, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Start_Date, item.Process_Type, item.Default_Printing_Type, item.Default_Currency, item.Default_Bank, item.Default_Payment_Terms, item.Default_Remarks);
                                //}
                            }
                            await _partyService.Add_Update_Party_Print_Process(dataTable);
                            //if (CoreService.Enable_Trace_Records(_configuration))
                            //{
                            //    await _partyService.Insert_Party_Print_Trace(dataTable1);
                            //}
                        }

                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = party_Master.Party_Id > 0 ? CoreCommonMessage.PartyMasterUpdated : CoreCommonMessage.PartyMasterCreated
                        });
                    }
                    else if (message == "_party_exists" && party_Id == 0)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.PartyAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Party_Detils", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_code")]
        public async Task<IActionResult> Get_Party_Code()
        {
            try
            {
                var result = await _partyService.Get_Party_Code();
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Code", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_user_name")]
        public async Task<IActionResult> Get_User_Name(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_User_Name_From_Party_Contact(party_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_User_Name", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Details
        [HttpPost]
        [Route("create_update_supplier_detail")]
        [Authorize]
        public virtual async Task<IActionResult> Create_Update_Supplier_Detail([FromForm] Supplier_Details supplier_Details, IFormFile File_Location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool success = false;
                    if (supplier_Details.Party_Api != null)
                    {
                        supplier_Details.Party_Api.Party_Id = supplier_Details.Party_Id;
                        var party_Api = await _partyService.Add_Update_Party_API(supplier_Details.Party_Api, supplier_Details.User_Id ?? 0);
                        if (party_Api > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Party_FTP != null)
                    {
                        supplier_Details.Party_FTP.Party_Id = supplier_Details.Party_Id;
                        var party_ftp = await _partyService.Add_Update_Party_FTP(supplier_Details.Party_FTP, supplier_Details.User_Id ?? 0);
                        if (party_ftp > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Party_File != null)
                    {
                        if (File_Location != null && File_Location.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierDetailFile");
                            if (!(Directory.Exists(filePath)))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            string fileName = Path.GetFileNameWithoutExtension(File_Location.FileName);
                            string fileExt = Path.GetExtension(File_Location.FileName);

                            string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                            using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                            {
                                await File_Location.CopyToAsync(fileStream);
                            }
                            supplier_Details.Party_File.File_Location = strFile;
                        }
                        supplier_Details.Party_File.Party_Id = supplier_Details.Party_Id;
                        var party_file = await _partyService.Add_Update_Party_File(supplier_Details.Party_File, supplier_Details.User_Id ?? 0);
                        if (party_file > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Supplier_Column_Mapping_List != null && supplier_Details.Supplier_Column_Mapping_List.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Supp_Col_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Id", typeof(int));
                        dataTable.Columns.Add("Col_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Col_Name", typeof(string));
                        dataTable.Columns.Add("Column_Type", typeof(string));
                        dataTable.Columns.Add("Column_Synonym", typeof(string));
                        dataTable.Columns.Add("Is_Split", typeof(bool));
                        dataTable.Columns.Add("Separator", typeof(string));

                        foreach (var item in supplier_Details.Supplier_Column_Mapping_List)
                        {
                            dataTable.Rows.Add(item.Supp_Col_Id, supplier_Details.Party_Id, item.Col_Id, item.Supp_Col_Name, item.Column_Type, item.Column_Synonym, item.Is_Split, item.Separator);
                        }
                        var result = await _supplierService.Add_Update_Supplier_Column_Mapping(dataTable);
                        if (result > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Supplier_Value_Mapping_List != null && supplier_Details.Supplier_Value_Mapping_List.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Sup_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Cat_Name", typeof(string));
                        dataTable.Columns.Add("Cat_val_Id", typeof(int));
                        dataTable.Columns.Add("Col_Id", typeof(int));
                        dataTable.Columns.Add("Status", typeof(bool));

                        foreach (var item in supplier_Details.Supplier_Value_Mapping_List)
                        {
                            dataTable.Rows.Add(supplier_Details.Party_Id, item.Supp_Cat_Name, item.Cat_val_Id, item.Col_Id, item.Status);
                        }
                        var result = await _supplierService.Insert_Update_Supplier_Value_Mapping(dataTable);
                        if (result > 0)
                        {
                            //var supplier_Id = supplier_Details.Supplier_Value_Mapping_List.Select(x => x.Sup_Id).FirstOrDefault();
                            //var stock_Data = await _supplierService.Get_Not_Uploaded_Stock_Data(supplier_Id ?? 0);
                            //DataTable dataTable1 = new DataTable();
                            //dataTable1 = Set_Supp_Stock_Column_In_Datatable(new DataTable(), stock_Data);
                            //await _supplierService.Supplier_Stock_Insert_Update(dataTable1, supplier_Id ?? 0);
                            success = true;
                        }
                    }
                    if (success)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SupplierDetailSavedSuccessfully
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Detail(int party_Id, string map_Flag, string column_Type)
        {
            try
            {
                var supplier_detail = new Supplier_Details();
                supplier_detail.Party_Id = party_Id;
                supplier_detail.Party_Api = await _partyService.Get_Party_API(0, party_Id);
                supplier_detail.Party_FTP = await _partyService.Get_Party_FTP(0, party_Id);
                supplier_detail.Party_File = await _partyService.Get_Party_File(0, party_Id);
                supplier_detail.Supplier_Column_Mapping_List = await _supplierService.Get_Supplier_Column_Mapping(party_Id, map_Flag, column_Type);
                if (supplier_detail.Party_File != null)
                {
                    supplier_detail.Party_File.File_Location = !string.IsNullOrEmpty(supplier_detail.Party_File.File_Location) ? _configuration["BaseUrl"] + CoreCommonFilePath.SupplierFilePath + supplier_detail.Party_File.File_Location : null;
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = supplier_detail
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_detail_list")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Detail_List(int party_Id)
        {
            try
            {
                var result = await _partyService.Get_Suplier_Detail_List(party_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Detail_List", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_supplier")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Supplier()
        {
            try
            {
                var result = await _partyService.Get_Party_Suplier();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Supplier", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_type_courier")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Type_Courier()
        {
            try
            {
                var result = await _partyService.Get_Party_Type_Courier();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Type_Courier", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_type_customer")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Type_Customer()
        {
            try
            {
                var result = await _partyService.Get_Party_Type_Customer();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Type_Customer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_column_mapping")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Column_Mapping(int party_Id, string map_Flag, string column_Type)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Column_Mapping(party_Id, map_Flag, column_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Column_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_party_api_with_column_mapping")]
        public async Task<IActionResult> Get_Party_API_With_Column_Mapping()
        {
            try
            {
                var result = await _partyService.Get_Party_API_With_Column_Mapping();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_API_With_Column_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_api_ftp_file")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_API_FTP_File(int id, string upload_Type)
        {
            try
            {
                Party_API_With_Column_Mapping supplier_API_FTP_File_Details = new Party_API_With_Column_Mapping();
                if (!string.IsNullOrEmpty(upload_Type) && upload_Type == "API")
                {
                    var result = await _partyService.Get_Party_API(id, 0);
                    if (result != null)
                    {
                        supplier_API_FTP_File_Details.API_Id = result.API_Id;
                        supplier_API_FTP_File_Details.Party_Id = result.Party_Id;
                        supplier_API_FTP_File_Details.Upload_Type = upload_Type;
                        supplier_API_FTP_File_Details.API_URL = result.API_URL;
                        supplier_API_FTP_File_Details.API_User = result.API_User;
                        supplier_API_FTP_File_Details.API_Password = result.API_Password;
                        supplier_API_FTP_File_Details.API_Method = result.API_Method;
                        supplier_API_FTP_File_Details.API_Response = result.API_Response;
                        supplier_API_FTP_File_Details.API_Status = result.API_Status;
                        supplier_API_FTP_File_Details.Disc_Inverse = result.Disc_Inverse;
                        supplier_API_FTP_File_Details.Auto_Ref_No = result.Auto_Ref_No;
                        supplier_API_FTP_File_Details.RepeateveryType = result.RepeateveryType;
                        supplier_API_FTP_File_Details.Repeatevery = result.Repeatevery;
                        supplier_API_FTP_File_Details.Lab = result.Lab;
                        supplier_API_FTP_File_Details.Overseas = result.Overseas;
                        supplier_API_FTP_File_Details.Stock_Url = result.Stock_Url;
                        supplier_API_FTP_File_Details.User_Id = result.User_Id;
                        supplier_API_FTP_File_Details.User_Caption = result.User_Caption;
                        supplier_API_FTP_File_Details.Password_Caption = result.Password_Caption;
                        supplier_API_FTP_File_Details.Action_Caption = result.Action_Caption;
                        supplier_API_FTP_File_Details.Action_Value = result.Action_Value;
                        supplier_API_FTP_File_Details.Action_Caption_1 = result.Action_Caption_1;
                        supplier_API_FTP_File_Details.Action_Value_1 = result.Action_Value_1;
                        supplier_API_FTP_File_Details.Action_Caption_2 = result.Action_Caption_2;
                        supplier_API_FTP_File_Details.Action_Value_2 = result.Action_Value_2;
                        supplier_API_FTP_File_Details.Short_Code = result.Short_Code;
                        supplier_API_FTP_File_Details.Stock_Api_Method = result.Stock_Api_Method;
                        supplier_API_FTP_File_Details.Method_Type = result.Method_Type;
                        supplier_API_FTP_File_Details.Supplier_Column_Mapping_List = await _partyService.Common_Funtion_To_Get_Supp_Col_Map(supplier_API_FTP_File_Details.Party_Id ?? 0, "API");
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = supplier_API_FTP_File_Details
                        });
                    }
                }
                else if (!string.IsNullOrEmpty(upload_Type) && upload_Type == "FTP")
                {
                    var result = await _partyService.Get_Party_FTP(id, 0);
                    if (result != null)
                    {
                        supplier_API_FTP_File_Details.FTP_Id = result.FTP_Id;
                        supplier_API_FTP_File_Details.Party_Id = result.Party_Id;
                        supplier_API_FTP_File_Details.Upload_Type = upload_Type;
                        supplier_API_FTP_File_Details.Disc_Inverse = result.Disc_Inverse;
                        supplier_API_FTP_File_Details.Auto_Ref_No = result.Auto_Ref_No;
                        supplier_API_FTP_File_Details.RepeateveryType = result.RepeateveryType;
                        supplier_API_FTP_File_Details.Repeatevery = result.Repeatevery;
                        supplier_API_FTP_File_Details.Lab = result.Lab;
                        supplier_API_FTP_File_Details.Overseas = result.Overseas;
                        supplier_API_FTP_File_Details.Short_Code = result.Short_Code;
                        supplier_API_FTP_File_Details.Host = result.Host;
                        supplier_API_FTP_File_Details.Ftp_Port = result.Ftp_Port;
                        supplier_API_FTP_File_Details.Ftp_User = result.Ftp_User;
                        supplier_API_FTP_File_Details.Ftp_Password = result.Ftp_Password;
                        supplier_API_FTP_File_Details.Ftp_File_Name = result.Ftp_File_Name;
                        supplier_API_FTP_File_Details.Ftp_File_Type = result.Ftp_File_Type;
                        supplier_API_FTP_File_Details.Secure_Ftp = result.Secure_Ftp;
                        supplier_API_FTP_File_Details.Supplier_Column_Mapping_List = await _partyService.Common_Funtion_To_Get_Supp_Col_Map(supplier_API_FTP_File_Details.Party_Id ?? 0, "FTP");
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = supplier_API_FTP_File_Details
                        });
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Send_Stock_On_Email", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Value Config
        [HttpGet]
        [Route("get_value_config")]
        [Authorize]
        public async Task<IActionResult> Get_Value_Config(int valueMap_ID)
        {
            try
            {
                var result = await _supplierService.Get_Value_Config(valueMap_ID);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_value_config")]
        [Authorize]
        public async Task<IActionResult> Create_Value_Config(Value_Config value_Config)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Add_Update_Value_Config(value_Config);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ValueConfigCreated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_value_config")]
        [Authorize]
        public async Task<IActionResult> Update_Value_Config(Value_Config value_Config)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Add_Update_Value_Config(value_Config);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ValueConfigUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_value_config")]
        [Authorize]
        public async Task<IActionResult> Delete_Value_Config(int valueMap_ID)
        {
            try
            {
                var result = await _supplierService.Delete_Value_Config(valueMap_ID);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ValueConfigDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Value_Config", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Pricing
        [HttpGet]
        [Route("get_supplier_pricing_list")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Pricing_List()
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Pricing_List();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Pricing_List", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_sunrise_pricing_list")]
        [Authorize]
        public async Task<IActionResult> Get_Sunrise_Pricing_List()
        {
            try
            {
                var result = await _supplierService.Get_Sunrise_Pricing_List();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Sunrise_Pricing_List", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_customer_pricing_list")]
        [Authorize]
        public async Task<IActionResult> Get_Customer_Pricing_List()
        {
            try
            {
                var result = await _supplierService.Get_Customer_Pricing_List();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Customer_Pricing_List", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id, string? user_pricing_Id)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Pricing(supplier_Pricing_Id, supplier_Id, supplier_Filter_Type, map_Flag, sunrise_pricing_Id, customer_pricing_Id, user_pricing_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Create_Supplier_Pricing(IList<Supplier_Pricing> supplier_Pricings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int supplier_Id = 0, sunrise_Pricing_Id = 0, customer_Pricing_Id = 0;
                    int sun_price_Id = 0; int supplier_Pricing_Id = 0;
                    var map_flag = string.Empty;
                    var selected_sun_price_Id = supplier_Pricings.Where(x => x.Sunrise_Pricing_Id > 0).Select(x => x.Sunrise_Pricing_Id).FirstOrDefault() ?? 0;
                    map_flag = supplier_Pricings.Select(x => x.Map_Flag).FirstOrDefault();
                    if (selected_sun_price_Id > 0)
                    {
                        sun_price_Id = selected_sun_price_Id;
                    }
                    else
                    {
                        var data = await _supplierService.Get_Max_Sunrice_Pricing_Id();
                        sun_price_Id = data.Id + 1;
                    }
                    if (supplier_Pricings != null && supplier_Pricings.Count > 0)
                    {
                        bool success = false;
                        if (supplier_Pricings != null && supplier_Pricings.Count > 0)
                        {
                            foreach (var item in supplier_Pricings)
                            {
                                supplier_Id = item.Supplier_Id ?? 0;
                                customer_Pricing_Id = item.Customer_Pricing_Id ?? 0;
                                if (item.Query_Flag == "D")
                                {
                                    var result_Supplier_Pricing_Key_To_Symbol = await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(item.Supplier_Pricing_Id, null);

                                    var result = await _supplierService.Delete_Supplier_Pricing(item.Supplier_Pricing_Id, 0);
                                    if (result > 0)
                                    {
                                        success = true;
                                    }
                                }
                                else
                                {
                                    if (item.Map_Flag == "SPLP" || item.Map_Flag == "SPLS" || item.Map_Flag == "SPOP" || item.Map_Flag == "SPOS")
                                    {
                                        item.Sunrise_Pricing_Id = sun_price_Id;
                                        sunrise_Pricing_Id = item.Sunrise_Pricing_Id ?? 0;
                                    }
                                    var (message, supplier_pricing_Id) = await _supplierService.Add_Update_Supplier_Pricing(item);
                                    if (message == "success" && supplier_pricing_Id > 0)
                                    {
                                        success = true;
                                        supplier_Pricing_Id = supplier_pricing_Id;
                                        if (item.Key_To_Symbol != null && item.Key_To_Symbol.Count > 0)
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Columns.Add("Supplier_Pricing_Id", typeof(int));
                                            dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                                            dataTable.Columns.Add("Symbol_Status", typeof(bool));
                                            dataTable.Columns.Add("Filter_Type", typeof(string));
                                            foreach (var obj in item.Key_To_Symbol)
                                            {
                                                dataTable.Rows.Add(supplier_pricing_Id, obj.Cat_Val_Id, obj.Symbol_Status, obj.Filter_Type);
                                            }
                                            await _supplierService.Add_Update_Supplier_Pricing_Key_To_Symbol(dataTable);
                                        }
                                        else
                                        {
                                            await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(supplier_pricing_Id, "S");
                                        }
                                        if (item.Lab_Comments != null && item.Lab_Comments.Count > 0)
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Columns.Add("Supplier_Pricing_Id", typeof(int));
                                            dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                                            dataTable.Columns.Add("Symbol_Status", typeof(bool));
                                            dataTable.Columns.Add("Filter_Type", typeof(string));
                                            foreach (var obj in item.Lab_Comments)
                                            {
                                                dataTable.Rows.Add(supplier_pricing_Id, obj.Cat_Val_Id, obj.Symbol_Status, obj.Filter_Type);
                                            }
                                            await _supplierService.Add_Update_Supplier_Pricing_Key_To_Symbol(dataTable);
                                        }
                                        else
                                        {
                                            await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(supplier_pricing_Id, "C");
                                        }
                                    }
                                }
                            }
                        }
                        if (success)
                        {
                            string resurn_message = string.Empty;
                            if (map_flag == "P")
                            {
                                resurn_message = CoreCommonMessage.SupplierPricingCreated;
                            }
                            else if (map_flag == "S")
                            {
                                resurn_message = CoreCommonMessage.SupplierStockCreated;
                            }
                            else if (map_flag == "SPLP" || map_flag == "SPLS" || map_flag == "SPOP" || map_flag == "SPOS")
                            {
                                resurn_message = CoreCommonMessage.SunrisePricingCreated;
                            }
                            else if (map_flag == "CPL" || map_flag == "CPO" || map_flag == "CSL" || map_flag == "CSO")
                            {
                                resurn_message = CoreCommonMessage.CustomerPricingCreated;
                            }
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = resurn_message,
                                supplier_Id = supplier_Id,
                                sunrise_Pricing_Id = sunrise_Pricing_Id,
                                customer_Pricing_Id = customer_Pricing_Id,
                                supplier_pricing_Id = supplier_Pricing_Id
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Supplier_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Delete_Supplier_Pricing(int supplier_Id)
        {
            try
            {
                var result = await _supplierService.Delete_Supplier_Pricing_By_Supplier(supplier_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.SupplierPricingDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Supplier_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_sunrise_pricing")]
        [Authorize]
        public async Task<IActionResult> Delete_Sunrise_Pricing(int sunrise_Pricing_Id)
        {
            try
            {
                var result = await _supplierService.Delete_Sunrise_Pricing(sunrise_Pricing_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.SunrisePricingDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Sunrise_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_customer_pricing")]
        [Authorize]
        public async Task<IActionResult> Delete_Customer_Pricing(int supplier_Pricing_Id)
        {
            try
            {
                var result = await _supplierService.Delete_Customer_Pricing(supplier_Pricing_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.CustomerPricingDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Customer_Pricing", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Value Mapping
        [HttpGet]
        [Route("get_supplier_value_mapping")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Value_Mapping(int sup_Id, int col_Id)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Value_Mapping(sup_Id, col_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Value_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_supplier_value_mapping")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Supplier_Value_Mapping([FromForm] IList<Supplier_Value_Mapping> supplier_Value_Mappings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (supplier_Value_Mappings != null && supplier_Value_Mappings.Count > 0)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Sup_Id", typeof(int));
                        dataTable.Columns.Add("Supp_Cat_Name", typeof(string));
                        dataTable.Columns.Add("Cat_val_Id", typeof(int));
                        dataTable.Columns.Add("Status", typeof(bool));

                        foreach (var item in supplier_Value_Mappings)
                        {
                            dataTable.Rows.Add(item.Sup_Id, item.Supp_Cat_Name, item.Cat_val_Id, item.Status);
                        }
                        var result = await _supplierService.Insert_Update_Supplier_Value_Mapping(dataTable);
                        if (result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = "Supplier value mapped successfully."
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Value_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_supplier_value_mapping")]
        [Authorize]
        public async Task<IActionResult> Delete_Supplier_Value_Mapping(int sup_Id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierValueMapping(sup_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.SupplierValueDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Supplier_Value_Mapping", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Stock
        [HttpPost]
        [Route("create_update_supplier_stock")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Supplier_Stock()
        {
            try
            {
                int supplier_Id = 16;
                var stock_data_master = new Stock_Data_Master()
                {
                    Stock_Data_Id = 0,
                    Supplier_Id = supplier_Id,
                    Upload_Method = "API",
                };

                var (message, stock_Data_Id) = await _supplierService.Stock_Data_Insert_Update(stock_data_master);

                if (message == "success" && stock_Data_Id > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://krinalgems.diamx.net/API/StockSearch?APIToken=e161dd39-44ed-4b67-8a48-8406da883892");

                        var response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            response.EnsureSuccessStatusCode();

                            var json = await response.Content.ReadAsStringAsync();

                            dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                            JArray list = data.StoneList;

                            // Create a DataTable
                            DataTable dt_our_stock_data = new DataTable();

                            List<Supplier_Column_Mapping> Supplier_Column_Mapping_List = (List<Supplier_Column_Mapping>)await _supplierService.Get_Supplier_Column_Mapping(supplier_Id, "C", "API");

                            if (Supplier_Column_Mapping_List != null && Supplier_Column_Mapping_List.Count > 0)
                            {
                                // Use LINQ to filter and map CSV columns based on matching column names
                                Supplier_Column_Mapping_List = Supplier_Column_Mapping_List.Where(x => !string.IsNullOrEmpty(x.Supp_Col_Name)).ToList();
                                Supplier_Column_Mapping_List
                                .Select(mapping => new DataColumn
                                {
                                    ColumnName = mapping.Display_Name,
                                    DataType = typeof(string)

                                }).ToList();

                                var columnsToAdd = new List<string>
                                     {
                                        "SUPPLIER_NO","CERTIFICATE_NO","LAB","SHAPE","CTS","BASE_DISC","BASE_RATE","BASE_AMOUNT","COLOR","CLARITY","CUT","POLISH","SYMM","FLS_COLOR","FLS_INTENSITY","LENGTH","WIDTH","DEPTH","MEASUREMENT","DEPTH_PER","TABLE_PER","CULET","SHADE","LUSTER","MILKY","BGM","LOCATION","STATUS","TABLE_BLACK","SIDE_BLACK","TABLE_WHITE","SIDE_WHITE","TABLE_OPEN","CROWN_OPEN","PAVILION_OPEN","GIRDLE_OPEN","GIRDLE_FROM","GIRDLE_TO","GIRDLE_CONDITION","GIRDLE_TYPE","LASER_INSCRIPTION","CERTIFICATE_DATE","CROWN_ANGLE","CROWN_HEIGHT","PAVILION_ANGLE","PAVILION_HEIGHT","GIRDLE_PER","LR_HALF","STAR_LN","CERT_TYPE","FANCY_COLOR","FANCY_INTENSITY","FANCY_OVERTONE","IMAGE_LINK","Image2","VIDEO_LINK","Video2","CERTIFICATE_LINK","DNA","IMAGE_HEART_LINK","IMAGE_ARROW_LINK","H_A_LINK","CERTIFICATE_TYPE_LINK","KEY_TO_SYMBOL","LAB_COMMENTS","SUPPLIER_COMMENTS","ORIGIN","BOW_TIE","EXTRA_FACET_TABLE","EXTRA_FACET_CROWN","EXTRA_FACET_PAVILION","INTERNAL_GRAINING","H_A","SUPPLIER_DISC","SUPPLIER_AMOUNT","OFFER_DISC","OFFER_VALUE","MAX_SLAB_BASE_DISC","MAX_SLAB_BASE_VALUE","EYE_CLEAN","Supp_Short_Name"
                                     };

                                var newColumns = columnsToAdd
                                    .Select(columnName => new DataColumn
                                    {
                                        ColumnName = columnName,
                                        DataType = typeof(string)
                                    }).ToList();

                                // Add the new columns to the DataTable
                                dt_our_stock_data.Columns.AddRange(newColumns.ToArray());

                                // Project the dynamic objects into a sequence of DataRow objects
                                var rowsToAdd = list.Select(dynamicObject =>
                                {
                                    DataRow row = dt_our_stock_data.NewRow();

                                    Supplier_Column_Mapping_List.ForEach(mapping =>
                                    {
                                        var columnNames = mapping.Supp_Col_Name.Split(',').Select(col => col.Trim()).ToArray();

                                        var propertyValues = columnNames
                                            .Select(columnName =>
                                            {
                                                var property = ((JObject)dynamicObject).Properties().FirstOrDefault(p => p.Name == columnName);
                                                return property != null ? property.Value.ToString() : string.Empty;
                                            }).Where(value => !string.IsNullOrEmpty(value));

                                        var column = dt_our_stock_data.Columns[mapping.Display_Name];

                                        if (column != null)
                                        {
                                            row[column] = string.Join(", ", propertyValues);
                                        }

                                    });

                                    return row;

                                });

                                // Add the rows to the DataTable using DataTable.Merge
                                dt_our_stock_data.Merge(rowsToAdd.CopyToDataTable(), false, MissingSchemaAction.Ignore);

                                var result = await _supplierService.Stock_Data_Detail_Insert_Update(dt_our_stock_data, stock_Data_Id);
                                if (result > 0)
                                {
                                    return Ok(new
                                    {
                                        statusCode = HttpStatusCode.OK,
                                        message = CoreCommonMessage.AddedSuccessfully
                                    });
                                }
                            }
                            else
                            {
                                return Conflict(new
                                {
                                    statusCode = HttpStatusCode.Conflict,
                                    message = "Supplier column not found on supplier column mapping"
                                });
                            }
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                statusCode = HttpStatusCode.InternalServerError,
                                message = "Failed to retrieve data from the API"
                            });
                        }
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "Failed to insert/update stock data"
                });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "HTTP request error"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.InternalServerError,
                    message = "An error occurred"
                });
            }
        }

        [HttpPost]
        [Route("create_update_supplier_data")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Supplier_Data(Stock_Data_Master stock_Data_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (message, stock_Data_Id) = await _supplierService.Stock_Data_Insert_Update(stock_Data_Master);
                    if (message == "success" && stock_Data_Id > 0)
                    {
                        if (stock_Data_Master.Stock_Data_List != null && stock_Data_Master.Stock_Data_List.Count > 0)
                        {
                            DataTable dt_our_stock_data = new DataTable();
                            dt_our_stock_data = await Set_Column_In_Datatable(stock_Data_Master.Supplier_Id ?? 0, new DataTable(), stock_Data_Master.Stock_Data_List);
                            var result = await _supplierService.Stock_Data_Detail_Insert_Update(dt_our_stock_data, stock_Data_Id);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.StockUploadedSuccessfully
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Data", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_supplier_stock_by_scheduler")]
        public async Task<IActionResult> Create_Update_Supplier_Stock_By_Scheduler(Stock_Data_Master_Schedular stock_Data_Master)
        {
            var party_List = await _partyService.GetParty_Raplicate(stock_Data_Master.Supplier_Id ?? 0, null);
            var party = party_List.FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    var (message, stock_Data_Id) = await _supplierService.Stock_Data_Custom_Insert_Update(stock_Data_Master);
                    if (message == "success" && stock_Data_Id > 0)
                    {
                        if (stock_Data_Master.Stock_Data_List != null && stock_Data_Master.Stock_Data_List.Count > 0)
                        {
                            DataTable dt_our_stock_data = new DataTable();
                            dt_our_stock_data = await Set_Column_In_Datatable_Scheduler(new DataTable(), stock_Data_Master.Stock_Data_List, stock_Data_Master.Supplier_Id ?? 0);
                            var result = await _supplierService.Stock_Data_Shedular_Insert_Update(dt_our_stock_data, stock_Data_Id);
                            if (result > 0)
                            {
                                if (stock_Data_Master.Upload_Type == "O")
                                {
                                    await _supplierService.Supplier_Stock_Insert_Update((int)stock_Data_Master.Supplier_Id, stock_Data_Id);
                                }
                            }
                        }
                        else
                        {
                            await _commonService.InsertErrorLog(stock_Data_Master.Error_Message + ":- " + party.Party_Name, "Create_Update_Supplier_Stock_By_Scheduler", null);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.StockUploadedSuccessfully
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message + ":- " + party.Party_Name, "Create_Update_Supplier_Stock_By_Scheduler", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_stock_data_distinct_column_values")]
        [Authorize]
        public async Task<IActionResult> Get_Stock_Data_Distinct_Column_Values(string column_Name, int supplier_Id)
        {
            try
            {
                var result = await _supplierService.Get_Stock_Data_Distinct_Column_Values(column_Name, supplier_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Stock_Data_Distinct_Column_Values", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Stock Number Generate
        [HttpGet]
        [Route("get_stock_number_generation")]
        [Authorize]
        public async Task<IActionResult> Get_Stock_Number_Generation(int Id)
        {
            try
            {
                var result = await _supplierService.Get_Stock_Number_Generation(Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Number_Generation", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_stock_number_generation")]
        [Authorize]
        public async Task<IActionResult> Create_Stock_Number_Generation(Stock_Number_Generation_List stock_Number_Generation_List)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = stock_Number_Generation_List.stock_Number_Generations.Select(x => x.Id).FirstOrDefault();
                    if (stock_Number_Generation_List.stock_Number_Generations != null && stock_Number_Generation_List.stock_Number_Generations.Count > 0)
                    {

                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Id", typeof(int));
                        dataTable.Columns.Add("Exc_Party_Id", typeof(string));
                        dataTable.Columns.Add("Pointer_Id", typeof(string));
                        dataTable.Columns.Add("Shape", typeof(string));
                        dataTable.Columns.Add("Stock_Type", typeof(string));
                        dataTable.Columns.Add("Front_Prefix", typeof(string));
                        dataTable.Columns.Add("Back_Prefix", typeof(string));
                        dataTable.Columns.Add("Front_Prefix_Alloted", typeof(string));
                        dataTable.Columns.Add("Start_Format", typeof(string));
                        dataTable.Columns.Add("Start_Number", typeof(string));
                        dataTable.Columns.Add("End_Number", typeof(string));
                        dataTable.Columns.Add("Supplier_Id", typeof(int));
                        dataTable.Columns.Add("Query_Flag", typeof(string));
                        dataTable.Columns.Add("User_Id", typeof(int));
                        string ids = string.Empty;
                        foreach (var item in stock_Number_Generation_List.stock_Number_Generations)
                        {
                            dataTable.Rows.Add(item.Id, item.Exc_Party_Id, item.Pointer_Id, item.Shape, item.Stock_Type, item.Front_Prefix, item.Back_Prefix, item.Front_Prefix_Alloted, item.Start_Format, item.Start_Number, item.End_Number, item.Supplier_Id, item.Id > 0 ? 'U' : 'I', stock_Number_Generation_List.User_Id);

                            if (item.Id > 0)
                                ids += (ids.Length > 0 ? "," : "") + item.Id;
                        }

                        //DataTable dataTableSinglePointer = new DataTable();
                        //dataTableSinglePointer.Columns.Add("Id", typeof(int));
                        //dataTableSinglePointer.Columns.Add("Exc_Party_Id", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Pointer_Id", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Shape", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Stock_Type", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Front_Prefix", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Back_Prefix", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Front_Prefix_Alloted", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Start_Format", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Start_Number", typeof(string));
                        //dataTableSinglePointer.Columns.Add("End_Number", typeof(string));
                        //dataTableSinglePointer.Columns.Add("Supplier_Id", typeof(int));
                        //dataTableSinglePointer.Columns.Add("Query_Flag", typeof(string));
                        //dataTableSinglePointer.Columns.Add("User_Id", typeof(int));
                        //string ids = string.Empty;
                        //foreach (var item in stock_Number_Generation_List.stock_Number_Generations)
                        //{
                        //    if (item.Id > 0)
                        //        ids += (ids.Length > 0 ? "," : "") + item.Id;

                        //    string[] arrPointer = item.Pointer_Id.Split(",");
                        //    string[] arrFrontPrifixAlloted = item.Front_Prefix_Alloted.Split(",");

                        //    foreach (string pointer in arrPointer)
                        //    {
                        //        foreach (string fromPrifixAlloted in arrFrontPrifixAlloted)
                        //        {
                        //            dataTableSinglePointer.Rows.Add(item.Id, item.Exc_Party_Id, pointer, item.Shape, item.Stock_Type, item.Front_Prefix, item.Back_Prefix, fromPrifixAlloted, item.Start_Format, item.Start_Number, item.End_Number, item.Supplier_Id, item.Id > 0 ? 'U' : 'I', stock_Number_Generation_List.User_Id);
                        //        }
                        //        if (item.Front_Prefix.Length > 0)
                        //        {
                        //            dataTableSinglePointer.Rows.Add(item.Id, item.Exc_Party_Id, pointer, item.Shape, item.Stock_Type, item.Front_Prefix, item.Back_Prefix, item.Front_Prefix, item.Start_Format, item.Start_Number, item.End_Number, item.Supplier_Id, item.Id > 0 ? 'U' : 'I', stock_Number_Generation_List.User_Id);
                        //        }
                        //    }
                        //}

                        var result = await _supplierService.Add_Update_Stock_Number_Generation(dataTable);
                        await _supplierService.Add_Update_Stock_Number_Generation_Raplicate(ids);
                        if (result == 5 || result == 6 || result == 7)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = result == 5 ? CoreCommonMessage.StockNumberAlreadyExistLab : (result == 6 ? CoreCommonMessage.StockNumberAlreadyExistOverses : (result == 7 ? CoreCommonMessage.StockNumberAlreadyExistSunrise : "")),
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = id > 0 ? CoreCommonMessage.StockNumberUpdated : CoreCommonMessage.StockNumberCreated,
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Stock_Number_Generation", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_stock_number_generation")]
        [Authorize]
        public async Task<IActionResult> Delete_Stock_Number_Generation(int Id)
        {
            try
            {
                var result = await _supplierService.Delete_Stock_Number_Generation(Id);
                if (result == 409)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StockNumberDeleted
                    });
                }
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Stock_Number_Generation", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Api/FTP/File Party Name
        [HttpGet]
        [Route("get_api_ftp_file_party_select")]
        [Authorize]
        public async Task<IActionResult> Get_Api_Ftp_File_Party_Select(int party_Id, bool lab, bool overseas, bool is_Stock_Gen)
        {
            try
            {
                var result = await _supplierService.Get_Api_Ftp_File_Party_Select(party_Id, lab, overseas, is_Stock_Gen);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Api_Ftp_File_Party_Select", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier FTP
        [HttpGet]
        [Route("get_supplier_ftp_file")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Ftp_File(int supp_Id)
        {
            try
            {
                var supplier = await _partyService.Get_Party_FTP(0, supp_Id);
                if (supplier != null)
                {
                    string ftpfileName = "/stock-" + Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + ".csv";
                    string ftpUrl = _configuration["BaseUrl"] + CoreCommonFilePath.FtpFilesPath + ftpfileName;
                    string localDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/FTPFile");

                    if (!Directory.Exists(localDirectoryPath))
                    {
                        Directory.CreateDirectory(localDirectoryPath);
                    }
                    string localDirectory = localDirectoryPath + ftpfileName;
                    CoreService.Ftp_File_Download(supplier.Host, supplier.Ftp_User, supplier.Ftp_Password, (int)supplier.Ftp_Port, supplier.Ftp_File_Name, localDirectory);

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.FileDownloadSuccessfully,
                        data = new { url = ftpUrl }
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Ftp_File", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_supplier_ftp_file_scheduler")]
        public async Task<IActionResult> Get_Supplier_Ftp_File_Scheduler(int supp_Id)
        {
            try
            {
                var supplier = await _partyService.Get_Party_FTP(0, supp_Id);
                if (supplier != null)
                {
                    string ftpfileName = "/stock-" + Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + ".csv";
                    string ftpUrl = _configuration["BaseUrl"] + CoreCommonFilePath.FtpFilesPath + ftpfileName;
                    string localDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/FTPFile");

                    if (!Directory.Exists(localDirectoryPath))
                    {
                        Directory.CreateDirectory(localDirectoryPath);
                    }
                    string localDirectory = localDirectoryPath + ftpfileName;
                    CoreService.Ftp_File_Download(supplier.Host, supplier.Ftp_User, supplier.Ftp_Password, (int)supplier.Ftp_Port, supplier.Ftp_File_Name, localDirectory);

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.FileDownloadSuccessfully,
                        data = new { url = ftpUrl }
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Supplier_Ftp_File_Scheduler", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Manual Upload
        [HttpGet]
        [Route("get_party_supplier_for_manual_file")]
        [Authorize]
        public async Task<IActionResult> Get_Party_Supplier_For_Manual_File()
        {
            try
            {
                var result = await _partyService.Get_Party_Suplier_For_Manual_File();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Party_Supplier_For_Manual_File", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_manual_upload")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Manual_Upload([FromForm] Party_File party_File, IFormFile File_Location)
        {
            string party_name = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    //var party_Api = await _partyService.Get_Party_API(0,party_File.Party_Id ?? 0);
                    //var party_FTP = await _partyService.Get_Party_FTP(0, party_File.Party_Id ?? 0);
                    //if (!party_File.is_Continue && party_Api != null && party_Api.API_Status == true && party_Api.Lab == true)
                    //{
                    //    return Conflict(new
                    //    {
                    //        statusCode = HttpStatusCode.Conflict,
                    //        api_Active = true,
                    //        message = "API is active for this supplier, Do you want to continue ?"
                    //    });
                    //}
                    //else if (!party_File.is_Continue && party_FTP != null && party_FTP.Ftp_Status == true && party_FTP.Lab == true)
                    //{
                    //    return Conflict(new
                    //    {
                    //        statusCode = HttpStatusCode.Conflict,
                    //        api_Active = true,
                    //        message = "FTP is active for this supplier, Do you want to continue ?"
                    //    });
                    //}
                    //else
                    //{
                    var supp_col_Map = await _supplierService.Get_Supplier_Column_Mapping(party_File.Party_Id ?? 0, "C", "FILE");
                    int? col_Map_sup_Id = supp_col_Map.Select(x => x.Supp_Id).FirstOrDefault();
                    if (col_Map_sup_Id > 0)
                    {
                        #region Update Party File
                        var party_file_obj = await _partyService.Get_Party_File(0, party_File.Party_Id ?? 0);
                        var parties = await _partyService.GetParty_Raplicate(party_File.Party_Id ?? 0, null);
                        party_name = parties.Select(x => x.Party_Name).FirstOrDefault();
                        if (party_file_obj != null)
                        {
                            if (party_file_obj.Lab == false && party_file_obj.Overseas == false)
                            {
                                return Conflict(new
                                {
                                    statusCode = HttpStatusCode.Conflict,
                                    Supplier_Id = party_File.Party_Id,
                                    Party_Name = party_name,
                                    message = "Kindly check status on supplier api."
                                });
                            }

                            party_file_obj.Sheet_Name = party_File.Sheet_Name;
                            party_file_obj.Validity_Days = party_File.Validity_Days;
                            party_file_obj.API_Flag = party_File.API_Flag;
                            party_file_obj.Exclude = party_File.Exclude;
                            party_file_obj.Overseas_Same_Id = party_File.Overseas_Same_Id;

                            var result = await _partyService.Add_Update_Party_File(party_file_obj, 0);

                            if (result > 0)
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/ManualUploadFiles");
                                if (!(Directory.Exists(filePath)))
                                {
                                    Directory.CreateDirectory(filePath);
                                }
                                string fileName = Path.GetFileNameWithoutExtension(File_Location.FileName);
                                string fileExt = Path.GetExtension(File_Location.FileName);
                                string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;

                                using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                                {
                                    await File_Location.CopyToAsync(fileStream);
                                }
                                party_File.File_Location = strFile;

                                DataTable excel_dataTable = new DataTable();
                                DataTable supplier_column_Mapping = await _supplierService.Get_Supplier_Column_Mapping_In_Datatable(party_File.Party_Id ?? 0, "C", "FILE");

                                var sheet_list = party_File.Sheet_Name.Split(",").ToList();
                                var fileLocation = Path.Combine(filePath, strFile);
                                if (fileExt == ".xls")
                                {
                                    if (sheet_list != null && sheet_list.Count > 0)
                                    {
                                        foreach (var sheet_name in sheet_list)
                                        {
                                            using (FileStream file = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
                                            {
                                                HSSFWorkbook workbook = new HSSFWorkbook(file);
                                                HSSFSheet sheet = (HSSFSheet)workbook.GetSheet(sheet_name);
                                                #region Start Remove above unused rows and columns
                                                var (hasDataInFirstTenRowsAndColumns, rowcnt) = CoreService.CheckDataInFirstTenRowsAndColumns(sheet);

                                                if (hasDataInFirstTenRowsAndColumns && rowcnt > 1)
                                                {
                                                    sheet.ShiftRows(1, rowcnt - 1, 0);
                                                    string outputFilePath = Path.Combine(filePath, strFile);
                                                    using (FileStream outputFile = new FileStream(outputFilePath, FileMode.Create))
                                                    {
                                                        workbook.Write(outputFile);
                                                    }
                                                }
                                                #endregion

                                                #region Start Remove belowe unused rows and columns
                                                int rowCount = sheet.LastRowNum + 1;
                                                int colCount = sheet.GetRow(0).LastCellNum;

                                                int rowsToCheck = Math.Max(rowCount - (rowCount - 1), 1);
                                                bool is_Removed = false;

                                                for (int row = rowCount - 1; row >= rowsToCheck; row--)
                                                {
                                                    bool hasData = false;

                                                    IRow currentRow = sheet.GetRow(row);
                                                    if (currentRow != null)
                                                    {
                                                        for (int col = colCount - 1; col >= colCount - 20; col--)
                                                        {
                                                            ICell cell = currentRow.GetCell(col);
                                                            if (cell != null && !string.IsNullOrEmpty(cell.ToString()))
                                                            {
                                                                hasData = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (!hasData)
                                                    {
                                                        sheet.RemoveRow(currentRow);
                                                        rowCount--;
                                                        is_Removed = true;
                                                    }
                                                }

                                                if (is_Removed)
                                                {
                                                    try
                                                    {
                                                        string outputFilePath2 = Path.Combine(filePath, strFile);
                                                        using (FileStream outputFile = new FileStream(outputFilePath2, FileMode.Create))
                                                        {
                                                            workbook.Write(outputFile);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // Handle exception
                                                    }
                                                }
                                                #endregion

                                                #region Start Update URL
                                                List<string> columnNames = new List<string>();
                                                List<Dictionary<string, object>> rowsData = new List<Dictionary<string, object>>();

                                                int totalColumns = sheet.GetRow(0).LastCellNum;
                                                IRow headerRow = sheet.GetRow(0);

                                                for (int col = 0; col < totalColumns; col++)
                                                {
                                                    var cellValue = headerRow.GetCell(col)?.ToString();
                                                    if (!string.IsNullOrEmpty(cellValue))
                                                    {
                                                        columnNames.Add(cellValue);
                                                    }
                                                }
                                                int _rowCount = sheet.PhysicalNumberOfRows;
                                                for (int row = 1; row < _rowCount; row++)
                                                {
                                                    IRow currentRow = sheet.GetRow(row);
                                                    Dictionary<string, object> rowData = new Dictionary<string, object>();

                                                    for (int col = 0; col < totalColumns; col++)
                                                    {
                                                        string columnName = columnNames[col];
                                                        var cell = currentRow.GetCell(col);
                                                        var cellValue = cell?.ToString();
                                                        string formula = string.Empty;
                                                        if (cell.CellType == CellType.Formula)
                                                        {
                                                            formula = cell?.CellFormula;
                                                        }
                                                        if (!string.IsNullOrEmpty(cell.Hyperlink?.Address))
                                                        {
                                                            string linkUrl = cell.Hyperlink.Address;
                                                            rowData.Add(columnName, linkUrl);
                                                        }
                                                        else if (cell.CellType == CellType.Formula && !string.IsNullOrEmpty(formula))
                                                        {
                                                            var urlMatch = Regex.Match(formula, "\"(.*?)\"");
                                                            if (urlMatch.Success)
                                                            {
                                                                string url = urlMatch.Groups[1].Value;
                                                                string text = Regex.Match(formula, ",\"(.*?)\"").Groups[1].Value;
                                                                bool containsOnlyNumbers = Regex.IsMatch(text, @"^[0-9]+$");
                                                                if (containsOnlyNumbers)
                                                                {
                                                                    rowData.Add(columnName, url + "," + text);
                                                                }
                                                                else
                                                                {
                                                                    rowData.Add(columnName, url);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            rowData.Add(columnName, cellValue);
                                                        }
                                                    }

                                                    rowsData.Add(rowData);
                                                }

                                                for (int colIndex = 0; colIndex < columnNames.Count; colIndex++)
                                                {
                                                    headerRow.GetCell(colIndex).SetCellValue(columnNames[colIndex]);
                                                }

                                                for (int rowIndex = 0; rowIndex < rowsData.Count; rowIndex++)
                                                {
                                                    IRow currentRow = sheet.GetRow(rowIndex + 1);
                                                    for (int colIndex = 0; colIndex < columnNames.Count; colIndex++)
                                                    {
                                                        currentRow.GetCell(colIndex)?.SetCellValue(rowsData[rowIndex][columnNames[colIndex]].ToString());
                                                    }
                                                }

                                                string outputFilePath1 = Path.Combine(filePath, strFile);
                                                using (FileStream outputFile = new FileStream(outputFilePath1, FileMode.Create))
                                                {
                                                    workbook.Write(outputFile);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                else if (fileExt == ".xlsx")
                                {
                                    List<Dictionary<string, object>> rowsData = new List<Dictionary<string, object>>();
                                    if (sheet_list != null && sheet_list.Count > 0)
                                    {
                                        foreach (var sheet_name in sheet_list)
                                        {
                                            using (var package = new ExcelPackage(new FileInfo(fileLocation)))
                                            {
                                                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet_name];
                                                worksheet.Calculate();
                                                #region Start Remove above unused rows and columns
                                                var (hasDataInFirstTenRowsAndColumns, row_count) = CoreService.CheckDataInFirstTenRowsAndColumns(worksheet);
                                                if (hasDataInFirstTenRowsAndColumns && row_count > 1)
                                                {
                                                    worksheet.DeleteRow(1, row_count - 1);
                                                    string outputFilePath = Path.Combine(filePath, strFile);
                                                    package.SaveAs(new FileInfo(outputFilePath));
                                                }
                                                #endregion

                                                #region Start Remove belowe unused rows and columns
                                                var rowCount = worksheet.Dimension.End.Row;
                                                var colCount = worksheet.Dimension.End.Column;

                                                int rowsToCheck = Math.Max(rowCount - (rowCount - 1), 1);
                                                bool is_Removed = false;
                                                for (int row = rowCount; row >= rowsToCheck; row--)
                                                {
                                                    bool hasData = false;

                                                    for (int col = colCount; col > colCount - 20; col--)
                                                    {
                                                        var cell = worksheet.Cells[row, col];

                                                        if (!string.IsNullOrEmpty(cell.Text))
                                                        {
                                                            hasData = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!hasData)
                                                    {
                                                        worksheet.DeleteRow(row);
                                                        rowCount--;
                                                        is_Removed = true;
                                                    }
                                                }
                                                if (is_Removed)
                                                {
                                                    try
                                                    {
                                                        string outputFilePath1 = Path.Combine(filePath, strFile);
                                                        package.SaveAs(new FileInfo(outputFilePath1));
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                #endregion

                                                List<string> columnNames = new List<string>();
                                                if (worksheet != null)
                                                {
                                                    // Get column names
                                                    int totalColumns = worksheet.Dimension.End.Column;
                                                    int headerRow = 1;

                                                    for (int col = 1; col <= totalColumns; col++)
                                                    {
                                                        var cellValue = worksheet.Cells[headerRow, col].Value;
                                                        if (cellValue != null)
                                                        {
                                                            columnNames.Add(cellValue.ToString().Trim());
                                                        }
                                                    }

                                                    // Get row values
                                                    int totalRows = worksheet.Dimension.End.Row;

                                                    for (int row = headerRow + 1; row <= totalRows; row++)
                                                    {
                                                        Dictionary<string, object> rowData = new Dictionary<string, object>();

                                                        for (int col = 1; col <= totalColumns; col++)
                                                        {
                                                            var _cellValue = worksheet.Cells[headerRow, col].Value;
                                                            if (_cellValue != null)
                                                            {
                                                                string columnName = columnNames[col - 1];
                                                                var cell = worksheet.Cells[row, col];
                                                                var cellValue = cell.Value;
                                                                var formula = cell.Formula;

                                                                if (cell.Hyperlink != null)
                                                                {
                                                                    string url = cell.Hyperlink.OriginalString.Replace("%22", "");

                                                                    if (url.Length > 0)
                                                                    {
                                                                        string linkUrl = url;
                                                                        rowData.Add(columnName, linkUrl + ", " + cellValue);
                                                                    }
                                                                    else
                                                                    {
                                                                        rowData.Add(columnName, cellValue);
                                                                    }
                                                                }
                                                                else if (!string.IsNullOrEmpty(formula) && formula.Contains("HYPERLINK"))
                                                                {
                                                                    int urlStartIndex = formula.IndexOf("\"") + 1;
                                                                    int urlEndIndex = formula.IndexOf("\",\"");

                                                                    if (urlStartIndex > 0 && urlEndIndex > urlStartIndex)
                                                                    {
                                                                        string url = formula.Substring(urlStartIndex, urlEndIndex - urlStartIndex);

                                                                        int textStartIndex = urlEndIndex + 3;
                                                                        int textEndIndex = formula.LastIndexOf("\"");

                                                                        if (textStartIndex > 0 && textEndIndex > textStartIndex)
                                                                        {
                                                                            string text = formula.Substring(textStartIndex, textEndIndex - textStartIndex);
                                                                            cell.Value = String.Format("{0},{1}", url, text);
                                                                            rowData.Add(columnName, cell.Value);
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    rowData.Add(columnName, cellValue != null ? cellValue : DBNull.Value);
                                                                }
                                                            }
                                                        }
                                                        rowsData.Add(rowData);
                                                    }
                                                }
                                            }
                                        }
                                        excel_dataTable = CoreService.ConvertToDataTable(rowsData);
                                        goto dataExist;
                                    }
                                }

                                if (fileExt == ".xls")
                                {
                                    var modifiedRows = supplier_column_Mapping.AsEnumerable()
                                                       .Select(row =>
                                                       {
                                                           string originalValue = Convert.ToString(row["Supp_Col_Name"]);
                                                           if (originalValue.Contains("."))
                                                           {
                                                               string modifiedValue = originalValue.Replace(".", "#");
                                                               row.SetField("Supp_Col_Name", modifiedValue);
                                                           }
                                                           return row;
                                                       }).CopyToDataTable();
                                    string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".xls", connString, party_File.Sheet_Name);
                                }
                                else if (fileExt == ".xlsx")
                                {
                                    var modifiedRows = supplier_column_Mapping.AsEnumerable()
                                                       .Select(row =>
                                                       {
                                                           string originalValue = Convert.ToString(row["Supp_Col_Name"]);
                                                           if (originalValue.Contains("."))
                                                           {
                                                               string modifiedValue = originalValue.Replace(".", "#");
                                                               row.SetField("Supp_Col_Name", modifiedValue);
                                                           }
                                                           return row;
                                                       }).CopyToDataTable();
                                    string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".xlsx", connString, party_File.Sheet_Name);
                                }
                                else if (fileExt == ".csv")
                                {
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".csv", fileLocation, "");
                                }

                            dataExist:
                                if (excel_dataTable != null && excel_dataTable.Rows.Count > 0)
                                {
                                    #region Add column to datatable
                                    DataTable dt_stock_data = new DataTable();
                                    dt_stock_data.Columns.Add("SUPPLIER_NO", typeof(string));
                                    dt_stock_data.Columns.Add("CERTIFICATE_NO", typeof(string));
                                    dt_stock_data.Columns.Add("LAB", typeof(string));
                                    dt_stock_data.Columns.Add("SHAPE", typeof(string));
                                    dt_stock_data.Columns.Add("CTS", typeof(string));
                                    dt_stock_data.Columns.Add("BASE_DISC", typeof(string));
                                    dt_stock_data.Columns.Add("BASE_RATE", typeof(string));
                                    dt_stock_data.Columns.Add("BASE_AMOUNT", typeof(string));
                                    dt_stock_data.Columns.Add("COLOR", typeof(string));
                                    dt_stock_data.Columns.Add("CLARITY", typeof(string));
                                    dt_stock_data.Columns.Add("CUT", typeof(string));
                                    dt_stock_data.Columns.Add("POLISH", typeof(string));
                                    dt_stock_data.Columns.Add("SYMM", typeof(string));
                                    dt_stock_data.Columns.Add("FLS_COLOR", typeof(string));
                                    dt_stock_data.Columns.Add("FLS_INTENSITY", typeof(string));
                                    dt_stock_data.Columns.Add("LENGTH", typeof(string));
                                    dt_stock_data.Columns.Add("WIDTH", typeof(string));
                                    dt_stock_data.Columns.Add("DEPTH", typeof(string));
                                    dt_stock_data.Columns.Add("MEASUREMENT", typeof(string));
                                    dt_stock_data.Columns.Add("DEPTH_PER", typeof(string));
                                    dt_stock_data.Columns.Add("TABLE_PER", typeof(string));
                                    dt_stock_data.Columns.Add("CULET", typeof(string));
                                    dt_stock_data.Columns.Add("SHADE", typeof(string));
                                    dt_stock_data.Columns.Add("LUSTER", typeof(string));
                                    dt_stock_data.Columns.Add("MILKY", typeof(string));
                                    dt_stock_data.Columns.Add("BGM", typeof(string));
                                    dt_stock_data.Columns.Add("LOCATION", typeof(string));
                                    dt_stock_data.Columns.Add("STATUS", typeof(string));
                                    dt_stock_data.Columns.Add("TABLE_BLACK", typeof(string));
                                    dt_stock_data.Columns.Add("SIDE_BLACK", typeof(string));
                                    dt_stock_data.Columns.Add("TABLE_WHITE", typeof(string));
                                    dt_stock_data.Columns.Add("SIDE_WHITE", typeof(string));
                                    dt_stock_data.Columns.Add("TABLE_OPEN", typeof(string));
                                    dt_stock_data.Columns.Add("CROWN_OPEN", typeof(string));
                                    dt_stock_data.Columns.Add("PAVILION_OPEN", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_OPEN", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_FROM", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_TO", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_CONDITION", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_TYPE", typeof(string));
                                    dt_stock_data.Columns.Add("LASER_INSCRIPTION", typeof(string));
                                    dt_stock_data.Columns.Add("CERTIFICATE_DATE", typeof(string));
                                    dt_stock_data.Columns.Add("CROWN_ANGLE", typeof(string));
                                    dt_stock_data.Columns.Add("CROWN_HEIGHT", typeof(string));
                                    dt_stock_data.Columns.Add("PAVILION_ANGLE", typeof(string));
                                    dt_stock_data.Columns.Add("PAVILION_HEIGHT", typeof(string));
                                    dt_stock_data.Columns.Add("GIRDLE_PER", typeof(string));
                                    dt_stock_data.Columns.Add("LR_HALF", typeof(string));
                                    dt_stock_data.Columns.Add("STAR_LN", typeof(string));
                                    dt_stock_data.Columns.Add("CERT_TYPE", typeof(string));
                                    dt_stock_data.Columns.Add("FANCY_COLOR", typeof(string));
                                    dt_stock_data.Columns.Add("FANCY_INTENSITY", typeof(string));
                                    dt_stock_data.Columns.Add("FANCY_OVERTONE", typeof(string));
                                    dt_stock_data.Columns.Add("IMAGE_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("Image2", typeof(string));
                                    dt_stock_data.Columns.Add("VIDEO_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("Video2", typeof(string));
                                    dt_stock_data.Columns.Add("CERTIFICATE_LINK", typeof(string));
                                    //dt_stock_data.Columns.Add("DNA_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("DNA", typeof(string));
                                    dt_stock_data.Columns.Add("IMAGE_HEART_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("IMAGE_ARROW_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("H_A_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("CERTIFICATE_TYPE_LINK", typeof(string));
                                    dt_stock_data.Columns.Add("KEY_TO_SYMBOL", typeof(string));
                                    dt_stock_data.Columns.Add("LAB_COMMENTS", typeof(string));
                                    dt_stock_data.Columns.Add("SUPPLIER_COMMENTS", typeof(string));
                                    dt_stock_data.Columns.Add("ORIGIN", typeof(string));
                                    dt_stock_data.Columns.Add("BOW_TIE", typeof(string));
                                    dt_stock_data.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                                    dt_stock_data.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                                    dt_stock_data.Columns.Add("EXTRA_FACET_PAVILION", typeof(string));
                                    dt_stock_data.Columns.Add("INTERNAL_GRAINING", typeof(string));
                                    dt_stock_data.Columns.Add("H_A", typeof(string));
                                    dt_stock_data.Columns.Add("SUPPLIER_DISC", typeof(string));
                                    dt_stock_data.Columns.Add("SUPPLIER_AMOUNT", typeof(string));
                                    dt_stock_data.Columns.Add("OFFER_DISC", typeof(string));
                                    dt_stock_data.Columns.Add("OFFER_VALUE", typeof(string));
                                    dt_stock_data.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                                    dt_stock_data.Columns.Add("MAX_SLAB_BASE_VALUE", typeof(string));
                                    dt_stock_data.Columns.Add("EYE_CLEAN", typeof(string));
                                    dt_stock_data.Columns.Add("GOOD_TYPE", typeof(string));
                                    dt_stock_data.Columns.Add("Short_Code", typeof(string));
                                    #endregion
                                    var mappedRows = excel_dataTable.AsEnumerable()
                                     .Select(row =>
                                     {
                                         var finalRow = dt_stock_data.NewRow();

                                         supplier_column_Mapping.AsEnumerable().ToList().ForEach(suppColRow =>
                                         {
                                             string displayColName = Convert.ToString(suppColRow["Display_Name"]);
                                             string suppColName = Convert.ToString(suppColRow["Supp_Col_Name"]);

                                             if (displayColName != "" && suppColName != "")
                                             {

                                                 if (displayColName != "SHADE")
                                                 {
                                                     finalRow[displayColName] = row[Convert.ToString(suppColRow["Supp_Col_Name"])];
                                                     //if (displayColName == "TABLE_OPEN" || displayColName == "CROWN_OPEN" || displayColName == "PAVILION_OPEN" || displayColName == "GIRDLE_OPEN")
                                                     //{
                                                     //    string foundColumnName = excel_dataTable.Columns.Cast<DataColumn>().FirstOrDefault(column => column.ColumnName == Convert.ToString(suppColRow["Supp_Col_Name"]))?.ColumnName;
                                                     //    if (!string.IsNullOrEmpty(foundColumnName))
                                                     //    {
                                                     //        finalRow[displayColName] = row[Convert.ToString(suppColRow["Supp_Col_Name"])];
                                                     //    }
                                                     //    else
                                                     //    {
                                                     //        finalRow[displayColName] = "";
                                                     //    }

                                                     //}
                                                     //else
                                                     //{
                                                     //    finalRow[displayColName] = row[Convert.ToString(suppColRow["Supp_Col_Name"])];
                                                     //}

                                                 }
                                                 else
                                                 {
                                                     if (suppColName.Contains(","))
                                                     {
                                                         string supp_Col_Name1 = Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",").Length == 3 || Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",").Length == 2 ? Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",")[0] : "";
                                                         string supp_Col_Name2 = Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",").Length == 3 || Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",").Length == 2 ? Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",")[1] : "";
                                                         string supp_Col_Name3 = Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",").Length == 3 ? Convert.ToString(suppColRow["Supp_Col_Name"]).Split(",")[2] : "";
                                                         string shade_Value_1 = !string.IsNullOrEmpty(supp_Col_Name1) ? row[supp_Col_Name1].ToString() : "";
                                                         string shade_Value_2 = !string.IsNullOrEmpty(supp_Col_Name2) ? row[supp_Col_Name2].ToString() : "";
                                                         string shade_Value_3 = !string.IsNullOrEmpty(supp_Col_Name3) ? row[supp_Col_Name3].ToString() : "";

                                                         finalRow[displayColName] = CoreService.ExtractStringWithLargestNumericValue(shade_Value_1, shade_Value_2, shade_Value_3);
                                                     }
                                                     else
                                                     {
                                                         finalRow[displayColName] = row[Convert.ToString(suppColRow["Supp_Col_Name"])];
                                                     }
                                                 }

                                                 if (displayColName == "CTS" || displayColName == "BASE_DISC" || displayColName == "BASE_RATE" ||
                                                     displayColName == "LENGTH" || displayColName == "WIDTH" || displayColName == "DEPTH" ||
                                                     displayColName == "DEPTH_PER" || displayColName == "TABLE_PER" || displayColName == "CROWN_ANGLE" ||
                                                     displayColName == "CROWN_HEIGHT" || displayColName == "PAVILION_ANGLE" ||
                                                     displayColName == "PAVILION_HEIGHT" || displayColName == "GIRDLE_PER" ||
                                                     displayColName == "SUPPLIER_DISC" || displayColName == "SUPPLIER_AMOUNT" ||
                                                     displayColName == "OFFER_DISC" || displayColName == "OFFER_VALUE" ||
                                                     displayColName == "MAX_SLAB_BASE_DISC" || displayColName == "MAX_SLAB_BASE_VALUE")
                                                 {
                                                     finalRow[displayColName] = CoreService.RemoveNonNumericAndDotAndNegativeCharacters(
                                                         Convert.ToString(finalRow[displayColName]));
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "GIRDLE_TO")
                                                 {
                                                     finalRow[displayColName] = !string.IsNullOrEmpty(Convert.ToString(finalRow[displayColName])) ? (Convert.ToString(finalRow[displayColName]).Contains("-") ? (Convert.ToString(finalRow[displayColName]).Split(" - ").Length == 2 ? Convert.ToString(finalRow[displayColName]).Split(" - ")[1] : Convert.ToString(finalRow[displayColName])) : (Convert.ToString(finalRow[displayColName]).ToUpper().Contains(" TO ") ? (Convert.ToString(finalRow[displayColName]).ToUpper().Split(" TO ").Length == 2 ? Convert.ToString(finalRow[displayColName]).ToUpper().Split(" TO ")[1] : Convert.ToString(finalRow[displayColName])) : Convert.ToString(finalRow[displayColName]))) : null;

                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "BASE_AMOUNT")
                                                 {
                                                     var base_amt = !string.IsNullOrEmpty(finalRow[displayColName].ToString()) ? (finalRow[displayColName].ToString().Contains("$") ? Convert.ToDecimal(finalRow[displayColName].ToString().Replace("$", "")) : Convert.ToDecimal(finalRow[displayColName].ToString())) : 0;
                                                     finalRow[displayColName] = base_amt.ToString("0.00");
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "DNA")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), false);
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "IMAGE_LINK")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), false);
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "VIDEO_LINK")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), false);
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "LAB")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), true);
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "CERTIFICATE_NO")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), true);
                                                 }
                                                 else if (!string.IsNullOrEmpty(displayColName) && displayColName == "CERTIFICATE_LINK")
                                                 {
                                                     finalRow[displayColName] = CoreService.GetCertificateNoOrUrl(
                                                        Convert.ToString(finalRow[displayColName]), false);
                                                 }
                                                 //else if (displayColName == "TABLE_OPEN" || displayColName == "CROWN_OPEN" || displayColName == "PAVILION_OPEN" || displayColName == "GIRDLE_OPEN")
                                                 //{
                                                 //    string foundColumnName = excel_dataTable.Columns.Cast<DataColumn>().FirstOrDefault(column => column.ColumnName == Convert.ToString(suppColRow["Supp_Col_Name"]))?.ColumnName;
                                                 //    if (!string.IsNullOrEmpty(foundColumnName))
                                                 //    {
                                                 //        finalRow[displayColName] = string.IsNullOrEmpty(Convert.ToString(finalRow[displayColName]))
                                                 //        ? null : Convert.ToString(finalRow[displayColName]);
                                                 //    }
                                                 //    else
                                                 //    {
                                                 //        finalRow[displayColName] = "";
                                                 //    }
                                                 //}
                                                 else
                                                 {

                                                     finalRow[displayColName] = string.IsNullOrEmpty(Convert.ToString(finalRow[displayColName]))
                                                         ? null : Convert.ToString(finalRow[displayColName]);

                                                 }
                                             }
                                         });

                                         dt_stock_data.Rows.Add(finalRow);
                                         return finalRow;
                                     })
                                     .ToList();
                                    var party_master = await _partyService.Get_Party_Details(party_File.Party_Id ?? 0);
                                    var party_file = await _partyService.Get_Party_File(0, party_File.Party_Id ?? 0);
                                    dt_stock_data.AsEnumerable().ToList().ForEach(stockDataRow =>
                                    {
                                        //Start Center Inclusion AND Black Inclusion
                                        var lakhi_Party_Code = _configuration["Lakhi_Party_Code"];
                                        var table_white = string.Empty;
                                        var table_Black = string.Empty;
                                        var side_White = string.Empty;
                                        var side_Black = string.Empty;
                                        if (party_master != null && party_master.Party_Code == lakhi_Party_Code)
                                        {
                                            (table_white, _) = CoreService.Table_And_Side_White(Convert.ToString(stockDataRow["TABLE_WHITE"]));
                                            (_, side_White) = CoreService.Table_And_Side_White(Convert.ToString(stockDataRow["SIDE_WHITE"]));

                                            (table_Black, _) = CoreService.Table_And_Side_Black(Convert.ToString(stockDataRow["TABLE_BLACK"]));
                                            (_, side_Black) = CoreService.Table_And_Side_Black(Convert.ToString(stockDataRow["SIDE_BLACK"]));
                                        }
                                        else
                                        {
                                            table_white = Convert.ToString(stockDataRow["TABLE_WHITE"]);
                                            table_Black = Convert.ToString(stockDataRow["SIDE_WHITE"]);
                                            side_White = Convert.ToString(stockDataRow["TABLE_BLACK"]);
                                            side_Black = Convert.ToString(stockDataRow["SIDE_BLACK"]);
                                        }
                                        stockDataRow["TABLE_WHITE"] = table_white;
                                        stockDataRow["SIDE_WHITE"] = table_Black;
                                        stockDataRow["TABLE_BLACK"] = side_White;
                                        stockDataRow["SIDE_BLACK"] = side_Black;
                                        stockDataRow["Short_Code"] = party_file.Short_Code;
                                        //END Center Inclusion AND Black Inclusion

                                        // Check if all three columns are currently null or empty
                                        if ((string.IsNullOrEmpty(Convert.ToString(stockDataRow["LENGTH"]))
                                            && string.IsNullOrEmpty(Convert.ToString(stockDataRow["WIDTH"]))
                                            && string.IsNullOrEmpty(Convert.ToString(stockDataRow["DEPTH"]))))
                                        {
                                            string measurementValue = Convert.ToString(stockDataRow["MEASUREMENT"]).Replace("-", "*").Replace("x", "*");

                                            // Call the function and handle possible null values
                                            string lengthValue = CoreService.Split_Supplier_Stock_Measurement(measurementValue, "LENGTH");
                                            string widthValue = CoreService.Split_Supplier_Stock_Measurement(measurementValue, "WIDTH");
                                            string depthValue = CoreService.Split_Supplier_Stock_Measurement(measurementValue, "DEPTH");

                                            // Set the values in the DataRow, handling nulls or empty strings
                                            stockDataRow["LENGTH"] = !string.IsNullOrEmpty(lengthValue) ? lengthValue : stockDataRow["LENGTH"];
                                            stockDataRow["WIDTH"] = !string.IsNullOrEmpty(widthValue) ? widthValue : stockDataRow["WIDTH"];
                                            stockDataRow["DEPTH"] = !string.IsNullOrEmpty(depthValue) ? depthValue : stockDataRow["DEPTH"];
                                        }
                                    });

                                    Stock_Data_Master_Schedular stock_Data_Master_Schedular = new Stock_Data_Master_Schedular();
                                    stock_Data_Master_Schedular.Stock_Data_Id = 0;
                                    stock_Data_Master_Schedular.Supplier_Id = party_File.Party_Id;
                                    stock_Data_Master_Schedular.Upload_Method = "FILE";
                                    stock_Data_Master_Schedular.Upload_Type = "O";

                                    var (message, stock_Data_Id) = await _supplierService.Stock_Data_Custom_Insert_Update(stock_Data_Master_Schedular);

                                    if (message == "success" && stock_Data_Id > 0)
                                    {
                                        var response = await _supplierService.Stock_Data_Detail_Insert_Update(dt_stock_data, stock_Data_Id);
                                        if (response > 0)
                                        {
                                            if (stock_Data_Master_Schedular.Upload_Type == "O")
                                            {
                                                await _supplierService.Supplier_Stock_Manual_File_Insert_Update((int)stock_Data_Master_Schedular.Supplier_Id, stock_Data_Id, party_File.Is_Overwrite ?? false);
                                            }
                                        }
                                        return Ok(new
                                        {
                                            statusCode = HttpStatusCode.OK,
                                            message = "File uploaded successfully.",
                                            Party_Name = party_name,
                                            Stock_Data_Id = stock_Data_Id
                                        });
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        Supplier_Id = party_File.Party_Id,
                        Party_Name = party_name,
                        message = "No column mapping found!"
                    });
                    //}
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                string message = string.Empty;
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Manual_Upload", ex.StackTrace);
                if (ex.Message.Contains("An item with the same key has already been added"))
                {
                    message = "Some column name is missing";
                }
                else
                {
                    message = "Either column or value mapping is wrong";
                }

                return Ok(new
                {
                    Supplier_Id = party_File.Party_Id,
                    Party_Name = party_name,
                    message = message,

                });
            }
        }

        [HttpGet]
        [Route("check_api_ftp_exist")]
        [Authorize]
        public async Task<IActionResult> Check_Api_Ftp_Exist(int party_Id, bool is_Continue)
        {
            try
            {
                if (!is_Continue)
                {
                    var party_Api = await _partyService.Get_Party_API(0, party_Id);
                    var party_FTP = await _partyService.Get_Party_FTP(0, party_Id);
                    if (party_Api != null && party_Api.API_Status == true && party_Api.Lab == true)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            api_Active = true,
                            message = "API is active for this supplier, Do you want to continue ?"
                        });
                    }
                    else if (party_FTP != null && party_FTP.Ftp_Status == true && party_FTP.Lab == true)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            api_Active = true,
                            message = "FTP is active for this supplier, Do you want to continue ?"
                        });
                    }
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Continue"
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "check_api_ftp_exist", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Supplier Stock Error Log
        [HttpGet]
        [Route("supplier_stock_error_log")]
        [Authorize]
        public async Task<IActionResult> Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Last_Entry)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_Error_Log(supplier_Ids, upload_Type, from_Date, from_Time, to_Date, to_Time, is_Last_Entry);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Supplier_Stock_Error_Log", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("supplier_stock_error_log_detail")]
        [Authorize]
        public async Task<IActionResult> Supplier_Stock_Error_Log_Detail(string supplier_Ids, string stock_Data_Ids, string upload_Type)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_Error_Log_Detail(supplier_Ids, stock_Data_Ids, upload_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Supplier_Stock_Error_Log_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("supplier_stock_file_error_error_log")]
        [Authorize]
        public async Task<IActionResult> Supplier_Stock_File_Error_Error_Log(int supplier_Id, int stock_Data_Id)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_File_Error_Log(supplier_Id, stock_Data_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Supplier_Stock_File_Error_Error_Log", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("supplier_stock_file_error_error_log_detail")]
        [Authorize]
        public async Task<IActionResult> Supplier_Stock_File_Error_Error_Log_Detail(int supplier_Id, string upload_Type)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_File_Error_Log_Detail(supplier_Id, upload_Type);
                var party = await _partyService.Get_Party_Details(supplier_Id);
                if (result != null && result.Rows.Count > 0)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierErrorLog");
                    if (!(Directory.Exists(folderPath)))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string strFile = party.Party_Name + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + ".xlsx";

                    using var package = new ExcelPackage();
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A2"].LoadFromDataTable(result, true);

                    int rowEnd = worksheet.Dimension.End.Row;

                    string cellAdress = worksheet.Cells[1, 1, rowEnd, 100].Address;
                    EpExcelExport.removingGreenTagWarning(worksheet, cellAdress);

                    worksheet.Cells[2, 1, 2, result.Columns.Count].AutoFilter = true;

                    worksheet.Cells[2, 1, 2, result.Columns.Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, result.Columns.Count].Style.Font.Bold = true;

                    worksheet.Cells[worksheet.Dimension.Address].Style.Font.Size = 10;
                    worksheet.Cells[worksheet.Dimension.Address].Offset(1, 0, rowEnd - 1, 100).Style.Font.Size = 9;

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 3].Formula = $"SUBTOTAL(103, F3:F{rowEnd})";
                    worksheet.Cells[1, 3].Style.Numberformat.Format = "#,##";

                    worksheet.Cells[1, 6].Formula = $"SUBTOTAL(109,F3:F{rowEnd})";

                    byte[] byteArray = package.GetAsByteArray();
                    string filePath = Path.Combine(folderPath, strFile);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(byteArray, 0, byteArray.Length);
                    }

                    var file_url = _configuration["BaseUrl"] + CoreCommonFilePath.SupplierErrorLogFilesPath + strFile;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = file_url
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Supplier_Stock_File_Error_Error_Log", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("supplier_stock_excel_error_log_detail")]
        [Authorize]
        public async Task<IActionResult> Supplier_Stock_Excel_Error_Log_Detail(Stock_Data_Master_Excel stock_Data_Master_Excel)
        {
            try
            {
                if (stock_Data_Master_Excel.Supplier_Stock_Data != null)
                {
                    IList<Supplier_Stock_Excel> Supplier_Stock_Excel = JsonConvert.DeserializeObject<IList<Supplier_Stock_Excel>>(stock_Data_Master_Excel.Supplier_Stock_Data.ToString());

                    DataTable dataTable = new DataTable();
                    dataTable = CoreService.ToDataTable(Supplier_Stock_Excel.ToArray());

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/SupplierErrorLog");
                    if (!(Directory.Exists(folderPath)))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string strFile = "Error_Log_File_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + ".xlsx";

                    using var package = new ExcelPackage();
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A2"].LoadFromDataTable(dataTable, true);

                    int rowEnd = worksheet.Dimension.End.Row;

                    worksheet.Cells[2, 1, 2, dataTable.Columns.Count].AutoFilter = true;

                    worksheet.Cells[2, 1, 2, dataTable.Columns.Count].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 1, dataTable.Columns.Count].Style.Font.Bold = true;

                    worksheet.Cells[worksheet.Dimension.Address].Style.Font.Size = 10;
                    worksheet.Cells[worksheet.Dimension.Address].Offset(1, 0, rowEnd - 1, 100).Style.Font.Size = 9;

                    worksheet.Cells[1, 1].Value = "Total";
                    worksheet.Cells[1, 3].Formula = $"SUBTOTAL(103, F3:F{rowEnd})";
                    worksheet.Cells[1, 3].Style.Numberformat.Format = "#,##";

                    worksheet.Cells[1, 6].Formula = $"SUBTOTAL(109,F3:F{rowEnd})";

                    string cellAdress = worksheet.Cells[1, 1, rowEnd, 100].Address;
                    EpExcelExport.removingGreenTagWarning(worksheet, cellAdress);

                    byte[] byteArray = package.GetAsByteArray();
                    string filePath = Path.Combine(folderPath, strFile);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(byteArray, 0, byteArray.Length);
                    }

                    var file_url = _configuration["BaseUrl"] + CoreCommonFilePath.SupplierErrorLogFilesPath + strFile;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = file_url
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Supplier_Stock_Excel_Error_Log_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        #endregion

        #region Report
        [HttpPost]
        [Route("create_update_report_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Report_Master(Report_Master report_Master)
        {
            try
            {

                var (message, Report_Id) = await _supplierService.Create_Update_Report_Master(report_Master);
                if (message == "success" && Report_Id > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ReportMasterCreated,
                        data = new { report_Id = Report_Id }
                    });
                }
                else if (message == "exist")
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReportNameExist,
                    });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Report_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_report_detail")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Report_Detail(IList<Report_Detail> report_Detail)
        {
            try
            {
                if (report_Detail != null && report_Detail.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Rm_Id", typeof(int));
                    dataTable.Columns.Add("Column_Type", typeof(string));
                    dataTable.Columns.Add("Col_Id", typeof(int));
                    dataTable.Columns.Add("Order_By", typeof(string));
                    dataTable.Columns.Add("Short_No", typeof(int));
                    dataTable.Columns.Add("Display_Type", typeof(string));
                    dataTable.Columns.Add("Width", typeof(int));
                    dataTable.Columns.Add("Column_Format", typeof(string));
                    dataTable.Columns.Add("Alignment", typeof(string));
                    dataTable.Columns.Add("Fore_Colour", typeof(string));
                    dataTable.Columns.Add("Back_Colour", typeof(string));
                    dataTable.Columns.Add("IsBold", typeof(bool));
                    foreach (var item in report_Detail)
                    {
                        dataTable.Rows.Add(item.Id, item.Rm_Id, item.Column_Type, item.Col_Id, item.Order_By, item.Short_No, item.Display_Type, item.Width, item.Column_Format, item.Alignment, item.Fore_Colour, item.Back_Colour, item.IsBold);
                    }
                    var result = await _supplierService.Create_Update_Report_Detail(dataTable);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ReportDetailCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Report_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_name")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Name(int id, int user_Id)
        {
            try
            {
                var result = await _supplierService.Get_Report_Name(id, user_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Name", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Detail(int id)
        {
            try
            {
                var result = await _supplierService.Get_Report_Detail(id);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_detail_filter_parameter")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Detail_Filter_Parameter(int id)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _supplierService.Get_Report_Detail_Filter_Parameter(id, user_Id ?? 0);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_users_role")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Users_Role(int id, int user_Id, string user_Type)
        {
            try
            {
                var result = await _supplierService.Get_Report_Users_Role(id, user_Id, user_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Users_Role", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_report_user_role")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Report_User_Role(Report_Users_Role_Model report_User_Roles)
        {
            try
            {
                if (report_User_Roles != null)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Rd_Id", typeof(int));
                    dataTable.Columns.Add("User_Id", typeof(int));
                    dataTable.Columns.Add("Display_Type", typeof(string));
                    dataTable.Columns.Add("Order_By", typeof(string));
                    dataTable.Columns.Add("Short_No", typeof(int));
                    dataTable.Columns.Add("Width", typeof(int));
                    dataTable.Columns.Add("Column_Format", typeof(string));
                    dataTable.Columns.Add("Alignment", typeof(string));
                    dataTable.Columns.Add("Fore_Colour", typeof(string));
                    dataTable.Columns.Add("Back_Colour", typeof(string));
                    dataTable.Columns.Add("IsBold", typeof(bool));

                    var UserIds = report_User_Roles.User_Ids.Split(",");

                    if (UserIds != null)
                    {
                        foreach (var item in UserIds)
                        {
                            foreach (var item1 in report_User_Roles.Report_Roles)
                            {
                                dataTable.Rows.Add(item1.Rd_Id, item, item1.Display_Type, item1.Order_By, item1.Short_No, item1.Width, item1.Column_Format, item1.Alignment, item1.Fore_Colour, item1.Back_Colour, item1.IsBold);
                            }
                        }
                        var result = await _supplierService.Create_Update_Report_User_Role(dataTable);
                        if (result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ReportRoles,
                            });
                        }
                    }

                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Report_User_Role", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_column_format")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Column_Format(int user_Id, int report_Id, string format_Type)
        {
            try
            {
                var result = await _supplierService.Get_Report_Column_Format(user_Id, report_Id, format_Type);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Column_Format", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get_report_search")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Search(Report_Filter report_Filter)
        {
            try
            {
                var (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseAmtr, totalBaseDiscr, totalOfferAmtr, totalOfferDiscr, dt_stock) = await _supplierService.Get_Report_Search(report_Filter.id, report_Filter.Report_Filter_Parameter, report_Filter.iPgNo ?? 0, report_Filter.iPgSize ?? 0, report_Filter.iSort, report_Filter.Is_Selected_Supp_Stock_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        total_Records = totalRecordr,
                        total_Cts = totalCtsr,
                        total_Amt = totalAmtr,
                        total_Disc = totalDiscr,
                        total_Base_Amt = totalBaseAmtr,
                        total_Base_Disc = totalBaseDiscr,
                        total_Offer_Amt = totalOfferAmtr,
                        total_Offer_Disc = totalOfferDiscr,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Search", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get_lab_report_search")]
        [Authorize]
        public async Task<IActionResult> Get_Lab_Report_Search(Report_Lab_Filter report_Lab_Filter)
        {
            try
            {
                string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                        CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                        POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                        LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                        IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                        DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                        CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                        RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                        OFFER_DISC = string.Empty, OFFER_VALUE = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                        PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                        BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                        TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                        GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                        KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                        FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                        STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                        SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty,
                        BID_SUPP_STOCK_ID = string.Empty, BID_BUYER_DISC = string.Empty, BID_BUYER_AMOUNT = string.Empty,
                        LUSTER = string.Empty, SHADE = string.Empty, MILKY = string.Empty, GIRDLE_FROM = string.Empty, GIRDLE_TO = string.Empty, GIRDLE_CONDITION = string.Empty, ORIGIN = string.Empty,
                        EXTRA_FACET_TABLE = string.Empty, EXTRA_FACET_CROWN = string.Empty, EXTRA_FACET_PAV = string.Empty,
                        INTERNAL_GRAINING = string.Empty, H_A = string.Empty, SUPPLIER_COMMENTS = string.Empty;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("SUPPLIER", typeof(string));
                dataTable.Columns.Add("CUSTOMER", typeof(string));
                dataTable.Columns.Add("SHAPE", typeof(string));
                dataTable.Columns.Add("CTS", typeof(string));
                dataTable.Columns.Add("COLOR", typeof(string));
                dataTable.Columns.Add("CLARITY", typeof(string));
                dataTable.Columns.Add("CUT", typeof(string));
                dataTable.Columns.Add("POLISH", typeof(string));
                dataTable.Columns.Add("SYMM", typeof(string));
                dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
                dataTable.Columns.Add("BGM", typeof(string));
                dataTable.Columns.Add("LAB", typeof(string));
                dataTable.Columns.Add("GOOD_TYPE", typeof(string));
                dataTable.Columns.Add("LOCATION", typeof(string));
                dataTable.Columns.Add("STATUS", typeof(string));
                dataTable.Columns.Add("IMAGE_LINK", typeof(string));
                dataTable.Columns.Add("VIDEO_LINK", typeof(string));
                dataTable.Columns.Add("LENGTH", typeof(string));
                dataTable.Columns.Add("WIDTH", typeof(string));
                dataTable.Columns.Add("DEPTH", typeof(string));
                dataTable.Columns.Add("TABLE_PER", typeof(string));
                dataTable.Columns.Add("DEPTH_PER", typeof(string));
                dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
                dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
                dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
                dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
                dataTable.Columns.Add("GIRDLE_PER", typeof(string));
                dataTable.Columns.Add("RAP_RATE", typeof(string));
                dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
                dataTable.Columns.Add("COST_DISC", typeof(string));
                dataTable.Columns.Add("COST_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("BASE_DISC", typeof(string));
                dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
                dataTable.Columns.Add("TABLE_BLACK", typeof(string));
                dataTable.Columns.Add("TABLE_WHITE", typeof(string));
                dataTable.Columns.Add("SIDE_BLACK", typeof(string));
                dataTable.Columns.Add("SIDE_WHITE", typeof(string));
                dataTable.Columns.Add("TABLE_OPEN", typeof(string));
                dataTable.Columns.Add("CROWN_OPEN", typeof(string));
                dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
                dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
                dataTable.Columns.Add("CULET", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                dataTable.Columns.Add("FANCY_COLOR", typeof(string));
                dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
                dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
                dataTable.Columns.Add("POINTER", typeof(string));
                dataTable.Columns.Add("SUB_POINTER", typeof(string));
                dataTable.Columns.Add("STAR_LN", typeof(string));
                dataTable.Columns.Add("LR_HALF", typeof(string));
                dataTable.Columns.Add("CERT_TYPE", typeof(string));
                dataTable.Columns.Add("COST_RATE", typeof(string));
                dataTable.Columns.Add("RATIO", typeof(string));
                dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
                dataTable.Columns.Add("SIGN", typeof(string));
                dataTable.Columns.Add("DISC_VALUE", typeof(string));
                dataTable.Columns.Add("DISC_VALUE1", typeof(string));
                dataTable.Columns.Add("DISC_VALUE2", typeof(string));
                dataTable.Columns.Add("DISC_VALUE3", typeof(string));
                dataTable.Columns.Add("BASE_TYPE", typeof(string));
                dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                dataTable.Columns.Add("LUSTER", typeof(string));
                dataTable.Columns.Add("SHADE", typeof(string));
                dataTable.Columns.Add("MILKY", typeof(string));
                dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
                dataTable.Columns.Add("GIRDLE_TO", typeof(string));
                dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
                dataTable.Columns.Add("ORIGIN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
                dataTable.Columns.Add("H_A", typeof(string));
                dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));

                if (report_Lab_Filter.Report_Filter_Parameter_List != null && report_Lab_Filter.Report_Filter_Parameter_List.Count > 0)
                {
                    foreach (var item in report_Lab_Filter.Report_Filter_Parameter_List)
                    {
                        STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER" || x.Column_Name == "COMPANY").Select(x => x.Category_Value).FirstOrDefault();
                        CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Value).FirstOrDefault();
                        SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();
                        CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                        COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Value).FirstOrDefault();
                        CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Value).FirstOrDefault();
                        POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Value).FirstOrDefault();
                        SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Value).FirstOrDefault();
                        FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Value).FirstOrDefault();
                        LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Value).FirstOrDefault();
                        GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Value).FirstOrDefault();
                        STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Value).FirstOrDefault();
                        IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                        VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                        LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                        WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_RATE = item.Where(x => x.Column_Name == "RAP RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_AMOUNT = item.Where(x => x.Column_Name == "RAP AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        COST_DISC = item.Where(x => x.Column_Name == "COST DISC").Select(x => x.Category_Value).FirstOrDefault();
                        COST_AMOUNT = item.Where(x => x.Column_Name == "COST AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_VALUE = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_DISC = item.Where(x => x.Column_Name == "BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_AMOUNT = item.Where(x => x.Column_Name == "BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_DISC = item.Where(x => x.Column_Name == "MAX SLAB BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_AMOUNT = item.Where(x => x.Column_Name == "MAX SLAB BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_PRICE_PER_CTS = item.Where(x => x.Column_Name == "MAX SLAB PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_DISC = item.Where(x => x.Column_Name == "BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_AMOUNT = item.Where(x => x.Column_Name == "BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CULET = item.Where(x => x.Column_Name == "CULET").Select(x => x.Category_Value).FirstOrDefault();
                        KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                        KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                        LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                        LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                        FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Value).FirstOrDefault();
                        POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        SUB_POINTER = item.Where(x => x.Column_Name == "SUB POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Value).FirstOrDefault();
                        LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Value).FirstOrDefault();
                        CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RATIO = item.Where(x => x.Column_Name == "RATIO").Select(x => x.Category_Value).FirstOrDefault();
                        DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_TYPE = item.Where(x => x.Column_Name == "BASE TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_SUPP_STOCK_ID = item.Where(x => x.Column_Name == "BID SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_DISC = item.Where(x => x.Column_Name == "BID BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_AMOUNT = item.Where(x => x.Column_Name == "BID BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        LUSTER = item.Where(x => x.Column_Name == "LUSTER").Select(x => x.Category_Value).FirstOrDefault();
                        SHADE = item.Where(x => x.Column_Name == "SHADE").Select(x => x.Category_Value).FirstOrDefault();
                        MILKY = item.Where(x => x.Column_Name == "MILKY").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_FROM = item.Where(x => x.Column_Name == "GIRDLE_FROM").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_TO = item.Where(x => x.Column_Name == "GIRDLE_TO").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_CONDITION = item.Where(x => x.Column_Name == "GIRDLE_CONDITION").Select(x => x.Category_Value).FirstOrDefault();
                        ORIGIN = item.Where(x => x.Column_Name == "ORIGIN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_TABLE = item.Where(x => x.Column_Name == "EXTRA_FACET_TABLE").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_CROWN = item.Where(x => x.Column_Name == "EXTRA_FACET_CROWN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_PAV = item.Where(x => x.Column_Name == "EXTRA_FACET_PAV").Select(x => x.Category_Value).FirstOrDefault();
                        INTERNAL_GRAINING = item.Where(x => x.Column_Name == "INTERNAL_GRAINING").Select(x => x.Category_Value).FirstOrDefault();
                        H_A = item.Where(x => x.Column_Name == "H_A").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER_COMMENTS = item.Where(x => x.Column_Name == "SUPPLIER_COMMENTS").Select(x => x.Category_Value).FirstOrDefault();

                        dataTable.Rows.Add(!string.IsNullOrEmpty(STOCK_ID) ? STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER) ? SUPPLIER : DBNull.Value,
                            !string.IsNullOrEmpty(CUSTOMER) ? CUSTOMER : DBNull.Value,
                            !string.IsNullOrEmpty(SHAPE) ? SHAPE : DBNull.Value,
                            !string.IsNullOrEmpty(CTS) ? CTS : DBNull.Value,
                            !string.IsNullOrEmpty(COLOR) ? COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(CLARITY) ? CLARITY : DBNull.Value,
                            !string.IsNullOrEmpty(CUT) ? CUT : DBNull.Value,
                            !string.IsNullOrEmpty(POLISH) ? POLISH : DBNull.Value,
                            !string.IsNullOrEmpty(SYMM) ? SYMM : DBNull.Value,
                            !string.IsNullOrEmpty(FLS_INTENSITY) ? FLS_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(BGM) ? BGM : DBNull.Value,
                            !string.IsNullOrEmpty(LAB) ? LAB : DBNull.Value,
                            !string.IsNullOrEmpty(GOOD_TYPE) ? GOOD_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(LOCATION) ? LOCATION : DBNull.Value,
                            !string.IsNullOrEmpty(STATUS) ? STATUS : DBNull.Value,
                            !string.IsNullOrEmpty(IMAGE_LINK) ? IMAGE_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(VIDEO_LINK) ? VIDEO_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(LENGTH) ? LENGTH : DBNull.Value,
                            !string.IsNullOrEmpty(WIDTH) ? WIDTH : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH) ? DEPTH : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_PER) ? TABLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH_PER) ? DEPTH_PER : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_ANGLE) ? CROWN_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_HEIGHT) ? CROWN_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_ANGLE) ? PAVILION_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_HEIGHT) ? PAVILION_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_PER) ? GIRDLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_RATE) ? RAP_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_AMOUNT) ? RAP_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(COST_DISC) ? COST_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(COST_AMOUNT) ? COST_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_DISC) ? OFFER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_VALUE) ? OFFER_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_DISC) ? BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_AMOUNT) ? BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(PRICE_PER_CTS) ? PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_DISC) ? MAX_SLAB_BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_AMOUNT) ? MAX_SLAB_BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_PRICE_PER_CTS) ? MAX_SLAB_PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_DISC) ? BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_AMOUNT) ? BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_BLACK) ? TABLE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_WHITE) ? TABLE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_BLACK) ? SIDE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_WHITE) ? SIDE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_OPEN) ? TABLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_OPEN) ? CROWN_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_OPEN) ? PAVILION_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_OPEN) ? GIRDLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CULET) ? CULET : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? KEY_TO_SYMBOL_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? KEY_TO_SYMBOL_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? LAB_COMMENTS_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? LAB_COMMENTS_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_COLOR) ? FANCY_COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_INTENSITY) ? FANCY_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_OVERTONE) ? FANCY_OVERTONE : DBNull.Value,
                            !string.IsNullOrEmpty(POINTER) ? POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(SUB_POINTER) ? SUB_POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(STAR_LN) ? STAR_LN : DBNull.Value,
                            !string.IsNullOrEmpty(LR_HALF) ? LR_HALF : DBNull.Value,
                            !string.IsNullOrEmpty(CERT_TYPE) ? CERT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(COST_RATE) ? COST_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RATIO) ? RATIO : DBNull.Value,
                            !string.IsNullOrEmpty(DISCOUNT_TYPE) ? DISCOUNT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SIGN) ? SIGN : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE) ? DISC_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE1) ? DISC_VALUE1 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE2) ? DISC_VALUE2 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE3) ? DISC_VALUE3 : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_TYPE) ? BASE_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SUPP_STOCK_ID) ? SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_SUPP_STOCK_ID) ? BID_SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_DISC) ? BID_BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_AMOUNT) ? BID_BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(LUSTER) ? LUSTER : DBNull.Value,
                            !string.IsNullOrEmpty(SHADE) ? SHADE : DBNull.Value,
                            !string.IsNullOrEmpty(MILKY) ? MILKY : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_FROM) ? GIRDLE_FROM : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_TO) ? GIRDLE_TO : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_CONDITION) ? GIRDLE_CONDITION : DBNull.Value,
                            !string.IsNullOrEmpty(ORIGIN) ? ORIGIN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_TABLE) ? EXTRA_FACET_TABLE : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_CROWN) ? EXTRA_FACET_CROWN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_PAV) ? EXTRA_FACET_PAV : DBNull.Value,
                            !string.IsNullOrEmpty(INTERNAL_GRAINING) ? INTERNAL_GRAINING : DBNull.Value,
                            !string.IsNullOrEmpty(H_A) ? H_A : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER_COMMENTS) ? SUPPLIER_COMMENTS : DBNull.Value);
                    }
                }
                else
                {
                    DataRow newRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        newRow[i] = DBNull.Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseDisc, totalBaseAmt, totalOfferDisc, totalOfferAmt, totalMaxSlabDisc, totalMaxSlabAmt) = await _supplierService.Get_Lab_Search_Report_Search(dataTable, report_Lab_Filter.iPgNo ?? 0, report_Lab_Filter.iPgSize ?? 0, report_Lab_Filter.iSort, user_Id, report_Lab_Filter.Is_Selected_Supp_Stock_Id);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        total_Records = totalRecordr,
                        total_Cts = totalCtsr,
                        total_Amt = totalAmtr,
                        total_Disc = totalDiscr,
                        total_Base_Disc = totalBaseDisc,
                        total_Base_Amt = totalBaseAmt,
                        total_Offer_Disc = totalOfferDisc,
                        total_Offer_Amt = totalOfferAmt,
                        total_MaxSlab_Disc = totalMaxSlabDisc,
                        total_MaxSlab_Amt = totalMaxSlabAmt,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Lab_Report_Search", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("get_lab_distinct_report_search")]
        [Authorize]
        public async Task<IActionResult> Get_Lab_Distinct_Report_Search(Report_Lab_Filter report_Lab_Filter)
        {
            try
            {
                string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                        CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                        POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                        LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                        IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                        DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                        CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                        RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                        OFFER_DISC = string.Empty, OFFER_VALUE = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                        PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                        BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                        TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                        GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                        KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                        FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                        STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                        SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty,
                        BID_SUPP_STOCK_ID = string.Empty, BID_BUYER_DISC = string.Empty, BID_BUYER_AMOUNT = string.Empty,
                        LUSTER = string.Empty, SHADE = string.Empty, MILKY = string.Empty, GIRDLE_FROM = string.Empty, GIRDLE_TO = string.Empty, GIRDLE_CONDITION = string.Empty, ORIGIN = string.Empty,
                        EXTRA_FACET_TABLE = string.Empty, EXTRA_FACET_CROWN = string.Empty, EXTRA_FACET_PAV = string.Empty,
                        INTERNAL_GRAINING = string.Empty, H_A = string.Empty, SUPPLIER_COMMENTS = string.Empty;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("SUPPLIER", typeof(string));
                dataTable.Columns.Add("CUSTOMER", typeof(string));
                dataTable.Columns.Add("SHAPE", typeof(string));
                dataTable.Columns.Add("CTS", typeof(string));
                dataTable.Columns.Add("COLOR", typeof(string));
                dataTable.Columns.Add("CLARITY", typeof(string));
                dataTable.Columns.Add("CUT", typeof(string));
                dataTable.Columns.Add("POLISH", typeof(string));
                dataTable.Columns.Add("SYMM", typeof(string));
                dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
                dataTable.Columns.Add("BGM", typeof(string));
                dataTable.Columns.Add("LAB", typeof(string));
                dataTable.Columns.Add("GOOD_TYPE", typeof(string));
                dataTable.Columns.Add("LOCATION", typeof(string));
                dataTable.Columns.Add("STATUS", typeof(string));
                dataTable.Columns.Add("IMAGE_LINK", typeof(string));
                dataTable.Columns.Add("VIDEO_LINK", typeof(string));
                dataTable.Columns.Add("LENGTH", typeof(string));
                dataTable.Columns.Add("WIDTH", typeof(string));
                dataTable.Columns.Add("DEPTH", typeof(string));
                dataTable.Columns.Add("TABLE_PER", typeof(string));
                dataTable.Columns.Add("DEPTH_PER", typeof(string));
                dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
                dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
                dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
                dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
                dataTable.Columns.Add("GIRDLE_PER", typeof(string));
                dataTable.Columns.Add("RAP_RATE", typeof(string));
                dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
                dataTable.Columns.Add("COST_DISC", typeof(string));
                dataTable.Columns.Add("COST_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("BASE_DISC", typeof(string));
                dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
                dataTable.Columns.Add("TABLE_BLACK", typeof(string));
                dataTable.Columns.Add("TABLE_WHITE", typeof(string));
                dataTable.Columns.Add("SIDE_BLACK", typeof(string));
                dataTable.Columns.Add("SIDE_WHITE", typeof(string));
                dataTable.Columns.Add("TABLE_OPEN", typeof(string));
                dataTable.Columns.Add("CROWN_OPEN", typeof(string));
                dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
                dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
                dataTable.Columns.Add("CULET", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                dataTable.Columns.Add("FANCY_COLOR", typeof(string));
                dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
                dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
                dataTable.Columns.Add("POINTER", typeof(string));
                dataTable.Columns.Add("SUB_POINTER", typeof(string));
                dataTable.Columns.Add("STAR_LN", typeof(string));
                dataTable.Columns.Add("LR_HALF", typeof(string));
                dataTable.Columns.Add("CERT_TYPE", typeof(string));
                dataTable.Columns.Add("COST_RATE", typeof(string));
                dataTable.Columns.Add("RATIO", typeof(string));
                dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
                dataTable.Columns.Add("SIGN", typeof(string));
                dataTable.Columns.Add("DISC_VALUE", typeof(string));
                dataTable.Columns.Add("DISC_VALUE1", typeof(string));
                dataTable.Columns.Add("DISC_VALUE2", typeof(string));
                dataTable.Columns.Add("DISC_VALUE3", typeof(string));
                dataTable.Columns.Add("BASE_TYPE", typeof(string));
                dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                dataTable.Columns.Add("LUSTER", typeof(string));
                dataTable.Columns.Add("SHADE", typeof(string));
                dataTable.Columns.Add("MILKY", typeof(string));
                dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
                dataTable.Columns.Add("GIRDLE_TO", typeof(string));
                dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
                dataTable.Columns.Add("ORIGIN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
                dataTable.Columns.Add("H_A", typeof(string));
                dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));

                if (report_Lab_Filter.Report_Filter_Parameter_List != null && report_Lab_Filter.Report_Filter_Parameter_List.Count > 0)
                {
                    foreach (var item in report_Lab_Filter.Report_Filter_Parameter_List)
                    {
                        STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER" || x.Column_Name == "COMPANY").Select(x => x.Category_Value).FirstOrDefault();
                        CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Value).FirstOrDefault();
                        SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();
                        CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                        COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Value).FirstOrDefault();
                        CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Value).FirstOrDefault();
                        POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Value).FirstOrDefault();
                        SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Value).FirstOrDefault();
                        FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Value).FirstOrDefault();
                        LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Value).FirstOrDefault();
                        GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Value).FirstOrDefault();
                        STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Value).FirstOrDefault();
                        IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                        VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                        LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                        WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_RATE = item.Where(x => x.Column_Name == "RAP RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_AMOUNT = item.Where(x => x.Column_Name == "RAP AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        COST_DISC = item.Where(x => x.Column_Name == "COST DISC").Select(x => x.Category_Value).FirstOrDefault();
                        COST_AMOUNT = item.Where(x => x.Column_Name == "COST AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_VALUE = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_DISC = item.Where(x => x.Column_Name == "BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_AMOUNT = item.Where(x => x.Column_Name == "BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_DISC = item.Where(x => x.Column_Name == "MAX SLAB BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_AMOUNT = item.Where(x => x.Column_Name == "MAX SLAB BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_PRICE_PER_CTS = item.Where(x => x.Column_Name == "MAX SLAB PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_DISC = item.Where(x => x.Column_Name == "BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_AMOUNT = item.Where(x => x.Column_Name == "BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CULET = item.Where(x => x.Column_Name == "CULET").Select(x => x.Category_Value).FirstOrDefault();
                        KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                        KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                        LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                        LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                        FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Value).FirstOrDefault();
                        POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        SUB_POINTER = item.Where(x => x.Column_Name == "SUB POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Value).FirstOrDefault();
                        LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Value).FirstOrDefault();
                        CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RATIO = item.Where(x => x.Column_Name == "RATIO").Select(x => x.Category_Value).FirstOrDefault();
                        DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_TYPE = item.Where(x => x.Column_Name == "BASE TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_SUPP_STOCK_ID = item.Where(x => x.Column_Name == "BID SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_DISC = item.Where(x => x.Column_Name == "BID BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_AMOUNT = item.Where(x => x.Column_Name == "BID BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        LUSTER = item.Where(x => x.Column_Name == "LUSTER").Select(x => x.Category_Value).FirstOrDefault();
                        SHADE = item.Where(x => x.Column_Name == "SHADE").Select(x => x.Category_Value).FirstOrDefault();
                        MILKY = item.Where(x => x.Column_Name == "MILKY").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_FROM = item.Where(x => x.Column_Name == "GIRDLE_FROM").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_TO = item.Where(x => x.Column_Name == "GIRDLE_TO").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_CONDITION = item.Where(x => x.Column_Name == "GIRDLE_CONDITION").Select(x => x.Category_Value).FirstOrDefault();
                        ORIGIN = item.Where(x => x.Column_Name == "ORIGIN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_TABLE = item.Where(x => x.Column_Name == "EXTRA_FACET_TABLE").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_CROWN = item.Where(x => x.Column_Name == "EXTRA_FACET_CROWN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_PAV = item.Where(x => x.Column_Name == "EXTRA_FACET_PAV").Select(x => x.Category_Value).FirstOrDefault();
                        INTERNAL_GRAINING = item.Where(x => x.Column_Name == "INTERNAL_GRAINING").Select(x => x.Category_Value).FirstOrDefault();
                        H_A = item.Where(x => x.Column_Name == "H_A").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER_COMMENTS = item.Where(x => x.Column_Name == "SUPPLIER_COMMENTS").Select(x => x.Category_Value).FirstOrDefault();

                        dataTable.Rows.Add(!string.IsNullOrEmpty(STOCK_ID) ? STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER) ? SUPPLIER : DBNull.Value,
                            !string.IsNullOrEmpty(CUSTOMER) ? CUSTOMER : DBNull.Value,
                            !string.IsNullOrEmpty(SHAPE) ? SHAPE : DBNull.Value,
                            !string.IsNullOrEmpty(CTS) ? CTS : DBNull.Value,
                            !string.IsNullOrEmpty(COLOR) ? COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(CLARITY) ? CLARITY : DBNull.Value,
                            !string.IsNullOrEmpty(CUT) ? CUT : DBNull.Value,
                            !string.IsNullOrEmpty(POLISH) ? POLISH : DBNull.Value,
                            !string.IsNullOrEmpty(SYMM) ? SYMM : DBNull.Value,
                            !string.IsNullOrEmpty(FLS_INTENSITY) ? FLS_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(BGM) ? BGM : DBNull.Value,
                            !string.IsNullOrEmpty(LAB) ? LAB : DBNull.Value,
                            !string.IsNullOrEmpty(GOOD_TYPE) ? GOOD_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(LOCATION) ? LOCATION : DBNull.Value,
                            !string.IsNullOrEmpty(STATUS) ? STATUS : DBNull.Value,
                            !string.IsNullOrEmpty(IMAGE_LINK) ? IMAGE_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(VIDEO_LINK) ? VIDEO_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(LENGTH) ? LENGTH : DBNull.Value,
                            !string.IsNullOrEmpty(WIDTH) ? WIDTH : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH) ? DEPTH : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_PER) ? TABLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH_PER) ? DEPTH_PER : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_ANGLE) ? CROWN_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_HEIGHT) ? CROWN_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_ANGLE) ? PAVILION_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_HEIGHT) ? PAVILION_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_PER) ? GIRDLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_RATE) ? RAP_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_AMOUNT) ? RAP_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(COST_DISC) ? COST_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(COST_AMOUNT) ? COST_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_DISC) ? OFFER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_VALUE) ? OFFER_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_DISC) ? BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_AMOUNT) ? BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(PRICE_PER_CTS) ? PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_DISC) ? MAX_SLAB_BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_AMOUNT) ? MAX_SLAB_BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_PRICE_PER_CTS) ? MAX_SLAB_PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_DISC) ? BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_AMOUNT) ? BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_BLACK) ? TABLE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_WHITE) ? TABLE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_BLACK) ? SIDE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_WHITE) ? SIDE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_OPEN) ? TABLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_OPEN) ? CROWN_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_OPEN) ? PAVILION_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_OPEN) ? GIRDLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CULET) ? CULET : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? KEY_TO_SYMBOL_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? KEY_TO_SYMBOL_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? LAB_COMMENTS_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? LAB_COMMENTS_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_COLOR) ? FANCY_COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_INTENSITY) ? FANCY_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_OVERTONE) ? FANCY_OVERTONE : DBNull.Value,
                            !string.IsNullOrEmpty(POINTER) ? POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(SUB_POINTER) ? SUB_POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(STAR_LN) ? STAR_LN : DBNull.Value,
                            !string.IsNullOrEmpty(LR_HALF) ? LR_HALF : DBNull.Value,
                            !string.IsNullOrEmpty(CERT_TYPE) ? CERT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(COST_RATE) ? COST_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RATIO) ? RATIO : DBNull.Value,
                            !string.IsNullOrEmpty(DISCOUNT_TYPE) ? DISCOUNT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SIGN) ? SIGN : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE) ? DISC_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE1) ? DISC_VALUE1 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE2) ? DISC_VALUE2 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE3) ? DISC_VALUE3 : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_TYPE) ? BASE_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SUPP_STOCK_ID) ? SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_SUPP_STOCK_ID) ? BID_SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_DISC) ? BID_BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_AMOUNT) ? BID_BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(LUSTER) ? LUSTER : DBNull.Value,
                            !string.IsNullOrEmpty(SHADE) ? SHADE : DBNull.Value,
                            !string.IsNullOrEmpty(MILKY) ? MILKY : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_FROM) ? GIRDLE_FROM : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_TO) ? GIRDLE_TO : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_CONDITION) ? GIRDLE_CONDITION : DBNull.Value,
                            !string.IsNullOrEmpty(ORIGIN) ? ORIGIN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_TABLE) ? EXTRA_FACET_TABLE : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_CROWN) ? EXTRA_FACET_CROWN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_PAV) ? EXTRA_FACET_PAV : DBNull.Value,
                            !string.IsNullOrEmpty(INTERNAL_GRAINING) ? INTERNAL_GRAINING : DBNull.Value,
                            !string.IsNullOrEmpty(H_A) ? H_A : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER_COMMENTS) ? SUPPLIER_COMMENTS : DBNull.Value);
                    }
                }
                else
                {
                    DataRow newRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        newRow[i] = DBNull.Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
                var result = await _supplierService.Get_Lab_Search_Distinct_Report_Search(dataTable);
                if (result != null)
                {
                    List<Report_Column_Distinct> Report_Distinct_Data = new List<Report_Column_Distinct>();
                    List<Report_Category_Value> Column_Values = new List<Report_Category_Value>();

                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "STOCK ID", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("STOCK ID") && x.ContainsKey("STOCK ID")).Select(x => new { Category_Name = x["STOCK ID"].ToString(), Category_Value = x["STOCK ID"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "IMAGE LINK", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "VIDEO LINK", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LAB", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("LAB") && x.ContainsKey("LAB_Id")).Select(x => new { Category_Name = x["LAB"].ToString(), Category_Value = x["LAB_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SUPPLIER NO", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SUPPLIER NO")).Select(x => new { Category_Name = x["SUPPLIER NO"].ToString(), Category_Value = x["SUPPLIER NO"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CERTIFICATE NO", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CERTIFICATE NO")).Select(x => new { Category_Name = x["CERTIFICATE NO"].ToString(), Category_Value = x["CERTIFICATE NO"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "COMPANY", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("COMPANY")).Select(x => new { Category_Name = x["COMPANY"].ToString(), Category_Value = x["COMPANY_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SHAPE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SHAPE") && x.ContainsKey("SHAPE_Id")).Select(x => new { Category_Name = x["SHAPE"].ToString(), Category_Value = x["SHAPE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "POINTER", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("POINTER") && x.ContainsKey("POINTER_Id")).Select(x => new { Category_Name = x["POINTER"].ToString(), Category_Value = x["POINTER_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SUB POINTER", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SUB POINTER") && x.ContainsKey("SUB_POINTER_Id")).Select(x => new { Category_Name = x["SUB POINTER"].ToString(), Category_Value = x["SUB_POINTER_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BGM", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("BGM") && x.ContainsKey("BGM_Id")).Select(x => new { Category_Name = x["BGM"].ToString(), Category_Value = x["BGM_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "COLOR", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("COLOR") && x.ContainsKey("COLOR_Id")).Select(x => new { Category_Name = x["COLOR"].ToString(), Category_Value = x["COLOR_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CLARITY", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CLARITY") && x.ContainsKey("CLARITY_Id")).Select(x => new { Category_Name = x["CLARITY"].ToString(), Category_Value = x["CLARITY_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CTS", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "RAP RATE", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "RAP AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "COST DISC", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "COST AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "OFFER DISC", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "OFFER AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BASE DISC", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BASE AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "PRICE PER CTS", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "MAX SLAB BASE DISC", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "MAX SLAB BASE AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CUT", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CUT") && x.ContainsKey("CUT_Id")).Select(x => new { Category_Name = x["CUT"].ToString(), Category_Value = x["CUT_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "POLISH", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("POLISH") && x.ContainsKey("POLISH_Id")).Select(x => new { Category_Name = x["POLISH"].ToString(), Category_Value = x["POLISH_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SYMM", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SYMM") && x.ContainsKey("SYMM_Id")).Select(x => new { Category_Name = x["SYMM"].ToString(), Category_Value = x["SYMM_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "FLS INTENSITY", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("FLS INTENSITY") && x.ContainsKey("FLS_INTENSITY_Id")).Select(x => new { Category_Name = x["FLS INTENSITY"].ToString(), Category_Value = x["FLS_INTENSITY_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "RATIO", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LENGTH", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "WIDTH", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "DEPTH", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "DEPTH PER", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "TABLE PER", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "KEY TO SYMBOL", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "COMMENT", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE PER", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CROWN ANGLE", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CROWN HEIGHT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "PAVILION ANGLE", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "PAVILION HEIGHT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "TABLE BLACK", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("TABLE BLACK") && x.ContainsKey("TABLE_BLACK_Id")).Select(x => new { Category_Name = x["TABLE BLACK"].ToString(), Category_Value = x["TABLE_BLACK_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SIDE BLACK", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SIDE BLACK") && x.ContainsKey("SIDE_BLACK_Id")).Select(x => new { Category_Name = x["SIDE BLACK"].ToString(), Category_Value = x["SIDE_BLACK_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "TABLE WHITE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("TABLE WHITE") && x.ContainsKey("TABLE_WHITE_Id")).Select(x => new { Category_Name = x["TABLE WHITE"].ToString(), Category_Value = x["TABLE_WHITE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SIDE WHITE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SIDE WHITE") && x.ContainsKey("SIDE_WHITE_Id")).Select(x => new { Category_Name = x["SIDE WHITE"].ToString(), Category_Value = x["SIDE_WHITE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CULET", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CULET") && x.ContainsKey("CULET_Id")).Select(x => new { Category_Name = x["CULET"].ToString(), Category_Value = x["CULET_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "TABLE OPEN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("TABLE OPEN") && x.ContainsKey("TABLE_OPEN_Id")).Select(x => new { Category_Name = x["TABLE OPEN"].ToString(), Category_Value = x["TABLE_OPEN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CROWN OPEN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CROWN OPEN") && x.ContainsKey("CROWN_OPEN_Id")).Select(x => new { Category_Name = x["CROWN OPEN"].ToString(), Category_Value = x["CROWN_OPEN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "PAVILION OPEN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("PAVILION OPEN") && x.ContainsKey("PAV_OPEN_Id")).Select(x => new { Category_Name = x["PAVILION OPEN"].ToString(), Category_Value = x["PAV_OPEN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE OPEN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GIRDLE OPEN") && x.ContainsKey("GIRDLE_OPEN_Id")).Select(x => new { Category_Name = x["GIRDLE OPEN"].ToString(), Category_Value = x["GIRDLE_OPEN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LUSTER", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("LUSTER") && x.ContainsKey("LUSTER_Id")).Select(x => new { Category_Name = x["LUSTER"].ToString(), Category_Value = x["LUSTER_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SHADE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("SHADE") && x.ContainsKey("SHADE_Id")).Select(x => new { Category_Name = x["SHADE"].ToString(), Category_Value = x["SHADE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "MILKY", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("MILKY") && x.ContainsKey("MILKY_Id")).Select(x => new { Category_Name = x["MILKY"].ToString(), Category_Value = x["MILKY_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LOCATION", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("LOCATION") && x.ContainsKey("LOCATION_Id")).Select(x => new { Category_Name = x["LOCATION"].ToString(), Category_Value = x["LOCATION_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE FROM", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GIRDLE FROM") && x.ContainsKey("GIRDLE_FROM_Id")).Select(x => new { Category_Name = x["GIRDLE FROM"].ToString(), Category_Value = x["GIRDLE_FROM_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE TO", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GIRDLE TO") && x.ContainsKey("GIRDLE_TO_Id")).Select(x => new { Category_Name = x["GIRDLE TO"].ToString(), Category_Value = x["GIRDLE_TO_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE CONDITION", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GIRDLE CONDITION") && x.ContainsKey("GIRDLE_CONDITION_Id")).Select(x => new { Category_Name = x["GIRDLE CONDITION"].ToString(), Category_Value = x["GIRDLE_CONDITION_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GIRDLE TYPE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GIRDLE TYPE") && x.ContainsKey("GIRDLE_TYPE_Id")).Select(x => new { Category_Name = x["GIRDLE TYPE"].ToString(), Category_Value = x["GIRDLE_TYPE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "FANCY COLOR", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("FANCY COLOR") && x.ContainsKey("FANCY_COLOR_Id")).Select(x => new { Category_Name = x["FANCY COLOR"].ToString(), Category_Value = x["FANCY_COLOR_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "FANCY INTENSITY", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("FANCY INTENSITY") && x.ContainsKey("FANCY_INTENSITY_Id")).Select(x => new { Category_Name = x["FANCY INTENSITY"].ToString(), Category_Value = x["FANCY_INTENSITY_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "FANCY OVERSTON", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("FANCY OVERSTON") && x.ContainsKey("FANCY_OVERSTON_Id")).Select(x => new { Category_Name = x["FANCY OVERSTON"].ToString(), Category_Value = x["FANCY_OVERSTON_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "ORIGIN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("ORIGIN") && x.ContainsKey("ORIGIN_Id")).Select(x => new { Category_Name = x["ORIGIN"].ToString(), Category_Value = x["ORIGIN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "EXTRA FACET TABLE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("EXTRA FACET TABLE") && x.ContainsKey("EXTRA_FACET_TABLE_Id")).Select(x => new { Category_Name = x["EXTRA FACET TABLE"].ToString(), Category_Value = x["EXTRA_FACET_TABLE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "EXTRA FACET CROWN", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("EXTRA FACET CROWN") && x.ContainsKey("EXTRA_FACET_CROWN_Id")).Select(x => new { Category_Name = x["EXTRA FACET CROWN"].ToString(), Category_Value = x["EXTRA_FACET_CROWN_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "EXTRA FACET PAVILION", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("EXTRA FACET PAVILION") && x.ContainsKey("EXTRA_FACET_PAV_Id")).Select(x => new { Category_Name = x["EXTRA FACET PAVILION"].ToString(), Category_Value = x["EXTRA_FACET_PAV_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "INTERNAL GRAINING", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("INTERNAL GRAINING") && x.ContainsKey("INTERNAL_GRAINING_Id")).Select(x => new { Category_Name = x["INTERNAL GRAINING"].ToString(), Category_Value = x["INTERNAL_GRAINING_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "H_A", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("H_A") && x.ContainsKey("H_A_Id")).Select(x => new { Category_Name = x["H_A"].ToString(), Category_Value = x["H_A_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "GOOD TYPE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("GOOD TYPE") && x.ContainsKey("GOOD_TYPE_Id")).Select(x => new { Category_Name = x["GOOD TYPE"].ToString(), Category_Value = x["GOOD_TYPE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LR HALF", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "STR LN", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CERTIFICATE LINK", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "DNA", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "RANK", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SUNRISE STATUS", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "STATUS", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("STATUS") && x.ContainsKey("STATUS_Id")).Select(x => new { Category_Name = x["STATUS"].ToString(), Category_Value = x["STATUS_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BID", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BUYER", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BUYER DISC", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "BUYER AMOUNT", Display_Type = "R", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG STOCK DISC", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG STOCK PCS", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG PURCHASE DISC", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG PURCHASE PCS", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG SALE DISC", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "AVG SALE PCS", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "KTS GRADE", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SUNRISE CLARITY GRADE", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "ZONE", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "PARAMETER GRADE", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "CERT TYPE", Display_Type = "M", Column_Value = result.Where(x => x.ContainsKey("CERT TYPE") && x.ContainsKey("CERT_TYPE_Id")).Select(x => new { Category_Name = x["CERT TYPE"].ToString(), Category_Value = x["CERT_TYPE_Id"].ToString() }).Distinct().ToList().Select(x => new Report_Category_Value { Category_Name = x.Category_Name, Category_Value = x.Category_Value }) });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "LAB COMMENTS", Display_Type = "", Column_Value = Column_Values });
                    Report_Distinct_Data.Add(new Report_Column_Distinct { Column_Name = "SUPPLIER COMMENTS", Display_Type = "", Column_Value = Column_Values });

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = Report_Distinct_Data
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Lab_Report_Search", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get_stock_availibility_report_search")]
        [Authorize]
        public async Task<IActionResult> Get_Stock_Availibility_Report_Search([FromForm] Stock_Avalibility stock_Avalibility, IFormFile? File_Location)
        {
            try
            {
                IList<Stock_Avalibility_Values> stock_Avalibility_Values = new List<Stock_Avalibility_Values>();
                var lst_Stock_Id = new List<string>();
                if (!string.IsNullOrEmpty(stock_Avalibility.stock_Id) && stock_Avalibility.stock_Id.Contains(";"))
                {
                    lst_Stock_Id = stock_Avalibility.stock_Id.Split('/').ToList();
                    if (lst_Stock_Id != null && lst_Stock_Id.Count > 0)
                    {
                        foreach (var item in lst_Stock_Id)
                        {
                            var model = new Stock_Avalibility_Values();
                            var lst_stock_val = item.Split(';').ToList();
                            if (lst_stock_val != null && lst_stock_val.Count > 0)
                            {
                                if (lst_stock_val.Count == 1)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(lst_stock_val[i]))
                                            continue;

                                        model.Stock_Id = lst_stock_val[0];
                                    }
                                }
                                else if (lst_stock_val.Count == 2)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                        }
                                    }
                                }
                                else if (lst_stock_val.Count == 3)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                            //var con_val_1 = Convert.ToDecimal(lst_stock_val[2]);
                                            //if (con_val_1 >= -100 && con_val_1 <= 100)
                                            //{
                                            //    model.Offer_Disc = lst_stock_val[2];
                                            //}
                                            //else
                                            //{
                                            //    model.Offer_Amount = lst_stock_val[2];
                                            //}
                                        }
                                    }
                                }
                            }
                            stock_Avalibility_Values.Add(model);
                        }
                        stock_Avalibility_Values = stock_Avalibility_Values.GroupBy(x => x.Stock_Id).Select(x => x.First()).ToList();
                    }
                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                if (File_Location != null)
                {
                    var filePath = Path.GetTempFileName();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File_Location.CopyToAsync(stream);
                    }
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int startRow = 2;

                        HashSet<string> uniqueValues = new HashSet<string>();
                        for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string value = worksheet.Cells[row, 1].GetValue<string>();
                            string offer_amt = worksheet.Cells[row, 3].GetValue<string>();
                            if (!uniqueValues.Contains(value))
                            {
                                uniqueValues.Add(value);
                                dataTable.Rows.Add(value, !string.IsNullOrEmpty(offer_amt) ? offer_amt.Replace(",", "") : DBNull.Value, worksheet.Cells[row, 2].GetValue<string>());
                            }
                        }
                        var uniqueRows = dataTable.AsEnumerable()
                                  .GroupBy(row => row.Field<string>("STOCK_ID"))
                                  .Select(grp => grp.First())
                                  .CopyToDataTable();
                    }
                }
                else if (stock_Avalibility_Values != null && stock_Avalibility_Values.Count > 0)
                {
                    stock_Avalibility.stock_Id = null;
                    foreach (var item in stock_Avalibility_Values)
                    {
                        dataTable.Rows.Add(!string.IsNullOrEmpty(item.Stock_Id) ? item.Stock_Id : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Amount) ? item.Offer_Amount : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Disc) ? item.Offer_Disc : DBNull.Value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(stock_Avalibility.stock_Id) && !stock_Avalibility.stock_Id.Contains(";"))
                    {
                        lst_Stock_Id = stock_Avalibility.stock_Id.Replace("//", "/").Split('/').Distinct().ToList();
                        foreach (var item in lst_Stock_Id)
                        {
                            dataTable.Rows.Add(!string.IsNullOrEmpty(item) ? item : DBNull.Value, DBNull.Value, DBNull.Value);
                        }
                    }
                }

                var (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseAmtr, totalBaseDiscr, totalOfferAmtr, totalOfferDiscr) = await _supplierService.Get_Stock_Avalibility_Report_Search(dataTable, stock_Avalibility.stock_Id, stock_Avalibility.stock_Type, stock_Avalibility.supp_Stock_Id, stock_Avalibility.iPgNo ?? 0, stock_Avalibility.iPgSize ?? 0, stock_Avalibility.iSort);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        total_Records = totalRecordr,
                        total_Cts = totalCtsr,
                        total_Amt = totalAmtr,
                        total_Disc = totalDiscr,
                        total_Base_Amt = totalBaseAmtr,
                        total_Base_Disc = totalBaseDiscr,
                        total_Offer_Amt = totalOfferAmtr,
                        total_Offer_Disc = totalOfferDiscr,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Search", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_saved_report_serach")]
        [Authorize]
        public async Task<IActionResult> Get_Saved_Report_Serach(int user_Id)
        {
            try
            {
                var result = await _supplierService.Get_Search_Save_Report_Search(user_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Saved_Report_Serach", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_report_search_save")]
        [Authorize]
        public async Task<IActionResult> Create_Report_Search_Save(Report_Search_Save report_Search_Save)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                            CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                            POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                            LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                            IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                            DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                            CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                            RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                            OFFER_DISC = string.Empty, OFFER_AMOUNT = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                            PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                            BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                            TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                            GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                            KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                            FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                            STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                            SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty;

                    Report_Lab_Filter report_Lab_Filter = JsonConvert.DeserializeObject<Report_Lab_Filter>(report_Search_Save.Search_Value.ToString());

                    string Search_Display = string.Empty;
                    Search_Display += "[";
                    int i = 1;
                    foreach (var item in report_Lab_Filter.Report_Filter_Parameter_List)
                    {
                        STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER").Select(x => x.Category_Name).FirstOrDefault();
                        CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Name).FirstOrDefault();
                        SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Name).FirstOrDefault();
                        CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                        COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Name).FirstOrDefault();
                        CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Name).FirstOrDefault();
                        CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Name).FirstOrDefault();
                        POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Name).FirstOrDefault();
                        SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Name).FirstOrDefault();
                        FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Name).FirstOrDefault();
                        BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Name).FirstOrDefault();
                        LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Name).FirstOrDefault();
                        GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Name).FirstOrDefault();
                        LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Name).FirstOrDefault();
                        STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Name).FirstOrDefault();
                        IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                        VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                        LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                        WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_AMOUNT = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Name).FirstOrDefault();
                        TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Name).FirstOrDefault();
                        SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Name).FirstOrDefault();
                        SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Name).FirstOrDefault();
                        TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Name).FirstOrDefault();
                        CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Name).FirstOrDefault();
                        PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Name).FirstOrDefault();
                        GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Name).FirstOrDefault();
                        KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                        KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                        LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                        LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                        FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Name).FirstOrDefault();
                        FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Name).FirstOrDefault();
                        FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Name).FirstOrDefault();
                        POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Name).FirstOrDefault();
                        STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Name).FirstOrDefault();
                        LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Name).FirstOrDefault();
                        CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Name).FirstOrDefault();
                        COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                        DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                        SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        Search_Display += " { \"Filter\" : \"";
                        Search_Display += !string.IsNullOrEmpty(STOCK_ID) ? " SUPPLIER NO/ CERT NO : " + STOCK_ID : "";
                        Search_Display += !string.IsNullOrEmpty(SUPPLIER) ? " SUPPLIER : " + SUPPLIER : "";
                        Search_Display += !string.IsNullOrEmpty(CUSTOMER) ? " CUSTOMER : " + CUSTOMER : "";
                        Search_Display += !string.IsNullOrEmpty(SHAPE) ? " SHAPE : " + SHAPE : "";
                        Search_Display += !string.IsNullOrEmpty(CTS) ? " CTS : " + CTS : "";
                        Search_Display += !string.IsNullOrEmpty(COLOR) ? " COLOR : " + COLOR : "";
                        Search_Display += !string.IsNullOrEmpty(CLARITY) ? " CLARITY : " + CLARITY : "";
                        Search_Display += !string.IsNullOrEmpty(CUT) ? " CUT : " + CUT : "";
                        Search_Display += !string.IsNullOrEmpty(POLISH) ? " POLISH : " + POLISH : "";
                        Search_Display += !string.IsNullOrEmpty(SYMM) ? " SYMM : " + SYMM : "";
                        Search_Display += !string.IsNullOrEmpty(FLS_INTENSITY) ? " FLS INTENSITY : " + FLS_INTENSITY : "";
                        Search_Display += !string.IsNullOrEmpty(BGM) ? " BGM : " + BGM : "";
                        Search_Display += !string.IsNullOrEmpty(LAB) ? " LAB : " + LAB : "";
                        Search_Display += !string.IsNullOrEmpty(GOOD_TYPE) ? " GOOD TYPE : " + GOOD_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(LOCATION) ? " LOCATION : " + LOCATION : "";
                        Search_Display += !string.IsNullOrEmpty(STATUS) ? " STATUS : " + STATUS : "";
                        Search_Display += !string.IsNullOrEmpty(IMAGE_LINK) ? " IMAGE LINK : " + IMAGE_LINK : "";
                        Search_Display += !string.IsNullOrEmpty(VIDEO_LINK) ? " VIDEO LINK : " + VIDEO_LINK : "";
                        Search_Display += !string.IsNullOrEmpty(LENGTH) ? " LENGTH : " + LENGTH : "";
                        Search_Display += !string.IsNullOrEmpty(WIDTH) ? " WIDTH : " + WIDTH : "";
                        Search_Display += !string.IsNullOrEmpty(DEPTH) ? " DEPTH : " + DEPTH : "";
                        Search_Display += !string.IsNullOrEmpty(TABLE_PER) ? " TABLE PER : " + TABLE_PER : "";
                        Search_Display += !string.IsNullOrEmpty(DEPTH_PER) ? " DEPTH PER : " + DEPTH_PER : "";
                        Search_Display += !string.IsNullOrEmpty(CROWN_ANGLE) ? " CROWN ANGLE : " + CROWN_ANGLE : "";
                        Search_Display += !string.IsNullOrEmpty(CROWN_HEIGHT) ? " CROWN HEIGHT : " + CROWN_HEIGHT : "";
                        Search_Display += !string.IsNullOrEmpty(PAVILION_ANGLE) ? " PAVILION ANGLE : " + PAVILION_ANGLE : "";
                        Search_Display += !string.IsNullOrEmpty(PAVILION_HEIGHT) ? " PAVILION HEIGHT : " + PAVILION_HEIGHT : "";
                        Search_Display += !string.IsNullOrEmpty(GIRDLE_PER) ? " GIRDLE PER : " + GIRDLE_PER : "";
                        Search_Display += !string.IsNullOrEmpty(OFFER_DISC) ? " OFFER DISC : " + OFFER_DISC : "";
                        Search_Display += !string.IsNullOrEmpty(OFFER_AMOUNT) ? " OFFER AMOUNT : " + OFFER_AMOUNT : "";
                        Search_Display += !string.IsNullOrEmpty(PRICE_PER_CTS) ? " PRICE PER CTS : " + PRICE_PER_CTS : "";
                        Search_Display += !string.IsNullOrEmpty(TABLE_BLACK) ? " TABLE BLACK : " + TABLE_BLACK : "";
                        Search_Display += !string.IsNullOrEmpty(TABLE_WHITE) ? " TABLE WHITE : " + TABLE_WHITE : "";
                        Search_Display += !string.IsNullOrEmpty(SIDE_BLACK) ? " SIDE BLACK : " + SIDE_BLACK : "";
                        Search_Display += !string.IsNullOrEmpty(SIDE_WHITE) ? " SIDE WHITE : " + SIDE_WHITE : "";
                        Search_Display += !string.IsNullOrEmpty(TABLE_OPEN) ? " TABLE OPEN : " + TABLE_OPEN : "";
                        Search_Display += !string.IsNullOrEmpty(CROWN_OPEN) ? " CROWN OPEN : " + CROWN_OPEN : "";
                        Search_Display += !string.IsNullOrEmpty(PAVILION_OPEN) ? " PAVILION OPEN : " + PAVILION_OPEN : "";
                        Search_Display += !string.IsNullOrEmpty(GIRDLE_OPEN) ? " GIRDLE OPEN : " + GIRDLE_OPEN : "";
                        Search_Display += !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? " KEY TO SYMBOL TRUE : " + KEY_TO_SYMBOL_TRUE : "";
                        Search_Display += !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? " KEY TO SYMBOL FALSE : " + KEY_TO_SYMBOL_FALSE : "";
                        Search_Display += !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? " LAB COMMENTS TRUE : " + LAB_COMMENTS_TRUE : "";
                        Search_Display += !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? " LAB COMMENTS FALSE : " + LAB_COMMENTS_FALSE : "";
                        Search_Display += !string.IsNullOrEmpty(FANCY_COLOR) ? " FANCY COLOR : " + FANCY_COLOR : "";
                        Search_Display += !string.IsNullOrEmpty(FANCY_INTENSITY) ? " FANCY INTENSITY : " + FANCY_INTENSITY : "";
                        Search_Display += !string.IsNullOrEmpty(FANCY_OVERTONE) ? " FANCY OVERTONE : " + FANCY_OVERTONE : "";
                        Search_Display += !string.IsNullOrEmpty(POINTER) ? " POINTER : " + POINTER : "";
                        Search_Display += !string.IsNullOrEmpty(STAR_LN) ? " STAR LN : " + STAR_LN : "";
                        Search_Display += !string.IsNullOrEmpty(LR_HALF) ? " LR HALF : " + LR_HALF : "";
                        Search_Display += !string.IsNullOrEmpty(CERT_TYPE) ? " CERT TYPE : " + CERT_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(COST_RATE) ? " COST RATE : " + COST_RATE : "";
                        Search_Display += !string.IsNullOrEmpty(DISCOUNT_TYPE) ? " DISCOUNT TYPE : " + DISCOUNT_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(DISCOUNT_TYPE) ? " DISCOUNT TYPE1 : " + DISCOUNT_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(DISCOUNT_TYPE) ? " DISCOUNT TYPE2 : " + DISCOUNT_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(DISCOUNT_TYPE) ? " DISCOUNT TYPE3 : " + DISCOUNT_TYPE : "";
                        Search_Display += !string.IsNullOrEmpty(SIGN) ? " SIGN : " + SIGN : "";
                        Search_Display += !string.IsNullOrEmpty(DISC_VALUE) ? " DISC VALUE : " + DISC_VALUE : "";
                        Search_Display += !string.IsNullOrEmpty(SUPP_STOCK_ID) ? " SUPP STOCK ID : " + SUPP_STOCK_ID : "";
                        Search_Display += " \"}";

                        if (report_Lab_Filter.Report_Filter_Parameter_List.Count() != i)
                        {
                            Search_Display += ",";
                        }
                        i++;
                    }
                    Search_Display += "]";
                    report_Search_Save.Search_Display = Search_Display;
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var result = await _supplierService.Create_Update_Report_Search(report_Search_Save, (int)user_Id);
                    if (result == "success")
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SearchedReportCreated
                        });
                    }
                    else if (result == "exist")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SearchedReportNameExist,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Report_Search_Save", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_report_search_save")]
        [Authorize]
        public async Task<IActionResult> Delete_Report_Search_Save(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Delete_Report_Search(id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SearchedReportDeleted
                        });
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Report_Search_Save", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_report_layout_save")]
        [Authorize]
        public async Task<IActionResult> Get_Report_Layout_Save(int User_Id, int Rm_Id)
        {
            try
            {
                var result = await _supplierService.Get_Report_Layout_Save(User_Id, Rm_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Layout_Save", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_report_layout_save")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Report_Layout_Save(Report_Layout_Save report_Layout_Save)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (message, report_layout_save_Id) = await _supplierService.Create_Update_Report_Layout_Save(report_Layout_Save);
                    if (message == "success" && report_layout_save_Id > 0)
                    {
                        if (report_Layout_Save.Report_Layout_Save_Detail_List != null && report_Layout_Save.Report_Layout_Save_Detail_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Id", typeof(int));
                            dataTable.Columns.Add("Report_Layout_Id", typeof(int));
                            dataTable.Columns.Add("colId", typeof(string));
                            dataTable.Columns.Add("width", typeof(int));
                            dataTable.Columns.Add("hide", typeof(bool));
                            dataTable.Columns.Add("pinned", typeof(string));
                            dataTable.Columns.Add("sort", typeof(string));
                            dataTable.Columns.Add("sortIndex", typeof(int));
                            dataTable.Columns.Add("aggFunc", typeof(string));
                            dataTable.Columns.Add("rowGroup", typeof(bool));
                            dataTable.Columns.Add("rowGroupIndex", typeof(int));
                            dataTable.Columns.Add("pivot", typeof(bool));
                            dataTable.Columns.Add("pivotIndex", typeof(int));
                            dataTable.Columns.Add("flex", typeof(int));

                            foreach (var item in report_Layout_Save.Report_Layout_Save_Detail_List)
                            {
                                dataTable.Rows.Add(item.Id, report_layout_save_Id, item.colId, item.width, item.hide, item.pinned, item.sort, item.sortIndex, item.aggFunc, item.rowGroup, item.rowGroupIndex, item.pivot, item.pivotIndex, item.flex);
                            }
                            await _supplierService.Insert_Update_Report_Layout_Save_Detail(dataTable);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = report_Layout_Save.Id > 0 ? CoreCommonMessage.LayoutReportUpdated : CoreCommonMessage.LayoutReportCreated
                        });
                    }
                    else if (message == "exist")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.LayoutReportNameExist,
                        });

                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Report_Layout_Save", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_report_layout_save")]
        [Authorize]
        public async Task<IActionResult> Delete_Report_Layout_Save(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Delete_Report_Layout_Save(id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.LayoutReportDeleted
                        });
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Report_Layout_Save", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_report_layout_save_status")]
        [Authorize]
        public async Task<IActionResult> Update_Report_Layout_Save_Status(int id, int user_Id)
        {
            try
            {
                var result = await _supplierService.Update_Report_Layout_Save_Status(id, user_Id);
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.TempLayoutStatusUpdate
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Report_Layout_Save_Status", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        //[HttpPost]
        //[Route("get_report_search_total")]
        //[Authorize]
        //public async Task<IActionResult> Get_Report_Search_Total(Report_Filter report_Filter)
        //{
        //    try
        //    {
        //        var (_, totalRecordr, totalCtsr, totalAmtr, totalDiscr, _) = await _supplierService.Get_Report_Search(report_Filter.id, report_Filter.Report_Filter_Parameter, report_Filter.iPgNo ?? 0, report_Filter.iPgSize ?? 0, report_Filter.iSort);
        //        if (!string.IsNullOrEmpty(totalRecordr) && !string.IsNullOrEmpty(totalCtsr) && !string.IsNullOrEmpty(totalAmtr) && !string.IsNullOrEmpty(totalDiscr))
        //        {
        //            return Ok(new
        //            {
        //                statusCode = HttpStatusCode.OK,
        //                message = CoreCommonMessage.DataSuccessfullyFound,
        //                total_Records = totalRecordr,
        //                total_Cts = totalCtsr,
        //                total_Amt = totalAmtr,
        //                total_Disc = totalDiscr
        //            });
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _commonService.InsertErrorLog(ex.Message, "Get_Report_Search_Total", ex.StackTrace);
        //        return StatusCode((int)HttpStatusCode.InternalServerError, new
        //        {
        //            message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        [Route("get_lab_report_search_total")]
        [Authorize]
        public async Task<IActionResult> Get_Lab_Report_Search_Total(Report_Lab_Filter report_Lab_Filter)
        {
            try
            {
                string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                         CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                         POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                         LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                         IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                         DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                         CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                         RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                         OFFER_DISC = string.Empty, OFFER_VALUE = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                         PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                         BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                         TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                         GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                         KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                         FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                         STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                         SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty,
                         BID_SUPP_STOCK_ID = string.Empty, BID_BUYER_DISC = string.Empty, BID_BUYER_AMOUNT = string.Empty,
                         LUSTER = string.Empty, SHADE = string.Empty, MILKY = string.Empty, GIRDLE_FROM = string.Empty, GIRDLE_TO = string.Empty, GIRDLE_CONDITION = string.Empty, ORIGIN = string.Empty,
                         EXTRA_FACET_TABLE = string.Empty, EXTRA_FACET_CROWN = string.Empty, EXTRA_FACET_PAV = string.Empty,
                         INTERNAL_GRAINING = string.Empty, H_A = string.Empty, SUPPLIER_COMMENTS = string.Empty;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("SUPPLIER", typeof(string));
                dataTable.Columns.Add("CUSTOMER", typeof(string));
                dataTable.Columns.Add("SHAPE", typeof(string));
                dataTable.Columns.Add("CTS", typeof(string));
                dataTable.Columns.Add("COLOR", typeof(string));
                dataTable.Columns.Add("CLARITY", typeof(string));
                dataTable.Columns.Add("CUT", typeof(string));
                dataTable.Columns.Add("POLISH", typeof(string));
                dataTable.Columns.Add("SYMM", typeof(string));
                dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
                dataTable.Columns.Add("BGM", typeof(string));
                dataTable.Columns.Add("LAB", typeof(string));
                dataTable.Columns.Add("GOOD_TYPE", typeof(string));
                dataTable.Columns.Add("LOCATION", typeof(string));
                dataTable.Columns.Add("STATUS", typeof(string));
                dataTable.Columns.Add("IMAGE_LINK", typeof(string));
                dataTable.Columns.Add("VIDEO_LINK", typeof(string));
                dataTable.Columns.Add("LENGTH", typeof(string));
                dataTable.Columns.Add("WIDTH", typeof(string));
                dataTable.Columns.Add("DEPTH", typeof(string));
                dataTable.Columns.Add("TABLE_PER", typeof(string));
                dataTable.Columns.Add("DEPTH_PER", typeof(string));
                dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
                dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
                dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
                dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
                dataTable.Columns.Add("GIRDLE_PER", typeof(string));
                dataTable.Columns.Add("RAP_RATE", typeof(string));
                dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
                dataTable.Columns.Add("COST_DISC", typeof(string));
                dataTable.Columns.Add("COST_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("BASE_DISC", typeof(string));
                dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
                dataTable.Columns.Add("TABLE_BLACK", typeof(string));
                dataTable.Columns.Add("TABLE_WHITE", typeof(string));
                dataTable.Columns.Add("SIDE_BLACK", typeof(string));
                dataTable.Columns.Add("SIDE_WHITE", typeof(string));
                dataTable.Columns.Add("TABLE_OPEN", typeof(string));
                dataTable.Columns.Add("CROWN_OPEN", typeof(string));
                dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
                dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
                dataTable.Columns.Add("CULET", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                dataTable.Columns.Add("FANCY_COLOR", typeof(string));
                dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
                dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
                dataTable.Columns.Add("POINTER", typeof(string));
                dataTable.Columns.Add("SUB_POINTER", typeof(string));
                dataTable.Columns.Add("STAR_LN", typeof(string));
                dataTable.Columns.Add("LR_HALF", typeof(string));
                dataTable.Columns.Add("CERT_TYPE", typeof(string));
                dataTable.Columns.Add("COST_RATE", typeof(string));
                dataTable.Columns.Add("RATIO", typeof(string));
                dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
                dataTable.Columns.Add("SIGN", typeof(string));
                dataTable.Columns.Add("DISC_VALUE", typeof(string));
                dataTable.Columns.Add("DISC_VALUE1", typeof(string));
                dataTable.Columns.Add("DISC_VALUE2", typeof(string));
                dataTable.Columns.Add("DISC_VALUE3", typeof(string));
                dataTable.Columns.Add("BASE_TYPE", typeof(string));
                dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                dataTable.Columns.Add("LUSTER", typeof(string));
                dataTable.Columns.Add("SHADE", typeof(string));
                dataTable.Columns.Add("MILKY", typeof(string));
                dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
                dataTable.Columns.Add("GIRDLE_TO", typeof(string));
                dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
                dataTable.Columns.Add("ORIGIN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
                dataTable.Columns.Add("H_A", typeof(string));
                dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));

                if (report_Lab_Filter.Report_Filter_Parameter_List != null && report_Lab_Filter.Report_Filter_Parameter_List.Count > 0)
                {
                    foreach (var item in report_Lab_Filter.Report_Filter_Parameter_List)
                    {
                        STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER" || x.Column_Name == "COMPANY").Select(x => x.Category_Value).FirstOrDefault();
                        CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Value).FirstOrDefault();
                        SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();
                        CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                        COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Value).FirstOrDefault();
                        CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Value).FirstOrDefault();
                        POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Value).FirstOrDefault();
                        SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Value).FirstOrDefault();
                        FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Value).FirstOrDefault();
                        LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Value).FirstOrDefault();
                        GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Value).FirstOrDefault();
                        STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Value).FirstOrDefault();
                        IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                        VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                        LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                        WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_RATE = item.Where(x => x.Column_Name == "RAP RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RAP_AMOUNT = item.Where(x => x.Column_Name == "RAP AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        COST_DISC = item.Where(x => x.Column_Name == "COST DISC").Select(x => x.Category_Value).FirstOrDefault();
                        COST_AMOUNT = item.Where(x => x.Column_Name == "COST AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        OFFER_VALUE = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_DISC = item.Where(x => x.Column_Name == "BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_AMOUNT = item.Where(x => x.Column_Name == "BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_DISC = item.Where(x => x.Column_Name == "MAX SLAB BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_BASE_AMOUNT = item.Where(x => x.Column_Name == "MAX SLAB BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        MAX_SLAB_PRICE_PER_CTS = item.Where(x => x.Column_Name == "MAX SLAB PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_DISC = item.Where(x => x.Column_Name == "BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BUYER_AMOUNT = item.Where(x => x.Column_Name == "BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                        SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                        TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                        CULET = item.Where(x => x.Column_Name == "CULET").Select(x => x.Category_Value).FirstOrDefault();
                        KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                        KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                        LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                        LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                        FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                        FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Value).FirstOrDefault();
                        POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        SUB_POINTER = item.Where(x => x.Column_Name == "SUB POINTER").Select(x => x.Category_Value).FirstOrDefault();
                        STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Value).FirstOrDefault();
                        LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Value).FirstOrDefault();
                        CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                        RATIO = item.Where(x => x.Column_Name == "RATIO").Select(x => x.Category_Value).FirstOrDefault();
                        DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                        DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                        BASE_TYPE = item.Where(x => x.Column_Name == "BASE TYPE").Select(x => x.Category_Value).FirstOrDefault();
                        SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_SUPP_STOCK_ID = item.Where(x => x.Column_Name == "BID SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_DISC = item.Where(x => x.Column_Name == "BID BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                        BID_BUYER_AMOUNT = item.Where(x => x.Column_Name == "BID BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                        LUSTER = item.Where(x => x.Column_Name == "LUSTER").Select(x => x.Category_Value).FirstOrDefault();
                        SHADE = item.Where(x => x.Column_Name == "SHADE").Select(x => x.Category_Value).FirstOrDefault();
                        MILKY = item.Where(x => x.Column_Name == "MILKY").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_FROM = item.Where(x => x.Column_Name == "GIRDLE_FROM").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_TO = item.Where(x => x.Column_Name == "GIRDLE_TO").Select(x => x.Category_Value).FirstOrDefault();
                        GIRDLE_CONDITION = item.Where(x => x.Column_Name == "GIRDLE_CONDITION").Select(x => x.Category_Value).FirstOrDefault();
                        ORIGIN = item.Where(x => x.Column_Name == "ORIGIN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_TABLE = item.Where(x => x.Column_Name == "EXTRA_FACET_TABLE").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_CROWN = item.Where(x => x.Column_Name == "EXTRA_FACET_CROWN").Select(x => x.Category_Value).FirstOrDefault();
                        EXTRA_FACET_PAV = item.Where(x => x.Column_Name == "EXTRA_FACET_PAV").Select(x => x.Category_Value).FirstOrDefault();
                        INTERNAL_GRAINING = item.Where(x => x.Column_Name == "INTERNAL_GRAINING").Select(x => x.Category_Value).FirstOrDefault();
                        H_A = item.Where(x => x.Column_Name == "H_A").Select(x => x.Category_Value).FirstOrDefault();
                        SUPPLIER_COMMENTS = item.Where(x => x.Column_Name == "SUPPLIER_COMMENTS").Select(x => x.Category_Value).FirstOrDefault();

                        dataTable.Rows.Add(!string.IsNullOrEmpty(STOCK_ID) ? STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER) ? SUPPLIER : DBNull.Value,
                            !string.IsNullOrEmpty(CUSTOMER) ? CUSTOMER : DBNull.Value,
                            !string.IsNullOrEmpty(SHAPE) ? SHAPE : DBNull.Value,
                            !string.IsNullOrEmpty(CTS) ? CTS : DBNull.Value,
                            !string.IsNullOrEmpty(COLOR) ? COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(CLARITY) ? CLARITY : DBNull.Value,
                            !string.IsNullOrEmpty(CUT) ? CUT : DBNull.Value,
                            !string.IsNullOrEmpty(POLISH) ? POLISH : DBNull.Value,
                            !string.IsNullOrEmpty(SYMM) ? SYMM : DBNull.Value,
                            !string.IsNullOrEmpty(FLS_INTENSITY) ? FLS_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(BGM) ? BGM : DBNull.Value,
                            !string.IsNullOrEmpty(LAB) ? LAB : DBNull.Value,
                            !string.IsNullOrEmpty(GOOD_TYPE) ? GOOD_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(LOCATION) ? LOCATION : DBNull.Value,
                            !string.IsNullOrEmpty(STATUS) ? STATUS : DBNull.Value,
                            !string.IsNullOrEmpty(IMAGE_LINK) ? IMAGE_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(VIDEO_LINK) ? VIDEO_LINK : DBNull.Value,
                            !string.IsNullOrEmpty(LENGTH) ? LENGTH : DBNull.Value,
                            !string.IsNullOrEmpty(WIDTH) ? WIDTH : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH) ? DEPTH : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_PER) ? TABLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(DEPTH_PER) ? DEPTH_PER : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_ANGLE) ? CROWN_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_HEIGHT) ? CROWN_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_ANGLE) ? PAVILION_ANGLE : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_HEIGHT) ? PAVILION_HEIGHT : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_PER) ? GIRDLE_PER : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_RATE) ? RAP_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RAP_AMOUNT) ? RAP_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(COST_DISC) ? COST_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(COST_AMOUNT) ? COST_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_DISC) ? OFFER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(OFFER_VALUE) ? OFFER_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_DISC) ? BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_AMOUNT) ? BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(PRICE_PER_CTS) ? PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_DISC) ? MAX_SLAB_BASE_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_BASE_AMOUNT) ? MAX_SLAB_BASE_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(MAX_SLAB_PRICE_PER_CTS) ? MAX_SLAB_PRICE_PER_CTS : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_DISC) ? BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BUYER_AMOUNT) ? BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_BLACK) ? TABLE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_WHITE) ? TABLE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_BLACK) ? SIDE_BLACK : DBNull.Value,
                            !string.IsNullOrEmpty(SIDE_WHITE) ? SIDE_WHITE : DBNull.Value,
                            !string.IsNullOrEmpty(TABLE_OPEN) ? TABLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CROWN_OPEN) ? CROWN_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(PAVILION_OPEN) ? PAVILION_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_OPEN) ? GIRDLE_OPEN : DBNull.Value,
                            !string.IsNullOrEmpty(CULET) ? CULET : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? KEY_TO_SYMBOL_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? KEY_TO_SYMBOL_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? LAB_COMMENTS_TRUE : DBNull.Value,
                            !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? LAB_COMMENTS_FALSE : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_COLOR) ? FANCY_COLOR : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_INTENSITY) ? FANCY_INTENSITY : DBNull.Value,
                            !string.IsNullOrEmpty(FANCY_OVERTONE) ? FANCY_OVERTONE : DBNull.Value,
                            !string.IsNullOrEmpty(POINTER) ? POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(SUB_POINTER) ? SUB_POINTER : DBNull.Value,
                            !string.IsNullOrEmpty(STAR_LN) ? STAR_LN : DBNull.Value,
                            !string.IsNullOrEmpty(LR_HALF) ? LR_HALF : DBNull.Value,
                            !string.IsNullOrEmpty(CERT_TYPE) ? CERT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(COST_RATE) ? COST_RATE : DBNull.Value,
                            !string.IsNullOrEmpty(RATIO) ? RATIO : DBNull.Value,
                            !string.IsNullOrEmpty(DISCOUNT_TYPE) ? DISCOUNT_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SIGN) ? SIGN : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE) ? DISC_VALUE : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE1) ? DISC_VALUE1 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE2) ? DISC_VALUE2 : DBNull.Value,
                            !string.IsNullOrEmpty(DISC_VALUE3) ? DISC_VALUE3 : DBNull.Value,
                            !string.IsNullOrEmpty(BASE_TYPE) ? BASE_TYPE : DBNull.Value,
                            !string.IsNullOrEmpty(SUPP_STOCK_ID) ? SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_SUPP_STOCK_ID) ? BID_SUPP_STOCK_ID : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_DISC) ? BID_BUYER_DISC : DBNull.Value,
                            !string.IsNullOrEmpty(BID_BUYER_AMOUNT) ? BID_BUYER_AMOUNT : DBNull.Value,
                            !string.IsNullOrEmpty(LUSTER) ? LUSTER : DBNull.Value,
                            !string.IsNullOrEmpty(SHADE) ? SHADE : DBNull.Value,
                            !string.IsNullOrEmpty(MILKY) ? MILKY : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_FROM) ? GIRDLE_FROM : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_TO) ? GIRDLE_TO : DBNull.Value,
                            !string.IsNullOrEmpty(GIRDLE_CONDITION) ? GIRDLE_CONDITION : DBNull.Value,
                            !string.IsNullOrEmpty(ORIGIN) ? ORIGIN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_TABLE) ? EXTRA_FACET_TABLE : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_CROWN) ? EXTRA_FACET_CROWN : DBNull.Value,
                            !string.IsNullOrEmpty(EXTRA_FACET_PAV) ? EXTRA_FACET_PAV : DBNull.Value,
                            !string.IsNullOrEmpty(INTERNAL_GRAINING) ? INTERNAL_GRAINING : DBNull.Value,
                            !string.IsNullOrEmpty(H_A) ? H_A : DBNull.Value,
                            !string.IsNullOrEmpty(SUPPLIER_COMMENTS) ? SUPPLIER_COMMENTS : DBNull.Value);
                    }
                }
                else
                {
                    DataRow newRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        newRow[i] = DBNull.Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
                var (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseDisc, totalBaseAmt, totalOfferDisc, totalOfferAmt, totalMaxSlabDisc, totalMaxSlabAmt, totalRap_Amount) = await _supplierService.Get_Lab_Search_Report_Search_Total(dataTable, report_Lab_Filter.iPgNo ?? 0, report_Lab_Filter.iPgSize ?? 0, report_Lab_Filter.iSort);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        total_Records = totalRecordr,
                        total_Cts = totalCtsr,
                        total_Amt = totalAmtr,
                        total_Disc = totalDiscr,
                        total_Base_Disc = totalBaseDisc,
                        total_Base_Amt = totalBaseAmt,
                        total_Offer_Disc = totalOfferDisc,
                        total_Offer_Amt = totalOfferAmt,
                        total_MaxSlab_Disc = totalMaxSlabDisc,
                        total_MaxSlab_Amt = totalMaxSlabAmt,
                        total_Rap_Amt = totalRap_Amount,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Lab_Report_Search_Total", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("download_image_video_certificate_stock")]
        [Authorize]
        public async Task<IActionResult> Download_Image_Video_Certificate_Stock(Report_Download report_Download)
        {
            try
            {
                var result = await _supplierService.Download_Image_Video_Certificate_Stock(report_Download.ids, report_Download.document_Type);
                if (result != null && result.Count > 0)
                {
                    List<Report_Download_Model> result_List = new List<Report_Download_Model>();
                    foreach (var item in result)
                    {
                        var filePath = report_Download.document_Type == "I" ?
                                   Path.Combine(Directory.GetCurrentDirectory(), "Files/Image") : (report_Download.document_Type == "V" ?
                                   Path.Combine(Directory.GetCurrentDirectory(), "Files/Video") : report_Download.document_Type == "C" ?
                                   Path.Combine(Directory.GetCurrentDirectory(), "Files/Certificate") : "");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        if (report_Download.document_Type == "I")
                        {
                            string filename = "/" + item.Stock_Id + "_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".jpg";
                            filePath += filename;
                            var path = _configuration["BaseUrl"] + "/Files/Image" + filename;
                            try
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(item.Url, filePath);
                                }
                                Report_Download_Model r = new Report_Download_Model()
                                {
                                    Stock_Id = item.Stock_Id,
                                    Url = path
                                };
                                result_List.Add(r);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else if (report_Download.document_Type == "V")
                        {
                            string filename = "/" + item.Stock_Id + "_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".mp4";
                            filePath += filename;
                            var path = _configuration["BaseUrl"] + "/Files/Video" + filename;
                            try
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(item.Url, filePath);
                                }
                                Report_Download_Model r = new Report_Download_Model()
                                {
                                    Stock_Id = item.Stock_Id,
                                    Url = path
                                };
                                result_List.Add(r);
                            }
                            catch (Exception)
                            {

                            }

                        }
                        else if (report_Download.document_Type == "C")
                        {
                            string filename = "/" + item.Stock_Id + "_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".pdf";
                            filePath += filename;
                            var path = _configuration["BaseUrl"] + "/Files/Certificate" + filename;
                            try
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.DownloadFile(item.Url, filePath);
                                }
                                Report_Download_Model r = new Report_Download_Model()
                                {
                                    Stock_Id = item.Stock_Id,
                                    Url = path
                                };
                                result_List.Add(r);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    if (result_List.Count > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = result_List
                        });
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Download_Image_Video_Certificate_Stock", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Cart/Approval Management        
        [HttpPost]
        [Route("create_update_cart")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Cart(Cart_Model cart_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IList<Cart_Detail> CartResult = JsonConvert.DeserializeObject<IList<Cart_Detail>>(cart_Model.Cart_Detail.ToString());

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Supp_Stock_Id", typeof(int));
                    dataTable.Columns.Add("Cts", typeof(double));
                    dataTable.Columns.Add("Cart_Base_Disc", typeof(double));
                    dataTable.Columns.Add("Cart_Base_Amt", typeof(double));
                    dataTable.Columns.Add("Cart_Final_Disc", typeof(double));
                    dataTable.Columns.Add("Cart_Final_Amt", typeof(double));
                    dataTable.Columns.Add("Cart_Final_Disc_Max_Slab", typeof(double));
                    dataTable.Columns.Add("Cart_Final_Amt_Max_Slab", typeof(double));
                    dataTable.Columns.Add("Cart_Offer_Disc", typeof(double));
                    dataTable.Columns.Add("Cart_Offer_Amt", typeof(double));
                    dataTable.Columns.Add("Buyer_Disc", typeof(double));
                    dataTable.Columns.Add("Buyer_Amt", typeof(double));
                    dataTable.Columns.Add("Buyer_Price_Per_Cts", typeof(double));
                    dataTable.Columns.Add("Expected_Final_Disc", typeof(double));
                    dataTable.Columns.Add("Expected_Final_Amt", typeof(double));
                    dataTable.Columns.Add("Cart_Status", typeof(string));


                    foreach (var item in CartResult)
                    {
                        dataTable.Rows.Add(item.Id, item.Supp_Stock_Id,
                            (item.Cts != null ? !string.IsNullOrEmpty(item.Cts.ToString()) ? Convert.ToDouble(item.Cts.ToString()) : null : null),
                            (item.Cart_Base_Disc != null ? !string.IsNullOrEmpty(item.Cart_Base_Disc.ToString()) ? Convert.ToDouble(item.Cart_Base_Disc.ToString()) : null : null),
                            (item.Cart_Base_Amt != null ? !string.IsNullOrEmpty(item.Cart_Base_Amt.ToString()) ? Convert.ToDouble(item.Cart_Base_Amt.ToString()) : null : null),
                            (item.Cart_Final_Disc != null ? !string.IsNullOrEmpty(item.Cart_Final_Disc.ToString()) ? Convert.ToDouble(item.Cart_Final_Disc.ToString()) : null : null),
                            (item.Cart_Final_Amt != null ? !string.IsNullOrEmpty(item.Cart_Final_Amt.ToString()) ? Convert.ToDouble(item.Cart_Final_Amt.ToString()) : null : null),
                            (item.Cart_Final_Disc_Max_Slab != null ? !string.IsNullOrEmpty(item.Cart_Final_Disc_Max_Slab.ToString()) ? Convert.ToDouble(item.Cart_Final_Disc_Max_Slab.ToString()) : null : null),
                            (item.Cart_Final_Amt_Max_Slab != null ? !string.IsNullOrEmpty(item.Cart_Final_Amt_Max_Slab.ToString()) ? Convert.ToDouble(item.Cart_Final_Amt_Max_Slab.ToString()) : null : null),
                            (item.Cart_Offer_Disc != null ? !string.IsNullOrEmpty(item.Cart_Offer_Disc.ToString()) ? Convert.ToDouble(item.Cart_Offer_Disc.ToString()) : null : null),
                            (item.Cart_Offer_Amt != null ? !string.IsNullOrEmpty(item.Cart_Offer_Amt.ToString()) ? Convert.ToDouble(item.Cart_Offer_Amt.ToString()) : null : null),
                            (item.Buyer_Disc != null ? !string.IsNullOrEmpty(item.Buyer_Disc.ToString()) ? Convert.ToDouble(item.Buyer_Disc.ToString()) : null : null),
                            (item.Buyer_Amt != null ? !string.IsNullOrEmpty(item.Buyer_Amt.ToString()) ? Convert.ToDouble(item.Buyer_Amt.ToString()) : null : null),
                            (item.Buyer_Price_Per_Cts != null ? !string.IsNullOrEmpty(item.Buyer_Price_Per_Cts.ToString()) ? Convert.ToDouble(item.Buyer_Price_Per_Cts.ToString()) : null : null),
                            (item.Expected_Final_Disc != null ? !string.IsNullOrEmpty(item.Expected_Final_Disc.ToString()) ? Convert.ToDouble(item.Expected_Final_Disc.ToString()) : null : null),
                            (item.Expected_Final_Amt != null ? !string.IsNullOrEmpty(item.Expected_Final_Amt.ToString()) ? Convert.ToDouble(item.Expected_Final_Amt.ToString()) : null : null),
                            (!string.IsNullOrEmpty(item.Cart_Status) ? Convert.ToDouble(item.Cart_Status.ToString()) : null));
                    }

                    var (message, result, msg) = await _cartService.Create_Update_Cart(dataTable, cart_Model.Id, (int)cart_Model.User_Id, cart_Model.Customer_Name, cart_Model.Remarks, cart_Model.Validity_Days ?? 0);
                    if ((message == "exist" && msg.Length > 0) || (message == "success" && msg.Length > 0))
                    {
                        // if alredy exists stone add again then message should show succsessfully added.
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = msg

                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Cart", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("delete_cart")]
        [Authorize]
        public async Task<IActionResult> Delete_Cart(JsonElement requestData)
        {
            try
            {
                if (requestData.TryGetProperty("ids", out var idsElement) && idsElement.ValueKind == JsonValueKind.String)
                {
                    string ids = idsElement.GetString();
                    if (!string.IsNullOrEmpty(ids))
                    {
                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                        var result = await _cartService.Delete_Cart(ids, user_Id ?? 0);
                        if (result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.CartStockDeleted
                            });
                        }
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Cart", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("approved_or_rejected_by_management")]
        [Authorize]
        public async Task<IActionResult> Approved_Or_Rejected_by_Management(Approval_Management approval_Management)
        {
            try
            {
                var result = await _cartService.Approved_Or_Rejected_by_Management(approval_Management);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = approval_Management.Is_Approved == true ? CoreCommonMessage.StokeApprovedByManagement : CoreCommonMessage.StokeRejectedByManagement
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Approved_Or_Rejected_by_Management", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("approved_management_update_status")]
        [Authorize]
        public async Task<IActionResult> Approved_Management_Update_Status(Approval_Management_Status approval_Management)
        {
            try
            {
                var result = await _cartService.Approved_Management_Update_Status(approval_Management);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StokeStatusManagement
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Approved_Or_Rejected_by_Management", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_approved_management")]
        [Authorize]
        public async Task<IActionResult> Create_Approved_Management(Approval_Management_Create_Update approval_Management)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IList<Approval_Detail> app_mang_Result = JsonConvert.DeserializeObject<IList<Approval_Detail>>(approval_Management.Approval_Detail.ToString());

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Supp_Stock_Id", typeof(int));
                    dataTable.Columns.Add("Cart_Id", typeof(int));
                    dataTable.Columns.Add("Buyer_Disc", typeof(double));
                    dataTable.Columns.Add("Buyer_Amt", typeof(double));
                    dataTable.Columns.Add("Expected_Final_Disc", typeof(double));
                    dataTable.Columns.Add("Expected_Final_Amt", typeof(double));

                    if (app_mang_Result != null && app_mang_Result.Count > 0)
                    {
                        foreach (var item in app_mang_Result)
                        {
                            dataTable.Rows.Add(item.Id, item.Supp_Stock_Id, item.Cart_Id,
                            (item.Buyer_Disc != null ? !string.IsNullOrEmpty(item.Buyer_Disc.ToString()) ? Convert.ToDouble(item.Buyer_Disc.ToString()) : null : null),
                            (item.Buyer_Amt != null ? !string.IsNullOrEmpty(item.Buyer_Amt.ToString()) ? Convert.ToDouble(item.Buyer_Amt.ToString()) : null : null),
                            (item.Expected_Final_Disc != null ? !string.IsNullOrEmpty(item.Expected_Final_Disc.ToString()) ? Convert.ToDouble(item.Expected_Final_Disc.ToString()) : null : null),
                            (item.Expected_Final_Amt != null ? !string.IsNullOrEmpty(item.Expected_Final_Amt.ToString()) ? Convert.ToDouble(item.Expected_Final_Amt.ToString()) : null : null));
                        }
                    }

                    var (message, result, msg) = await _cartService.Create_Approved_Management(dataTable, approval_Management.Id, approval_Management.User_Id ?? 0, approval_Management.Remarks, approval_Management.Status);
                    if ((message == "exist" && msg.Length > 0) || (message == "success" && msg.Length > 0))
                    {
                        // if alredy exists stone add again then message should show succsessfully added.
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = msg

                        });
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Approved_Management", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_order_processing")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Order_Processing(Order_Processing order_Processing)
        {
            try
            {
                IList<Order_Processing_Detail> OrderResult = JsonConvert.DeserializeObject<IList<Order_Processing_Detail>>(order_Processing.Order_Detail.ToString());

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Supp_Stock_Id", typeof(int));
                dataTable.Columns.Add("Buyer_Disc", typeof(double));
                dataTable.Columns.Add("Buyer_Amt", typeof(double));
                dataTable.Columns.Add("Base_Disc", typeof(double));
                dataTable.Columns.Add("Base_Amt", typeof(double));
                dataTable.Columns.Add("Offer_Disc", typeof(double));
                dataTable.Columns.Add("Offer_Amt", typeof(double));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("QC_Remarks", typeof(string));

                foreach (var item in OrderResult)
                {
                    dataTable.Rows.Add(item.Id, item.Supp_Stock_Id,
                        (item.Buyer_Disc != null ? !string.IsNullOrEmpty(item.Buyer_Disc.ToString()) ? Convert.ToDouble(item.Buyer_Disc.ToString()) : null : null),
                        (item.Buyer_Amt != null ? !string.IsNullOrEmpty(item.Buyer_Amt.ToString()) ? Convert.ToDouble(item.Buyer_Amt.ToString()) : null : null),
                        (item.Base_Disc != null ? !string.IsNullOrEmpty(item.Base_Disc.ToString()) ? Convert.ToDouble(item.Base_Disc.ToString()) : null : null),
                        (item.Base_Amt != null ? !string.IsNullOrEmpty(item.Base_Amt.ToString()) ? Convert.ToDouble(item.Base_Amt.ToString()) : null : null),
                        (item.Offer_Disc != null ? !string.IsNullOrEmpty(item.Offer_Disc.ToString()) ? Convert.ToDouble(item.Offer_Disc.ToString()) : null : null),
                        (item.Offer_Amt != null ? !string.IsNullOrEmpty(item.Offer_Amt.ToString()) ? Convert.ToDouble(item.Offer_Amt.ToString()) : null : null),
                        Convert.ToString(item.Status), Convert.ToString(item.QC_Remarks));
                }

                var (message, result, msg) = await _cartService.Create_Update_Order_Processing(dataTable, order_Processing.Id, order_Processing.User_Id, order_Processing.Customer_Name, order_Processing.Remarks, order_Processing.Status);
                if ((message == "exist" && msg.Length > 0) || (message == "success" && msg.Length > 0))
                {
                    // if alredy exists stone add again then message should show succsessfully added.
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = msg

                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Order_Processing", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("order_processing_inactive")]
        [Authorize]
        public async Task<IActionResult> Order_Processing_Inactive(Order_Processing_Inactive order_processing)
        {
            try
            {
                var result = await _cartService.Order_Processing_Inactive(order_processing);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.OrderInactive
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Order_Processing_Inactive", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("cart_approval_order_excel_download")]
        [Authorize]
        public async Task<IActionResult> Cart_Approval_Order_Excel_Download(Report_Filter report_Filter)
        {
            try
            {
                var dt_stock = await _supplierService.Get_Report_Search_Excel(report_Filter.id, report_Filter.Report_Filter_Parameter);
                if (dt_stock != null && dt_stock.Rows.Count > 0)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var result = await _supplierService.Get_Report_Users_Role(report_Filter.id, (int)user_Id, null);
                    List<string> columnNames = new List<string>();
                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            if (item.ContainsKey("Display_Name") && item.ContainsKey("Display_Type") && item["Display_Type"].ToString() == "D")
                            {
                                columnNames.Add(item["Display_Name"].ToString().Trim());
                            }
                        }
                    }
                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;
                    if (report_Filter.id == 2)
                    {
                        filename = "Cart_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Cart_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    else if (report_Filter.id == 3)
                    {
                        filename = "Approval_Management_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Approval_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    else if (report_Filter.id == 4)
                    {
                        filename = "Order_Processing_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Order_Processing_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Cart_Approval_Order_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("cart_approval_order_column_wise_excel_download")]
        [Authorize]
        public async Task<IActionResult> Cart_Approval_Order_Column_Wise_Excel_Download(Report_Filter report_Filter)
        {
            try
            {
                var dt_stock = await _supplierService.Get_Report_Search_Excel(report_Filter.id, report_Filter.Report_Filter_Parameter);
                if (dt_stock != null && dt_stock.Rows.Count > 0)
                {

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in report_Filter.column_Name)
                    {
                        if (columnName != "CERTIFICATE LINK")
                        {
                            columnNamesTable.Rows.Add(columnName);
                        }
                    }
                    columnNamesTable.Rows.Add("CERTIFICATE LINK");
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;
                    if (report_Filter.id == 2)
                    {
                        filename = "Cart_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Cart_Column_Wise_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    if (report_Filter.id == 3)
                    {
                        filename = "Approval_Management_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Approval_Column_Wise_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    else if (report_Filter.id == 4)
                    {
                        filename = "Order_Processing_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Order_Processing_Column_Wise_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                        excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    }
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Cart_Approval_Order_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("stock_availability_report_excel_download")]
        [Authorize]
        public async Task<IActionResult> Stock_Availability_Report_Excel_Download([FromForm] Stock_Avalibility stock_Avalibility, IFormFile? File_Location)
        {
            try
            {
                IList<Stock_Avalibility_Values> stock_Avalibility_Values = new List<Stock_Avalibility_Values>();
                var lst_Stock_Id = new List<string>();
                if (!string.IsNullOrEmpty(stock_Avalibility.stock_Id) && stock_Avalibility.stock_Id.Contains(";"))
                {
                    lst_Stock_Id = stock_Avalibility.stock_Id.Split('/').ToList();
                    if (lst_Stock_Id != null && lst_Stock_Id.Count > 0)
                    {
                        foreach (var item in lst_Stock_Id)
                        {
                            var model = new Stock_Avalibility_Values();
                            var lst_stock_val = item.Split(';').ToList();
                            if (lst_stock_val != null && lst_stock_val.Count > 0)
                            {
                                if (lst_stock_val.Count == 1)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(lst_stock_val[i]))
                                            continue;

                                        model.Stock_Id = lst_stock_val[0];
                                    }
                                }
                                else if (lst_stock_val.Count == 2)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                        }
                                    }
                                }
                                else if (lst_stock_val.Count == 3)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                            //var con_val_1 = Convert.ToDecimal(lst_stock_val[2]);
                                            //if (con_val_1 >= -100 && con_val_1 <= 100)
                                            //{
                                            //    model.Offer_Disc = lst_stock_val[2];
                                            //}
                                            //else
                                            //{
                                            //    model.Offer_Amount = lst_stock_val[2];
                                            //}
                                        }
                                    }
                                }
                            }
                            stock_Avalibility_Values.Add(model);
                        }
                        stock_Avalibility_Values = stock_Avalibility_Values.GroupBy(x => x.Stock_Id).Select(x => x.First()).ToList();
                    }
                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                if (File_Location != null)
                {
                    var filePath = Path.GetTempFileName();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File_Location.CopyToAsync(stream);
                    }
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int startRow = 2;

                        HashSet<string> uniqueValues = new HashSet<string>();
                        for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string value = worksheet.Cells[row, 1].GetValue<string>();
                            string offer_amt = worksheet.Cells[row, 3].GetValue<string>();
                            if (!uniqueValues.Contains(value))
                            {
                                uniqueValues.Add(value);

                                dataTable.Rows.Add(value, !string.IsNullOrEmpty(offer_amt) ? offer_amt.Replace(",", "") : DBNull.Value, worksheet.Cells[row, 2].GetValue<string>());

                            }
                        }
                        var uniqueRows = dataTable.AsEnumerable()
                                  .GroupBy(row => row.Field<string>("STOCK_ID"))
                                  .Select(grp => grp.First())
                                  .CopyToDataTable();
                    }
                }
                else if (stock_Avalibility_Values != null && stock_Avalibility_Values.Count > 0)
                {
                    stock_Avalibility.stock_Id = null;
                    foreach (var item in stock_Avalibility_Values)
                    {
                        dataTable.Rows.Add(!string.IsNullOrEmpty(item.Stock_Id) ? item.Stock_Id : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Amount) ? item.Offer_Amount : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Disc) ? item.Offer_Disc : DBNull.Value);
                    }
                }

                var dt_stock = await _supplierService.Get_Stock_Availability_Report_Excel(dataTable, stock_Avalibility.stock_Id, stock_Avalibility.stock_Type);
                if (dt_stock != null && dt_stock.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_stock.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Stock_Availability_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Stock_Availability_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Stock_Availability_Report_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        //[HttpGet]
        //[Route("get_order_summary")]
        //[Authorize]
        //public async Task<IActionResult> Get_Order_Summary(int order_No)
        //{
        //    try
        //    {
        //        var result = await _cartService.Get_Order_Summary(order_No);
        //        if (result != null && result.Count > 0)
        //        {
        //            return Ok(new
        //            {
        //                statusCode = HttpStatusCode.OK,
        //                message = CoreCommonMessage.DataSuccessfullyFound,
        //                data = result
        //            });
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _commonService.InsertErrorLog(ex.Message, "Get_Order_Summary", ex.StackTrace);
        //        return StatusCode((int)HttpStatusCode.InternalServerError, new
        //        {
        //            message = ex.Message
        //        });
        //    }
        //}
        #endregion

        #region Get GIA Certificate Data
        [HttpGet]
        [Route("get_gia_cert_data")]
        [Authorize]
        public async Task<IActionResult> Get_GIA_Cert_Data(string cert_no, string report_Date)
        {
            var key = _configuration["Sunrise_Key"];
            try
            {
                if (!string.IsNullOrEmpty(report_Date))
                {
                    var result = await _supplierService.GIA_Lab_Parameter(report_Date);
                    if (result != null && result.Count > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = result
                        });
                    }
                    return NoContent();
                }
                else
                {
                    var query = @"
                             query ReportQuery($ReportNumber: String!) {
                                getReport(report_number: $ReportNumber){
                                    report_number
                                    report_date
                                    results {
                                        __typename
                                        ... on DiamondGradingReportResults {
                                             measurements
                                             carat_weight
                                             color_grade
                                             color_origin
                                             color_distribution
                                             clarity_grade
                                             cut_grade
                                             polish
                                             symmetry
                                             fluorescence
                                             clarity_characteristics
                                             key_to_symbols
                                             {
                                                characteristic
                                             }
                                             inscriptions
                                             report_comments
                                             proportions {
                                                      depth_pct
                                                      table_pct
                                                      crown_angle
                                                      crown_height
                                                      pavilion_angle
                                                      pavilion_depth
                                                      star_length
                                                      lower_half
                                                      girdle
                                                      culet
                                                    }
                                              data {
                                                    shape {
                                                        shape_category
                                                        shape_code
                                                        shape_group
                                                        shape_group_code
                                                        }
                                                        weight {
                                                            weight
                                                            weight_unit
                                                        }
                                                        color {
                                                            color_grade_code
                                                            color_modifier
                                                        }
                                                        clarity
                                                        cut
                                                        polish
                                                        symmetry
                                                        fluorescence {
                                                            fluorescence_intensity
                                                            fluorescence_color
                                                        }
                                                        girdle {
                                                            girdle_condition
                                                            girdle_condition_code
                                                            girdle_pct
                                                            girdle_size
                                                            girdle_size_code
                                                        }
                                                        culet {
                                                            culet_code
                                                        }
                                                    }
                                                }
                                            }
                                            links {
                                                pdf
                                                proportions_diagram
                                                plotting_diagram
                                                digital_card
                                            }
                                    quota {
                                        remaining
                                    }
                                }
                            }
                            ";

                    var lst_Cert_No = cert_no.Split(",").ToList();
                    DataTable dataTable = new DataTable();
                    #region Set Column In Datatable
                    dataTable.Columns.Add("Report_No", typeof(string));
                    dataTable.Columns.Add("Report_Date", typeof(string));
                    dataTable.Columns.Add("Length", typeof(float));
                    dataTable.Columns.Add("Width", typeof(float));
                    dataTable.Columns.Add("Depth", typeof(float));
                    dataTable.Columns.Add("Color_Grade", typeof(string));
                    dataTable.Columns.Add("Clarity_Grade", typeof(string));
                    dataTable.Columns.Add("Cut_Grade", typeof(string));
                    dataTable.Columns.Add("Polish_Grade", typeof(string));
                    dataTable.Columns.Add("Symmetry_Grade", typeof(string));
                    dataTable.Columns.Add("Fluorescence", typeof(string));
                    dataTable.Columns.Add("Inscriptions", typeof(string));
                    dataTable.Columns.Add("Key_To_Symbols", typeof(string));
                    dataTable.Columns.Add("Report_Comments", typeof(string));
                    dataTable.Columns.Add("Table_Pct", typeof(float));
                    dataTable.Columns.Add("Depth_Pct", typeof(float));
                    dataTable.Columns.Add("Crown_Angle", typeof(float));
                    dataTable.Columns.Add("Crown_Height", typeof(float));
                    dataTable.Columns.Add("Pavilion_Angle", typeof(float));
                    dataTable.Columns.Add("Pavilion_Depth", typeof(float));
                    dataTable.Columns.Add("Star_Length", typeof(float));
                    dataTable.Columns.Add("Lower_Half", typeof(float));
                    dataTable.Columns.Add("Shape_Code", typeof(string));
                    dataTable.Columns.Add("Shape_Group", typeof(string));
                    dataTable.Columns.Add("Carats", typeof(float));
                    dataTable.Columns.Add("Clarity", typeof(string));
                    dataTable.Columns.Add("Cut", typeof(string));
                    dataTable.Columns.Add("Polish", typeof(string));
                    dataTable.Columns.Add("Symmetry", typeof(string));
                    dataTable.Columns.Add("Fluorescence_Intensity", typeof(string));
                    dataTable.Columns.Add("Fluorescence_Color");
                    dataTable.Columns.Add("Girdle_Condition", typeof(string));
                    dataTable.Columns.Add("Girdle_Condition_Code", typeof(string));
                    dataTable.Columns.Add("Girdle_Pct", typeof(float));
                    dataTable.Columns.Add("Girdle_Size", typeof(string));
                    dataTable.Columns.Add("Girdle_Size_Code", typeof(string));
                    dataTable.Columns.Add("Culet_Code", typeof(string));
                    dataTable.Columns.Add("Certificate_PDF", typeof(string));
                    dataTable.Columns.Add("Plotting_Diagram", typeof(string));
                    dataTable.Columns.Add("Proportions_Diagram", typeof(string));
                    dataTable.Columns.Add("Digital_Card", typeof(string));
                    #endregion
                    bool success = false;
                    string certificate_no = "";
                    if (lst_Cert_No != null && lst_Cert_No.Count > 0)
                    {
                        foreach (var item in lst_Cert_No)
                        {
                            certificate_no = item;
                            var query_variables = new Dictionary<string, string>
                            {
                                { "ReportNumber", item}
                            };

                            var body = new Dictionary<string, object>
                            {
                                { "query", query },
                                { "variables", query_variables }
                            };

                            string json = System.Text.Json.JsonSerializer.Serialize(body);
                            var client = new WebClient();
                            string url = "https://api.reportresults.gia.edu";

                            client.Headers.Add(HttpRequestHeader.Authorization, key);
                            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                            var responseData = client.UploadString(url, json);

                            GIA_Report gIA_Report = JsonConvert.DeserializeObject<GIA_Report>(responseData);
                            if (gIA_Report != null && gIA_Report.Data.GetReport != null && !string.IsNullOrEmpty(gIA_Report.Data.GetReport.ReportNumber))
                            {
                                DataRow dataRow = dataTable.NewRow();
                                var measurements = gIA_Report.Data.GetReport.Results.Measurements;
                                string[] arrMeasurements = null;
                                arrMeasurements = measurements.Split('-', 'x');

                                dataRow["Report_No"] = gIA_Report.Data.GetReport.ReportNumber;
                                dataRow["Report_Date"] = Convert.ToDateTime(gIA_Report.Data.GetReport.ReportDate).ToString("dd-MM-yyyy");
                                if (arrMeasurements != null && arrMeasurements.Length == 3)
                                {
                                    var length = arrMeasurements[1].ToString().Replace("mm", "").Trim();
                                    var width = arrMeasurements[0].ToString().Replace("mm", "").Trim();
                                    var depth = arrMeasurements[2].ToString().Replace("mm", "").Trim();
                                    dataRow["Length"] = float.Parse(length);
                                    dataRow["Width"] = float.Parse(width);
                                    dataRow["Depth"] = float.Parse(depth);
                                }
                                else
                                {
                                    dataRow["Length"] = DBNull.Value;
                                    dataRow["Width"] = DBNull.Value;
                                    dataRow["Depth"] = DBNull.Value;
                                }
                                dataRow["Color_Grade"] = gIA_Report.Data.GetReport.Results.ColorGrade;
                                dataRow["Clarity_Grade"] = gIA_Report.Data.GetReport.Results.ClarityGrade;
                                dataRow["Cut_Grade"] = gIA_Report.Data.GetReport.Results.CutGrade;
                                dataRow["Polish_Grade"] = gIA_Report.Data.GetReport.Results.Polish;
                                dataRow["Symmetry_Grade"] = gIA_Report.Data.GetReport.Results.Symmetry;
                                dataRow["Fluorescence"] = gIA_Report.Data.GetReport.Results.Fluorescence;
                                dataRow["Inscriptions"] = gIA_Report.Data.GetReport.Results.Inscriptions;
                                if (gIA_Report.Data.GetReport.Results.KeyToSymbols != null)
                                {
                                    string keyToSymbols = "";
                                    foreach (var obj in gIA_Report.Data.GetReport.Results.KeyToSymbols)
                                    {
                                        keyToSymbols += (keyToSymbols.Length == 0 ? "" : ", ") + obj.characteristic;
                                    }
                                    dataRow["Key_To_Symbols"] = keyToSymbols;
                                }
                                else
                                {
                                    dataRow["Key_To_Symbols"] = DBNull.Value;
                                }
                                dataRow["Report_Comments"] = gIA_Report.Data.GetReport.Results.ReportComments;
                                if (gIA_Report.Data.GetReport.Results.Proportions != null)
                                {
                                    dataRow["Table_Pct"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.TablePct) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.TablePct) : DBNull.Value;
                                    dataRow["Depth_Pct"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.DepthPct) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.DepthPct) : DBNull.Value;
                                    dataRow["Crown_Angle"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.CrownAngle) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.CrownAngle) : DBNull.Value;
                                    dataRow["Crown_Height"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.CrownHeight) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.CrownHeight) : DBNull.Value;
                                    dataRow["Pavilion_Angle"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.PavilionAngle) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.PavilionAngle) : DBNull.Value;
                                    dataRow["Pavilion_Depth"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.PavilionDepth) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.PavilionDepth) : DBNull.Value;
                                    dataRow["Star_Length"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.StarLength) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.StarLength) : DBNull.Value;
                                    dataRow["Lower_Half"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Proportions.LowerHalf) ? float.Parse(gIA_Report.Data.GetReport.Results.Proportions.LowerHalf) : DBNull.Value;
                                }
                                else
                                {
                                    dataRow["Table_Pct"] = DBNull.Value;
                                    dataRow["Depth_Pct"] = DBNull.Value;
                                    dataRow["Crown_Angle"] = DBNull.Value;
                                    dataRow["Crown_Height"] = DBNull.Value;
                                    dataRow["Pavilion_Angle"] = DBNull.Value;
                                    dataRow["Pavilion_Depth"] = DBNull.Value;
                                    dataRow["Star_Length"] = DBNull.Value;
                                    dataRow["Lower_Half"] = DBNull.Value;
                                }
                                if (gIA_Report.Data.GetReport.Results.Data.Shape != null)
                                {
                                    dataRow["Shape_Code"] = gIA_Report.Data.GetReport.Results.Data.Shape.ShapeCode;
                                    dataRow["Shape_Group"] = gIA_Report.Data.GetReport.Results.Data.Shape.ShapeGroup;
                                }
                                else
                                {
                                    dataRow["Shape_Code"] = DBNull.Value;
                                    dataRow["Shape_Group"] = DBNull.Value;
                                }
                                dataRow["Carats"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Data.Weight.WeightWeight) ? float.Parse(gIA_Report.Data.GetReport.Results.Data.Weight.WeightWeight) : DBNull.Value;
                                dataRow["Clarity"] = gIA_Report.Data.GetReport.Results.Data.Clarity;
                                dataRow["Cut"] = gIA_Report.Data.GetReport.Results.Data.Cut;
                                dataRow["Polish"] = gIA_Report.Data.GetReport.Results.Data.Polish;
                                dataRow["Symmetry"] = gIA_Report.Data.GetReport.Results.Data.Symmetry;
                                if (gIA_Report.Data.GetReport.Results.Data.Fluorescence != null)
                                {
                                    dataRow["Fluorescence_Intensity"] = gIA_Report.Data.GetReport.Results.Data.Fluorescence.FluorescenceIntensity;
                                    dataRow["Fluorescence_Color"] = gIA_Report.Data.GetReport.Results.Data.Fluorescence.FluorescenceColor;
                                }
                                else
                                {
                                    dataRow["Fluorescence_Intensity"] = DBNull.Value;
                                    dataRow["Fluorescence_Color"] = DBNull.Value;
                                }
                                if (gIA_Report.Data.GetReport.Results.Data.Girdle != null)
                                {
                                    dataRow["Girdle_Condition"] = gIA_Report.Data.GetReport.Results.Data.Girdle.GirdleCondition;
                                    dataRow["Girdle_Condition_Code"] = gIA_Report.Data.GetReport.Results.Data.Girdle.GirdleConditionCode;
                                    dataRow["Girdle_Pct"] = !string.IsNullOrEmpty(gIA_Report.Data.GetReport.Results.Data.Girdle.GirdlePct) ? float.Parse(gIA_Report.Data.GetReport.Results.Data.Girdle.GirdlePct) : DBNull.Value;
                                    dataRow["Girdle_Size"] = gIA_Report.Data.GetReport.Results.Data.Girdle.GirdleSize;
                                    dataRow["Girdle_Size_Code"] = gIA_Report.Data.GetReport.Results.Data.Girdle.GirdleSizeCode;
                                }
                                else
                                {
                                    dataRow["Girdle_Condition"] = DBNull.Value;
                                    dataRow["Girdle_Condition_Code"] = DBNull.Value;
                                    dataRow["Girdle_Pct"] = DBNull.Value;
                                    dataRow["Girdle_Size"] = DBNull.Value;
                                    dataRow["Girdle_Size_Code"] = DBNull.Value;
                                }
                                dataRow["Culet_Code"] = gIA_Report.Data.GetReport.Results.Data.Culet.CuletCode;
                                if (gIA_Report.Data.GetReport.Links != null)
                                {
                                    dataRow["Certificate_PDF"] = gIA_Report.Data.GetReport.Links.Pdf;
                                    dataRow["Plotting_Diagram"] = gIA_Report.Data.GetReport.Links.PlottingDiagram;
                                    dataRow["Proportions_Diagram"] = gIA_Report.Data.GetReport.Links.ProportionsDiagram;
                                    dataRow["Digital_Card"] = gIA_Report.Data.GetReport.Links.DigitalCard;
                                }
                                else
                                {
                                    dataRow["Certificate_PDF"] = DBNull.Value;
                                    dataRow["Plotting_Diagram"] = DBNull.Value;
                                    dataRow["Proportions_Diagram"] = DBNull.Value;
                                    dataRow["Digital_Card"] = DBNull.Value;
                                }
                                dataTable.Rows.Add(dataRow);
                                success = true;
                                if (lst_Cert_No.Count > 1)
                                {
                                    certificate_no += ", " + certificate_no;
                                }
                            }
                            else
                            {
                                success = false;
                            }
                        }

                        var result = new List<Dictionary<string, object>>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var dict = new Dictionary<string, object>();
                            foreach (DataColumn col in dataTable.Columns)
                            {
                                if (row[col] == DBNull.Value)
                                {
                                    dict[col.ColumnName] = null;
                                }
                                else
                                {
                                    dict[col.ColumnName] = row[col];
                                }
                            }
                            result.Add(dict);
                        }
                        if (success)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.DataSuccessfullyFound,
                                data = result
                            });
                        }
                        else if (!success)
                        {
                            return NoContent();
                        }
                    }
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_GIA_Cert_Data", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("save_gia_lab_cert_data")]
        [Authorize]
        public async Task<IActionResult> Save_GIA_Lab_Cert_Data(IList<GIA_Lab_Parameter> gIA_Lab_Parameters)
        {
            try
            {
                if (gIA_Lab_Parameters != null && gIA_Lab_Parameters.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable = Set_GIA_Lab_Parameter_Column_In_Datatable(new DataTable(), gIA_Lab_Parameters);
                    var result = await _supplierService.Insert_GIA_Lab_Parameter(dataTable);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "GIA lab parameter saved successfully."
                        });
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Save_GIA_Lab_Cert_Data", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Create Stock In Excel Format
        [HttpPost]
        [Route("export_stock_excel_new")]
        [Authorize]
        public async Task<IActionResult> Export_Stock_Excel_New(Excel_Model excel_Model)
        {
            //DataTable supp_stock_dt = await _supplierService.Get_Stock_In_Datatable(excel_Model.supplier_Ref_No, excel_Model.excel_Format);
            DataTable supp_stock_dt = await _supplierService.Get_Excel_Report_Search_New(excel_Model.Report_Filter_Parameter, excel_Model.excel_Format, excel_Model.supplier_Ref_No);
            if (supp_stock_dt != null && supp_stock_dt.Rows.Count > 0)
            {
                List<string> columnNames = new List<string>();
                foreach (DataColumn column in supp_stock_dt.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }

                DataTable columnNamesTable = new DataTable();
                columnNamesTable.Columns.Add("Column_Name", typeof(string));

                foreach (string columnName in columnNames)
                {
                    columnNamesTable.Rows.Add(columnName);
                }
                var excelPath = string.Empty;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                if (!(Directory.Exists(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }
                string filename = string.Empty;
                if (excel_Model.excel_Format == "Customer")
                {
                    filename = "Customer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Customer_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                else if (excel_Model.excel_Format == "Buyer")
                {
                    filename = "Buyer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Buyer_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                else if (excel_Model.excel_Format == "Supplier")
                {
                    filename = "Supplier_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Supplier_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    result = excelPath,
                    file_name = filename
                });
            }
            return NoContent();
        }

        [HttpPost]
        [Route("export_stock_excel")]
        [Authorize]
        public async Task<IActionResult> Export_Stock_Excel(Excel_Model_New excel_Model)
        {


            string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                        CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                        POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                        LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                        IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                        DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                        CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                        RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                        OFFER_DISC = string.Empty, OFFER_VALUE = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                        PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                        BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                        TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                        GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                        KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                        FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                        STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                        SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty,
                        BID_SUPP_STOCK_ID = string.Empty, BID_BUYER_DISC = string.Empty, BID_BUYER_AMOUNT = string.Empty,
                        LUSTER = string.Empty, SHADE = string.Empty, MILKY = string.Empty, GIRDLE_FROM = string.Empty, GIRDLE_TO = string.Empty, GIRDLE_CONDITION = string.Empty, ORIGIN = string.Empty,
                        EXTRA_FACET_TABLE = string.Empty, EXTRA_FACET_CROWN = string.Empty, EXTRA_FACET_PAV = string.Empty,
                        INTERNAL_GRAINING = string.Empty, H_A = string.Empty, SUPPLIER_COMMENTS = string.Empty;

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("STOCK_ID", typeof(string));
            dataTable.Columns.Add("SUPPLIER", typeof(string));
            dataTable.Columns.Add("CUSTOMER", typeof(string));
            dataTable.Columns.Add("SHAPE", typeof(string));
            dataTable.Columns.Add("CTS", typeof(string));
            dataTable.Columns.Add("COLOR", typeof(string));
            dataTable.Columns.Add("CLARITY", typeof(string));
            dataTable.Columns.Add("CUT", typeof(string));
            dataTable.Columns.Add("POLISH", typeof(string));
            dataTable.Columns.Add("SYMM", typeof(string));
            dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
            dataTable.Columns.Add("BGM", typeof(string));
            dataTable.Columns.Add("LAB", typeof(string));
            dataTable.Columns.Add("GOOD_TYPE", typeof(string));
            dataTable.Columns.Add("LOCATION", typeof(string));
            dataTable.Columns.Add("STATUS", typeof(string));
            dataTable.Columns.Add("IMAGE_LINK", typeof(string));
            dataTable.Columns.Add("VIDEO_LINK", typeof(string));
            dataTable.Columns.Add("LENGTH", typeof(string));
            dataTable.Columns.Add("WIDTH", typeof(string));
            dataTable.Columns.Add("DEPTH", typeof(string));
            dataTable.Columns.Add("TABLE_PER", typeof(string));
            dataTable.Columns.Add("DEPTH_PER", typeof(string));
            dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
            dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
            dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
            dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
            dataTable.Columns.Add("GIRDLE_PER", typeof(string));
            dataTable.Columns.Add("RAP_RATE", typeof(string));
            dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
            dataTable.Columns.Add("COST_DISC", typeof(string));
            dataTable.Columns.Add("COST_AMOUNT", typeof(string));
            dataTable.Columns.Add("OFFER_DISC", typeof(string));
            dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
            dataTable.Columns.Add("BASE_DISC", typeof(string));
            dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
            dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
            dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
            dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
            dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
            dataTable.Columns.Add("BUYER_DISC", typeof(string));
            dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
            dataTable.Columns.Add("TABLE_BLACK", typeof(string));
            dataTable.Columns.Add("TABLE_WHITE", typeof(string));
            dataTable.Columns.Add("SIDE_BLACK", typeof(string));
            dataTable.Columns.Add("SIDE_WHITE", typeof(string));
            dataTable.Columns.Add("TABLE_OPEN", typeof(string));
            dataTable.Columns.Add("CROWN_OPEN", typeof(string));
            dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
            dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
            dataTable.Columns.Add("CULET", typeof(string));
            dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
            dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
            dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
            dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
            dataTable.Columns.Add("FANCY_COLOR", typeof(string));
            dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
            dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
            dataTable.Columns.Add("POINTER", typeof(string));
            dataTable.Columns.Add("SUB_POINTER", typeof(string));
            dataTable.Columns.Add("STAR_LN", typeof(string));
            dataTable.Columns.Add("LR_HALF", typeof(string));
            dataTable.Columns.Add("CERT_TYPE", typeof(string));
            dataTable.Columns.Add("COST_RATE", typeof(string));
            dataTable.Columns.Add("RATIO", typeof(string));
            dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
            dataTable.Columns.Add("SIGN", typeof(string));
            dataTable.Columns.Add("DISC_VALUE", typeof(string));
            dataTable.Columns.Add("DISC_VALUE1", typeof(string));
            dataTable.Columns.Add("DISC_VALUE2", typeof(string));
            dataTable.Columns.Add("DISC_VALUE3", typeof(string));
            dataTable.Columns.Add("BASE_TYPE", typeof(string));
            dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
            dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
            dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
            dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
            dataTable.Columns.Add("LUSTER", typeof(string));
            dataTable.Columns.Add("SHADE", typeof(string));
            dataTable.Columns.Add("MILKY", typeof(string));
            dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
            dataTable.Columns.Add("GIRDLE_TO", typeof(string));
            dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
            dataTable.Columns.Add("ORIGIN", typeof(string));
            dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
            dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
            dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
            dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
            dataTable.Columns.Add("H_A", typeof(string));
            dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));

            var Round_Cat_Val_Id = _configuration["Round_Cat_Val_Id"];
            bool Is_Round = false;
            if (excel_Model.Report_Filter_Parameter_List != null && excel_Model.Report_Filter_Parameter_List.Count == 1)
            {
                foreach (var item in excel_Model.Report_Filter_Parameter_List)
                {
                    SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();

                    if (item.Count == 1 && SHAPE == Round_Cat_Val_Id)
                    {
                        Is_Round = true;
                    }
                }
            }
            if (excel_Model.Report_Filter_Parameter_List != null && excel_Model.Report_Filter_Parameter_List.Count > 0)
            {
                foreach (var item in excel_Model.Report_Filter_Parameter_List)
                {
                    STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                    SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER" || x.Column_Name == "COMPANY").Select(x => x.Category_Value).FirstOrDefault();
                    CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Value).FirstOrDefault();
                    SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();
                    CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                    COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Value).FirstOrDefault();
                    CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Value).FirstOrDefault();
                    CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Value).FirstOrDefault();
                    POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Value).FirstOrDefault();
                    SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Value).FirstOrDefault();
                    FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                    BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Value).FirstOrDefault();
                    LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Value).FirstOrDefault();
                    GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Value).FirstOrDefault();
                    LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Value).FirstOrDefault();
                    STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Value).FirstOrDefault();
                    IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                    VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                    LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                    WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                    DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                    TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                    DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                    CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                    CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                    PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                    PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                    GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                    RAP_RATE = item.Where(x => x.Column_Name == "RAP RATE").Select(x => x.Category_Value).FirstOrDefault();
                    RAP_AMOUNT = item.Where(x => x.Column_Name == "RAP AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    COST_DISC = item.Where(x => x.Column_Name == "COST DISC").Select(x => x.Category_Value).FirstOrDefault();
                    COST_AMOUNT = item.Where(x => x.Column_Name == "COST AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                    OFFER_VALUE = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    BASE_DISC = item.Where(x => x.Column_Name == "BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                    BASE_AMOUNT = item.Where(x => x.Column_Name == "BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                    MAX_SLAB_BASE_DISC = item.Where(x => x.Column_Name == "MAX SLAB BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                    MAX_SLAB_BASE_AMOUNT = item.Where(x => x.Column_Name == "MAX SLAB BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    MAX_SLAB_PRICE_PER_CTS = item.Where(x => x.Column_Name == "MAX SLAB PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                    BUYER_DISC = item.Where(x => x.Column_Name == "BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                    BUYER_AMOUNT = item.Where(x => x.Column_Name == "BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                    TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                    SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                    SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                    TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                    CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Value).FirstOrDefault();
                    PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Value).FirstOrDefault();
                    GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                    CULET = item.Where(x => x.Column_Name == "CULET").Select(x => x.Category_Value).FirstOrDefault();
                    KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                    KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                    LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                    LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                    FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Value).FirstOrDefault();
                    FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                    FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Value).FirstOrDefault();
                    POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Value).FirstOrDefault();
                    SUB_POINTER = item.Where(x => x.Column_Name == "SUB POINTER").Select(x => x.Category_Value).FirstOrDefault();
                    STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Value).FirstOrDefault();
                    LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Value).FirstOrDefault();
                    CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                    COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                    RATIO = item.Where(x => x.Column_Name == "RATIO").Select(x => x.Category_Value).FirstOrDefault();
                    DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                    SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                    DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                    DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                    DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                    DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                    BASE_TYPE = item.Where(x => x.Column_Name == "BASE TYPE").Select(x => x.Category_Value).FirstOrDefault();
                    SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                    BID_SUPP_STOCK_ID = item.Where(x => x.Column_Name == "BID SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                    BID_BUYER_DISC = item.Where(x => x.Column_Name == "BID BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                    BID_BUYER_AMOUNT = item.Where(x => x.Column_Name == "BID BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                    LUSTER = item.Where(x => x.Column_Name == "LUSTER").Select(x => x.Category_Value).FirstOrDefault();
                    SHADE = item.Where(x => x.Column_Name == "SHADE").Select(x => x.Category_Value).FirstOrDefault();
                    MILKY = item.Where(x => x.Column_Name == "MILKY").Select(x => x.Category_Value).FirstOrDefault();
                    GIRDLE_FROM = item.Where(x => x.Column_Name == "GIRDLE_FROM").Select(x => x.Category_Value).FirstOrDefault();
                    GIRDLE_TO = item.Where(x => x.Column_Name == "GIRDLE_TO").Select(x => x.Category_Value).FirstOrDefault();
                    GIRDLE_CONDITION = item.Where(x => x.Column_Name == "GIRDLE_CONDITION").Select(x => x.Category_Value).FirstOrDefault();
                    ORIGIN = item.Where(x => x.Column_Name == "ORIGIN").Select(x => x.Category_Value).FirstOrDefault();
                    EXTRA_FACET_TABLE = item.Where(x => x.Column_Name == "EXTRA_FACET_TABLE").Select(x => x.Category_Value).FirstOrDefault();
                    EXTRA_FACET_CROWN = item.Where(x => x.Column_Name == "EXTRA_FACET_CROWN").Select(x => x.Category_Value).FirstOrDefault();
                    EXTRA_FACET_PAV = item.Where(x => x.Column_Name == "EXTRA_FACET_PAV").Select(x => x.Category_Value).FirstOrDefault();
                    INTERNAL_GRAINING = item.Where(x => x.Column_Name == "INTERNAL_GRAINING").Select(x => x.Category_Value).FirstOrDefault();
                    H_A = item.Where(x => x.Column_Name == "H_A").Select(x => x.Category_Value).FirstOrDefault();
                    SUPPLIER_COMMENTS = item.Where(x => x.Column_Name == "SUPPLIER_COMMENTS").Select(x => x.Category_Value).FirstOrDefault();

                    dataTable.Rows.Add(!string.IsNullOrEmpty(STOCK_ID) ? STOCK_ID : DBNull.Value,
                        !string.IsNullOrEmpty(SUPPLIER) ? SUPPLIER : DBNull.Value,
                        !string.IsNullOrEmpty(CUSTOMER) ? CUSTOMER : DBNull.Value,
                        !string.IsNullOrEmpty(SHAPE) ? SHAPE : DBNull.Value,
                        !string.IsNullOrEmpty(CTS) ? CTS : DBNull.Value,
                        !string.IsNullOrEmpty(COLOR) ? COLOR : DBNull.Value,
                        !string.IsNullOrEmpty(CLARITY) ? CLARITY : DBNull.Value,
                        !string.IsNullOrEmpty(CUT) ? CUT : DBNull.Value,
                        !string.IsNullOrEmpty(POLISH) ? POLISH : DBNull.Value,
                        !string.IsNullOrEmpty(SYMM) ? SYMM : DBNull.Value,
                        !string.IsNullOrEmpty(FLS_INTENSITY) ? FLS_INTENSITY : DBNull.Value,
                        !string.IsNullOrEmpty(BGM) ? BGM : DBNull.Value,
                        !string.IsNullOrEmpty(LAB) ? LAB : DBNull.Value,
                        !string.IsNullOrEmpty(GOOD_TYPE) ? GOOD_TYPE : DBNull.Value,
                        !string.IsNullOrEmpty(LOCATION) ? LOCATION : DBNull.Value,
                        !string.IsNullOrEmpty(STATUS) ? STATUS : DBNull.Value,
                        !string.IsNullOrEmpty(IMAGE_LINK) ? IMAGE_LINK : DBNull.Value,
                        !string.IsNullOrEmpty(VIDEO_LINK) ? VIDEO_LINK : DBNull.Value,
                        !string.IsNullOrEmpty(LENGTH) ? LENGTH : DBNull.Value,
                        !string.IsNullOrEmpty(WIDTH) ? WIDTH : DBNull.Value,
                        !string.IsNullOrEmpty(DEPTH) ? DEPTH : DBNull.Value,
                        !string.IsNullOrEmpty(TABLE_PER) ? TABLE_PER : DBNull.Value,
                        !string.IsNullOrEmpty(DEPTH_PER) ? DEPTH_PER : DBNull.Value,
                        !string.IsNullOrEmpty(CROWN_ANGLE) ? CROWN_ANGLE : DBNull.Value,
                        !string.IsNullOrEmpty(CROWN_HEIGHT) ? CROWN_HEIGHT : DBNull.Value,
                        !string.IsNullOrEmpty(PAVILION_ANGLE) ? PAVILION_ANGLE : DBNull.Value,
                        !string.IsNullOrEmpty(PAVILION_HEIGHT) ? PAVILION_HEIGHT : DBNull.Value,
                        !string.IsNullOrEmpty(GIRDLE_PER) ? GIRDLE_PER : DBNull.Value,
                        !string.IsNullOrEmpty(RAP_RATE) ? RAP_RATE : DBNull.Value,
                        !string.IsNullOrEmpty(RAP_AMOUNT) ? RAP_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(COST_DISC) ? COST_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(COST_AMOUNT) ? COST_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(OFFER_DISC) ? OFFER_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(OFFER_VALUE) ? OFFER_VALUE : DBNull.Value,
                        !string.IsNullOrEmpty(BASE_DISC) ? BASE_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(BASE_AMOUNT) ? BASE_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(PRICE_PER_CTS) ? PRICE_PER_CTS : DBNull.Value,
                        !string.IsNullOrEmpty(MAX_SLAB_BASE_DISC) ? MAX_SLAB_BASE_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(MAX_SLAB_BASE_AMOUNT) ? MAX_SLAB_BASE_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(MAX_SLAB_PRICE_PER_CTS) ? MAX_SLAB_PRICE_PER_CTS : DBNull.Value,
                        !string.IsNullOrEmpty(BUYER_DISC) ? BUYER_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(BUYER_AMOUNT) ? BUYER_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(TABLE_BLACK) ? TABLE_BLACK : DBNull.Value,
                        !string.IsNullOrEmpty(TABLE_WHITE) ? TABLE_WHITE : DBNull.Value,
                        !string.IsNullOrEmpty(SIDE_BLACK) ? SIDE_BLACK : DBNull.Value,
                        !string.IsNullOrEmpty(SIDE_WHITE) ? SIDE_WHITE : DBNull.Value,
                        !string.IsNullOrEmpty(TABLE_OPEN) ? TABLE_OPEN : DBNull.Value,
                        !string.IsNullOrEmpty(CROWN_OPEN) ? CROWN_OPEN : DBNull.Value,
                        !string.IsNullOrEmpty(PAVILION_OPEN) ? PAVILION_OPEN : DBNull.Value,
                        !string.IsNullOrEmpty(GIRDLE_OPEN) ? GIRDLE_OPEN : DBNull.Value,
                        !string.IsNullOrEmpty(CULET) ? CULET : DBNull.Value,
                        !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? KEY_TO_SYMBOL_TRUE : DBNull.Value,
                        !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? KEY_TO_SYMBOL_FALSE : DBNull.Value,
                        !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? LAB_COMMENTS_TRUE : DBNull.Value,
                        !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? LAB_COMMENTS_FALSE : DBNull.Value,
                        !string.IsNullOrEmpty(FANCY_COLOR) ? FANCY_COLOR : DBNull.Value,
                        !string.IsNullOrEmpty(FANCY_INTENSITY) ? FANCY_INTENSITY : DBNull.Value,
                        !string.IsNullOrEmpty(FANCY_OVERTONE) ? FANCY_OVERTONE : DBNull.Value,
                        !string.IsNullOrEmpty(POINTER) ? POINTER : DBNull.Value,
                        !string.IsNullOrEmpty(SUB_POINTER) ? SUB_POINTER : DBNull.Value,
                        !string.IsNullOrEmpty(STAR_LN) ? STAR_LN : DBNull.Value,
                        !string.IsNullOrEmpty(LR_HALF) ? LR_HALF : DBNull.Value,
                        !string.IsNullOrEmpty(CERT_TYPE) ? CERT_TYPE : DBNull.Value,
                        !string.IsNullOrEmpty(COST_RATE) ? COST_RATE : DBNull.Value,
                        !string.IsNullOrEmpty(RATIO) ? RATIO : DBNull.Value,
                        !string.IsNullOrEmpty(DISCOUNT_TYPE) ? DISCOUNT_TYPE : DBNull.Value,
                        !string.IsNullOrEmpty(SIGN) ? SIGN : DBNull.Value,
                        !string.IsNullOrEmpty(DISC_VALUE) ? DISC_VALUE : DBNull.Value,
                        !string.IsNullOrEmpty(DISC_VALUE1) ? DISC_VALUE1 : DBNull.Value,
                        !string.IsNullOrEmpty(DISC_VALUE2) ? DISC_VALUE2 : DBNull.Value,
                        !string.IsNullOrEmpty(DISC_VALUE3) ? DISC_VALUE3 : DBNull.Value,
                        !string.IsNullOrEmpty(BASE_TYPE) ? BASE_TYPE : DBNull.Value,
                        !string.IsNullOrEmpty(SUPP_STOCK_ID) ? SUPP_STOCK_ID : DBNull.Value,
                        !string.IsNullOrEmpty(BID_SUPP_STOCK_ID) ? BID_SUPP_STOCK_ID : DBNull.Value,
                        !string.IsNullOrEmpty(BID_BUYER_DISC) ? BID_BUYER_DISC : DBNull.Value,
                        !string.IsNullOrEmpty(BID_BUYER_AMOUNT) ? BID_BUYER_AMOUNT : DBNull.Value,
                        !string.IsNullOrEmpty(LUSTER) ? LUSTER : DBNull.Value,
                        !string.IsNullOrEmpty(SHADE) ? SHADE : DBNull.Value,
                        !string.IsNullOrEmpty(MILKY) ? MILKY : DBNull.Value,
                        !string.IsNullOrEmpty(GIRDLE_FROM) ? GIRDLE_FROM : DBNull.Value,
                        !string.IsNullOrEmpty(GIRDLE_TO) ? GIRDLE_TO : DBNull.Value,
                        !string.IsNullOrEmpty(GIRDLE_CONDITION) ? GIRDLE_CONDITION : DBNull.Value,
                        !string.IsNullOrEmpty(ORIGIN) ? ORIGIN : DBNull.Value,
                        !string.IsNullOrEmpty(EXTRA_FACET_TABLE) ? EXTRA_FACET_TABLE : DBNull.Value,
                        !string.IsNullOrEmpty(EXTRA_FACET_CROWN) ? EXTRA_FACET_CROWN : DBNull.Value,
                        !string.IsNullOrEmpty(EXTRA_FACET_PAV) ? EXTRA_FACET_PAV : DBNull.Value,
                        !string.IsNullOrEmpty(INTERNAL_GRAINING) ? INTERNAL_GRAINING : DBNull.Value,
                        !string.IsNullOrEmpty(H_A) ? H_A : DBNull.Value,
                        !string.IsNullOrEmpty(SUPPLIER_COMMENTS) ? SUPPLIER_COMMENTS : DBNull.Value);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(excel_Model.supplier_Ref_No))
                {
                    var destinationFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (excel_Model.excel_Format == "Customer" && Is_Round == false)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Customer.xlsx";
                        string newFileName = "Customer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                    else if (excel_Model.excel_Format == "Buyer" && Is_Round == false)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Buyer.xlsx";
                        string newFileName = "Buyer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                    else if (excel_Model.excel_Format == "Supplier" && Is_Round == false)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Supplier.xlsx";
                        string newFileName = "Supplier_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                    else if (excel_Model.excel_Format == "Customer" && Is_Round == true)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Customer_Round.xlsx";
                        string newFileName = "Customer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                    else if (excel_Model.excel_Format == "Buyer" && Is_Round == true)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Buyer_Round.xlsx";
                        string newFileName = "Buyer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                    else if (excel_Model.excel_Format == "Supplier" && Is_Round == true)
                    {
                        string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Supplier_Round.xlsx";
                        string newFileName = "Supplier_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                        if (System.IO.File.Exists(sourceFilePath))
                        {
                            System.IO.File.Copy(sourceFilePath, destinationFilePath);
                            if (System.IO.File.Exists(destinationFilePath))
                            {
                                var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.DataSuccessfullyFound,
                                    result = excelPath,
                                    file_name = newFileName
                                });
                            }
                        }
                    }
                }
                else
                {
                    DataRow newRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        newRow[i] = DBNull.Value;
                    }
                    dataTable.Rows.Add(newRow);
                }
            }

            DataTable supp_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable, excel_Model.excel_Format, excel_Model.supplier_Ref_No);
            if (supp_stock_dt != null && supp_stock_dt.Rows.Count > 0)
            {
                List<string> columnNames = new List<string>();
                foreach (DataColumn column in supp_stock_dt.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }

                DataTable columnNamesTable = new DataTable();
                columnNamesTable.Columns.Add("Column_Name", typeof(string));

                foreach (string columnName in columnNames)
                {
                    columnNamesTable.Rows.Add(columnName);
                }
                var excelPath = string.Empty;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                if (!(Directory.Exists(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }
                string filename = string.Empty;
                if (excel_Model.excel_Format == "Customer")
                {
                    filename = "Customer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Customer_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                else if (excel_Model.excel_Format == "Buyer")
                {
                    filename = "Buyer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Buyer_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                else if (excel_Model.excel_Format == "Supplier")
                {
                    filename = "Supplier_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Supplier_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    result = excelPath,
                    file_name = filename
                });
            }
            return NoContent();
        }

        [HttpGet]
        [Route("pre_generated_export_stock_excel")]
        [Authorize]
        public async Task<IActionResult> Pre_Generated_Export_Stock_Excel()
        {
            try
            {
                #region ALL STOCK
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("SUPPLIER", typeof(string));
                dataTable.Columns.Add("CUSTOMER", typeof(string));
                dataTable.Columns.Add("SHAPE", typeof(string));
                dataTable.Columns.Add("CTS", typeof(string));
                dataTable.Columns.Add("COLOR", typeof(string));
                dataTable.Columns.Add("CLARITY", typeof(string));
                dataTable.Columns.Add("CUT", typeof(string));
                dataTable.Columns.Add("POLISH", typeof(string));
                dataTable.Columns.Add("SYMM", typeof(string));
                dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
                dataTable.Columns.Add("BGM", typeof(string));
                dataTable.Columns.Add("LAB", typeof(string));
                dataTable.Columns.Add("GOOD_TYPE", typeof(string));
                dataTable.Columns.Add("LOCATION", typeof(string));
                dataTable.Columns.Add("STATUS", typeof(string));
                dataTable.Columns.Add("IMAGE_LINK", typeof(string));
                dataTable.Columns.Add("VIDEO_LINK", typeof(string));
                dataTable.Columns.Add("LENGTH", typeof(string));
                dataTable.Columns.Add("WIDTH", typeof(string));
                dataTable.Columns.Add("DEPTH", typeof(string));
                dataTable.Columns.Add("TABLE_PER", typeof(string));
                dataTable.Columns.Add("DEPTH_PER", typeof(string));
                dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
                dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
                dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
                dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
                dataTable.Columns.Add("GIRDLE_PER", typeof(string));
                dataTable.Columns.Add("RAP_RATE", typeof(string));
                dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
                dataTable.Columns.Add("COST_DISC", typeof(string));
                dataTable.Columns.Add("COST_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("BASE_DISC", typeof(string));
                dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                dataTable.Columns.Add("BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
                dataTable.Columns.Add("TABLE_BLACK", typeof(string));
                dataTable.Columns.Add("TABLE_WHITE", typeof(string));
                dataTable.Columns.Add("SIDE_BLACK", typeof(string));
                dataTable.Columns.Add("SIDE_WHITE", typeof(string));
                dataTable.Columns.Add("TABLE_OPEN", typeof(string));
                dataTable.Columns.Add("CROWN_OPEN", typeof(string));
                dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
                dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
                dataTable.Columns.Add("CULET", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                dataTable.Columns.Add("FANCY_COLOR", typeof(string));
                dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
                dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
                dataTable.Columns.Add("POINTER", typeof(string));
                dataTable.Columns.Add("SUB_POINTER", typeof(string));
                dataTable.Columns.Add("STAR_LN", typeof(string));
                dataTable.Columns.Add("LR_HALF", typeof(string));
                dataTable.Columns.Add("CERT_TYPE", typeof(string));
                dataTable.Columns.Add("COST_RATE", typeof(string));
                dataTable.Columns.Add("RATIO", typeof(string));
                dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
                dataTable.Columns.Add("SIGN", typeof(string));
                dataTable.Columns.Add("DISC_VALUE", typeof(string));
                dataTable.Columns.Add("DISC_VALUE1", typeof(string));
                dataTable.Columns.Add("DISC_VALUE2", typeof(string));
                dataTable.Columns.Add("DISC_VALUE3", typeof(string));
                dataTable.Columns.Add("BASE_TYPE", typeof(string));
                dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
                dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                dataTable.Columns.Add("LUSTER", typeof(string));
                dataTable.Columns.Add("SHADE", typeof(string));
                dataTable.Columns.Add("MILKY", typeof(string));
                dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
                dataTable.Columns.Add("GIRDLE_TO", typeof(string));
                dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
                dataTable.Columns.Add("ORIGIN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
                dataTable.Columns.Add("H_A", typeof(string));
                dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));


                DataRow newRow = dataTable.NewRow();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    newRow[i] = DBNull.Value;
                }
                dataTable.Rows.Add(newRow);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/PreGeneratedStockExcelFiles/");
                if (!(Directory.Exists(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }

                //Supplier all stock
                DataTable supp_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable, "Supplier", null);
                if (supp_stock_dt != null && supp_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in supp_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Supplier_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + "Supplier.xlsx");
                }
                //Customer all stock
                DataTable cust_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable, "Customer", null);
                if (cust_stock_dt != null && cust_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in cust_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Customer_Excel(cust_stock_dt, columnNamesTable, filePath, filePath + "Customer.xlsx");
                }
                //Buyer all stock
                DataTable buyer_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable, "Buyer", null);
                if (buyer_stock_dt != null && buyer_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in buyer_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Buyer_Excel(buyer_stock_dt, columnNamesTable, filePath, filePath + "Buyer.xlsx");
                }
                #endregion

                #region ROUND STOCK
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add("STOCK_ID", typeof(string));
                dataTable1.Columns.Add("SUPPLIER", typeof(string));
                dataTable1.Columns.Add("CUSTOMER", typeof(string));
                dataTable1.Columns.Add("SHAPE", typeof(string));
                dataTable1.Columns.Add("CTS", typeof(string));
                dataTable1.Columns.Add("COLOR", typeof(string));
                dataTable1.Columns.Add("CLARITY", typeof(string));
                dataTable1.Columns.Add("CUT", typeof(string));
                dataTable1.Columns.Add("POLISH", typeof(string));
                dataTable1.Columns.Add("SYMM", typeof(string));
                dataTable1.Columns.Add("FLS_INTENSITY", typeof(string));
                dataTable1.Columns.Add("BGM", typeof(string));
                dataTable1.Columns.Add("LAB", typeof(string));
                dataTable1.Columns.Add("GOOD_TYPE", typeof(string));
                dataTable1.Columns.Add("LOCATION", typeof(string));
                dataTable1.Columns.Add("STATUS", typeof(string));
                dataTable1.Columns.Add("IMAGE_LINK", typeof(string));
                dataTable1.Columns.Add("VIDEO_LINK", typeof(string));
                dataTable1.Columns.Add("LENGTH", typeof(string));
                dataTable1.Columns.Add("WIDTH", typeof(string));
                dataTable1.Columns.Add("DEPTH", typeof(string));
                dataTable1.Columns.Add("TABLE_PER", typeof(string));
                dataTable1.Columns.Add("DEPTH_PER", typeof(string));
                dataTable1.Columns.Add("CROWN_ANGLE", typeof(string));
                dataTable1.Columns.Add("CROWN_HEIGHT", typeof(string));
                dataTable1.Columns.Add("PAVILION_ANGLE", typeof(string));
                dataTable1.Columns.Add("PAVILION_HEIGHT", typeof(string));
                dataTable1.Columns.Add("GIRDLE_PER", typeof(string));
                dataTable1.Columns.Add("RAP_RATE", typeof(string));
                dataTable1.Columns.Add("RAP_AMOUNT", typeof(string));
                dataTable1.Columns.Add("COST_DISC", typeof(string));
                dataTable1.Columns.Add("COST_AMOUNT", typeof(string));
                dataTable1.Columns.Add("OFFER_DISC", typeof(string));
                dataTable1.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable1.Columns.Add("BASE_DISC", typeof(string));
                dataTable1.Columns.Add("BASE_AMOUNT", typeof(string));
                dataTable1.Columns.Add("PRICE_PER_CTS", typeof(string));
                dataTable1.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                dataTable1.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                dataTable1.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                dataTable1.Columns.Add("BUYER_DISC", typeof(string));
                dataTable1.Columns.Add("BUYER AMOUNT", typeof(string));
                dataTable1.Columns.Add("TABLE_BLACK", typeof(string));
                dataTable1.Columns.Add("TABLE_WHITE", typeof(string));
                dataTable1.Columns.Add("SIDE_BLACK", typeof(string));
                dataTable1.Columns.Add("SIDE_WHITE", typeof(string));
                dataTable1.Columns.Add("TABLE_OPEN", typeof(string));
                dataTable1.Columns.Add("CROWN_OPEN", typeof(string));
                dataTable1.Columns.Add("PAVILION_OPEN", typeof(string));
                dataTable1.Columns.Add("GIRDLE_OPEN", typeof(string));
                dataTable1.Columns.Add("CULET", typeof(string));
                dataTable1.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                dataTable1.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                dataTable1.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                dataTable1.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                dataTable1.Columns.Add("FANCY_COLOR", typeof(string));
                dataTable1.Columns.Add("FANCY_INTENSITY", typeof(string));
                dataTable1.Columns.Add("FANCY_OVERTONE", typeof(string));
                dataTable1.Columns.Add("POINTER", typeof(string));
                dataTable1.Columns.Add("SUB_POINTER", typeof(string));
                dataTable1.Columns.Add("STAR_LN", typeof(string));
                dataTable1.Columns.Add("LR_HALF", typeof(string));
                dataTable1.Columns.Add("CERT_TYPE", typeof(string));
                dataTable1.Columns.Add("COST_RATE", typeof(string));
                dataTable1.Columns.Add("RATIO", typeof(string));
                dataTable1.Columns.Add("DISCOUNT_TYPE", typeof(string));
                dataTable1.Columns.Add("SIGN", typeof(string));
                dataTable1.Columns.Add("DISC_VALUE", typeof(string));
                dataTable1.Columns.Add("DISC_VALUE1", typeof(string));
                dataTable1.Columns.Add("DISC_VALUE2", typeof(string));
                dataTable1.Columns.Add("DISC_VALUE3", typeof(string));
                dataTable1.Columns.Add("BASE_TYPE", typeof(string));
                dataTable1.Columns.Add("SUPP_STOCK_ID", typeof(string));
                dataTable1.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                dataTable1.Columns.Add("BID_BUYER_DISC", typeof(string));
                dataTable1.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                dataTable1.Columns.Add("LUSTER", typeof(string));
                dataTable1.Columns.Add("SHADE", typeof(string));
                dataTable1.Columns.Add("MILKY", typeof(string));
                dataTable1.Columns.Add("GIRDLE_FROM", typeof(string));
                dataTable1.Columns.Add("GIRDLE_TO", typeof(string));
                dataTable1.Columns.Add("GIRDLE_CONDITION", typeof(string));
                dataTable1.Columns.Add("ORIGIN", typeof(string));
                dataTable1.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                dataTable1.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                dataTable1.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                dataTable1.Columns.Add("INTERNAL_GRAINING", typeof(string));
                dataTable1.Columns.Add("H_A", typeof(string));
                dataTable1.Columns.Add("SUPPLIER_COMMENTS", typeof(string));
                var Round_Cat_Val_Id = _configuration["Round_Cat_Val_Id"];
                dataTable1.Rows.Add(DBNull.Value, DBNull.Value, DBNull.Value, Round_Cat_Val_Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                            DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value);

                //Supplier round stock
                DataTable supp_round_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable1, "Supplier", null);
                if (supp_round_stock_dt != null && supp_round_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in supp_round_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Supplier_Excel(supp_round_stock_dt, columnNamesTable, filePath, filePath + "Supplier_Round.xlsx");
                }
                //Customer round stock
                DataTable cust_round_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable1, "Customer", null);
                if (cust_round_stock_dt != null && cust_round_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in cust_round_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Customer_Excel(cust_round_stock_dt, columnNamesTable, filePath, filePath + "Customer_Round.xlsx");
                }
                //Buyer round stock
                DataTable buyer_round_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable1, "Buyer", null);
                if (buyer_round_stock_dt != null && buyer_round_stock_dt.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in buyer_round_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    EpExcelExport.Create_Buyer_Excel(buyer_round_stock_dt, columnNamesTable, filePath, filePath + "Buyer_Round.xlsx");
                }
                #endregion

                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Pre_Generated_Export_Stock_Excel", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("download_all_stock_excel")]
        [Authorize]
        public async Task<IActionResult> Download_All_Stock_Excel(string excel_Format)
        {
            try
            {
                string filename = string.Empty;
                var destinationFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                if (excel_Format == "Customer")
                {
                    string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Customer.xlsx";
                    string newFileName = "Customer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Move(sourceFilePath, destinationFilePath);
                        if (System.IO.File.Exists(destinationFilePath))
                        {
                            var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.DataSuccessfullyFound,
                                result = excelPath,
                                file_name = filename
                            });
                        }
                    }
                }
                else if (excel_Format == "Buyer")
                {
                    string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Buyer.xlsx";
                    string newFileName = "Buyer_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
                        if (System.IO.File.Exists(destinationFilePath))
                        {
                            var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.DataSuccessfullyFound,
                                result = excelPath,
                                file_name = filename
                            });
                        }
                    }
                }
                else if (excel_Format == "Supplier")
                {
                    string sourceFilePath = Directory.GetCurrentDirectory() + CoreCommonFilePath.PreGeneratedStockExcelFilesPath + "Supplier.xlsx";
                    string newFileName = "Supplier_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    string destinationFilePath = Path.Combine(destinationFolderPath, newFileName);
                    if (System.IO.File.Exists(sourceFilePath))
                    {
                        System.IO.File.Move(sourceFilePath, destinationFilePath);
                        if (System.IO.File.Exists(destinationFilePath))
                        {
                            var excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + newFileName;
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.DataSuccessfullyFound,
                                result = excelPath,
                                file_name = filename
                            });
                        }
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Download_All_Stock_Excel", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Send Stock On Email
        [HttpPost]
        [Route("send_stock_on_email")]
        [Authorize]
        public async Task<IActionResult> Send_Stock_On_Email(Stock_Email_Model stock_Email_Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string STOCK_ID = string.Empty, SUPPLIER = string.Empty, CUSTOMER = string.Empty, SHAPE = string.Empty,
                        CTS = string.Empty, COLOR = string.Empty, CLARITY = string.Empty, CUT = string.Empty,
                        POLISH = string.Empty, SYMM = string.Empty, FLS_INTENSITY = string.Empty, BGM = string.Empty,
                        LAB = string.Empty, GOOD_TYPE = string.Empty, LOCATION = string.Empty, STATUS = string.Empty,
                        IMAGE_LINK = string.Empty, VIDEO_LINK = string.Empty, LENGTH = string.Empty, WIDTH = string.Empty,
                        DEPTH = string.Empty, TABLE_PER = string.Empty, DEPTH_PER = string.Empty, CROWN_ANGLE = string.Empty,
                        CROWN_HEIGHT = string.Empty, PAVILION_ANGLE = string.Empty, PAVILION_HEIGHT = string.Empty, GIRDLE_PER = string.Empty,
                        RAP_RATE = string.Empty, RAP_AMOUNT = string.Empty, COST_DISC = string.Empty, COST_AMOUNT = string.Empty,
                        OFFER_DISC = string.Empty, OFFER_VALUE = string.Empty, BASE_DISC = string.Empty, BASE_AMOUNT = string.Empty,
                        PRICE_PER_CTS = string.Empty, MAX_SLAB_BASE_DISC = string.Empty, MAX_SLAB_BASE_AMOUNT = string.Empty, MAX_SLAB_PRICE_PER_CTS = string.Empty,
                        BUYER_DISC = string.Empty, BUYER_AMOUNT = string.Empty, TABLE_BLACK = string.Empty, TABLE_WHITE = string.Empty, SIDE_BLACK = string.Empty, SIDE_WHITE = string.Empty,
                        TABLE_OPEN = string.Empty, CROWN_OPEN = string.Empty, PAVILION_OPEN = string.Empty,
                        GIRDLE_OPEN = string.Empty, CULET = string.Empty, KEY_TO_SYMBOL_TRUE = string.Empty,
                        KEY_TO_SYMBOL_FALSE = string.Empty, LAB_COMMENTS_TRUE = string.Empty, LAB_COMMENTS_FALSE = string.Empty,
                        FANCY_COLOR = string.Empty, FANCY_INTENSITY = string.Empty, FANCY_OVERTONE = string.Empty, POINTER = string.Empty, SUB_POINTER = string.Empty,
                        STAR_LN = string.Empty, LR_HALF = string.Empty, CERT_TYPE = string.Empty, COST_RATE = string.Empty, RATIO = string.Empty, DISCOUNT_TYPE = string.Empty,
                        SIGN = string.Empty, DISC_VALUE = string.Empty, DISC_VALUE1 = string.Empty, DISC_VALUE2 = string.Empty, DISC_VALUE3 = string.Empty, BASE_TYPE = string.Empty, SUPP_STOCK_ID = string.Empty,
                        BID_SUPP_STOCK_ID = string.Empty, BID_BUYER_DISC = string.Empty, BID_BUYER_AMOUNT = string.Empty,
                        LUSTER = string.Empty, SHADE = string.Empty, MILKY = string.Empty, GIRDLE_FROM = string.Empty, GIRDLE_TO = string.Empty, GIRDLE_CONDITION = string.Empty, ORIGIN = string.Empty,
                        EXTRA_FACET_TABLE = string.Empty, EXTRA_FACET_CROWN = string.Empty, EXTRA_FACET_PAV = string.Empty,
                        INTERNAL_GRAINING = string.Empty, H_A = string.Empty, SUPPLIER_COMMENTS = string.Empty;

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("STOCK_ID", typeof(string));
                    dataTable.Columns.Add("SUPPLIER", typeof(string));
                    dataTable.Columns.Add("CUSTOMER", typeof(string));
                    dataTable.Columns.Add("SHAPE", typeof(string));
                    dataTable.Columns.Add("CTS", typeof(string));
                    dataTable.Columns.Add("COLOR", typeof(string));
                    dataTable.Columns.Add("CLARITY", typeof(string));
                    dataTable.Columns.Add("CUT", typeof(string));
                    dataTable.Columns.Add("POLISH", typeof(string));
                    dataTable.Columns.Add("SYMM", typeof(string));
                    dataTable.Columns.Add("FLS_INTENSITY", typeof(string));
                    dataTable.Columns.Add("BGM", typeof(string));
                    dataTable.Columns.Add("LAB", typeof(string));
                    dataTable.Columns.Add("GOOD_TYPE", typeof(string));
                    dataTable.Columns.Add("LOCATION", typeof(string));
                    dataTable.Columns.Add("STATUS", typeof(string));
                    dataTable.Columns.Add("IMAGE_LINK", typeof(string));
                    dataTable.Columns.Add("VIDEO_LINK", typeof(string));
                    dataTable.Columns.Add("LENGTH", typeof(string));
                    dataTable.Columns.Add("WIDTH", typeof(string));
                    dataTable.Columns.Add("DEPTH", typeof(string));
                    dataTable.Columns.Add("TABLE_PER", typeof(string));
                    dataTable.Columns.Add("DEPTH_PER", typeof(string));
                    dataTable.Columns.Add("CROWN_ANGLE", typeof(string));
                    dataTable.Columns.Add("CROWN_HEIGHT", typeof(string));
                    dataTable.Columns.Add("PAVILION_ANGLE", typeof(string));
                    dataTable.Columns.Add("PAVILION_HEIGHT", typeof(string));
                    dataTable.Columns.Add("GIRDLE_PER", typeof(string));
                    dataTable.Columns.Add("RAP_RATE", typeof(string));
                    dataTable.Columns.Add("RAP_AMOUNT", typeof(string));
                    dataTable.Columns.Add("COST_DISC", typeof(string));
                    dataTable.Columns.Add("COST_AMOUNT", typeof(string));
                    dataTable.Columns.Add("OFFER_DISC", typeof(string));
                    dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                    dataTable.Columns.Add("BASE_DISC", typeof(string));
                    dataTable.Columns.Add("BASE_AMOUNT", typeof(string));
                    dataTable.Columns.Add("PRICE_PER_CTS", typeof(string));
                    dataTable.Columns.Add("MAX_SLAB_BASE_DISC", typeof(string));
                    dataTable.Columns.Add("MAX_SLAB_BASE_AMOUNT", typeof(string));
                    dataTable.Columns.Add("MAX_SLAB_PRICE_PER_CTS", typeof(string));
                    dataTable.Columns.Add("BUYER_DISC", typeof(string));
                    dataTable.Columns.Add("BUYER AMOUNT", typeof(string));
                    dataTable.Columns.Add("TABLE_BLACK", typeof(string));
                    dataTable.Columns.Add("TABLE_WHITE", typeof(string));
                    dataTable.Columns.Add("SIDE_BLACK", typeof(string));
                    dataTable.Columns.Add("SIDE_WHITE", typeof(string));
                    dataTable.Columns.Add("TABLE_OPEN", typeof(string));
                    dataTable.Columns.Add("CROWN_OPEN", typeof(string));
                    dataTable.Columns.Add("PAVILION_OPEN", typeof(string));
                    dataTable.Columns.Add("GIRDLE_OPEN", typeof(string));
                    dataTable.Columns.Add("CULET", typeof(string));
                    dataTable.Columns.Add("KEY_TO_SYMBOL_TRUE", typeof(string));
                    dataTable.Columns.Add("KEY_TO_SYMBOL_FALSE", typeof(string));
                    dataTable.Columns.Add("LAB_COMMENTS_TRUE", typeof(string));
                    dataTable.Columns.Add("LAB_COMMENTS_FALSE", typeof(string));
                    dataTable.Columns.Add("FANCY_COLOR", typeof(string));
                    dataTable.Columns.Add("FANCY_INTENSITY", typeof(string));
                    dataTable.Columns.Add("FANCY_OVERTONE", typeof(string));
                    dataTable.Columns.Add("POINTER", typeof(string));
                    dataTable.Columns.Add("SUB_POINTER", typeof(string));
                    dataTable.Columns.Add("STAR_LN", typeof(string));
                    dataTable.Columns.Add("LR_HALF", typeof(string));
                    dataTable.Columns.Add("CERT_TYPE", typeof(string));
                    dataTable.Columns.Add("COST_RATE", typeof(string));
                    dataTable.Columns.Add("RATIO", typeof(string));
                    dataTable.Columns.Add("DISCOUNT_TYPE", typeof(string));
                    dataTable.Columns.Add("SIGN", typeof(string));
                    dataTable.Columns.Add("DISC_VALUE", typeof(string));
                    dataTable.Columns.Add("DISC_VALUE1", typeof(string));
                    dataTable.Columns.Add("DISC_VALUE2", typeof(string));
                    dataTable.Columns.Add("DISC_VALUE3", typeof(string));
                    dataTable.Columns.Add("BASE_TYPE", typeof(string));
                    dataTable.Columns.Add("SUPP_STOCK_ID", typeof(string));
                    dataTable.Columns.Add("BID_SUPP_STOCK_ID", typeof(string));
                    dataTable.Columns.Add("BID_BUYER_DISC", typeof(string));
                    dataTable.Columns.Add("BID_BUYER_AMOUNT", typeof(string));
                    dataTable.Columns.Add("LUSTER", typeof(string));
                    dataTable.Columns.Add("SHADE", typeof(string));
                    dataTable.Columns.Add("MILKY", typeof(string));
                    dataTable.Columns.Add("GIRDLE_FROM", typeof(string));
                    dataTable.Columns.Add("GIRDLE_TO", typeof(string));
                    dataTable.Columns.Add("GIRDLE_CONDITION", typeof(string));
                    dataTable.Columns.Add("ORIGIN", typeof(string));
                    dataTable.Columns.Add("EXTRA_FACET_TABLE", typeof(string));
                    dataTable.Columns.Add("EXTRA_FACET_CROWN", typeof(string));
                    dataTable.Columns.Add("EXTRA_FACET_PAV", typeof(string));
                    dataTable.Columns.Add("INTERNAL_GRAINING", typeof(string));
                    dataTable.Columns.Add("H_A", typeof(string));
                    dataTable.Columns.Add("SUPPLIER_COMMENTS", typeof(string));

                    if (stock_Email_Model.Report_Filter_Parameter_List != null && stock_Email_Model.Report_Filter_Parameter_List.Count > 0)
                    {
                        foreach (var item in stock_Email_Model.Report_Filter_Parameter_List)
                        {
                            STOCK_ID = item.Where(x => x.Column_Name == "STOCK ID" || x.Column_Name == "CERTIFICATE NO" || x.Column_Name == "SUPPLIER NO").Select(x => x.Category_Value).FirstOrDefault();
                            SUPPLIER = item.Where(x => x.Column_Name == "SUPPLIER" || x.Column_Name == "COMPANY").Select(x => x.Category_Value).FirstOrDefault();
                            CUSTOMER = item.Where(x => x.Column_Name == "CUSTOMER").Select(x => x.Category_Value).FirstOrDefault();
                            SHAPE = item.Where(x => x.Column_Name == "SHAPE").Select(x => x.Category_Value).FirstOrDefault();
                            CTS = item.Where(x => x.Column_Name == "CTS").Select(x => x.Category_Value).FirstOrDefault();
                            COLOR = item.Where(x => x.Column_Name == "COLOR").Select(x => x.Category_Value).FirstOrDefault();
                            CLARITY = item.Where(x => x.Column_Name == "CLARITY").Select(x => x.Category_Value).FirstOrDefault();
                            CUT = item.Where(x => x.Column_Name == "CUT").Select(x => x.Category_Value).FirstOrDefault();
                            POLISH = item.Where(x => x.Column_Name == "POLISH").Select(x => x.Category_Value).FirstOrDefault();
                            SYMM = item.Where(x => x.Column_Name == "SYMM").Select(x => x.Category_Value).FirstOrDefault();
                            FLS_INTENSITY = item.Where(x => x.Column_Name == "FLS INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                            BGM = item.Where(x => x.Column_Name == "BGM").Select(x => x.Category_Value).FirstOrDefault();
                            LAB = item.Where(x => x.Column_Name == "LAB").Select(x => x.Category_Value).FirstOrDefault();
                            GOOD_TYPE = item.Where(x => x.Column_Name == "GOOD TYPE").Select(x => x.Category_Value).FirstOrDefault();
                            LOCATION = item.Where(x => x.Column_Name == "LOCATION").Select(x => x.Category_Value).FirstOrDefault();
                            STATUS = item.Where(x => x.Column_Name == "STATUS").Select(x => x.Category_Value).FirstOrDefault();
                            IMAGE_LINK = item.Where(x => x.Column_Name == "IMAGE LINK").Select(x => x.Category_Value).FirstOrDefault();
                            VIDEO_LINK = item.Where(x => x.Column_Name == "VIDEO LINK").Select(x => x.Category_Value).FirstOrDefault();
                            LENGTH = item.Where(x => x.Column_Name == "LENGTH").Select(x => x.Category_Value).FirstOrDefault();
                            WIDTH = item.Where(x => x.Column_Name == "WIDTH").Select(x => x.Category_Value).FirstOrDefault();
                            DEPTH = item.Where(x => x.Column_Name == "DEPTH").Select(x => x.Category_Value).FirstOrDefault();
                            TABLE_PER = item.Where(x => x.Column_Name == "TABLE PER").Select(x => x.Category_Value).FirstOrDefault();
                            DEPTH_PER = item.Where(x => x.Column_Name == "DEPTH PER").Select(x => x.Category_Value).FirstOrDefault();
                            CROWN_ANGLE = item.Where(x => x.Column_Name == "CROWN ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                            CROWN_HEIGHT = item.Where(x => x.Column_Name == "CROWN HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                            PAVILION_ANGLE = item.Where(x => x.Column_Name == "PAVILION ANGLE").Select(x => x.Category_Value).FirstOrDefault();
                            PAVILION_HEIGHT = item.Where(x => x.Column_Name == "PAVILION HEIGHT").Select(x => x.Category_Value).FirstOrDefault();
                            GIRDLE_PER = item.Where(x => x.Column_Name == "GIRDLE PER").Select(x => x.Category_Value).FirstOrDefault();
                            RAP_RATE = item.Where(x => x.Column_Name == "RAP RATE").Select(x => x.Category_Value).FirstOrDefault();
                            RAP_AMOUNT = item.Where(x => x.Column_Name == "RAP AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            COST_DISC = item.Where(x => x.Column_Name == "COST DISC").Select(x => x.Category_Value).FirstOrDefault();
                            COST_AMOUNT = item.Where(x => x.Column_Name == "COST AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            OFFER_DISC = item.Where(x => x.Column_Name == "OFFER DISC").Select(x => x.Category_Value).FirstOrDefault();
                            OFFER_VALUE = item.Where(x => x.Column_Name == "OFFER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            BASE_DISC = item.Where(x => x.Column_Name == "BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                            BASE_AMOUNT = item.Where(x => x.Column_Name == "BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            PRICE_PER_CTS = item.Where(x => x.Column_Name == "PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                            MAX_SLAB_BASE_DISC = item.Where(x => x.Column_Name == "MAX SLAB BASE DISC").Select(x => x.Category_Value).FirstOrDefault();
                            MAX_SLAB_BASE_AMOUNT = item.Where(x => x.Column_Name == "MAX SLAB BASE AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            MAX_SLAB_PRICE_PER_CTS = item.Where(x => x.Column_Name == "MAX SLAB PRICE PER CTS").Select(x => x.Category_Value).FirstOrDefault();
                            BUYER_DISC = item.Where(x => x.Column_Name == "BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                            BUYER_AMOUNT = item.Where(x => x.Column_Name == "BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            TABLE_BLACK = item.Where(x => x.Column_Name == "TABLE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                            TABLE_WHITE = item.Where(x => x.Column_Name == "TABLE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                            SIDE_BLACK = item.Where(x => x.Column_Name == "SIDE BLACK").Select(x => x.Category_Value).FirstOrDefault();
                            SIDE_WHITE = item.Where(x => x.Column_Name == "SIDE WHITE").Select(x => x.Category_Value).FirstOrDefault();
                            TABLE_OPEN = item.Where(x => x.Column_Name == "TABLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                            CROWN_OPEN = item.Where(x => x.Column_Name == "CROWN OPEN").Select(x => x.Category_Value).FirstOrDefault();
                            PAVILION_OPEN = item.Where(x => x.Column_Name == "PAVILION OPEN").Select(x => x.Category_Value).FirstOrDefault();
                            GIRDLE_OPEN = item.Where(x => x.Column_Name == "GIRDLE OPEN").Select(x => x.Category_Value).FirstOrDefault();
                            CULET = item.Where(x => x.Column_Name == "CULET").Select(x => x.Category_Value).FirstOrDefault();
                            KEY_TO_SYMBOL_TRUE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_TRUE).FirstOrDefault();
                            KEY_TO_SYMBOL_FALSE = item.Where(x => x.Column_Name == "KEY TO SYMBOL").Select(x => x.KEY_TO_SYMBOL_FALSE).FirstOrDefault();
                            LAB_COMMENTS_TRUE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_TRUE).FirstOrDefault();
                            LAB_COMMENTS_FALSE = item.Where(x => x.Column_Name == "LAB COMMENTS").Select(x => x.LAB_COMMENTS_FALSE).FirstOrDefault();
                            FANCY_COLOR = item.Where(x => x.Column_Name == "FANCY COLOR").Select(x => x.Category_Value).FirstOrDefault();
                            FANCY_INTENSITY = item.Where(x => x.Column_Name == "FANCY INTENSITY").Select(x => x.Category_Value).FirstOrDefault();
                            FANCY_OVERTONE = item.Where(x => x.Column_Name == "FANCY OVERTONE").Select(x => x.Category_Value).FirstOrDefault();
                            POINTER = item.Where(x => x.Column_Name == "POINTER").Select(x => x.Category_Value).FirstOrDefault();
                            SUB_POINTER = item.Where(x => x.Column_Name == "SUB POINTER").Select(x => x.Category_Value).FirstOrDefault();
                            STAR_LN = item.Where(x => x.Column_Name == "STAR LN").Select(x => x.Category_Value).FirstOrDefault();
                            LR_HALF = item.Where(x => x.Column_Name == "LR HALF").Select(x => x.Category_Value).FirstOrDefault();
                            CERT_TYPE = item.Where(x => x.Column_Name == "CERT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                            COST_RATE = item.Where(x => x.Column_Name == "COST RATE").Select(x => x.Category_Value).FirstOrDefault();
                            RATIO = item.Where(x => x.Column_Name == "RATIO").Select(x => x.Category_Value).FirstOrDefault();
                            DISCOUNT_TYPE = item.Where(x => x.Column_Name == "DISCOUNT TYPE").Select(x => x.Category_Value).FirstOrDefault();
                            SIGN = item.Where(x => x.Column_Name == "SIGN").Select(x => x.Category_Value).FirstOrDefault();
                            DISC_VALUE = item.Where(x => x.Column_Name == "DISC VALUE").Select(x => x.Category_Value).FirstOrDefault();
                            DISC_VALUE1 = item.Where(x => x.Column_Name == "DISC VALUE1").Select(x => x.Category_Value).FirstOrDefault();
                            DISC_VALUE2 = item.Where(x => x.Column_Name == "DISC VALUE2").Select(x => x.Category_Value).FirstOrDefault();
                            DISC_VALUE3 = item.Where(x => x.Column_Name == "DISC VALUE3").Select(x => x.Category_Value).FirstOrDefault();
                            BASE_TYPE = item.Where(x => x.Column_Name == "BASE TYPE").Select(x => x.Category_Value).FirstOrDefault();
                            SUPP_STOCK_ID = item.Where(x => x.Column_Name == "SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                            BID_SUPP_STOCK_ID = item.Where(x => x.Column_Name == "BID SUPP STOCK ID").Select(x => x.Category_Value).FirstOrDefault();
                            BID_BUYER_DISC = item.Where(x => x.Column_Name == "BID BUYER DISC").Select(x => x.Category_Value).FirstOrDefault();
                            BID_BUYER_AMOUNT = item.Where(x => x.Column_Name == "BID BUYER AMOUNT").Select(x => x.Category_Value).FirstOrDefault();
                            LUSTER = item.Where(x => x.Column_Name == "LUSTER").Select(x => x.Category_Value).FirstOrDefault();
                            SHADE = item.Where(x => x.Column_Name == "SHADE").Select(x => x.Category_Value).FirstOrDefault();
                            MILKY = item.Where(x => x.Column_Name == "MILKY").Select(x => x.Category_Value).FirstOrDefault();
                            GIRDLE_FROM = item.Where(x => x.Column_Name == "GIRDLE_FROM").Select(x => x.Category_Value).FirstOrDefault();
                            GIRDLE_TO = item.Where(x => x.Column_Name == "GIRDLE_TO").Select(x => x.Category_Value).FirstOrDefault();
                            GIRDLE_CONDITION = item.Where(x => x.Column_Name == "GIRDLE_CONDITION").Select(x => x.Category_Value).FirstOrDefault();
                            ORIGIN = item.Where(x => x.Column_Name == "ORIGIN").Select(x => x.Category_Value).FirstOrDefault();
                            EXTRA_FACET_TABLE = item.Where(x => x.Column_Name == "EXTRA_FACET_TABLE").Select(x => x.Category_Value).FirstOrDefault();
                            EXTRA_FACET_CROWN = item.Where(x => x.Column_Name == "EXTRA_FACET_CROWN").Select(x => x.Category_Value).FirstOrDefault();
                            EXTRA_FACET_PAV = item.Where(x => x.Column_Name == "EXTRA_FACET_PAV").Select(x => x.Category_Value).FirstOrDefault();
                            INTERNAL_GRAINING = item.Where(x => x.Column_Name == "INTERNAL_GRAINING").Select(x => x.Category_Value).FirstOrDefault();
                            H_A = item.Where(x => x.Column_Name == "H_A").Select(x => x.Category_Value).FirstOrDefault();
                            SUPPLIER_COMMENTS = item.Where(x => x.Column_Name == "SUPPLIER_COMMENTS").Select(x => x.Category_Value).FirstOrDefault();

                            dataTable.Rows.Add(!string.IsNullOrEmpty(STOCK_ID) ? STOCK_ID : DBNull.Value,
                                !string.IsNullOrEmpty(SUPPLIER) ? SUPPLIER : DBNull.Value,
                                !string.IsNullOrEmpty(CUSTOMER) ? CUSTOMER : DBNull.Value,
                                !string.IsNullOrEmpty(SHAPE) ? SHAPE : DBNull.Value,
                                !string.IsNullOrEmpty(CTS) ? CTS : DBNull.Value,
                                !string.IsNullOrEmpty(COLOR) ? COLOR : DBNull.Value,
                                !string.IsNullOrEmpty(CLARITY) ? CLARITY : DBNull.Value,
                                !string.IsNullOrEmpty(CUT) ? CUT : DBNull.Value,
                                !string.IsNullOrEmpty(POLISH) ? POLISH : DBNull.Value,
                                !string.IsNullOrEmpty(SYMM) ? SYMM : DBNull.Value,
                                !string.IsNullOrEmpty(FLS_INTENSITY) ? FLS_INTENSITY : DBNull.Value,
                                !string.IsNullOrEmpty(BGM) ? BGM : DBNull.Value,
                                !string.IsNullOrEmpty(LAB) ? LAB : DBNull.Value,
                                !string.IsNullOrEmpty(GOOD_TYPE) ? GOOD_TYPE : DBNull.Value,
                                !string.IsNullOrEmpty(LOCATION) ? LOCATION : DBNull.Value,
                                !string.IsNullOrEmpty(STATUS) ? STATUS : DBNull.Value,
                                !string.IsNullOrEmpty(IMAGE_LINK) ? IMAGE_LINK : DBNull.Value,
                                !string.IsNullOrEmpty(VIDEO_LINK) ? VIDEO_LINK : DBNull.Value,
                                !string.IsNullOrEmpty(LENGTH) ? LENGTH : DBNull.Value,
                                !string.IsNullOrEmpty(WIDTH) ? WIDTH : DBNull.Value,
                                !string.IsNullOrEmpty(DEPTH) ? DEPTH : DBNull.Value,
                                !string.IsNullOrEmpty(TABLE_PER) ? TABLE_PER : DBNull.Value,
                                !string.IsNullOrEmpty(DEPTH_PER) ? DEPTH_PER : DBNull.Value,
                                !string.IsNullOrEmpty(CROWN_ANGLE) ? CROWN_ANGLE : DBNull.Value,
                                !string.IsNullOrEmpty(CROWN_HEIGHT) ? CROWN_HEIGHT : DBNull.Value,
                                !string.IsNullOrEmpty(PAVILION_ANGLE) ? PAVILION_ANGLE : DBNull.Value,
                                !string.IsNullOrEmpty(PAVILION_HEIGHT) ? PAVILION_HEIGHT : DBNull.Value,
                                !string.IsNullOrEmpty(GIRDLE_PER) ? GIRDLE_PER : DBNull.Value,
                                !string.IsNullOrEmpty(RAP_RATE) ? RAP_RATE : DBNull.Value,
                                !string.IsNullOrEmpty(RAP_AMOUNT) ? RAP_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(COST_DISC) ? COST_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(COST_AMOUNT) ? COST_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(OFFER_DISC) ? OFFER_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(OFFER_VALUE) ? OFFER_VALUE : DBNull.Value,
                                !string.IsNullOrEmpty(BASE_DISC) ? BASE_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(BASE_AMOUNT) ? BASE_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(PRICE_PER_CTS) ? PRICE_PER_CTS : DBNull.Value,
                                !string.IsNullOrEmpty(MAX_SLAB_BASE_DISC) ? MAX_SLAB_BASE_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(MAX_SLAB_BASE_AMOUNT) ? MAX_SLAB_BASE_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(MAX_SLAB_PRICE_PER_CTS) ? MAX_SLAB_PRICE_PER_CTS : DBNull.Value,
                                !string.IsNullOrEmpty(BUYER_DISC) ? BUYER_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(BUYER_AMOUNT) ? BUYER_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(TABLE_BLACK) ? TABLE_BLACK : DBNull.Value,
                                !string.IsNullOrEmpty(TABLE_WHITE) ? TABLE_WHITE : DBNull.Value,
                                !string.IsNullOrEmpty(SIDE_BLACK) ? SIDE_BLACK : DBNull.Value,
                                !string.IsNullOrEmpty(SIDE_WHITE) ? SIDE_WHITE : DBNull.Value,
                                !string.IsNullOrEmpty(TABLE_OPEN) ? TABLE_OPEN : DBNull.Value,
                                !string.IsNullOrEmpty(CROWN_OPEN) ? CROWN_OPEN : DBNull.Value,
                                !string.IsNullOrEmpty(PAVILION_OPEN) ? PAVILION_OPEN : DBNull.Value,
                                !string.IsNullOrEmpty(GIRDLE_OPEN) ? GIRDLE_OPEN : DBNull.Value,
                                !string.IsNullOrEmpty(CULET) ? CULET : DBNull.Value,
                                !string.IsNullOrEmpty(KEY_TO_SYMBOL_TRUE) ? KEY_TO_SYMBOL_TRUE : DBNull.Value,
                                !string.IsNullOrEmpty(KEY_TO_SYMBOL_FALSE) ? KEY_TO_SYMBOL_FALSE : DBNull.Value,
                                !string.IsNullOrEmpty(LAB_COMMENTS_TRUE) ? LAB_COMMENTS_TRUE : DBNull.Value,
                                !string.IsNullOrEmpty(LAB_COMMENTS_FALSE) ? LAB_COMMENTS_FALSE : DBNull.Value,
                                !string.IsNullOrEmpty(FANCY_COLOR) ? FANCY_COLOR : DBNull.Value,
                                !string.IsNullOrEmpty(FANCY_INTENSITY) ? FANCY_INTENSITY : DBNull.Value,
                                !string.IsNullOrEmpty(FANCY_OVERTONE) ? FANCY_OVERTONE : DBNull.Value,
                                !string.IsNullOrEmpty(POINTER) ? POINTER : DBNull.Value,
                                !string.IsNullOrEmpty(SUB_POINTER) ? SUB_POINTER : DBNull.Value,
                                !string.IsNullOrEmpty(STAR_LN) ? STAR_LN : DBNull.Value,
                                !string.IsNullOrEmpty(LR_HALF) ? LR_HALF : DBNull.Value,
                                !string.IsNullOrEmpty(CERT_TYPE) ? CERT_TYPE : DBNull.Value,
                                !string.IsNullOrEmpty(COST_RATE) ? COST_RATE : DBNull.Value,
                                !string.IsNullOrEmpty(RATIO) ? RATIO : DBNull.Value,
                                !string.IsNullOrEmpty(DISCOUNT_TYPE) ? DISCOUNT_TYPE : DBNull.Value,
                                !string.IsNullOrEmpty(SIGN) ? SIGN : DBNull.Value,
                                !string.IsNullOrEmpty(DISC_VALUE) ? DISC_VALUE : DBNull.Value,
                                !string.IsNullOrEmpty(DISC_VALUE1) ? DISC_VALUE1 : DBNull.Value,
                                !string.IsNullOrEmpty(DISC_VALUE2) ? DISC_VALUE2 : DBNull.Value,
                                !string.IsNullOrEmpty(DISC_VALUE3) ? DISC_VALUE3 : DBNull.Value,
                                !string.IsNullOrEmpty(BASE_TYPE) ? BASE_TYPE : DBNull.Value,
                                !string.IsNullOrEmpty(SUPP_STOCK_ID) ? SUPP_STOCK_ID : DBNull.Value,
                                !string.IsNullOrEmpty(BID_SUPP_STOCK_ID) ? BID_SUPP_STOCK_ID : DBNull.Value,
                                !string.IsNullOrEmpty(BID_BUYER_DISC) ? BID_BUYER_DISC : DBNull.Value,
                                !string.IsNullOrEmpty(BID_BUYER_AMOUNT) ? BID_BUYER_AMOUNT : DBNull.Value,
                                !string.IsNullOrEmpty(LUSTER) ? LUSTER : DBNull.Value,
                                !string.IsNullOrEmpty(SHADE) ? SHADE : DBNull.Value,
                                !string.IsNullOrEmpty(MILKY) ? MILKY : DBNull.Value,
                                !string.IsNullOrEmpty(GIRDLE_FROM) ? GIRDLE_FROM : DBNull.Value,
                                !string.IsNullOrEmpty(GIRDLE_TO) ? GIRDLE_TO : DBNull.Value,
                                !string.IsNullOrEmpty(GIRDLE_CONDITION) ? GIRDLE_CONDITION : DBNull.Value,
                                !string.IsNullOrEmpty(ORIGIN) ? ORIGIN : DBNull.Value,
                                !string.IsNullOrEmpty(EXTRA_FACET_TABLE) ? EXTRA_FACET_TABLE : DBNull.Value,
                                !string.IsNullOrEmpty(EXTRA_FACET_CROWN) ? EXTRA_FACET_CROWN : DBNull.Value,
                                !string.IsNullOrEmpty(EXTRA_FACET_PAV) ? EXTRA_FACET_PAV : DBNull.Value,
                                !string.IsNullOrEmpty(INTERNAL_GRAINING) ? INTERNAL_GRAINING : DBNull.Value,
                                !string.IsNullOrEmpty(H_A) ? H_A : DBNull.Value,
                                !string.IsNullOrEmpty(SUPPLIER_COMMENTS) ? SUPPLIER_COMMENTS : DBNull.Value);
                        }
                    }
                    else
                    {
                        DataRow newRow = dataTable.NewRow();
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            newRow[i] = DBNull.Value;
                        }
                        dataTable.Rows.Add(newRow);
                    }

                    DataTable supp_stock_dt = await _supplierService.Get_Excel_Report_Search(dataTable, "Customer", stock_Email_Model.Supplier_Ref_No);
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in supp_stock_dt.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }
                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string filename = "Stock_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Customer_Excel(supp_stock_dt, columnNamesTable, filePath, filePath + filename);
                    excelPath = Directory.GetCurrentDirectory() + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(excelPath);
                    using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                    {
                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                        //var employeeEmails = await _employeeService.GetEmployeeMail(user_Id ?? 0);
                        var emp_email = await _employeeService.Get_Employee_Email_Or_Default_Email(user_Id ?? 0);
                        if (emp_email != null)
                        {
                            if (!(stock_Email_Model.Send_From_Default ?? false) && (emp_email.Is_Default ?? false))
                                return Conflict(new
                                {
                                    statusCode = HttpStatusCode.Conflict,
                                    message = CoreCommonMessage.EmailSendFromDefaultSuccessMessage
                                });

                            IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "excelFile", Path.GetFileName(excelPath));
                            _emailSender.Send_Stock_Email(toEmail: stock_Email_Model.To_Email, externalLink: "", subject: CoreCommonMessage.StoneSelectionSubject, formFile: formFile, strBody: stock_Email_Model.Remarks, user_Id: user_Id ?? 0, employee_Mail: emp_email);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmailSendSuccessMessage
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Send_Stock_On_Email", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("send_cart_approval_order_email")]
        [Authorize]
        public async Task<IActionResult> Send_Cart_Approval_Order_Email(Cart_Approval_Order_Email_Model cart_Approval_Order_Email_Model)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                if (cart_Approval_Order_Email_Model.Report_Filter_Parameter != null)
                {
                    cart_Approval_Order_Email_Model.Report_Filter_Parameter.Add(new Report_Filter_Parameter()
                    {
                        Column_Name = "USER ID",
                        Category_Value = Convert.ToString(user_Id ?? 0),
                        Col_Id = 1163
                    });
                }
                var dt_stock = await _supplierService.Get_Report_Search_Excel(cart_Approval_Order_Email_Model.id, cart_Approval_Order_Email_Model.Report_Filter_Parameter);
                if (dt_stock != null && dt_stock.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_stock.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;
                    if (cart_Approval_Order_Email_Model.id == 2)
                    {
                        filename = "Cart_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Cart_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                    }
                    else if (cart_Approval_Order_Email_Model.id == 3)
                    {
                        filename = "Approval_Management_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Approval_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                    }
                    else if (cart_Approval_Order_Email_Model.id == 4)
                    {
                        filename = "Order_Processing_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                        EpExcelExport.Create_Order_Processing_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);
                    }
                    excelPath = Directory.GetCurrentDirectory() + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(excelPath);
                    using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                    {
                        //string user_Id = cart_Approval_Order_Email_Model.Report_Filter_Parameter.Where(x => x.Column_Name == "USER_ID").Select(x => x.Category_Value).FirstOrDefault();
                        //int? login_user_id = Convert.ToInt32(user_Id);
                        //var employeeEmails = await _employeeService.GetEmployeeMail(user_Id ?? 0);
                        var emp_email = await _employeeService.Get_Employee_Email_Or_Default_Email(user_Id ?? 0);
                        if (emp_email != null)
                        {
                            if (!(cart_Approval_Order_Email_Model.Send_From_Default ?? false) && (emp_email.Is_Default ?? false))
                                return Conflict(new
                                {
                                    statusCode = HttpStatusCode.Conflict,
                                    message = CoreCommonMessage.EmailSendFromDefaultSuccessMessage
                                });

                            IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "excelFile", Path.GetFileName(excelPath));
                            _emailSender.Send_Stock_Email(toEmail: cart_Approval_Order_Email_Model.To_Email, externalLink: "", subject: CoreCommonMessage.StoneSelectionSubject, formFile: formFile, strBody: cart_Approval_Order_Email_Model.Remarks, user_Id: user_Id ?? 0, employee_Mail: emp_email);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmailSendSuccessMessage
                        });
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Send_Cart_Approval_Order_Email", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("send_stock_availability_email")]
        [Authorize]
        public async Task<IActionResult> Send_Stock_Availability_Email([FromForm] Stock_Avalibility_Send_Email stock_Avalibility, IFormFile? File_Location)
        {
            try
            {
                IList<Stock_Avalibility_Values> stock_Avalibility_Values = new List<Stock_Avalibility_Values>();
                var lst_Stock_Id = new List<string>();
                if (!string.IsNullOrEmpty(stock_Avalibility.stock_Id) && stock_Avalibility.stock_Id.Contains(";"))
                {
                    lst_Stock_Id = stock_Avalibility.stock_Id.Split('/').ToList();
                    if (lst_Stock_Id != null && lst_Stock_Id.Count > 0)
                    {
                        foreach (var item in lst_Stock_Id)
                        {
                            var model = new Stock_Avalibility_Values();
                            var lst_stock_val = item.Split(';').ToList();
                            if (lst_stock_val != null && lst_stock_val.Count > 0)
                            {
                                if (lst_stock_val.Count == 1)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(lst_stock_val[i]))
                                            continue;

                                        model.Stock_Id = lst_stock_val[0];
                                    }
                                }
                                else if (lst_stock_val.Count == 2)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                        }
                                    }
                                }
                                else if (lst_stock_val.Count == 3)
                                {
                                    for (int i = 0; i < lst_stock_val.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            model.Stock_Id = lst_stock_val[0];
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lst_stock_val[i]))
                                                continue;

                                            var con_val = Convert.ToDecimal(lst_stock_val[i]);
                                            if (con_val >= -100 && con_val <= 100)
                                            {
                                                model.Offer_Disc = lst_stock_val[i];
                                            }
                                            else
                                            {
                                                model.Offer_Amount = lst_stock_val[i];
                                            }
                                            //var con_val_1 = Convert.ToDecimal(lst_stock_val[2]);
                                            //if (con_val_1 >= -100 && con_val_1 <= 100)
                                            //{
                                            //    model.Offer_Disc = lst_stock_val[2];
                                            //}
                                            //else
                                            //{
                                            //    model.Offer_Amount = lst_stock_val[2];
                                            //}
                                        }
                                    }
                                }
                            }
                            stock_Avalibility_Values.Add(model);
                        }
                        stock_Avalibility_Values = stock_Avalibility_Values.GroupBy(x => x.Stock_Id).Select(x => x.First()).ToList();
                    }
                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("STOCK_ID", typeof(string));
                dataTable.Columns.Add("OFFER_AMOUNT", typeof(string));
                dataTable.Columns.Add("OFFER_DISC", typeof(string));
                if (File_Location != null)
                {
                    var filePath = Path.GetTempFileName();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File_Location.CopyToAsync(stream);
                    }
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int startRow = 2;

                        HashSet<string> uniqueValues = new HashSet<string>();
                        for (int row = startRow; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string value = worksheet.Cells[row, 1].GetValue<string>();
                            string offer_amt = worksheet.Cells[row, 3].GetValue<string>();
                            if (!uniqueValues.Contains(value))
                            {
                                uniqueValues.Add(value);
                                dataTable.Rows.Add(value, !string.IsNullOrEmpty(offer_amt) ? offer_amt.Replace(",", "") : DBNull.Value, worksheet.Cells[row, 2].GetValue<string>());
                            }
                        }
                        var uniqueRows = dataTable.AsEnumerable()
                                  .GroupBy(row => row.Field<string>("STOCK_ID"))
                                  .Select(grp => grp.First())
                                  .CopyToDataTable();
                    }
                }
                else if (stock_Avalibility_Values != null && stock_Avalibility_Values.Count > 0)
                {
                    stock_Avalibility.stock_Id = null;
                    foreach (var item in stock_Avalibility_Values)
                    {
                        dataTable.Rows.Add(!string.IsNullOrEmpty(item.Stock_Id) ? item.Stock_Id : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Amount) ? item.Offer_Amount : DBNull.Value, !string.IsNullOrEmpty(item.Offer_Disc) ? item.Offer_Disc : DBNull.Value);
                    }
                }
                var dt_stock = await _supplierService.Get_Stock_Availability_Report_Excel(dataTable, stock_Avalibility.stock_Id, stock_Avalibility.stock_Type);
                if (dt_stock != null && dt_stock.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_stock.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Stock_Availability_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Stock_Availability_Excel(dt_stock, columnNamesTable, filePath, filePath + filename);

                    excelPath = Directory.GetCurrentDirectory() + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(excelPath);
                    using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                    {
                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                        int? login_user_id = Convert.ToInt32(user_Id);
                        //var employeeEmails = await _employeeService.GetEmployeeMail(user_Id ?? 0);
                        var emp_email = await _employeeService.Get_Employee_Email_Or_Default_Email(user_Id ?? 0);
                        if (emp_email != null)
                        {
                            if (!(stock_Avalibility.Send_From_Default ?? false) && (emp_email.Is_Default ?? false))
                                return Conflict(new
                                {
                                    statusCode = HttpStatusCode.Conflict,
                                    message = CoreCommonMessage.EmailSendFromDefaultSuccessMessage
                                });

                            IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "excelFile", Path.GetFileName(excelPath));
                            _emailSender.Send_Stock_Email(toEmail: stock_Avalibility.To_Email, externalLink: "", subject: CoreCommonMessage.StoneSelectionSubject, formFile: formFile, strBody: stock_Avalibility.Remarks, user_Id: login_user_id ?? 0, employee_Mail: emp_email);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.EmailSendSuccessMessage
                        });
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Send_Stock_Availability_Email", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Purchase Order
        [HttpPost]
        [Route("purchase_order")]
        [Authorize]
        public async Task<IActionResult> Purchase_Order(string supp_ref_No)
        {
            try
            {
                if (!string.IsNullOrEmpty(supp_ref_No))
                {
                    var lst_sup_ref_no = supp_ref_No.Split(",").ToList();
                    if (lst_sup_ref_no != null && lst_sup_ref_no.Count > 0)
                    {
                        foreach (var obj_ref_no in lst_sup_ref_no)
                        {
                            if (!string.IsNullOrEmpty(obj_ref_no))
                            {
                                var supplier = await _supplierService.Get_Purchase_Order_Supplier(obj_ref_no);
                                if (supplier != null && !string.IsNullOrEmpty(supplier.Name))
                                {
                                    if (supplier.Name.Equals("J.B. AND BROTHERS PVT. LTD - (S)"))
                                    {

                                    }
                                    else if (supplier.Name.Equals("SHAIRU GEMS DIAMONDS PVT. LTD - (S)"))
                                    {

                                    }
                                    else if (supplier.Name.Equals("RATNA"))
                                    {

                                    }
                                    else if (supplier.Name.Equals("VENUS JEWEL - (S)"))
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Purchase_Order", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Lab User
        [HttpPost]
        [Route("create_update_lab_user")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Lab_User(Lab_User_Detail lab_User_Detail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("User_Name", typeof(string));
                    dataTable.Columns.Add("Password", typeof(string));
                    dataTable.Columns.Add("Active_Status", typeof(bool));
                    dataTable.Columns.Add("User_Type", typeof(string));
                    dataTable.Columns.Add("Stock_View", typeof(bool));
                    dataTable.Columns.Add("Stock_Download", typeof(bool));
                    dataTable.Columns.Add("Order_Placed", typeof(bool));
                    dataTable.Columns.Add("Enable_Status", typeof(bool));
                    dataTable.Columns.Add("Last_Login_Date", typeof(string));
                    dataTable.Columns.Add("Query_Flag", typeof(string));

                    if (lab_User_Detail.Lab_User_Masters != null && lab_User_Detail.Lab_User_Masters.Count > 0)
                    {
                        foreach (var item in lab_User_Detail.Lab_User_Masters)
                        {
                            dataTable.Rows.Add(item.Id,
                                !string.IsNullOrEmpty(item.User_Name) ? item.User_Name : DBNull.Value,
                                !string.IsNullOrEmpty(item.Password) ? CoreService.Encrypt(item.Password) : DBNull.Value,
                                item.Active_Status,
                                !string.IsNullOrEmpty(item.User_Type) ? item.User_Type : DBNull.Value,
                                item.Stock_View,
                                item.Stock_Download,
                                item.Order_Placed,
                                item.Enable_Status,
                                DBNull.Value,
                                item.Query_Flag);
                        }
                        var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                        int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                        var result = await _labUserService.Create_Update_Lab_User(dataTable, lab_User_Detail.Party_Id, user_Id ?? 0);
                        if (result == 409)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = "User name already exists."
                            });
                        }
                        if (result == 410)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = "Primary user already exists."
                            });
                        }
                        else if (result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = "Lab user saved successfully."
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (SqlException ex)
            {
                string message = ex.Message;
                if (ex.Number == 515)
                {
                    if (message.Contains("User_Name") && message.Contains("Password"))
                    {
                        message = "User name and password is required.";
                    }
                    else if (message.Contains("User_Name"))
                    {
                        message = "User name is required.";
                    }
                    else if (message.Contains("Password"))
                    {
                        message = "Password is required.";
                    }
                    else if (message.Contains("User_Type"))
                    {
                        message = "User type is required.";
                    }
                }
                else if (ex.Number == 547)
                {
                    message = "Company name is required.";
                }
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Lab_User", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = message
                });
            }
        }

        [HttpGet]
        [Route("get_lab_user")]
        [Authorize]
        public async Task<IActionResult> Get_Lab_User(int id, int party_Id)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _labUserService.Get_Lab_User(id, party_Id, user_Id ?? 0);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        party_Id = party_Id > 0 ? party_Id : 0,
                        party_Name = party_Id > 0 ? result.Where(x => x.ContainsKey("Party_Name")).Select(x => x["Party_Name"]).FirstOrDefault() : "",
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Lab_User", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("change_lab_user_active_status")]
        [Authorize]
        public async Task<IActionResult> Change_Lab_User_Active_Status(int id, bool active_Status)
        {
            try
            {
                var result = await _labUserService.Change_Active_Status(id, active_Status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Change_Lab_User_Active_Status", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_suspend_day")]
        [Authorize]
        public async Task<IActionResult> Get_Suspend_Day()
        {
            try
            {
                var result = await _labUserService.Get_Suspend_Day();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Suspend_Day", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_lab_user")]
        [Authorize]
        public async Task<IActionResult> Delete_Lab_User(int id, bool check_Primary_User)
        {
            try
            {
                var (result, message) = await _labUserService.Delete_Lab_User(id, check_Primary_User);
                if (result > 0 && message == "success")
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Lab user deleted successfully.",
                    });
                }
                else if (result == 409 && message == "exist")
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "Sub user available are you sure you want to delete?",
                    });
                }
                else if (result == 409 && message == "subExist")
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "Are you sure you want to delete?",
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Lab_User", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_suspend_days")]
        [Authorize]
        public async Task<IActionResult> Update_Suspend_Days(int id, int days)
        {
            try
            {
                var result = await _labUserService.Create_Update_Suspend_Days(id, days);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Suspend days updated successfully."
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Suspend_Days", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_customer_lab_user")]
        [Authorize]
        public async Task<IActionResult> Get_Customer_Lab_User(int party_Id)
        {
            try
            {
                var result = await _labUserService.Get_Customer_Lab_User(party_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Customer_Lab_User", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        #endregion

        #region Customer API/FILE/FTP
        [HttpPost]
        [Route("create_update_customer_detail")]
        [Authorize]
        public virtual async Task<IActionResult> Create_Update_Customer_Detail(List<Customer_Detail> customer_Details_data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    bool success = false;
                    foreach (var customer_Details in customer_Details_data)
                    {
                        if (customer_Details.Customer_Party_Api != null && customer_Details.Customer_Party_Api.Customer_Column_Caption != null && customer_Details.Customer_Party_Api.Customer_Column_Caption.Count > 0)
                        {
                            customer_Details.Customer_Party_Api.Party_Id = customer_Details.Party_Id;
                            var party_ftp = await _partyService.Add_Update_Customer_Party_API(customer_Details.Customer_Party_Api, user_Id ?? 0, customer_Details.Map_Flag);
                            if (party_ftp > 0)
                            {
                                success = true;
                                if (customer_Details.Customer_Party_Api.Customer_Column_Caption != null && customer_Details.Customer_Party_Api.Customer_Column_Caption.Count > 0)
                                {

                                    DataTable dataTable = new DataTable();
                                    dataTable.Columns.Add("Party_Id", typeof(int));
                                    dataTable.Columns.Add("Col_Id", typeof(int));
                                    dataTable.Columns.Add("Caption_Name", typeof(string));
                                    dataTable.Columns.Add("Upload_Method", typeof(string));
                                    dataTable.Columns.Add("Status", typeof(bool));
                                    dataTable.Columns.Add("Map_Flag", typeof(string));

                                    foreach (var item in customer_Details.Customer_Party_Api.Customer_Column_Caption)
                                    {
                                        dataTable.Rows.Add(customer_Details.Party_Id, item.Col_Id, item.Caption_Name, "API", item.Status, customer_Details.Map_Flag);
                                    }
                                    var column_Caption = await _partyService.Add_Update_Customer_Column_Caption(dataTable, user_Id ?? 0);
                                    if (column_Caption > 0)
                                    {
                                        success = true;
                                    }
                                }

                            }
                        }
                        if (customer_Details.Customer_Party_FTP != null && customer_Details.Customer_Party_FTP.Customer_Column_Caption != null && customer_Details.Customer_Party_FTP.Customer_Column_Caption.Count > 0)
                        {
                            customer_Details.Customer_Party_FTP.Party_Id = customer_Details.Party_Id;
                            var party_ftp = await _partyService.Add_Update_Customer_Party_FTP(customer_Details.Customer_Party_FTP, user_Id ?? 0, customer_Details.Map_Flag);
                            if (party_ftp > 0)
                            {
                                success = true;
                                if (customer_Details.Customer_Party_FTP.Customer_Column_Caption != null && customer_Details.Customer_Party_FTP.Customer_Column_Caption.Count > 0)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Columns.Add("Party_Id", typeof(int));
                                    dataTable.Columns.Add("Col_Id", typeof(int));
                                    dataTable.Columns.Add("Caption_Name", typeof(string));
                                    dataTable.Columns.Add("Upload_Method", typeof(string));
                                    dataTable.Columns.Add("Status", typeof(bool));
                                    dataTable.Columns.Add("Map_Flag", typeof(string));

                                    foreach (var item in customer_Details.Customer_Party_FTP.Customer_Column_Caption)
                                    {
                                        dataTable.Rows.Add(customer_Details.Party_Id, item.Col_Id, item.Caption_Name, "FTP", item.Status, customer_Details.Map_Flag);
                                    }
                                    var column_Caption = await _partyService.Add_Update_Customer_Column_Caption(dataTable, user_Id ?? 0);
                                    if (column_Caption > 0)
                                    {
                                        success = true;
                                    }
                                }

                            }
                        }
                        if (customer_Details.Customer_Party_File != null && customer_Details.Customer_Party_File.Customer_Column_Caption != null && customer_Details.Customer_Party_File.Customer_Column_Caption.Count > 0)
                        {
                            customer_Details.Customer_Party_File.Party_Id = customer_Details.Party_Id;
                            var party_file = await _partyService.Add_Update_Customer_Party_File(customer_Details.Customer_Party_File, user_Id ?? 0, customer_Details.Map_Flag);
                            if (party_file > 0)
                            {
                                success = true;
                                if (customer_Details.Customer_Party_File.Customer_Column_Caption != null && customer_Details.Customer_Party_File.Customer_Column_Caption.Count > 0)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Columns.Add("Party_Id", typeof(int));
                                    dataTable.Columns.Add("Col_Id", typeof(int));
                                    dataTable.Columns.Add("Caption_Name", typeof(string));
                                    dataTable.Columns.Add("Upload_Method", typeof(string));
                                    dataTable.Columns.Add("Status", typeof(bool));
                                    dataTable.Columns.Add("Map_Flag", typeof(string));

                                    foreach (var item in customer_Details.Customer_Party_File.Customer_Column_Caption)
                                    {
                                        dataTable.Rows.Add(customer_Details.Party_Id, item.Col_Id, item.Caption_Name, "URL", item.Status, customer_Details.Map_Flag);
                                    }
                                    var column_Caption = await _partyService.Add_Update_Customer_Column_Caption(dataTable, user_Id ?? 0);
                                    if (column_Caption > 0)
                                    {
                                        success = true;
                                    }

                                }
                            }
                        }
                    }
                    if (success)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CustomerDetailSavedSuccessfully
                        });
                    }
                    else
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CustomerDetailRequired
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Customer_Detail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet]
        [Route("get_customer_pricing_column_caption")]
        [Authorize]
        public async Task<IActionResult> Get_Customer_Pricing_Column_Caption()
        {
            try
            {
                var result = await _partyService.Get_Customer_Pricing_Column_Caption(null, null, null);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Customer_Pricing_Column_Caption", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_customer_pricing_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Customer_Pricing_Detail(int party_Id, string map_flag)
        {
            try
            {
                Customer_Detail customer_Detail = new Customer_Detail();
                customer_Detail.Customer_Party_Api = await _partyService.Get_Customer_Party_API(null, party_Id, map_flag);
                var Customer_Party_Api = await _partyService.Get_Customer_Pricing_Column_Caption(party_Id, map_flag, "API");
                if (customer_Detail.Customer_Party_Api != null && Customer_Party_Api != null && Customer_Party_Api.Count > 0)
                {
                    customer_Detail.Customer_Party_Api.Customer_Column_Caption = Customer_Party_Api;
                }
                customer_Detail.Customer_Party_File = await _partyService.Get_Customer_Party_File(null, party_Id, map_flag);
                var Customer_Party_File = await _partyService.Get_Customer_Pricing_Column_Caption(party_Id, map_flag, "URL");
                if (customer_Detail.Customer_Party_File != null && Customer_Party_File != null && Customer_Party_File.Count > 0)
                {
                    customer_Detail.Customer_Party_File.Customer_Column_Caption = Customer_Party_File;
                }
                customer_Detail.Customer_Party_FTP = await _partyService.Get_Customer_Party_FTP(null, party_Id, map_flag);
                var Customer_Party_FTP = await _partyService.Get_Customer_Pricing_Column_Caption(party_Id, map_flag, "FTP");
                if (customer_Detail.Customer_Party_FTP != null && Customer_Party_FTP != null && Customer_Party_FTP.Count > 0)
                {
                    customer_Detail.Customer_Party_FTP.Customer_Column_Caption = Customer_Party_FTP;
                }
                if (customer_Detail != null && (customer_Detail.Customer_Party_FTP != null || customer_Detail.Customer_Party_Api != null || customer_Detail.Customer_Party_File != null))
                {
                    customer_Detail.Party_Id = party_Id;
                    customer_Detail.Map_Flag = customer_Detail.Customer_Party_FTP != null ? customer_Detail.Customer_Party_FTP.Map_Flag :
                        (customer_Detail.Customer_Party_Api != null ? customer_Detail.Customer_Party_Api.Map_Flag : (customer_Detail.Customer_Party_File != null ? customer_Detail.Customer_Party_File.Map_Flag : map_flag));
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = customer_Detail
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Customer_Pricing_Detail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Order Processing New
        [HttpPost]
        [Route("get_order_summary")]
        [Authorize]
        public async Task<IActionResult> Get_Order_Summary(Order_Processing_Summary order_Processing_Summary)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _supplierService.Get_Order_Summary(user_Id ?? 0, order_Processing_Summary);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Order_Summary", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_stone_order_process")]
        [Authorize]
        public async Task<IActionResult> Create_Stone_Order_Process(Order_Stone_Process order_Stone_Process)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var result = await _supplierService.Create_Stone_Order_Process(order_Stone_Process, user_Id ?? 0);
                    if (result > 0)
                    {
                        var employees = await _employeeService.GetEmployees(user_Id ?? 0, null, null);
                        var employee = employees.FirstOrDefault();
                        var (dt_Order, is_Admin) = await _supplierService.Get_Order_Excel_Data(null, user_Id ?? 0, order_Stone_Process.Order_Id);
                        if (dt_Order != null && dt_Order.Rows.Count > 0)
                        {
                            List<string> columnNames = new List<string>();
                            foreach (DataColumn column in dt_Order.Columns)
                            {
                                columnNames.Add(column.ColumnName);
                            }

                            DataTable columnNamesTable = new DataTable();
                            columnNamesTable.Columns.Add("Column_Name", typeof(string));

                            foreach (string columnName in columnNames)
                            {
                                columnNamesTable.Rows.Add(columnName);
                            }
                            var excelPath = string.Empty;
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                            if (!(Directory.Exists(filePath)))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            string filename = string.Empty;

                            filename = "Order_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                            EpExcelExport.Create_Order_Processing_Excel_User(dt_Order, columnNamesTable, filePath, filePath + filename);

                            excelPath = Directory.GetCurrentDirectory() + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;
                            byte[] fileBytes = System.IO.File.ReadAllBytes(excelPath);
                            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                            {
                                var emp_email = await _employeeService.Get_Employee_Email_Or_Default_Email(0);
                                if (emp_email != null)
                                {
                                    IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "excelFile", Path.GetFileName(excelPath));
                                    _emailSender.Send_Stock_Email(toEmail: employee.Personal_Email, externalLink: "", subject: CoreCommonMessage.StoneSelectionSubject, formFile: formFile, user_Id: user_Id ?? 0, employee_Mail: emp_email);
                                }
                            }
                        }

                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Stone has been proceed successfully."
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Stone_Order_Process", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get_order_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Order_Detail(Order_Process_Detail order_Process_Detail)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _supplierService.Get_Order_Detail(user_Id ?? 0, order_Process_Detail);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Order_Detail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("order_processing_delete")]
        public async Task<IActionResult> Order_Processing_Delete(string Order_No, int Sub_Order_Id)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _supplierService.Delete_Order_Process(Order_No, Sub_Order_Id, user_Id ?? 0);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Order deleted successfully.",
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Order_Detail", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("order_process_request_accept")]
        [Authorize]
        public async Task<IActionResult> Order_Process_Request_Accept(Order_Process_Detail order_Process_Detail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    var result = await _supplierService.Accept_Request_Order_Process(order_Process_Detail, user_Id ?? 0);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Order request accepted successfully."
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Order_Process_Request_Accept", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("order_stones_delete")]
        [Authorize]
        public async Task<IActionResult> Order_Stones_Delete(string order_Id)
        {
            try
            {
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var result = await _supplierService.Delete_Order_Stones(order_Id, user_Id ?? 0);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Order stones deleted successfully.",
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Order_Stones_Delete", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("order_processing_reply_to_assist")]
        [Authorize]
        public async Task<IActionResult> Order_Processing_Reply_To_Assist(Order_Processing_Reply_To_Assist order_Processing_Reply_To_Assist)
        {
            try
            {
                IList<Order_Processing_Reply_To_Assist_Detail> OrderResult = JsonConvert.DeserializeObject<IList<Order_Processing_Reply_To_Assist_Detail>>(order_Processing_Reply_To_Assist.Order_Detail.ToString());

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Remarks", typeof(string));
                dataTable.Columns.Add("Cost_Disc", typeof(double));
                dataTable.Columns.Add("Cost_Amt", typeof(double));

                foreach (var item in OrderResult)
                {
                    dataTable.Rows.Add(item.Id, Convert.ToString(item.Status), Convert.ToString(item.Remarks),
                        (item.Cost_Disc != null ? !string.IsNullOrEmpty(item.Cost_Disc.ToString()) ? Convert.ToDouble(item.Cost_Disc.ToString()) : null : null),
                        (item.Cost_Amt != null ? !string.IsNullOrEmpty(item.Cost_Amt.ToString()) ? Convert.ToDouble(item.Cost_Amt.ToString()) : null : null));
                }

                var result = await _supplierService.Order_Processing_Reply_To_Assist(dataTable, order_Processing_Reply_To_Assist.Order_No, order_Processing_Reply_To_Assist.Sub_Order_Id ?? 0);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Order partially completed successfully."
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Order_Processing_Reply_To_Assist", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("order_processing_completed")]
        [Authorize]
        public async Task<IActionResult> Order_Processing_Completed(Order_Processing_Reply_To_Assist order_Processing_Reply_To_Assist)
        {
            try
            {
                IList<Order_Processing_Reply_To_Assist_Detail> OrderResult = JsonConvert.DeserializeObject<IList<Order_Processing_Reply_To_Assist_Detail>>(order_Processing_Reply_To_Assist.Order_Detail.ToString());

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Remarks", typeof(string));
                dataTable.Columns.Add("Cost_Disc", typeof(double));
                dataTable.Columns.Add("Cost_Amt", typeof(double));

                foreach (var item in OrderResult)
                {
                    dataTable.Rows.Add(item.Id, Convert.ToString(item.Status), Convert.ToString(item.Remarks),
                        (item.Cost_Disc != null ? !string.IsNullOrEmpty(item.Cost_Disc.ToString()) ? Convert.ToDouble(item.Cost_Disc.ToString()) : null : null),
                        (item.Cost_Amt != null ? !string.IsNullOrEmpty(item.Cost_Amt.ToString()) ? Convert.ToDouble(item.Cost_Amt.ToString()) : null : null));
                }

                var result = await _supplierService.Order_Processing_Completed(dataTable, order_Processing_Reply_To_Assist.Order_No, order_Processing_Reply_To_Assist.Sub_Order_Id ?? 0);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = "Order completed successfully."
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "order_processing_completed", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("order_excel_export")]
        [Authorize]
        public async Task<IActionResult> Order_Excel_Export(Report_Filter report_Filter)
        {
            try
            {   
                var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                var (dt_Order, is_Admin) = await _supplierService.Get_Order_Excel_Data(report_Filter.Report_Filter_Parameter, user_Id ?? 0, null);
                if (dt_Order != null && dt_Order.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_Order.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Order_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    if (is_Admin)
                    {
                        EpExcelExport.Create_Order_Processing_Excel_Admin(dt_Order, columnNamesTable, filePath, filePath + filename);
                    }
                    else
                    {
                        EpExcelExport.Create_Order_Processing_Excel_User(dt_Order, columnNamesTable, filePath, filePath + filename);
                    }
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Order_Excel_Export", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_company_name")]
        [Authorize]
        public async Task<IActionResult> Get_Company_Name()
        {
            try
            {
                var result = await _supplierService.Get_Company_Name();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Company_Name", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Account Group Master
        [HttpGet]
        [Route("get_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Group(int ac_Group_Code)
        {
            try
            {
                var result = await _account_Group_Service.Get_Account_Group(ac_Group_Code);
                if(result != null && result.Count > 0)
                {
                    return Ok(new 
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_account_group")]
        [Authorize]
        public async Task<IActionResult> Create_Account_Group(Account_Group_Master account_Group_Master)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var result = await _account_Group_Service.Create_Update_Account_Group(account_Group_Master);
                    if(result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.AccountGroupCreated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_account_group")]
        [Authorize]
        public async Task<IActionResult> Update_Account_Group(Account_Group_Master account_Group_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _account_Group_Service.Create_Update_Account_Group(account_Group_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.AccountGroupUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_account_group")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Group(int ac_Group_Code)
        {
            try
            {
                var result = await _account_Group_Service.Delete_Account_Group(ac_Group_Code);
                if(result > 0)
                {
                    return Ok(new 
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountGroupDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("account_group_excel_download")]
        [Authorize]
        public async Task<IActionResult> Account_Group_Excel_Download()
        {
            try
            {
                var dt_acc_group = await _account_Group_Service.Get_Account_Group_Excel();
                if (dt_acc_group != null && dt_acc_group.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_acc_group.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Account_Group_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Account_Group_Excel(dt_acc_group, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Account_Group_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_perent_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Perent_Account_Group()
        {
            try
            {
                var result = await _account_Group_Service.Get_Perent_Account_Group();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "get_perent_account_group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_sub_account_group")]
        [Authorize]
        public async Task<IActionResult> Get_Sub_Account_Group(int Id)
        {
            try
            {
                var result = await _account_Group_Service.Get_Sub_Account_Group(Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "get_sub_account_group", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Account Master

        [HttpGet]
        [Route("get_account_master")]
        [Authorize]
        public async Task<IActionResult> Get_Account_Master(int account_Id)
        {
            try
            {
                var result = await _account_Master_Service.Get_Account_Master(account_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_account_master")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Account_Master(Account_Master account_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    account_Master.User_Id = _jWTAuthentication.Validate_Jwt_Token(token);

                    var result = await _account_Master_Service.Create_Update_Account_Master(account_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = account_Master.Account_Id == 0 ? CoreCommonMessage.AccountMasterCreated : CoreCommonMessage.AccountMasterUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_account_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Account_Master(int account_Id)
        {
            try
            {
                var result = await _account_Master_Service.Delete_Account_Master(account_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.AccountMasterDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Delete_Account_Master", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("account_master_excel_download")]
        [Authorize]
        public async Task<IActionResult> Account_Master_Excel_Download()
        {
            try
            {
                var dt_acc_group = await _account_Master_Service.Get_Account_Master_Excel();
                if (dt_acc_group != null && dt_acc_group.Rows.Count > 0)
                {
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dt_acc_group.Columns)
                    {
                        columnNames.Add(column.ColumnName);
                    }

                    DataTable columnNamesTable = new DataTable();
                    columnNamesTable.Columns.Add("Column_Name", typeof(string));

                    foreach (string columnName in columnNames)
                    {
                        columnNamesTable.Rows.Add(columnName);
                    }
                    var excelPath = string.Empty;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/DownloadStockExcelFiles/");
                    if (!(Directory.Exists(filePath)))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string filename = string.Empty;

                    filename = "Account_Master_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
                    EpExcelExport.Create_Account_Group_Excel(dt_acc_group, columnNamesTable, filePath, filePath + filename);
                    excelPath = _configuration["BaseUrl"] + CoreCommonFilePath.DownloadStockExcelFilesPath + filename;

                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        result = excelPath,
                        file_name = filename
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Account_Master_Excel_Download", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        #endregion
    }
}
