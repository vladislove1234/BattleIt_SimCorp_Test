using System;
using SC.DevChallenge.Api.Model;
using Xunit;

namespace BattleIt_Tests
{
    public class PriceManagerTest
    {
        [Fact]
        public void TimeSlotConvert()
        {
            int period = 2409;
            DateTime converted = PriceManager.DateFromPeriod(period);
            DateTime trueDate = DateTime.ParseExact("06/10/2018 19:40:00", "dd/MM/yyyy HH:mm:ss", null);
            Assert.Equal(trueDate, converted);
        }
    }
}
