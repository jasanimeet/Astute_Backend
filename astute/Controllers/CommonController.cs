using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        #region Fields
        private readonly ICommonService _commonService;
        private readonly ITermsService _termsService;
        private readonly IProcessService _processService;
        private readonly ICurrencyService _currencyService;
        private readonly IPointerService _pointerService;
        private readonly IBGMService _bGMService;
        private readonly IBankService _bankService;
        private readonly IInvoiceRemarksService _invoiceRemarksService;
        private readonly IHolidayService _holidayService;
        private readonly ITermsAndConditionService _termsAndConditionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IExchange_Rate_Service _exchange_Rate_Service;
        #endregion

        #region Ctor
        public CommonController(ICommonService commonService,
            ITermsService termsService,
            IProcessService processService,
            ICurrencyService currencyService,
            IPointerService pointerService,
            IBGMService bGMService,
            IBankService bankService,
            IInvoiceRemarksService invoiceRemarksService,
            IHolidayService holidayService,
            ITermsAndConditionService termsAndConditionService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IExchange_Rate_Service exchange_Rate_Service)
        {
            _commonService = commonService;
            _termsService = termsService;
            _processService = processService;
            _currencyService = currencyService;
            _pointerService = pointerService;
            _bGMService = bGMService;
            _bankService = bankService;
            _invoiceRemarksService = invoiceRemarksService;
            _holidayService = holidayService;
            _termsAndConditionService = termsAndConditionService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _exchange_Rate_Service = exchange_Rate_Service;

        }
        #endregion

        #region Methods
        #region Country Master
        [HttpGet]
        [Route("getcountries")]
        [Authorize]
        public async Task<IActionResult> GetCountries(int country_Id, string country, string isd_Code, string short_Code)
        {
            try
            {
                //if (!string.IsNullOrEmpty(isd_Code))
                //{
                //    isd_Code = "+" + isd_Code;
                //}
                var result = await _commonService.GetCountry(country_Id, country, isd_Code, short_Code);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NotFound(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = CoreCommonMessage.DataNotFound
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetCountries", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_countries")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Countries()
        {
            try
            {
                var result = await _commonService.Get_Active_Country();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NotFound(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = CoreCommonMessage.DataNotFound
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Countries", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createcountry")]
        [Authorize]
        public async Task<IActionResult> CreateCountry(Country_Master country_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertCountry(country_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CountryCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CountryExists
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateCountry", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatecountry")]
        [Authorize]
        public async Task<IActionResult> UpdateCountry(Country_Master country_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.UpdateCountry(country_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CountryUpdated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CountryExists
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateCountry", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletecountry")]
        [Authorize]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            try
            {
                if (countryId > 0)
                {
                    var result = await _commonService.DeleteCountry(countryId);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CountryDeleted
                        });
                    }
                }
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatuscountry")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusCountry(int country_Id, bool status)
        {
            try
            {
                var result = await _commonService.CountryChangeStatus(country_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusCountry", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_country_max_order_no")]
        public async Task<IActionResult> Get_Country_Max_Order_No()
        {
            var result = await _commonService.Get_Country_Master_Max_Order_No();

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                order_no = result
            });
        }
        #endregion

        #region State Master
        [HttpGet]
        [Route("getstates")]
        [Authorize]
        public async Task<IActionResult> GetStates(int state_Id, string state, int country_Id)
        {
            try
            {
                var result = await _commonService.GetStates(state_Id, state, country_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NotFound(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = CoreCommonMessage.DataNotFound
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetStates", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_states")]
        [Authorize]
        public async Task<IActionResult> Get_Active_States()
        {
            try
            {
                var result = await _commonService.Get_Active_State();
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NotFound(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    message = CoreCommonMessage.DataNotFound
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_States", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createstate")]
        [Authorize]
        public async Task<IActionResult> CreateState(State_Master state_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertState(state_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.StateCreated
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.StateExists
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateState", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatestate")]
        [Authorize]
        public async Task<IActionResult> UpdateState(State_Master state_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (state_Mas.State_Id > 0)
                    {
                        var result = await _commonService.UpdateState(state_Mas);
                        if (result == 1)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.StateUpdated
                            });
                        }
                        else if (result == 4)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.StateExists
                            });
                        }
                        else if (result == 2)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.OrderNoAlreadyExist
                            });
                        }
                        else if (result == 3)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.SortNoAlreadyExist
                            });
                        }
                    }
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateState", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletestate")]
        [Authorize]
        public async Task<IActionResult> DeleteState(int stateId)
        {
            try
            {
                if (stateId > 0)
                {
                    var result = await _commonService.DeleteState(stateId);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.StateDeleted
                        });
                    }
                }
                return BadRequest(HttpStatusCode.BadRequest);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatusstate")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusState(int state_Id, bool status)
        {
            try
            {
                var result = await _commonService.StateChangeStatus(state_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusState", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_state_max_order_no")]
        public async Task<IActionResult> Get_State_Max_Order_No()
        {
            var result = await _commonService.Get_State_Master_Max_Order_No();

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                order_no = result
            });
        }
        #endregion

        #region City Master
        [HttpGet]
        [Route("getcities")]
        [Authorize]
        public async Task<IActionResult> GetCities(int cityId, int stateId, string city, string state, string country, string std_code, int order_no, string common_search, int iPgNo, int iPgSize)
        {
            try
            {
                var result = await _commonService.GetCity(cityId, stateId, city, state, country, std_code, order_no, common_search, iPgNo, iPgSize);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result,
                        total_Records = result.Select(x => x.iTotalRec).FirstOrDefault()
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetCities", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_cities")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Cities(string city)
        {
            try
            {
                var result = await _commonService.Get_Active_Cities(city);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Cities", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createcity")]
        [Authorize]
        public async Task<IActionResult> CreateCity(City_Master city_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertCity(city_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CityCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateCity", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatecity")]
        [Authorize]
        public async Task<IActionResult> UpdateCity(City_Master city_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (city_Mas.City_Id > 0)
                    {
                        var result = await _commonService.UpdateCity(city_Mas);
                        if (result == 1)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.CityUpdated
                            });
                        }
                        else if (result == 2)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.OrderNoAlreadyExist
                            });
                        }
                        else if (result == 3)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.SortNoAlreadyExist
                            });
                        }
                    }
                    return BadRequest(HttpStatusCode.BadRequest);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateCity", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletecity")]
        [Authorize]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            try
            {
                if (cityId > 0)
                {
                    var result = await _commonService.DeleteCity(cityId);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CityDeleted
                        });
                    }
                }
                return BadRequest(HttpStatusCode.BadRequest);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatuscity")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusCity(int city_Id, bool status)
        {
            try
            {
                var result = await _commonService.CityChangeStatus(city_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusCity", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_city_max_order_no")]
        [Authorize]
        public async Task<IActionResult> Get_City_Max_Order_No()
        {
            var result = await _commonService.Get_City_Master_Max_Order_No();

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                order_no = result
            });
        }

        [HttpGet]
        [Route("export_city_excel")]
        [Authorize]
        public async Task<IActionResult> Export_City_Excel()
        {   
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Files/CityFiles");
            if (!(Directory.Exists(folder)))
            {
                Directory.CreateDirectory(folder);
            }
            string excelName = $"City_List-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            string downloadUrl = _configuration["BaseUrl"] + CoreCommonFilePath.CityFilesPath + excelName;
            FileInfo file = new FileInfo(Path.Combine(folder, excelName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(folder, excelName));
            }

            // query data from database  
            await Task.Yield();

            var list = await _commonService.Get_Cities_Export();

            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("City");
                workSheet.Cells.LoadFromCollection(list, true);

                // Get the header row range
                var headerRow = workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column];
                // Apply bold style to the header row
                headerRow.Style.Font.Bold = true;

                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                package.Save();
            }

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                result = downloadUrl
            });
        }

        [HttpGet]
        [Route("get_city_combo")]
        [Authorize]
        public async Task<IActionResult> Get_City_Combo(string city)
        {
            try
            {
                var result = await _commonService.Get_City_Master_Combo(city);
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
                await _commonService.InsertErrorLog(ex.Message, "get_city_combo", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Terms Master
        [HttpGet]
        [Route("getterms")]
        [Authorize]
        public async Task<IActionResult> GetTerms(int terms_Id)
        {
            try
            {
                var result = await _termsService.GetTerms(terms_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "GetTerms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_terms")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Terms(int terms_Id)
        {
            try
            {
                var result = await _termsService.Get_Active_Terms(terms_Id);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Terms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createterms")]
        [Authorize]
        public async Task<IActionResult> CreateTerms(Terms_Master terms_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _termsService.InsertTerms(terms_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.TermsCreated
                        });
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistTerms
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateTerms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateterms")]
        [Authorize]
        public async Task<IActionResult> UpdateTerms(Terms_Master terms_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _termsService.UpdateTerms(terms_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.TermsUpdated
                        });
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistTerms
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateTerms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteterms")]
        [Authorize]
        public async Task<IActionResult> DeleteTerms(int terms_Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _termsService.DeleteTerms(terms_Id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.TermsDeleted
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteTerms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatusterms")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusTerms(int terms_Id, bool status)
        {
            try
            {
                var result = await _termsService.TermsChangeStatus(terms_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusTerms", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Years Master
        [HttpGet]
        [Route("getyears")]
        [Authorize]
        public async Task<IActionResult> GetYears(int yearId)
        {
            try
            {
                var result = await _commonService.GetYear(yearId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetYears", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createyear")]
        [Authorize]
        public async Task<IActionResult> CreateYear(Year_Master year_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertYears(year_Mas);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.YearMasterCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateYear", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateyear")]
        [Authorize]
        public async Task<IActionResult> UpdateYear(Year_Master year_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.UpdateYears(year_Mas);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.YearMasterUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateYear", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteyear")]
        [Authorize]
        public async Task<IActionResult> DeleteYear(int yearId)
        {
            try
            {
                var result = await _commonService.DeleteYears(yearId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.YearMasterDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched,
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteYear", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Quote Mas
        [HttpGet]
        [Route("getrandomquote")]
        public async Task<IActionResult> GetRandomQuote()
        {
            try
            {   
                var result = await _commonService.GetRandomQuote();
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = result
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetRandomQuote", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("getquote")]
        [Authorize]
        public async Task<IActionResult> GetQuote(int quoteId)
        {
            try
            {
                var result = await _commonService.GetQuote(quoteId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetQuote", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createquote")]
        [Authorize]
        public async Task<IActionResult> CreateQuote(Quote_Master quote_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertQuote(quote_Mas);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.QuoteMasterCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateQuote", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatequote")]
        [Authorize]
        public async Task<IActionResult> UpdateQuote(Quote_Master quote_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.UpdateQuote(quote_Mas);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.QuoteMasterUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateQuote", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletequote")]
        [Authorize]
        public async Task<IActionResult> DeleteQuote(int quoteId)
        {
            try
            {
                var result = await _commonService.DeleteQuote(quoteId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.QuoteMasterDeleted
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched,
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteQuote", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Process Master
        [HttpGet]
        [Route("getprocess")]
        [Authorize]
        public async Task<IActionResult> GetProcess(int processId)
        {
            try
            {
                var result = await _processService.GetProcess(processId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetProcess", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createprocessmas")]
        [Authorize]
        public async Task<IActionResult> CreateProcessMas(Process_Master process_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _processService.InsertProcessMas(process_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ProccessMasterCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateProcessMas", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateprocessmas")]
        [Authorize]
        public async Task<IActionResult> UpdateProcessMas(Process_Master process_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _processService.UpdateProcessMas(process_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ProccessMasterUpdated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateProcessMas", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteprocessmas")]
        [Authorize]
        public async Task<IActionResult> DeleteProcessMas(int processId)
        {
            try
            {
                if (processId > 0)
                {
                    var result = await _processService.DeleteProcessMas(processId);
                    if (result > 0)
                    {
                        return Ok(new
                        {

                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ProccessMasterDeleted
                        });
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatusprocess")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusProcess(int process_Id, bool status)
        {
            try
            {
                var result = await _processService.StateChangeStatus(process_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusProcess", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Currency Master
        [HttpGet]
        [Route("getcurrency")]
        [Authorize]
        public async Task<IActionResult> GetCurrency(int currency_Id)
        {
            try
            {
                var result = await _currencyService.GetCurrency(currency_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "GetCurrency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_currency")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Currency(int currency_Id)
        {
            try
            {
                var result = await _currencyService.Get_Active_Currency(currency_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Currency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createcurrency")]
        [Authorize]
        public async Task<IActionResult> CreateCurrency(Currency_Master currency_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _currencyService.InsertCurrency(currency_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CurrencyCreated
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CurrencyExists
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateCurrency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatecurrency")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrency(Currency_Master currency_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _currencyService.UpdateCurrency(currency_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CurrencyUpdated
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CurrencyExists
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateCurrency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletecurrency")]
        [Authorize]
        public async Task<IActionResult> DeleteCurrency(int currency_Id)
        {
            try
            {

                var result = await _currencyService.DeleteCurrency(currency_Id);
                if (result == 1)
                {
                    return Ok(new
                    {

                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.CurrencyDeleted
                    });
                }
                else if (result == 2)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "First remove currency reference from company bank."
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteCurrency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatuscurrency")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusCurrency(int currency_Id, bool status)
        {
            try
            {
                var result = await _currencyService.CurrencyChangeStatus(currency_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusCurrency", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Pointer Master
        [HttpGet]
        [Route("getpointer")]
        [Authorize]
        public async Task<IActionResult> GetPointer(int pointerId)
        {
            try
            {
                var result = await _pointerService.GetPointer(pointerId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetPointer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("add_update_pointer_detail")]
        [Authorize]
        public async Task<IActionResult> Add_Update_Pointer_Detail([FromForm] Pointer_Master pointer_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (message, pointer_Id) = await _pointerService.Add_Update_Pointer(pointer_Mas);
                    if (message == "success" && pointer_Id > 0)
                    {
                        if (pointer_Mas.Pointer_Detail_List != null && pointer_Mas.Pointer_Detail_List.Count > 0)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Pointer_Det_Id", typeof(int));
                            dataTable.Columns.Add("Pointer_id", typeof(int));
                            dataTable.Columns.Add("From_Cts", typeof(decimal));
                            dataTable.Columns.Add("To_Cts", typeof(decimal));
                            dataTable.Columns.Add("Order_No", typeof(int));
                            dataTable.Columns.Add("Sort_No", typeof(int));
                            dataTable.Columns.Add("Status", typeof(bool));
                            dataTable.Columns.Add("Sub_Pointer", typeof(string));
                            dataTable.Columns.Add("QueryFlag", typeof(string));

                            DataTable dataTable2 = new DataTable();
                            var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable2.Columns.Add("Employee_Id", typeof(int));
                                dataTable2.Columns.Add("IP_Address", typeof(string));
                                dataTable2.Columns.Add("Trace_Date", typeof(DateTime));
                                dataTable2.Columns.Add("Trace_Time", typeof(TimeSpan));
                                dataTable2.Columns.Add("Record_Type", typeof(string));
                                dataTable2.Columns.Add("Pointer_id", typeof(int));
                                dataTable2.Columns.Add("From_Cts", typeof(decimal));
                                dataTable2.Columns.Add("To_Cts", typeof(decimal));
                                dataTable2.Columns.Add("Order_No", typeof(int));
                                dataTable2.Columns.Add("Sort_No", typeof(int));
                                dataTable2.Columns.Add("Status", typeof(bool));
                                dataTable2.Columns.Add("Sub_Pointer", typeof(string));
                            }

                            foreach (var item in pointer_Mas.Pointer_Detail_List)
                            {
                                dataTable.Rows.Add(item.Pointer_Det_Id, pointer_Id, item.From_Cts, item.To_Cts, item.Order_No, item.Sort_No, item.Status, item.Sub_Pointer, item.QueryFlag);
                                if (CoreService.Enable_Trace_Records(_configuration))
                                {
                                    dataTable2.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, pointer_Id, item.From_Cts, item.To_Cts, item.Order_No, item.Sort_No, item.Status, item.Sub_Pointer);
                                }
                            }
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                await _pointerService.Insert_Pointer_Detail_Trace(dataTable2);
                            }
                            await _pointerService.InsertPointerDetail(dataTable);
                        }
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PointerMasterCreated
                        });
                    }
                    else if (message == "_error_pointer_name")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.PointerMasterAlreadyExist
                        });
                    }
                    else if (message == "_error_order_no")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (message == "_error_sort_no")
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Add_Update_Pointer_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_poiner_details")]
        [Authorize]
        public async Task<IActionResult> Get_Poiner_Details(int pointer_Id)
        {
            try
            {
                var result = await _pointerService.Get_Pointer_Details(pointer_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Poiner_Details", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatepointer")]
        [Authorize]
        public async Task<IActionResult> UpdatePointer(Pointer_Master pointer_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _pointerService.UpdatePointer(pointer_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PointerMasterUpdated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdatePointer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletepointer")]
        [Authorize]
        public async Task<IActionResult> DeletePointer(int pointerId)
        {
            try
            {
                if (pointerId > 0)
                {
                    var result = await _pointerService.DeletePointer(pointerId);
                    if (result > 0)
                    {
                        return Ok(new
                        {

                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.PointerMasterDeleted
                        });
                    }
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatuspointer")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusPointer(int pointer_Id, bool status)
        {
            try
            {
                var result = await _pointerService.PointerChangeStatus(pointer_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusPointer", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_pointer_max_order_no")]
        [Authorize]
        public async Task<IActionResult> Get_Pointer_Max_Order_No()
        {
            var result = await _pointerService.Get_Pointer_Master_Max_Order_No();

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                order_no = result
            });
        }
        #endregion

        #region BGM Master
        [HttpGet]
        [Route("getbgm")]
        [Authorize]
        public async Task<IActionResult> GetBgm(int bgm_Id, int shade, int milky)
        {
            var result = await _bGMService.GetBgm(bgm_Id, shade, milky);
            if (result != null && result.Count > 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = result
                });
            }
            return NoContent();
        }
        [HttpPost]
        [Route("createbgm")]
        [Authorize]
        public async Task<IActionResult> CreateBgm(BGM_Master bGM_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bGMService.InsertBGM(bGM_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.BGMCreated,
                        });
                    }
                    else if (result == 5)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.IsExistShade_Milky
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateBgm", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPut]
        [Route("updatebgm")]
        [Authorize]
        public async Task<IActionResult> UpdateBgm(BGM_Master bGM_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bGMService.UpdateBGM(bGM_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.BGMUpdated,
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateBgm", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpDelete]
        [Route("deletebgm")]
        [Authorize]
        public async Task<IActionResult> DeleteBgm(int bgm_Id)
        {
            try
            {
                var result = await _bGMService.DeleteBGM(bgm_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.BGMDeleted,
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteBgm", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatusbgm")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusBgm(int bgm_Id, bool status)
        {
            try
            {
                var result = await _bGMService.BGMChangeStatus(bgm_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusBgm", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Bank
        [HttpGet]
        [Route("getbank")]
        [Authorize]
        public async Task<IActionResult> GetBank(int bankId)
        {
            var result = await _bankService.GetBank(bankId);
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

        [HttpGet]
        [Route("get_active_bank")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Bank(int bankId, string bank_Name, int currency_Id)
        {
            var result = await _bankService.Get_Active_Bank(bankId, bank_Name, currency_Id);
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
        [Route("createbank")]
        [Authorize]
        public async Task<IActionResult> CreateBank(Bank_Master bank_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bankService.InsertBank(bank_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.BankMasterCreated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateBank", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatebank")]
        [Authorize]
        public async Task<IActionResult> UpdateBank(Bank_Master bank_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _bankService.UpdateBank(bank_Mas);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.BankMasterUpdated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateBank", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletebank")]
        [Authorize]
        public async Task<IActionResult> DeleteBank(int bankId)
        {
            try
            {
                var result = await _bankService.DeleteBank(bankId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.BankMasterDeleted,
                    });
                }
                return BadRequest(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                throw;
            }
        }

        [HttpPut]
        [Route("changestatusbank")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusBank(int bank_Id, bool status)
        {
            try
            {
                var result = await _bankService.BankChangeStatus(bank_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusBank", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_bank_distinct")]
        [Authorize]
        public async Task<IActionResult> Get_Bank_Distinct()
        {
            var result = await _bankService.Get_Bank_Distinct();
            if (result != null && result.Count > 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = result
                });
            }
            return NoContent();
        }

        [HttpGet]
        [Route("get_bank_branch")]
        [Authorize]
        public async Task<IActionResult> Get_Bank_Branch(string bank_Name)
        {
            var result = await _bankService.Get_Bank_Branch(bank_Name);
            if (result != null && result.Count > 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = result
                });
            }
            return NoContent();
        }
        #endregion

        #region Invoice Remarks
        [HttpGet]
        [Route("getinvoiceremarks")]
        [Authorize]
        public async Task<IActionResult> GetInvoiceRemarks(int processId, DateTime startDate)
        {
            var result = await _invoiceRemarksService.GetInvoiceRemarks(processId, startDate);
            if (result != null && result.Count > 0)
            {
                return Ok(new
                {
                    StatusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataSuccessfullyFound,
                    data = result
                });
            }
            return NoContent();
        }

        [HttpPost]
        [Route("createinvoiceremarks")]
        [Authorize]
        public async Task<IActionResult> CreateInvoiceRemarks(Invoice_Remarks invoice_Remarks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _invoiceRemarksService.InsertInvoiceRemarks(invoice_Remarks);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.InvoiceRemarksCreated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateInvoiceRemarks", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateinvoiceremarks")]
        [Authorize]
        public async Task<IActionResult> UpdateInvoiceRemarks(Invoice_Remarks invoice_Remarks)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _invoiceRemarksService.UpdateInvoiceRemarks(invoice_Remarks);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.InvoiceRemarksUpdated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateInvoiceRemarks", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteinvoiceremarks")]
        [Authorize]
        public async Task<IActionResult> DeleteInvoiceRemarks(int processId, DateTime startDate)
        {
            try
            {
                var result = await _invoiceRemarksService.DeleteInvoiceRemarks(processId, startDate);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.InvoiceRemarksDeleted,
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteInvoiceRemarks", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatusinvoiceremarks")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusInvoiceRemarks(int process_Id, bool status)
        {
            try
            {
                var result = await _invoiceRemarksService.InvoiceRemarksChangeStatus(process_Id, status);
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
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusInvoiceRemarks", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Holiday Master
        [HttpGet]
        [Route("getholiday")]
        [Authorize]
        public async Task<IActionResult> GetHoliday(string date)
        {
            try
            {
                var result = await _holidayService.Get_Holidays(date);
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
                await _commonService.InsertErrorLog(ex.Message, "GetHoliday", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_holiday")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Holiday([FromForm] IList<Holiday_Master> holiday_Masters)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Holiday_Id", typeof(int));
                    dataTable.Columns.Add("Date", typeof(string));
                    dataTable.Columns.Add("Start_Time", typeof(string));
                    dataTable.Columns.Add("End_Time", typeof(string));
                    dataTable.Columns.Add("Holiday_Flag", typeof(bool));
                    dataTable.Columns.Add("Description", typeof(string));
                    dataTable.Columns.Add("QueryFlag", typeof(string));

                    #region Holiday Master Log
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Columns.Add("Employee_Id", typeof(int));
                    dataTable1.Columns.Add("IP_Address", typeof(string));
                    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                    dataTable1.Columns.Add("Record_Type", typeof(string));
                    dataTable1.Columns.Add("Date", typeof(string));
                    dataTable1.Columns.Add("Start_Time", typeof(string));
                    dataTable1.Columns.Add("End_Time", typeof(string));
                    dataTable1.Columns.Add("Holiday_Flag", typeof(bool));
                    dataTable1.Columns.Add("Description", typeof(string));
                    #endregion

                    if (holiday_Masters != null && holiday_Masters.Count > 0)
                    {
                        foreach (var item in holiday_Masters)
                        {
                            dataTable.Rows.Add(item.Holiday_Id, item.Date, item.Start_Time, item.End_Time, item.Holiday_Flag, item.Description, item.QueryFlag);
                            if (CoreService.Enable_Trace_Records(_configuration))
                            {
                                dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, item.Date, item.Start_Time, item.End_Time, item.Holiday_Flag, item.Description);
                            }
                        }

                        var result = await _holidayService.Insert_Update_Holiday(dataTable);
                        if (CoreService.Enable_Trace_Records(_configuration))
                        {
                            await _holidayService.Insert_Holiday_Trace(dataTable1);
                        }
                        if (result > 0)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.HolidayMasterCreated,
                            });
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateHoliday", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updateholiday")]
        [Authorize]
        public async Task<IActionResult> UpdateHoliday(Holiday_Master holiday_Mas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _holidayService.UpdateHoliday(holiday_Mas);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.HolidayMasterUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateHoliday", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteholiday")]
        [Authorize]
        public async Task<IActionResult> DeleteHoliday(int holiday_Id)
        {
            try
            {
                var result = await _holidayService.DeleteHoliday(holiday_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.HolidayMasterDeleted,
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteHoliday", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Layout Master
        [HttpGet]
        [Route("getlayout")]
        [Authorize]
        public async Task<IActionResult> GetLayout(int layoutId, int menuId, int employeeId)
        {
            try
            {
                var result = await _commonService.GetLayout(layoutId, menuId, employeeId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetLayout", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("getlayoutdetail")]
        [Authorize]
        public async Task<IActionResult> GetLayoutDetail(int layoutDetailId, int layoutId)
        {
            try
            {
                var result = await _commonService.GetLayout_Details(layoutDetailId, layoutId);
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
                await _commonService.InsertErrorLog(ex.Message, "GetLayoutDetail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createlayout")]
        [Authorize]
        public async Task<IActionResult> CreateLayout(Layout_Master layout_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.InsertLayoutMas(layout_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.LayoutAddedSuccessMessage
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateLayout", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletelayout")]
        [Authorize]
        public async Task<IActionResult> DeleteLayout(int menuId, int employeeId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.DeleteLayout(menuId, employeeId);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.LayoutDeletedSuccessMessage
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteLayout", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletelayoutdetail")]
        [Authorize]
        public async Task<IActionResult> DeleteLayoutDetail(int layout_Detail_Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.DeleteLayoutDetail(layout_Detail_Id);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.LayoutDeletedSuccessMessage
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteLayoutDetail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatelayoutdetailstatus")]
        [Authorize]
        public async Task<IActionResult> UpdateLayoutDetailStatus(int layoutDetailId, int layoutId)
        {
            try
            {
                var result = await _commonService.UpdateLayoutStatus(layoutDetailId, layoutId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.LayoutUpdatedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateLayoutDetailStatus", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("getdefaultlayout")]
        [Authorize]
        public async Task<IActionResult> GetDefaultLayout()
        {
            var result = await _commonService.DefaultLayout();
            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                message = CoreCommonMessage.DataSuccessfullyFound,
                data = result
            });
        }
        #endregion

        #region Terms & Conditions
        [HttpGet]
        [Route("gettermsandconditions")]
        [Authorize]
        public async Task<IActionResult> GetTermsAndConditions(int condition_Id, int process_Id)
        {
            try
            {
                var result = await _termsAndConditionService.GetTermsAndCondition(condition_Id, process_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "GetTermsAndConditions", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createtermsandconditions")]
        [Authorize]
        public async Task<IActionResult> CreateTermsAndConditions(TermsAndCondition termsAndCondition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _termsAndConditionService.AddUpdateTermsAndCondition(termsAndCondition);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            StatusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.TermsAndConditionsCreated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            StatusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            StatusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateTermsAndConditions", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatetermsandconditions")]
        [Authorize]
        public async Task<IActionResult> UpdateTermsAndConditions(TermsAndCondition termsAndCondition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _termsAndConditionService.AddUpdateTermsAndCondition(termsAndCondition);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            StatusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.TermsAndConditionsUpdated,
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            StatusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.OrderNoAlreadyExist
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            StatusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.SortNoAlreadyExist
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateTermsAndConditions", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletetermsandconditions")]
        [Authorize]
        public async Task<IActionResult> DeleteTermsAndConditions(int condition_Id)
        {
            try
            {
                var result = await _termsAndConditionService.DeleteTermsAndCondition(condition_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.TermsAndConditionsDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteTermsAndConditions", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatustermsandconditions")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusTermsAndConditions(int condition_Id, bool status)
        {
            try
            {
                var result = await _termsAndConditionService.TermsAndConditionChangeStatus(condition_Id, status);
                if (result == 1)
                {
                    return Ok(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage,
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusTermsAndConditions", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Loader Master
        [HttpGet]
        [Route("get_loder")]
        [Authorize]
        public async Task<IActionResult> Get_Loder()
        {
            try
            {
                var result = await _commonService.GetLoader();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Loder", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_loader")]
        [Authorize]
        public async Task<IActionResult> Create_Loader([FromForm] Loader_Master loader_Master, IFormFile Loader_Img)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Loader_Img != null && Loader_Img.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/LoaderImages");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(Loader_Img.FileName);
                        string fileExt = Path.GetExtension(Loader_Img.FileName);

                        string strFile = fileName + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await Loader_Img.CopyToAsync(fileStream);
                        }
                        loader_Master.Loader_Img = strFile;
                    }
                    var result = await _commonService.InsertLoader(loader_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.LoaderMasterCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Loader", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("set_default_loader")]
        public async Task<IActionResult> Set_Default_Loader(int employee_Id, int loader_Id, bool default_Loader)
        {
            try
            {
                var result = await _commonService.Set_Default_Loader(employee_Id, loader_Id, default_Loader);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.SetDefaultLoader,
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Set_Default_Loader", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleteloader")]
        [Authorize]
        public async Task<IActionResult> DeleteLoader(int loader_Id)
        {
            try
            {
                var result = await _commonService.DeleteLoader(loader_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.LoaderMasterDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteLoader", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("add_employee_loader")]
        [Authorize]
        public async Task<IActionResult> Add_Employee_Loader(Employee_Loader employee_Loader)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _commonService.Add_Employee_Loader(employee_Loader);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = "Employee loader added successfully."
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Add_Employee_Loader", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_employee_loader")]
        public async Task<IActionResult> Get_Employee_Loader(int employee_Id)
        {
            try
            {
                var result = await _commonService.Get_Employee_Loader(employee_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Employee_Loader", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Exchange Rate Master
        [HttpGet]
        [Route("get_exchange_rate")]
        [Authorize]
        public async Task<IActionResult> Get_Exchange_Rate(int exchange_Id)
        {
            try
            {
                var result = await _exchange_Rate_Service.Get_Exchange_Rate(exchange_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Exchange_Rate", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_update_exchange_rate")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Exchange_Rate([FromForm] IList<Exchange_Rate_Master> exchange_Rate_Masters)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ip_Address = await CoreService.GetIP_Address(_httpContextAccessor);
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Exchange_Id", typeof(int));
                    dataTable.Columns.Add("Trans_date", typeof(string));
                    dataTable.Columns.Add("Currency_Id", typeof(int));
                    dataTable.Columns.Add("Bank_Rate", typeof(decimal));
                    dataTable.Columns.Add("Custom_Rate", typeof(decimal));
                    dataTable.Columns.Add("QueryFlag", typeof(string));

                    #region Exchange Rate Log
                    DataTable dataTable1 = new DataTable();
                    dataTable1.Columns.Add("Employee_Id", typeof(int));
                    dataTable1.Columns.Add("IP_Address", typeof(string));
                    dataTable1.Columns.Add("Trace_Date", typeof(DateTime));
                    dataTable1.Columns.Add("Trace_Time", typeof(TimeSpan));
                    dataTable1.Columns.Add("Record_Type", typeof(string));
                    dataTable1.Columns.Add("Trans_date", typeof(string));
                    dataTable1.Columns.Add("Currency_Id", typeof(int));
                    dataTable1.Columns.Add("Bank_Rate", typeof(decimal));
                    dataTable1.Columns.Add("Custom_Rate", typeof(decimal));
                    #endregion

                    foreach (var item in exchange_Rate_Masters)
                    {
                        dataTable.Rows.Add(item.Exchange_Id, item.Trans_date, item.Currency_Id, item.Bank_Rate, item.Custom_Rate, item.QueryFlag);
                        if (CoreService.Enable_Trace_Records(_configuration))
                        {
                            dataTable1.Rows.Add(16, ip_Address, DateTime.Now, DateTime.Now.TimeOfDay, item.QueryFlag, item.Trans_date, item.Currency_Id, item.Bank_Rate, item.Custom_Rate);
                        }
                    }
                    if (CoreService.Enable_Trace_Records(_configuration))
                    {
                        await _exchange_Rate_Service.Insert_Exchange_Rate_Trace(dataTable1);
                    }
                    var result = await _exchange_Rate_Service.Insert_Update_Exchange_Rate(dataTable);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ExchangeRateCreated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Update_Exchange_Rate", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
        #endregion
    }
}
