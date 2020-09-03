using Hmm.Contract.Core;
using Hmm.Core.Manager.Validation;
using Hmm.DomainEntity.User;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Core.Manager
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IGuidRepository<Author> _userRepo;
        private readonly AuthorValidator _validator;

        public AuthorManager(IGuidRepository<Author> userRepo, AuthorValidator validator)
        {
            Guard.Against<ArgumentNullException>(userRepo == null, nameof(userRepo));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));
            _userRepo = userRepo;
            _validator = validator;
        }

        public Author Create(Author authorInfo)
        {
            if (!_validator.IsValidEntity(authorInfo, ProcessResult))
            {
                return null;
            }

            try
            {
                var addedUsr = _userRepo.Add(authorInfo);
                return addedUsr;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public bool AuthorExists(string id)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(id), nameof(id));

            if (!Guid.TryParse(id, out Guid userId))
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return GetEntities().Any(u => u.Id == userId);
        }

        public Author Update(Author authorInfo)
        {
            try
            {
                if (!_validator.IsValidEntity(authorInfo, ProcessResult))
                {
                    return null;
                }

                var updatedUser = _userRepo.Update(authorInfo);
                if (updatedUser == null)
                {
                    ProcessResult.PropagandaResult(_userRepo.ProcessMessage);
                }

                return updatedUser;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public IEnumerable<Author> GetEntities()
        {
            try
            {
                var users = _userRepo.GetEntities();

                return users;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public void DeActivate(Guid id)
        {
            var user = _userRepo.GetEntities().FirstOrDefault(u => u.Id == id && u.IsActivated);
            if (user == null)
            {
                ProcessResult.Success = false;
                ProcessResult.AddErrorMessage($"Cannot find user with id : {id}", true);
            }
            else
            {
                try
                {
                    user.IsActivated = false;
                    _userRepo.Update(user);
                }
                catch (Exception ex)
                {
                    ProcessResult.WrapException(ex);
                }
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();
    }
}