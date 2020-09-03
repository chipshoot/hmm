using Microsoft.AspNetCore.Authorization;

namespace Hmm.Api.Authorization
{
    /// <summary>
    /// The requirement is used to confirm that only author can browse his/her note
    /// </summary>
    public class MustOwnGasLogRequirement : IAuthorizationRequirement
    {
        public MustOwnGasLogRequirement()
        {
            
        }
        
    }
}