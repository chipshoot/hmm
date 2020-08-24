using Hmm.Utility.Validation;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hmm.IDP.Services
{
    public class LocalUserProfileService : IProfileService
    {
        private readonly ILocalUserService _localUserService;

        public LocalUserProfileService(ILocalUserService localUserService)
        {
            Guard.Against<ArgumentNullException>(localUserService == null, nameof(localUserService));

            _localUserService = localUserService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var userClaims = (await _localUserService.GetUserClaimsBySubjectAsync(subjectId)).ToList();

            // the claim in context is all lower case
            var claims = userClaims.Select(c => new Claim(c.Type.ToLower(), c.Value)).ToHashSet();
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = await _localUserService.IsUserActive(subjectId);
        }
    }
}