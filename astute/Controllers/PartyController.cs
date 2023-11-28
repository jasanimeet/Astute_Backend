using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        #endregion

        #region Ctor
        public PartyController(IPartyService partyService,
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            ISupplierService supplierService)
        {
            _partyService = partyService;
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _supplierService = supplierService;
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
            dt_stock_data.Columns.Add("Short_Code", typeof(string));
            if (stock_Datas != null && stock_Datas.Count > 0)
            {
                foreach (var item in stock_Datas)
                {
                    dt_stock_data.Rows.Add(item.SUPPLIER_NO ?? null, item.CERTIFICATE_NO ?? null, item.LAB ?? null, item.SHAPE ?? null, item.CTS ?? null, item.BASE_DISC ?? null, item.BASE_RATE ?? null, item.BASE_AMOUNT ?? null, item.COLOR ?? null, item.CLARITY ?? null, item.CUT ?? null,
                        item.POLISH ?? null, item.SYMM ?? null, item.FLS_COLOR ?? null, item.FLS_INTENSITY ?? null, item.LENGTH ?? null, item.WIDTH ?? null, item.DEPTH ?? null, item.MEASUREMENT ?? null, item.DEPTH_PER ?? null, item.TABLE_PER ?? null, item.CULET ?? null, item.SHADE ?? null, item.LUSTER ?? null,
                        item.MILKY ?? null, item.BGM ?? null, item.LOCATION ?? null, item.STATUS ?? null, item.TABLE_BLACK ?? null, item.SIDE_BLACK ?? null, item.TABLE_WHITE ?? null, item.SIDE_WHITE ?? null, item.TABLE_OPEN ?? null, item.CROWN_OPEN ?? null, item.PAVILION_OPEN ?? null, item.GIRDLE_OPEN ?? null,
                        item.GIRDLE_FROM ?? null, item.GIRDLE_TO ?? null, item.GIRDLE_CONDITION ?? null, item.GIRDLE_TYPE ?? null, item.LASER_INSCRIPTION ?? null, item.CERTIFICATE_DATE ?? null, item.CROWN_ANGLE ?? null, item.CROWN_HEIGHT ?? null, item.PAVILION_ANGLE ?? null, item.PAVILION_HEIGHT ?? null,
                        item.GIRDLE_PER ?? null, item.LR_HALF ?? null, item.STAR_LN ?? null, item.CERT_TYPE ?? null, item.FANCY_COLOR ?? null, item.FANCY_INTENSITY ?? null, item.FANCY_OVERTONE ?? null, item.IMAGE_LINK ?? null, item.Image2 ?? null, item.VIDEO_LINK ?? null, item.Video2 ?? null, item.CERTIFICATE_LINK ?? null,
                        item.DNA ?? null, item.IMAGE_HEART_LINK ?? null, item.IMAGE_ARROW_LINK ?? null, item.H_A_LINK ?? null, item.CERTIFICATE_TYPE_LINK ?? null, item.KEY_TO_SYMBOL ?? null, item.LAB_COMMENTS ?? null, item.SUPPLIER_COMMENTS ?? null, item.ORIGIN ?? null, item.BOW_TIE ?? null,
                        item.EXTRA_FACET_TABLE ?? null, item.EXTRA_FACET_CROWN ?? null, item.EXTRA_FACET_PAVILION ?? null, item.INTERNAL_GRAINING ?? null, item.H_A ?? null, item.SUPPLIER_DISC ?? null, item.SUPPLIER_AMOUNT ?? null, item.OFFER_DISC ?? null, item.OFFER_VALUE ?? null,
                        item.MAX_SLAB_BASE_DISC ?? null, item.MAX_SLAB_BASE_VALUE ?? null, item.EYE_CLEAN ?? null, item.Short_Code ?? null);
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
        private DataTable Set_Supplier_Pricing_Column_In_Datatable(DataTable dt_Supplier_Pricing, IList<Supplier_Pricing> supplier_Pricings)
        {
            dt_Supplier_Pricing.Columns.Add("Supplier_Pricing_Id", typeof(int));
            dt_Supplier_Pricing.Columns.Add("Supplier_Id", typeof(int));
            dt_Supplier_Pricing.Columns.Add("Map_Flag", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Shape", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Cts", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Color", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Fancy_Color", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Clarity", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Cut", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Polish", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Symm", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Fls_Intensity", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Lab", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Shade", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Luster", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Bgm", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Culet", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Location", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Status", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Good_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Length_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Length_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Width_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Width_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Depth_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Depth_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Depth_Per_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Depth_Per_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Table_Per_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Table_Per_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Crown_Angle_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Crown_Angle_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Crown_Height_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Crown_Height_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Pavilion_Angle_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Pavilion_Angle_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Pavilion_Height_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Pavilion_Height_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Girdle_Per_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Girdle_Per_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Table_Black", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Side_Black", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Table_White", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Side_white", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Comment", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Cert_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Table_Open", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Crown_Open", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Pavilion_Open", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Girdle_Open", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Base_Disc_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Base_Disc_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Base_Amount_From", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Base_Amount_To", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Supplier_Filter_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Calculation_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Sign", typeof(string));
            dt_Supplier_Pricing.Columns.Add("Value_1", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Value_2", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Value_3", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Value_4", typeof(float));
            dt_Supplier_Pricing.Columns.Add("SP_Calculation_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_Sign", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_Start_Date", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_Start_Time", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_End_Date", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_End_Time", typeof(string));
            dt_Supplier_Pricing.Columns.Add("SP_Value_1", typeof(float));
            dt_Supplier_Pricing.Columns.Add("SP_Value_2", typeof(float));
            dt_Supplier_Pricing.Columns.Add("SP_Value_3", typeof(float));
            dt_Supplier_Pricing.Columns.Add("SP_Value_4", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_Calculation_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_Sign", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_Value_1", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_Value_2", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_Value_3", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_Value_4", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Calculation_Type", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Sign", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Start_Date", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Start_Time", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_End_Date", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_End_Time", typeof(string));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Value_1", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Value_2", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Value_3", typeof(float));
            dt_Supplier_Pricing.Columns.Add("MS_SP_Value_4", typeof(float));
            dt_Supplier_Pricing.Columns.Add("Query_Flag", typeof(string));

            if (supplier_Pricings != null && supplier_Pricings.Count > 0)
            {
                foreach (var item in supplier_Pricings)
                {
                    dt_Supplier_Pricing.Rows.Add(item.Supplier_Pricing_Id, item.Supplier_Id, item.Map_Flag, item.Shape, item.Cts, item.Color, item.Fancy_Color, item.Clarity, item.Cut,
                        item.Polish, item.Symm, item.Fls_Intensity, item.Lab, item.Shade, item.Luster, item.Bgm, item.Culet, item.Location, item.Status, item.Good_Type, item.Length_From,
                        item.Length_To, item.Width_From, item.Width_To, item.Depth_From, item.Depth_To, item.Depth_Per_From, item.Depth_Per_To, item.Table_Per_From, item.Table_Per_To,
                        item.Crown_Angle_From, item.Crown_Angle_To, item.Crown_Height_From, item.Crown_Height_To, item.Pavilion_Angle_From, item.Pavilion_Angle_To, item.Pavilion_Height_From,
                        item.Pavilion_Height_To, item.Girdle_Per_From, item.Girdle_Per_To, item.Table_Black, item.Side_Black, item.Table_White, item.Side_white, item.Comment, item.Cert_Type,
                        item.Table_Open, item.Crown_Open, item.Pavilion_Open, item.Girdle_Open, item.Base_Disc_From, item.Base_Disc_To, item.Base_Amount_From, item.Base_Amount_To, item.Supplier_Filter_Type,
                        item.Calculation_Type, item.Sign, item.Value_1, item.Value_2, item.Value_3, item.Value_4, item.SP_Calculation_Type, item.SP_Sign, item.SP_Start_Date, item.SP_Start_Time,
                        item.SP_End_Date, item.SP_End_Time, item.SP_Value_1, item.SP_Value_2, item.SP_Value_3, item.SP_Value_4, item.MS_Calculation_Type, item.MS_Sign, item.MS_Value_1, item.MS_Value_2,
                        item.MS_Value_3, item.MS_Value_4, item.MS_SP_Calculation_Type, item.MS_SP_Sign, item.MS_SP_Start_Date, item.MS_SP_Start_Time, item.MS_SP_End_Date, item.MS_SP_End_Time,
                        item.MS_SP_Value_1, item.MS_SP_Value_2, item.MS_SP_Value_3, item.MS_SP_Value_4, item.Query_Flag);
                }
            }
            return dt_Supplier_Pricing;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Contact_Name", typeof(string));
                                dataTable1.Columns.Add("Sex", typeof(string));
                                dataTable1.Columns.Add("Designation_Id", typeof(int));
                                dataTable1.Columns.Add("Mobile_No", typeof(string));
                                dataTable1.Columns.Add("Email", typeof(string));
                                dataTable1.Columns.Add("Birth_Date", typeof(string));
                            }

                            foreach (var item in party_Master.Party_Contact_List)
                            {
                                dataTable.Rows.Add(item.Contact_Id, party_Id, item.Prefix, item.First_Name, item.Last_Name, item.Designation_Id, item.Phone_No, item.Phone_No_Country_Code, item.Mobile_No, item.Mobile_No_Country_Code, item.Email, item.QueryFlag);
                                //if (CoreService.Enable_Trace_Records(_configuration))
                                //{
                                //    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Contact_Name, item.Sex, item.Designation_Id, item.Mobile_No, item.Email, item.Birth_Date, false);
                                //}
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Contact_Trace(dataTable1);
                            }
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
                            dataTable.Columns.Add("Per_1", typeof(decimal));
                            dataTable.Columns.Add("Assist_2", typeof(int));
                            dataTable.Columns.Add("Per_2", typeof(decimal));
                            dataTable.Columns.Add("Viewing_Rights_To", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));
                            dataTable.Columns.Add("Date", typeof(string));

                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Diamond_Type", typeof(int));
                                dataTable1.Columns.Add("Assist_1", typeof(int));
                                dataTable1.Columns.Add("Per_1", typeof(decimal));
                                dataTable1.Columns.Add("Assist_2", typeof(int));
                                dataTable1.Columns.Add("Per_2", typeof(decimal));
                                dataTable1.Columns.Add("Assist_3", typeof(int));
                            }
                            foreach (var item in party_Master.Party_Assist_List)
                            {
                                dataTable.Rows.Add(item.Assist_Id, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Viewing_Rights_To, item.QueryFlag, item.Date);

                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Diamond_Type, item.Assist_1, item.Per_1, item.Assist_2, item.Per_2, item.Viewing_Rights_To);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Assist_Trace(dataTable1);
                            }
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
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Bank_Id", typeof(int));
                                dataTable1.Columns.Add("Account_No", typeof(int));
                                dataTable1.Columns.Add("Status", typeof(bool));
                                dataTable1.Columns.Add("Account_Type", typeof(int));
                            }

                            foreach (var item in party_Master.Party_Bank_List)
                            {
                                dataTable.Rows.Add(item.Account_Id, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type, item.Default_Bank, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Bank_Id, item.Account_No, item.Status, item.Account_Type);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Bank_Trace(dataTable1);
                            }
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
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Document_Type", typeof(int));
                                dataTable1.Columns.Add("Document_No", typeof(string));
                                dataTable1.Columns.Add("Upload_Path", typeof(int));
                                dataTable1.Columns.Add("Valid_From", typeof(DateTime));
                                dataTable1.Columns.Add("Valid_To", typeof(DateTime));
                                dataTable1.Columns.Add("Kyc_Grade", typeof(int));
                            }

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

                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Document_Type, item.Document_No, item.Upload_Path, item.Valid_From, item.Valid_To, item.Kyc_Grade);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Document_Trace(dataTable1);
                            }
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
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Cat_val_Id, item.ID);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Media_Trace(dataTable1);
                            }
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
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Company_Name", typeof(string));
                                dataTable1.Columns.Add("Address_1", typeof(string));
                                dataTable1.Columns.Add("Address_2", typeof(string));
                                dataTable1.Columns.Add("Address_3", typeof(string));
                                dataTable1.Columns.Add("City_Id", typeof(int));
                                dataTable1.Columns.Add("Mobile_No", typeof(string));
                                dataTable1.Columns.Add("Phone_No", typeof(string));
                                dataTable1.Columns.Add("Contact_Person", typeof(string));
                                dataTable1.Columns.Add("Contact_Email", typeof(string));
                                dataTable1.Columns.Add("Default_Address", typeof(bool));
                                dataTable1.Columns.Add("Status", typeof(bool));
                            }

                            foreach (var item in party_Master.Party_Shipping_List)
                            {
                                dataTable.Rows.Add(item.Ship_Id, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Mobile_No_Country_Code, item.Phone_No, item.Phone_No_Country_Code, item.Contact_Person, item.TIN_No, item.Default_Address, item.Status, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Company_Name, item.Address_1, item.Address_2, item.Address_3, item.City_Id, item.Mobile_No, item.Phone_No, item.Contact_Person, item.TIN_No, item.Default_Address, item.Status);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Shipping_Trace(dataTable1);
                            }
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
                            DataTable dataTable1 = new DataTable();
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Columns.Add("Employee_Id", typeof(int));
                                dataTable1.Columns.Add("IP_Address", typeof(string));
                                dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable1.Columns.Add("Record_Type", typeof(string));
                                dataTable1.Columns.Add("Party_Id", typeof(int));
                                dataTable1.Columns.Add("Start_Date", typeof(string));
                                dataTable1.Columns.Add("Process_Type", typeof(string));
                                dataTable1.Columns.Add("Default_Printing_Type", typeof(int));
                                dataTable1.Columns.Add("Default_Currency", typeof(int));
                                dataTable1.Columns.Add("Default_Bank", typeof(int));
                                dataTable1.Columns.Add("Default_Payment_Terms", typeof(int));
                                dataTable1.Columns.Add("Default_Remarks", typeof(int));
                            }
                            #endregion

                            foreach (var item in party_Master.Party_Print_Process_List)
                            {
                                dataTable.Rows.Add(item.Print_Process_Id, party_Id, item.Start_Date, item.Process_Type, item.Default_Printing_Type, item.Default_Currency, item.Default_Bank, item.Default_Payment_Terms, item.Default_Remarks, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, party_Id, item.Start_Date, item.Process_Type, item.Default_Printing_Type, item.Default_Currency, item.Default_Bank, item.Default_Payment_Terms, item.Default_Remarks);
                                }
                            }
                            await _partyService.Add_Update_Party_Print_Process(dataTable);
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _partyService.Insert_Party_Print_Trace(dataTable1);
                            }
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PartyMasterCreated
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
        [Route("get_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag)
        {
            try
            {
                var result = await _supplierService.Get_Supplier_Pricing(supplier_Pricing_Id, supplier_Id, supplier_Filter_Type, map_Flag);
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

        //[HttpPost]
        //[Route("create_supplier_pricing")]
        //[Authorize]
        //public async Task<IActionResult> Create_Supplier_Pricing(Supplier_Pricing supplier_Pricing)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var (message, supplier_Pricing_Id) = await _supplierService.Add_Update_Supplier_Pricing(supplier_Pricing);

        //            if (message == "success" && supplier_Pricing_Id > 0)
        //            {
        //                if (supplier_Pricing.Key_To_Symbol != null && supplier_Pricing.Key_To_Symbol.Count > 0)
        //                {
        //                    DataTable dataTable = new DataTable();
        //                    dataTable.Columns.Add("Supplier_Pricing_Id", typeof(int));
        //                    dataTable.Columns.Add("Cat_Val_Id", typeof(int));
        //                    dataTable.Columns.Add("Symbol_Status", typeof(bool));

        //                    foreach (var item in supplier_Pricing.Key_To_Symbol)
        //                    {
        //                        dataTable.Rows.Add(supplier_Pricing_Id, item.Cat_Val_Id, item.Symbol_Status);
        //                    }
        //                    await _supplierService.Add_Update_Supplier_Pricing_Key_To_Symbole(dataTable);
        //                }
        //                return Ok(new
        //                {
        //                    statusCode = HttpStatusCode.OK,
        //                    message = supplier_Pricing.Supplier_Pricing_Id == 0 ? CoreCommonMessage.SupplierPricingCreated : CoreCommonMessage.SupplierPricingUpdated
        //                });
        //            }
        //        }
        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _commonService.InsertErrorLog(ex.Message, "Create_Supplier_Pricing", ex.StackTrace);
        //        return Ok(new
        //        {
        //            message = ex.Message
        //        });
        //    }
        //}

        [HttpPost]
        [Route("create_supplier_pricing")]
        [Authorize]
        public async Task<IActionResult> Create_Supplier_Pricing(IList<Supplier_Pricing> supplier_Pricings)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (supplier_Pricings != null && supplier_Pricings.Count > 0)
                    {
                        bool success = false;
                        if (supplier_Pricings != null && supplier_Pricings.Count > 0)
                        {
                            foreach (var item in supplier_Pricings)
                            {
                                if (item.Query_Flag == "D")
                                {
                                    var result_Supplier_Pricing_Key_To_Symbol = await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(item.Supplier_Pricing_Id);
                                    var result = await _supplierService.Delete_Supplier_Pricing(item.Supplier_Pricing_Id);
                                }
                                else
                                {
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
                                            foreach (var obj in item.Key_To_Symbol)
                                            {
                                                dataTable.Rows.Add(supplier_pricing_Id, obj.Cat_Val_Id, obj.Symbol_Status);
                                            }
                                            await _supplierService.Add_Update_Supplier_Pricing_Key_To_Symbol(dataTable);
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
                                message = CoreCommonMessage.SupplierPricingCreated
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
        public async Task<IActionResult> Delete_Supplier_Pricing(int supplier_Pricing_Id)
        {
            try
            {
                var result_Supplier_Pricing_Key_To_Symbol = await _supplierService.Delete_Supplier_Pricing_Key_To_Symbol(supplier_Pricing_Id);
                var result = await _supplierService.Delete_Supplier_Pricing(supplier_Pricing_Id);
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
        #endregion

        #region Supplier Value Mapping
        [HttpGet]
        [Route("get_supplier_value_mapping")]
        [Authorize]
        public async Task<IActionResult> Get_Supplier_Value_Mapping(int sup_Id, int col_Id)
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
            catch
            {
                throw;
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
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Supplier_Value_Mapping", ex.StackTrace);
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

        #region Stock Generate
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
                    bool success = false;
                    if (stock_Number_Generations != null && stock_Number_Generations.Count > 0)
                    {
                        foreach (var item in stock_Number_Generations)
                        {
                            var result = await _supplierService.Add_Update_Stock_Number_Generation(item);
                            if (result > 0)
                            {
                                success = true;
                            }
                        }
                        if (success)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.StockNumberUpdated,
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
        public async Task<IActionResult> Get_Api_Ftp_File_Party_Select(int party_Id, bool lab, bool overseas)
        {
            try
            {
                var result = await _supplierService.Get_Api_Ftp_File_Party_Select(party_Id, lab, overseas);
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

        #region FTP Party

        [HttpGet]
        [Route("get_supplier_ftp_file")]
        //[Authorize]
        public async Task<IActionResult> Get_Supplier_Ftp_File(int supp_Id)
        {
            try
            {
                  var supplier = await _partyService.Get_Party_FTP(0, supp_Id);
                if (supplier != null)
                {   
                    string ftpfileName = "/stock-" + Guid.NewGuid().ToString() + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + ".csv";                    
                    string ftpUrl = _configuration["BaseUrl"] + CoreCommonFilePath.FtpFilesPath + ftpfileName + ".csv";
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
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });

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
        #endregion
    }
}
