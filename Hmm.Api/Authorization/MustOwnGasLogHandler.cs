using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hmm.Api.Authorization
{
    public class MustOwnGasLogHandler : AuthorizationHandler<MustOwnGasLogRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAutoEntityManager<GasLog> _gasLogManager;

        public MustOwnGasLogHandler(IHttpContextAccessor httpContextAccessor, IAutoEntityManager<GasLog> gasLogManager)
        {
            Guard.Against<ArgumentNullException>(httpContextAccessor == null, nameof(httpContextAccessor));
            Guard.Against<ArgumentNullException>(gasLogManager == null, nameof(gasLogManager));

            _httpContextAccessor = httpContextAccessor;
            _gasLogManager = gasLogManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnGasLogRequirement requirement)
        {
            var logIdText = _httpContextAccessor.HttpContext.GetRouteValue("id").ToString();
            if (!int.TryParse(logIdText, out var logId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userIdText = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (!Guid.TryParse(userIdText, out var userId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (!_gasLogManager.IsEntityOwner(logId, userId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}