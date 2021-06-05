using System;
using System.Collections.Generic;

namespace Postera.WebApp.Data.Models
{
    public class PriceModel
    {
        public Guid PostOfficeId { get; set; }

        public Address SourceAddress { get; set; }

        public Address DestinationAddress { get; set; }

        public List<Package> Packages { get; set; }
    }
}