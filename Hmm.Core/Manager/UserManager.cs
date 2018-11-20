using DomainEntity.User;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Encrypt;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Contract.Core;

namespace Hmm.Core.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IDataStore<User> _dataSource;

        public UserManager(IDataStore<User> dataSource)
        {
            Guard.Against<ArgumentNullException>(dataSource == null, nameof(dataSource));
            _dataSource = dataSource;
        }

        public User Create(User userInfo)
        {
            // Get password salt
            if (string.IsNullOrEmpty(userInfo.Salt))
            {
                userInfo.Salt = EncryptHelper.GenerateSalt();
            }

            var pwd = EncryptHelper.EncodePassword(userInfo.Password, userInfo.Salt, false);
            userInfo.Password = pwd;

            try
            {
                var addedUsr = _dataSource.Add(userInfo);
                return addedUsr;
            }
            catch (Exception ex)
            {
                ProcessResult.Success = false;
                ProcessResult.MessageList.Add(ex.Message);
                return null;
            }
        }

        public User Update(User userInfo)
        {
            try
            {
                var updatedUser = _dataSource.Update(userInfo);
                return updatedUser;
            }
            catch (Exception ex)
            {
                ProcessResult.Success = false;
                ProcessResult.MessageList.Add(ex.Message);
                return null;
            }
        }

        public User FindUser(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.Id == id);
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = _dataSource.GetEntities();

            return users;
        }

        public void Delete(int id)
        {
            var user = _dataSource.GetEntities().FirstOrDefault(u => u.Id == id && u.IsActivated);
            if (user == null)
            {
                ProcessResult.Rest();
                ProcessResult.Success = false;
                ProcessResult.AddMessage($"Cannot find user with id : {id}");
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
                    ProcessResult.Success = false;
                    ProcessResult.AddMessage(ex.Message);
                    ProcessResult.AddMessage(ex.InnerException.Message);
                }
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();
    }
}