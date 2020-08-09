using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Validation;
using Hmm.WebConsole.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hmm.WebConsole.Controllers
{
    public class AutomobileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AutomobileController(IHttpClientFactory httpClientFactory)
        {
            Guard.Against<ArgumentNullException>(httpClientFactory == null, nameof(httpClientFactory));
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/automobiles/gaslogs");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return View(new GasLogIndexViewModel(await JsonSerializer.DeserializeAsync<List<GasLog>>(responseStream)));
            }
        }
    }
}