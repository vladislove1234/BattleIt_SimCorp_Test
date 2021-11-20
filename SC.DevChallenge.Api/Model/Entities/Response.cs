using System;
namespace SC.DevChallenge.Api.Model.Entities
{
    public class Response
    {
        public DateTime date { get; set; }
        public float price { get; set; }
        public string exception { get; set; }
        public override string ToString()
        {
            return $"date: {date.ToString()} \n price:{price}";
        }
    }
}
