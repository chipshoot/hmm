using AutoMapper;
using Hmm.DtoEntity.Api.GasLogNotes;
using Hmm.Infrastructure;
using Hmm.Utility.Validation;
using Hmm.WebConsole.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hmm.WebConsole.Controllers
{
    [Authorize]
    public class AutomobileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public AutomobileController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            Guard.Against<ArgumentNullException>(httpClientFactory == null, nameof(httpClientFactory));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // todo: remove this helper method in production
            await WriteOutIdentityInformation();

            // todo: remove this in production
            // try to get user address information from IDP's user info endpoint
            var idpClient = _httpClientFactory.CreateClient(HmmWebConsoleConstants.HttpClient.Idp);
            var addr = await IdpUserProfileProvider.GetUserClaimAsync("address", HttpContext, idpClient);

            var httpClient = _httpClientFactory.CreateClient(HmmWebConsoleConstants.HttpClient.Api);
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/automobiles/gaslogs");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var gaslogs = await JsonSerializer.DeserializeAsync<List<ApiGasLog>>(responseStream);
                    return View(new GasLogIndexViewModel(gaslogs, addr, _mapper));
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }

            throw new Exception("Problem accessing the API");
        }

        [Authorize(Policy = HmmWebConsoleConstants.Policy.CanAddGasLog)]
        public IActionResult Add()
        {
            return View(new GasLogAddViewModel(_mapper));
        }

        public async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var tokenInfo = $"Identity Token: {identityToken}";
            Debug.WriteLine(tokenInfo);
            Log.Debug(tokenInfo);

            foreach (var claim in User.Claims)
            {
                var claimInfo = $"Claim type: {claim.Type} - Claim value: {claim.Value}";
                Debug.WriteLine(claimInfo);
                Log.Debug(tokenInfo);
            }
        }
    }
}