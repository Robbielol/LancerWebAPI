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
                // 1. Prepare the JSON body
                // The new API handles natural language flawlessly
                var requestBody = new
                {
                    textQuery = $"{query} in {location}",
                    pageSize = 20,
                    pageToken = activeToken // Will be null on the first run, which is perfect
                };

                // 2. Set up the POST request
                var request = new HttpRequestMessage(HttpMethod.Post, endpointUrl);

                // 3. Required Headers for the New API
                request.Headers.Add("X-Goog-Api-Key", _apiKey);

                // FIELD MASKING: This is the magic. We tell Google EXACTLY what to return.
                request.Headers.Add("X-Goog-FieldMask", "places.id,places.displayName.text,places.formattedAddress,places.rating,places.websiteUri,places.nationalPhoneNumber,nextPageToken");

                request.Content = JsonContent.Create(requestBody);

                // 4. Send the Request
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                // Check if it fails and print the exact reason from Google
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Google API Error: {errorContent}");
                    response.EnsureSuccessStatusCode();
                }

                // 5. Read the response
                var resultData = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>();

                // 6. Add the current page to our master list
                if (resultData != null && resultData.Places != null)
                {
                    allResults.AddRange(resultData.Places);
                }

                // 7. Pagination - Tokens in the New API are valid instantly! No delays needed.
                activeToken = resultData?.NextPageToken;

            } while (!string.IsNullOrEmpty(activeToken));

            return allResults;
        }

        //public async Task<List<GooglePlaceModel>> GetGooglePlacesDetails(List<GooglePlaceModel> googlePlaces)
        //{
            
        //    List<GooglePlaceModel> placesDetails = new List<GooglePlaceModel>();

        //    foreach (var place in googlePlaces)
        //    {
        //        if (string.IsNullOrEmpty(place.Place_Id)) continue;

        //        // 1. Build URL correctly (avoiding double question marks)
        //        string endpointPath = "maps/api/place/details/json";
        //        string queryParams = $"?place_id={place.Place_Id}&key={_apiKey}";
        //        string fullRequestUri = endpointPath + queryParams;

        //        // 2. Use 'await' instead of '.Result' to prevent blocking the thread
        //        var response = await _httpClient.GetAsync(fullRequestUri);

        //        response.EnsureSuccessStatusCode();

        //        // 3. Read into the specific Details response wrapper
        //        var detailsResponse = await response.Content.ReadFromJsonAsync<GooglePlaceDetailsResponse>();

        //        // 4. Add the successfully parsed detailed Result to our list
        //        if (detailsResponse != null && detailsResponse.Result != null)
        //        {
        //            placesDetails.Add(detailsResponse.Result);
        //        }
        //    }

        //    return placesDetails;
        //}
    }
}
