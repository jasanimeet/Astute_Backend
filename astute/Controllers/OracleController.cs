using astute.CoreModel;
using astute.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
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
                if (result.Item1 > 0 && result.Item2 > 0 && result.Item3 > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.Fortune_Discount_Added
                    });
                }
                return NoContent();
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
        #endregion
    }
}
