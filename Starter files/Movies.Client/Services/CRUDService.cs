using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.Services
{
    public class CrudService : IIntegrationService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public CrudService()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
        }

        public async Task Run()
        {
            await GetResource();
        }

        public async Task GetResource()
        {
            var response = await _httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(content);
        }
    }
}
