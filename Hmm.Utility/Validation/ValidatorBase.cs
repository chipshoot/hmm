using System;
using System.Collections.Generic;
using Hmm.Utility.Dal;

namespace Hmm.Utility.Validation
{
    public abstract class ValidatorBase<T> : IValidator<T> where T : class
    {
        #region constructor

        protected ValidatorBase(ILookupRepository lookupRepo)
        {
            Guard.Against<ArgumentNullException>(lookupRepo == null, "lookupRepo");
            LookupRepo = lookupRepo;
        }

        #endregion constructor

        #region protected properties

        protected ILookupRepository LookupRepo { get; private set; }

        #endregion protected properties

        public List<string> ValidationErrors { get; protected set; }

        public abstract bool IsValid(T entity, bool isNewEntity);
    }
}