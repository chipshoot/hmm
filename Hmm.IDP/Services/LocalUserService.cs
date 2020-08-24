using Hmm.IDP.DbContexts;
using Hmm.IDP.Entities;
using Hmm.Utility.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hmm.IDP.Services
{
    public class LocalUserService : ILocalUserService
    {
        private readonly IdentityDbContext _dbContext;

        public LocalUserService(IdentityDbContext dbContext)
        {
            Guard.Against<ArgumentNullException>(dbContext == null, nameof(dbContext));

            _dbContext = dbContext;
        }

        public async Task<bool> ValidateClearTextCredentialsAsync(string userName, string password)
        {
            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            return user.Password == password;
        }

        public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(subject), nameof(subject));
            return await _dbContext.UserClaims.Where(uc => uc.User.Subject == subject).ToListAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(userName), nameof(userName));

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User> GetUserBySubjectAsync(string subject)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(subject), nameof(subject));

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject);
        }

        public void AddUser(User userToAdd)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsUserActive(string subject)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(subject), nameof(subject));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject);
            return user != null && user.IsActive;
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}