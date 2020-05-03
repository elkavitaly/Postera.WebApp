using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Postera.WebApp.Data.Models
{
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public bool IsLocked { get; set; }

        public IList<Role> Roles { get; set; }
    }
}