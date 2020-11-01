using System;
using System.ComponentModel.DataAnnotations;

namespace Hmm.IDP.Entities
{
    public class VersionAwareEntity
    {
        [ConcurrencyCheck]
        [MaxLength(36)]
        public string Version { get; set; } = Guid.NewGuid().ToString();
    }
}