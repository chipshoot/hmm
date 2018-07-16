using DomainEntity.User;
using Hmm.Utility.Misc;

namespace Hmm.Contract
{
    public interface IUserManager
    {
        /// <summary>
        /// Creates the specified user with user information.
        /// </summary>
        /// <param name="userinfo">The <see cref="User"/> object which contains all
        /// new user information except user id.</param>
        /// <returns>if user successfully be created, return the user with unique id,
        /// otherwise return null</returns>
        User Create(User userinfo);

        /// <summary>
        /// Updates the specified user with new information.
        /// </summary>
        /// <param name="userInfo">The <see cref="User"/> with update information and id </param>
        /// <returns>if use has been updated successfully, return updated user, otherwise return
        ///  null </returns>
        User Update(User userInfo);


        User FindUser(int id);

        void Delete(int id);

        ProcessingResult ErrorMessage { get; }
    }
}