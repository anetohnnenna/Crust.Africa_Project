using System;
using System.Collections.Generic;

namespace UserAuthentication.Entities
{
    public partial class RegistrationTable : BaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
