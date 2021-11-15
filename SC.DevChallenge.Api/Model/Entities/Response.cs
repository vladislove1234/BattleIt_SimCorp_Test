using System;
namespace SC.DevChallenge.Api.Model.Entities
{
    public class Response
    {
        public DateTime date;
        public float price;
        public override string ToString()
        {
            return $"date: {date.ToString()} \n price:{price}";
        }
    }
}
