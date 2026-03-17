using System.Text.Json.Nodes;

namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {

        private IDatabaseService local;
        private readonly GoogleMapsAPIService _googleMapsAPIService;
        public WebsiteServices(GoogleMapsAPIService googleMapsAPIService) { 
            _googleMapsAPIService = googleMapsAPIService;
            local = new DatabaseService(); 
        }


        public async Task<IEnumerable<WebsiteModel>> GetAllPlaces(string location, string query, int distance)
        {

            //Check if similiar query in in DB
            IEnumerable<WebsiteModel> existingPlaces = await local.Read<WebsiteModel>(query);

            //Return if has values if not continue
            if (existingPlaces != null && existingPlaces.Any())
            {
                return existingPlaces;
            }

            //Go to GoogleAPI and Get Data
            List<JsonObject> placesDetails = await _googleMapsAPIService.GetGooglePlaces(location, query, distance);

            List<JsonObject> detailedPlaces = await _googleMapsAPIService.GetGooglePlacesDetails(placesDetails);
            List<WebsiteModel> filteredPlaces = FilterPlaces(detailedPlaces);
            //Send data to DB
            await local.Create<WebsiteModel>(filteredPlaces);

            //Return data            
            return filteredPlaces;
        }

        public List<WebsiteModel> FilterPlaces(List<JsonObject> placesDetails)
        {
            List<WebsiteModel> placesNoWebsite = new();

            foreach (JsonObject place in placesDetails)
            {
                if (place["result"] != null)
                {
                    var placeDetails = place["result"];
                    string website = placeDetails["website"]?.ToString();
                    double rating = 0;
                    if (placeDetails["rating"] != null && double.TryParse(placeDetails["rating"]?.ToString(), out double parsedRating))
                    {
                        rating = parsedRating;
                    }

                    if ((string.IsNullOrEmpty(website) || website.Contains("facebook") || website.Contains("instagram")) && rating >= 3.9)
                    {
                        string phoneNum = placeDetails["international_phone_number"]?.ToString() ?? "No phone number available";
                        string address = placeDetails["formatted_address"]?.ToString() ?? "No address available";

                        var jsonObj = new WebsiteModel
                        {
                            Name = placeDetails["name"]?.ToString(),
                            WebsiteUrl = website,
                            Rating = rating,
                            Phone = int.TryParse(phoneNum, out int phone) ? phone : 0,
                            Address = address
                        };
                        placesNoWebsite.Add(jsonObj);
                    }
                }
            }
            return placesNoWebsite;
        }
    }
}
