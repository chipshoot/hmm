using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hmm.IDP.Entities
{
    public class User : VersionAwareEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Subject { get; set; }

        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public IEnumerable<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}