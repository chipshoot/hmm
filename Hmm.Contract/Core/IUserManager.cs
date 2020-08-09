using Hmm.DomainEntity.User;
using Hmm.Utility.Misc;
using System;
using System.Collections.Generic;

namespace Hmm.Contract.Core
{
    public interface IUserManager
    {
        IEnumerable<User> GetEntities();

        User Create(User userInfo);

        User Update(User userInfo);

        /// <summary>Update the user account password, because we encrypt the password before save it
        /// to table, so we cannot just update password with other information</summary>
        /// <param name="userId">The user id whose password need to be changed</param>
        /// <param name="newPassword">the new password to be set</param>
        bool ResetPassword(Guid userId, string newPassword);

        /// <summary>
        /// Set the flag to de-activate user to make it invisible for system.
        /// user may associated with note so we not want to delete everything
        /// </summary>
        /// <param name="id">The id of user whose activate flag will be set</param>
        void DeActivate(Guid id);

        ProcessingResult ProcessResult { get; }
    }
}