using DomainEntity.User;
using Hmm.Contract.Core;
using Hmm.Core.Manager.Validation;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Encrypt;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Core.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IDataStore<User> _dataSource;
        private readonly UserValidator _validator;

        public UserManager(IDataStore<User> dataSource, UserValidator validator)
        {
            Guard.Against<ArgumentNullException>(dataSource == null, nameof(dataSource));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));
            _dataSource = dataSource;
            _validator = validator;
        }

        public User Create(User userInfo)
        {
            if (!_validator.IsValidEntity(userInfo, ProcessResult))
            {
                return null;
            }

            // Get password salt
            GetPassword(userInfo);
            try
            {
                var addedUsr = _dataSource.Add(userInfo);
                return addedUsr;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public User Update(User userInfo)
        {
            try
            {
                if (!_validator.IsValidEntity(userInfo, ProcessResult))
                {
                    return null;
                }

                var updatedUser = _dataSource.Update(userInfo);
                if (updatedUser == null)
                {
                    ProcessResult.PropagandaResult(_dataSource.ProcessMessage);
                }

                return updatedUser;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public bool ResetPassword(int userId, string newPassword)
        {
            try
            {
                var user = GetEntities().FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    ProcessResult.Success = false;
                    ProcessResult.AddMessage($"Cannot find user with id {userId}");
                    return false;
                }

                user.Password = newPassword;
                if (!_validator.IsValidEntity(user, ProcessResult))
                {
                    return false;
                }

                GetPassword(user);
                var updatedUser = _dataSource.Update(user);
                if (updatedUser == null)
                {
                    ProcessResult.PropagandaResult(_dataSource.ProcessMessage);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return false;
            }
        }

        public IEnumerable<User> GetEntities()
        {
            try
            {
                var users = _dataSource.GetEntities();

                return users;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public void DeActivate(int id)
        {
            var user = _dataSource.GetEntities().FirstOrDefault(u => u.Id == id && u.IsActivated);
            if (user == null)
            {
                ProcessResult.Success = false;
                ProcessResult.AddMessage($"Cannot find user with id : {id}", true);
            }
            else
            {
                try
                {
                    user.IsActivated = false;
                    _dataSource.Update(user);
                }
                catch (Exception ex)
                {
                    ProcessResult.WrapException(ex);
                }
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();

        private static void GetPassword(User userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.Salt))
            {
                userInfo.Salt = EncryptHelper.GenerateSalt();
            }

            var pwd = EncryptHelper.EncodePassword(userInfo.Password, userInfo.Salt, false);
            userInfo.Password = pwd;
        }
    }
}