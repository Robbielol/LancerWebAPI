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
                string placeId = place.Place_Id?.ToString();
                if (string.IsNullOrEmpty(placeId)) continue;

                string detailsUrl = "/maps/api/place/details/json?";
                var parameters = new Dictionary<string, string>
                {
                    { "key", _apiKey },
                    { "place_id", placeId }
                };

                string url = $"{detailsUrl}?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();
                    //placesDetails.Add((JsonObject)JsonObject.Parse(responseText));
                };
            }

            return placesDetails;
        }
    }
}
