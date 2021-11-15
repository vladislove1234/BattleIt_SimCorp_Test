using System;
using System.Linq;
using SC.DevChallenge.Api.Model.Entities;
using SC.DevChallenge.Api.Model.Services;

namespace SC.DevChallenge.Api.Model
{
    public static class PriceManager
    {
        public static int timePeriod = 10000;
        public static readonly DateTime StartDate = new DateTime(2018,1,1,0,0,0,0);
        public static int PeriodFromDate(DateTime date)
        {
            var currDate = StartDate.AddSeconds(timePeriod);
            int period = 0;
            while(currDate < date)
            {
                period++;
                currDate = currDate.AddSeconds(timePeriod);
            }
            return period;
        }
        public static DateTime DateFromPeriod(int period)
        {
            if (period >= 0)
                return StartDate.AddSeconds(timePeriod * period);
            else
                return StartDate;
        }
        public static Response GetResponse(string portfolio, string owner, string instrument, DateTime time)
        {
            var db = new PriceInfosDB();
            int period = -1;
            if (time > StartDate)
            period = PeriodFromDate(time);
            double sum = 0;
            int count = 0;
            var priceInfos = db.PriceInfos.Where(x => !string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true &&
                !string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true
                && !string.IsNullOrEmpty(instrument) ? x.Instrument.Equals(instrument, StringComparison.CurrentCultureIgnoreCase) : true &&
                period != -1 ? x.Period == period : true).ToList();
            var x = db.PriceInfos[3];
            Console.WriteLine(!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true);
            Console.WriteLine(!string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true);
            Console.WriteLine(!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true);
            foreach (var info in priceInfos)
                sum += info.Price;
            if (priceInfos.Count != 0)
                return new Response()
                {
                    date = DateFromPeriod(period),
                    price = (float)Math.Round((sum / priceInfos.Count), 2)
                };
            else return new Response()
            {
                date = DateFromPeriod(period),
                price = -1
            };
        }
        public static Response GetResponse(string portfolio, string owner, string instrument, int time)
        {
            var db = new PriceInfosDB();
            if (time < 0)
                return null;
            int period = time;
            double sum = 0;
            int count = 0;
            var priceInfos = db.PriceInfos.Where(x => !string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true &&
                !string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true
                && !string.IsNullOrEmpty(instrument) ? x.Instrument.Equals(instrument, StringComparison.CurrentCultureIgnoreCase) : true &&
                period != -1 ? x.Period == period : true).ToList();
            var x = db.PriceInfos[3];
            Console.WriteLine(!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true);
            Console.WriteLine(!string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true);
            Console.WriteLine(!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true);
            foreach (var info in priceInfos)
                sum += info.Price;
            if (priceInfos.Count != 0)
                return new Response()
                {
                    date = DateFromPeriod(period),
                    price = (float)Math.Round((sum / priceInfos.Count), 2)
                };
            else return new Response()
            {
                date = DateFromPeriod(period),
                price = -1
            };
        }
    }
}
