using System;
using System.Collections.Generic;
using System.Linq;
using SC.DevChallenge.Api.Model.Entities;
using SC.DevChallenge.Api.Model.Interfaces;

namespace SC.DevChallenge.Api.Model.Services
{
    public class FileDataStorage : IDataStorage
    {
        public FileDataStorage()
        {
            var csvHelper = new CSVHelper();
            _priceInfos = csvHelper.GetPriceInfos("./Input/data.csv");
        }
        public FileDataStorage(string path)
        {
            var csvHelper = new CSVHelper();
            _priceInfos = csvHelper.GetPriceInfos(path);
        }
        public List<PriceInfo> PriceInfos
        {
            get
            {
                return _priceInfos;
            }
            set
            {
                _priceInfos = value;
            }
        }
        private List<PriceInfo> _priceInfos;

        public List<PriceInfo> GetPriceinfosFromPIIT(string portfolio, string owner, string instrument, int period)
        {
            return PriceInfos.Where(x => (!string.IsNullOrEmpty(portfolio) ? x.Portfolio.Equals(portfolio, StringComparison.CurrentCultureIgnoreCase) : true) &&
                (!string.IsNullOrEmpty(owner) ? x.InstrumentOwner.Equals(owner, StringComparison.CurrentCultureIgnoreCase) : true)
                && (!string.IsNullOrEmpty(instrument) ? x.Instrument.Equals(instrument, StringComparison.CurrentCultureIgnoreCase) : true) &&
                (period != -1 ? x.Period == period : true)).ToList();
        }
    }
}
