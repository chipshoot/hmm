using Hmm.IDP.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hmm.IDP.Services
{
    public interface ILocalUserService
    {
        Task<bool> ValidateClearTextCredentialsAsync(string username, string password);

        Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject);

        Task<User> GetUserByUserNameAsync(string username);

        void AddUser(User userToAdd);

        Task<bool> IsUserActive(string subject);

        Task<bool> SaveChangesAsync();
    }
}