using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class RapaportController : ControllerBase
    {
        #region Fields
        private readonly IRapaportService _rapaportService;
        private readonly ICommonService _commonService;

        #endregion

        #region Ctor
        public RapaportController(IRapaportService rapaportService,
            ICommonService commonService)
        {
            _rapaportService = rapaportService;
            _commonService = commonService;
        }
        #endregion

        #region Methods
        #region Rapaport Master
        [HttpGet]
        [Route("getrapaport")]
        [Authorize]
        public async Task<IActionResult> GetRapaport(int rapId)
        {
            try
            {
                var result = await _rapaportService.GetRapaport(rapId);
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetRapaport", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createrapaport")]
        [Authorize]
        public async Task<IActionResult> CreateRapaport(Rapaport_Master rapaport_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rapaportService.InsertRapaport(rapaport_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.RapaportMasterCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateRapaport", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updaterapaport")]
        [Authorize]
        public async Task<IActionResult> UpdateRapaport(Rapaport_Master rapaport_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rapaportService.UpdateRapaport(rapaport_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.RapaportMasterUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateRapaport", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleterapaport")]
        [Authorize]
        public async Task<IActionResult> DeleteRapaport(int rapId)
        {
            try
            {
                var result = await _rapaportService.DeleteRapaport(rapId);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.RapaportMasterDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteRapaport", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Rapaport Detail
        [HttpGet]
        [Route("getrapaportdetail")]
        [Authorize]
        public async Task<IActionResult> GetRapaportDetail(string? shape, string? color, string? clarity, decimal? frmCts, decimal? toCts, decimal? rate, string? date)
        {
            try
            {
                var result = await _rapaportService.GetRapaportDetail(shape, color, clarity, frmCts, toCts, rate, date);
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
                await _commonService.InsertErrorLog(ex.Message, "GetRapaportDetail", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createrapaportdetail")]
        [Authorize]
        public async Task<IActionResult> CreateRapaportDetail(IList<RapaportPriceModel> rapaport_Detail_Models)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Rap_Det_Id", typeof(int));
                dataTable.Columns.Add("Rap_Id", typeof(int));
                dataTable.Columns.Add("From_Cts", typeof(decimal));
                dataTable.Columns.Add("To_Cts", typeof(decimal));
                dataTable.Columns.Add("Color", typeof(string));
                dataTable.Columns.Add("Clarity", typeof(string));
                dataTable.Columns.Add("Rate", typeof(decimal));
                dataTable.Columns.Add("Shape", typeof(string));

                var _date = rapaport_Detail_Models.Select(x => x.Date).FirstOrDefault();
                var rapaport = new Rapaport_Master()
                {
                    Rap_Id = 0,
                    Insert_Date = _date,
                };

                var rap_Id = await _rapaportService.InsertRapaport(rapaport);
                if (rapaport_Detail_Models != null && rapaport_Detail_Models.Count > 0)
                {
                    foreach (var item in rapaport_Detail_Models)
                    {
                        dataTable.Rows.Add(item.Rap_Det_Id, rap_Id, item.From_Cts, item.To_Cts, item.Color, item.Clarity, item.Rate, item.Shape);
                    }
                }

                var result = await _rapaportService.InsertRapaportDetail(dataTable);

                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.RapaportDetailCreated
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateRapaportDetail", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updaterapaportdetail")]
        [Authorize]
        public async Task<IActionResult> UpdateRapaportDetail(Rapaport_Detail rapaport_Detail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (rapaport_Detail.From_Cts > rapaport_Detail.To_Cts)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.WeightErrorMessage
                        });
                    }
                    var result = await _rapaportService.UpdateRapaportDetail(rapaport_Detail);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.RapaportDetailUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateRapaportDetail", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleterapaportdetail")]
        [Authorize]
        public async Task<IActionResult> DeleteRapaportDetail(int rap_Det_Id)
        {
            try
            {
                var result = await _rapaportService.DeleteRapaportDetail(rap_Det_Id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.RapaportDetailDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteRapaportDetail", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Rapaport User
        [HttpGet]
        [Route("getrapaportuser")]
        public async Task<IActionResult> GetRapaportUser(string rapUser)
        {
            try
            {
                var result = await _rapaportService.GetRapaportUser(rapUser);
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
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetRapaportUser", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createrapaportuser")]
        public async Task<IActionResult> CreateRapaportUser(Rapaport_User rapaport_User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rapaportService.InsertRapaportUser(rapaport_User);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.RapaportUserCreated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateRapaportUser", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updaterapaportuser")]
        public async Task<IActionResult> UpdateRapaportUser(Rapaport_User rapaport_User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _rapaportService.UpdateRapaportUser(rapaport_User);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.RapaportUserUpdated,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateRapaportUser", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deleterapaportuser")]
        public async Task<IActionResult> DeleteRapaportUser(string rapUser)
        {
            try
            {
                var result = await _rapaportService.DeleteRapaportUser(rapUser);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.RapaportUserDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "DeleteRapaportUser", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        [HttpPost]
        [Route("rapaport_pricelist_round_pear")]
        [Authorize]
        public async Task<IActionResult> Rapaport_PriceList_Round_Pear(string userName, string password, [FromForm] IFormFile[] files)
        {
            try
            {
                var dataList = new List<RapaportPriceModel>();

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        #region ROUND
                        var requestround = new HttpRequestMessage(HttpMethod.Post, "https://technet.rapaport.com/HTTP/Prices/CSV2_Round.aspx");
                        requestround.Headers.Add("Cookie", "ASP.NET_SessionId=zj3ftyrz0up3spcdyldio2fo");

                        var collectionRound = new List<KeyValuePair<string, string>>
                        {
                            new("Content-Type", "application/json"),
                            new("Username", userName),
                            new("Password", password)
                        };

                        requestround.Content = new FormUrlEncodedContent(collectionRound);

                        var responseRound = await client.SendAsync(requestround);

                        if (responseRound.StatusCode == HttpStatusCode.OK)
                        {
                            responseRound.EnsureSuccessStatusCode();

                            using (var streamReader = new StreamReader(await responseRound.Content.ReadAsStreamAsync()))
                            using (var csvReader = new CsvHelper.CsvReader(streamReader, new CultureInfo("en-US")))
                            {
                                while (csvReader.Read())
                                {
                                    var dataItem = new RapaportPriceModel
                                    {
                                        Shape = csvReader.GetField<string>(0),
                                        Clarity = csvReader.GetField<string>(1),
                                        Color = csvReader.GetField<string>(2),
                                        From_Cts = csvReader.GetField<decimal>(3),
                                        To_Cts = csvReader.GetField<decimal>(4),
                                        Rate = csvReader.GetField<decimal>(5),
                                        Date = csvReader.GetField<string>(6).Trim()
                                    };

                                    dataList.Add(dataItem);
                                }
                            }
                        }

                        #endregion

                        #region PEAR

                        var request = new HttpRequestMessage(HttpMethod.Post, "https://technet.rapaport.com/HTTP/Prices/CSV2_Pear.aspx");
                        request.Headers.Add("Cookie", "ASP.NET_SessionId=zj3ftyrz0up3spcdyldio2fo");

                        var collection = new List<KeyValuePair<string, string>>
                        {
                            new("Content-Type", "application/json"),
                            new("Username", userName),
                            new("Password", password)
                        };

                        request.Content = new FormUrlEncodedContent(collection);

                        var response = await client.SendAsync(request);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            response.EnsureSuccessStatusCode();

                            using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                            using (var csvReader = new CsvHelper.CsvReader(streamReader, new CultureInfo("en-US")))
                            {
                                while (csvReader.Read())
                                {
                                    var dataItem = new RapaportPriceModel
                                    {
                                        Shape = csvReader.GetField<string>(0),
                                        Clarity = csvReader.GetField<string>(1),
                                        Color = csvReader.GetField<string>(2),
                                        From_Cts = csvReader.GetField<decimal>(3),
                                        To_Cts = csvReader.GetField<decimal>(4),
                                        Rate = csvReader.GetField<decimal>(5),
                                        Date = csvReader.GetField<string>(6).Trim()
                                    };

                                    dataList.Add(dataItem);
                                }
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return Unauthorized(new
                            {
                                statusCode = HttpStatusCode.Unauthorized,
                                message = CoreCommonMessage.UnauthorizedUser
                            });
                        }

                        #endregion
                    }

                    await CreateRapaportDetail(dataList);
                    string jsonData = CoreService.ConvertModelListToJson(dataList);
                    return Ok(jsonData);
                }
                else if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0 && Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var streamReader = new StreamReader(file.OpenReadStream()))
                            using (var csvReader = new CsvHelper.CsvReader(streamReader, new CultureInfo("en-US")))
                            {
                                while (csvReader.Read())
                                {
                                    var dataItem = new RapaportPriceModel
                                    {
                                        Shape = csvReader.GetField<string>(0),
                                        Clarity = csvReader.GetField<string>(1),
                                        Color = csvReader.GetField<string>(2),
                                        From_Cts = csvReader.GetField<decimal>(3),
                                        To_Cts = csvReader.GetField<decimal>(4),
                                        Rate = csvReader.GetField<decimal>(5),
                                        Date = csvReader.GetField<string>(6).Trim()
                                    };

                                    dataList.Add(dataItem);
                                }
                            }
                        }
                    }

                    await CreateRapaportDetail(dataList);
                    string jsonData = CoreService.ConvertModelListToJson(dataList);
                    return Ok(jsonData);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new
                    {
                        message = "No valid input provided."
                    });
                }
            }
            catch (HttpRequestException ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Rapaport_PriceList_Round_Pear", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = $"HTTP request error: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Rapaport_PriceList_Round_Pear", ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet]
        [Route("get_rapaport_clarity_filter_value")]
        [Authorize]
        public async Task<IActionResult> Get_Rapaport_Clarity_Filter_Value()
        {
            try
            {
                var result = await _rapaportService.Get_Rapaport_Clarity_Filter_Value();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Rapaport_Clarity_Filter_Value", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_rapaport_color_filter_value")]
        [Authorize]
        public async Task<IActionResult> Get_Rapaport_Color_Filter_Value()
        {
            try
            {
                var result = await _rapaportService.Get_Rapaport_Color_Filter_Value();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Rapaport_Color_Filter_Value", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_rapaport_date_filter_value")]
        [Authorize]
        public async Task<IActionResult> Get_Rapaport_Date_Filter_Value()
        {
            try
            {
                var result = await _rapaportService.Get_Rapaport_Date_Filter_Value();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Rapaport_Date_Filter_Value", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_rapaport_color")]
        [Authorize]
        public async Task<IActionResult> Get_Rapaport_Color()
        {
            try
            {
                var result = await _rapaportService.Get_Rapaport_Color();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Rapaport_Color", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_shape_filter_value")]
        [Authorize]
        public async Task<IActionResult> Get_Shape_Filter_Value()
        {
            try
            {
                var result = await _rapaportService.Get_Shape_Filter_Value();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Shape_Filter_Value", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_diamond_type_filter_value")]
        [Authorize]
        public async Task<IActionResult> Get_Diamond_Type_Filter_Value()
        {
            try
            {
                var result = await _rapaportService.Get_Diamond_Type_Filter_Value();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Diamond_Type_Filter_Value", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get_rapaport_rate_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Rapaport_Rate_Detail(List<Rapaport_Rate_Detail_Model> rapaport_Rate_Detail_Model)
        {
            try
            {
                List<Dictionary<string, object>> rapaport_Rate_Detail = new List<Dictionary<string, object>>();

                foreach (var item in rapaport_Rate_Detail_Model)
                {
                    var result = await _rapaportService.Get_Rapaport_Rate_Detail(item);
                    if (result != null)
                    {
                        foreach (var dict in result)
                        {
                            rapaport_Rate_Detail.Add(dict);
                        }
                    }
                }
                if (rapaport_Rate_Detail != null && rapaport_Rate_Detail.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = rapaport_Rate_Detail
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetRapaportDetail", ex.StackTrace);
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        #endregion
    }
}
