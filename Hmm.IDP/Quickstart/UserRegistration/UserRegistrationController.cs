using Hmm.IDP.Entities;
using Hmm.IDP.Services;
using Hmm.Utility.Validation;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hmm.IDP.Quickstart.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly ILocalUserService _localUserService;
        private readonly IIdentityServerInteractionService _interaction;

        public UserRegistrationController(ILocalUserService localUserService, IIdentityServerInteractionService interaction)
        {
            Guard.Against<ArgumentNullException>(localUserService == null, nameof(localUserService));
            Guard.Against<ArgumentNullException>(interaction == null, nameof(interaction));

            _localUserService = localUserService;
            _interaction = interaction;
        }

        [HttpGet]
        public IActionResult RegisterUser(string returnUrl)
        {
            var viewModel = new UserRegistrationViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userToCreate = new User
            {
                UserName = model.UserName,
                Subject = Guid.NewGuid().ToString(),
                IsActive = false
            };

            var claims = new List<UserClaim>
            {
                new UserClaim{ Type = JwtClaimTypes.Name, Value = $"{model.UserName}"},
                new UserClaim{ Type = JwtClaimTypes.Address, Value = model.Address},
                new UserClaim{ Type = JwtClaimTypes.FamilyName, Value = model.LastName},
                new UserClaim{ Type = JwtClaimTypes.GivenName, Value = model.FirstName},
                new UserClaim{ Type = JwtClaimTypes.Email, Value = model.Email},
            };

            userToCreate.Claims = claims;
            _localUserService.AddUser(userToCreate, model.Password);
            await _localUserService.SaveChangesAsync();

            // create an activation link
            var link = Url.ActionLink("ActivateUser", "UserRegistration",
                new { securityCode = userToCreate.SecurityCode });

            // todo: send link to user via email

            Debug.WriteLine(link);

            return View("ActivationCodeSent");

            //await HttpContext.SignInAsync(
            //    new IdentityServerUser(userToCreate.Subject) { DisplayName = userToCreate.UserName }
            //    );

            //if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
            //{
            //    return Redirect(model.ReturnUrl);
            //}

            //return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> ActivateUser(string securityCode)
        {
            if (await _localUserService.ActivateUser(securityCode))
            {
                ViewData["Message"] =
                    "Your account was successfully activated. Navigate to your client application to log in.";
            }
            else
            {
                ViewData["Message"] =
                    "Your account could not be activated. Please contact administrator";
            }

            await _localUserService.SaveChangesAsync();
            return View();
        }
    }
}