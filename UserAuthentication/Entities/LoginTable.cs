using System;
using System.Collections.Generic;

namespace UserAuthentication.Entities
{
    public partial class LoginTable : BaseEntity
    {
        public int Id { get; set; }
        public string LoginMethod { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public string Email { get; set; } = null!;
        public string? Status { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? LoginExpireTime { get; set; }
    }
}
