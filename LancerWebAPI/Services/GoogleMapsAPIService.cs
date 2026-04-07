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
            _apiKey = config["GoogleMaps:G_API_KEY"];
            _httpClient = new HttpClient();
        }

        public async Task<List<GooglePlaceModel>> GetGooglePlaces(string location, string query, int distance)
        {
            List<GooglePlaceModel> allResults = new List<GooglePlaceModel>();
            string activeToken = null;

            // The New API Endpoint
            string endpointUrl = "https://places.googleapis.com/v1/places:searchText";

            do
            {
                // Prepare the JSON body
                // The API handles natural language 
                var requestBody = new
                {
                    textQuery = $"{query} in {location}",
                    pageSize = 20,
                    pageToken = activeToken // Will be null on the first run
                };

                // Set up the POST request
                var request = new HttpRequestMessage(HttpMethod.Post, endpointUrl);

                // Required Headers for the New API
                request.Headers.Add("X-Goog-Api-Key", _apiKey);

                // Tell Google EXACTLY what to return.
                request.Headers.Add("X-Goog-FieldMask", "places.id,places.displayName.text,places.formattedAddress,places.rating,places.websiteUri,places.nationalPhoneNumber,nextPageToken");

                request.Content = JsonContent.Create(requestBody);

                // Send the Request
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // Check if it fails and print the exact reason from Google
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Google API Error: {errorContent}");
                    response.EnsureSuccessStatusCode();
                }

                // Read the response
                var resultData = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>();

                // Add the current page to our master list
                if (resultData != null && resultData.Places != null)
                {
                    allResults.AddRange(resultData.Places);
                }

                // Pagination.
                activeToken = resultData?.NextPageToken;

            } while (!string.IsNullOrEmpty(activeToken));

            return allResults;
        }

        
    }
}
