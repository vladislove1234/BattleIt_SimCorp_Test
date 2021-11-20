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
            if (date < StartDate)
                return -1;
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
        
    }
}
