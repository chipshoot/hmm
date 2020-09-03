using Hmm.IDP.DbContexts;
using Hmm.IDP.Entities;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Hmm.IDP.Services
{
    public class LocalUserService : ILocalUserService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LocalUserService(IdentityDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            Guard.Against<ArgumentNullException>(dbContext == null, nameof(dbContext));
            Guard.Against<ArgumentNullException>(passwordHasher == null, nameof(passwordHasher));

            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success;
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

        public void AddUser(User userToAdd, string password)
        {
            Guard.Against<ArgumentNullException>(userToAdd == null, nameof(userToAdd));
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(password), nameof(password));

            if (_dbContext.Users.Any(u => u.UserName == userToAdd.UserName))
            {
                throw new Exception("Username must be unique");
            }

            using (var randomNumberGen = new RNGCryptoServiceProvider())
            {
                var securityCodeData = new byte[128];
                randomNumberGen.GetBytes(securityCodeData);
                userToAdd!.SecurityCode = Convert.ToBase64String(securityCodeData);
            }
            userToAdd.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);
            userToAdd!.Password = _passwordHasher.HashPassword(userToAdd, password);
            _dbContext.Users.Add(userToAdd!);
        }

        public async Task<bool> ActivateUser(string securityCode)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(securityCode), nameof(securityCode));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.SecurityCode == securityCode && u.SecurityCodeExpirationDate >= DateTime.UtcNow);
            if (user == null)
            {
                return false;
            }

            user.IsActive = true;
            user.SecurityCode = null;
            return true;
        }

        public async Task<bool> IsUserActive(string subject)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(subject), nameof(subject));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Subject == subject);
            return user != null && user.IsActive;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}