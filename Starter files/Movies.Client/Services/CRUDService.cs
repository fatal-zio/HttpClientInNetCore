using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public CRUDService()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public async Task Run()
        {
            await GetResource();
        }

        public async Task<IEnumerable<Movie>> GetResource()
        {
            var response = await _httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var movies = new List<Movie>();

            switch (response.Content.Headers.ContentType.MediaType)
            {
                case "application/json":
                    movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(content).ToList();
                    break;
                case "application/xml":
                {
                    var serializer = new XmlSerializer(typeof(List<Movie>));
                    movies = (List<Movie>) serializer.Deserialize(new StringReader(content));
                    break;
                }
            }

            return movies;
        }
    }
}
