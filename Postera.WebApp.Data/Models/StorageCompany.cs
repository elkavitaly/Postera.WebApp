using System;
using System.Collections.Generic;

namespace Postera.WebApp.Data.Models
{
    public class StorageCompany
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        public List<Storage> Storages { get; set; }
    }
}