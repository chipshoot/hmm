using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;
using System;

namespace Hmm.Dal.Querys
{
    public class EfEntityLookup : IEntityLookup
    {
        private readonly IHmmDataContext _dataContext;

        public EfEntityLookup(IHmmDataContext dataContext)
        {
            Guard.Against<ArgumentNullException>(dataContext == null, nameof(dataContext));
            _dataContext = dataContext;
        }

        public T GetEntity<T>(int id) where T : Entity
        {
            T entity = null;
            if (typeof(T) == typeof(User))
            {
                entity = _dataContext.Users.Find(id) as T;
            }
            else if (typeof(T) == typeof(NoteRender))
            {
                entity = _dataContext.Renders.Find(id) as T;
            }

            return entity;
        }
    }
}