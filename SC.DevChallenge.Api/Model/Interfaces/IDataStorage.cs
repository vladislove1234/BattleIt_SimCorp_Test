using System;
using System.Collections.Generic;
using SC.DevChallenge.Api.Model.Entities;

namespace SC.DevChallenge.Api.Model.Interfaces
{
    public interface IDataStorage
    {
        List<PriceInfo> PriceInfos { get; set; }
    }
}
