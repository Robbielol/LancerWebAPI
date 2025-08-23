using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Driver;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LancerWebAPI.Services
{
    public class GoogleMapsAPIService
    {

        private string _apiKey;
        private string _apiKeySecret;
        private readonly string _url;
        private HttpClient _httpClient;

        public GoogleMapsAPIService()
        {
            _apiKey = Environment.GetEnvironmentVariable("G_API_KEY"); ;
            _apiKeySecret = Environment.GetEnvironmentVariable("G_PLACEID_URL");
            _httpClient = new HttpClient();
            _url = Environment.GetEnvironmentVariable("G_URL");
            _httpClient.BaseAddress = new Uri(_url);

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<List<JsonObject>> GetGooglePlaces(string location, string distance, string query)
        {            
             // Creates list of JSON Objects
            var allResults = new List<JsonObject>();
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
                string urlParameters = string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
                HttpResponseMessage response =  _httpClient.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    allResults = (await response.Content.ReadFromJsonAsync<IEnumerable<JsonObject>>()).ToList();
                }

                if (!string.IsNullOrEmpty(nextPageToken))
                {
                    await Task.Delay(1000); // Delay for the next page token to activate
                }
            } while (!string.IsNullOrEmpty(nextPageToken));

            return allResults;
        }

        public async Task<List<JsonObject>> GetGooglePlacesWithoutWebsite(List<JsonObject> googlePlaces)
        {
            List<WebsiteModel> businessNoWebsiteList = new List<WebsiteModel>();
            foreach (var place in placeIdList)
            {
                string placeId = place["place_id"]?.ToString();
                if (string.IsNullOrEmpty(placeId)) continue;

                string detailsUrl = Environment.GetEnvironmentVariable("G_PLACE_DETAILS_URL");
                var parameters = new Dictionary<string, string>
                {
                    { "key", apiKey },
                    { "place_id", placeId }
                };

                string url = $"{detailsUrl}?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                JsonObject placeData;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();
                    placeData = JsonObject.Parse(responseText);
                };

                if (placeData["result"] != null)
                {
                    var placeDetails = placeData["result"];
                    string website = placeDetails["website"]?.ToString();
                    double rating = placeDetails["rating"]?.ToObject<double>() ?? 0;

                    if ((string.IsNullOrEmpty(website) || website.Contains("facebook") || website.Contains("instagram")) && rating >= 3.9)
                    {
                        string phoneNum = placeDetails["international_phone_number"]?.ToString() ?? "No phone number available";
                        string address = placeDetails["formatted_address"]?.ToString() ?? "No address available";

                        businessNoWebsiteList.Add(new WebsiteModel()
                        {
                            Name = placeDetails["name"]?.ToString(),
                            Website = website,
                            Rating = rating,
                            PhoneNumber = phoneNum,
                            Address = address
                        });
                    }
                }
                else
                {
                    Console.WriteLine("Error: No place details found.");
                }
            }
            Console.WriteLine(businessNoWebsiteList);

            return businessNoWebsiteList;
        }
    }
}
