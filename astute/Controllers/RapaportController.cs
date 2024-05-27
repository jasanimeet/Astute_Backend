using astute.CoreModel;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using astute.CoreServices;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class RapaportController : ControllerBase
    {
        #region Fields
        private readonly IRapaportService _rapaportService;
        #endregion

        #region Ctor
        public RapaportController(IRapaportService rapaportService)
        {
            _rapaportService = rapaportService;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        [Route("rapaport_pricelist_round_pear")]
        [Authorize]
        public async Task<IActionResult> Rapaport_PriceList_Round_Pear(string userName, string password)
        {
            try
            {
                var dataList = new List<RapaportPriceModel>();
                var dataList1 = new List<RapaportPriceModel>();
                using (HttpClient client = new HttpClient())
                {
                    #region ROUND
                    var requestround = new HttpRequestMessage(HttpMethod.Post, "https://technet.rapaport.com/HTTP/Prices/CSV2_Round.aspx");
                    requestround.Headers.Add("Cookie", "ASP.NET_SessionId=zj3ftyrz0up3spcdyldio2fo");

                    var collectionRound = new List<KeyValuePair<string, string>>();
                    collectionRound.Add(new("Content-Type", "application/json"));
                    collectionRound.Add(new("Username", userName));
                    collectionRound.Add(new("Password", password));
                    var contentRound = new FormUrlEncodedContent(collectionRound);
                    requestround.Content = contentRound;
                    var responseRound = await client.SendAsync(requestround);
                    #endregion

                    if (responseRound.StatusCode == HttpStatusCode.OK)
                    {
                        responseRound.EnsureSuccessStatusCode();
                        using (var streamReader = new StreamReader(await responseRound.Content.ReadAsStreamAsync()))
                        using (var csvReader = new CsvHelper.CsvReader(streamReader, new CultureInfo("en-US")))
                        {
                            var records = csvReader.GetRecords<dynamic>();
                            while (csvReader.Read())
                            {
                                RapaportPriceModel dataItem = new RapaportPriceModel
                                {
                                    Shape = csvReader.GetField<string>(0),
                                    Clarity = csvReader.GetField<string>(1),
                                    Color = csvReader.GetField<string>(2),
                                    From_Cts = csvReader.GetField<decimal>(3),
                                    To_Cts = csvReader.GetField<decimal>(4),
                                    Rate = csvReader.GetField<decimal>(5),
                                    Date = csvReader.GetField<string>(6).Trim() //DateTime.ParseExact(csvReader.GetField<string>(6).Trim(), "M/d/yyyy", CultureInfo.InvariantCulture)
                                };
                                string date = CoreService.ConvertDateFormat(dataItem.Date);
                                dataItem.Date = date;
                                dataList.Add(dataItem);

                                RapaportPriceModel dataItem1 = new RapaportPriceModel
                                {
                                    Shape = csvReader.GetField<string>(0),
                                    Clarity = csvReader.GetField<string>(1),
                                    Color = csvReader.GetField<string>(2),
                                    From_Cts = csvReader.GetField<decimal>(3),
                                    To_Cts = csvReader.GetField<decimal>(4),
                                    Rate = csvReader.GetField<decimal>(5),
                                    Date = csvReader.GetField<string>(6).Trim() //DateTime.ParseExact(csvReader.GetField<string>(6).Trim(), "M/d/yyyy", CultureInfo.InvariantCulture)
                                };
                                dataList1.Add(dataItem1);
                            }
                        }
                    }
                    #region PEAR
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://technet.rapaport.com/HTTP/Prices/CSV2_Pear.aspx");
                    request.Headers.Add("Cookie", "ASP.NET_SessionId=zj3ftyrz0up3spcdyldio2fo");

                    var collection = new List<KeyValuePair<string, string>>();
                    collection.Add(new("Content-Type", "application/json"));
                    collection.Add(new("Username", userName));
                    collection.Add(new("Password", password));
                    var content = new FormUrlEncodedContent(collection);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    #endregion

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.EnsureSuccessStatusCode();
                        using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        using (var csvReader = new CsvHelper.CsvReader(streamReader, new CultureInfo("en-US")))
                        {
                            var records = csvReader.GetRecords<dynamic>();
                            while (csvReader.Read())
                            {
                                RapaportPriceModel dataItem = new RapaportPriceModel
                                {
                                    Shape = csvReader.GetField<string>(0),
                                    Clarity = csvReader.GetField<string>(1),
                                    Color = csvReader.GetField<string>(2),
                                    From_Cts = csvReader.GetField<decimal>(3),
                                    To_Cts = csvReader.GetField<decimal>(4),
                                    Rate = csvReader.GetField<decimal>(5),
                                    Date = csvReader.GetField<string>(6).Trim() //ateTime.ParseExact(csvReader.GetField<string>(6).Trim(), "M/d/yyyy", CultureInfo.InvariantCulture)
                                };
                                string date = CoreService.ConvertDateFormat(dataItem.Date);
                                dataItem.Date = date;
                                dataList.Add(dataItem);

                                RapaportPriceModel dataItem1 = new RapaportPriceModel
                                {
                                    Shape = csvReader.GetField<string>(0),
                                    Clarity = csvReader.GetField<string>(1),
                                    Color = csvReader.GetField<string>(2),
                                    From_Cts = csvReader.GetField<decimal>(3),
                                    To_Cts = csvReader.GetField<decimal>(4),
                                    Rate = csvReader.GetField<decimal>(5),
                                    Date = csvReader.GetField<string>(6).Trim() //ateTime.ParseExact(csvReader.GetField<string>(6).Trim(), "M/d/yyyy", CultureInfo.InvariantCulture)
                                };
                                dataList1.Add(dataItem1);
                            }
                        }
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized(new
                        {
                            statusCode = HttpStatusCode.Unauthorized,
                            message = CoreCommonMessage.UnauthorizedUser
                        });
                    }

                    await CreateRapaportDetail(dataList1);
                    string jsonData = CoreService.ConvertModelListToJson(dataList);
                    return Ok(jsonData);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return BadRequest();
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
