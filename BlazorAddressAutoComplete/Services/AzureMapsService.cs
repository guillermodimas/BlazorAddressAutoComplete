using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorAddressAutoComplete.Models;

namespace BlazorAddressAutoComplete.Services
{
    public class AzureMapsService : IAzureMapsService
    {
        private HttpClient _apiClient;
        private readonly IConfiguration _config;
        private readonly ILogger<AzureMapsService> _logger;

        public AzureMapsService(IConfiguration config, ILogger<AzureMapsService> logger) //constructor
        {
            _config = config;
            _logger = logger;
            InitializeClient();
        }
        private void InitializeClient()
        {
            string api = _config["AzureMaps:Domain"]; //ConfigurationManager.AppSettings["api"]; //api URL from app.config
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<ResultsAutoComplete> AutocompleteAddress(string Address)
        {

            using (HttpResponseMessage response = await _apiClient.GetAsync($"/search/address/json?typeahead=true&api-version=1&query={Address}&language=en-US&lon=0&lat=0&countrySet=US&view=Auto&subscription-key={_config["AzureMaps:ApiKey"]}"))
            {

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResultsAutoComplete>();
                }
                else
                {
                    _logger.LogError(response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }

}
