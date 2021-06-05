using System;

namespace Postera.WebApp.Data.ResponseModels
{
    public class PriceResult
    {
        public decimal Price { get; set; }

        public double Weight { get; set; }

        public DateTime ExpectedArrivalDate { get; set; }
    }
}