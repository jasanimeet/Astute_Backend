using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial class SupplierService : ISupplierService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public SupplierService(AstuteDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IJWTAuthentication jWTAuthentication)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jWTAuthentication = jWTAuthentication;
        }
        #endregion

        #region Utilities
        private async Task Insert_Value_Config_Trace(Value_Config value_Config, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var length_From = value_Config.Length_From > 0 ? new SqlParameter("@Length_From", value_Config.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = value_Config.Length_To > 0 ? new SqlParameter("@Length_To", value_Config.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = value_Config.Width_From > 0 ? new SqlParameter("@Width_From", value_Config.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = value_Config.Width_To > 0 ? new SqlParameter("@Width_To", value_Config.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = value_Config.Depth_From > 0 ? new SqlParameter("@Depth_From", value_Config.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = value_Config.Depth_To > 0 ? new SqlParameter("@Depth_To", value_Config.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = value_Config.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", value_Config.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = value_Config.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", value_Config.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = value_Config.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", value_Config.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = value_Config.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", value_Config.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = value_Config.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", value_Config.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = value_Config.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", value_Config.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = value_Config.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", value_Config.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = value_Config.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", value_Config.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = value_Config.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", value_Config.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = value_Config.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", value_Config.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = value_Config.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", value_Config.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = value_Config.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", value_Config.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = value_Config.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", value_Config.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = value_Config.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", value_Config.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var lr_Half_From = value_Config.Lr_Half_From > 0 ? new SqlParameter("@Lr_Half_From", value_Config.Lr_Half_From) : new SqlParameter("@Lr_Half_From", DBNull.Value);
            var lr_Half_To = value_Config.Lr_Half_To > 0 ? new SqlParameter("@Lr_Half_To", value_Config.Lr_Half_To) : new SqlParameter("@Lr_Half_To", DBNull.Value);
            var star_Ln_From = value_Config.Star_Ln_From > 0 ? new SqlParameter("@Star_Ln_From", value_Config.Star_Ln_From) : new SqlParameter("@Star_Ln_From", DBNull.Value);
            var star_Ln_To = value_Config.Star_Ln_To > 0 ? new SqlParameter("@Star_Ln_To", value_Config.Star_Ln_To) : new SqlParameter("@Star_Ln_To", DBNull.Value);
            var shape_Group = !string.IsNullOrEmpty(value_Config.Shape_Group) ? new SqlParameter("@Shape_Group", value_Config.Shape_Group) : new SqlParameter("@Shape_Group", DBNull.Value);
            var shape = !string.IsNullOrEmpty(value_Config.Shape) ? new SqlParameter("@Shape", value_Config.Shape) : new SqlParameter("@Shape", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Value_Config_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From, @Depth_Per_To, @Table_Per_From,
                        @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Lr_Half_From, @Lr_Half_To, @Star_Ln_From, @Star_Ln_To, @Shape_Group, @Shape", empId, ipaddress, date, time, record_Type, length_From, length_To,
            width_From, width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
            pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, lr_Half_From, lr_Half_To, star_Ln_From, star_Ln_To, shape_Group, shape));
        }
        private async Task Insert_Supplier_Pricing_Trace(Supplier_Pricing supplier_Pricing, string recordType)
        {
            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
            var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
            var user_Id = _jWTAuthentication.Validate_Jwt_Token(token);

            var (empId, ipaddress, date, time, record_Type) = CoreService.Get_SqlParameter_Values(user_Id ?? 0, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, recordType);

            var supplier_Pricing_Id = new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing.Supplier_Pricing_Id);
            var supplier_Id = supplier_Pricing.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Pricing.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var sunrise_Pricing_Id = supplier_Pricing.Sunrise_Pricing_Id > 0 ? new SqlParameter("@Sunrise_Pricing_Id", supplier_Pricing.Sunrise_Pricing_Id) : new SqlParameter("@Sunrise_Pricing_Id", DBNull.Value);
            var customer_Pricing_Id = supplier_Pricing.Customer_Pricing_Id > 0 ? new SqlParameter("@Customer_Pricing_Id", supplier_Pricing.Customer_Pricing_Id) : new SqlParameter("@Customer_Pricing_Id", DBNull.Value);
            var user_Pricing_Id = !string.IsNullOrEmpty(supplier_Pricing.User_Pricing_Id) ? new SqlParameter("@User_Pricing_Id", supplier_Pricing.User_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var map_Flag = !string.IsNullOrEmpty(supplier_Pricing.Map_Flag) ? new SqlParameter("@Map_Flag", supplier_Pricing.Map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);
            var shape = !string.IsNullOrEmpty(supplier_Pricing.Shape) ? new SqlParameter("@Shape", supplier_Pricing.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var cts = !string.IsNullOrEmpty(supplier_Pricing.Cts) ? new SqlParameter("@Cts", supplier_Pricing.Cts) : new SqlParameter("@Cts", DBNull.Value);
            var color = !string.IsNullOrEmpty(supplier_Pricing.Color) ? new SqlParameter("@Color", supplier_Pricing.Color) : new SqlParameter("@Color", DBNull.Value);
            var fancy_Color = !string.IsNullOrEmpty(supplier_Pricing.Fancy_Color) ? new SqlParameter("@Fancy_Color", supplier_Pricing.Fancy_Color) : new SqlParameter("@Fancy_Color", DBNull.Value);
            var clarity = !string.IsNullOrEmpty(supplier_Pricing.Clarity) ? new SqlParameter("@Clarity", supplier_Pricing.Clarity) : new SqlParameter("@Clarity", DBNull.Value);
            var cut = !string.IsNullOrEmpty(supplier_Pricing.Cut) ? new SqlParameter("@Cut", supplier_Pricing.Cut) : new SqlParameter("@Cut", DBNull.Value);
            var polish = !string.IsNullOrEmpty(supplier_Pricing.Polish) ? new SqlParameter("@Polish", supplier_Pricing.Polish) : new SqlParameter("@Polish", DBNull.Value);
            var symm = !string.IsNullOrEmpty(supplier_Pricing.Symm) ? new SqlParameter("@Symm", supplier_Pricing.Symm) : new SqlParameter("@Symm", DBNull.Value);
            var fls_Intensity = !string.IsNullOrEmpty(supplier_Pricing.Fls_Intensity) ? new SqlParameter("@Fls_Intensity", supplier_Pricing.Fls_Intensity) : new SqlParameter("@Fls_Intensity", DBNull.Value);
            var lab = !string.IsNullOrEmpty(supplier_Pricing.Lab) ? new SqlParameter("@Lab", supplier_Pricing.Lab) : new SqlParameter("@Lab", DBNull.Value);
            var shade = !string.IsNullOrEmpty(supplier_Pricing.Shade) ? new SqlParameter("@Shade", supplier_Pricing.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var luster = !string.IsNullOrEmpty(supplier_Pricing.Luster) ? new SqlParameter("@Luster", supplier_Pricing.Luster) : new SqlParameter("@Luster", DBNull.Value);
            var bgm = !string.IsNullOrEmpty(supplier_Pricing.Bgm) ? new SqlParameter("@Bgm", supplier_Pricing.Bgm) : new SqlParameter("@Bgm", DBNull.Value);
            var culet = !string.IsNullOrEmpty(supplier_Pricing.Culet) ? new SqlParameter("@Culet", supplier_Pricing.Culet) : new SqlParameter("@Culet", DBNull.Value);
            var location = !string.IsNullOrEmpty(supplier_Pricing.Location) ? new SqlParameter("@Location", supplier_Pricing.Location) : new SqlParameter("@Location", DBNull.Value);
            var status = !string.IsNullOrEmpty(supplier_Pricing.Status) ? new SqlParameter("@Status", supplier_Pricing.Status) : new SqlParameter("@Status", DBNull.Value);
            var good_Type = !string.IsNullOrEmpty(supplier_Pricing.Good_Type) ? new SqlParameter("@Good_Type", supplier_Pricing.Good_Type) : new SqlParameter("@Good_Type", DBNull.Value);
            var length_From = supplier_Pricing.Length_From > 0 ? new SqlParameter("@Length_From", supplier_Pricing.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = supplier_Pricing.Length_To > 0 ? new SqlParameter("@Length_To", supplier_Pricing.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = supplier_Pricing.Width_From > 0 ? new SqlParameter("@Width_From", supplier_Pricing.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = supplier_Pricing.Width_To > 0 ? new SqlParameter("@Width_To", supplier_Pricing.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = supplier_Pricing.Depth_From > 0 ? new SqlParameter("@Depth_From", supplier_Pricing.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = supplier_Pricing.Depth_To > 0 ? new SqlParameter("@Depth_To", supplier_Pricing.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = supplier_Pricing.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", supplier_Pricing.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = supplier_Pricing.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", supplier_Pricing.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = supplier_Pricing.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", supplier_Pricing.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = supplier_Pricing.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", supplier_Pricing.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = supplier_Pricing.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", supplier_Pricing.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = supplier_Pricing.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", supplier_Pricing.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = supplier_Pricing.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", supplier_Pricing.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = supplier_Pricing.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", supplier_Pricing.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = supplier_Pricing.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", supplier_Pricing.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = supplier_Pricing.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", supplier_Pricing.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = supplier_Pricing.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", supplier_Pricing.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = supplier_Pricing.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", supplier_Pricing.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = supplier_Pricing.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", supplier_Pricing.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = supplier_Pricing.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", supplier_Pricing.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var table_Black = !string.IsNullOrEmpty(supplier_Pricing.Table_Black) ? new SqlParameter("@Table_Black", supplier_Pricing.Table_Black) : new SqlParameter("@Table_Black", DBNull.Value);
            var side_Black = !string.IsNullOrEmpty(supplier_Pricing.Side_Black) ? new SqlParameter("@Side_Black", supplier_Pricing.Side_Black) : new SqlParameter("@Side_Black", DBNull.Value);
            var table_White = !string.IsNullOrEmpty(supplier_Pricing.Table_White) ? new SqlParameter("@Table_White", supplier_Pricing.Table_White) : new SqlParameter("@Table_White", DBNull.Value);
            var side_white = !string.IsNullOrEmpty(supplier_Pricing.Side_white) ? new SqlParameter("@Side_white", supplier_Pricing.Side_white) : new SqlParameter("@Side_white", DBNull.Value);
            var cert_Type = !string.IsNullOrEmpty(supplier_Pricing.Cert_Type) ? new SqlParameter("@Cert_Type", supplier_Pricing.Cert_Type) : new SqlParameter("@Cert_Type", DBNull.Value);
            var table_Open = !string.IsNullOrEmpty(supplier_Pricing.Table_Open) ? new SqlParameter("@Table_Open", supplier_Pricing.Table_Open) : new SqlParameter("@Table_Open", DBNull.Value);
            var crown_Open = !string.IsNullOrEmpty(supplier_Pricing.Crown_Open) ? new SqlParameter("@Crown_Open", supplier_Pricing.Crown_Open) : new SqlParameter("@Crown_Open", DBNull.Value);
            var pavilion_Open = !string.IsNullOrEmpty(supplier_Pricing.Pavilion_Open) ? new SqlParameter("@Pavilion_Open", supplier_Pricing.Pavilion_Open) : new SqlParameter("@Pavilion_Open", DBNull.Value);
            var girdle_Open = !string.IsNullOrEmpty(supplier_Pricing.Girdle_Open) ? new SqlParameter("@Girdle_Open", supplier_Pricing.Girdle_Open) : new SqlParameter("@Girdle_Open", DBNull.Value);
            var base_Disc_From = supplier_Pricing.Base_Disc_From > 0 ? new SqlParameter("@Base_Disc_From", supplier_Pricing.Base_Disc_From) : new SqlParameter("@Base_Disc_From", DBNull.Value);
            var base_Disc_To = supplier_Pricing.Base_Disc_To > 0 ? new SqlParameter("@Base_Disc_To", supplier_Pricing.Base_Disc_To) : new SqlParameter("@Base_Disc_To", DBNull.Value);
            var base_Amount_From = supplier_Pricing.Base_Amount_From > 0 ? new SqlParameter("@Base_Amount_From", supplier_Pricing.Base_Amount_From) : new SqlParameter("@Base_Amount_From", DBNull.Value);
            var base_Amount_To = supplier_Pricing.Base_Amount_To > 0 ? new SqlParameter("@Base_Amount_To", supplier_Pricing.Base_Amount_To) : new SqlParameter("@Base_Amount_To", DBNull.Value);
            var company = !string.IsNullOrEmpty(supplier_Pricing.Company) ? new SqlParameter("@Company", supplier_Pricing.Company) : new SqlParameter("@Company", DBNull.Value);
            var supplier_Filter_Type = !string.IsNullOrEmpty(supplier_Pricing.Supplier_Filter_Type) ? new SqlParameter("@Supplier_Filter_Type", supplier_Pricing.Supplier_Filter_Type) : new SqlParameter("@Supplier_Filter_Type", DBNull.Value);
            var calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.Calculation_Type) ? new SqlParameter("@Calculation_Type", supplier_Pricing.Calculation_Type) : new SqlParameter("@Calculation_Type", DBNull.Value);
            var sign = !string.IsNullOrEmpty(supplier_Pricing.Sign) ? new SqlParameter("@Sign", supplier_Pricing.Sign) : new SqlParameter("@Sign", DBNull.Value);
            var value_1 = supplier_Pricing.Value_1 > 0 ? new SqlParameter("@Value_1", supplier_Pricing.Value_1) : new SqlParameter("@Value_1", DBNull.Value);
            var value_2 = supplier_Pricing.Value_2 > 0 ? new SqlParameter("@Value_2", supplier_Pricing.Value_2) : new SqlParameter("@Value_2", DBNull.Value);
            var value_3 = supplier_Pricing.Value_3 > 0 ? new SqlParameter("@Value_3", supplier_Pricing.Value_3) : new SqlParameter("@Value_3", DBNull.Value);
            var value_4 = supplier_Pricing.Value_4 > 0 ? new SqlParameter("@Value_4", supplier_Pricing.Value_4) : new SqlParameter("@Value_4", DBNull.Value);
            var sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.SP_Calculation_Type) ? new SqlParameter("@SP_Calculation_Type", supplier_Pricing.SP_Calculation_Type) : new SqlParameter("@SP_Calculation_Type", DBNull.Value);
            var sp_sign = !string.IsNullOrEmpty(supplier_Pricing.SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Date) ? new SqlParameter("@SP_Start_Date", supplier_Pricing.SP_Start_Date) : new SqlParameter("@SP_Start_Date", DBNull.Value);
            var sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Time) ? new SqlParameter("@SP_Start_Time", supplier_Pricing.SP_Start_Time) : new SqlParameter("@SP_Start_Time", DBNull.Value);
            var sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Date) ? new SqlParameter("@SP_End_Date", supplier_Pricing.SP_End_Date) : new SqlParameter("@SP_End_Date", DBNull.Value);
            var sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Time) ? new SqlParameter("@SP_End_Time", supplier_Pricing.SP_End_Time) : new SqlParameter("@SP_End_Time", DBNull.Value);
            var sp_value_1 = supplier_Pricing.SP_Value_1 > 0 ? new SqlParameter("@SP_Value_1", supplier_Pricing.SP_Value_1) : new SqlParameter("@SP_Value_1", DBNull.Value);
            var sp_value_2 = supplier_Pricing.SP_Value_2 > 0 ? new SqlParameter("@SP_Value_2", supplier_Pricing.SP_Value_2) : new SqlParameter("@SP_Value_2", DBNull.Value);
            var sp_value_3 = supplier_Pricing.SP_Value_3 > 0 ? new SqlParameter("@SP_Value_3", supplier_Pricing.SP_Value_3) : new SqlParameter("@SP_Value_3", DBNull.Value);
            var sp_value_4 = supplier_Pricing.SP_Value_4 > 0 ? new SqlParameter("@SP_Value_4", supplier_Pricing.SP_Value_4) : new SqlParameter("@SP_Value_4", DBNull.Value);
            var ms_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_Calculation_Type) ? new SqlParameter("@MS_Calculation_Type", supplier_Pricing.MS_Calculation_Type) : new SqlParameter("@MS_Calculation_Type", DBNull.Value);
            var ms_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_Sign) ? new SqlParameter("@MS_Sign", supplier_Pricing.MS_Sign) : new SqlParameter("@MS_Sign", DBNull.Value);
            var ms_value_1 = supplier_Pricing.MS_Value_1 > 0 ? new SqlParameter("@MS_Value_1", supplier_Pricing.MS_Value_1) : new SqlParameter("@MS_Value_1", DBNull.Value);
            var ms_value_2 = supplier_Pricing.MS_Value_2 > 0 ? new SqlParameter("@MS_Value_2", supplier_Pricing.MS_Value_2) : new SqlParameter("@MS_Value_2", DBNull.Value);
            var ms_value_3 = supplier_Pricing.MS_Value_3 > 0 ? new SqlParameter("@MS_Value_3", supplier_Pricing.MS_Value_3) : new SqlParameter("@MS_Value_3", DBNull.Value);
            var ms_value_4 = supplier_Pricing.MS_Value_4 > 0 ? new SqlParameter("@MS_Value_4", supplier_Pricing.MS_Value_4) : new SqlParameter("@MS_Value_4", DBNull.Value);
            var ms_sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Calculation_Type) ? new SqlParameter("@MS_SP_Calculation_Type", supplier_Pricing.MS_SP_Calculation_Type) : new SqlParameter("@MS_SP_Calculation_Type", DBNull.Value);
            var ms_sp_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Sign) ? new SqlParameter("@MS_SP_Sign", supplier_Pricing.MS_SP_Sign) : new SqlParameter("@MS_SP_Sign", DBNull.Value);
            var ms_sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Date) ? new SqlParameter("@MS_SP_Start_Date", supplier_Pricing.MS_SP_Start_Date) : new SqlParameter("@MS_SP_Start_Date", DBNull.Value);
            var ms_sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Time) ? new SqlParameter("@MS_SP_Start_Time", supplier_Pricing.MS_SP_Start_Time) : new SqlParameter("@MS_SP_Start_Time", DBNull.Value);
            var ms_sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Date) ? new SqlParameter("@MS_SP_End_Date", supplier_Pricing.MS_SP_End_Date) : new SqlParameter("@MS_SP_End_Date", DBNull.Value);
            var ms_sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Time) ? new SqlParameter("@MS_SP_End_Time", supplier_Pricing.MS_SP_End_Time) : new SqlParameter("@MS_SP_End_Time", DBNull.Value);
            var ms_sp_value_1 = supplier_Pricing.MS_SP_Value_1 > 0 ? new SqlParameter("@MS_SP_Value_1", supplier_Pricing.MS_SP_Value_1) : new SqlParameter("@MS_SP_Value_1", DBNull.Value);
            var ms_sp_value_2 = supplier_Pricing.MS_SP_Value_2 > 0 ? new SqlParameter("@MS_SP_Value_2", supplier_Pricing.MS_SP_Value_2) : new SqlParameter("@MS_SP_Value_2", DBNull.Value);
            var ms_sp_value_3 = supplier_Pricing.MS_SP_Value_3 > 0 ? new SqlParameter("@MS_SP_Value_3", supplier_Pricing.MS_SP_Value_3) : new SqlParameter("@MS_SP_Value_3", DBNull.Value);
            var ms_sp_value_4 = supplier_Pricing.MS_SP_Value_4 > 0 ? new SqlParameter("@MS_SP_Value_4", supplier_Pricing.MS_SP_Value_4) : new SqlParameter("@MS_SP_Value_4", DBNull.Value);
            var sP_Toggle_Bar = new SqlParameter("@SP_Toggle_Bar", supplier_Pricing.SP_Toggle_Bar);
            var mSP_Toggle_Bar = new SqlParameter("@MS_SP_Toggle_Bar", supplier_Pricing.MS_SP_Toggle_Bar);
            var modified_By = supplier_Pricing.Modified_By > 0 ? new SqlParameter("@Modified_By", supplier_Pricing.Modified_By) : new SqlParameter("@Modified_By", DBNull.Value);

            await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Trace_Insert @Employee_Id, @IP_Address, @Trace_Date, @Trace_Time, @Record_Type, @Supplier_Pricing_Id, @Supplier_Id, @Sunrise_Pricing_Id, @Customer_Pricing_Id, @User_Pricing_Id, @Map_Flag, @Shape, @Cts, @Color, @Fancy_Color, @Clarity, @Cut, @Polish, @Symm,
                        @Fls_Intensity, @Lab, @Shade, @Luster, @Bgm, @Culet, @Location, @Status, @Good_Type, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From,
                        @Depth_Per_To, @Table_Per_From, @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Table_Black, @Side_Black, @Table_White, @Side_white, @Cert_Type, @Table_Open, @Crown_Open, @Pavilion_Open,
                        @Girdle_Open, @Base_Disc_From, @Base_Disc_To, @Base_Amount_From, @Base_Amount_To, @Company, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4,
                        @SP_Calculation_Type, @SP_Sign, @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type,
                        @MS_Sign, @MS_Value_1, @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time,
                        @MS_SP_Value_1, @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4, @SP_Toggle_Bar, @MS_SP_Toggle_Bar, @Modified_By",
            empId, ipaddress, date, time, record_Type, supplier_Pricing_Id, supplier_Id, sunrise_Pricing_Id, customer_Pricing_Id, user_Pricing_Id, map_Flag, shape, cts, color, fancy_Color, clarity, cut, polish, symm, fls_Intensity, lab, shade, luster, bgm, culet, location, status, good_Type, length_From, length_To, width_From,
            width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
            pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, table_Black, side_Black, table_White, side_white, cert_Type, table_Open, crown_Open, pavilion_Open, girdle_Open,
            base_Disc_From, base_Disc_To, base_Amount_From, base_Amount_To, company, supplier_Filter_Type, calculation_Type, sign, value_1, value_2, value_3, value_4, sp_calculation_Type, sp_sign, sp_start_date,
            sp_start_time, sp_end_date, sp_end_time, sp_value_1, sp_value_2, sp_value_3, sp_value_4, ms_calculation_Type, ms_sign, ms_value_1, ms_value_2, ms_value_3, ms_value_4, ms_sp_calculation_Type,
            ms_sp_sign, ms_sp_start_date, ms_sp_start_time, ms_sp_end_date, ms_sp_end_time, ms_sp_value_1, ms_sp_value_2, ms_sp_value_3, ms_sp_value_4, sP_Toggle_Bar, mSP_Toggle_Bar, modified_By));
        }
        #endregion
        public partial class Common_Model
        {
            public int Id { get; set; }
        }

        #region Supplier Column And Value
        public async Task<int> Insert_Update_Supplier_Value_Mapping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblSupplier_Value_Mapping", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Value_Mapping_Table_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @tblSupplier_Value_Mapping", parameter));

            return result;
        }
        public async Task<int> InsertSupplierValueMapping(Supplier_Value_Mapping supplier_Value)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@SupId", supplier_Value.Sup_Id));
            parameter.Add(new SqlParameter("@SuppCatname", supplier_Value.Supp_Cat_Name));
            parameter.Add(new SqlParameter("@CatValId", supplier_Value.Cat_val_Id));
            parameter.Add(new SqlParameter("@Status", supplier_Value.Status));
            parameter.Add(new SqlParameter("@RecordType", "Insert"));

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @SupId, @SuppCatname, @CatValId, @Status, @RecordType", parameter.ToArray()));

            return result;
        }
        public async Task<int> UpdateSupplierValueMapping(Supplier_Value_Mapping supplier_Value)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@SupId", supplier_Value.Sup_Id));
            parameter.Add(new SqlParameter("@SuppCatname", supplier_Value.Supp_Cat_Name));
            parameter.Add(new SqlParameter("@CatValId", supplier_Value.Cat_val_Id));
            parameter.Add(new SqlParameter("@Status", supplier_Value.Status));
            parameter.Add(new SqlParameter("@RecordType", "Update"));

            var result = await Task.Run(() => _dbContext.Database
          .ExecuteSqlRawAsync(@"exec Supplier_Value_Mapping_Insert_Update @SupId, @SuppCatname, @CatValId, @Status, @RecordType", parameter.ToArray()));

            return result;
        }
        public async Task<int> DeleteSupplierValueMapping(int supId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Supplier_Value_Delete {supId}"));
        }
        public async Task<IList<Supplier_Value_Mapping>> Get_Supplier_Value_Mapping(int sup_Id, int col_Id)
        {
            var _sup_Id = sup_Id > 0 ? new SqlParameter("@Sup_Id", sup_Id) : new SqlParameter("@Sup_Id", DBNull.Value);
            var _col_Id = col_Id > 0 ? new SqlParameter("@Col_Id", col_Id) : new SqlParameter("@Col_Id", DBNull.Value);

            var categoryValue = await Task.Run(() => _dbContext.Supplier_Value_Mapping
                            .FromSqlRaw(@"exec Supplier_Value_Mapping_Select @Sup_Id, @Col_Id", _sup_Id, _col_Id).ToListAsync());

            return categoryValue;
        }
        public async Task<int> Add_Update_Supplier_Column_Mapping(DataTable dataTable)
        {
            var parameter = new SqlParameter("@supplier_column", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Column_Mapping_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Column_Mapping_Insert_Update @supplier_column", parameter));
            return result;
        }
        public async Task<IList<Supplier_Column_Mapping>> Get_Supplier_Column_Mapping(int supp_Id, string map_Flag, string column_Type)
        {
            var _supp_Id = supp_Id > 0 ? new SqlParameter("@Supp_Id", supp_Id) : new SqlParameter("@Supp_Id", DBNull.Value);
            var _map_Flag = !string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);
            var _column_Type = !string.IsNullOrEmpty(column_Type) ? new SqlParameter("@Column_Type", column_Type) : new SqlParameter("@Column_Type", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Supplier_Column_Mapping
                            .FromSqlRaw(@"exec Supplier_Column_Mapping_Select @Supp_Id, @Map_Flag, @Column_Type", _supp_Id, _map_Flag, _column_Type)
                            .ToListAsync());
            return result;
        }
        public async Task<DataTable> Get_Supplier_Column_Mapping_In_Datatable(int supp_Id, string map_Flag, string column_Type)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Column_Mapping_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supp_Id > 0 ? new SqlParameter("@Supp_Id", supp_Id) : new SqlParameter("@Supp_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(column_Type) ? new SqlParameter("@Column_Type", column_Type) : new SqlParameter("@Column_Type", DBNull.Value));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }

            return dataTable;
        }
        #endregion

        #region Value Config
        public async Task<int> Add_Update_Value_Config(Value_Config value_Config)
        {
            var valueMap_ID = new SqlParameter("@ValueMap_ID", value_Config.ValueMap_ID);
            var length_From = value_Config.Length_From > 0 ? new SqlParameter("@Length_From", value_Config.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = value_Config.Length_To > 0 ? new SqlParameter("@Length_To", value_Config.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = value_Config.Width_From > 0 ? new SqlParameter("@Width_From", value_Config.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = value_Config.Width_To > 0 ? new SqlParameter("@Width_To", value_Config.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = value_Config.Depth_From > 0 ? new SqlParameter("@Depth_From", value_Config.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = value_Config.Depth_To > 0 ? new SqlParameter("@Depth_To", value_Config.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = value_Config.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", value_Config.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = value_Config.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", value_Config.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = value_Config.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", value_Config.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = value_Config.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", value_Config.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = value_Config.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", value_Config.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = value_Config.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", value_Config.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = value_Config.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", value_Config.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = value_Config.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", value_Config.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = value_Config.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", value_Config.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = value_Config.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", value_Config.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = value_Config.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", value_Config.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = value_Config.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", value_Config.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = value_Config.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", value_Config.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = value_Config.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", value_Config.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var lr_Half_From = value_Config.Lr_Half_From > 0 ? new SqlParameter("@Lr_Half_From", value_Config.Lr_Half_From) : new SqlParameter("@Lr_Half_From", DBNull.Value);
            var lr_Half_To = value_Config.Lr_Half_To > 0 ? new SqlParameter("@Lr_Half_To", value_Config.Lr_Half_To) : new SqlParameter("@Lr_Half_To", DBNull.Value);
            var star_Ln_From = value_Config.Star_Ln_From > 0 ? new SqlParameter("@Star_Ln_From", value_Config.Star_Ln_From) : new SqlParameter("@Star_Ln_From", DBNull.Value);
            var star_Ln_To = value_Config.Star_Ln_To > 0 ? new SqlParameter("@Star_Ln_To", value_Config.Star_Ln_To) : new SqlParameter("@Star_Ln_To", DBNull.Value);
            var shape_Group = !string.IsNullOrEmpty(value_Config.Shape_Group) ? new SqlParameter("@Shape_Group", value_Config.Shape_Group) : new SqlParameter("@Shape_Group", DBNull.Value);
            var shape = !string.IsNullOrEmpty(value_Config.Shape) ? new SqlParameter("@Shape", value_Config.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var user_Id = value_Config.User_Id > 0 ? new SqlParameter("@User_Id", value_Config.User_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Value_Config_Insert_Update @ValueMap_ID, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From, @Depth_Per_To, @Table_Per_From,
                        @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Lr_Half_From, @Lr_Half_To, @Star_Ln_From, @Star_Ln_To, @Shape_Group, @Shape,@User_Id", valueMap_ID, length_From, length_To,
                        width_From, width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
                        pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, lr_Half_From, lr_Half_To, star_Ln_From, star_Ln_To, shape_Group, shape, user_Id));

            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    if (value_Config.ValueMap_ID > 0)
            //        await Insert_Value_Config_Trace(value_Config, "Update");
            //    else
            //        await Insert_Value_Config_Trace(value_Config, "Insert");
            //}

            return result;
        }
        public async Task<int> Delete_Value_Config(int valueMap_ID)
        {
            //if (CoreService.Enable_Trace_Records(_configuration))
            //{
            //    var _valueMap_ID = valueMap_ID > 0 ? new SqlParameter("@ValueMap_ID", valueMap_ID) : new SqlParameter("@ValueMap_ID", DBNull.Value);

            //    var result = await Task.Run(() => _dbContext.Value_Config
            //                    .FromSqlRaw(@"exec Value_Config_Select @ValueMap_ID", _valueMap_ID)
            //                    .AsEnumerable()
            //                    .FirstOrDefault());
            //    if(result != null)
            //    {
            //        await Insert_Value_Config_Trace(result, "Delete");
            //    }
            //}
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Value_Config_Delete {valueMap_ID}"));
        }
        public async Task<IList<Value_Config>> Get_Value_Config(int valueMap_ID)
        {
            var _valueMap_ID = valueMap_ID > 0 ? new SqlParameter("@ValueMap_ID", valueMap_ID) : new SqlParameter("@ValueMap_ID", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Value_Config
                            .FromSqlRaw(@"exec Value_Config_Select @ValueMap_ID", _valueMap_ID).ToListAsync());

            return result;
        }
        #endregion

        #region Supplier Pricing
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Pricing_List()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Pricing_Select_List", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

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
                }
            }

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Sunrise_Pricing_List()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Sunrise_Pricing_Select_List", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

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
                }
            }

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Customer_Pricing_List()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Customer_Pricing_Select_List", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

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
                }
            }

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id, string? user_pricing_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("Supplier_Pricing_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supplier_Pricing_Id > 0 ? new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing_Id) : new SqlParameter("@Supplier_Pricing_Id", DBNull.Value));
                    command.Parameters.Add(supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(supplier_Filter_Type) ? new SqlParameter("@Supplier_Filter_Type", supplier_Filter_Type) : new SqlParameter("@Supplier_Filter_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(map_Flag) ? new SqlParameter("@Map_Flag", map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value));
                    command.Parameters.Add(sunrise_pricing_Id > 0 ? new SqlParameter("@Sunrise_pricing_Id", sunrise_pricing_Id) : new SqlParameter("@Sunrise_pricing_Id", DBNull.Value));
                    command.Parameters.Add(customer_pricing_Id > 0 ? new SqlParameter("@Customer_Pricing_Id", customer_pricing_Id) : new SqlParameter("@Customer_Pricing_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(user_pricing_Id) ? new SqlParameter("@User_Pricing_Id", user_pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value));

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

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

                        int supplierPricingId = Convert.ToInt32(dict["Supplier_Pricing_Id"]);
                        var supp_Pricing_Key_To_Sym_List = new List<Dictionary<string, object>>();

                        using (var cmd = new SqlCommand("Supplier_Pricing_Key_To_Symbol_Select", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(supplierPricingId > 0 ? new SqlParameter("@Supplier_Pricing_Id", supplierPricingId) : new SqlParameter("@Supplier_Pricing_Id", DBNull.Value));

                            using var daSym = new SqlDataAdapter();
                            daSym.SelectCommand = cmd;

                            using var dsSym = new DataSet();
                            daSym.Fill(dsSym);

                            var dataTableSym = dsSym.Tables[dsSym.Tables.Count - 1];

                            foreach (DataRow rowSym in dataTableSym.Rows)
                            {
                                var dictSym = new Dictionary<string, object>();
                                foreach (DataColumn colSym in dataTableSym.Columns)
                                {
                                    if (rowSym[colSym] == DBNull.Value)
                                    {
                                        dictSym[colSym.ColumnName] = null;
                                    }
                                    else
                                    {
                                        dictSym[colSym.ColumnName] = rowSym[colSym];
                                    }
                                }
                                supp_Pricing_Key_To_Sym_List.Add(dictSym);
                            }
                        }

                        dict["Key_To_Symbol"] = supp_Pricing_Key_To_Sym_List;


                        var supp_Comments_List = new List<Dictionary<string, object>>();

                        using (var cmd = new SqlCommand("Supplier_Pricing_Comments_Select", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(supplierPricingId > 0 ? new SqlParameter("@Supplier_Pricing_Id", supplierPricingId) : new SqlParameter("@Supplier_Pricing_Id", DBNull.Value));

                            using var daSym = new SqlDataAdapter();
                            daSym.SelectCommand = cmd;

                            using var dsSym = new DataSet();
                            daSym.Fill(dsSym);

                            var dataTableSym = dsSym.Tables[dsSym.Tables.Count - 1];

                            foreach (DataRow rowSym in dataTableSym.Rows)
                            {
                                var dictSym = new Dictionary<string, object>();
                                foreach (DataColumn colSym in dataTableSym.Columns)
                                {
                                    if (rowSym[colSym] == DBNull.Value)
                                    {
                                        dictSym[colSym.ColumnName] = null;
                                    }
                                    else
                                    {
                                        dictSym[colSym.ColumnName] = rowSym[colSym];
                                    }
                                }
                                supp_Comments_List.Add(dictSym);
                            }
                        }

                        dict["Lab_Comments"] = supp_Comments_List;
                        result.Add(dict);
                    }
                }
                await connection.CloseAsync();
            }

            return result;
        }
        public async Task<(string, int)> Add_Update_Supplier_Pricing(Supplier_Pricing supplier_Pricing)
        {
            var supplier_Pricing_Id = new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing.Supplier_Pricing_Id);
            var supplier_Id = supplier_Pricing.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Pricing.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var sunrise_Pricing_Id = supplier_Pricing.Sunrise_Pricing_Id > 0 ? new SqlParameter("@Sunrise_Pricing_Id", supplier_Pricing.Sunrise_Pricing_Id) : new SqlParameter("@Sunrise_Pricing_Id", DBNull.Value);
            var customer_Pricing_Id = supplier_Pricing.Customer_Pricing_Id > 0 ? new SqlParameter("@Customer_Pricing_Id", supplier_Pricing.Customer_Pricing_Id) : new SqlParameter("@Customer_Pricing_Id", DBNull.Value);
            var user_Pricing_Id = !string.IsNullOrEmpty(supplier_Pricing.User_Pricing_Id) ? new SqlParameter("@User_Pricing_Id", supplier_Pricing.User_Pricing_Id) : new SqlParameter("@User_Pricing_Id", DBNull.Value);
            var map_Flag = !string.IsNullOrEmpty(supplier_Pricing.Map_Flag) ? new SqlParameter("@Map_Flag", supplier_Pricing.Map_Flag) : new SqlParameter("@Map_Flag", DBNull.Value);
            var stock_Lab = supplier_Pricing.Stock_Lab != null ? new SqlParameter("@Stock_Lab", supplier_Pricing.Stock_Lab) : new SqlParameter("@Stock_Lab", DBNull.Value);
            var stock_Overseas = supplier_Pricing.Stock_Overseas != null ? new SqlParameter("@Stock_Overseas", supplier_Pricing.Stock_Overseas) : new SqlParameter("@Stock_Overseas", DBNull.Value);
            var stock_Buyer = supplier_Pricing.Stock_Buyer != null ? new SqlParameter("@Stock_Buyer", supplier_Pricing.Stock_Buyer) : new SqlParameter("@Stock_Buyer", DBNull.Value);
            var stock_Saler = supplier_Pricing.Stock_Saler != null ? new SqlParameter("@Stock_Saler", supplier_Pricing.Stock_Saler) : new SqlParameter("@Stock_Saler", DBNull.Value);
            var stock_Defualt = supplier_Pricing.Stock_Defualt != null ? new SqlParameter("@Stock_Defualt", supplier_Pricing.Stock_Defualt) : new SqlParameter("@Stock_Defualt", DBNull.Value);
            var shape = !string.IsNullOrEmpty(supplier_Pricing.Shape) ? new SqlParameter("@Shape", supplier_Pricing.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var cts = !string.IsNullOrEmpty(supplier_Pricing.Cts) ? new SqlParameter("@Cts", supplier_Pricing.Cts) : new SqlParameter("@Cts", DBNull.Value);
            var color = !string.IsNullOrEmpty(supplier_Pricing.Color) ? new SqlParameter("@Color", supplier_Pricing.Color) : new SqlParameter("@Color", DBNull.Value);
            var fancy_Color = !string.IsNullOrEmpty(supplier_Pricing.Fancy_Color) ? new SqlParameter("@Fancy_Color", supplier_Pricing.Fancy_Color) : new SqlParameter("@Fancy_Color", DBNull.Value);
            var clarity = !string.IsNullOrEmpty(supplier_Pricing.Clarity) ? new SqlParameter("@Clarity", supplier_Pricing.Clarity) : new SqlParameter("@Clarity", DBNull.Value);
            var cut = !string.IsNullOrEmpty(supplier_Pricing.Cut) ? new SqlParameter("@Cut", supplier_Pricing.Cut) : new SqlParameter("@Cut", DBNull.Value);
            var polish = !string.IsNullOrEmpty(supplier_Pricing.Polish) ? new SqlParameter("@Polish", supplier_Pricing.Polish) : new SqlParameter("@Polish", DBNull.Value);
            var symm = !string.IsNullOrEmpty(supplier_Pricing.Symm) ? new SqlParameter("@Symm", supplier_Pricing.Symm) : new SqlParameter("@Symm", DBNull.Value);
            var fls_Intensity = !string.IsNullOrEmpty(supplier_Pricing.Fls_Intensity) ? new SqlParameter("@Fls_Intensity", supplier_Pricing.Fls_Intensity) : new SqlParameter("@Fls_Intensity", DBNull.Value);
            var lab = !string.IsNullOrEmpty(supplier_Pricing.Lab) ? new SqlParameter("@Lab", supplier_Pricing.Lab) : new SqlParameter("@Lab", DBNull.Value);
            var shade = !string.IsNullOrEmpty(supplier_Pricing.Shade) ? new SqlParameter("@Shade", supplier_Pricing.Shade) : new SqlParameter("@Shade", DBNull.Value);
            var luster = !string.IsNullOrEmpty(supplier_Pricing.Luster) ? new SqlParameter("@Luster", supplier_Pricing.Luster) : new SqlParameter("@Luster", DBNull.Value);
            var bgm = !string.IsNullOrEmpty(supplier_Pricing.Bgm) ? new SqlParameter("@Bgm", supplier_Pricing.Bgm) : new SqlParameter("@Bgm", DBNull.Value);
            var culet = !string.IsNullOrEmpty(supplier_Pricing.Culet) ? new SqlParameter("@Culet", supplier_Pricing.Culet) : new SqlParameter("@Culet", DBNull.Value);
            var location = !string.IsNullOrEmpty(supplier_Pricing.Location) ? new SqlParameter("@Location", supplier_Pricing.Location) : new SqlParameter("@Location", DBNull.Value);
            var status = !string.IsNullOrEmpty(supplier_Pricing.Status) ? new SqlParameter("@Status", supplier_Pricing.Status) : new SqlParameter("@Status", DBNull.Value);
            var good_Type = !string.IsNullOrEmpty(supplier_Pricing.Good_Type) ? new SqlParameter("@Good_Type", supplier_Pricing.Good_Type) : new SqlParameter("@Good_Type", DBNull.Value);
            var length_From = supplier_Pricing.Length_From > 0 ? new SqlParameter("@Length_From", supplier_Pricing.Length_From) : new SqlParameter("@Length_From", DBNull.Value);
            var length_To = supplier_Pricing.Length_To > 0 ? new SqlParameter("@Length_To", supplier_Pricing.Length_To) : new SqlParameter("@Length_To", DBNull.Value);
            var width_From = supplier_Pricing.Width_From > 0 ? new SqlParameter("@Width_From", supplier_Pricing.Width_From) : new SqlParameter("@Width_From", DBNull.Value);
            var width_To = supplier_Pricing.Width_To > 0 ? new SqlParameter("@Width_To", supplier_Pricing.Width_To) : new SqlParameter("@Width_To", DBNull.Value);
            var depth_From = supplier_Pricing.Depth_From > 0 ? new SqlParameter("@Depth_From", supplier_Pricing.Depth_From) : new SqlParameter("@Depth_From", DBNull.Value);
            var depth_To = supplier_Pricing.Depth_To > 0 ? new SqlParameter("@Depth_To", supplier_Pricing.Depth_To) : new SqlParameter("@Depth_To", DBNull.Value);
            var depth_Per_From = supplier_Pricing.Depth_Per_From > 0 ? new SqlParameter("@Depth_Per_From", supplier_Pricing.Depth_Per_From) : new SqlParameter("@Depth_Per_From", DBNull.Value);
            var depth_Per_To = supplier_Pricing.Depth_Per_To > 0 ? new SqlParameter("@Depth_Per_To", supplier_Pricing.Depth_Per_To) : new SqlParameter("@Depth_Per_To", DBNull.Value);
            var table_Per_From = supplier_Pricing.Table_Per_From > 0 ? new SqlParameter("@Table_Per_From", supplier_Pricing.Table_Per_From) : new SqlParameter("@Table_Per_From", DBNull.Value);
            var table_Per_To = supplier_Pricing.Table_Per_To > 0 ? new SqlParameter("@Table_Per_To", supplier_Pricing.Table_Per_To) : new SqlParameter("@Table_Per_To", DBNull.Value);
            var crown_Angle_From = supplier_Pricing.Crown_Angle_From > 0 ? new SqlParameter("@Crown_Angle_From", supplier_Pricing.Crown_Angle_From) : new SqlParameter("@Crown_Angle_From", DBNull.Value);
            var crown_Angle_To = supplier_Pricing.Crown_Angle_To > 0 ? new SqlParameter("@Crown_Angle_To", supplier_Pricing.Crown_Angle_To) : new SqlParameter("@Crown_Angle_To", DBNull.Value);
            var crown_Height_From = supplier_Pricing.Crown_Height_From > 0 ? new SqlParameter("@Crown_Height_From", supplier_Pricing.Crown_Height_From) : new SqlParameter("@Crown_Height_From", DBNull.Value);
            var crown_Height_To = supplier_Pricing.Crown_Height_To > 0 ? new SqlParameter("@Crown_Height_To", supplier_Pricing.Crown_Height_To) : new SqlParameter("@Crown_Height_To", DBNull.Value);
            var pavilion_Angle_From = supplier_Pricing.Pavilion_Angle_From > 0 ? new SqlParameter("@Pavilion_Angle_From", supplier_Pricing.Pavilion_Angle_From) : new SqlParameter("@Pavilion_Angle_From", DBNull.Value);
            var pavilion_Angle_To = supplier_Pricing.Pavilion_Angle_To > 0 ? new SqlParameter("@Pavilion_Angle_To", supplier_Pricing.Pavilion_Angle_To) : new SqlParameter("@Pavilion_Angle_To", DBNull.Value);
            var pavilion_Height_From = supplier_Pricing.Pavilion_Height_From > 0 ? new SqlParameter("@Pavilion_Height_From", supplier_Pricing.Pavilion_Height_From) : new SqlParameter("@Pavilion_Height_From", DBNull.Value);
            var pavilion_Height_To = supplier_Pricing.Pavilion_Height_To > 0 ? new SqlParameter("@Pavilion_Height_To", supplier_Pricing.Pavilion_Height_To) : new SqlParameter("@Pavilion_Height_To", DBNull.Value);
            var girdle_Per_From = supplier_Pricing.Girdle_Per_From > 0 ? new SqlParameter("@Girdle_Per_From", supplier_Pricing.Girdle_Per_From) : new SqlParameter("@Girdle_Per_From", DBNull.Value);
            var girdle_Per_To = supplier_Pricing.Girdle_Per_To > 0 ? new SqlParameter("@Girdle_Per_To", supplier_Pricing.Girdle_Per_To) : new SqlParameter("@Girdle_Per_To", DBNull.Value);
            var table_Black = !string.IsNullOrEmpty(supplier_Pricing.Table_Black) ? new SqlParameter("@Table_Black", supplier_Pricing.Table_Black) : new SqlParameter("@Table_Black", DBNull.Value);
            var side_Black = !string.IsNullOrEmpty(supplier_Pricing.Side_Black) ? new SqlParameter("@Side_Black", supplier_Pricing.Side_Black) : new SqlParameter("@Side_Black", DBNull.Value);
            var table_White = !string.IsNullOrEmpty(supplier_Pricing.Table_White) ? new SqlParameter("@Table_White", supplier_Pricing.Table_White) : new SqlParameter("@Table_White", DBNull.Value);
            var side_white = !string.IsNullOrEmpty(supplier_Pricing.Side_white) ? new SqlParameter("@Side_white", supplier_Pricing.Side_white) : new SqlParameter("@Side_white", DBNull.Value);
            var cert_Type = !string.IsNullOrEmpty(supplier_Pricing.Cert_Type) ? new SqlParameter("@Cert_Type", supplier_Pricing.Cert_Type) : new SqlParameter("@Cert_Type", DBNull.Value);
            var table_Open = !string.IsNullOrEmpty(supplier_Pricing.Table_Open) ? new SqlParameter("@Table_Open", supplier_Pricing.Table_Open) : new SqlParameter("@Table_Open", DBNull.Value);
            var crown_Open = !string.IsNullOrEmpty(supplier_Pricing.Crown_Open) ? new SqlParameter("@Crown_Open", supplier_Pricing.Crown_Open) : new SqlParameter("@Crown_Open", DBNull.Value);
            var pavilion_Open = !string.IsNullOrEmpty(supplier_Pricing.Pavilion_Open) ? new SqlParameter("@Pavilion_Open", supplier_Pricing.Pavilion_Open) : new SqlParameter("@Pavilion_Open", DBNull.Value);
            var girdle_Open = !string.IsNullOrEmpty(supplier_Pricing.Girdle_Open) ? new SqlParameter("@Girdle_Open", supplier_Pricing.Girdle_Open) : new SqlParameter("@Girdle_Open", DBNull.Value);
            var base_Disc_From = supplier_Pricing.Base_Disc_From > 0 ? new SqlParameter("@Base_Disc_From", supplier_Pricing.Base_Disc_From) : new SqlParameter("@Base_Disc_From", DBNull.Value);
            var base_Disc_To = supplier_Pricing.Base_Disc_To > 0 ? new SqlParameter("@Base_Disc_To", supplier_Pricing.Base_Disc_To) : new SqlParameter("@Base_Disc_To", DBNull.Value);
            var base_Amount_From = supplier_Pricing.Base_Amount_From > 0 ? new SqlParameter("@Base_Amount_From", supplier_Pricing.Base_Amount_From) : new SqlParameter("@Base_Amount_From", DBNull.Value);
            var base_Amount_To = supplier_Pricing.Base_Amount_To > 0 ? new SqlParameter("@Base_Amount_To", supplier_Pricing.Base_Amount_To) : new SqlParameter("@Base_Amount_To", DBNull.Value);

            SqlParameter Final_Disc_From_Param;
            if (supplier_Pricing.Final_Disc_From == null)
            {
                Final_Disc_From_Param = new SqlParameter("@Final_Disc_From", DBNull.Value);
            }
            else
            {
                Final_Disc_From_Param = new SqlParameter("@Final_Disc_From", supplier_Pricing.Final_Disc_From);
            }
            var final_Disc_From = Final_Disc_From_Param;

            SqlParameter Final_Disc_To_Param;
            if (supplier_Pricing.Final_Disc_To == null)
            {
                Final_Disc_To_Param = new SqlParameter("@Final_Disc_To", DBNull.Value);
            }
            else
            {
                Final_Disc_To_Param = new SqlParameter("@Final_Disc_To", supplier_Pricing.Final_Disc_To);
            }
            var final_Disc_To = Final_Disc_To_Param;

            var final_Amount_From = supplier_Pricing.Final_Amount_From > 0 ? new SqlParameter("@Final_Amount_From", supplier_Pricing.Final_Amount_From) : new SqlParameter("@Final_Amount_From", DBNull.Value);
            var final_Amount_To = supplier_Pricing.Final_Amount_To > 0 ? new SqlParameter("@Final_Amount_To", supplier_Pricing.Final_Amount_To) : new SqlParameter("@Final_Amount_To", DBNull.Value);
            var company = !string.IsNullOrEmpty(supplier_Pricing.Company) ? new SqlParameter("@Company", supplier_Pricing.Company) : new SqlParameter("@Company", DBNull.Value);
            var supplier_Filter_Type = !string.IsNullOrEmpty(supplier_Pricing.Supplier_Filter_Type) ? new SqlParameter("@Supplier_Filter_Type", supplier_Pricing.Supplier_Filter_Type) : new SqlParameter("@Supplier_Filter_Type", DBNull.Value);
            var calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.Calculation_Type) ? new SqlParameter("@Calculation_Type", supplier_Pricing.Calculation_Type) : new SqlParameter("@Calculation_Type", DBNull.Value);
            var sign = !string.IsNullOrEmpty(supplier_Pricing.Sign) ? new SqlParameter("@Sign", supplier_Pricing.Sign) : new SqlParameter("@Sign", DBNull.Value);
            var value_1 = supplier_Pricing.Value_1.HasValue ? new SqlParameter("@Value_1", supplier_Pricing.Value_1) : new SqlParameter("@Value_1", DBNull.Value);
            var value_2 = supplier_Pricing.Value_2.HasValue ? new SqlParameter("@Value_2", supplier_Pricing.Value_2) : new SqlParameter("@Value_2", DBNull.Value);
            var value_3 = supplier_Pricing.Value_3.HasValue ? new SqlParameter("@Value_3", supplier_Pricing.Value_3) : new SqlParameter("@Value_3", DBNull.Value);
            var value_4 = supplier_Pricing.Value_4.HasValue ? new SqlParameter("@Value_4", supplier_Pricing.Value_4) : new SqlParameter("@Value_4", DBNull.Value);
            var sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.SP_Calculation_Type) ? new SqlParameter("@SP_Calculation_Type", supplier_Pricing.SP_Calculation_Type) : new SqlParameter("@SP_Calculation_Type", DBNull.Value);
            var sp_sign = !string.IsNullOrEmpty(supplier_Pricing.SP_Sign) ? new SqlParameter("@SP_Sign", supplier_Pricing.SP_Sign) : new SqlParameter("@SP_Sign", DBNull.Value);
            var sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Date) ? new SqlParameter("@SP_Start_Date", supplier_Pricing.SP_Start_Date) : new SqlParameter("@SP_Start_Date", DBNull.Value);
            var sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.SP_Start_Time) ? new SqlParameter("@SP_Start_Time", supplier_Pricing.SP_Start_Time) : new SqlParameter("@SP_Start_Time", DBNull.Value);
            var sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Date) ? new SqlParameter("@SP_End_Date", supplier_Pricing.SP_End_Date) : new SqlParameter("@SP_End_Date", DBNull.Value);
            var sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.SP_End_Time) ? new SqlParameter("@SP_End_Time", supplier_Pricing.SP_End_Time) : new SqlParameter("@SP_End_Time", DBNull.Value);
            var sp_value_1 = supplier_Pricing.SP_Value_1.HasValue ? new SqlParameter("@SP_Value_1", supplier_Pricing.SP_Value_1) : new SqlParameter("@SP_Value_1", DBNull.Value);
            var sp_value_2 = supplier_Pricing.SP_Value_2.HasValue ? new SqlParameter("@SP_Value_2", supplier_Pricing.SP_Value_2) : new SqlParameter("@SP_Value_2", DBNull.Value);
            var sp_value_3 = supplier_Pricing.SP_Value_3.HasValue ? new SqlParameter("@SP_Value_3", supplier_Pricing.SP_Value_3) : new SqlParameter("@SP_Value_3", DBNull.Value);
            var sp_value_4 = supplier_Pricing.SP_Value_4.HasValue ? new SqlParameter("@SP_Value_4", supplier_Pricing.SP_Value_4) : new SqlParameter("@SP_Value_4", DBNull.Value);
            var ms_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_Calculation_Type) ? new SqlParameter("@MS_Calculation_Type", supplier_Pricing.MS_Calculation_Type) : new SqlParameter("@MS_Calculation_Type", DBNull.Value);
            var ms_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_Sign) ? new SqlParameter("@MS_Sign", supplier_Pricing.MS_Sign) : new SqlParameter("@MS_Sign", DBNull.Value);
            var ms_value_1 = supplier_Pricing.MS_Value_1.HasValue ? new SqlParameter("@MS_Value_1", supplier_Pricing.MS_Value_1) : new SqlParameter("@MS_Value_1", DBNull.Value);
            var ms_value_2 = supplier_Pricing.MS_Value_2.HasValue ? new SqlParameter("@MS_Value_2", supplier_Pricing.MS_Value_2) : new SqlParameter("@MS_Value_2", DBNull.Value);
            var ms_value_3 = supplier_Pricing.MS_Value_3.HasValue ? new SqlParameter("@MS_Value_3", supplier_Pricing.MS_Value_3) : new SqlParameter("@MS_Value_3", DBNull.Value);
            var ms_value_4 = supplier_Pricing.MS_Value_4.HasValue ? new SqlParameter("@MS_Value_4", supplier_Pricing.MS_Value_4) : new SqlParameter("@MS_Value_4", DBNull.Value);
            var ms_sp_calculation_Type = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Calculation_Type) ? new SqlParameter("@MS_SP_Calculation_Type", supplier_Pricing.MS_SP_Calculation_Type) : new SqlParameter("@MS_SP_Calculation_Type", DBNull.Value);
            var ms_sp_sign = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Sign) ? new SqlParameter("@MS_SP_Sign", supplier_Pricing.MS_SP_Sign) : new SqlParameter("@MS_SP_Sign", DBNull.Value);
            var ms_sp_start_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Date) ? new SqlParameter("@MS_SP_Start_Date", supplier_Pricing.MS_SP_Start_Date) : new SqlParameter("@MS_SP_Start_Date", DBNull.Value);
            var ms_sp_start_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_Start_Time) ? new SqlParameter("@MS_SP_Start_Time", supplier_Pricing.MS_SP_Start_Time) : new SqlParameter("@MS_SP_Start_Time", DBNull.Value);
            var ms_sp_end_date = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Date) ? new SqlParameter("@MS_SP_End_Date", supplier_Pricing.MS_SP_End_Date) : new SqlParameter("@MS_SP_End_Date", DBNull.Value);
            var ms_sp_end_time = !string.IsNullOrEmpty(supplier_Pricing.MS_SP_End_Time) ? new SqlParameter("@MS_SP_End_Time", supplier_Pricing.MS_SP_End_Time) : new SqlParameter("@MS_SP_End_Time", DBNull.Value);
            var ms_sp_value_1 = supplier_Pricing.MS_SP_Value_1.HasValue ? new SqlParameter("@MS_SP_Value_1", supplier_Pricing.MS_SP_Value_1) : new SqlParameter("@MS_SP_Value_1", DBNull.Value);
            var ms_sp_value_2 = supplier_Pricing.MS_SP_Value_2.HasValue ? new SqlParameter("@MS_SP_Value_2", supplier_Pricing.MS_SP_Value_2) : new SqlParameter("@MS_SP_Value_2", DBNull.Value);
            var ms_sp_value_3 = supplier_Pricing.MS_SP_Value_3.HasValue ? new SqlParameter("@MS_SP_Value_3", supplier_Pricing.MS_SP_Value_3) : new SqlParameter("@MS_SP_Value_3", DBNull.Value);
            var ms_sp_value_4 = supplier_Pricing.MS_SP_Value_4.HasValue ? new SqlParameter("@MS_SP_Value_4", supplier_Pricing.MS_SP_Value_4) : new SqlParameter("@MS_SP_Value_4", DBNull.Value);
            var sP_Toggle_Bar = new SqlParameter("@SP_Toggle_Bar", supplier_Pricing.SP_Toggle_Bar);
            var mSP_Toggle_Bar = new SqlParameter("@MS_SP_Toggle_Bar", supplier_Pricing.MS_SP_Toggle_Bar);
            var modified_By = supplier_Pricing.Modified_By > 0 ? new SqlParameter("@Modified_By", supplier_Pricing.Modified_By) : new SqlParameter("@Modified_By", DBNull.Value);
            var C_Length = !string.IsNullOrEmpty(supplier_Pricing.C_Length) ? new SqlParameter("@C_Length", supplier_Pricing.C_Length) : new SqlParameter("@C_Length", DBNull.Value);
            var C_Width = !string.IsNullOrEmpty(supplier_Pricing.C_Width) ? new SqlParameter("@C_Width", supplier_Pricing.C_Width) : new SqlParameter("@C_Width", DBNull.Value);
            var Cost_Disc = !string.IsNullOrEmpty(supplier_Pricing.Cost_Disc) ? new SqlParameter("@Cost_Disc", supplier_Pricing.Cost_Disc) : new SqlParameter("@Cost_Disc", DBNull.Value);
            var Cost_Amount = !string.IsNullOrEmpty(supplier_Pricing.Cost_Amount) ? new SqlParameter("@Cost_Amount", supplier_Pricing.Cost_Amount) : new SqlParameter("@Cost_Amount", DBNull.Value);
            var default_Price = new SqlParameter("@Default_Price", supplier_Pricing.Default_Price ?? false);
            var cost_Price_Flag = new SqlParameter("@Cost_Price_Flag", supplier_Pricing.Cost_Price_Flag ?? false);
            var final_Price_Flag = new SqlParameter("@Final_Price_Flag", supplier_Pricing.Final_Price_Flag ?? false);
            var is_All_Bgm = new SqlParameter("@Is_All_Bgm", supplier_Pricing.Is_All_Bgm ?? false);
            var is_All_Clarity = new SqlParameter("@Is_All_Clarity", supplier_Pricing.Is_All_Clarity ?? false);
            var is_All_Color = new SqlParameter("@Is_All_Color", supplier_Pricing.Is_All_Color ?? false);
            var is_All_Culet = new SqlParameter("@Is_All_Culet", supplier_Pricing.Is_All_Culet ?? false);
            var is_All_Cut = new SqlParameter("@Is_All_Cut", supplier_Pricing.Is_All_Cut ?? false);
            var is_All_Fls_Intensity = new SqlParameter("@Is_All_Fls_Intensity", supplier_Pricing.Is_All_Fls_Intensity ?? false);
            var is_All_Good_Type = new SqlParameter("@Is_All_Good_Type", supplier_Pricing.Is_All_Good_Type ?? false);
            var is_All_Location = new SqlParameter("@Is_All_Location", supplier_Pricing.Is_All_Location ?? false);
            var is_All_Lab = new SqlParameter("@Is_All_Lab", supplier_Pricing.Is_All_Lab ?? false);
            var is_All_Luster = new SqlParameter("@Is_All_Luster", supplier_Pricing.Is_All_Luster ?? false);
            var is_All_Polish = new SqlParameter("@Is_All_Polish", supplier_Pricing.Is_All_Polish ?? false);
            var is_All_Shade = new SqlParameter("@Is_All_Shade", supplier_Pricing.Is_All_Shade ?? false);
            var is_All_Shape = new SqlParameter("@Is_All_Shape", supplier_Pricing.Is_All_Shape ?? false);
            var is_All_Symm = new SqlParameter("@Is_All_Symm", supplier_Pricing.Is_All_Symm ?? false);
            var is_All_Status = new SqlParameter("@Is_All_Status", supplier_Pricing.Is_All_Status ?? false);
            var is_All_Cert_Type = new SqlParameter("@Is_All_Cert_Type", supplier_Pricing.Is_All_Cert_Type ?? false);
            var is_All_Fancy_Color = new SqlParameter("@Is_All_Fancy_Color", supplier_Pricing.Is_All_Fancy_Color ?? false);
            var is_All_Girdle_Open = new SqlParameter("@Is_All_Girdle_Open", supplier_Pricing.Is_All_Girdle_Open ?? false);
            var is_All_Table_Open = new SqlParameter("@Is_All_Table_Open", supplier_Pricing.Is_All_Table_Open ?? false);
            var is_All_Table_Black = new SqlParameter("@Is_All_Table_Black", supplier_Pricing.Is_All_Table_Black ?? false);
            var is_All_Table_White = new SqlParameter("@Is_All_Table_White", supplier_Pricing.Is_All_Table_White ?? false);
            var is_All_Side_Black = new SqlParameter("@Is_All_Side_Black", supplier_Pricing.Is_All_Side_Black ?? false);
            var is_All_Side_white = new SqlParameter("@Is_All_Side_white", supplier_Pricing.Is_All_Side_white ?? false);
            var is_All_Pavilion_Open = new SqlParameter("@Is_All_Pavilion_Open", supplier_Pricing.Is_All_Pavilion_Open ?? false);
            var is_All_Crown_Open = new SqlParameter("@Is_All_Crown_Open", supplier_Pricing.Is_All_Crown_Open ?? false);
            var is_All_Company = new SqlParameter("@Is_All_Company", supplier_Pricing.Is_All_Company ?? false);
            var is_API_FTP_URL = new SqlParameter("@Is_API_FTP_URL", supplier_Pricing.Is_API_FTP_URL ?? false);
            var is_Sequence = supplier_Pricing.Is_Sequence > 0 ? new SqlParameter("@Is_Sequence", supplier_Pricing.Is_Sequence) : new SqlParameter("@Is_Sequence", DBNull.Value);
            var company_Status = new SqlParameter("@Company_Status", supplier_Pricing.Company_Status.HasValue ? (object)supplier_Pricing.Company_Status.Value : DBNull.Value);

            var query_Flag = !string.IsNullOrEmpty(supplier_Pricing.Query_Flag) ? new SqlParameter("@Query_Flag", supplier_Pricing.Query_Flag) : new SqlParameter("@Query_Flag", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Insert_Update @Supplier_Pricing_Id, @Supplier_Id, @Sunrise_Pricing_Id, @Customer_Pricing_Id, @User_Pricing_Id, @Map_Flag,@Stock_Lab,@Stock_Overseas,@Stock_Buyer,@Stock_Saler,@Stock_Defualt, @Shape, @Cts, @Color, @Fancy_Color, @Clarity, @Cut, @Polish, @Symm,
                        @Fls_Intensity, @Lab, @Shade, @Luster, @Bgm, @Culet, @Location, @Status, @Good_Type, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From,
                        @Depth_Per_To, @Table_Per_From, @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Table_Black, @Side_Black, @Table_White, @Side_white, @Cert_Type, @Table_Open, @Crown_Open, @Pavilion_Open,
                        @Girdle_Open, @Base_Disc_From, @Base_Disc_To, @Base_Amount_From, @Base_Amount_To, @Final_Disc_From, @Final_Disc_To, @Final_Amount_From, @Final_Amount_To, @Company, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4,
                        @SP_Calculation_Type, @SP_Sign, @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type,
                        @MS_Sign, @MS_Value_1, @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time,
                        @MS_SP_Value_1, @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4, @SP_Toggle_Bar, @MS_SP_Toggle_Bar, @Modified_By, @C_Length, @C_Width, @Cost_Disc, @Cost_Amount, @Default_Price, 
                        @Cost_Price_Flag, @Final_Price_Flag, @Is_All_Bgm,@Is_All_Clarity,@Is_All_Color,@Is_All_Culet,@Is_All_Cut,@Is_All_Fls_Intensity,@Is_All_Good_Type,@Is_All_Location,@Is_All_Lab,
                        @Is_All_Luster,@Is_All_Polish,@Is_All_Shade,@Is_All_Shape,@Is_All_Symm,@Is_All_Status,@Is_All_Cert_Type,@Is_All_Fancy_Color,@Is_All_Girdle_Open,@Is_All_Table_Open,@Is_All_Table_Black,
                        @Is_All_Table_White,@Is_All_Side_Black,@Is_All_Side_white,@Is_All_Pavilion_Open,@Is_All_Crown_Open,@Is_All_Company,@Is_API_FTP_URL, @Query_Flag, @Is_Sequence, @Company_Status, @Inserted_Id OUT",
                        supplier_Pricing_Id, supplier_Id, sunrise_Pricing_Id, customer_Pricing_Id, user_Pricing_Id, map_Flag, stock_Lab, stock_Overseas, stock_Buyer, stock_Saler, stock_Defualt, shape, cts, color, fancy_Color, clarity, cut, polish, symm, fls_Intensity, lab, shade, luster, bgm, culet, location, status, good_Type, length_From, length_To, width_From,
                        width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
                        pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, table_Black, side_Black, table_White, side_white, cert_Type, table_Open, crown_Open, pavilion_Open, girdle_Open,
                        base_Disc_From, base_Disc_To, base_Amount_From, base_Amount_To, final_Disc_From, final_Disc_To, final_Amount_From, final_Amount_To, company, supplier_Filter_Type, calculation_Type, sign, value_1, value_2, value_3, value_4, sp_calculation_Type, sp_sign, sp_start_date,
                        sp_start_time, sp_end_date, sp_end_time, sp_value_1, sp_value_2, sp_value_3, sp_value_4, ms_calculation_Type, ms_sign, ms_value_1, ms_value_2, ms_value_3, ms_value_4, ms_sp_calculation_Type,
                        ms_sp_sign, ms_sp_start_date, ms_sp_start_time, ms_sp_end_date, ms_sp_end_time, ms_sp_value_1, ms_sp_value_2, ms_sp_value_3, ms_sp_value_4, sP_Toggle_Bar, mSP_Toggle_Bar, modified_By, C_Length, C_Width, Cost_Disc, Cost_Amount, default_Price, cost_Price_Flag, final_Price_Flag,
                        is_All_Bgm, is_All_Clarity, is_All_Color, is_All_Culet, is_All_Cut, is_All_Fls_Intensity, is_All_Good_Type, is_All_Location, is_All_Lab, is_All_Luster, is_All_Polish, is_All_Shade, is_All_Shape,
                        is_All_Symm, is_All_Status, is_All_Cert_Type, is_All_Fancy_Color, is_All_Girdle_Open, is_All_Table_Open, is_All_Table_Black, is_All_Table_White, is_All_Side_Black, is_All_Side_white, is_All_Pavilion_Open,
                        is_All_Crown_Open, is_All_Company, is_API_FTP_URL, query_Flag, is_Sequence, company_Status, inserted_Id));
            int _insertedId = (int)inserted_Id.Value;
            if (_insertedId > 0)
            {
                if (CoreService.Enable_Trace_Records(_configuration))
                {
                    supplier_Pricing.Supplier_Pricing_Id = _insertedId;
                    if (supplier_Pricing.Query_Flag == "I")
                    {
                        await Insert_Supplier_Pricing_Trace(supplier_Pricing, "Insert");
                    }
                    else if (supplier_Pricing.Query_Flag == "U")
                    {
                        await Insert_Supplier_Pricing_Trace(supplier_Pricing, "Update");
                    }
                    else if (supplier_Pricing.Query_Flag == "D")
                    {
                        await Insert_Supplier_Pricing_Trace(supplier_Pricing, "Delete");
                    }
                }
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<int> Delete_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id)
        {
            var _supplier_Pricing_Id = supplier_Pricing_Id > 0 ? new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing_Id) : new SqlParameter("@Supplier_Pricing_Id", DBNull.Value);

            //var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            //var isCheckInStock = new SqlParameter("@IsCheckInStock", SqlDbType.Bit)
            //{
            //    Direction = ParameterDirection.Output
            //};

            //var result = await Task.Run(() => _dbContext.Database.
            //ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Delete @Supplier_Pricing_Id, @Supplier_Id, @IsCheckInStock OUT", _supplier_Pricing_Id, _supplier_Id, isCheckInStock));

            var result = await Task.Run(() => _dbContext.Database.
            ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Delete @Supplier_Pricing_Id", _supplier_Pricing_Id));

            //bool _isCheckInStock = (bool)isCheckInStock.Value;
            //if (_isCheckInStock)
            //{
            //    return ("exist", 574);
            //}
            return result;
        }
        public async Task<int> Delete_Supplier_Pricing_By_Supplier(int supplier_Id)
        {
            var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Database.
            ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Delete_By_Supplier @Supplier_Id", _supplier_Id));

            return result;
        }
        public async Task<Common_Model> Get_Max_Sunrice_Pricing_Id()
        {
            var result = await Task.Run(() => _dbContext.Common_Model
                            .FromSqlRaw(@"exec Get_Max_Sunrice_Id")
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        public async Task<int> Delete_Sunrise_Pricing(int sunrise_Pricing_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Sunrise_Pricing_Delete {sunrise_Pricing_Id}"));
        }
        public async Task<int> Delete_Customer_Pricing(int User_Pricing_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Customer_Pricing_Delete {User_Pricing_Id}"));
        }
        #endregion

        #region Supplier Pricing Key To Symbol
        public async Task<int> Add_Update_Supplier_Pricing_Key_To_Symbol(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Supplier_Pricing_Key_To_Symbol", SqlDbType.Structured)
            {
                TypeName = "dbo.Supplier_Pricing_Key_To_Symbol_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Supplier_Pricing_Key_To_Symbol_Insert_Update @Supplier_Pricing_Key_To_Symbol", parameter);

            return result;
        }
        public async Task<int> Delete_Supplier_Pricing_Key_To_Symbol(int supplier_Pricing_Id, string filter_Type)
        {
            //return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Supplier_Pricing_Key_To_Symbol_Delete {supplier_Pricing_Id}"));
            var supp_Pricing_Id = new SqlParameter("@Supplier_Pricing_Id", supplier_Pricing_Id);
            var _filter_Type = !string.IsNullOrEmpty(filter_Type) ? new SqlParameter("@Filter_Type", filter_Type) : new SqlParameter("@Filter_Type", DBNull.Value);

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Supplier_Pricing_Key_To_Symbol_Delete @Supplier_Pricing_Id, @Filter_Type", supp_Pricing_Id, _filter_Type);

            return result;
        }
        #endregion

        #region Supplier Stock
        public async Task<(string, int)> Stock_Data_Insert_Update(Stock_Data_Master stock_Data_Master)
        {
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Master.Stock_Data_Id);
            var supplier_Id = stock_Data_Master.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Data_Master.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var upload_Method = !string.IsNullOrEmpty(stock_Data_Master.Upload_Method) ? new SqlParameter("@Upload_Method", stock_Data_Master.Upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(stock_Data_Master.Upload_Type) ? new SqlParameter("@Upload_Type", stock_Data_Master.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var upload_From = !string.IsNullOrEmpty(stock_Data_Master.Upload_From) ? new SqlParameter("@Upload_From", stock_Data_Master.Upload_From) : new SqlParameter("@Upload_From", DBNull.Value);
            var stock_Type = !string.IsNullOrEmpty(stock_Data_Master.Stock_Type) ? new SqlParameter("@Stock_Type", stock_Data_Master.Stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Data_Master_Insert_Update @Stock_Data_Id, @Supplier_Id, @Upload_Method, @Upload_Type, @Upload_From, Stock_Type, @Inserted_Id OUT",
                        stock_Data_Id, supplier_Id, upload_Method, upload_Type, upload_From, stock_Type, inserted_Id));

            int _insertedId = (int)inserted_Id.Value;
            if (_insertedId > 0)
            {
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<(string, int)> Stock_Data_Custom_Insert_Update(Stock_Data_Master_Schedular stock_Data_Master)
        {
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Master.Stock_Data_Id);
            var supplier_Id = stock_Data_Master.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Data_Master.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var upload_Method = !string.IsNullOrEmpty(stock_Data_Master.Upload_Method) ? new SqlParameter("@Upload_Method", stock_Data_Master.Upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(stock_Data_Master.Upload_Type) ? new SqlParameter("@Upload_Type", stock_Data_Master.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var upload_From = !string.IsNullOrEmpty(stock_Data_Master.Upload_From) ? new SqlParameter("@Upload_From", stock_Data_Master.Upload_From) : new SqlParameter("@Upload_From", DBNull.Value);
            var stock_Type = !string.IsNullOrEmpty(stock_Data_Master.Stock_Type) ? new SqlParameter("@Stock_Type", stock_Data_Master.Stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Data_Master_Insert_Update @Stock_Data_Id, @Supplier_Id, @Upload_Method, @Upload_Type, @Upload_From, @Stock_Type, @Inserted_Id OUT",
                        stock_Data_Id, supplier_Id, upload_Method, upload_Type, upload_From, stock_Type, inserted_Id));

            int _insertedId = (int)inserted_Id.Value;
            if (_insertedId > 0)
            {
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<int> Stock_Data_Detail_Insert_Update(DataTable dataTable, int Stock_Data_Id)
        {
            var parameter = new SqlParameter("@Stock_data", SqlDbType.Structured)
            {
                TypeName = "dbo.[Stock_Data_Type]",
                Value = dataTable
            };
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", Stock_Data_Id);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC Stock_Data_Details_Insert_Update @Stock_data,@Stock_Data_Id", parameter, stock_Data_Id));

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Stock_Data_Distinct_Column_Values(string column_Name, int supplier_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Data_Distinct_Column_Value_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Column_Name", column_Name));
                    command.Parameters.Add(new SqlParameter("@Supplier_Id", supplier_Id));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (row[col] == DBNull.Value)
                            {
                                dict[col.ColumnName] = "Blank";
                            }
                            else
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        result.Add(dict);
                    }
                }
            }

            return result;
        }
        public async Task<IList<Stock_Data>> Get_Not_Uploaded_Stock_Data(int supplier_Id)
        {
            var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Stock_Data
                            .FromSqlRaw(@"exec Stock_Data_Not_Uploaded_Select @Supplier_Id", _supplier_Id).ToListAsync());

            return result;
        }
        public async Task<int> Supplier_Stock_Insert_Update(int supplier_Id, int stock_Data_Id)
        {
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id);
            var _stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Id);

            var sqlCommand = @"exec [Supplier_Stock_Scheduler_Insert_Update] @Supplier_Id, @Stock_Data_Id";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(_supplier_Id);
                    command.Parameters.Add(_stock_Data_Id);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = Convert.ToInt32(_configuration["CommandTimeout"]);

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }

        public async Task<string> Stock_Data_Supplier_Count_Select(int supplier_Id)
        {
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id);

            var sqlCommand = @"exec [Stock_Data_Supplier_Count_Select] @Supplier_Id";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(_supplier_Id);

                    command.CommandTimeout = Convert.ToInt32(_configuration["CommandTimeout"]);

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        return result;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result.ToString();
        }

        public async Task<int> Supplier_Stock_Manual_File_Insert_Update(int supplier_Id, int stock_Data_Id, bool is_Overwrite, bool Priority)
        {
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id);
            var _stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Id);
            var _is_Overwrite = new SqlParameter("@Is_Overwrite", is_Overwrite);
            var _priority = new SqlParameter("@Priority", Priority);

            var sqlCommand = @"exec [Supplier_Stock_Manual_File_Insert_Update] @Supplier_Id, @Stock_Data_Id, @Is_Overwrite, @Priority";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(_supplier_Id);
                    command.Parameters.Add(_stock_Data_Id);
                    command.Parameters.Add(_is_Overwrite);
                    command.Parameters.Add(_priority);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = Convert.ToInt32(_configuration["CommandTimeout"]);

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }
        public async Task<int> Stock_Data_Shedular_Insert_Update(DataTable dataTable, int Stock_Data_Id)
        {
            var parameter = new SqlParameter("@Stock_data", SqlDbType.Structured)
            {
                TypeName = "dbo.[Stock_Data_Type]",
                Value = dataTable
            };
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", Stock_Data_Id);

            var sqlCommand = @"exec [Stock_Data_Details_Insert_Update] @Stock_data, @Stock_Data_Id";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(stock_Data_Id);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = 1800;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }
        public async Task<DropdownModel> Get_Purchase_Order_Supplier(string supp_Ref_No)
        {
            var _supp_Ref_No = new SqlParameter("@Supplier_Ref_No", supp_Ref_No);

            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"exec Purchase_Order_Supplier_Select @Supplier_Ref_No", _supp_Ref_No)
                            .AsEnumerable()
                            .FirstOrDefault());
            return result;
        }

        public async Task<int> Supplier_Stock_Start_End_Time_Update(Supplier_Stock_Update supplier_Stock_Update)
        {
            var supplier_Id = new SqlParameter("@Supplier_Id", supplier_Stock_Update.Supplier_Id);
            var stock_Data_Id = supplier_Stock_Update.Stock_Data_Id > 0 ? new SqlParameter("@Stock_Data_Id", supplier_Stock_Update.Stock_Data_Id) : new SqlParameter("@Stock_Data_Id", DBNull.Value);
            var upload_Method = !string.IsNullOrEmpty(supplier_Stock_Update.Upload_Method) ? new SqlParameter("@Upload_Method", supplier_Stock_Update.Upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(supplier_Stock_Update.Upload_Type) ? new SqlParameter("@Upload_Type", supplier_Stock_Update.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var start_Time = new SqlParameter("@Start_Time", supplier_Stock_Update.Start_Time ?? (object)DBNull.Value);
            var supplier_Response_Time = new SqlParameter("@Supplier_Response_Time", supplier_Stock_Update.Supplier_Response_Time ?? (object)DBNull.Value);
            var end_Time = new SqlParameter("@End_Time", supplier_Stock_Update.End_Time ?? (object)DBNull.Value);
            var upload_Status = !string.IsNullOrEmpty(supplier_Stock_Update.Upload_Status) ? new SqlParameter("@Upload_Status", supplier_Stock_Update.Upload_Status) : new SqlParameter("@Upload_Status", DBNull.Value);
            var stock_Type = !string.IsNullOrEmpty(supplier_Stock_Update.Stock_Type) ? new SqlParameter("@Stock_Type", supplier_Stock_Update.Stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value);


            var sqlCommand = @"exec [Supplier_Stock_Start_End_Process] @Supplier_Id, @Stock_Data_Id, @Upload_Method, @Upload_Type, @Start_Time, @Supplier_Response_Time, @End_Time, @Upload_Status, @Stock_Type";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(supplier_Id);
                    command.Parameters.Add(stock_Data_Id);
                    command.Parameters.Add(upload_Method);
                    command.Parameters.Add(upload_Type);
                    command.Parameters.Add(start_Time);
                    command.Parameters.Add(supplier_Response_Time);
                    command.Parameters.Add(end_Time);
                    command.Parameters.Add(upload_Status);
                    command.Parameters.Add(stock_Type);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = 1800;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;

        }

        public async Task<List<Dictionary<string, object>>> Get_Stock_Data_By_Rapaport_Increase_Decrease(string rap_increase, string rap_decrease)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Data_By_Rapaport_Increase_Decrease", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@IncreaseType", rap_increase));
                    command.Parameters.Add(new SqlParameter("@DecreaseType", rap_decrease));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (row[col] == DBNull.Value)
                            {
                                dict[col.ColumnName] = "Blank";
                            }
                            else
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        result.Add(dict);
                    }
                }
            }

            return result;
        }

        public async Task<int> Update_Stock_Data_By_Rapaport_Increase_Decrease(string rap_increase, string rap_decrease)
        {
            var sqlCommand = @"exec [Stock_Data_Update_By_Rapaport_Increase_Decrease] @IncreaseType, @DecreaseType";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    var increaseType = new SqlParameter("@IncreaseType", rap_increase);
                    var decreaseType = new SqlParameter("@DecreaseType", rap_decrease);
                    command.Parameters.Add(increaseType);
                    command.Parameters.Add(decreaseType);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = 1800;

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;

        }

        public async Task<int> Supplier_Overseas_Stock_Insert_Update(int supplier_Id, int stock_Data_Id)
        {
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id);
            var _stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Id);

            var sqlCommand = @"exec [Supplier_Overseas_Stock_Scheduler_Insert_Update] @Supplier_Id, @Stock_Data_Id";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(_supplier_Id);
                    command.Parameters.Add(_stock_Data_Id);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = Convert.ToInt32(_configuration["CommandTimeout"]);

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }

        public async Task<int> Supplier_Overseas_Stock_Manual_File_Insert_Update(int supplier_Id, int stock_Data_Id, bool is_Overwrite, bool Priority)
        {
            var _supplier_Id = new SqlParameter("@Supplier_Id", supplier_Id);
            var _stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Id);
            var _is_Overwrite = new SqlParameter("@Is_Overwrite", is_Overwrite);
            var _priority = new SqlParameter("@Priority", Priority);

            var sqlCommand = @"exec [Supplier_Overseas_Stock_Manual_File_Insert_Update] @Supplier_Id, @Stock_Data_Id, @Is_Overwrite, @Priority";

            var result = await Task.Run(async () =>
            {
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sqlCommand;
                    command.Parameters.Add(_supplier_Id);
                    command.Parameters.Add(_stock_Data_Id);
                    command.Parameters.Add(_is_Overwrite);
                    command.Parameters.Add(_priority);

                    // Set the command timeout to 30 minutes (in seconds)
                    command.CommandTimeout = Convert.ToInt32(_configuration["CommandTimeout"]);

                    await _dbContext.Database.OpenConnectionAsync();
                    try
                    {
                        var affectedRows = await command.ExecuteNonQueryAsync();
                        return affectedRows;
                    }
                    finally
                    {
                        _dbContext.Database.CloseConnection();
                    }
                }
            });

            return result;
        }

        #endregion

        #region Stock Number Generation
        public async Task<IList<Stock_Number_Generation>> Get_Stock_Number_Generation(int Id)
        {
            var _Id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Stock_Number_Generation
                            .FromSqlRaw(@"exec Stock_Number_Generation_Select @Id", _Id).ToListAsync());

            return result;
        }

        public async Task<int> Add_Update_Stock_Number_Generation(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Stock_Number_Generation_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Stock_Number_Generation_Table_Type",
                Value = dataTable
            };

            var isExist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Insert_Update @Stock_Number_Generation_Table_Type,@IsExist OUT", parameter, isExist));

            int _isExist = (int)isExist.Value;
            if (_isExist == 1)
                return 5;
            else if (_isExist == 2)
                return 6;
            else if (_isExist == 3)
                return 7;
            return result;
        }
        //public async Task<int> Add_Update_Stock_Number_Generation(Stock_Number_Generation stock_Number_Generation)
        //{
        //    var _Id = stock_Number_Generation.Id > 0 ? new SqlParameter("@Id", stock_Number_Generation.Id) : new SqlParameter("@Id", DBNull.Value);
        //    var _Exc_Party_Id = new SqlParameter("@Party_Id", DBNull.Value);
        //    var _Pointer_Id = !string.IsNullOrEmpty(stock_Number_Generation.Pointer_Id) ? new SqlParameter("@Pointer_Id", stock_Number_Generation.Pointer_Id) : new SqlParameter("@Pointer_Id", DBNull.Value);
        //    var _Shape = !string.IsNullOrEmpty(stock_Number_Generation.Shape) ? new SqlParameter("@Shape", stock_Number_Generation.Shape) : new SqlParameter("@Shape", DBNull.Value);
        //    var _Stock_Type = !string.IsNullOrEmpty(stock_Number_Generation.Stock_Type) ? new SqlParameter("@Stock_Type", stock_Number_Generation.Stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value);
        //    var _Front_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Front_Prefix) ? new SqlParameter("@Front_Prefix", stock_Number_Generation.Front_Prefix) : new SqlParameter("@Front_Prefix", DBNull.Value);
        //    var _Back_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Back_Prefix) ? new SqlParameter("@Back_Prefix", stock_Number_Generation.Back_Prefix) : new SqlParameter("@Back_Prefix", DBNull.Value);
        //    var _Front_Prefix_Alloted = !string.IsNullOrEmpty(stock_Number_Generation.Front_Prefix_Alloted) ? new SqlParameter("@Front_Prefix_Alloted", stock_Number_Generation.Front_Prefix_Alloted) : new SqlParameter("@Front_Prefix_Alloted", DBNull.Value);
        //    var _Start_Format = !string.IsNullOrEmpty(stock_Number_Generation.Start_Format) ? new SqlParameter("@Start_Format", stock_Number_Generation.Start_Format) : new SqlParameter("@Start_Format", DBNull.Value);
        //    var _End_Format = !string.IsNullOrEmpty(stock_Number_Generation.End_Format) ? new SqlParameter("@End_Format", stock_Number_Generation.End_Format) : new SqlParameter("@End_Format", DBNull.Value);
        //    var _Start_Number = stock_Number_Generation.Start_Number > 0 ? new SqlParameter("@Start_Number", stock_Number_Generation.Start_Number) : new SqlParameter("@Start_Number", DBNull.Value);
        //    var _End_Number = stock_Number_Generation.End_Number > 0 ? new SqlParameter("@End_Number", stock_Number_Generation.End_Number) : new SqlParameter("@End_Number", DBNull.Value);
        //    var _Live_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Live_Prefix) ? new SqlParameter("@Live_Prefix", stock_Number_Generation.Live_Prefix) : new SqlParameter("@Live_Prefix", DBNull.Value);
        //    var _Supplier_Id = stock_Number_Generation.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Number_Generation.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
        //    var isExist = new SqlParameter("@IsExist", SqlDbType.Bit)
        //    {
        //        Direction = ParameterDirection.Output
        //    };

        //    var result = await Task.Run(() => _dbContext.Database
        //                .ExecuteSqlRawAsync(@"EXEC Stock_Number_Generation_Insert_Update @Id, @Party_Id, @Pointer_Id, @Shape, @Stock_Type, @Front_Prefix, @Back_Prefix, @Front_Prefix_Alloted, @Start_Format, @End_Format, @Start_Number, @End_Number, @Live_Prefix, @Supplier_Id, @IsExist OUT",
        //                _Id, _Exc_Party_Id, _Pointer_Id, _Shape, _Stock_Type, _Front_Prefix, _Back_Prefix, _Front_Prefix_Alloted, _Start_Format, _End_Format, _Start_Number, _End_Number, _Live_Prefix, _Supplier_Id, isExist));

        //    bool _isExist = (bool)isExist.Value;
        //    if (_isExist)
        //        return 5;

        //    return result;
        //}
        public async Task<int> Delete_Stock_Number_Generation(int Id)
        {
            //return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Stock_Number_Generation_Delete {Id}"));

            var _id = new SqlParameter("@Id", Id);
            var isExist = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Delete @Id, @IsExist OUT", _id, isExist));

            var _isExist = (bool)isExist.Value;
            if (_isExist)
                return 409;


            return result;
        }
        //public async Task<int> Add_Update_Stock_Number_Generation_Raplicate(DataTable dataTable, string ids)
        //{
        //    var parameter = new SqlParameter("@Stock_Number_Generation_Table_Type", SqlDbType.Structured)
        //    {
        //        TypeName = "dbo.Stock_Number_Generation_Table_Type",
        //        Value = dataTable
        //    };
        //    var _ids = new SqlParameter("@Ids", ids);

        //    var result = await Task.Run(() => _dbContext.Database
        //           .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Replicate_Insert_Update @Stock_Number_Generation_Table_Type, @Ids", parameter, _ids));


        //    return result;
        //}
        public async Task<int> Add_Update_Stock_Number_Generation_Raplicate(string ids)
        {
            var _ids = new SqlParameter("@Ids", ids);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Replicate_Insert_Update @Ids", _ids));


            return result;
        }

        public async Task<IList<Stock_Number_Generation_Overseas>> Get_Stock_Number_Generation_Overseas(int Id)
        {
            var _Id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.Stock_Number_Generation_Overseas
                            .FromSqlRaw(@"exec Stock_Number_Generation_Overseas_Select @Id", _Id).ToListAsync());

            return result;
        }
        public async Task<int> Add_Update_Stock_Number_Generation_Overseas(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Stock_Number_Generation_Overseas_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Stock_Number_Generation_Overseas_Table_Type",
                Value = dataTable
            };

            var isExist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Overseas_Insert_Update @Stock_Number_Generation_Overseas_Table_Type,@IsExist OUT", parameter, isExist));

            int _isExist = (int)isExist.Value;
            if (_isExist == 1)
                return -1;

            return result;
        }
        public async Task<int> Delete_Stock_Number_Generation_Overseas(int Id)
        {
            var _id = new SqlParameter("@Id", Id);
            var isExist = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Overseas_Delete @Id, @IsExist OUT", _id, isExist));

            var _isExist = (bool)isExist.Value;
            if (_isExist)
                return 409;

            return result;
        }
        public async Task<int> Add_Update_Stock_Number_Generation_Overseas_Raplicate(string ids)
        {
            var _ids = new SqlParameter("@Ids", ids);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"exec Stock_Number_Generation_Overseas_Replicate_Insert_Update @Ids", _ids));


            return result;
        }
        public async Task<List<Dictionary<string, object>>> Stock_Number_Generation_Replicate_Availability()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Number_Generation_Replicate_Availability", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    var dataTable = ds.Tables[ds.Tables.Count - 1];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            if (row[col] == DBNull.Value)
                            {
                                dict[col.ColumnName] = "Blank";
                            }
                            else
                            {
                                dict[col.ColumnName] = row[col];
                            }
                        }
                        result.Add(dict);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Api/FTP/File Party Name 
        public async Task<IList<DropdownModel>> Get_Api_Ftp_File_Party_Select(int party_Id, bool lab, bool overseas, bool is_Stock_Gen)
        {
            var partyIdParam = party_Id > 0 ? new SqlParameter("@Party_Id", party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var _lab = lab == true ? new SqlParameter("@Lab", lab) : new SqlParameter("@Lab", DBNull.Value);
            var _overseas = overseas == true ? new SqlParameter("@Overseas", overseas) : new SqlParameter("@Overseas", DBNull.Value);
            var _is_Stock_Gen = new SqlParameter("@Is_Stock_Gen", is_Stock_Gen);

            var result = await Task.Run(() => _dbContext.DropdownModel
                .FromSqlRaw("EXEC Party_Name_From_Api_FTP_File_Select @Party_Id, @Lab, @Overseas, @Is_Stock_Gen", partyIdParam, _lab, _overseas, _is_Stock_Gen)
                .ToListAsync());

            return result;
        }
        #endregion

        #region Supplier Stock Error Log
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Last_Entry, string stock_Type, string supplierNo_CertNo)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Stock_Error_Log_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(supplier_Ids) ? new SqlParameter("@Supplier_Ids", supplier_Ids) : new SqlParameter("@Supplier_Ids", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(upload_Type) ? new SqlParameter("@Upload_Type", upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(from_Date) ? new SqlParameter("@From_Date", from_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(from_Time) ? new SqlParameter("@From_Time", from_Time) : new SqlParameter("@From_Time", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(to_Date) ? new SqlParameter("@To_Date", to_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(to_Time) ? new SqlParameter("@To_Time", to_Time) : new SqlParameter("@To_Time", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Type) ? new SqlParameter("@Stock_Type", stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Is_Last_Entry", is_Last_Entry));
                    command.Parameters.Add(!string.IsNullOrEmpty(supplierNo_CertNo) ? new SqlParameter("@SupplierNo_CertNo", supplierNo_CertNo) : new SqlParameter("@SupplierNo_CertNo", DBNull.Value));
                    command.CommandTimeout = 3600;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log_Detail(string supplier_Ids, string stock_Data_Ids, string upload_Type, string supplierNo_CertNo, string stock_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Stock_Error_Log_Detail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(supplier_Ids) ? new SqlParameter("@Supplier_Ids", supplier_Ids) : new SqlParameter("@Supplier_Ids", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Data_Ids) ? new SqlParameter("@Stock_Data_Ids", stock_Data_Ids) : new SqlParameter("@Stock_Data_Ids", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(upload_Type) ? new SqlParameter("@Upload_Type", upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(supplierNo_CertNo) ? new SqlParameter("@SupplierNo_CertNo", supplierNo_CertNo) : new SqlParameter("@SupplierNo_CertNo", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Type) ? new SqlParameter("@Stock_Type", stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_File_Error_Log(int supplier_Id, int stock_Data_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_File_Upload_Error_Log_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(stock_Data_Id > 0 ? new SqlParameter("@Stock_Data_Id", stock_Data_Id) : new SqlParameter("@Stock_Data_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<DataTable> Get_Supplier_Stock_File_Error_Log_Detail(int supplier_Id, string upload_Type)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_File_Upload_Error_Log_Detail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supp_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(upload_Type) ? new SqlParameter("@Upload_Type", upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }

            return dataTable;
        }
        public async Task<List<Dictionary<string, object>>> Get_Data_Transfer_Log(string from_Date, string to_Date)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Data_Transfer_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(from_Date) ? new SqlParameter("@From_Date", from_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(to_Date) ? new SqlParameter("@To_Date", to_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.CommandTimeout = 3600;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Report
        public async Task<(string, int)> Create_Update_Report_Master(Report_Master report_Master)
        {
            var id = new SqlParameter("@Id", report_Master.Id);
            var rpt_Name = !string.IsNullOrEmpty(report_Master.Name) ? new SqlParameter("@Rpt_Name", report_Master.Name) : new SqlParameter("@Rpt_Name", DBNull.Value);
            var rpt_Sp_Name = !string.IsNullOrEmpty(report_Master.Sp_Name) ? new SqlParameter("@Rpt_Sp_Name", report_Master.Sp_Name) : new SqlParameter("@Rpt_Sp_Name", DBNull.Value);
            var status = report_Master.Status != null ? new SqlParameter("@Status", report_Master.Status) : new SqlParameter("@Status", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var is_Exist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Report_Master_Insert_Update @Id, @Rpt_Name, @Rpt_Sp_Name, @Status, @Inserted_Id OUT,@IsExist OUT",
                        id, rpt_Name, rpt_Sp_Name, status, inserted_Id, is_Exist));

            int _insertedId = (int)inserted_Id.Value;
            if (_insertedId > 0)
            {
                return ("success", _insertedId);
            }
            if ((int)is_Exist.Value == 1)
            {
                return ("exist", 0);
            }
            return ("error", 0);
        }
        public async Task<int> Create_Update_Report_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblReport_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Report_Detail_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Report_Detail_Insert_Update @tblReport_Detail", parameter);

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Name(int id, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Name_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Detail(int id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Detail_Filter_Parameter(int id, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Detail_Filter_Parameter_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Create_Update_Report_User_Role(DataTable dataTable, string? user_Type)
        {
            var parameter = new SqlParameter("@Report_Users_Role_Table_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Report_Users_Role_Table_Type",
                Value = dataTable
            };

            var _user_Type = !string.IsNullOrEmpty(user_Type) ? new SqlParameter("@User_Type", user_Type) : new SqlParameter("@User_Type", DBNull.Value);

            var result = await _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Report_Users_Role_Insert_Update @Report_Users_Role_Table_Type,@User_Type", parameter, _user_Type);

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Users_Role(int id, int user_Id, string user_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Users_Role_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(user_Type) ? new SqlParameter("@User_Type", user_Type) : new SqlParameter("@User_Type", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Users_Role_Format_Type(int id, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Users_Role_Format_Type_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Rm_Id", id) : new SqlParameter("@Rm_Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Delete_Report_User_Role(int id, int user_Id, string format_Type)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Report_Users_Role_Delete {id},{user_Id}, {format_Type}"));
        }

        public async Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, DataTable)> Get_Report_Search(int id, IList<Report_Filter_Parameter> report_Filter_Parameters, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, string is_Selected_Supp_Stock_Id, string act_Mod_Id)
        {
            DataTable dataTable = new DataTable();
            var result = new List<Dictionary<string, object>>();
            var totalRecordr = string.Empty;
            var totalCtsr = string.Empty;
            var totalAmtr = string.Empty;
            var totalDiscr = string.Empty;
            var totalBaseAmtr = string.Empty;
            var totalBaseDiscr = string.Empty;
            var totalOfferAmtr = string.Empty;
            var totalOfferDiscr = string.Empty;

            var _id = new SqlParameter("@Id", id);
            var result_Master = await Task.Run(() => _dbContext.Bank_Dropdown_Model
            .FromSqlRaw(@"exec Report_Master_Select @Id", _id).AsEnumerable().FirstOrDefault());

            if (result_Master != null)
            {
                string report_Sp = result_Master.Name;

                using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
                {
                    using (var command = new SqlCommand(report_Sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var item in report_Filter_Parameters.Where(x => !string.IsNullOrEmpty(x.Category_Value)).ToList())
                        {
                            command.Parameters.Add(!string.IsNullOrEmpty(item.Category_Value) ? new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), item.Category_Value) : new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), DBNull.Value));
                        }

                        if (iSort.Count() > 0)
                        {
                            string iSorting = string.Empty;

                            foreach (var item in iSort)
                            {
                                iSorting += "[" + item.col_name + "] " + item.sort + " ";

                                if (item != iSort.Last())
                                {
                                    iSorting += ",";
                                }
                            }
                            command.Parameters.Add(!string.IsNullOrEmpty(iSorting) ? new SqlParameter("@iSort", iSorting) : new SqlParameter("@iSort", DBNull.Value));
                        }
                        var totalRecordParameter = new SqlParameter("@iTotalRec", SqlDbType.Int);
                        totalRecordParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(totalRecordParameter);

                        var totalCtsParameter = new SqlParameter("@iTotalCts", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalCtsParameter);

                        var totalAmtParameter = new SqlParameter("@iTotalAmt", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalAmtParameter);

                        var totalDiscParameter = new SqlParameter("@iTotalDisc", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalDiscParameter);

                        var totalBaseAmtParameter = new SqlParameter("@iTotalBaseAmt", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalBaseAmtParameter);

                        var totalBaseDiscParameter = new SqlParameter("@iTotalBaseDisc", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalBaseDiscParameter);

                        var totalOfferAmtParameter = new SqlParameter("@iTotalOfferAmt", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalOfferAmtParameter);

                        var totalOfferDiscParameter = new SqlParameter("@iTotalOfferDisc", SqlDbType.NVarChar)
                        {
                            Size = -1, // -1 is used for max size
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalOfferDiscParameter);

                        command.Parameters.Add(iPgNo > 0 ? new SqlParameter("@iPgNo", iPgNo) : new SqlParameter("@iPgNo", DBNull.Value));
                        command.Parameters.Add(iPgSize > 0 ? new SqlParameter("@iPgSize", iPgSize) : new SqlParameter("@iPgSize", DBNull.Value));
                        command.Parameters.Add(!string.IsNullOrEmpty(is_Selected_Supp_Stock_Id) ? new SqlParameter("@Is_Selected_Supp_Stock_Id", is_Selected_Supp_Stock_Id) : new SqlParameter("@Is_Selected_Supp_Stock_Id", DBNull.Value));
                        if (!string.IsNullOrEmpty(act_Mod_Id))
                        {
                            command.Parameters.Add(!string.IsNullOrEmpty(act_Mod_Id) ? new SqlParameter("@Act_Mod_Id", act_Mod_Id) : new SqlParameter("@Act_Mod_Id", DBNull.Value));
                        }
                        command.CommandTimeout = 1800;
                        await connection.OpenAsync();

                        using var da = new SqlDataAdapter();
                        da.SelectCommand = command;

                        using var ds = new DataSet();
                        da.Fill(ds);

                        dataTable = ds.Tables[ds.Tables.Count - 1];

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dict = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var columnValue = reader.GetValue(i);

                                    dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                                }

                                result.Add(dict);
                            }
                        }

                        totalRecordr = Convert.ToString(totalRecordParameter.Value);
                        totalCtsr = Convert.ToString(totalCtsParameter.Value);
                        totalAmtr = Convert.ToString(totalAmtParameter.Value);
                        totalDiscr = Convert.ToString(totalDiscParameter.Value);
                        totalBaseAmtr = Convert.ToString(totalBaseAmtParameter.Value);
                        totalBaseDiscr = Convert.ToString(totalBaseDiscParameter.Value);
                        totalOfferAmtr = Convert.ToString(totalOfferAmtParameter.Value);
                        totalOfferDiscr = Convert.ToString(totalOfferDiscParameter.Value);
                    }
                }
            }
            return (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseAmtr, totalBaseDiscr, totalOfferAmtr, totalOfferDiscr, dataTable);
        }
        public async Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, string, string)> Get_Lab_Search_Report_Search(DataTable dataTable, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, int? user_Id, string is_Selected_Supp_Stock_Id, string user_Format)
        {
            var result = new List<Dictionary<string, object>>();
            var totalRecordr = string.Empty;
            var totalCtsr = string.Empty;
            var totalAmtr = string.Empty;
            var totalDiscr = string.Empty;
            string totalBaseDiscr = string.Empty;
            string totalBaseAmtr = string.Empty;
            string totalOfferDiscr = string.Empty;
            string totalOfferAmtr = string.Empty;
            string totalMaxSlabDiscr = string.Empty;
            string totalMaxSlabAmtr = string.Empty;

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Multiple_Search_Lab_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@Report_Search_Lab_Table_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Report_Search_Lab_Table_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(iPgNo > 0 ? new SqlParameter("@iPgNo", iPgNo) : new SqlParameter("@iPgNo", DBNull.Value));
                    command.Parameters.Add(iPgSize > 0 ? new SqlParameter("@iPgSize", iPgSize) : new SqlParameter("@iPgSize", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(user_Format) ? new SqlParameter("@User_Format", user_Format) : new SqlParameter("@User_Format", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(is_Selected_Supp_Stock_Id) ? new SqlParameter("@Is_Selected_Supp_Stock_Id", is_Selected_Supp_Stock_Id) : new SqlParameter("@Is_Selected_Supp_Stock_Id", DBNull.Value));

                    if (iSort.Count() > 0)
                    {
                        string iSorting = string.Empty;

                        foreach (var item in iSort)
                        {
                            iSorting += "[" + item.col_name + "] " + item.sort + " ";

                            if (item != iSort.Last())
                            {
                                iSorting += ",";
                            }
                        }
                        command.Parameters.Add(!string.IsNullOrEmpty(iSorting) ? new SqlParameter("@iSort", iSorting) : new SqlParameter("@iSort", DBNull.Value));
                    }
                    var totalRecordParameter = new SqlParameter("@iTotalRec", SqlDbType.Int);
                    totalRecordParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(totalRecordParameter);

                    var totalCtsParameter = new SqlParameter("@iTotalCts", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalCtsParameter);

                    var totalAmtParameter = new SqlParameter("@iTotalAmt", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalAmtParameter);

                    var totalDiscParameter = new SqlParameter("@iTotalDisc", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalDiscParameter);

                    var totalBaseDiscParameter = new SqlParameter("@iTotalBaseDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalBaseDiscParameter);

                    var totalBaseAmtParameter = new SqlParameter("@iTotalBaseAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalBaseAmtParameter);

                    var totalOfferDiscParameter = new SqlParameter("@iTotalOfferDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalOfferDiscParameter);

                    var totalOfferAmtParameter = new SqlParameter("@iTotalOfferAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalOfferAmtParameter);

                    var totalMaxSlabDiscParameter = new SqlParameter("@iTotalMaxSlabDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalMaxSlabDiscParameter);

                    var totalMaxSlabAmtParameter = new SqlParameter("@iTotalMaxSlabAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalMaxSlabAmtParameter);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }
                            result.Add(dict);
                        }
                    }
                    totalRecordr = Convert.ToString(totalRecordParameter.Value);
                    totalCtsr = Convert.ToString(totalCtsParameter.Value);
                    totalAmtr = Convert.ToString(totalAmtParameter.Value);
                    totalDiscr = Convert.ToString(totalDiscParameter.Value);
                    totalBaseDiscr = Convert.ToString(totalBaseDiscParameter.Value);
                    totalBaseAmtr = Convert.ToString(totalBaseAmtParameter.Value);
                    totalOfferDiscr = Convert.ToString(totalOfferDiscParameter.Value);
                    totalOfferAmtr = Convert.ToString(totalOfferAmtParameter.Value);
                    totalMaxSlabDiscr = Convert.ToString(totalMaxSlabDiscParameter.Value);
                    totalMaxSlabAmtr = Convert.ToString(totalMaxSlabAmtParameter.Value);
                }
            }
            return (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseDiscr, totalBaseAmtr, totalOfferDiscr, totalOfferAmtr, totalMaxSlabDiscr, totalMaxSlabAmtr);
        }
        public async Task<List<Dictionary<string, object>>> Get_Lab_Search_Distinct_Report_Search(DataTable dataTable)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Multiple_Search_Lab_Distinct_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@Report_Search_Lab_Table_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Report_Search_Lab_Table_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);
                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }
                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<(List<Dictionary<string, object>>, string, string, string, string, string, string, string, string, string, string, string)> Get_Lab_Search_Report_Search_Total(DataTable dataTable, int iPgNo, int iPgSize, IList<Report_Sorting> iSort)
        {
            var result = new List<Dictionary<string, object>>();
            var totalRecordr = string.Empty;
            var totalCtsr = string.Empty;
            var totalAmtr = string.Empty;
            var totalDiscr = string.Empty;
            string totalBaseDiscr = string.Empty;
            string totalBaseAmtr = string.Empty;
            string totalOfferDiscr = string.Empty;
            string totalOfferAmtr = string.Empty;
            string totalMaxSlabDiscr = string.Empty;
            string totalMaxSlabAmtr = string.Empty;
            string totalRaP_Amount = string.Empty;

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Multiple_Search_Lab_Total_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@Report_Search_Lab_Table_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Report_Search_Lab_Table_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(iPgNo > 0 ? new SqlParameter("@iPgNo", iPgNo) : new SqlParameter("@iPgNo", DBNull.Value));
                    command.Parameters.Add(iPgSize > 0 ? new SqlParameter("@iPgSize", iPgSize) : new SqlParameter("@iPgSize", DBNull.Value));

                    if (iSort.Count() > 0)
                    {
                        string iSorting = string.Empty;

                        foreach (var item in iSort)
                        {
                            iSorting += "[" + item.col_name + "] " + item.sort + " ";

                            if (item != iSort.Last())
                            {
                                iSorting += ",";
                            }
                        }
                        command.Parameters.Add(!string.IsNullOrEmpty(iSorting) ? new SqlParameter("@iSort", iSorting) : new SqlParameter("@iSort", DBNull.Value));
                    }
                    var totalRecordParameter = new SqlParameter("@iTotalRec", SqlDbType.Int);
                    totalRecordParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(totalRecordParameter);

                    var totalCtsParameter = new SqlParameter("@iTotalCts", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalCtsParameter);

                    var totalAmtParameter = new SqlParameter("@iTotalAmt", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalAmtParameter);

                    var totalDiscParameter = new SqlParameter("@iTotalDisc", SqlDbType.NVarChar)
                    {
                        Size = -1, // -1 is used for max size
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalDiscParameter);

                    var totalBaseDiscParameter = new SqlParameter("@iTotalBaseDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalBaseDiscParameter);

                    var totalBaseAmtParameter = new SqlParameter("@iTotalBaseAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalBaseAmtParameter);

                    var totalOfferDiscParameter = new SqlParameter("@iTotalOfferDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalOfferDiscParameter);

                    var totalOfferAmtParameter = new SqlParameter("@iTotalOfferAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalOfferAmtParameter);

                    var totalMaxSlabDiscParameter = new SqlParameter("@iTotalMaxSlabDisc", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalMaxSlabDiscParameter);

                    var totalMaxSlabAmtParameter = new SqlParameter("@iTotalMaxSlabAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalMaxSlabAmtParameter);
                    var totalRap_Amount = new SqlParameter("@iTotalRapAmt", SqlDbType.NVarChar)
                    {
                        Size = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRap_Amount);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }
                            result.Add(dict);
                        }
                    }
                    totalRecordr = Convert.ToString(totalRecordParameter.Value);
                    totalCtsr = Convert.ToString(totalCtsParameter.Value);
                    totalAmtr = Convert.ToString(totalAmtParameter.Value);
                    totalDiscr = Convert.ToString(totalDiscParameter.Value);
                    totalBaseDiscr = Convert.ToString(totalBaseDiscParameter.Value);
                    totalBaseAmtr = Convert.ToString(totalBaseAmtParameter.Value);
                    totalOfferDiscr = Convert.ToString(totalOfferDiscParameter.Value);
                    totalOfferAmtr = Convert.ToString(totalOfferAmtParameter.Value);
                    totalMaxSlabDiscr = Convert.ToString(totalMaxSlabDiscParameter.Value);
                    totalMaxSlabAmtr = Convert.ToString(totalMaxSlabAmtParameter.Value);
                    totalRaP_Amount = Convert.ToString(totalRap_Amount.Value);
                }
            }
            return (result, totalRecordr, totalCtsr, totalAmtr, totalDiscr, totalBaseDiscr, totalBaseAmtr, totalOfferDiscr, totalOfferAmtr, totalMaxSlabDiscr, totalMaxSlabAmtr, totalRaP_Amount);
        }
        public async Task<List<Dictionary<string, object>>> Get_Stock_Avalibility_Report_Search(DataTable dataTable, string stock_Id, string stock_Type, string supp_Stock_Id, int iPgNo, int iPgSize, IList<Report_Sorting> iSort, int party_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Availability_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (iSort.Count() > 0)
                    {
                        string iSorting = string.Empty;

                        foreach (var item in iSort)
                        {
                            iSorting += "[" + item.col_name + "] " + item.sort + " ";

                            if (item != iSort.Last())
                            {
                                iSorting += ",";
                            }
                        }
                        command.Parameters.Add(!string.IsNullOrEmpty(iSorting) ? new SqlParameter("@iSort", iSorting) : new SqlParameter("@iSort", DBNull.Value));
                    }

                    var parameter = new SqlParameter("@Stock_Availibity_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Stock_Availibity_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);

                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Type) ? new SqlParameter("@STOCK_TYPE", stock_Type) : new SqlParameter("@STOCK_TYPE", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(supp_Stock_Id) ? new SqlParameter("@SUPP_STOCK_ID", supp_Stock_Id) : new SqlParameter("@SUPP_STOCK_ID", DBNull.Value));
                    command.Parameters.Add(iPgNo > 0 ? new SqlParameter("@iPgNo", iPgNo) : new SqlParameter("@iPgNo", DBNull.Value));
                    command.Parameters.Add(iPgSize > 0 ? new SqlParameter("@iPgSize", iPgSize) : new SqlParameter("@iPgSize", DBNull.Value));
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@PARTY_ID", party_Id) : new SqlParameter("@PARTY_ID", DBNull.Value));
                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }

                }
            }

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Report_Column_Format(int user_Id, int report_Id, string format_Type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Column_Format_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(report_Id > 0 ? new SqlParameter("@Report_Id", report_Id) : new SqlParameter("@Report_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(format_Type) ? new SqlParameter("@Format_Type", format_Type) : new SqlParameter("@Format_Type", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<string> Create_Update_Report_Search(Report_Search_Save report_Search_Save, int user_Id)
        {
            var id = report_Search_Save.Id > 0 ? new SqlParameter("@Id", report_Search_Save.Id) : new SqlParameter("@Id", DBNull.Value);
            var name = new SqlParameter("@Name", report_Search_Save.Name);
            var userId = new SqlParameter("@User_Id", user_Id);
            var search_Value = new SqlParameter("@Search_Value", report_Search_Save.Search_Value.ToString());
            var search_Display = new SqlParameter("@Search_Display", report_Search_Save.Search_Display);
            var is_Exist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Report_Search_Save_Insert_Update @Id, @Name, @Search_Value, @Search_Display,@User_Id, @IsExist OUT", id, name, search_Value, search_Display, userId, is_Exist));
            if ((int)is_Exist.Value == 1)
            {
                return "exist";
            }
            return "success";
        }
        public async Task<List<Dictionary<string, object>>> Get_Search_Save_Report_Search(int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Report_Search_Save_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                if (columnName == "Search_Value" && columnValue != null)
                                {
                                    Report_Lab_Filter report_Lab_Filter = JsonConvert.DeserializeObject<Report_Lab_Filter>(columnValue.ToString());
                                    dict[columnName] = report_Lab_Filter;
                                }
                                else if (columnName == "Search_Display" && columnValue != null)
                                {
                                    IList<Report_Filter_Display> report_Filter_Display = JsonConvert.DeserializeObject<List<Report_Filter_Display>>(columnValue.ToString());
                                    dict[columnName] = report_Filter_Display;
                                }
                                else
                                {
                                    dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                                }

                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Delete_Report_Search(int id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Report_Search_Save_Delete {id}"));
        }
        public async Task<(string, int)> Create_Update_Report_Layout_Save(Report_Layout_Save report_Layout_Save)
        {
            var id = new SqlParameter("@Id", report_Layout_Save.Id);
            var user_Id = new SqlParameter("@User_Id", report_Layout_Save.User_Id);
            var rm_Id = new SqlParameter("@Rm_Id", report_Layout_Save.Rm_Id);
            var name = !string.IsNullOrEmpty(report_Layout_Save.Name) ? new SqlParameter("@Name", report_Layout_Save.Name) : new SqlParameter("@Name", DBNull.Value);
            var status = new SqlParameter("@Status", report_Layout_Save.Status);
            var insertedId = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var is_Exist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Report_Layout_Save_Insert_Update @Id, @User_Id,@Rm_Id, @Name, @Status, @Inserted_Id OUT, @IsExist OUT",
                id, user_Id, rm_Id, name, status, insertedId, is_Exist));

            if ((int)is_Exist.Value == 1)
            {
                return ("exist", 0);
            }
            else
            {
                var _inserted_Id = (int)insertedId.Value;
                return ("success", _inserted_Id);
            }
        }
        public async Task<int> Insert_Update_Report_Layout_Save_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblReport_Layout_Save_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Report_Layout_Save_Detail_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Report_Layout_Save_Detail_Insert_Update @tblReport_Layout_Save_Detail", parameter);

            return result;
        }
        public async Task<IList<Report_Layout_Save>> Get_Report_Layout_Save(int User_Id, int Rm_Id)
        {
            var user_Id = User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var rm_Id = Rm_Id > 0 ? new SqlParameter("@Rm_Id", Rm_Id) : new SqlParameter("@Rm_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Report_Layout_Save
                            .FromSqlRaw(@"EXEC Report_Layout_Save_Select @User_Id,@Rm_Id", user_Id, rm_Id)
                            .ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    var report_layout_Id = item.Id > 0 ? new SqlParameter("@Report_Layout_Id", item.Id) : new SqlParameter("@Report_Layout_Id", DBNull.Value);

                    item.Report_Layout_Save_Detail_List = await Task.Run(() => _dbContext.Report_Layout_Save_Detail
                            .FromSqlRaw(@"EXEC Report_Layout_Save_Detail_Select @Report_Layout_Id", report_layout_Id)
                            .ToListAsync());
                }
            }
            return result;
        }
        public async Task<int> Update_Report_Layout_Save_Status(int id, int user_Id, int rm_Id)
        {
            var _id = id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value);
            var _user_Id = user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _rm_Id = rm_Id > 0 ? new SqlParameter("@Rm_Id", rm_Id) : new SqlParameter("@Rm_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Report_Layout_Save_Status_Update @Id, @User_Id, @Rm_Id",
                _id, _user_Id, _rm_Id));

            return result;
        }
        public async Task<int> Delete_Report_Layout_Save(int id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Report_Layout_Save_Delete {id}"));
        }
        public async Task<DataTable> Get_Report_Search_Excel(int id, IList<Report_Filter_Parameter> report_Filter_Parameters)
        {
            DataTable dataTable = new DataTable();

            var _id = new SqlParameter("@Id", id);
            var result_Master = await Task.Run(() => _dbContext.Bank_Dropdown_Model
            .FromSqlRaw(@"exec Report_Master_Select @Id", _id).AsEnumerable().FirstOrDefault());

            if (result_Master != null)
            {
                string report_Sp = result_Master.Name + "_Excel";

                using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
                {
                    using (var command = new SqlCommand(report_Sp, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var item in report_Filter_Parameters.Where(x => !string.IsNullOrEmpty(x.Category_Value)).ToList())
                        {
                            command.Parameters.Add(!string.IsNullOrEmpty(item.Category_Value) ? new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), item.Category_Value) : new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), DBNull.Value));
                        }
                        command.CommandTimeout = 1800;
                        await connection.OpenAsync();

                        using var da = new SqlDataAdapter();
                        da.SelectCommand = command;

                        using var ds = new DataSet();
                        da.Fill(ds);

                        dataTable = ds.Tables[ds.Tables.Count - 1];
                    }
                }
            }
            return dataTable;
        }
        public async Task<DataTable> Get_Stock_Availability_Report_Excel(DataTable dataTable, string stock_Id, string stock_Type, int party_Id, string excel_Format)
        {
            DataTable dataTable1 = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Stock_Availability_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter = new SqlParameter("@Stock_Availibity_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Stock_Availibity_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Id) ? new SqlParameter("@STOCK_ID", stock_Id) : new SqlParameter("@STOCK_ID", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Type) ? new SqlParameter("@STOCK_TYPE", stock_Type) : new SqlParameter("@STOCK_TYPE", DBNull.Value));
                    command.Parameters.Add(party_Id > 0 ? new SqlParameter("@PARTY_ID", party_Id) : new SqlParameter("@PARTY_ID", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(excel_Format) ? new SqlParameter("@Excel_Format", excel_Format) : new SqlParameter("@Excel_Format", DBNull.Value));
                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable1 = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable1;
        }
        public async Task<IList<Report_Image_Video_Certificate>> Download_Image_Video_Certificate_Stock(string? Ids, string? document_Type)
        {
            var ids = !string.IsNullOrEmpty(Ids) ? new SqlParameter("@Ids", Ids) : new SqlParameter("@Ids", DBNull.Value);
            var documentType = !string.IsNullOrEmpty(document_Type) ? new SqlParameter("@Document_Type", document_Type) : new SqlParameter("@Document_Type", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Report_Image_Video_Certificate
                            .FromSqlRaw(@"EXEC Get_Report_Image_Video_Certificate @Ids, @Document_Type", ids, documentType)
                            .ToListAsync());
            return result;
        }
        #endregion

        #region GIA Lap Parameter
        //public async Task<int> Insert_GIA_Lab_Parameter(DataTable dataTable)
        //{
        //    var parameter = new SqlParameter("@tblGIA_Lab_Parameter", SqlDbType.Structured)
        //    {
        //        TypeName = "dbo.[GIA_Lab_Parameter_Table_Type]",
        //        Value = dataTable
        //    };

        //    var result = await Task.Run(() => _dbContext.Database
        //    .ExecuteSqlRawAsync(@"EXEC GIA_Lab_Parameter_Insert_Update @tblGIA_Lab_Parameter", parameter));

        //    return result;
        //}

        public async Task<int> Insert_GIA_Certificate_Data(DataTable dataTable, int supplier_Id, string customer_Name)
        {
            var parameter = new SqlParameter("@tblGIACertificate", SqlDbType.Structured)
            {
                TypeName = "dbo.[GIA_Certificate_Data_Table_Type]",
                Value = dataTable
            };

            var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var _customer_Name = !string.IsNullOrEmpty(customer_Name) ? new SqlParameter("@Customer_Name", customer_Name) : new SqlParameter("@Customer_Name", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC GIA_Certificate_Data_Insert_Update @tblGIACertificate, @Supplier_Id, @Customer_Name", parameter, _supplier_Id, _customer_Name));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_GIA_Certificate_Data(string report_Date)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("GIA_Certificate_Data_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Date) ? new SqlParameter("@Certificate_Date", report_Date) : new SqlParameter("@Certificate_Date", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_GIA_Certificate_Update_Data(DataTable dataTable, string supplier_Name, string customer_Name)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("GIA_Certificate_Data_Table_Values_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@tblGIACertificate", SqlDbType.Structured)
                    {
                        TypeName = "dbo.GIA_Certificate_Data_Table_Type",
                        Value = dataTable
                    };
                    command.Parameters.Add(parameter);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            dict["Supplier_Name"] = supplier_Name;
                            dict["Customer_Name"] = customer_Name;

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> GIA_Certificate_Placed_Order(DataTable dataTable, int supplier_Id, string customer_Name)
        {
            var parameter = new SqlParameter("@tblGIA_Certi_Placed_Order", SqlDbType.Structured)
            {
                TypeName = "dbo.[GIA_Certificate_Placed_Order_Table_Type]",
                Value = dataTable
            };

            var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var _customer_Name = !string.IsNullOrEmpty(customer_Name) ? new SqlParameter("@Customer_Name", customer_Name) : new SqlParameter("@Customer_Name", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"EXEC GIA_Certificate_Placed_Order @tblGIA_Certi_Placed_Order, @Supplier_Id, @Customer_Name", parameter, _supplier_Id, _customer_Name));

            return result;
        }

        //public async Task<List<Dictionary<string, object>>> GIA_Lab_Parameter(string report_Date)
        //{
        //    var result = new List<Dictionary<string, object>>();
        //    using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
        //    {
        //        using (var command = new SqlCommand("GIA_Lab_Parameter_Select", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.Add(!string.IsNullOrEmpty(report_Date) ? new SqlParameter("@Report_Date", report_Date) : new SqlParameter("@Report_Date", DBNull.Value));
        //            await connection.OpenAsync();

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var dict = new Dictionary<string, object>();

        //                    for (int i = 0; i < reader.FieldCount; i++)
        //                    {
        //                        var columnName = reader.GetName(i);
        //                        var columnValue = reader.GetValue(i);

        //                        dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
        //                    }

        //                    result.Add(dict);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region Get Excel Formet Stock Result
        public async Task<DataTable> Get_Stock_In_Datatable(string supp_ref_no, string excel_Format, int user_Id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Excel_Format_Stock_Search_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(supp_ref_no) ? new SqlParameter("@Supplier_Ref_No", supp_ref_no) : new SqlParameter("@Supplier_Ref_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(excel_Format) ? new SqlParameter("@Excel_Format", excel_Format) : new SqlParameter("@Excel_Format", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }

            return dataTable;
        }
        public async Task<DataTable> Get_Excel_Report_Search_New(IList<Report_Filter_Parameter> report_Filter_Parameters, string excel_Format, string supplier_Ref_No, int user_Id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Excel_Format_Stock_Search_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var item in report_Filter_Parameters.Where(x => !string.IsNullOrEmpty(x.Category_Value)).ToList())
                    {
                        command.Parameters.Add(!string.IsNullOrEmpty(item.Category_Value) ? new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), item.Category_Value) : new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), DBNull.Value));
                    }
                    command.Parameters.Add(!string.IsNullOrEmpty(excel_Format) ? new SqlParameter("@Excel_Format", excel_Format) : new SqlParameter("@Excel_Format", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(supplier_Ref_No) ? new SqlParameter("@Supplier_Ref_No", supplier_Ref_No) : new SqlParameter("@Supplier_Ref_No", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }
        public async Task<DataTable> Get_Excel_Report_Search(DataTable dt_Search, string excel_Format, string supplier_Ref_No, int user_Id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Excel_Format_Stock_Search_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@Report_Search_Lab_Table_Type", SqlDbType.Structured)
                    {
                        TypeName = "dbo.Report_Search_Lab_Table_Type",
                        Value = dt_Search
                    };
                    command.Parameters.Add(parameter);
                    command.Parameters.Add(!string.IsNullOrEmpty(excel_Format) ? new SqlParameter("@Excel_Format", excel_Format) : new SqlParameter("@Excel_Format", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(supplier_Ref_No) ? new SqlParameter("@Supplier_Ref_No", supplier_Ref_No) : new SqlParameter("@Supplier_Ref_No", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }
        #endregion

        #region Order Processing New
        public async Task<List<Dictionary<string, object>>> Get_Order_Summary(int user_Id, Order_Processing_Summary order_Processing_Summary)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Summary_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.From_Date) ? new SqlParameter("@From_Date", order_Processing_Summary.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.To_Date) ? new SqlParameter("@To_Date", order_Processing_Summary.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Order_Status) ? new SqlParameter("@Order_Status", order_Processing_Summary.Order_Status) : new SqlParameter("@Order_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Stone_Status) ? new SqlParameter("@Stone_Status", order_Processing_Summary.Stone_Status) : new SqlParameter("@Stone_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Stock_Id) ? new SqlParameter("@Stock_Id", order_Processing_Summary.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    if (!string.IsNullOrEmpty(order_Processing_Summary.Act_Mod_Id))
                    {
                        command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Act_Mod_Id) ? new SqlParameter("@Act_Mod_Id", order_Processing_Summary.Act_Mod_Id) : new SqlParameter("@Act_Mod_Id", DBNull.Value));
                    }
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Module_Id) ? new SqlParameter("@Module_Id", order_Processing_Summary.Module_Id) : new SqlParameter("@Module_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<DataTable> Get_Order_Summary_Pre_Post_Excel(int user_Id, Order_Processing_Summary order_Processing_Summary)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Summary_Pre_Post_Excel_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.From_Date) ? new SqlParameter("@From_Date", order_Processing_Summary.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.To_Date) ? new SqlParameter("@To_Date", order_Processing_Summary.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Order_Status) ? new SqlParameter("@Order_Status", order_Processing_Summary.Order_Status) : new SqlParameter("@Order_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Stone_Status) ? new SqlParameter("@Stone_Status", order_Processing_Summary.Stone_Status) : new SqlParameter("@Stone_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Stock_Id) ? new SqlParameter("@Stock_Id", order_Processing_Summary.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    if (!string.IsNullOrEmpty(order_Processing_Summary.Act_Mod_Id))
                    {
                        command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Act_Mod_Id) ? new SqlParameter("@Act_Mod_Id", order_Processing_Summary.Act_Mod_Id) : new SqlParameter("@Act_Mod_Id", DBNull.Value));
                    }
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Module_Id) ? new SqlParameter("@Module_Id", order_Processing_Summary.Module_Id) : new SqlParameter("@Module_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }

            return dataTable;
        }
        public async Task<int> Create_Stone_Order_Process(Order_Stone_Process order_Stone_Processing, int user_Id)
        {
            string request_For = string.Empty;
            string _order_Status = string.Empty;
            if (!string.IsNullOrEmpty(order_Stone_Processing.QC_Request))
            {
                request_For = order_Stone_Processing.Order_Status + ", " + order_Stone_Processing.QC_Request;
            }
            else
            {
                request_For = order_Stone_Processing.Order_Status;
            }

            if (order_Stone_Processing.Order_Status == "CANCEL")
            {
                _order_Status = "COMPLETED";
            }
            else
            {
                _order_Status = "REQUESTED";
            }

            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var order_Id = !string.IsNullOrEmpty(order_Stone_Processing.Order_Id) ? new SqlParameter("@Order_Id", order_Stone_Processing.Order_Id) : new SqlParameter("@Order_Id", DBNull.Value);
            var order_No = !string.IsNullOrEmpty(order_Stone_Processing.Order_No) ? new SqlParameter("@Order_No", order_Stone_Processing.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var order_Status = new SqlParameter("@Order_Status", _order_Status);
            var stone_Status = !string.IsNullOrEmpty(request_For) ? new SqlParameter("@Stone_Status", request_For) : new SqlParameter("@Stone_Status", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(order_Stone_Processing.Remarks) ? new SqlParameter("@Remarks", order_Stone_Processing.Remarks) : new SqlParameter("@Remarks", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Request_Process @User_Id, @Order_Id, @Order_No, @Order_Status, @Stone_Status,
                    @Remarks", _user_Id, order_Id, order_No, order_Status, stone_Status, remarks));

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Order_Detail(int user_Id, Order_Process_Detail order_Process_Detail)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Detail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Process_Detail.Order_No) ? new SqlParameter("@Order_No", order_Process_Detail.Order_No) : new SqlParameter("@Order_No", DBNull.Value));
                    command.Parameters.Add(order_Process_Detail.Sub_Order_Id > 0 ? new SqlParameter("@Sub_Order_Id", order_Process_Detail.Sub_Order_Id) : new SqlParameter("@Sub_Order_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Process_Detail.Company_Name) ? new SqlParameter("@Company_Name", order_Process_Detail.Company_Name) : new SqlParameter("@Company_Name", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Process_Detail.Is_Selected_Supp_Stock_Id) ? new SqlParameter("@Is_Selected_Supp_Stock_Id", order_Process_Detail.Is_Selected_Supp_Stock_Id) : new SqlParameter("@Is_Selected_Supp_Stock_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Delete_Order_Process(string order_No, int sub_Order_Id, int user_Id)
        {
            var _order_No = !string.IsNullOrEmpty(order_No) ? new SqlParameter("@Order_No", order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var _sub_Order_Id = sub_Order_Id > 0 ? new SqlParameter("@Sub_Order_Id", sub_Order_Id) : new SqlParameter("@Sub_Order_Id", DBNull.Value);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Delete @Order_No, @Sub_Order_Id, @User_Id", _order_No, _sub_Order_Id, _user_Id));

            return result;
        }
        public async Task<(string, int)> Delete_Entire_Order_Process(string order_No, int user_Id)
        {
            var _order_No = !string.IsNullOrEmpty(order_No) ? new SqlParameter("@Order_No", order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var is_Exist = new SqlParameter("@Is_Exist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Entire_Delete @Order_No, @User_Id, @Is_Exist OUT", _order_No, _user_Id, is_Exist));
            bool _is_Exist = (bool)is_Exist.Value;

            if (_is_Exist)
                return ("exist", 409);

            return ("success", result);
        }
        public async Task<int> Accept_Request_Order_Process(Order_Process_Detail order_Process_Detail, int user_Id)
        {
            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var order_No = !string.IsNullOrEmpty(order_Process_Detail.Order_No) ? new SqlParameter("@Order_No", order_Process_Detail.Order_No) : new SqlParameter("@Order_No", DBNull.Value);
            var sub_Order_Id = order_Process_Detail.Sub_Order_Id > 0 ? new SqlParameter("@Sub_Order_Id", order_Process_Detail.Sub_Order_Id) : new SqlParameter("@Sub_Order_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Accept_Request @Order_No, @Sub_Order_Id, @User_Id", order_No, sub_Order_Id, _user_Id));

            return result;
        }
        public async Task<int> Delete_Order_Stones(string order_Id, int user_Id)
        {
            var _order_Id = !string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Stones_Delete @Order_Id, @User_Id", _order_Id, _user_Id));

            return result;
        }
        public async Task<int> Order_Processing_Reply_To_Assist(DataTable dataTable, string order_No, int sub_Order_Id, int user_Id)
        {
            var parameter = new SqlParameter("@tbl_Order", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Order_Processing_Reply_To_Assist_Table_Type]",
                Value = dataTable
            };

            var _order_No = new SqlParameter("@Order_No", order_No);
            var _sub_Order_Id = new SqlParameter("@Sub_Order_Id", sub_Order_Id);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Reply_To_Assist @tbl_Order, @Order_No, @Sub_Order_Id, @User_Id", parameter, _order_No, _sub_Order_Id, _user_Id));

            return result;
        }
        public async Task<int> Order_Processing_Completed(DataTable dataTable, string order_No, int sub_Order_Id, int user_Id, string customer_Name)
        {
            var parameter = new SqlParameter("@tbl_Order", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Order_Processing_Reply_To_Assist_Table_Type]",
                Value = dataTable
            };

            var _order_No = new SqlParameter("@Order_No", order_No);
            var _sub_Order_Id = new SqlParameter("@Sub_Order_Id", sub_Order_Id);
            var _user_Id = new SqlParameter("@User_Id", user_Id);
            var _customer_Name = new SqlParameter("@Customer_Name", customer_Name);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Completed @tbl_Order, @Order_No, @Sub_Order_Id, @User_Id, @Customer_Name", parameter, _order_No, _sub_Order_Id, _user_Id, _customer_Name));

            return result;
        }
        public async Task<int> Order_Procesing_Stone_Location_Solar(string order_No, string stock_ids)
        {
            var _order_No = new SqlParameter("@Order_No", order_No);
            var _stock_ids = new SqlParameter("@Stock_Ids", stock_ids);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Procesing_Stone_Location_Solar @Order_No, @Stock_Ids", _order_No, _stock_ids));

            return result;
        }

        public async Task<(DataTable, bool)> Get_Order_Excel_Data(IList<Report_Filter_Parameter> report_Filter_Parameters, int user_Id, string order_Id, string sub_Order_Id)
        {
            DataTable dataTable = new DataTable();
            bool _is_Admin = false;
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (report_Filter_Parameters != null && report_Filter_Parameters.Count > 0)
                    {
                        foreach (var item in report_Filter_Parameters.Where(x => !string.IsNullOrEmpty(x.Category_Value)).ToList())
                        {
                            command.Parameters.Add(!string.IsNullOrEmpty(item.Category_Value) ? new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), item.Category_Value) : new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), DBNull.Value));
                        }
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@STOCK_ID", DBNull.Value));
                    }
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(sub_Order_Id) ? new SqlParameter("@Sub_Order_Id", sub_Order_Id) : new SqlParameter("@Sub_Order_Id", DBNull.Value));

                    var is_Admin = new SqlParameter("@Is_Admin", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(is_Admin);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                    _is_Admin = (bool)is_Admin.Value;
                }
            }
            return (dataTable, _is_Admin);
        }

        public async Task<(DataTable, bool)> Get_Order_Data_Excel(string stock_Id, int user_Id, string order_Id)
        {
            DataTable dataTable = new DataTable();
            bool _is_Admin = false;
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Id) ? new SqlParameter("@STOCK_ID", stock_Id) : new SqlParameter("@STOCK_ID", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value));

                    var is_Admin = new SqlParameter("@Is_Admin", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(is_Admin);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                    _is_Admin = (bool)is_Admin.Value;
                }
            }
            return (dataTable, _is_Admin);
        }

        public async Task<DataTable> Get_Order_Data_Mazal_Excel(string stock_Id, string order_Id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Select_Excel_Mazal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(stock_Id) ? new SqlParameter("@STOCK_ID", stock_Id) : new SqlParameter("@STOCK_ID", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value));

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<List<Dictionary<string, object>>> Get_Order_Processing_Name_Status_Select(int sub_order_Id, string order_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Name_Status_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(sub_order_Id > 0 ? new SqlParameter("@Sub_Order_Id", sub_order_Id) : new SqlParameter("@Sub_Order_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DataTable> Get_Order_Excel_Data_Mazal(IList<Report_Filter_Parameter> report_Filter_Parameters, string order_Id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Select_Excel_Mazal", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (report_Filter_Parameters != null && report_Filter_Parameters.Count > 0)
                    {
                        foreach (var item in report_Filter_Parameters.Where(x => !string.IsNullOrEmpty(x.Category_Value)).ToList())
                        {
                            command.Parameters.Add(!string.IsNullOrEmpty(item.Category_Value) ? new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), item.Category_Value) : new SqlParameter("@" + item.Column_Name.Replace(" ", "_"), DBNull.Value));
                        }
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@STOCK_ID", DBNull.Value));
                    }
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Id) ? new SqlParameter("@Order_Id", order_Id) : new SqlParameter("@Order_Id", DBNull.Value));

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }
        public async Task<List<Dictionary<string, object>>> Get_Company_Name()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Company_Name_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Final_Order(Final_Order_Model final_Order_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Processing_Final_Order_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(final_Order_Model.From_Date) ? new SqlParameter("@From_Date", final_Order_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(final_Order_Model.To_Date) ? new SqlParameter("@To_Date", final_Order_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(final_Order_Model.Stock_Type) ? new SqlParameter("@STOCK_TYPE", final_Order_Model.Stock_Type) : new SqlParameter("@STOCK_TYPE", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(final_Order_Model.Stock_Id) ? new SqlParameter("@STOCK_ID", final_Order_Model.Stock_Id) : new SqlParameter("@STOCK_ID", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(final_Order_Model.Is_Selected_Supp_Stock_Id) ? new SqlParameter("@Is_Selected_Supp_Stock_Id", final_Order_Model.Is_Selected_Supp_Stock_Id) : new SqlParameter("@Is_Selected_Supp_Stock_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Order_Processing_Status_Update(Order_Processing_Status_Model order_Processing_Status_Model, int user_Id)
        {
            var id = new SqlParameter("@Id", order_Processing_Status_Model.id);
            var status = !string.IsNullOrEmpty(order_Processing_Status_Model.status) ? new SqlParameter("@Status", order_Processing_Status_Model.status) : new SqlParameter("@Status", DBNull.Value);
            var remarks = !string.IsNullOrEmpty(order_Processing_Status_Model.remarks) ? new SqlParameter("@Remarks", order_Processing_Status_Model.remarks) : new SqlParameter("@Remarks", DBNull.Value);
            var current_cost_amt = !string.IsNullOrEmpty(order_Processing_Status_Model.current_cost_amt) ? new SqlParameter("@Current_cost_amt", order_Processing_Status_Model.current_cost_amt) : new SqlParameter("@Current_cost_amt", DBNull.Value);
            var current_cost_disc = !string.IsNullOrEmpty(order_Processing_Status_Model.current_cost_disc) ? new SqlParameter("@Current_cost_disc", order_Processing_Status_Model.current_cost_disc) : new SqlParameter("@Current_cost_disc", DBNull.Value);
            var offer_amt = !string.IsNullOrEmpty(order_Processing_Status_Model.offer_amt) ? new SqlParameter("@Offer_amt", order_Processing_Status_Model.offer_amt) : new SqlParameter("@Offer_amt", DBNull.Value);
            var offer_disc = !string.IsNullOrEmpty(order_Processing_Status_Model.offer_disc) ? new SqlParameter("@Offer_disc", order_Processing_Status_Model.offer_disc) : new SqlParameter("@Offer_disc", DBNull.Value);
            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Processing_Status_Update @Id, @Status, @Remarks, @Current_cost_amt, @Current_cost_disc, @Offer_amt, @Offer_disc, @User_Id", id, status, remarks, current_cost_amt, current_cost_disc, offer_amt, offer_disc, _user_Id));

            return result;
        }

        public async Task<int> Final_Order_Processing_Create_Update(DataTable dataTable, int? user_Id)
        {
            int totalUpdatedRecords = 0;
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                using (var command = new SqlCommand("Final_Order_Processing_Create_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Final_Order_Processing_Type", dataTable);
                    command.Parameters.AddWithValue("@User_Id", user_Id);

                    var totalUpdatedRecordsParameter = new SqlParameter("@TotalUpdatedRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalUpdatedRecordsParameter);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                    totalUpdatedRecords = (int)totalUpdatedRecordsParameter.Value;
                }
            }

            return totalUpdatedRecords;
        }
        public async Task<int> Final_Order_Processing_Create_Update_Save(DataTable dataTable, int? user_Id)
        {
            int totalUpdatedRecords = 0;
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"]))
            {
                using (var command = new SqlCommand("Final_Order_Processing_Create_Update_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Final_Order_Processing_Type", dataTable);
                    command.Parameters.AddWithValue("@User_Id", user_Id);

                    var totalUpdatedRecordsParameter = new SqlParameter("@TotalUpdatedRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalUpdatedRecordsParameter);

                    command.CommandTimeout = 1800;
                    await connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                    totalUpdatedRecords = (int)totalUpdatedRecordsParameter.Value;
                }
            }

            return totalUpdatedRecords;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Summary(int user_Id, Order_Processing_Summary order_Processing_Summary)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.Stock_Id) ? new SqlParameter("@Trans_Id", order_Processing_Summary.Stock_Id) : new SqlParameter("@Trans_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.From_Date) ? new SqlParameter("@From_Date", order_Processing_Summary.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(order_Processing_Summary.To_Date) ? new SqlParameter("@To_Date", order_Processing_Summary.To_Date) : new SqlParameter("@To_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail(int trans_id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Detail_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(trans_id > 0 ? new SqlParameter("@Trans_Id", trans_id) : new SqlParameter("@Trans_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Is_Img_Cert(Lab_Entry_Is_Img_Cert_Model lab_Entry_Is_Img_Cert_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Lab_Entry_Validate_Party_Url_Format_Id_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Is_Img_Cert_Model.Supplier_Ids) ? new SqlParameter("@Supplier_Ids", lab_Entry_Is_Img_Cert_Model.Supplier_Ids) : new SqlParameter("@Supplier_Ids", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Order_Processing_Hold(int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Order_Processing_With_Hold_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Insert_Update_Lab_Entry(DataTable masterDataTable, DataTable detailDataTable, int user_Id)
        {
            var masterParameter = new SqlParameter("@Lab_Entry_Master_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Lab_Entry_Master_Table_Type]",
                Value = masterDataTable
            };

            var detailParameter = new SqlParameter("@Lab_Entry_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Lab_Entry_Detail_Table_Type]",
                Value = detailDataTable
            };

            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var transIdParameter = new SqlParameter("@Trans_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Lab_Entry_Insert_Update @Lab_Entry_Master_Table_Type, @Lab_Entry_Detail_Table_Type, @User_Id, @Trans_Id OUTPUT", masterParameter, detailParameter, _user_Id, transIdParameter));

            return (int)transIdParameter.Value;
        }

        public async Task<(int, bool)> Delete_Lab_Entry(int id)
        {
            var _id = id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value);

            var is_Exist = new SqlParameter("@Is_Exist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Lab_Entry_Delete @Id, @Is_Exist OUT", _id, is_Exist));

            var _is_Exist = (bool)is_Exist.Value;
            if (_is_Exist)
                return (409, _is_Exist);

            return (result, _is_Exist);
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Summary(int user_Id, Lab_Entry_Summary lab_Entry_Summary)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.From_Date) ? new SqlParameter("@From_Date", lab_Entry_Summary.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.To_Date) ? new SqlParameter("@To_Date", lab_Entry_Summary.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.Order_Type) ? new SqlParameter("@Order_Type", lab_Entry_Summary.Order_Type) : new SqlParameter("@Order_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.Stone_Status) ? new SqlParameter("@Stone_Status", lab_Entry_Summary.Stone_Status) : new SqlParameter("@Stone_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.Order_Status) ? new SqlParameter("@Order_Status", lab_Entry_Summary.Order_Status) : new SqlParameter("@Order_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(lab_Entry_Summary.Stock_Id) ? new SqlParameter("@Stock_Id", lab_Entry_Summary.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    command.Parameters.Add(lab_Entry_Summary.PreSold != null ? new SqlParameter("@PreSold", lab_Entry_Summary.PreSold) : new SqlParameter("@PreSold", DBNull.Value));
                    command.Parameters.Add(lab_Entry_Summary.Party_Id != null ? new SqlParameter("@Party_Id", lab_Entry_Summary.Party_Id) : new SqlParameter("@Party_Id", DBNull.Value));
                    command.Parameters.Add(lab_Entry_Summary.Delivered != null ? new SqlParameter("@Delivered", lab_Entry_Summary.Delivered) : new SqlParameter("@Delivered", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DataTable> Get_Lab_Entry_Report_Data(Report_Lab_Entry_Filter report_Lab_Entry_Filter)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Lab_Entry_Filter.Stock_Id) ? new SqlParameter("@Stock_Id", report_Lab_Entry_Filter.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<DataTable> Get_Lab_Entry_Report_Data_Dynamic(Report_Lab_Entry_Filter report_Lab_Entry_Filter)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Dynamic_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Lab_Entry_Filter.Stock_Id) ? new SqlParameter("@Stock_Id", report_Lab_Entry_Filter.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<DataTable> Get_Lab_Entry_Report_Data_Pickup(Report_Lab_Entry_Filter report_Lab_Entry_Filter)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Pickup_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Lab_Entry_Filter.Stock_Id) ? new SqlParameter("@Stock_Id", report_Lab_Entry_Filter.Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<DataTable> Get_Lab_Entry_Auto_Order_Not_Placed_Overseas_Email()
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Auto_Order_Not_Placed_Overseas_Email", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }

            return dataTable;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail_For_Shipment_Verification(int supplier_Id, string certificate_No)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Detail_Shipment_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(certificate_No) ? new SqlParameter("@Certificate_No", certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Unavailable_Lab_Entry_Detail_For_Shipment_Verification(string certificate_No)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Detail_Unavailable_Shipment_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(certificate_No) ? new SqlParameter("@Certificate_No", certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Detail_For_Shipment_Verification_By_Id(string Lab_Entry_Detail_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Detail_Shipment_By_Id_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Lab_Entry_Detail_Id) ? new SqlParameter("@Lab_Entry_Detail_Id", Lab_Entry_Detail_Id) : new SqlParameter("@Lab_Entry_Detail_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Expenses_DropDown()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Expenses_DropDown_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<(int, bool)> Insert_Update_Purchase(DataTable masterDataTable, DataTable detailDataTable, DataTable termsDataTable, DataTable expensesDataTable, DataTable purchaseDetailLooseDataTable, int user_Id)
        {
            var masterParameter = new SqlParameter("@Purchase_Master_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Master_Table_Type]",
                Value = masterDataTable
            };

            var detailParameter = new SqlParameter("@Purchase_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Table_Type]",
                Value = detailDataTable
            };

            var termsParameter = new SqlParameter("@Purchase_Terms_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Terms_Table_Type]",
                Value = termsDataTable
            };

            var expensesParameter = new SqlParameter("@Purchase_Expenses_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Expenses_Table_Type]",
                Value = expensesDataTable
            };

            var purchaseDetailLooseParameter = new SqlParameter("@Purchase_Detail_Loose_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Loose_Table_Type]",
                Value = purchaseDetailLooseDataTable
            };

            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var is_Exists = new SqlParameter("@Is_Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Insert_Update @Purchase_Master_Table_Type, @Purchase_Detail_Table_Type, @Purchase_Terms_Table_Type, @Purchase_Expenses_Table_Type, @Purchase_Detail_Loose_Table_Type, @User_Id, @Is_Exists OUT", masterParameter, detailParameter, termsParameter, expensesParameter, purchaseDetailLooseParameter, _user_Id, is_Exists));

            var _is_Exist = (bool)is_Exists.Value;

            if (_is_Exist)
                return (409, _is_Exist);

            return (result, _is_Exist);
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Master(Purchase_Master_Search_Model purchase_Master_Search_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.From_Date) ? new SqlParameter("@From_Date", purchase_Master_Search_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.To_Date) ? new SqlParameter("@To_Date", purchase_Master_Search_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.Doc_Type) ? new SqlParameter("@Doc_Type", purchase_Master_Search_Model.Doc_Type) : new SqlParameter("@Doc_Type", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.Stock_Status) ? new SqlParameter("@Stock_Status", purchase_Master_Search_Model.Stock_Status) : new SqlParameter("@Stock_Status", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.Stock_Certificate_No) ? new SqlParameter("@Stock_Certificate_No", purchase_Master_Search_Model.Stock_Certificate_No) : new SqlParameter("@Stock_Certificate_No", DBNull.Value));
                    command.Parameters.Add(purchase_Master_Search_Model.Company_Id > 0 ? new SqlParameter("@Company_Id", purchase_Master_Search_Model.Company_Id) : new SqlParameter("@Company_Id", DBNull.Value));
                    command.Parameters.Add(purchase_Master_Search_Model.Year_Id > 0 ? new SqlParameter("@Year_Id", purchase_Master_Search_Model.Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }


        public async Task<Dictionary<string, object>> Get_Purchase(int Trans_Id)
        {
            var output = new Dictionary<string, object>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();

                // Fetch Purchase_Master
                var Purchase_Master_Result = await ExecuteStoredProcedure(connection, "Purchase_Master_By_Trans_Id_Select", Trans_Id);
                if (Purchase_Master_Result != null)
                {
                    output["Purchase_Master"] = Purchase_Master_Result.FirstOrDefault();
                }

                // Fetch Purchase_Detail
                var Purchase_Detail_Result = await ExecuteStoredProcedure(connection, "Purchase_Detail_By_Trans_Id_Select", Trans_Id);
                if (Purchase_Detail_Result != null)
                {
                    output["Purchase_Detail_List"] = Purchase_Detail_Result;
                }
                else
                {
                    output["Purchase_Detail_List"] = new List<object>();
                }

                // Fetch terms_Trans_Dets
                var Purchase_Terms_Result = await ExecuteStoredProcedure(connection, "Purchase_Terms_By_Trans_Id_Select", Trans_Id);
                if (Purchase_Terms_Result != null)
                {
                    output["Purchase_Terms_List"] = Purchase_Terms_Result;
                }
                else
                {
                    output["Purchase_Terms_List"] = new List<object>();
                }

                // Fetch expense_Trans_Dets
                var expense_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Purchase_Expenses_By_Trans_Id_Select", Trans_Id);
                if (expense_Trans_Dets_Result != null)
                {
                    output["Purchase_Expenses_List"] = expense_Trans_Dets_Result;
                }
                else
                {
                    output["Purchase_Expenses_List"] = new List<object>();
                }

                //Fetch purchase_Detail_Loose_Trans_Dets
                var purchase_Detail_Loose_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Purchase_Detail_Loose_By_Trans_Id_Select", Trans_Id);
                if (purchase_Detail_Loose_Trans_Dets_Result != null)
                {
                    output["Purchase_Detail_Loose_List"] = purchase_Detail_Loose_Trans_Dets_Result;
                }
                else
                {
                    output["Purchase_Detail_Loose_List"] = new List<object>();
                }
            }

            return output;
        }
        private async Task<List<Dictionary<string, object>>> ExecuteStoredProcedure(SqlConnection connection, string storedProcedureName, int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Trans_Id", Trans_Id));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dict = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.GetValue(i);

                            dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                        }

                        result.Add(dict);
                    }
                }
            }

            return result.Count > 0 ? result : null;
        }

        public async Task<int> Delete_Purchase(int Trans_Id, int User_Id)
        {
            var trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Delete @Trans_Id, @User_Id", trans_Id, user_Id);

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Status_Summary(string Stock_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Status_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Stock_Id) ? new SqlParameter("@Stock_Id", Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Non_Status_Summary(string Stock_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Non_Update_Status_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Stock_Id) ? new SqlParameter("@Stock_Id", Stock_Id) : new SqlParameter("@Stock_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Lab_Entry_Report_Status_Update(DataTable statusDataTable, int user_Id)
        {
            var masterParameter = new SqlParameter("@Lab_Entry_Status_Update_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Lab_Entry_Status_Update_Table_Type]",
                Value = statusDataTable
            };

            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Lab_Entry_Status_Update @Lab_Entry_Status_Update_Table_Type, @User_Id", masterParameter, _user_Id));

            return result;
        }

        public async Task<DataTable> Get_Purchase_Detail_Excel(int Trans_Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Purchase_Detail_By_Trans_Id_Select_Excel", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Trans_Id", Trans_Id > 0 ? (object)Trans_Id : DBNull.Value);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        public async Task<DataTable> Get_Purchase_Detail_QC_Excel(string Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Purchase_Detail_By_Id_QC_Select_Excel", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(!string.IsNullOrEmpty(Id) ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value));

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Contract(string certificate_No)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Contract_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(certificate_No) ? new SqlParameter("@Certificate_No", certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Purchase_Detail_Contract_Update(DataTable purchase_Detail_Contract_DataTable, int User_Id)
        {
            var _purchase_Detail_Contract_DataTable = new SqlParameter("@Purchase_Detail_Contract_Update_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Contract_Update_Table_Type]",
                Value = purchase_Detail_Contract_DataTable
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_Contract_Update @Purchase_Detail_Contract_Update_Table_Type, @User_Id", _purchase_Detail_Contract_DataTable, user_Id));

            return result;
        }

        public async Task<int> Purchase_Detail_Outward_Update(DataTable dataTable, int Trans_Id, int User_Id)
        {
            var _dataTable = new SqlParameter("@Purchase_Detail_Outward_Update_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Outward_Update_Table_Type]",
                Value = dataTable
            };

            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var _user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_Outward_Update @Purchase_Detail_Outward_Update_Table_Type, @Trans_Id, @User_Id", _dataTable, _trans_Id, _user_Id));

            return result;
        }

        public async Task<int> Purchase_Detail_QC_Update(DataTable dataTable, int Trans_Id, int User_Id)
        {
            var _dataTable = new SqlParameter("@Purchase_Detail_QC_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_QC_Table_Type]",
                Value = dataTable
            };

            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var _user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_QC_Update @Purchase_Detail_QC_Table_Type, @Trans_Id, @User_Id", _dataTable, _trans_Id, _user_Id));

            return result;
        }

        public async Task<int> Purchase_Confirm_Update(int Trans_Id, int User_Id)
        {
            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Confirm_Update @Trans_Id, @User_Id", _trans_Id, user_Id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Order_Process_Pending_FCM_Token()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Order_Process_Pending_FCM_Token", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Order_Process_Pending_FCM_Token_Update(string Order_No)
        {
            var _Order_No = new SqlParameter("@Order_No", Order_No);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Order_Process_Pending_FCM_Token_Update @Order_No", _Order_No));

            return result;
        }

        public async Task<int> Update_Purchase_Master_File_Status(int Trans_Id, bool File_Status, int User_Id)
        {
            var trans_Id = new SqlParameter("@Trans_Id", Trans_Id);
            var file_Status = new SqlParameter("@File_Status", File_Status);
            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Master_File_Status_Update @Trans_Id, @File_Status, @User_Id", trans_Id, file_Status, user_Id);

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Pricing(int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();

                result = await ExecuteStoredProcedure(connection, "Purchase_Detail_Pricing_By_Trans_Id_Select", Trans_Id);

            }

            return result;
        }

        public async Task<DataTable> Get_Purchase_Pricing_Excel(int Trans_Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Purchase_Detail_Pricing_By_Trans_Id_Select_Excel", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Trans_Id", Trans_Id > 0 ? (object)Trans_Id : DBNull.Value);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        public async Task<int> Purchase_Detail_Pricing_Update(DataTable dataTable, int User_Id)
        {
            var Parameter = new SqlParameter("@Purchase_Detail_Pricing_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Pricing_Table_Type]",
                Value = dataTable
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_Pricing_Update @Purchase_Detail_Pricing_Table_Type, @User_Id", Parameter, user_Id));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Lab_Entry_Report_Status_Sunrise_Summary(string Sunrise_Stock_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Lab_Entry_Report_Status_Sunrise_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(Sunrise_Stock_Id) ? new SqlParameter("@Sunrise_Stock_Id", Sunrise_Stock_Id) : new SqlParameter("@Sunrise_Stock_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Lab_Entry_Report_Status_Sunrise_Update(DataTable dataTable)
        {
            var Parameter = new SqlParameter("@Lab_Entry_Status_Update_Sunrise_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Lab_Entry_Status_Update_Sunrise_Table_Type]",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Lab_Entry_Status_Sunrise_Update @Lab_Entry_Status_Update_Sunrise_Table_Type", Parameter));

            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Fortune_Lab_Entry_Data()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Fortune_Lab_Entry_Data_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Master_Pricing(Purchase_Master_Search_Model purchase_Master_Search_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Master_Pricing_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.From_Date) ? new SqlParameter("@From_Date", purchase_Master_Search_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.To_Date) ? new SqlParameter("@To_Date", purchase_Master_Search_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Pricing(int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();

                result = await ExecuteStoredProcedure(connection, "Purchase_Detail_By_Trans_Id_Pricing_Select", Trans_Id);

            }

            return result;
        }

        public async Task<int> Purchase_Pricing_Update(DataTable dataTable, int User_Id)
        {
            var Parameter = new SqlParameter("@Purchase_Pricing_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Pricing_Table_Type]",
                Value = dataTable
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Pricing_Update @Purchase_Pricing_Table_Type, @User_Id", Parameter, user_Id));

            return result;
        }

        public async Task<int> Update_Purchase_Master_Is_Upcoming_Approval(Purchase_Approval purchase_Approval, int User_Id)
        {
            var trans_Id = new SqlParameter("@Trans_Id", purchase_Approval.Trans_Id);
            var is_Upcoming_Approval = new SqlParameter("@Is_Upcoming_Approval", purchase_Approval.Is_Upcoming_Approval);
            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Master_Is_Upcoming_Approval_Update @Trans_Id, @Is_Upcoming_Approval, @User_Id", trans_Id, is_Upcoming_Approval, user_Id);

            return result;
        }

        public async Task<int> Update_Purchase_Master_Is_Repricing_Approval(Purchase_Approval purchase_Approval, int User_Id)
        {
            var trans_Id = new SqlParameter("@Trans_Id", purchase_Approval.Trans_Id);
            var is_Repricing_Approval = new SqlParameter("@Is_Repricing_Approval", purchase_Approval.Is_Repricing_Approval);
            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Master_Is_Repricing_Approval_Update @Trans_Id, @Is_Repricing_Approval, @User_Id", trans_Id, is_Repricing_Approval, user_Id);

            return result;
        }

        public async Task<DataTable> Get_Purchase_Detail_Pricing_Excel(int Trans_Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Purchase_Detail_By_Trans_Id_Pricing_Select_Excel", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Trans_Id", Trans_Id > 0 ? (object)Trans_Id : DBNull.Value);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Media_Upload(Purchase_Media_Upload_Search_Model purchase_Media_Upload_Search_Model, DataTable dt)
        {
            var result = new List<Dictionary<string, object>>();

            if (dt != null)
            {
                using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
                {
                    using (var command = new SqlCommand("Purchase_Media_Upload_Select", connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("Sunrise_Stock_Id", typeof(string));

                        foreach (DataRow item in dt.Rows)
                        {
                            dataTable.Rows.Add(item["REF_NO"]);
                        }

                        var parameter = new SqlParameter("@Media_Inward_Table_Type", SqlDbType.Structured)
                        {
                            TypeName = "[dbo].[Media_Inward_Table_Type]",
                            Value = dataTable
                        };

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(!string.IsNullOrEmpty(purchase_Media_Upload_Search_Model.From_Date) ? new SqlParameter("@From_Date", purchase_Media_Upload_Search_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                        command.Parameters.Add(!string.IsNullOrEmpty(purchase_Media_Upload_Search_Model.To_Date) ? new SqlParameter("@To_Date", purchase_Media_Upload_Search_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                        command.Parameters.Add(!string.IsNullOrEmpty(purchase_Media_Upload_Search_Model.Image_Status.ToString()) ? new SqlParameter("@Image_Status", purchase_Media_Upload_Search_Model.Image_Status) : new SqlParameter("@Image_Status", DBNull.Value));
                        command.Parameters.Add(!string.IsNullOrEmpty(purchase_Media_Upload_Search_Model.Video_Status.ToString()) ? new SqlParameter("@Video_Status", purchase_Media_Upload_Search_Model.Video_Status) : new SqlParameter("@Video_Status", DBNull.Value));
                        command.Parameters.Add(!string.IsNullOrEmpty(purchase_Media_Upload_Search_Model.Certificate_Status.ToString()) ? new SqlParameter("@Certificate_Status", purchase_Media_Upload_Search_Model.Certificate_Status) : new SqlParameter("@Certificate_Status", DBNull.Value));
                        command.Parameters.Add(purchase_Media_Upload_Search_Model.PreSold != null ? new SqlParameter("@PreSold", purchase_Media_Upload_Search_Model.PreSold) : new SqlParameter("@PreSold", DBNull.Value));
                        command.Parameters.Add(parameter);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var dict = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    var columnValue = reader.GetValue(i);

                                    dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                                }

                                result.Add(dict);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<int> Update_Purchase_Media_Upload(Purchase_Media_Upload_Model purchase_Media_Upload_Model)
        {
            var Id = new SqlParameter("@Id", purchase_Media_Upload_Model.Id ?? (object)DBNull.Value);
            var Image_Status = new SqlParameter("@Image_Status", purchase_Media_Upload_Model.Image_Status ?? (object)DBNull.Value);
            var Video_Status = new SqlParameter("@Video_Status", purchase_Media_Upload_Model.Video_Status ?? (object)DBNull.Value);
            var Certificate_Status = new SqlParameter("@Certificate_Status", purchase_Media_Upload_Model.Certificate_Status ?? (object)DBNull.Value);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Media_Upload_Update @Id, @Image_Status, @Video_Status, @Certificate_Status", Id, Image_Status, Video_Status, Certificate_Status);

            return result;
        }

        public async Task<int> Update_Purchase_Shipment_Receive(string Trans_Id)
        {
            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Purchase_Shipment_Receive_Update @Trans_Id", _trans_Id);

            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Qc(int Trans_Id, string Doc_Type)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_For_Qc_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Trans_Id", Trans_Id));
                    command.Parameters.Add(!string.IsNullOrEmpty(Doc_Type) ? new SqlParameter("@Doc_Type", Doc_Type) : new SqlParameter("@Doc_Type", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Purchase QC Approval

        public async Task<List<Dictionary<string, object>>> Get_Purchase_QC_Approval(Purchase_Master_Search_Model purchase_Master_Search_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Qc_Approval_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.From_Date) ? new SqlParameter("@From_Date", purchase_Master_Search_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.To_Date) ? new SqlParameter("@To_Date", purchase_Master_Search_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Master_Search_Model.Stock_Certificate_No) ? new SqlParameter("@Stock_Certificate_No", purchase_Master_Search_Model.Stock_Certificate_No) : new SqlParameter("@Stock_Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<DataTable> Get_Purchase_QC_Approval_Data_Dynamic(Report_Lab_Entry_Filter report_Lab_Entry_Filter)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Qc_Approval_Dynamic_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Lab_Entry_Filter.Stock_Id) ? new SqlParameter("@Ids", report_Lab_Entry_Filter.Stock_Id) : new SqlParameter("@Ids", DBNull.Value));
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<DataTable> Get_Purchase_QC_Approval_Data(Report_Lab_Entry_Filter report_Lab_Entry_Filter)
        {
            DataTable dataTable = new DataTable();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Qc_Approval_Select_Excel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(report_Lab_Entry_Filter.Stock_Id) ? new SqlParameter("@Ids", report_Lab_Entry_Filter.Stock_Id) : new SqlParameter("@Ids", DBNull.Value));
                    await connection.OpenAsync();

                    using var da = new SqlDataAdapter();
                    da.SelectCommand = command;

                    using var ds = new DataSet();
                    da.Fill(ds);

                    dataTable = ds.Tables[ds.Tables.Count - 1];
                }
            }
            return dataTable;
        }

        public async Task<int> Purchase_QC_Reply_Status_Update(DataTable dataTable, int User_Id)
        {
            var Parameter = new SqlParameter("@Purchase_Detail_QC_Reply_Status_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_QC_Reply_Status_Table_Type]",
                Value = dataTable
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_QC_Reply_Status_Update @Purchase_Detail_QC_Reply_Status_Table_Type, @User_Id", Parameter, user_Id));

            return result;
        }

        public async Task<int> Purchase_Detail_QC_Complete_Update(DataTable dataTable, int Trans_Id, int User_Id)
        {
            var _dataTable = new SqlParameter("@Purchase_Detail_QC_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_QC_Table_Type]",
                Value = dataTable
            };

            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var _user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_QC_Complete_Update @Purchase_Detail_QC_Table_Type, @Trans_Id, @User_Id", _dataTable, _trans_Id, _user_Id));

            return result;
        }

        public async Task<int> Purchase_Detail_QC_Close_Update(int Trans_Id, int User_Id)
        {
            var _trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var _user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_QC_Close_Update @Trans_Id, @User_Id", _trans_Id, _user_Id));

            return result;
        }

        #endregion

        #region Transaction

        public async Task<List<Dictionary<string, object>>> Get_Transaction_Master(Transaction_Master_Search_Model transaction_Master_Search_Model)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Transaction_Master_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(transaction_Master_Search_Model.From_Date) ? new SqlParameter("@From_Date", transaction_Master_Search_Model.From_Date) : new SqlParameter("@From_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(transaction_Master_Search_Model.To_Date) ? new SqlParameter("@To_Date", transaction_Master_Search_Model.To_Date) : new SqlParameter("@To_Date", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(transaction_Master_Search_Model.Process_Id) ? new SqlParameter("@Process_Id", transaction_Master_Search_Model.Process_Id) : new SqlParameter("@Process_Id", DBNull.Value));
                    command.Parameters.Add(transaction_Master_Search_Model.Company_Id > 0 ? new SqlParameter("@Company_Id", transaction_Master_Search_Model.Company_Id) : new SqlParameter("@Company_Id", DBNull.Value));
                    command.Parameters.Add(transaction_Master_Search_Model.Year_Id > 0 ? new SqlParameter("@Year_Id", transaction_Master_Search_Model.Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Dictionary<string, object>> Get_Transaction(int Trans_Id)
        {
            var output = new Dictionary<string, object>();

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                await connection.OpenAsync();

                // Fetch Transaction_Master
                var Transaction_Master_Result = await ExecuteStoredProcedure(connection, "Transaction_Master_By_Trans_Id_Select", Trans_Id);
                if (Transaction_Master_Result != null)
                {
                    output["Transaction_Master"] = Transaction_Master_Result.FirstOrDefault();
                }

                // Fetch Transaction_Detail
                var Transaction_Detail_Result = await ExecuteStoredProcedure(connection, "Transaction_Detail_By_Trans_Id_Select", Trans_Id);
                if (Transaction_Detail_Result != null)
                {
                    output["Transaction_Detail_List"] = Transaction_Detail_Result;
                }
                else
                {
                    output["Transaction_Detail_List"] = new List<object>();
                }

                // Fetch terms_Trans_Dets
                var Transaction_Terms_Result = await ExecuteStoredProcedure(connection, "Transaction_Terms_By_Trans_Id_Select", Trans_Id);
                if (Transaction_Terms_Result != null)
                {
                    output["Transaction_Terms_List"] = Transaction_Terms_Result;
                }
                else
                {
                    output["Transaction_Terms_List"] = new List<object>();
                }

                // Fetch expense_Trans_Dets
                var Transaction_Expense_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Transaction_Expenses_By_Trans_Id_Select", Trans_Id);
                if (Transaction_Expense_Trans_Dets_Result != null)
                {
                    output["Transaction_Expenses_List"] = Transaction_Expense_Trans_Dets_Result;
                }
                else
                {
                    output["Transaction_Expenses_List"] = new List<object>();
                }

                //Fetch Transaction_Detail_Loose_Trans_Dets
                var Transaction_Detail_Loose_Trans_Dets_Result = await ExecuteStoredProcedure(connection, "Transaction_Detail_Loose_By_Trans_Id_Select", Trans_Id);
                if (Transaction_Detail_Loose_Trans_Dets_Result != null)
                {
                    output["Transaction_Detail_Loose_List"] = Transaction_Detail_Loose_Trans_Dets_Result;
                }
                else
                {
                    output["Transaction_Detail_Loose_List"] = new List<object>();
                }
            }

            return output;
        }

        public async Task<(int, bool)> Insert_Update_Transaction(DataTable masterDataTable, DataTable detailDataTable, DataTable termsDataTable, DataTable expensesDataTable, DataTable detailLooseDataTable, int user_Id)
        {
            var masterParameter = new SqlParameter("@Transaction_Master_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Transaction_Master_Table_Type]",
                Value = masterDataTable
            };

            var detailParameter = new SqlParameter("@Transaction_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Transaction_Detail_Table_Type]",
                Value = detailDataTable
            };

            var termsParameter = new SqlParameter("@Transaction_Terms_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Transaction_Terms_Table_Type]",
                Value = termsDataTable
            };

            var expensesParameter = new SqlParameter("@Transaction_Expenses_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Transaction_Expenses_Table_Type]",
                Value = expensesDataTable
            };

            var purchaseDetailLooseParameter = new SqlParameter("@Transaction_Detail_Loose_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Transaction_Detail_Loose_Table_Type]",
                Value = detailLooseDataTable
            };

            var _user_Id = new SqlParameter("@User_Id", user_Id);

            var is_Exists = new SqlParameter("@Is_Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Transaction_Insert_Update @Transaction_Master_Table_Type, @Transaction_Detail_Table_Type, @Transaction_Terms_Table_Type, @Transaction_Expenses_Table_Type, @Transaction_Detail_Loose_Table_Type, @User_Id, @Is_Exists OUT", masterParameter, detailParameter, termsParameter, expensesParameter, purchaseDetailLooseParameter, _user_Id, is_Exists));

            var _is_Exist = (bool)is_Exists.Value;

            if (_is_Exist)
                return (409, _is_Exist);

            return (result, _is_Exist);
        }

        public async Task<int> Delete_Transaction(int Trans_Id, int User_Id)
        {
            var trans_Id = new SqlParameter("@Trans_Id", Trans_Id);

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await _dbContext.Database
                                .ExecuteSqlRawAsync("EXEC Transaction_Delete @Trans_Id, @User_Id", trans_Id, user_Id);

            return result;
        }

        #endregion

        #region Purchase Return

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Purchase_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_For_Purchase_Return_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Purchase_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Unavailable_For_Purchase_Return_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Consignment Return

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_For_Consignment_Return_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Return(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Unavailable_For_Consignment_Return_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Purchase from Consignment

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Consignment_Purchase(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_For_Consignment_Purchase_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Consignment_Purchase(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Unavailable_For_Consignment_Purchase_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(purchase_Detail_For_Purchase_Return.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", purchase_Detail_For_Purchase_Return.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Trans_Date) ? new SqlParameter("@Trans_Date", purchase_Detail_For_Purchase_Return.Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Hold

        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_For_Hold(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_For_Hold_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<Dictionary<string, object>>> Get_Unavailable_Purchase_Detail_For_Hold(Purchase_Detail_For_Purchase_Return purchase_Detail_For_Purchase_Return)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Purchase_Detail_Unavailable_For_Hold_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(!string.IsNullOrEmpty(purchase_Detail_For_Purchase_Return.Certificate_No) ? new SqlParameter("@Certificate_No", purchase_Detail_For_Purchase_Return.Certificate_No) : new SqlParameter("@Certificate_No", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Party Url Format

        public async Task<IList<Party_Url_Format>> Get_Party_Url_Format(int Id)
        {
            var _Id = Id > 0 ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value);

            var party_Url_Formats = await Task.Run(() => _dbContext.Party_Url_Format
                            .FromSqlRaw(@"exec Party_Url_Format_Select @Id", _Id).ToListAsync());

            return party_Url_Formats;
        }

        public async Task<int> Create_Update_Party_Url_Format(DataTable dataTable)
        {
            var parameter = new SqlParameter("@Party_Url_Format_Data_Type", SqlDbType.Structured)
            {
                TypeName = "dbo.Party_Url_Format_Data_Type",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Party_Url_Format_Insert_Update @Party_Url_Format_Data_Type", parameter));
            return result;
        }

        public async Task<int> Delete_Party_Url_Format(int id)
        {
            var _id = id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Party_Url_Format_Delete @Id", _id));

            return result;
        }

        #endregion

        #region Get Lastest Supplier Stock

        public async Task<DataTable> Get_Latest_Supplier_Stock_Excel_Download(int supplier_Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Latest_Supplier_Stock_Select", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Supplier_Id", supplier_Id > 0 ? (object)supplier_Id : DBNull.Value);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }

        #endregion

        #region Connect GIA Report Layout Save

        public async Task<List<Dictionary<string, object>>> Get_Connect_GIA_Report_Users_Role(int id, int user_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Connect_GIA_Report_Users_Role_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(id > 0 ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));
                    command.Parameters.Add(user_Id > 0 ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value));
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<IList<Report_Layout_Save>> Get_Connect_GIA_Report_Layout_Save(int User_Id, int Rm_Id)
        {
            var user_Id = User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var rm_Id = Rm_Id > 0 ? new SqlParameter("@Rm_Id", Rm_Id) : new SqlParameter("@Rm_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Report_Layout_Save
                            .FromSqlRaw(@"EXEC Connect_GIA_Report_Layout_Save_Select @User_Id,@Rm_Id", user_Id, rm_Id)
                            .ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    var report_layout_Id = item.Id > 0 ? new SqlParameter("@Report_Layout_Id", item.Id) : new SqlParameter("@Report_Layout_Id", DBNull.Value);

                    item.Report_Layout_Save_Detail_List = await Task.Run(() => _dbContext.Report_Layout_Save_Detail
                            .FromSqlRaw(@"EXEC Connect_GIA_Report_Layout_Save_Detail_Select @Report_Layout_Id", report_layout_Id)
                            .ToListAsync());
                }
            }
            return result;
        }

        public async Task<(string, int)> Create_Update_Connect_GIA_Report_Layout_Save(Report_Layout_Save report_Layout_Save)
        {
            var id = new SqlParameter("@Id", report_Layout_Save.Id);
            var user_Id = new SqlParameter("@User_Id", report_Layout_Save.User_Id);
            var rm_Id = new SqlParameter("@Rm_Id", report_Layout_Save.Rm_Id);
            var name = !string.IsNullOrEmpty(report_Layout_Save.Name) ? new SqlParameter("@Name", report_Layout_Save.Name) : new SqlParameter("@Name", DBNull.Value);
            var status = new SqlParameter("@Status", report_Layout_Save.Status);
            var insertedId = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var is_Exist = new SqlParameter("@IsExist", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Connect_GIA_Report_Layout_Save_Insert_Update @Id, @User_Id,@Rm_Id, @Name, @Status, @Inserted_Id OUT, @IsExist OUT",
                id, user_Id, rm_Id, name, status, insertedId, is_Exist));

            if ((int)is_Exist.Value == 1)
            {
                return ("exist", 0);
            }
            else
            {
                var _inserted_Id = (int)insertedId.Value;
                return ("success", _inserted_Id);
            }
        }

        public async Task<int> Insert_Update_Connect_GIA_Report_Layout_Save_Detail(DataTable dataTable)
        {
            var parameter = new SqlParameter("@tblConnect_GIA_Report_Layout_Save_Detail", SqlDbType.Structured)
            {
                TypeName = "dbo.Report_Layout_Save_Detail_Table_Type",
                Value = dataTable
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Connect_GIA_Report_Layout_Save_Detail_Insert_Update @tblConnect_GIA_Report_Layout_Save_Detail", parameter);

            return result;
        }

        #endregion

        #region Purchase Detail Manual Discount
        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_Manual_Discount(DataTable dataTable, int type)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Purchase_Detail_Manual_Discount_Select]", connection))
                {
                    var Parameter = new SqlParameter("@Purchase_Detail_Manual_Discount_Table_Type", SqlDbType.Structured)
                    {
                        TypeName = "[dbo].[Purchase_Detail_Manual_Discount_Table_Type]",
                        Value = dataTable
                    };

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(Parameter);
                    command.Parameters.Add(new SqlParameter("@Type", type));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Set_Purchase_Detail_Manual_Discount(DataTable dataTable, int User_Id)
        {
            var Parameter = new SqlParameter("@Purchase_Detail_Manual_Discount_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Purchase_Detail_Manual_Discount_Table_Type]",
                Value = dataTable
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"EXEC Purchase_Detail_Manual_Discount_Update @Purchase_Detail_Manual_Discount_Table_Type, @User_Id", Parameter, user_Id));

            return result;
        }
        #endregion

        #region Quotation Master
        public async Task<Dictionary<string, object>> Get_Quotation_Master(int Quotation_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Quotation_Master_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Quotation_Id > 0 ? new SqlParameter("@Quotation_Id", Quotation_Id) : new SqlParameter("@Quotation_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result.FirstOrDefault();
        }
        public async Task<List<Dictionary<string, object>>> Get_Quotation_Detail(int Quotation_Id, bool isSummary = false)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Quotation_Detail_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Quotation_Id > 0 ? new SqlParameter("@Quotation_Id", Quotation_Id) : new SqlParameter("@Quotation_Id", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Is_Summary", isSummary));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> Set_Quotation_Master(DataTable dataTable, DataTable quotaion_Expense, Quotation_Master model, int User_Id)
        {
            var _labEntryData = new SqlParameter("@LabEntryData", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Quotation_Master_Table_Type]",
                Value = dataTable
            };

            var _quotationExpenseData = new SqlParameter("@Quotation_Expense", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Quotation_Expense_Table_Type]",
                Value = quotaion_Expense
            };

            var user_Id = new SqlParameter("@User_Id", User_Id);

            DateTime transDate;
            if (!DateTime.TryParseExact(model.Trans_Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out transDate))
            {
                transDate = DateTime.Now.Date;
            }
            var _trance_Date = new SqlParameter("@Trans_Date", SqlDbType.Date)
            {
                Value = transDate
            };
            var _trance_date_param = (!string.IsNullOrEmpty(model.Trans_Date) ? _trance_Date : new SqlParameter("@Trans_Date", DBNull.Value));
            var _bill_to_id = (model.Bill_To_Id > 0 ? new SqlParameter("@Bill_To_Id", model.Bill_To_Id) : new SqlParameter("@Bill_To_Id", DBNull.Value));
            var _ship_to_id = (model.Ship_To_Id > 0 ? new SqlParameter("@Ship_To_Id", model.Ship_To_Id) : new SqlParameter("@Ship_To_Id", DBNull.Value));
            var _terms_Id = (model.Terms_Id > 0 ? new SqlParameter("@Terms_Id", model.Terms_Id) : new SqlParameter("@Terms_Id", DBNull.Value));
            var _currency_Id = (model.Currency_Id > 0 ? new SqlParameter("@Currency_Id", model.Currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value));
            var _ex_Rate = (model.Ex_Rate > 0 ? new SqlParameter("@Ex_Rate", model.Ex_Rate) : new SqlParameter("@Ex_Rate", DBNull.Value));
            var _bank_Id = (model.Bank_Id > 0 ? new SqlParameter("@Bank_Id", model.Bank_Id) : new SqlParameter("@Bank_Id", DBNull.Value));
            var _remark = (!string.IsNullOrEmpty(model.Remark) ? new SqlParameter("@Remark", model.Remark) : new SqlParameter("@Remark", DBNull.Value));
            var _year_Id = (model.Year_Id > 0 ? new SqlParameter("@Year_Id", model.Year_Id) : new SqlParameter("@Year_Id", DBNull.Value));
            var _user_Id = (User_Id > 0 ? new SqlParameter("@User_Id", User_Id) : new SqlParameter("@User_Id", DBNull.Value));
            var _company_Id = (model.Company_Id > 0 ? new SqlParameter("@Company_Id", model.Company_Id) : new SqlParameter("@Company_Id", DBNull.Value));
            var _inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                   .ExecuteSqlRawAsync(@"
                        EXEC [dbo].[Quotation_Master_Insert_Update]
                        @Trans_Date, @Bill_To_Id, @Ship_To_Id, @Terms_Id, @Currency_Id, @Ex_Rate, @Bank_Id, @Remark,
                        @Year_Id, @User_Id, @Company_Id,
                        @Quotation_Expense, @LabEntryData, @Inserted_Id OUT",
                        _trance_Date, _bill_to_id, _ship_to_id, _terms_Id, _currency_Id, _ex_Rate, _bank_Id, _remark,
                        _year_Id, _user_Id, _company_Id,
                        _quotationExpenseData, _labEntryData, _inserted_Id));

            return (int)_inserted_Id.Value;
        }
        public async Task<List<Dictionary<string, object>>> Get_Quotation_Other_Detail(string Trans_Date)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Quotation_Other_Detail]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(!string.IsNullOrEmpty(Trans_Date) ? new SqlParameter("@Trans_Date", Trans_Date) : new SqlParameter("@Trans_Date", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<IList<DropdownModel>> Get_Quotation_Remarks_List()
        {
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"EXEC [dbo].[Quotation_Master_Remark_List]").ToListAsync());
            return result;
        }
        public async Task<(IList<DropdownModel>, int)> Get_Quotation_Company_Bank_List(int Company_Id, int Currency_Id)
        {
            var _company_Id = Company_Id > 0 ? new SqlParameter("@Company_Id", Company_Id) : new SqlParameter("@Company_Id", DBNull.Value);
            var _currency_Id = Currency_Id > 0 ? new SqlParameter("@Currency_Id", Currency_Id) : new SqlParameter("@Currency_Id", DBNull.Value);
            var _selected_Id = new SqlParameter("@Selected_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = await Task.Run(() => _dbContext.DropdownModel
                            .FromSqlRaw(@"EXEC [dbo].[Quotation_Company_Bank_List] @Company_Id, @Currency_Id, @Selected_Id OUT", _company_Id, _currency_Id, _selected_Id).ToListAsync());

            int selectedId = (int)_selected_Id.Value;
            return (result, selectedId);
        }
        public async Task<List<Dictionary<string, object>>> Get_Quotation_Expense_Detail(int Quotation_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Quotation_Expense_Detail_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Quotation_Id > 0 ? new SqlParameter("@Quotation_Id", Quotation_Id) : new SqlParameter("@Quotation_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Grade Master
        public async Task<(IList<Dictionary<string, object>>, int)> Get_Grade_Master(int Grade_Id)
        {
            var result = new List<Dictionary<string, object>>();
            int totalRecord = 0;
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Grade_Master_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Grade_Id > 0 ? new SqlParameter("@Trans_Id", Grade_Id) : new SqlParameter("@Trans_Id", DBNull.Value));

                    var totalRecordParameter = new SqlParameter("@iTotalRec", SqlDbType.Int);
                    totalRecordParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(totalRecordParameter);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                    totalRecord = Convert.ToInt32(totalRecordParameter.Value);
                }
            }
            return (result, totalRecord);
        }
        public async Task<Grade_Master> Get_Grade_Detail(int Grade_Id)
        {
            var trans_Id = Grade_Id > 0 ? new SqlParameter("@Trans_Id", Grade_Id) : new SqlParameter("@Trans_Id", DBNull.Value);
            var totalRecordParameter = new SqlParameter("@iTotalRec", SqlDbType.Int);
            totalRecordParameter.Direction = ParameterDirection.Output;

            var result = await Task.Run(() => _dbContext.Grade_Master
            .FromSqlRaw(@"EXEC [dbo].[Grade_Master_Select] @Trans_Id, @iTotalRec", trans_Id, totalRecordParameter)
            .AsEnumerable()
            .FirstOrDefault());

            if (result != null)
            {
                if (Grade_Id > 0)
                {
                    result.Grade_Detail_List = await Task.Run(() => _dbContext.Grade_Detail
                            .FromSqlRaw(@"EXEC [dbo].[Grade_Detail_Select] @Trans_Id", trans_Id).ToList());
                }
            }
            return result;
        }
        public async Task<(string, int)> Set_Grade_Master(Grade_Master model, DataTable dataTable, int user_Id)
        {
            var transId = new SqlParameter("@Trans_Id", model.Trans_Id);
            var grade = new SqlParameter("@Grade", model.Grade);
            var grade_type = new SqlParameter("@Grade_Type", model.Grade_Type);
            var userId = (user_Id > 0) ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var gradeDetail = new SqlParameter("@Grade_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[Grade_Detail_Table_Type]",
                Value = dataTable
            };

            var insertedId = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var party_Exists = new SqlParameter("@Party_Exists", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [dbo].[Grade_Master_Insert_Update]
                            @Trans_Id, @Grade, @Grade_Type, @User_Id,
                            @Grade_Detail_Table_Type,
                            @Inserted_Id OUT, @Party_Exists OUT",
                            transId, grade, grade_type, userId,
                            gradeDetail,
                            insertedId, party_Exists));

            var _party_exists = (bool)party_Exists.Value;
            if (_party_exists)
                return ("_grade_exists", 0);

            if (result > 0)
            {
                int _insertedId = (int)insertedId.Value;
                return ("success", _insertedId);
            }
            return ("error", 0);
        }
        public async Task<(string, int)> Delete_Grade_Master(int Grade_Id)
        {
            var isReferencedParameter = new SqlParameter("@IsReference", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC Grade_Master_Delete @Trans_Id, @IsReference OUT",
                                        new SqlParameter("@Trans_Id", Grade_Id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return ("_reference_found", (int)HttpStatusCode.Conflict);
            return ("success", result);
        }
        #endregion

        #region QC Master / Detail
        public async Task<IList<QC_Master>> Get_QC_Master()
        {
            var result = await Task.Run(() => _dbContext.QC_Master
                          .FromSqlRaw(@"exec QC_Master_Select").ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.QC_Detail_List = await Get_QC_Detail(item.Trans_Id);
                }
            }
            return result;
        }
        public async Task<IList<QC_Detail>> Get_QC_Detail(int Trans_Id)
        {
            var _trans_Id = Trans_Id > 0 ? new SqlParameter("@Trans_Id", Trans_Id) : new SqlParameter("@Trans_Id", DBNull.Value);
            var result = await Task.Run(() => _dbContext.QC_Detail
                         .FromSqlRaw(@"exec QC_Detail_Select @Trans_Id", _trans_Id).ToListAsync());
            return result;
        }
        public async Task<int> Create_Update_QC_Master(DataTable dataTable, int user_Id)
        {
            var _userId = (user_Id > 0) ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);

            var _qcMaster = new SqlParameter("@QC_Master_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[QC_Master_Table_Type]",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [dbo].[QC_Master_Insert_Update]
                            @User_Id, @QC_Master_Table_Type",
                             _userId, _qcMaster
                            ));
            return result;
        }
        public async Task<int> Create_Update_QC_Detail(DataTable dataTable, int user_Id)
        {
            //var _userId = (user_Id > 0) ? new SqlParameter("@User_Id", user_Id) : new SqlParameter("@User_Id", DBNull.Value);
            var _qcDetail = new SqlParameter("@QC_Detail_Table_Type", SqlDbType.Structured)
            {
                TypeName = "[dbo].[QC_Detail_Table_Type]",
                Value = dataTable
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC [dbo].[QC_Detail_Insert_Update] @QC_Detail_Table_Type", _qcDetail));
            return result;
        }
        public async Task<(string, int)> Delete_QC_Master(int Trans_Id)
        {
            var isReferencedParameter = new SqlParameter("@IsReference", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await _dbContext.Database.ExecuteSqlRawAsync("EXEC QC_Master_Delete @Trans_Id, @IsReference OUT",
                                        new SqlParameter("@Trans_Id", Trans_Id),
                                        isReferencedParameter);

            var isReferenced = (bool)isReferencedParameter.Value;
            if (isReferenced)
                return ("_reference_found", (int)HttpStatusCode.Conflict);
            return ("success", result);
        }
        #endregion

        #region QC Pricing Skip
        public async Task<List<Dictionary<string, object>>> Get_Supplier_With_Pending_Upcoming_QC_Pricing()
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Party_Master_With_Pending_Upcoming_QC_Pricing_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Purchase_Master_With_Pending_Upcoming_QC_Pricing(int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Purchase_Master_With_Pending_Upcoming_QC_Pricing_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Trans_Id > 0 ? new SqlParameter("@Trans_Id", Trans_Id) : new SqlParameter("@Trans_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<Dictionary<string, object>>> Get_Purchase_Detail_With_Pending_Upcoming_QC_Pricing(int Trans_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Purchase_Detail_With_Pending_Upcoming_QC_Pricing_Select]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(Trans_Id > 0 ? new SqlParameter("@Trans_Id", Trans_Id) : new SqlParameter("@Trans_Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<DataTable> Get_Purchase_Detail_With_Pending_Upcoming_QC_Pricing_Excel(string Id)
        {
            var dataTable = new DataTable();

            var connectionString = _configuration["ConnectionStrings:AstuteConnection"];
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("Purchase_Detail_With_Pending_Upcoming_QC_Pricing_Select_Excel", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(!string.IsNullOrEmpty(Id) ? new SqlParameter("@Id", Id) : new SqlParameter("@Id", DBNull.Value));

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }
        #endregion

        #region Stone Trace Report
        public async Task<Dictionary<string, object>> Get_Stone_Trace_Master_Report(string id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Stone_Trace_Master_Report]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(!string.IsNullOrEmpty(id) ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result.FirstOrDefault();
        }
        public async Task<List<Dictionary<string, object>>> Get_Stone_Trace_Detail_Report(string id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("[dbo].[Stone_Trace_Detail_Report]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(!string.IsNullOrEmpty(id) ? new SqlParameter("@Id", id) : new SqlParameter("@Id", DBNull.Value));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dict = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                dict[columnName] = columnValue == DBNull.Value ? null : columnValue;
                            }

                            result.Add(dict);
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}