using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hmm.Api.Models;
using Hmm.Api.Models.Validation;

namespace Hmm.Api.Areas.HmmNote.Models
{
    public class ApiNote : ApiEntity, IValidatableObject
    {
        [StringLength(1000)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public ApiUser Author { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CreateDate > DateTime.Now)
            {
                var error = new ValidationError($"The create date {CreateDate} is in the future", nameof(CreateDate));
            }
            var errRst = new ValidationFailedResult();
            throw new NotImplementedException();
        }
    }
}