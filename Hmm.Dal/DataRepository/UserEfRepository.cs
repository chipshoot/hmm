using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Hmm.Dal.DataRepository
{
    public class UserEfRepository : IGuidRepository<User>
    {
        private readonly IHmmDataContext _dataContext;
        private readonly IEntityLookup _lookupRepo;

        public UserEfRepository(IHmmDataContext dataContext, IEntityLookup lookupRepo)
        {
            Guard.Against<ArgumentNullException>(dataContext == null, nameof(dataContext));
            Guard.Against<ArgumentNullException>(lookupRepo == null, nameof(lookupRepo));

            _dataContext = dataContext;
            _lookupRepo = lookupRepo;
        }

        public IQueryable<User> GetEntities(Expression<Func<User, bool>> query = null)
        {
            return _lookupRepo.GetEntities<User>(query);
        }

        public User Add(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _dataContext.Users.Add(entity);
                Flush();
                return entity;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public User Update(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once PossibleNullReferenceException
                if (entity.Id == Guid.Empty)
                {
                    ProcessMessage.Success = false;
                    ProcessMessage.AddErrorMessage($"Can not update user with id {entity.Id}");
                    return null;
                }

                _dataContext.Users.Update(entity);
                Flush();
                var updateUser = _lookupRepo.GetEntity<User>(entity.Id);
                return updateUser;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public bool Delete(User entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _dataContext.Users.Remove(entity);
                Flush();
                return true;
            }
            catch (DataSourceException ex)
            {
                ProcessMessage.WrapException(ex);
                return false;
            }
        }

        public void Flush()
        {
            _dataContext.Save();
        }

        public ProcessingResult ProcessMessage { get; } = new ProcessingResult();
    }
}