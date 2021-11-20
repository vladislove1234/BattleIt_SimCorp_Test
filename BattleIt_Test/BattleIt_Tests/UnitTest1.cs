using System;
using SC.DevChallenge.Api.Model.Interfaces;
using SC.DevChallenge.Api.Controllers;
using Xunit;
using SC.DevChallenge.Api.Model.Services;
using System.Threading.Tasks;
using SC.DevChallenge.Api.Model.Entities;

namespace BattleIt_Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            IDataStorage storage = new FileDataStorage("/Users/vlad/Documents/GitHub/BattleIt_SimCorp_Test/BattleIt_Test/BattleIt_Tests/Input/data.csv");
            var controller = new PricesController(storage);

            var response = controller.Average("State Bank of India", "Google", "Loan",  DateTime.ParseExact("14/07/2018 11:46:55", "dd/MM/yyyy HH:mm:ss", null));
            var model = Assert.IsAssignableFrom<Response>(response);
            Assert.Equal(200, model.price);
        }
        public async Task RequestTest()
        {
            PriceInfo x = new PriceInfo() { Portfolio = "" };
            /*(x => (!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true)
                && (!string.IsNullOrEmpty(instrument) ? x.Instrument.Equals(instrument, StringComparison.CurrentCultureIgnoreCase) : true) &&
                (period != -1 ? x.Period == period : true)*/
        }
    }
}
