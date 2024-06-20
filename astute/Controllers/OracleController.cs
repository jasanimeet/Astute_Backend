using astute.CoreModel;
using astute.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OracleController : ControllerBase
    {
        #region Fields
        private readonly IOracleService _oracleService;
        private readonly ICommonService _commonService;
        #endregion

        #region Ctor
        public OracleController(IOracleService oracleService,
            ICommonService commonService) 
        {
            _oracleService = oracleService;
            _commonService = commonService;
        }
        #endregion

        #region Task Scheduler : Data Upload From Oracle
        [HttpGet]
        [Route("get_fortune_discount")]
        public async Task<IActionResult> Get_Fortune_Discount()
        {
            try
            {
                var result = await _oracleService.Get_Fortune_Discount();

                List<string> errorMessages = new List<string>();

                if (result.Item1 <= 0)
                {
                    errorMessages.Add(CoreCommonMessage.Fortune_Purchase_Disc);
                }

                if (result.Item2 <= 0)
                {
                    errorMessages.Add(CoreCommonMessage.Fortune_Sale_Disc);
                }

                if (result.Item3 <= 0)
                {
                    errorMessages.Add(CoreCommonMessage.Fortune_Stock_Disc);
                }

                if (result.Item4 <= 0)
                {
                    errorMessages.Add(CoreCommonMessage.Fortune_Sale_Disc_Kts);
                }

                if (result.Item5 <= 0)
                {
                    errorMessages.Add(CoreCommonMessage.Fortune_Stock_Disc_Kts);
                }

                if (errorMessages.Any())
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.BadRequest,
                        message = string.Join("\n", errorMessages)
                    });
                }
                else
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.Fortune_Discount_Added
                    });
                }
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Fortune_Discount", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_fortune_party")]
        public async Task<IActionResult> Get_Fortune_Party()
        {
            try
            {
                var result = await _oracleService.Get_Fortune_Party();
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.Fortune_Party_Added
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Fortune_Party", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("get_fortune_party_master")]
        public async Task<IActionResult> Get_Fortune_Party_Master()
        {
            try
            {
                var result = await _oracleService.Get_Fortune_Party_Master();
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.Fortune_Party_Master_Added
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Fortune_Party_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
