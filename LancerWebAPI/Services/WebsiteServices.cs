using System.Text.Json.Nodes;

namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {
        public WebsiteServices() { }


        public async Task<IEnumerable<WebsiteModel>> GetAllPlaces(string location, string query, int distance)
        {
            //Check if similiar query in in DB
            IDatabaseService service = new DatabaseService();

            IEnumerable<WebsiteModel> existingPlaces = await service.Read<WebsiteModel>(query);

            //Return if true if not continue
            if (existingPlaces != null && existingPlaces.Any())
            {
                return existingPlaces;
            }

            //Go to GoogleAPI and Get Data
            GoogleMapsAPIService googleMapsAPIService = new GoogleMapsAPIService();
            List<JsonObject> placesDetails = await googleMapsAPIService.GetGooglePlaces(location, query, distance);

            List<JsonObject> detailedPlaces = await googleMapsAPIService.GetGooglePlacesDetails(placesDetails);
            List<WebsiteModel> filteredPlaces = FilterPlaces(detailedPlaces);
            //Send data to DB


            //Return data            
            return filteredPlaces;
        }

        public List<WebsiteModel> FilterPlaces(List<JsonObject> placesDetails)
        {
            List<WebsiteModel> placesNoWebsite = new List<WebsiteModel>();

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
                            Phone = phoneNum,
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
