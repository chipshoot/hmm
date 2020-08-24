using Microsoft.AspNetCore.Authorization;

namespace Hmm.Api.Authorization
{
    public class MustOwnGasLogRequirement : IAuthorizationRequirement
    {
        public MustOwnGasLogRequirement()
        {
            
        }
        
    }
}