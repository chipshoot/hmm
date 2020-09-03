using Hmm.DtoEntity.Api.HmmNote;
using Hmm.Utility.Validation;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Hmm.WebConsole.Infrastructure.PostConfigurationOptions
{
    public class OpenIdConnectOptionsPostConfigureOptions : IPostConfigureOptions<OpenIdConnectOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenIdConnectOptionsPostConfigureOptions(IHttpClientFactory httpClientFactory)
        {
            Guard.Against<ArgumentNullException>(httpClientFactory == null, nameof(httpClientFactory));

            _httpClientFactory = httpClientFactory;
        }

        public void PostConfigure(string name, OpenIdConnectOptions options)
        {
            options.Events = new OpenIdConnectEvents
            {
                OnTicketReceived = async ticketReceiveContext =>
                {
                    var subject = ticketReceiveContext.Principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    var apiClient = _httpClientFactory.CreateClient(HmmWebConsoleConstants.HttpClient.ApiNoToken);

                    var request = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"/api/authors/{subject}");
                    request.SetBearerToken(
                        ticketReceiveContext.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken));

                    var response = await apiClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();
                    ApiAuthor authors;
                    await using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        authors = await JsonSerializer.DeserializeAsync<ApiAuthor>(responseStream);
                    }

                    var newClaimsIdentity = new ClaimsIdentity();
                    newClaimsIdentity.AddClaim(
                        new Claim("role", authors.Role)
                    );
                    ticketReceiveContext.Principal.AddIdentity(newClaimsIdentity);
                }
            };
        }
    }
}