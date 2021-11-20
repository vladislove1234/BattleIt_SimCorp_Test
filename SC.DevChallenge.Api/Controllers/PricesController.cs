using System;
using System.Collections.Generic;
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
        private BenchmarkCalculator _benchmarkCalculator;
        public PricesController(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            _benchmarkCalculator = new BenchmarkCalculator(_dataStorage);
        }
        [HttpGet("average")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Average([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] DateTime datetime)//
        {
            if (string.IsNullOrEmpty(portfolio) || string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(instrument) || datetime == DateTime.MinValue)
                return StatusCode(404);
            int period = PriceManager.PeriodFromDate(datetime);
            double sum = 0;
            var RequestedPriceInfos = _dataStorage.GetPriceinfosFromPIIT(portfolio, owner, instrument, period);
            RequestedPriceInfos.ForEach(x => sum += x.Price);
            if (RequestedPriceInfos.Count != 0)
                return new JsonResult(new Response()
                {
                    date = PriceManager.DateFromPeriod(period),
                    price = (float)Math.Round((sum / RequestedPriceInfos.Count), 2)
                }.ToString());
            else return StatusCode(404);
        }
        [HttpGet("avarage_for_period")]
        public IActionResult AverageForPeriod([FromQuery] string portfolio, [FromQuery] string owner, [FromQuery] string instrument, [FromQuery] DateTime? time)//
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
                return new JsonResult(new Response()
                {
                    date = PriceManager.DateFromPeriod((int)period),
                    price = (float)Math.Round((sum / priceInfos.Count), 2)
                }.ToString());
            else return StatusCode(404);
        }
        [HttpGet("benchmark")]
        public IActionResult Benchmark([FromQuery] string portfolio, [FromQuery] DateTime date)//return benchmark for period
        {
            if (date < PriceManager.StartDate || string.IsNullOrEmpty(portfolio))//checking for right entry
                return StatusCode(404);
            int period = PriceManager.PeriodFromDate(date);
            var prices = _dataStorage.GetPriceinfosFromPIIT(portfolio, "", "", period);
            double? avrPrice = _benchmarkCalculator.CalculatePrice(prices);// getting average price
            if (avrPrice == null)
                return StatusCode(404);
            else
                return new JsonResult(new Response()
                {
                    date = PriceManager.DateFromPeriod(period),
                    price = (float)Math.Round((double)avrPrice, 2)
                });

        }
        [HttpGet("benchmark_period")]
        public IActionResult Benchmark([FromQuery] string portfolio, [FromQuery] int period)//return benchmark for period(easier to test)
        {
            if (period < 0 || string.IsNullOrEmpty(portfolio))//checking for right entry
                return StatusCode(404);
            var prices = _dataStorage.GetPriceinfosFromPIIT(portfolio, "", "", period);
            double? avrPrice = _benchmarkCalculator.CalculatePrice(prices);// getting average price
            if (avrPrice == null)
                return StatusCode(404);
            else
                return new JsonResult(new Response()
                {
                    date = PriceManager.DateFromPeriod(period),
                    price = (float)Math.Round((double)avrPrice, 2)
                });

        }
        [HttpGet("aggregate")]
        public IActionResult Aggregate([FromQuery] string portfolio, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, int intervals)//returns benchmarks for intervals
        {
            if (string.IsNullOrEmpty(portfolio) || startDate == DateTime.MinValue || endDate == DateTime.MinValue || intervals <= 0 || startDate > endDate)
                return StatusCode(404);
            int startTs = PriceManager.PeriodFromDate(startDate),//getting start timeslot
                endTs = PriceManager.PeriodFromDate(endDate);// and end timeslot
            var responses = _benchmarkCalculator.CalculateAggregateBenchmark(portfolio, startTs, endTs, intervals);
            if (responses.Where(x => x.price != 0).FirstOrDefault() == null)
                return StatusCode(404);
            return new JsonResult(responses);
        }
    }
}
