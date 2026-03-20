using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Driver;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;

namespace LancerWebAPI.Services
{
    public class GoogleMapsAPIService
    {
        private readonly string? _apiKey;
        private readonly string? _apiKeySecret;
        private readonly string? _url;
        private readonly HttpClient _httpClient;

        public GoogleMapsAPIService(IConfiguration config)
        {
            _apiKeySecret = Environment.GetEnvironmentVariable("G_PLACEID_URL");
            _apiKey = config["GoogleMaps:G_API_KEY"];
            _url = config["GoogleMaps:BaseUrl"];

            _httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(_url))
            {
                _httpClient.BaseAddress = new Uri(_url);
            }

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<GooglePlaceModel>> GetGooglePlaces(string location, string query, int distance)
        {            
            // Creates list of JSON Objects
            GooglePlacesResponse resultData = new GooglePlacesResponse();
            string nextPageToken = null;

            Dictionary<string, string> parameters = new()
            {
                { "key", _apiKey },
                { "location", location },
                { "radius", distance.ToString() },
                { "query", query }
            };

            do
            {
                // Checking 
                if (!string.IsNullOrEmpty(nextPageToken))
                {
                    parameters.Add("pagetoken", nextPageToken);
                }

                //Creating the URL
                string endpointPath = "maps/api/place/textsearch/json?";
                string fullRequestUri = endpointPath + string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
                HttpResponseMessage response =  _httpClient.GetAsync(fullRequestUri).Result;
                
                // Throw an exception if the HTTP request failed
                response.EnsureSuccessStatusCode();

                // Read into the ROOT object class
                resultData = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>();
              
                if (!string.IsNullOrEmpty(nextPageToken))
                {
                    await Task.Delay(1000); // Delay for the next page token to activate
                }

            } while (!string.IsNullOrEmpty(nextPageToken));

            return resultData.Results;
        }

        public async Task<List<GooglePlaceModel>> GetGooglePlacesDetails(List<GooglePlaceModel> googlePlaces)
        {
            
            List<GooglePlaceModel> placesDetails = new List<GooglePlaceModel>();

            foreach (var place in googlePlaces)
            {
                if (string.IsNullOrEmpty(place.Place_Id)) continue;

                // 1. Build URL correctly (avoiding double question marks)
                string endpointPath = "maps/api/place/details/json";
                string queryParams = $"?place_id={place.Place_Id}&key={_apiKey}";
                string fullRequestUri = endpointPath + queryParams;

                // 2. Use 'await' instead of '.Result' to prevent blocking the thread
                var response = await _httpClient.GetAsync(fullRequestUri);

                response.EnsureSuccessStatusCode();

                // 3. Read into the specific Details response wrapper
                var detailsResponse = await response.Content.ReadFromJsonAsync<GooglePlaceDetailsResponse>();

                // 4. Add the successfully parsed detailed Result to our list
                if (detailsResponse != null && detailsResponse.Result != null)
                {
                    placesDetails.Add(detailsResponse.Result);
                }
            }

            return placesDetails;
        }
    }
}
