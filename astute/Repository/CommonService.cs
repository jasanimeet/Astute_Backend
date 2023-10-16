using astute.CoreModel;
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
    public partial class CommonService : ICommonService
    {
        #region Fields
        private readonly AstuteDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public CommonService(AstuteDbContext dbContext,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        #region Country Master
        public async Task<int> InsertCountry(Country_Master country_Mas)
        {
            //country_Mas.Isd_Code = "+" + country_Mas.Isd_Code;
            var countryId = new SqlParameter("@countryId", country_Mas.Country_Id);
            var country = new SqlParameter("@country", country_Mas.Country);
            var isdCode = new SqlParameter("@isdCode", country_Mas.Isd_Code);
            var orderNo = country_Mas.Order_No > 0 ? new SqlParameter("@orderNo", country_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = country_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", country_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", country_Mas.Status);
            var shortCode = !string.IsNullOrEmpty(country_Mas.Short_Code) ? new SqlParameter("@shortCode", country_Mas.Short_Code) : new SqlParameter("@shortCode", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isExistCountry = new SqlParameter("@IsExistCountry", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Country_Mas_Insert_Update @countryId, @country, @isdCode, @orderNo, @sortNo, @status, @shortCode, @recordType, @IsExistCountry OUT,
            @IsExistOrderNo OUT, @IsExistSortNo OUT", countryId, country, isdCode, orderNo, sortNo, status, shortCode, recordType,
            isExistCountry, isExistOrderNo, isExistSortNo));

            bool isExist_Country = (bool)isExistCountry.Value;
            if (isExist_Country)
                return 2;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            return result;
        }
        public async Task<int> UpdateCountry(Country_Master country_Mas)
        {
            //country_Mas.Isd_Code = "+" + country_Mas.Isd_Code;
            var countryId = new SqlParameter("@countryId", country_Mas.Country_Id);
            var country = new SqlParameter("@country", country_Mas.Country);
            var isdCode = new SqlParameter("@isdCode", country_Mas.Isd_Code);
            var orderNo = country_Mas.Order_No > 0 ? new SqlParameter("@orderNo", country_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = country_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", country_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", country_Mas.Status);
            var shortCode = !string.IsNullOrEmpty(country_Mas.Short_Code) ? new SqlParameter("@shortCode", country_Mas.Short_Code) : new SqlParameter("@shortCode", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");
            var isExistCountry = new SqlParameter("@IsExistCountry", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec Country_Mas_Insert_Update @countryId, @country, @isdCode, @orderNo, @sortNo, @status, @shortCode, @recordType, @IsExistCountry OUT,
            @IsExistOrderNo OUT, @IsExistSortNo OUT", countryId, country, isdCode, orderNo, sortNo, status, shortCode, recordType,
            isExistCountry, isExistOrderNo, isExistSortNo));

            bool isExist_Country = (bool)isExistCountry.Value;
            if (isExist_Country)
                return 2;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 3;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 4;

            return result;
        }
        public async Task<int> DeleteCountry(int countryId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Country_Mas_Delete {countryId}"));
        }
        public async Task<IList<Country_Master>> GetCountry(int country_Id, string country, string isd_Code, string short_Code)
        {
            var result = new List<Country_Master>();

            var CountryId = country_Id > 0 ? new SqlParameter("@countryId", country_Id) : new SqlParameter("@countryId", DBNull.Value);
            var CountryName = !string.IsNullOrEmpty(country) ? new SqlParameter("@countryName", country) : new SqlParameter("@countryName", DBNull.Value);
            var IsdCode = !string.IsNullOrEmpty(isd_Code) ? new SqlParameter("@isdCode", isd_Code) : new SqlParameter("@isdCode", DBNull.Value);
            var ShortCode = !string.IsNullOrEmpty(short_Code) ? new SqlParameter("@shortCode", short_Code) : new SqlParameter("@shortCode", DBNull.Value);

            result = await Task.Run(() => _dbContext.Country_Master
                 .FromSqlRaw(@"exec Country_Mas_Select @countryId, @countryName, @isdCode, @shortCode", CountryId, CountryName, IsdCode, ShortCode).ToListAsync());

            //if (result != null && result.Count > 0)
            //{
            //    foreach (var item in result)
            //    {
            //        if (!string.IsNullOrEmpty(item.Isd_Code))
            //        {
            //            item.Isd_Code = item.Isd_Code.Replace("+", "");
            //        }
            //    }
            //}

            return result;
        }

        public async Task<IList<Country_Master>> Get_Active_Country()
        {   
            var result = await Task.Run(() => _dbContext.Country_Master
                 .FromSqlRaw(@"exec Country_Mas_Active_Select").ToListAsync());

            return result;
        }
        public async Task<int> CountryChangeStatus(int country_Id, bool status)
        {
            var countryId = new SqlParameter("@Country_Id", country_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Country_Master_Update_Status @Country_Id, @Status", countryId, Status));
            return result;
        }
        #endregion

        #region State Master
        public async Task<int> InsertState(State_Master state_Mas)
        {
            var stateId = new SqlParameter("@stateId", state_Mas.State_Id);
            var state = new SqlParameter("@state", state_Mas.State);
            var countryId = new SqlParameter("@countryId", state_Mas.Country_Id);
            var orderNo = state_Mas.Order_No > 0 ? new SqlParameter("@orderNo", state_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = state_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", state_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", state_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", state_Mas.IsForceInsert);
            var isExistState = new SqlParameter("@IsExistState", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec State_Mas_Insert_Update @stateId, @state, @countryId, @orderNo, @sortNo, @status, @recordType, @IsExistState OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT,
            @IsForceInsert",
            stateId, state, countryId, orderNo, sortNo, status, recordType, isExistState, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool stateIsExist = (bool)isExistState.Value;
            if (stateIsExist)
                return 4;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> UpdateState(State_Master state_Mas)
        {
            var parameter = new List<SqlParameter>();

            var stateId = new SqlParameter("@stateId", state_Mas.State_Id);
            var state = new SqlParameter("@state", state_Mas.State);
            var countryId = new SqlParameter("@countryId", state_Mas.Country_Id);
            var orderNo = state_Mas.Order_No > 0 ? new SqlParameter("@orderNo", state_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = state_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", state_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", state_Mas.Status);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", state_Mas.IsForceInsert);
            var isExistState = new SqlParameter("@IsExistState", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec State_Mas_Insert_Update @stateId, @state, @countryId, @orderNo, @sortNo, @status, @recordType, @IsExistState OUT, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            stateId, state, countryId, orderNo, sortNo, status, recordType, isExistState, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool stateIsExist = (bool)isExistState.Value;
            if (stateIsExist)
                return 4;

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> DeleteState(int stateId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"State_Mas_Delete {stateId}"));
        }
        public async Task<IList<State_Master>> GetStates(int state_Id, string state, int country_Id)
        {
            var StateId = state_Id > 0 ? new SqlParameter("@stateId", state_Id) : new SqlParameter("@stateId", DBNull.Value);
            var State = !string.IsNullOrEmpty(state) ? new SqlParameter("@state", state) : new SqlParameter("@state", DBNull.Value);
            var CountryId = country_Id > 0 ? new SqlParameter("@countryId", country_Id) : new SqlParameter("@countryId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.State_Master
                        .FromSqlRaw(@"exec State_Mas_Select @stateId, @state, @countryId", StateId, State, CountryId).ToListAsync());

            return result;
        }

        public async Task<IList<State_Master>> Get_Active_State()
        {
            var result = await Task.Run(() => _dbContext.State_Master
                 .FromSqlRaw(@"exec State_Master_Active_Select").ToListAsync());

            return result;
        }
        public async Task<int> StateChangeStatus(int state_Id, bool status)
        {
            var stateId = new SqlParameter("@State_Id", state_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC State_Master_Update_Status @State_Id, @Status", stateId, Status));
            return result;
        }
        #endregion

        #region City Master
        public async Task<int> InsertCity(City_Master city_Mas)
        {
            var cityId = new SqlParameter("@cityId", city_Mas.City_Id);
            var city = !string.IsNullOrWhiteSpace(city_Mas.City) ? new SqlParameter("@city", city_Mas.City) : new SqlParameter("@city", DBNull.Value);
            var stateId = city_Mas.State_Id > 0 ? new SqlParameter("@stateId", city_Mas.State_Id) : new SqlParameter("@stateId", DBNull.Value);
            var orderNo = city_Mas.Order_No > 0 ? new SqlParameter("@orderNo", city_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = city_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", city_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", city_Mas.Status);
            var stdCode = !string.IsNullOrEmpty(city_Mas.Std_Code) ? new SqlParameter("@std_Code", city_Mas.Std_Code) : new SqlParameter("@std_Code", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");
            var isForce_Insert = new SqlParameter("@IsForceInsert", city_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec City_Mas_Insert_Update @cityId, @city, @stateId, @orderNo, @sortNo, @status, @std_Code, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            cityId, city, stateId, orderNo, sortNo, status, stdCode, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> UpdateCity(City_Master city_Mas)
        {
            var cityId = new SqlParameter("@cityId", city_Mas.City_Id);
            var city = !string.IsNullOrWhiteSpace(city_Mas.City) ? new SqlParameter("@city", city_Mas.City) : new SqlParameter("@city", DBNull.Value);
            var stateId = city_Mas.State_Id > 0 ? new SqlParameter("@stateId", city_Mas.State_Id) : new SqlParameter("@stateId", DBNull.Value);
            var orderNo = city_Mas.Order_No > 0 ? new SqlParameter("@orderNo", city_Mas.Order_No) : new SqlParameter("@orderNo", DBNull.Value);
            var sortNo = city_Mas.Sort_No > 0 ? new SqlParameter("@sortNo", city_Mas.Sort_No) : new SqlParameter("@sortNo", DBNull.Value);
            var status = new SqlParameter("@status", city_Mas.Status);
            var stdCode = !string.IsNullOrEmpty(city_Mas.Std_Code) ? new SqlParameter("@std_Code", city_Mas.Std_Code) : new SqlParameter("@std_Code", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");
            var isForce_Insert = new SqlParameter("@IsForceInsert", city_Mas.IsForceInsert);
            var isExistOrderNo = new SqlParameter("@IsExistOrderNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistSortNo = new SqlParameter("@IsExistSortNo", System.Data.SqlDbType.Bit)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec City_Mas_Insert_Update @cityId, @city, @stateId, @orderNo, @sortNo, @status, @std_Code, @recordType, @IsExistOrderNo OUT, @IsExistSortNo OUT, @IsForceInsert",
            cityId, city, stateId, orderNo, sortNo, status, stdCode, recordType, isExistOrderNo, isExistSortNo, isForce_Insert));

            bool orderNoIsExist = (bool)isExistOrderNo.Value;
            if (orderNoIsExist)
                return 2;

            bool sortNoIsExist = (bool)isExistSortNo.Value;
            if (sortNoIsExist)
                return 3;

            return result;
        }
        public async Task<int> DeleteCity(int cityId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"City_Mas_Delete {cityId}"));
        }
        public async Task<IList<City_Master>> GetCity(int cityId, string city, int stateId, int iPgNo, int iPgSize)
        {
            var CityId = cityId > 0 ? new SqlParameter("@cityId", cityId) : new SqlParameter("@cityId", DBNull.Value);
            var City = !string.IsNullOrEmpty(city) ? new SqlParameter("@city", city) : new SqlParameter("@city", DBNull.Value);
            var StateId = stateId > 0 ? new SqlParameter("@stateId", stateId) : new SqlParameter("@stateId", DBNull.Value);
            var pageIndex = iPgNo > 0 ? new SqlParameter("@iPgNo", iPgNo) : new SqlParameter("@iPgNo", DBNull.Value);
            var pageSize = iPgSize > 0 ? new SqlParameter("@iPgSize", iPgSize) : new SqlParameter("@iPgSize", DBNull.Value);

            var result = await Task.Run(() => _dbContext.City_Master
                           .FromSqlRaw(@"exec City_Mas_Select @cityId, @city, @stateId, @iPgNo, @iPgSize", CityId, City, StateId, pageIndex, pageSize).ToListAsync());

            return result;
        }

        public async Task<IList<City_Master>> Get_Active_Cities()
        {
            var result = await Task.Run(() => _dbContext.City_Master
                 .FromSqlRaw(@"exec City_Mas_Active_Select").ToListAsync());

            return result;
        }
        public async Task<int> CityChangeStatus(int city_Id, bool status)
        {
            var cityId = new SqlParameter("@City_Id", city_Id);
            var Status = new SqlParameter("@Status", status);

            var result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC City_Master_Update_Status @City_Id, @Status", cityId, Status));
            return result;
        }
        #endregion

        #region Error Log
        public async Task<int> InsertErrorLog(string message, string method, string stackTrace)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@Error_Message", message));
            parameter.Add(new SqlParameter("@Module_Name", method));
            parameter.Add(new SqlParameter("@Arise_Date", DateTime.UtcNow));
            parameter.Add(new SqlParameter("@Error_Trace", stackTrace));

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@" exec Error_Log_Insert @Error_Message, @Module_Name, @Arise_Date, @Error_Trace", parameter.ToArray()));

            return result;
        }
        #endregion

        #region Years
        public async Task<IList<Year_Master>> GetYear(int yearId)
        {
            var year_Id = yearId > 0 ? new SqlParameter("@YearId", yearId) : new SqlParameter("@YearId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Year_Master
                            .FromSqlRaw(@"exec Year_Mas_Select @YearId", year_Id).ToListAsync());

            return result;
        }
        public async Task<int> InsertYears(Year_Master year_Mas)
        {
            var parametar = new List<SqlParameter>();
            parametar.Add(new SqlParameter("@Year_Id", year_Mas.Year_Id));
            parametar.Add(new SqlParameter("@Year", year_Mas.Year));
            parametar.Add(new SqlParameter("@Current_Status", year_Mas.Current_Status));
            parametar.Add(new SqlParameter("@Status", year_Mas.Status));
            parametar.Add(new SqlParameter("@From_Date", year_Mas.From_Date));
            parametar.Add(new SqlParameter("@To_Date", year_Mas.To_Date));
            parametar.Add(new SqlParameter("@recordType", "Insert"));

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Year_Mas_Insert_Update @Year_Id, @Year, @Current_Status, @Status, @From_Date, @To_Date, @recordType", parametar.ToArray()));

            return result;
        }
        public async Task<int> UpdateYears(Year_Master year_Mas)
        {
            var parametar = new List<SqlParameter>();
            parametar.Add(new SqlParameter("@Year_Id", year_Mas.Year_Id));
            parametar.Add(new SqlParameter("@Year", year_Mas.Year));
            parametar.Add(new SqlParameter("@Current_Status", year_Mas.Current_Status));
            parametar.Add(new SqlParameter("@Status", year_Mas.Status));
            parametar.Add(new SqlParameter("@From_Date", year_Mas.From_Date));
            parametar.Add(new SqlParameter("@To_Date", year_Mas.To_Date));
            parametar.Add(new SqlParameter("@recordType", "Update"));

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Year_Mas_Insert_Update @Year_Id, @Year, @Current_Status, @Status, @From_Date, @To_Date, @recordType", parametar.ToArray()));

            return result;
        }
        public async Task<int> DeleteYears(int yearId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Year_Mas_Delete {yearId}"));
        }
        #endregion

        #region Quote Mas
        public async Task<Quote_Mas_Model> GetRandomQuote()
        {
            var result = await Task.Run(() => _dbContext.Quote_Mas_Model
                            .FromSqlRaw(@"exec Quote_Mas_Random_Select")
                            .AsEnumerable()
                            .FirstOrDefault());

            return result;
        }
        public async Task<IList<Quote_Master>> GetQuote(int quoteId)
        {
            var quote_Id = quoteId > 0 ? new SqlParameter("@QuoteId", quoteId) : new SqlParameter("@QuoteId", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Quote_Master
                            .FromSqlRaw(@"exec Quote_Mas_Select @Quote_Id", quote_Id).ToListAsync());

            return result;
        }
        public async Task<int> InsertQuote(Quote_Master quote_Mas)
        {
            var quoteId = quote_Mas.Quote_Id > 0 ? new SqlParameter("@Quote_Id", quote_Mas.Quote_Id) : new SqlParameter("@Quote_Id", DBNull.Value);
            var quote = !string.IsNullOrEmpty(quote_Mas.Quote) ? new SqlParameter("@Quote", quote_Mas.Quote) : new SqlParameter("@Quote", DBNull.Value);
            var author = !string.IsNullOrEmpty(quote_Mas.Author) ? new SqlParameter("@Author", quote_Mas.Author) : new SqlParameter("@Author", DBNull.Value);
            var status = new SqlParameter("@Status", quote_Mas.Status);
            var sr = quote_Mas.Sr > 0 ? new SqlParameter("@Sr", quote_Mas.Sr) : new SqlParameter("@Sr", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Quote_Mas_Insert_Update @Quote_Id, @Quote, @Author, @Status, @Sr, @recordType", quoteId, quote, author, status, sr, recordType));

            return result;
        }
        public async Task<int> UpdateQuote(Quote_Master quote_Mas)
        {
            var quoteId = quote_Mas.Quote_Id > 0 ? new SqlParameter("@Quote_Id", quote_Mas.Quote_Id) : new SqlParameter("@Quote_Id", DBNull.Value);
            var quote = !string.IsNullOrEmpty(quote_Mas.Quote) ? new SqlParameter("@Quote", quote_Mas.Quote) : new SqlParameter("@Quote", DBNull.Value);
            var author = !string.IsNullOrEmpty(quote_Mas.Author) ? new SqlParameter("@Author", quote_Mas.Author) : new SqlParameter("@Author", DBNull.Value);
            var status = new SqlParameter("@Status", quote_Mas.Status);
            var sr = quote_Mas.Sr > 0 ? new SqlParameter("@Sr", quote_Mas.Sr) : new SqlParameter("@Sr", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Update");

            var result = await Task.Run(() => _dbContext.Database
                        .ExecuteSqlRawAsync(@"EXEC Quote_Mas_Insert_Update @Quote_Id, @Quote, @Author, @Status, @Sr, @recordType", quoteId, quote, author, status, sr, recordType));

            return result;
        }
        public async Task<int> DeleteQuote(int quoteId)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Quote_Mas_Delete {quoteId}"));
        }
        #endregion

        #region Layout Master
        public async Task<IList<Layout_Master>> GetLayout(int layoutId, int menuId, int employeeId)
        {
            var layout_Id = layoutId > 0 ? new SqlParameter("@Layout_Id", layoutId) : new SqlParameter("@Layout_Id", DBNull.Value);
            var menu_Id = menuId > 0 ? new SqlParameter("@Menu_Id", menuId) : new SqlParameter("@Menu_Id", DBNull.Value);
            var employee_Id = employeeId > 0 ? new SqlParameter("@Employee_Id", employeeId) : new SqlParameter("@Employee_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Layout_Master
                             .FromSqlRaw(@"exec Layout_Master_Select @Layout_Id, @Menu_Id, @Employee_Id", layout_Id, menu_Id, employee_Id).ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Layout_Details = await GetLayout_Details(0, item.Layout_Id);
                }
            }

            return result;
        }
        public async Task<IList<Layout_Detail>> GetLayout_Details(int layoutDetailId, int layoutId)
        {
            var layout_Detail_Id = layoutDetailId > 0 ? new SqlParameter("@LayoutDet_ID", layoutDetailId) : new SqlParameter("@LayoutDet_ID", DBNull.Value);
            var layout_Id = layoutId > 0 ? new SqlParameter("@Layout_Id", layoutId) : new SqlParameter("@Layout_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Layout_Detail
                            .FromSqlRaw(@"exec Layout_Detail_Select @LayoutDet_ID, @Layout_Id", layout_Detail_Id, layout_Id).ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Layout_Columns = await GetLayout_Columns(0, item.LayoutDet_ID);
                }
            }
            return result;
        }
        public async Task<IList<Layout_Column>> GetLayout_Columns(int layoutColumnId, int layoutDetId)
        {
            var layout_Column_Id = layoutColumnId > 0 ? new SqlParameter("@Layout_Column_Id", layoutColumnId) : new SqlParameter("@Layout_Column_Id", DBNull.Value);
            var layout_Det_Id = layoutDetId > 0 ? new SqlParameter("@LayoutDet_ID", layoutDetId) : new SqlParameter("@LayoutDet_ID", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Layout_Column
                            .FromSqlRaw(@"exec Layout_Column_Select @Layout_Column_Id, @LayoutDet_ID", layout_Column_Id, layout_Det_Id).ToListAsync());
            return result;
        }
        public async Task<int> InsertLayoutMas(Layout_Master layout_Master)
        {
            int result = 0;
            var layoutId = new SqlParameter("@Layout_Id", layout_Master.Layout_Id);
            var menuId = layout_Master.Menu_id > 0 ? new SqlParameter("@Menu_id", layout_Master.Menu_id) : new SqlParameter("@Menu_id", DBNull.Value);
            var employeeId = layout_Master.Employee_id > 0 ? new SqlParameter("@Employee_id", layout_Master.Employee_id) : new SqlParameter("@Employee_id", DBNull.Value);
            var recordType = new SqlParameter("@recordType", "Insert");
            var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var isExistLayoutId = new SqlParameter("@IsExistLayoutId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Layout_Master_Insert_Update @Layout_Id, @Menu_id, @Employee_id, @recordType, @InsertedId OUT, @IsExistLayoutId OUT", layoutId,
                                menuId, employeeId, recordType, insertedId, isExistLayoutId));

            int inserted_Layout_Id = 0;
            int isExistId = (int)isExistLayoutId.Value;
            if (isExistId > 0)
                inserted_Layout_Id = isExistId;
            else
                inserted_Layout_Id = (int)insertedId.Value;

            if (layout_Master.Layout_Details != null && layout_Master.Layout_Details.Count > 0)
            {
                foreach (var item in layout_Master.Layout_Details)
                {
                    var layoutDetId = new SqlParameter("@LayoutDet_ID", item.LayoutDet_ID);
                    var layout_Id = new SqlParameter("@Layout_Id", inserted_Layout_Id);
                    var Individual_Layout_Id = !string.IsNullOrEmpty(item.Individual_Layout_Id) ? new SqlParameter("@Individual_Layout_Id", item.Individual_Layout_Id) : new SqlParameter("@Individual_Layout_Id", DBNull.Value);
                    var Individual_Layout_Name = !string.IsNullOrEmpty(item.Individual_Layout_Name) ? new SqlParameter("@Individual_Layout_Name", item.Individual_Layout_Name) : new SqlParameter("@Individual_Layout_Name", DBNull.Value);
                    var status = new SqlParameter("@Status", item.Status);
                    var record_Type = new SqlParameter("@recordType", "Insert");
                    var inserted_det_Id = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };

                    result = await Task.Run(() => _dbContext.Database
                                .ExecuteSqlRawAsync(@"EXEC Layout_Detail_Insert_Update @LayoutDet_ID, @Layout_Id, @Individual_Layout_Id, @Individual_Layout_Name, @Status, @recordType, @InsertedId OUT", layoutDetId,
                                layout_Id, Individual_Layout_Id, Individual_Layout_Name, status, record_Type, inserted_det_Id));

                    int inserted_LayoutDetail_Id = (int)inserted_det_Id.Value;

                    if (item.Layout_Columns != null && item.Layout_Columns.Count > 0)
                    {
                        foreach (var col in item.Layout_Columns)
                        {
                            var layout_Column_Id = new SqlParameter("@Layout_Column_Id", col.Layout_Column_Id);
                            var layout_Det_Id = new SqlParameter("@LayoutDet_ID", inserted_LayoutDetail_Id);
                            var column_Name = !string.IsNullOrEmpty(col.Column_Name) ? new SqlParameter("@Column_Name", col.Column_Name) : new SqlParameter("@Column_Name", DBNull.Value);
                            var type = !string.IsNullOrEmpty(col.Type) ? new SqlParameter("@Type", col.Type) : new SqlParameter("@Type", DBNull.Value);
                            var value = !string.IsNullOrEmpty(col.Value) ? new SqlParameter("@Value", col.Value) : new SqlParameter("@Value", DBNull.Value);
                            var column_status = new SqlParameter("@Status", col.Status);
                            var column_record_Type = new SqlParameter("@recordType", "Insert");

                            result = await Task.Run(() => _dbContext.Database
                                            .ExecuteSqlRawAsync(@"EXEC Layout_Column_Insert_Update @Layout_Column_Id, @LayoutDet_ID, @Column_Name, @Type, @Value, @Status,
                                            @recordType", layout_Column_Id, layout_Det_Id, column_Name, type, value, column_status, column_record_Type));
                        }
                    }
                }
            }
            return result;
        }
        public async Task<int> DeleteLayout(int menuId, int employeeId)
        {
            int result = 0;
            int layout_Id = 0;
            int layout_Detail_Id = 0;
            var layout_Master = await GetLayout(0, menuId, employeeId);
            if (layout_Master != null && layout_Master.Count > 0)
            {
                var menu_Id = new SqlParameter("@Menu_Id", menuId);
                var employee_Id = new SqlParameter("@Employee_Id", employeeId);

                var layout = layout_Master.FirstOrDefault();
                layout_Id = layout.Layout_Id;
                var layout_Detail = await GetLayout_Details(0, layout_Id);
                if (layout_Detail != null && layout_Detail.Count > 0)
                {
                    var layout_det = layout_Detail.FirstOrDefault();
                    layout_Detail_Id = layout_det.LayoutDet_ID;
                    var layout_Column = await GetLayout_Columns(0, layout_Detail_Id);
                    if (layout_Column != null && layout_Column.Count > 0)
                    {
                        result = await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Layout_Column_Delete {layout_Detail_Id}"));
                    }
                    result = await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Layout_Detail_Delete {layout_Id}"));
                }
                result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Layout_Master_Delete @Menu_Id, @Employee_Id", menu_Id, employee_Id));
            }
            return result;
        }
        public async Task<int> DeleteLayoutDetail(int layout_Detail_Id)
        {
            var result = 0;
            var layout_Detail = await GetLayout_Details(layout_Detail_Id, 0);
            if (layout_Detail != null && layout_Detail.Count > 0)
            {
                var layout_det = layout_Detail.FirstOrDefault();
                var layout_Column = await GetLayout_Columns(0, layout_det.LayoutDet_ID);
                if (layout_Column != null && layout_Column.Count > 0)
                {
                    result = await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete_Layout_Column {layout_Detail_Id}"));
                }
                result = await Task.Run(() => _dbContext.Database.ExecuteSqlRawAsync(@"EXEC Delete_Layout_Detail @Layout_Id, @LayoutDet_ID",
                    new SqlParameter("@Layout_Id", DBNull.Value), new SqlParameter("@LayoutDet_ID", layout_Detail_Id)));
            }
            return result;
        }
        public async Task<int> UpdateLayoutStatus(int layoutDetailId, int layoutId)
        {
            var layout_Detail_Id = layoutDetailId > 0 ? new SqlParameter("@LayoutDet_ID", layoutDetailId) : new SqlParameter("@LayoutDet_ID", DBNull.Value);
            var layout_Id = layoutId > 0 ? new SqlParameter("@Layout_Id", layoutId) : new SqlParameter("@Layout_Id", DBNull.Value);

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"exec Layout_Detail_Update_Status @LayoutDet_ID, @Layout_Id", layout_Detail_Id, layout_Id));
            return result;
        }
        public async Task<Layout_Detail> DefaultLayout()
        {
            var layoutDetail = new Layout_Detail();
            layoutDetail.LayoutDet_ID = 0;
            layoutDetail.Layout_Id = 0;
            layoutDetail.Individual_Layout_Id = "0";
            layoutDetail.Individual_Layout_Name = "Default";
            layoutDetail.Status = false;
            layoutDetail.Layout_Columns = new List<Layout_Column>();
            layoutDetail.Layout_Columns.Add(new Layout_Column()
            {
                Layout_Column_Id = 0,
                LayoutDet_ID = 0,
                Column_Name = "Sort",
                Type = "columnWidth",
                Value = "{}",
                Status = false
            });
            layoutDetail.Layout_Columns.Add(new Layout_Column()
            {
                Layout_Column_Id = 0,
                LayoutDet_ID = 0,
                Column_Name = "Sort",
                Type = "columnHide",
                Value = "{}",
                Status = false
            });
            layoutDetail.Layout_Columns.Add(new Layout_Column()
            {
                Layout_Column_Id = 0,
                LayoutDet_ID = 0,
                Column_Name = "Sort",
                Type = "draganddrop",
                Value = "['mrt-row-select']",
                Status = false
            });
            layoutDetail.Layout_Columns.Add(new Layout_Column()
            {
                Layout_Column_Id = 0,
                LayoutDet_ID = 0,
                Column_Name = "Sort",
                Type = "sort",
                Value = "[{ id: 'Order_No', desc: false }]",
                Status = false
            });
            layoutDetail.Layout_Columns.Add(new Layout_Column()
            {
                Layout_Column_Id = 0,
                LayoutDet_ID = 0,
                Column_Name = "Sort",
                Type = "columnPin",
                Value = "{ left: ['mrt-row-select'], right: ['Action'] }",
                Status = false
            });
            return layoutDetail;
        }

        //public async Task<int> UpdateLayoutMas(Layout_Master layout_Master)
        //{
        //    int result = 0;
        //    var layoutId = new SqlParameter("@Layout_Id", layout_Master.Layout_Id);
        //    var menuId = layout_Master.Menu_id > 0 ? new SqlParameter("@Menu_id", layout_Master.Menu_id) : new SqlParameter("@Menu_id", DBNull.Value);
        //    var employeeId = layout_Master.Employee_id > 0 ? new SqlParameter("@Employee_id", layout_Master.Employee_id) : new SqlParameter("@Employee_id", DBNull.Value);
        //    var layout_Type = !string.IsNullOrEmpty(layout_Master.Layout_Type) ? new SqlParameter("@Layout_Type", layout_Master.Layout_Type) : new SqlParameter("@Layout_Type", DBNull.Value);
        //    var recordType = new SqlParameter("@recordType", "Update");
        //    var insertedId = new SqlParameter("@InsertedId", System.Data.SqlDbType.Int)
        //    {
        //        Direction = System.Data.ParameterDirection.Output
        //    };

        //    result = await Task.Run(() => _dbContext.Database
        //                        .ExecuteSqlRawAsync(@"EXEC Layout_Master_Insert_Update @Layout_Id, @Menu_id, @Employee_id, @Layout_Type, @recordType, @InsertedId OUT", layoutId,
        //                        menuId, employeeId, layout_Type, recordType, insertedId));
        //    //int inserted_Layout_Id = (int)insertedId.Value;

        //    if (layout_Master.Layout_Details != null && layout_Master.Layout_Details.Count > 0)
        //    {
        //        foreach (var item in layout_Master.Layout_Details)
        //        {
        //            var layoutDetId = new SqlParameter("@LayoutDet_ID", item.LayoutDet_ID);
        //            var layout_Id = layoutId; //new SqlParameter("@Layout_Id", layoutId);
        //            var columnName = !string.IsNullOrEmpty(item.Column_Name) ? new SqlParameter("@Column_Name", item.Column_Name) : new SqlParameter("@Column_Name", DBNull.Value);
        //            var value = !string.IsNullOrEmpty(item.Value) ? new SqlParameter("@Value", item.Value) : new SqlParameter("@Value", DBNull.Value);
        //            var status = new SqlParameter("@Status", item.Status);
        //            var record_Type = new SqlParameter("@recordType", "Update");

        //            result = await Task.Run(() => _dbContext.Database
        //                        .ExecuteSqlRawAsync(@"EXEC Layout_Detail_Insert_Update @LayoutDet_ID, @Layout_Id, @Column_Name, @Value, @Status, @recordType", layoutDetId,
        //                        layout_Id, columnName, value, status, record_Type));
        //        }
        //    }
        //    return result;
        //}
        #endregion

        #region Loader Master
        public async Task<int> InsertLoader(Loader_Master loader_Master)
        {
            var loader_Image = new SqlParameter("@Loader_Img", loader_Master.Loader_Img);
            var default_Loader = new SqlParameter("@Default_Loader", loader_Master.Default_Loader);

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Loader_Master_Insert @Loader_Img, @Default_Loader", loader_Image, default_Loader));
            return result;
        }
        public async Task<int> Set_Default_Loader(int employee_Id, int loader_Id, bool default_Loader)
        {
            var _employee_Id = employee_Id > 0 ? new SqlParameter("@Employee_Id", employee_Id) : new SqlParameter("@Employee_Id", DBNull.Value);
            var _loader_Id = loader_Id > 0 ? new SqlParameter("@Loader_Id", loader_Id) : new SqlParameter("@Loader_Id", DBNull.Value);
            var _default_Loader = new SqlParameter("@Default_Loader", default_Loader);

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Loader_Master_Set_Default_Loader @Employee_Id, @Loader_Id, @Default_Loader", _employee_Id, _loader_Id, _default_Loader));
            return result;
        }
        public async Task<int> DeleteLoader(int loader_Id)
        {
            return await Task.Run(() => _dbContext.Database.ExecuteSqlInterpolatedAsync($"Loader_Master_Delete {loader_Id}"));
        }
        public async Task<IList<Loader_Master>> GetLoader()
        {
            var result = await Task.Run(() => _dbContext.Loader_Master
                            .FromSqlRaw(@"exec Loader_Master_Select")
                            .ToListAsync());
            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.Loader_Img = !string.IsNullOrEmpty(item.Loader_Img) ? _configuration["BaseUrl"] + CoreCommonFilePath.LoaderImagesPath + item.Loader_Img : null;
                }
            }

            return result;
        }
        public async Task<int> Add_Employee_Loader(Employee_Loader employee_Loader)
        {
            var _employee_Id = new SqlParameter("@Employee_Id", employee_Loader.Employee_Id);
            var _loader_Id = new SqlParameter("@Loader_Id", employee_Loader.Loader_Id);
            var status = new SqlParameter("@Status", employee_Loader.Status);

            var result = await Task.Run(() => _dbContext.Database
                            .ExecuteSqlRawAsync(@"EXEC Employee_Loader_Set @Employee_Id, @Loader_Id, @Status", _employee_Id, _loader_Id, status));
            return result;
        }
        public async Task<IList<Loader_Master>> Get_Employee_Loader(int employee_Id)
        {
            var _employee_Id = new SqlParameter("@Employee_Id", employee_Id);

            var employee_loader = await Task.Run(() => _dbContext.Employee_Loader
                                .FromSqlRaw(@"exec Employee_Loader_Select @Employee_Id", _employee_Id)
                                .AsEnumerable()
                                .FirstOrDefault());

            var result = await Task.Run(() => _dbContext.Loader_Master
                            .FromSqlRaw(@"exec Loader_Master_Select")
                            .ToListAsync());

            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    if (employee_loader != null)
                    {
                        if (item.Loader_Id == employee_loader.Loader_Id)
                            item.IsEmployee_Loader = true;
                        else
                            item.IsEmployee_Loader = false;
                    }
                    item.Loader_Img = !string.IsNullOrEmpty(item.Loader_Img) ? _configuration["BaseUrl"] + CoreCommonFilePath.LoaderImagesPath + item.Loader_Img : null;
                }
            }
            return result;
        }
        #endregion

        public async Task<DataTable> Get_Stock()
        {
            using var con = new SqlConnection(_configuration["ConnectionStrings:AstuteConnection"].ToString());
            using var cmd = new SqlCommand("sunrise.IPD_Search_Diamonds_Full_stock", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_for_usercode", 1);
            cmd.Parameters.AddWithValue("@p_for_page", 1);
            cmd.Parameters.AddWithValue("@UsedFor", "Download");
            cmd.Parameters.AddWithValue("@p_for_ActivityType", "Excel Export");
            cmd.Parameters.AddWithValue("@p_for_FormName", "Search Stock");

            con.Open();

            using var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            using var ds = new DataSet();
            da.Fill(ds);

            return ds.Tables[ds.Tables.Count - 1];

            //var p_for_usercode = new SqlParameter("@p_for_usercode", 1);
            //var p_for_page = new SqlParameter("@p_for_page", 1);
            //var UsedFor = new SqlParameter("@UsedFor", "Download");
            //var p_for_ActivityType = new SqlParameter("@p_for_ActivityType", "Excel Export");
            //var p_for_FormName = new SqlParameter("@p_for_FormName", "Search Stock");

            //var result = await Task.Run(() => _dbContext.Temp_Stock
            //    .FromSqlRaw(@"exec sunrise.IPD_Search_Diamonds_Full_stock @p_for_usercode, @p_for_page, @UsedFor, @p_for_ActivityType, @p_for_FormName", p_for_usercode, p_for_page, UsedFor, p_for_ActivityType, p_for_FormName).ToListAsync());
            //return result;
        }
        #endregion
    }
}
