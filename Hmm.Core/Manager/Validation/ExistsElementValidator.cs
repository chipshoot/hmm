﻿using FluentValidation;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Validation
{
    public class ExistsElementValidator<T> : AbstractValidator<T> where T : Entity
    {
        private readonly IEntityLookup _lookupRepo;

        public ExistsElementValidator(IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));

            _lookupRepo = lookupRepo;
            RuleFor(e => e.Id).GreaterThan(0).Must(Exists);
        }

        private bool Exists(int id)
        {
            var entity = _lookupRepo.GetEntity<T>(id);
            return entity != null;
        }
    }
}