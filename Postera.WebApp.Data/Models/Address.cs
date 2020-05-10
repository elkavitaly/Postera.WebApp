using System.ComponentModel.DataAnnotations;

namespace Postera.WebApp.Data.Models
{
    public class Address
    {
        [Required]
        public string Country { get; set; }

        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        public string District { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string House { get; set; }
    }
}