using astute.CoreServices;
using astute.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            var comment = !string.IsNullOrEmpty(supplier_Pricing.Comment) ? new SqlParameter("@Comment", supplier_Pricing.Comment) : new SqlParameter("@Comment", DBNull.Value);
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
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Table_Black, @Side_Black, @Table_White, @Side_white, @Comment, @Cert_Type, @Table_Open, @Crown_Open, @Pavilion_Open,
                        @Girdle_Open, @Base_Disc_From, @Base_Disc_To, @Base_Amount_From, @Base_Amount_To, @Company, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4,
                        @SP_Calculation_Type, @SP_Sign, @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type,
                        @MS_Sign, @MS_Value_1, @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time,
                        @MS_SP_Value_1, @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4, @SP_Toggle_Bar, @MS_SP_Toggle_Bar, @Modified_By",
            empId, ipaddress, date, time, record_Type, supplier_Pricing_Id, supplier_Id, sunrise_Pricing_Id, customer_Pricing_Id, user_Pricing_Id, map_Flag, shape, cts, color, fancy_Color, clarity, cut, polish, symm, fls_Intensity, lab, shade, luster, bgm, culet, location, status, good_Type, length_From, length_To, width_From,
            width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
            pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, table_Black, side_Black, table_White, side_white, comment, cert_Type, table_Open, crown_Open, pavilion_Open, girdle_Open,
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

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Value_Config_Insert_Update @ValueMap_ID, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From, @Depth_Per_To, @Table_Per_From,
                        @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Lr_Half_From, @Lr_Half_To, @Star_Ln_From, @Star_Ln_To, @Shape_Group, @Shape", valueMap_ID, length_From, length_To,
                        width_From, width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
                        pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, lr_Half_From, lr_Half_To, star_Ln_From, star_Ln_To, shape_Group, shape));

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
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Pricing(int supplier_Pricing_Id, int supplier_Id, string supplier_Filter_Type, string map_Flag, int sunrise_pricing_Id, int customer_pricing_Id)
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
            var comment = !string.IsNullOrEmpty(supplier_Pricing.Comment) ? new SqlParameter("@Comment", supplier_Pricing.Comment) : new SqlParameter("@Comment", DBNull.Value);
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
            var query_Flag = !string.IsNullOrEmpty(supplier_Pricing.Query_Flag) ? new SqlParameter("@Query_Flag", supplier_Pricing.Query_Flag) : new SqlParameter("@Query_Flag", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Supplier_Pricing_Insert_Update @Supplier_Pricing_Id, @Supplier_Id, @Sunrise_Pricing_Id, @Customer_Pricing_Id, @User_Pricing_Id, @Map_Flag, @Shape, @Cts, @Color, @Fancy_Color, @Clarity, @Cut, @Polish, @Symm,
                        @Fls_Intensity, @Lab, @Shade, @Luster, @Bgm, @Culet, @Location, @Status, @Good_Type, @Length_From, @Length_To, @Width_From, @Width_To, @Depth_From, @Depth_To, @Depth_Per_From,
                        @Depth_Per_To, @Table_Per_From, @Table_Per_To, @Crown_Angle_From, @Crown_Angle_To, @Crown_Height_From, @Crown_Height_To, @Pavilion_Angle_From, @Pavilion_Angle_To, @Pavilion_Height_From,
                        @Pavilion_Height_To, @Girdle_Per_From, @Girdle_Per_To, @Table_Black, @Side_Black, @Table_White, @Side_white, @Comment, @Cert_Type, @Table_Open, @Crown_Open, @Pavilion_Open,
                        @Girdle_Open, @Base_Disc_From, @Base_Disc_To, @Base_Amount_From, @Base_Amount_To, @Company, @Supplier_Filter_Type, @Calculation_Type, @Sign, @Value_1, @Value_2, @Value_3, @Value_4,
                        @SP_Calculation_Type, @SP_Sign, @SP_Start_Date, @SP_Start_Time, @SP_End_Date, @SP_End_Time, @SP_Value_1, @SP_Value_2, @SP_Value_3, @SP_Value_4, @MS_Calculation_Type,
                        @MS_Sign, @MS_Value_1, @MS_Value_2, @MS_Value_3, @MS_Value_4, @MS_SP_Calculation_Type, @MS_SP_Sign, @MS_SP_Start_Date, @MS_SP_Start_Time, @MS_SP_End_Date, @MS_SP_End_Time,
                        @MS_SP_Value_1, @MS_SP_Value_2, @MS_SP_Value_3, @MS_SP_Value_4, @SP_Toggle_Bar, @MS_SP_Toggle_Bar, @Modified_By, @Query_Flag, @Inserted_Id OUT",
                        supplier_Pricing_Id, supplier_Id, sunrise_Pricing_Id, customer_Pricing_Id, user_Pricing_Id, map_Flag, shape, cts, color, fancy_Color, clarity, cut, polish, symm, fls_Intensity, lab, shade, luster, bgm, culet, location, status, good_Type, length_From, length_To, width_From,
                        width_To, depth_From, depth_To, depth_Per_From, depth_Per_To, table_Per_From, table_Per_To, crown_Angle_From, crown_Angle_To, crown_Height_From, crown_Height_To, pavilion_Angle_From,
                        pavilion_Angle_To, pavilion_Height_From, pavilion_Height_To, girdle_Per_From, girdle_Per_To, table_Black, side_Black, table_White, side_white, comment, cert_Type, table_Open, crown_Open, pavilion_Open, girdle_Open,
                        base_Disc_From, base_Disc_To, base_Amount_From, base_Amount_To, company, supplier_Filter_Type, calculation_Type, sign, value_1, value_2, value_3, value_4, sp_calculation_Type, sp_sign, sp_start_date,
                        sp_start_time, sp_end_date, sp_end_time, sp_value_1, sp_value_2, sp_value_3, sp_value_4, ms_calculation_Type, ms_sign, ms_value_1, ms_value_2, ms_value_3, ms_value_4, ms_sp_calculation_Type,
                        ms_sp_sign, ms_sp_start_date, ms_sp_start_time, ms_sp_end_date, ms_sp_end_time, ms_sp_value_1, ms_sp_value_2, ms_sp_value_3, ms_sp_value_4, sP_Toggle_Bar, mSP_Toggle_Bar, modified_By, query_Flag, inserted_Id));
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
            var _supplier_Id = supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            return await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"Supplier_Pricing_Delete @Supplier_Pricing_Id, @Supplier_Id", _supplier_Pricing_Id, _supplier_Id));
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
        public async Task<int> Delete_Customer_Pricing(int customer_Pricing_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Customer_Pricing_Delete {customer_Pricing_Id}"));
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
        public async Task<int> Delete_Supplier_Pricing_Key_To_Symbol(int supplier_Pricing_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Supplier_Pricing_Key_To_Symbol_Delete {supplier_Pricing_Id}"));
        }
        #endregion

        #region Supplier Stock
        public async Task<(string, int)> Stock_Data_Insert_Update(Stock_Data_Master stock_Data_Master)
        {
            var stock_Data_Id = new SqlParameter("@Stock_Data_Id", stock_Data_Master.Stock_Data_Id);
            var supplier_Id = stock_Data_Master.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Data_Master.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var upload_Method = !string.IsNullOrEmpty(stock_Data_Master.Upload_Method) ? new SqlParameter("@Upload_Method", stock_Data_Master.Upload_Method) : new SqlParameter("@Upload_Method", DBNull.Value);
            var upload_Type = !string.IsNullOrEmpty(stock_Data_Master.Upload_Type) ? new SqlParameter("@Upload_Type", stock_Data_Master.Upload_Type) : new SqlParameter("@Upload_Type", DBNull.Value);
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Data_Master_Insert_Update @Stock_Data_Id, @Supplier_Id, @Upload_Method, @Upload_Type, @Inserted_Id OUT",
                        stock_Data_Id, supplier_Id, upload_Method, upload_Type, inserted_Id));

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
            var inserted_Id = new SqlParameter("@Inserted_Id", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Data_Master_Insert_Update @Stock_Data_Id, @Supplier_Id, @Upload_Method, @Upload_Type, @Inserted_Id OUT",
                        stock_Data_Id, supplier_Id, upload_Method, upload_Type, inserted_Id));

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

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec [Supplier_Stock_Scheduler_Insert_Update] @Supplier_Id,@Stock_Data_Id", _supplier_Id, _stock_Data_Id));

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
        public async Task<int> Add_Update_Stock_Number_Generation(Stock_Number_Generation stock_Number_Generation)
        {
            var _Id = stock_Number_Generation.Id > 0 ? new SqlParameter("@Id", stock_Number_Generation.Id) : new SqlParameter("@Id", DBNull.Value);
            var _Exc_Party_Id = !string.IsNullOrEmpty(stock_Number_Generation.Exc_Party_Id) ? new SqlParameter("@Party_Id", stock_Number_Generation.Exc_Party_Id) : new SqlParameter("@Party_Id", DBNull.Value);
            var _Pointer_Id = !string.IsNullOrEmpty(stock_Number_Generation.Pointer_Id) ? new SqlParameter("@Pointer_Id", stock_Number_Generation.Pointer_Id) : new SqlParameter("@Pointer_Id", DBNull.Value);
            var _Shape = !string.IsNullOrEmpty(stock_Number_Generation.Shape) ? new SqlParameter("@Shape", stock_Number_Generation.Shape) : new SqlParameter("@Shape", DBNull.Value);
            var _Stock_Type = !string.IsNullOrEmpty(stock_Number_Generation.Stock_Type) ? new SqlParameter("@Stock_Type", stock_Number_Generation.Stock_Type) : new SqlParameter("@Stock_Type", DBNull.Value);
            var _Front_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Front_Prefix) ? new SqlParameter("@Front_Prefix", stock_Number_Generation.Front_Prefix) : new SqlParameter("@Front_Prefix", DBNull.Value);
            var _Back_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Back_Prefix) ? new SqlParameter("@Back_Prefix", stock_Number_Generation.Back_Prefix) : new SqlParameter("@Back_Prefix", DBNull.Value);
            var _Front_Prefix_Alloted = !string.IsNullOrEmpty(stock_Number_Generation.Front_Prefix_Alloted) ? new SqlParameter("@Front_Prefix_Alloted", stock_Number_Generation.Front_Prefix_Alloted) : new SqlParameter("@Front_Prefix_Alloted", DBNull.Value);
            var _Start_Format = !string.IsNullOrEmpty(stock_Number_Generation.Start_Format) ? new SqlParameter("@Start_Format", stock_Number_Generation.Start_Format) : new SqlParameter("@Start_Format", DBNull.Value);
            var _End_Format = !string.IsNullOrEmpty(stock_Number_Generation.End_Format) ? new SqlParameter("@End_Format", stock_Number_Generation.End_Format) : new SqlParameter("@End_Format", DBNull.Value);
            var _Start_Number = stock_Number_Generation.Start_Number > 0 ? new SqlParameter("@Start_Number", stock_Number_Generation.Start_Number) : new SqlParameter("@Start_Number", DBNull.Value);
            var _End_Number = stock_Number_Generation.End_Number > 0 ? new SqlParameter("@End_Number", stock_Number_Generation.End_Number) : new SqlParameter("@End_Number", DBNull.Value);
            var _Live_Prefix = !string.IsNullOrEmpty(stock_Number_Generation.Live_Prefix) ? new SqlParameter("@Live_Prefix", stock_Number_Generation.Live_Prefix) : new SqlParameter("@Live_Prefix", DBNull.Value);
            var _Supplier_Id = stock_Number_Generation.Supplier_Id > 0 ? new SqlParameter("@Supplier_Id", stock_Number_Generation.Supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value);
            var isExist = new SqlParameter("@IsExist", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Stock_Number_Generation_Insert_Update @Id, @Party_Id, @Pointer_Id, @Shape, @Stock_Type, @Front_Prefix, @Back_Prefix, @Front_Prefix_Alloted, @Start_Format, @End_Format, @Start_Number, @End_Number, @Live_Prefix, @Supplier_Id, @IsExist OUT",
                        _Id, _Exc_Party_Id, _Pointer_Id, _Shape, _Stock_Type, _Front_Prefix, _Back_Prefix, _Front_Prefix_Alloted, _Start_Format, _End_Format, _Start_Number, _End_Number, _Live_Prefix, _Supplier_Id, isExist));

            bool _isExist = (bool)isExist.Value;
            if (_isExist)
                return 5;

            return result;
        }
        public async Task<int> Delete_Stock_Number_Generation(int Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Stock_Number_Generation_Delete {Id}"));
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
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log(string supplier_Ids, string upload_Type, string from_Date, string from_Time, string to_Date, string to_Time, bool is_Lab_Entry)
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
                    command.Parameters.Add(new SqlParameter("@Is_Last_Entry", is_Lab_Entry));
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
        public async Task<List<Dictionary<string, object>>> Get_Supplier_Stock_Error_Log_Detail(int supplier_Id)
        {
            var result = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString()))
            {
                using (var command = new SqlCommand("Supplier_Stock_Error_Log_Detail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(supplier_Id > 0 ? new SqlParameter("@Supplier_Id", supplier_Id) : new SqlParameter("@Supplier_Id", DBNull.Value));
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
