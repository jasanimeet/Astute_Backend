using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        #endregion

        #region Ctor
        public PartyController(IPartyService partyService,
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            ISupplierService supplierService,
            ICartService cartService)
        {
            _partyService = partyService;
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _supplierService = supplierService;
            _cartService = cartService;
        }
        #endregion

        #region Utilities
        private DataTable Set_Column_In_Datatable(DataTable dt_stock_data, IList<Stock_Data> stock_Datas)
        {
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
                foreach (var item in stock_Datas)
                {
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
                        item.TABLE_BLACK ?? null,
                        item.SIDE_BLACK ?? null,
                        item.TABLE_WHITE ?? null,
                        item.SIDE_WHITE ?? null,
                        item.TABLE_OPEN ?? null,
                        item.CROWN_OPEN ?? null,
                        item.PAVILION_OPEN ?? null,
                        item.GIRDLE_OPEN ?? null,
                        item.GIRDLE_FROM ?? null,
                        item.GIRDLE_TO ?? null,
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
        private DataTable Set_Column_In_Datatable_Scheduler(DataTable dt_stock_data, IList<Stock_Data_Schedular> stock_Datas)
        {
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
                foreach (var item in stock_Datas)
                {
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
                        item.TABLE_BLACK != null ? Convert.ToString(item.TABLE_BLACK) : DBNull.Value,
                        item.SIDE_BLACK != null ? Convert.ToString(item.SIDE_BLACK) : DBNull.Value,
                        item.TABLE_WHITE != null ? Convert.ToString(item.TABLE_WHITE) : DBNull.Value,
                        item.SIDE_WHITE != null ? Convert.ToString(item.SIDE_WHITE) : DBNull.Value,
                        item.TABLE_OPEN != null ? Convert.ToString(item.TABLE_OPEN) : DBNull.Value,
                        item.CROWN_OPEN != null ? Convert.ToString(item.CROWN_OPEN) : DBNull.Value,
                        item.PAVILION_OPEN != null ? Convert.ToString(item.PAVILION_OPEN) : DBNull.Value,
                        item.GIRDLE_OPEN != null ? Convert.ToString(item.GIRDLE_OPEN) : DBNull.Value,
                        item.GIRDLE_FROM != null ? Convert.ToString(item.GIRDLE_FROM) : DBNull.Value,
                        item.GIRDLE_TO != null ? Convert.ToString(item.GIRDLE_TO) : DBNull.Value,
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
                        item.IMAGE_LINK != null ? Convert.ToString(item.IMAGE_LINK) : DBNull.Value,
                        item.Image2 != null ? Convert.ToString(item.Image2) : DBNull.Value,
                        item.VIDEO_LINK != null ? Convert.ToString(item.VIDEO_LINK) : DBNull.Value,
                        item.Video2 != null ? Convert.ToString(item.Video2) : DBNull.Value,
                        item.CERTIFICATE_LINK != null ? Convert.ToString(item.CERTIFICATE_LINK) : DBNull.Value,
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

            if(gIA_Lab_Parameters != null && gIA_Lab_Parameters.Count > 0)
            {
                foreach (var item in gIA_Lab_Parameters)
                {
                    dataTable.Rows.Add(item.Report_No,item.Report_Date, item.Length, item.Width, item.Depth, item.Color_Grade, item.Clarity_Grade, item.Cut_Grade, item.Polish_Grade
                                        ,item.Symmetry_Grade, item.Fluorescence, item.Inscriptions, item.Key_To_Symbols, item.Report_Comments, item.Table_Pct, item.Depth_Pct, item.Crown_Angle
                                        ,item.Crown_Height, item.Pavilion_Angle, item.Pavilion_Depth, item.Star_Length, item.Lower_Half, item.Shape_Code, item.Shape_Group, item.Carats
                                        ,item.Clarity, item.Cut, item.Polish, item.Symmetry, item.Fluorescence_Intensity, item.Fluorescence_Color, item.Girdle_Condition, item.Girdle_Condition_Code
                                        ,item.Girdle_Pct, item.Girdle_Size, item.Girdle_Size_Code, item.Culet_Code, item.Certificate_PDF, item.Plotting_Diagram, item.Proportions_Diagram
                                        ,item.Digital_Card);
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
        public async Task<IActionResult> GetParty(int party_Id)
        {
            try
            {
                var result = await _partyService.GetParty(party_Id);
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
                                dataTable.Rows.Add(item.Ship_Id, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Mobile_No_Country_Code, item.Phone_No, item.Phone_No_Country_Code, item.Contact_Person, item.TIN_No, item.Default_Address, item.Status, item.QueryFlag);
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
                        var party_Api = await _partyService.Add_Update_Party_API(supplier_Details.Party_Api);
                        if (party_Api > 0)
                        {
                            success = true;
                        }
                    }
                    if (supplier_Details.Party_FTP != null)
                    {
                        supplier_Details.Party_FTP.Party_Id = supplier_Details.Party_Id;
                        var party_ftp = await _partyService.Add_Update_Party_FTP(supplier_Details.Party_FTP);
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
                        var party_file = await _partyService.Add_Update_Party_File(supplier_Details.Party_File);
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

                        foreach (var item in supplier_Details.Supplier_Column_Mapping_List)
                        {
                            dataTable.Rows.Add(item.Supp_Col_Id, supplier_Details.Party_Id, item.Col_Id, item.Supp_Col_Name, item.Column_Type, item.Column_Synonym);
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
                        dataTable.Columns.Add("Status", typeof(bool));

                        foreach (var item in supplier_Details.Supplier_Value_Mapping_List)
                        {
                            dataTable.Rows.Add(supplier_Details.Party_Id, item.Supp_Cat_Name, item.Cat_val_Id, item.Status);
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
        public async Task<IActionResult> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Pricing(supplier_Pricing_Id, supplier_Id, supplier_Filter_Type, map_Flag, sunrise_pricing_Id, customer_pricing_Id);
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
                    int sun_price_Id = 0;
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
                                    var result_Supplier_Pricing_Key_To_Symbol = await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(item.Supplier_Pricing_Id);

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
                                        if (item.Comments != null && item.Comments.Count > 0)
                                        {
                                            DataTable dataTable = new DataTable();
                                            dataTable.Columns.Add("Supplier_Pricing_Id", typeof(int));
                                            dataTable.Columns.Add("Cat_Val_Id", typeof(int));
                                            dataTable.Columns.Add("Symbol_Status", typeof(bool));
                                            dataTable.Columns.Add("Filter_Type", typeof(string));
                                            foreach (var obj in item.Comments)
                                            {
                                                dataTable.Rows.Add(supplier_pricing_Id, obj.Cat_Val_Id, obj.Symbol_Status, obj.Filter_Type);
                                            }
                                            await _supplierService.Add_Update_Supplier_Pricing_Key_To_Symbol(dataTable);
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
                                customer_Pricing_Id = customer_Pricing_Id
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
        public async Task<IActionResult> Delete_Customer_Pricing(int customer_Pricing_Id)
        {
            try
            {
                var result = await _supplierService.Delete_Customer_Pricing(customer_Pricing_Id);
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
                            dt_our_stock_data = Set_Column_In_Datatable(new DataTable(), stock_Data_Master.Stock_Data_List);
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
                            dt_our_stock_data = Set_Column_In_Datatable_Scheduler(new DataTable(), stock_Data_Master.Stock_Data_List);
                            var result = await _supplierService.Stock_Data_Shedular_Insert_Update(dt_our_stock_data, stock_Data_Id);
                            if (result > 0)
                            {
                                if (stock_Data_Master.Upload_Type == "O")
                                {
                                    await _supplierService.Supplier_Stock_Insert_Update((int)stock_Data_Master.Supplier_Id, stock_Data_Id);
                                }
                            }
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
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Stock_By_Scheduler", ex.StackTrace);
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
        public async Task<IActionResult> Create_Stock_Number_Generation(IList<Stock_Number_Generation> stock_Number_Generations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = stock_Number_Generations.Select(x => x.Id).FirstOrDefault();
                    if (stock_Number_Generations != null && stock_Number_Generations.Count > 0)
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

                        foreach (var item in stock_Number_Generations)
                        {
                            dataTable.Rows.Add(item.Id, item.Exc_Party_Id, item.Pointer_Id, item.Shape, item.Stock_Type, item.Front_Prefix, item.Back_Prefix, item.Front_Prefix_Alloted, item.Start_Format, item.Start_Number, item.End_Number, item.Supplier_Id, id > 0 ? 'U' : 'I');
                        }

                        var result = await _supplierService.Add_Update_Stock_Number_Generation(dataTable);
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
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StockNumberDeleted
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
            try
            {
                if (ModelState.IsValid)
                {
                    var supp_col_Map = await _supplierService.Get_Supplier_Column_Mapping(party_File.Party_Id ?? 0, "C", "FILE");
                    int? col_Map_sup_Id = supp_col_Map.Select(x => x.Supp_Id).FirstOrDefault();
                    if (col_Map_sup_Id > 0)
                    {
                        #region Update Party File
                        var party_file_obj = await _partyService.Get_Party_File(0, party_File.Party_Id ?? 0);
                        if (party_file_obj != null)
                        {
                            party_file_obj.Sheet_Name = party_File.Sheet_Name;
                            party_file_obj.Validity_Days = party_File.Validity_Days;
                            party_file_obj.API_Flag = party_File.API_Flag;
                            party_file_obj.Exclude = party_File.Exclude;
                            party_file_obj.Overseas_Same_Id = party_File.Overseas_Same_Id;

                            var result = await _partyService.Add_Update_Party_File(party_file_obj);

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

                                                var (hasDataInFirstTenRowsAndColumns, rowCount) = CoreService.CheckDataInFirstTenRowsAndColumns(sheet);

                                                if (hasDataInFirstTenRowsAndColumns && rowCount > 1)
                                                {
                                                    sheet.ShiftRows(1, rowCount - 1, 0);
                                                    string outputFilePath = Path.Combine(filePath, strFile);
                                                    using (FileStream outputFile = new FileStream(outputFilePath, FileMode.Create))
                                                    {
                                                        workbook.Write(outputFile);
                                                    }
                                                }

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
                                            }
                                        }
                                    }
                                }
                                else if (fileExt == ".xlsx")
                                {
                                    if (sheet_list != null && sheet_list.Count > 0)
                                    {
                                        foreach (var sheet_name in sheet_list)
                                        {
                                            using (var package = new ExcelPackage(new FileInfo(fileLocation)))
                                            {
                                                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet_name];

                                                var (hasDataInFirstTenRowsAndColumns, row_count) = CoreService.CheckDataInFirstTenRowsAndColumns(worksheet);
                                                if (hasDataInFirstTenRowsAndColumns && row_count > 1)
                                                {
                                                    worksheet.DeleteRow(1, row_count - 1);
                                                    string outputFilePath = Path.Combine(filePath, strFile);
                                                    package.SaveAs(new FileInfo(outputFilePath));
                                                }

                                                var rowCount = worksheet.Dimension.End.Row;
                                                var colCount = worksheet.Dimension.End.Column;

                                                int rowsToCheck = Math.Max(rowCount - (rowCount - 1), 1);

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
                                                    }
                                                }
                                                try
                                                {
                                                    string outputFilePath1 = Path.Combine(filePath, strFile);
                                                    package.SaveAs(new FileInfo(outputFilePath1));
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                                List<string> columnNames = new List<string>();
                                                List<Dictionary<string, object>> rowsData = new List<Dictionary<string, object>>();

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
                                                            columnNames.Add(cellValue.ToString());
                                                        }
                                                    }

                                                    // Get row values
                                                    int totalRows = worksheet.Dimension.End.Row;

                                                    for (int row = headerRow + 1; row <= totalRows; row++)
                                                    {
                                                        Dictionary<string, object> rowData = new Dictionary<string, object>();

                                                        for (int col = 1; col <= totalColumns; col++)
                                                        {
                                                            string columnName = columnNames[col - 1];
                                                            var cell = worksheet.Cells[row, col];
                                                            var cellValue = cell.Value;

                                                            if (cell.Hyperlink != null && cell.Hyperlink.AbsoluteUri != null)
                                                            {
                                                                string linkUrl = cell.Hyperlink.AbsoluteUri;
                                                                rowData.Add(columnName, linkUrl);
                                                            }
                                                            else
                                                            {
                                                                rowData.Add(columnName, cellValue);
                                                            }
                                                        }

                                                        rowsData.Add(rowData);
                                                    }

                                                    //Save File
                                                    int colIndex = 1;
                                                    foreach (var columnName in columnNames)
                                                    {
                                                        worksheet.Cells[1, colIndex].Value = columnName;
                                                        colIndex++;
                                                    }

                                                    // Write row data
                                                    int rowIndex = 2;
                                                    foreach (var row in rowsData)
                                                    {
                                                        colIndex = 1;
                                                        foreach (var cellValue in row.Values)
                                                        {
                                                            worksheet.Cells[rowIndex, colIndex].Value = cellValue;
                                                            colIndex++;
                                                        }
                                                        rowIndex++;
                                                    }
                                                    try
                                                    {
                                                        string outputFilePath2 = Path.Combine(filePath, strFile);
                                                        package.SaveAs(new FileInfo(outputFilePath2));
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (fileExt == ".xls")
                                {
                                    foreach (DataRow item in supplier_column_Mapping.Rows)
                                    {
                                        string originalValue = Convert.ToString(item["Supp_Col_Name"]);
                                        if (originalValue.Contains("."))
                                        {
                                            string modifiedValue = originalValue.Replace(".", "#");
                                            item["Supp_Col_Name"] = modifiedValue;
                                        }
                                    }
                                    string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".xls", connString, party_File.Sheet_Name);
                                }
                                else if (fileExt == ".xlsx")
                                {
                                    foreach (DataRow item in supplier_column_Mapping.Rows)
                                    {
                                        string originalValue = Convert.ToString(item["Supp_Col_Name"]);
                                        if (originalValue.Contains("."))
                                        {
                                            string modifiedValue = originalValue.Replace(".", "#");
                                            item["Supp_Col_Name"] = modifiedValue;
                                        }
                                    }
                                    string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".xlsx", connString, party_File.Sheet_Name);
                                }
                                else if (fileExt == ".csv")
                                {
                                    excel_dataTable = CoreService.Convert_File_To_DataTable(".csv", fileLocation, "");
                                }
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

                                    foreach (DataRow row in excel_dataTable.Rows)
                                    {
                                        DataRow Final_row = dt_stock_data.NewRow();
                                        foreach (DataRow SuppCol_row in supplier_column_Mapping.Rows)
                                        {
                                            Final_row["SUPPLIER_NO"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SUPPLIER_NO") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SUPPLIER_NO"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SUPPLIER_NO"] = (Convert.ToString(Final_row["SUPPLIER_NO"]) == "") ? null : Convert.ToString(Final_row["SUPPLIER_NO"]);

                                            Final_row["CERTIFICATE_NO"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CERTIFICATE_NO") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CERTIFICATE_NO"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CERTIFICATE_NO"] = (Convert.ToString(Final_row["CERTIFICATE_NO"]) == "") ? null : Convert.ToString(Final_row["CERTIFICATE_NO"]);

                                            Final_row["LAB"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LAB") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LAB"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LAB"] = (Convert.ToString(Final_row["LAB"]) == "") ? null : Convert.ToString(Final_row["LAB"]);

                                            Final_row["SHAPE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SHAPE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SHAPE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SHAPE"] = (Convert.ToString(Final_row["SHAPE"]) == "") ? null : Convert.ToString(Final_row["SHAPE"]);

                                            Final_row["CTS"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CTS") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CTS"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CTS"] = (Convert.ToString(Final_row["CTS"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["CTS"]));

                                            Final_row["BASE_DISC"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "BASE_DISC") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["BASE_DISC"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["BASE_DISC"] = (Convert.ToString(Final_row["BASE_DISC"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["BASE_DISC"]));

                                            Final_row["BASE_RATE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "BASE_RATE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["BASE_RATE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["BASE_RATE"] = (Convert.ToString(Final_row["BASE_RATE"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["BASE_RATE"]));

                                            Final_row["BASE_AMOUNT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "BASE_AMOUNT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["BASE_AMOUNT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["BASE_AMOUNT"] = (Convert.ToString(Final_row["BASE_AMOUNT"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["BASE_AMOUNT"]));

                                            Final_row["COLOR"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "COLOR") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["COLOR"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["COLOR"] = (Convert.ToString(Final_row["COLOR"]) == "") ? null : Convert.ToString(Final_row["COLOR"]);

                                            Final_row["CLARITY"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CLARITY") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CLARITY"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CLARITY"] = (Convert.ToString(Final_row["CLARITY"]) == "") ? null : Convert.ToString(Final_row["CLARITY"]);

                                            Final_row["CUT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CUT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CUT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CUT"] = (Convert.ToString(Final_row["CUT"]) == "") ? null : Convert.ToString(Final_row["CUT"]);

                                            Final_row["POLISH"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "POLISH") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["POLISH"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["POLISH"] = (Convert.ToString(Final_row["POLISH"]) == "") ? null : Convert.ToString(Final_row["POLISH"]);

                                            Final_row["SYMM"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SYMM") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SYMM"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SYMM"] = (Convert.ToString(Final_row["SYMM"]) == "") ? null : Convert.ToString(Final_row["SYMM"]);

                                            Final_row["FLS_COLOR"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "FLS_COLOR") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["FLS_COLOR"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["FLS_COLOR"] = (Convert.ToString(Final_row["FLS_COLOR"]) == "") ? null : Convert.ToString(Final_row["FLS_COLOR"]);

                                            Final_row["FLS_INTENSITY"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "FLS_INTENSITY") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["FLS_INTENSITY"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["FLS_INTENSITY"] = (Convert.ToString(Final_row["FLS_INTENSITY"]) == "") ? null : Convert.ToString(Final_row["FLS_INTENSITY"]);

                                            Final_row["LENGTH"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LENGTH") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LENGTH"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LENGTH"] = (Convert.ToString(Final_row["LENGTH"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["LENGTH"]));

                                            Final_row["WIDTH"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "WIDTH") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["WIDTH"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["WIDTH"] = (Convert.ToString(Final_row["WIDTH"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["WIDTH"]));

                                            Final_row["DEPTH"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "DEPTH") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["DEPTH"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["DEPTH"] = (Convert.ToString(Final_row["DEPTH"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["DEPTH"]));

                                            Final_row["MEASUREMENT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "MEASUREMENT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["MEASUREMENT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["MEASUREMENT"] = (Convert.ToString(Final_row["MEASUREMENT"]) == "") ? null : Convert.ToString(Final_row["MEASUREMENT"]);

                                            Final_row["DEPTH_PER"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "DEPTH_PER") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["DEPTH_PER"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["DEPTH_PER"] = (Convert.ToString(Final_row["DEPTH_PER"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["DEPTH_PER"]));

                                            Final_row["TABLE_PER"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "TABLE_PER") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["TABLE_PER"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["TABLE_PER"] = (Convert.ToString(Final_row["TABLE_PER"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["TABLE_PER"]));

                                            Final_row["CULET"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CULET") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CULET"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CULET"] = (Convert.ToString(Final_row["CULET"]) == "") ? null : Convert.ToString(Final_row["CULET"]);

                                            Final_row["SHADE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SHADE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SHADE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SHADE"] = (Convert.ToString(Final_row["SHADE"]) == "") ? null : Convert.ToString(Final_row["SHADE"]);

                                            Final_row["LUSTER"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LUSTER") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LUSTER"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LUSTER"] = (Convert.ToString(Final_row["LUSTER"]) == "") ? null : Convert.ToString(Final_row["LUSTER"]);

                                            Final_row["MILKY"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "MILKY") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["MILKY"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["MILKY"] = (Convert.ToString(Final_row["MILKY"]) == "") ? null : Convert.ToString(Final_row["MILKY"]);

                                            Final_row["BGM"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "BGM") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["BGM"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["BGM"] = (Convert.ToString(Final_row["BGM"]) == "") ? null : Convert.ToString(Final_row["BGM"]);

                                            Final_row["LOCATION"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LOCATION") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LOCATION"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LOCATION"] = (Convert.ToString(Final_row["LOCATION"]) == "") ? null : Convert.ToString(Final_row["LOCATION"]);

                                            Final_row["STATUS"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "STATUS") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["STATUS"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["STATUS"] = (Convert.ToString(Final_row["STATUS"]) == "") ? null : Convert.ToString(Final_row["STATUS"]);

                                            Final_row["TABLE_BLACK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "TABLE_BLACK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["TABLE_BLACK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["TABLE_BLACK"] = (Convert.ToString(Final_row["TABLE_BLACK"]) == "") ? null : Convert.ToString(Final_row["TABLE_BLACK"]);

                                            Final_row["SIDE_BLACK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SIDE_BLACK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SIDE_BLACK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SIDE_BLACK"] = (Convert.ToString(Final_row["SIDE_BLACK"]) == "") ? null : Convert.ToString(Final_row["SIDE_BLACK"]);

                                            Final_row["TABLE_WHITE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "TABLE_WHITE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["TABLE_WHITE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["TABLE_WHITE"] = (Convert.ToString(Final_row["TABLE_WHITE"]) == "") ? null : Convert.ToString(Final_row["TABLE_WHITE"]);

                                            Final_row["SIDE_WHITE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SIDE_WHITE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SIDE_WHITE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SIDE_WHITE"] = (Convert.ToString(Final_row["SIDE_WHITE"]) == "") ? null : Convert.ToString(Final_row["SIDE_WHITE"]);

                                            Final_row["TABLE_OPEN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "TABLE_OPEN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["TABLE_OPEN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["TABLE_OPEN"] = (Convert.ToString(Final_row["TABLE_OPEN"]) == "") ? null : Convert.ToString(Final_row["TABLE_OPEN"]);

                                            Final_row["CROWN_OPEN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CROWN_OPEN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CROWN_OPEN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CROWN_OPEN"] = (Convert.ToString(Final_row["CROWN_OPEN"]) == "") ? null : Convert.ToString(Final_row["CROWN_OPEN"]);

                                            Final_row["PAVILION_OPEN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "PAVILION_OPEN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["PAVILION_OPEN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["PAVILION_OPEN"] = (Convert.ToString(Final_row["PAVILION_OPEN"]) == "") ? null : Convert.ToString(Final_row["PAVILION_OPEN"]);

                                            Final_row["GIRDLE_OPEN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_OPEN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_OPEN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_OPEN"] = (Convert.ToString(Final_row["GIRDLE_OPEN"]) == "") ? null : Convert.ToString(Final_row["GIRDLE_OPEN"]);

                                            Final_row["GIRDLE_FROM"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_FROM") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_FROM"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_FROM"] = (Convert.ToString(Final_row["GIRDLE_FROM"]) == "") ? null : Convert.ToString(Final_row["GIRDLE_FROM"]);

                                            Final_row["GIRDLE_TO"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_TO") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_TO"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_TO"] = (Convert.ToString(Final_row["GIRDLE_TO"]) == "") ? null : Convert.ToString(Final_row["GIRDLE_TO"]);

                                            Final_row["GIRDLE_CONDITION"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_CONDITION") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_CONDITION"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_CONDITION"] = (Convert.ToString(Final_row["GIRDLE_CONDITION"]) == "") ? null : Convert.ToString(Final_row["GIRDLE_CONDITION"]);

                                            Final_row["GIRDLE_TYPE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_TYPE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_TYPE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_TYPE"] = (Convert.ToString(Final_row["GIRDLE_TYPE"]) == "") ? null : Convert.ToString(Final_row["GIRDLE_TYPE"]);

                                            Final_row["LASER_INSCRIPTION"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LASER_INSCRIPTION") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LASER_INSCRIPTION"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LASER_INSCRIPTION"] = (Convert.ToString(Final_row["LASER_INSCRIPTION"]) == "") ? null : Convert.ToString(Final_row["LASER_INSCRIPTION"]);

                                            Final_row["CERTIFICATE_DATE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CERTIFICATE_DATE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CERTIFICATE_DATE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CERTIFICATE_DATE"] = (Convert.ToString(Final_row["CERTIFICATE_DATE"]) == "") ? null : Convert.ToString(Final_row["CERTIFICATE_DATE"]);

                                            Final_row["CROWN_ANGLE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CROWN_ANGLE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CROWN_ANGLE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CROWN_ANGLE"] = (Convert.ToString(Final_row["CROWN_ANGLE"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["CROWN_ANGLE"]));

                                            Final_row["CROWN_HEIGHT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CROWN_HEIGHT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CROWN_HEIGHT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CROWN_HEIGHT"] = (Convert.ToString(Final_row["CROWN_HEIGHT"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["CROWN_HEIGHT"]));

                                            Final_row["PAVILION_ANGLE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "PAVILION_ANGLE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["PAVILION_ANGLE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["PAVILION_ANGLE"] = (Convert.ToString(Final_row["PAVILION_ANGLE"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["PAVILION_ANGLE"]));

                                            Final_row["PAVILION_HEIGHT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "PAVILION_HEIGHT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["PAVILION_HEIGHT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["PAVILION_HEIGHT"] = (Convert.ToString(Final_row["PAVILION_HEIGHT"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["PAVILION_HEIGHT"]));

                                            Final_row["GIRDLE_PER"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GIRDLE_PER") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GIRDLE_PER"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GIRDLE_PER"] = (Convert.ToString(Final_row["GIRDLE_PER"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["GIRDLE_PER"]));

                                            Final_row["LR_HALF"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LR_HALF") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LR_HALF"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LR_HALF"] = (Convert.ToString(Final_row["LR_HALF"]) == "") ? null : Convert.ToString(Final_row["LR_HALF"]);

                                            Final_row["STAR_LN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "STAR_LN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["STAR_LN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["STAR_LN"] = (Convert.ToString(Final_row["STAR_LN"]) == "") ? null : Convert.ToString(Final_row["STAR_LN"]);

                                            Final_row["CERT_TYPE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CERT_TYPE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CERT_TYPE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CERT_TYPE"] = (Convert.ToString(Final_row["CERT_TYPE"]) == "") ? null : Convert.ToString(Final_row["CERT_TYPE"]);

                                            Final_row["FANCY_COLOR"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "FANCY_COLOR") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["FANCY_COLOR"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["FANCY_COLOR"] = (Convert.ToString(Final_row["FANCY_COLOR"]) == "") ? null : Convert.ToString(Final_row["FANCY_COLOR"]);

                                            Final_row["FANCY_INTENSITY"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "FANCY_INTENSITY") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["FANCY_INTENSITY"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["FANCY_INTENSITY"] = (Convert.ToString(Final_row["FANCY_INTENSITY"]) == "") ? null : Convert.ToString(Final_row["FANCY_INTENSITY"]);

                                            Final_row["FANCY_OVERTONE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "FANCY_OVERTONE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["FANCY_OVERTONE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["FANCY_OVERTONE"] = (Convert.ToString(Final_row["FANCY_OVERTONE"]) == "") ? null : Convert.ToString(Final_row["FANCY_OVERTONE"]);

                                            Final_row["IMAGE_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "IMAGE_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["IMAGE_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["IMAGE_LINK"] = (Convert.ToString(Final_row["IMAGE_LINK"]) == "") ? null : Convert.ToString(Final_row["IMAGE_LINK"]);

                                            Final_row["Image2"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "Image2") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["Image2"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["Image2"] = (Convert.ToString(Final_row["Image2"]) == "") ? null : Convert.ToString(Final_row["Image2"]);

                                            Final_row["VIDEO_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "VIDEO_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["VIDEO_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["VIDEO_LINK"] = (Convert.ToString(Final_row["VIDEO_LINK"]) == "") ? null : Convert.ToString(Final_row["VIDEO_LINK"]);

                                            Final_row["Video2"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "Video2") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["Video2"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["Video2"] = (Convert.ToString(Final_row["Video2"]) == "") ? null : Convert.ToString(Final_row["Video2"]);

                                            Final_row["CERTIFICATE_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CERTIFICATE_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CERTIFICATE_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CERTIFICATE_LINK"] = (Convert.ToString(Final_row["CERTIFICATE_LINK"]) == "") ? null : Convert.ToString(Final_row["CERTIFICATE_LINK"]);

                                            Final_row["DNA"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "DNA") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["DNA"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["DNA"] = (Convert.ToString(Final_row["DNA"]) == "") ? null : Convert.ToString(Final_row["DNA"]);

                                            Final_row["IMAGE_HEART_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "IMAGE_HEART_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["IMAGE_HEART_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["IMAGE_HEART_LINK"] = (Convert.ToString(Final_row["IMAGE_HEART_LINK"]) == "") ? null : Convert.ToString(Final_row["IMAGE_HEART_LINK"]);

                                            Final_row["IMAGE_ARROW_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "IMAGE_ARROW_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["IMAGE_ARROW_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["IMAGE_ARROW_LINK"] = (Convert.ToString(Final_row["IMAGE_ARROW_LINK"]) == "") ? null : Convert.ToString(Final_row["IMAGE_ARROW_LINK"]);

                                            Final_row["H_A_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "H_A_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["H_A_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["H_A_LINK"] = (Convert.ToString(Final_row["H_A_LINK"]) == "") ? null : Convert.ToString(Final_row["H_A_LINK"]);

                                            Final_row["CERTIFICATE_TYPE_LINK"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "CERTIFICATE_TYPE_LINK") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["CERTIFICATE_TYPE_LINK"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["CERTIFICATE_TYPE_LINK"] = (Convert.ToString(Final_row["CERTIFICATE_TYPE_LINK"]) == "") ? null : Convert.ToString(Final_row["CERTIFICATE_TYPE_LINK"]);

                                            Final_row["KEY_TO_SYMBOL"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "KEY_TO_SYMBOL") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["KEY_TO_SYMBOL"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["KEY_TO_SYMBOL"] = (Convert.ToString(Final_row["KEY_TO_SYMBOL"]) == "") ? null : Convert.ToString(Final_row["KEY_TO_SYMBOL"]);

                                            Final_row["LAB_COMMENTS"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "LAB_COMMENTS") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["LAB_COMMENTS"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["LAB_COMMENTS"] = (Convert.ToString(Final_row["LAB_COMMENTS"]) == "") ? null : Convert.ToString(Final_row["LAB_COMMENTS"]);

                                            Final_row["SUPPLIER_COMMENTS"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SUPPLIER_COMMENTS") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SUPPLIER_COMMENTS"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SUPPLIER_COMMENTS"] = (Convert.ToString(Final_row["SUPPLIER_COMMENTS"]) == "") ? null : Convert.ToString(Final_row["SUPPLIER_COMMENTS"]);

                                            Final_row["ORIGIN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "ORIGIN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["ORIGIN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["ORIGIN"] = (Convert.ToString(Final_row["ORIGIN"]) == "") ? null : Convert.ToString(Final_row["ORIGIN"]);

                                            Final_row["BOW_TIE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "BOW_TIE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["BOW_TIE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["BOW_TIE"] = (Convert.ToString(Final_row["BOW_TIE"]) == "") ? null : Convert.ToString(Final_row["BOW_TIE"]);

                                            Final_row["EXTRA_FACET_TABLE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "EXTRA_FACET_TABLE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["EXTRA_FACET_TABLE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["EXTRA_FACET_TABLE"] = (Convert.ToString(Final_row["EXTRA_FACET_TABLE"]) == "") ? null : Convert.ToString(Final_row["EXTRA_FACET_TABLE"]);

                                            Final_row["EXTRA_FACET_CROWN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "EXTRA_FACET_CROWN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["EXTRA_FACET_CROWN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["EXTRA_FACET_CROWN"] = (Convert.ToString(Final_row["EXTRA_FACET_CROWN"]) == "") ? null : Convert.ToString(Final_row["EXTRA_FACET_CROWN"]);

                                            Final_row["EXTRA_FACET_PAVILION"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "EXTRA_FACET_PAVILION") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["EXTRA_FACET_PAVILION"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["EXTRA_FACET_PAVILION"] = (Convert.ToString(Final_row["EXTRA_FACET_PAVILION"]) == "") ? null : Convert.ToString(Final_row["EXTRA_FACET_PAVILION"]);

                                            Final_row["INTERNAL_GRAINING"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "INTERNAL_GRAINING") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["INTERNAL_GRAINING"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["INTERNAL_GRAINING"] = (Convert.ToString(Final_row["INTERNAL_GRAINING"]) == "") ? null : Convert.ToString(Final_row["INTERNAL_GRAINING"]);

                                            Final_row["H_A"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "H_A") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["H_A"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["H_A"] = (Convert.ToString(Final_row["H_A"]) == "") ? null : Convert.ToString(Final_row["H_A"]);

                                            Final_row["SUPPLIER_DISC"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SUPPLIER_DISC") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SUPPLIER_DISC"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SUPPLIER_DISC"] = (Convert.ToString(Final_row["SUPPLIER_DISC"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["SUPPLIER_DISC"]));

                                            Final_row["SUPPLIER_AMOUNT"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "SUPPLIER_AMOUNT") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["SUPPLIER_AMOUNT"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["SUPPLIER_AMOUNT"] = (Convert.ToString(Final_row["SUPPLIER_AMOUNT"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["SUPPLIER_AMOUNT"]));

                                            Final_row["OFFER_DISC"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "OFFER_DISC") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["OFFER_DISC"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["OFFER_DISC"] = (Convert.ToString(Final_row["OFFER_DISC"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["OFFER_DISC"]));

                                            Final_row["OFFER_VALUE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "OFFER_VALUE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["OFFER_VALUE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["OFFER_VALUE"] = (Convert.ToString(Final_row["OFFER_VALUE"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["OFFER_VALUE"]));

                                            Final_row["MAX_SLAB_BASE_DISC"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "MAX_SLAB_BASE_DISC") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["MAX_SLAB_BASE_DISC"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["MAX_SLAB_BASE_DISC"] = (Convert.ToString(Final_row["MAX_SLAB_BASE_DISC"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["MAX_SLAB_BASE_DISC"]));

                                            Final_row["MAX_SLAB_BASE_VALUE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "MAX_SLAB_BASE_VALUE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["MAX_SLAB_BASE_VALUE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["MAX_SLAB_BASE_VALUE"] = (Convert.ToString(Final_row["MAX_SLAB_BASE_VALUE"]) == "") ? null : CoreService.RemoveNonNumericAndDotAndNegativeCharacters(Convert.ToString(Final_row["MAX_SLAB_BASE_VALUE"]));

                                            Final_row["EYE_CLEAN"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "EYE_CLEAN") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["EYE_CLEAN"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["EYE_CLEAN"] = (Convert.ToString(Final_row["EYE_CLEAN"]) == "") ? null : Convert.ToString(Final_row["EYE_CLEAN"]);

                                            Final_row["GOOD_TYPE"] = ((Convert.ToString(SuppCol_row["Display_Name"]) != "GOOD_TYPE") || (Convert.ToString(SuppCol_row["Supp_Col_Name"]) == "")) ? Convert.ToString(Final_row["GOOD_TYPE"]) : row[Convert.ToString(SuppCol_row["Supp_Col_Name"])];
                                            Final_row["GOOD_TYPE"] = (Convert.ToString(Final_row["GOOD_TYPE"]) == "") ? null : Convert.ToString(Final_row["GOOD_TYPE"]);
                                        }
                                        dt_stock_data.Rows.Add(Final_row);
                                    }

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
                                                await _supplierService.Supplier_Stock_Insert_Update((int)stock_Data_Master_Schedular.Supplier_Id, stock_Data_Id);
                                            }
                                        }
                                        return Ok(new
                                        {
                                            statusCode = HttpStatusCode.OK,
                                            message = "File uploaded successfully."
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
                        message = "No column mapping found!"
                    });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Manual_Upload", ex.StackTrace);
                return Ok(new
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
        public async Task<IActionResult> Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Lab_Entry)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_Error_Log(supplier_Ids, upload_Type, from_Date, from_Time, to_Date, to_Time, is_Lab_Entry);
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
        public async Task<IActionResult> Supplier_Stock_File_Error_Error_Log(int supplier_Id)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Stock_File_Error_Log(supplier_Id);
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
                    worksheet.Cells["A1"].LoadFromDataTable(result, true);

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
        public async Task<IActionResult> Get_Report_Name(int id)
        {
            try
            {
                var result = await _supplierService.Get_Report_Name(id);
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
                var result = await _supplierService.Get_Report_Detail_Filter_Parameter(id);
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
                                dataTable.Rows.Add(item1.Rd_Id, item, item1.Display_Type,item1.Order_By, item1.Short_No,item1.Width,item1.Column_Format, item1.Alignment, item1.Fore_Colour, item1.Back_Colour, item1.IsBold);
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
                var result = await _supplierService.Get_Report_Search(report_Filter.id, report_Filter.Report_Filter_Parameter);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Report_Search", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_saved_report_serach")]
        [Authorize]
        public async Task<IActionResult> Get_Saved_Report_Serach()
        {
            try
            {
                var result = await _supplierService.Get_Report_Search();
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
                    var result = await _supplierService.Create_Update_Report_Search(report_Search_Save);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SearchedReportCreated
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

        [HttpPut]
        [Route("update_report_search_save")]
        [Authorize]
        public async Task<IActionResult> Update_Report_Search_Save(Report_Search_Save report_Search_Save)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _supplierService.Create_Update_Report_Search(report_Search_Save);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.SearchedReportUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Report_Search_Save", ex.StackTrace);
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
        public async Task<IActionResult> Get_Report_Layout_Save(int User_Id)
        {
            try
            {
                var result = await _supplierService.Get_Report_Layout_Save(User_Id);
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
                return Ok(new
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
                    var result = await _supplierService.Create_Update_Report_Layout_Save(report_Layout_Save);
                    if (result == "success")
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = report_Layout_Save.Id> 0 ? CoreCommonMessage.LayoutReportUpdated : CoreCommonMessage.LayoutReportCreated
                        });
                    }
                    else if (result == "exist")
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
                return Ok(new
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
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Cart/Approval Management        
        [HttpPost]
        [Route("create_cart_review_approval_management")]
        [Authorize]
        public async Task<IActionResult> Create_Cart_Review_Approval_Management(Cart_Review_Approval_Management cart_Review_Approval_Management)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _cartService.Insert_Cart_Review_Aproval_Management(cart_Review_Approval_Management);
                    if (result > 0)
                    {
                        string msg = string.Empty;
                        if (cart_Review_Approval_Management.Upload_Type == "C")
                        {
                            msg = CoreCommonMessage.CartAdded;
                        }
                        else if (cart_Review_Approval_Management.Upload_Type == "R")
                        {
                            msg = CoreCommonMessage.ReviewAdded;
                        }
                        else if (cart_Review_Approval_Management.Upload_Type == "A")
                        {
                            msg = CoreCommonMessage.StockApproved;
                        }
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
                await _commonService.InsertErrorLog(ex.Message, "Create_Cart_Review_Approval_Management", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_cart_review_approval_management")]
        [Authorize]
        public async Task<IActionResult> Get_Cart_Review_Approval_Management(string upload_Type, string userIds)
        {
            try
            {
                var result = await _cartService.Get_Cart_Review_Approval_Management(upload_Type, userIds);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Cart_Review_Approval_Management", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("delete_cart_review_approval_management")]
        [Authorize]
        public async Task<IActionResult> Delete_Cart_Review_Approval_Management(string ids, int user_Id, string upload_Type)
        {
            try
            {
                var result = await _cartService.Delete_Cart_Review_Aproval_Management(ids, user_Id, upload_Type);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = upload_Type == "C" ? CoreCommonMessage.CartStockDeleted : CoreCommonMessage.ApprovalStockDeleted,
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Cart_Review_Approval_Management", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("approved_or_rejected_by_management")]
        [Authorize]
        public async Task<IActionResult> Approved_Or_Rejected_by_Management(string ids, bool? is_Approved, bool? is_Rejected, int user_Id)
        {
            try
            {
                var result = await _cartService.Approved_Or_Rejected_by_Management(ids, is_Approved ?? false, is_Rejected ?? false, user_Id);
                if(result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = is_Approved == true ? CoreCommonMessage.StokeApprovedByManagement : CoreCommonMessage.StokeRejectedByManagement
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Approved_Or_Rejected_by_Management", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
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
                    if (lst_Cert_No != null && lst_Cert_No.Count > 0)
                    {
                        foreach (var item in lst_Cert_No)
                        {
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
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.DataSuccessfullyFound,
                            data = result
                        });
                    }
                    return NoContent();
                }
                return BadRequest();
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
                if(gIA_Lab_Parameters != null && gIA_Lab_Parameters.Count > 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable = Set_GIA_Lab_Parameter_Column_In_Datatable(new DataTable(), gIA_Lab_Parameters);
                    var result = await _supplierService.Insert_GIA_Lab_Parameter(dataTable);
                    if(result > 0)
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
    }
}
