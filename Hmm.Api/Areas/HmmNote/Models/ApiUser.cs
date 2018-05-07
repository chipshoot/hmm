using System;
using System.ComponentModel.DataAnnotations;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiUser : ApiEntity
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        [Required]
        [StringLength(256)]
        public string AccountName { get; set; }

        [StringLength(128)]
        public string Password { get; set; }

        public bool IsActivated { get; set; }
    }
}