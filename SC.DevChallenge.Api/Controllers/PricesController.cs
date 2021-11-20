using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SC.DevChallenge.Api.Model;
using SC.DevChallenge.Api.Model.Entities;
using SC.DevChallenge.Api.Model.Interfaces;
using SC.DevChallenge.Api.Model.Services;

namespace SC.DevChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController : ControllerBase
    {
        private ILogger _logger;
        private IDataStorage _dataStorage;
        public PricesController(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }
        [HttpGet("average")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Average([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] DateTime datetime)//
        {
            if (string.IsNullOrEmpty(portfolio) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(instrument) || datetime == DateTime.MinValue)
                return StatusCode(404);
            int period = PriceManager.PeriodFromDate(datetime);
            double sum = 0;
            var RequestedPriceInfos = _dataStorage.GetPriceinfosFromPIIT(portfolio, owner, instrument, period);
            RequestedPriceInfos.ForEach(x => sum += x.Price);
            if (RequestedPriceInfos.Count != 0)
                return Content(new Response()
                {
                    date = PriceManager.DateFromPeriod(period),
                    price = (float)Math.Round((sum / RequestedPriceInfos.Count ), 2)
                }.ToString());
            else return StatusCode(404);
        }
        [HttpGet("avarage_for_period")]
        public ActionResult AverageForPeriod([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] DateTime? time)//
        {
            int period;
            if (time == null)
                period = -1;
            else
                period = PriceManager.PeriodFromDate((DateTime)time);
            double sum = 0;
            var priceInfos = _dataStorage.GetPriceinfosFromPIIT(portfolio, owner, instrument, period);
                foreach (var info in priceInfos)
                sum += info.Price;
            if (priceInfos.Count != 0)
                return Content(new Response()
                {
                    date = PriceManager.DateFromPeriod((int)period),
                    price = (float)Math.Round((sum / priceInfos.Count), 2)
                }.ToString());
            else return StatusCode(404);
        }
    }
}
