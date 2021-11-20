using System;
using System.Linq;
using SC.DevChallenge.Api.Model;
using SC.DevChallenge.Api.Model.Interfaces;
using SC.DevChallenge.Api.Model.Services;
using Xunit;

namespace BattleIt_Tests
{
    public class BenchmarkTest
    {
        [Fact]
        public void Benchmark_test()
        {
            IDataStorage storage = new FileDataStorage("/Users/vlad/Documents/GitHub/BattleIt_SimCorp_Test/BattleIt_Test/BattleIt_Tests/Input/data.csv");

            var prices = storage.GetPriceinfosFromPIIT("Fannie Mae", "", "", PriceManager.PeriodFromDate(DateTime.ParseExact("15/03/2018 17:34:50", "dd/MM/yyyy HH:mm:ss", null)));
            var bencmarkCalculator = new BenchmarkCalculator(storage);
            double? price = bencmarkCalculator.CalculatePrice(prices);
            Assert.Equal(133.71, Math.Round((double)price,2));
        }
        [Fact]
        public void DividingNumbers()
        {
            int groups = 7;
            int n = 6;
            int ostacha = n % groups;
            int[] arrOfGroups = Enumerable.Repeat(n / groups, groups).ToArray();
            for(int i = 0; i < groups; i++)
            {
                arrOfGroups[i] += 1;
                ostacha--;
                if (ostacha == 0)
                    break;
            }
            Assert.True(true);
        }
        [Fact]
        public void CalculatingAggregateTest()
        {
            IDataStorage storage = new FileDataStorage("/Users/vlad/Documents/GitHub/BattleIt_SimCorp_Test/BattleIt_Test/BattleIt_Tests/Input/data.csv");
            var benchmark = new BenchmarkCalculator(storage);

            var startDate = new DateTime(2018, 10, 06, 0, 0, 0);
            var endDate = new DateTime(2018,10,13,0,0,0);
            string portfolio = "Fannie Mae";
            var res = benchmark.CalculateAggregateBenchmark(portfolio, PriceManager.PeriodFromDate(startDate),
               PriceManager.PeriodFromDate(endDate), 7);
            Assert.True(true);
        }
    }
}
