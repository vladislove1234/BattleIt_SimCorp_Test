using System;
using System.Collections.Generic;
using System.Linq;
using SC.DevChallenge.Api.Model.Entities;
using SC.DevChallenge.Api.Model.Interfaces;

namespace SC.DevChallenge.Api.Model.Services
{
    public class BenchmarkCalculator
    {
        private IDataStorage _prices;
        public BenchmarkCalculator(IDataStorage prices)
        {
            _prices = prices;
        }
        public double? CalculatePrice(List<PriceInfo> infos)
        {
            float[] prices = getArrayFromPriceInfoList(infos);//sorted prices
            int q1, q3;
            if (prices.Length < 4)// if size of prisez is 1,2 or 3
            {
                q1 = 0;
                q3 = prices.Length - 1;
            }
            else// if size is bigger
            {
                q1 = (int)Math.Ceiling((double)((infos.Count - 1) / 4));
                q3 = (int)Math.Ceiling((double)((3 * infos.Count - 3) / 4));
            }
            int IQR = q3 - q1;
            double avrPrice = 0;
            int counter = 0;
            foreach(var info in infos)//calculating avarage price
            {
                if(info.Price >= prices[q1] - 1.5* IQR && info.Price <= prices[q3] + 1.5 * IQR) 
                {
                    counter++;
                    avrPrice += info.Price;
                }
            }
            if (counter != 0)
                return avrPrice / counter;
            else
                return null;

        }
        public List<Response> CalculateAggregateBenchmark(string portfolio,int startTs, int endTs, int groups)
        {
            var tsGroups = getDevidedGroups(endTs - startTs, groups);//groups of intervals
            var responses = new List<Response>();
            int tsDelta = 0;//time slot delta
            foreach(var tsGroup in tsGroups)
            {
                if(tsGroup > 0)
                {
                    int countOfBenchmarks = 0;
                    double avrPrice = 0;
                    for(int i = 0; startTs + tsDelta + i < startTs + tsDelta + tsGroup; i++)//calculating all benchmarks
                    {
                        var infos = _prices.GetPriceinfosFromPIIT(portfolio, "", "", startTs + tsDelta + i);// getting all price infos in current Time slot
                        double? currTsAvrPrice = CalculatePrice(infos);
                        if(currTsAvrPrice != null)
                        {
                            countOfBenchmarks++;
                            avrPrice += (double)currTsAvrPrice;
                        }
                    }
                    if(avrPrice != 0)//if average price was calculted succesfully
                    {
                        responses.Add(new Response()
                        {
                            date = PriceManager.DateFromPeriod(startTs + tsDelta),
                            price = (float)Math.Round((double)(avrPrice / countOfBenchmarks),2),
                        });
                    }
                    else
                    {
                        responses.Add(new Response()
                        {
                            date = DateTime.MinValue,
                            price = 0,
                            exception = "Not found enough price infos to calculate benchmark"
                        });
                    }
                    tsDelta += tsGroup;
                }
                else //if intervals count is bigger than count of timeslots
                {
                    responses.Add(new Response()
                    {
                        date = DateTime.MinValue,
                        price = 0,
                        exception = "Count of intervals is greater than count of timeSlots!"
                    });
                }
            }
            return responses;
        }
        private int[] getDevidedGroups(int count, int groups)//devide timeslots by intervals
        {
            int[] arrOfGroups;
            if (count < groups)
            {
                arrOfGroups = new int[groups];
                int temp = count;
                for(int i = 0; i < groups; i++)
                {
                    arrOfGroups[i] = 1;
                    temp--;
                    if (temp == 0)
                        break;
                }
            }
            else
            {
                int ostacha = count % groups;
                arrOfGroups = Enumerable.Repeat(count / groups, groups).ToArray();
                for (int i = 0; i < groups; i++)
                {
                    arrOfGroups[i] += 1;
                    ostacha--;
                    if (ostacha == 0)
                        break;
                }
            }
            return arrOfGroups;
        }
        private float[] getArrayFromPriceInfoList(List<PriceInfo> infos)// getting sorted array of prices from list of PriceInfo
        {
            float[] retArr = new float[infos.Count];
            for(int i = 0; i < infos.Count; i++)
                retArr[i] = infos[i].Price;
            Array.Sort(retArr);
            return retArr;
        }
        
    }
}