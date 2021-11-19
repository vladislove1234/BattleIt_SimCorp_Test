using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;
using SC.DevChallenge.Api.Model.Entities;

namespace SC.DevChallenge.Api.Model.Services
{
    public class CSVHelper
    {
        public List<PriceInfo> GetPriceInfos(string path)
        {
            var list = new List<PriceInfo>();
            using (var reader = new StreamReader(path))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        var priceInfo = new PriceInfo();
                    try
                    {
                        priceInfo.Portfolio = values[0];
                        priceInfo.InstrumentOwner = values[1];
                        priceInfo.Instrument = values[2];
                        priceInfo.Date = DateTime.ParseExact(values[3], "dd/MM/yyyy HH:mm:ss", null);
                        priceInfo.Price = (float)Convert.ToDouble(values[4]);
                        priceInfo.Period = PriceManager.PeriodFromDate(priceInfo.Date);
                        list.Add(priceInfo);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Failed to parse date{values[3]}");
                    }
                }

            }
            return list;
        }
    }
}
