using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Postera.WebApp.Data.Enums;

namespace Postera.WebApp.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime? ArriveDate { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public Guid PostOfficeId { get; set; }

        public PostOffice PostOffice { get; set; }

        [Required]
        public Guid SourceStorageId { get; set; }

        public Storage SourceStorage { get; set; }

        public string SourceClientEmail { get; set; }

        public User SourceClient { get; set; }

        [Required]
        public Guid DestinationStorageId { get; set; }

        public Storage DestinationStorage { get; set; }

        [Required]
        public string DestinationClientId { get; set; }

        public User DestinationClient { get; set; }

        public List<Package> Packages { get; set; }
    }
}