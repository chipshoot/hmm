using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hmm.Api.Areas.GaslogNote.Models
{
    public class ApiGaslog : ApiEntity, IValidatableObject
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public ApiUser Author { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public float Distance { get; set; }

        public float Gas { get; set; }

        public decimal Price { get; set; }

        public List<ApiDiscount> Discounts { get; set; }

        public string GasStation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}