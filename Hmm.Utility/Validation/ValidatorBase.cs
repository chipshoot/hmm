using System;
using System.Collections.Generic;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;

namespace Hmm.Utility.Validation
{
    public abstract class ValidatorBase<T> : IValidator<T> where T : class
    {
        #region constructor

        protected ValidatorBase(IQuery<T> lookupRepo)
        {
            Guard.Against<ArgumentNullException>(lookupRepo == null, "lookupRepo");
            LookupRepo = lookupRepo;
        }

        #endregion constructor

        #region protected properties

        protected IQuery<T> LookupRepo { get; private set; }

        #endregion protected properties

        public List<string> ValidationErrors { get; protected set; }

        public abstract bool IsValid(T entity, bool isNewEntity);
    }
}