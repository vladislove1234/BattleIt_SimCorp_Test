using System;
using System.Collections.Generic;
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
    }
}
