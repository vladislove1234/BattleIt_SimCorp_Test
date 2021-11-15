using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SC.DevChallenge.Api.Model;
using SC.DevChallenge.Api.Model.Entities;
using SC.DevChallenge.Api.Model.Services;

namespace SC.DevChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController : ControllerBase
    {
        private ILogger _logger;
        [HttpGet("average")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Average([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] DateTime datetime)//
        {
            if (string.IsNullOrEmpty(portfolio) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(instrument) || datetime == DateTime.MinValue)
                return StatusCode(404);
            var ret = PriceManager.GetResponse(portfolio, owner, instrument, datetime);
            if (ret != null)
                return Content(ret.ToString());
            else
                return StatusCode(404);
        }
        [HttpGet("avarage_for_period")]
        public ActionResult AverageForPeriod([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] int period)//
        {
            var ret = PriceManager.GetResponse(portfolio, owner, instrument, period);
            if (ret != null)
                return Content(ret.ToString());
            else
                return StatusCode(404);
        }
    }
}
