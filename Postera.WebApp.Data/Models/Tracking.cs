using System;
using Postera.WebApp.Data.Enums;

namespace Postera.WebApp.Data.Models
{
    public class Tracking
    {
        public Guid Id { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Price { get; set; }

        public double Weight { get; set; }

        public User DestinationClient { get; set; }

        public Address CurrentLocation { get; set; }

        public RoutePoint CurrentCoordinates { get; set; }

        public DateTime ExpectedArrivalDate { get; set; }

        public Storage SourceStorage { get; set; }

        public Storage DestinationStorage { get; set; }
    }
}