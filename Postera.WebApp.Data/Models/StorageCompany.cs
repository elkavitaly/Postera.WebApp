using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Postera.WebApp.Data.Models
{
    public class StorageCompany
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public Address Address { get; set; }

        public List<Storage> Storages { get; set; }
    }
}