using Hmm.DtoEntity.Api.HmmNote;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Hmm.WebConsole.ViewModels;

namespace Hmm.WebConsole.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private List<ApiSubsystem> _subsystems;
        private SectionInfo _subsystemInfo;
        private readonly IHttpClientFactory _httpClientFactory;

        public NavigationMenuViewComponent(IHttpClientFactory httpClientFactory)
        {
            Guard.Against<ArgumentNullException>(httpClientFactory == null, nameof(httpClientFactory));
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await SetupSubsystems();
            var viewMode = new NavigationComponentViewModel
            {
                Subsystems = _subsystems,
                SubsystemsInfo = _subsystemInfo
            };
            return View(viewMode);
        }

        private async Task SetupSubsystems()
        {
            if (_subsystems != null)
            {
                return;
            }

            var client = _httpClientFactory.CreateClient(HmmWebConsoleConstants.HttpClient.Api);
            var request = new HttpRequestMessage(HttpMethod.Get, HmmWebConsoleConstants.Urls.GetSubsystems);
            var response = await client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var subsystems = await JsonSerializer.DeserializeAsync<List<ApiSubsystem>>(responseStream);
                _subsystems = subsystems.ToList();
                _subsystemInfo = new SectionInfo
                {
                    ItemsPerSection = subsystems.Count,
                    TotalItems = subsystems.Count
                };
            }
        }
    }
}