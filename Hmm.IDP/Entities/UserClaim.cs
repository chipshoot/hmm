using System;
using System.ComponentModel.DataAnnotations;

namespace Hmm.IDP.Entities
{
    public class UserClaim : VersionAwareEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(250)]
        [Required]
        public string Type { get; set; }

        [MaxLength(250)]
        [Required]
        public string Value { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}