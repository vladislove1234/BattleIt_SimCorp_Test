using System;
namespace SC.DevChallenge.Api.Model.Entities
{
    public class PriceInfo
    {
        public string Portfolio { get; set; }
        public string InstrumentOwner { get; set; }
        public string Instrument { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
        public int Period { get; set; }
    }
}
