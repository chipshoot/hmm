using DomainEntity.User;
using Hmm.Contract;
using Hmm.Utility.Dal.DataStore;
using Hmm.Utility.Encrypt;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Linq;

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

        public User Create(User userinfo)
        {
            // Get password salt
            if (string.IsNullOrEmpty(userinfo.Salt))
            {
                userinfo.Salt = EncryptHelper.GenerateSalt();
            }

            var pwd = EncryptHelper.EncodePassword(userinfo.Password, userinfo.Salt, false);
            userinfo.Password = pwd;

            try
            {
                var addedusr = _dataSource.Add(userinfo);
                return addedusr;
            }
            catch (Exception ex)
            {
                ErrorMessage.Success = false;
                ErrorMessage.MessageList.Add(ex.Message);
                return null;
            }
        }

        public User Update(User userInfo)
        {
            try
            {
                var updatedusr = _dataSource.Update(userInfo);
                return updatedusr;
            }
            catch (Exception ex)
            {
                ErrorMessage.Success = false;
                ErrorMessage.MessageList.Add(ex.Message);
                return null;
            }
        }

        public User FindUser(int id)
        {
            var user = _dataSource.GetEntities().FirstOrDefault(u => u.Id == id);
            return user;
        }

        public void Delete(int id)
        {
            var user = _dataSource.GetEntities().FirstOrDefault(u => u.Id == id && u.IsActivated);
            if (user == null)
            {
                ErrorMessage.Rest();
                ErrorMessage.Success = false;
                ErrorMessage.AddMessage($"Cannot find user with id : {id}");
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
                    ErrorMessage.Success = false;
                    ErrorMessage.AddMessage(ex.Message);
                    ErrorMessage.AddMessage(ex.InnerException.Message);
                }
            }
        }

        public ProcessingResult ErrorMessage { get; } = new ProcessingResult();
    }
}