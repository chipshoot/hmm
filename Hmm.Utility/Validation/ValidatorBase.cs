using Hmm.Utility.Dal.Query;
using System;
using System.Collections.Generic;

namespace Hmm.Utility.Validation
{
    public abstract class ValidatorBase<T> : IValidator<T> where T : class
    {
        #region constructor

        protected ValidatorBase(IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(lookupRepo == null, "lookupRepo");
            LookupRepo = lookupRepo;
        }

        #endregion constructor

        #region protected properties

        protected IEntityLookup LookupRepo { get; }

        #endregion protected properties

        public List<string> ValidationErrors { get; } = new List<string>();

        public abstract bool IsValid(T entity, bool isNewEntity);
    }
}