using System;
using Postera.WebApp.Data.Enums;

namespace Postera.WebApp.Data.Models
{
    public class Role
    {
        public Guid ItemId { get; set; }

        public string ItemType { get; set; }

        public RoleType Type { get; set; }
    }
}