using System;
using Postera.WebApp.Data.Enums;

namespace Postera.WebApp.Data.Models
{
    public class Storage
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public StorageType Type { get; set; }

        public float Square { get; set; }

        public Address Address { get; set; }

        public Guid StorageCompanyId { get; set; }
    }
}