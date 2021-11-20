using System;
using System.Collections.Generic;
using SC.DevChallenge.Api.Model.Entities;

namespace SC.DevChallenge.Api.Model.Interfaces
{
    public interface IDataStorage
    {
        List<PriceInfo> PriceInfos { get; set; }//all price infos
        List<PriceInfo> GetPriceinfosFromPIIT(string portfolio, string owner, string instrument, int period);// getting price infos for PIIT
        List<PriceInfo> GetPriceinfosFromPIIT(string portfolio, string owner, string instrument, int periodStart,int periodEnd);// getting price infos for PII and range of time slot
    }
}
