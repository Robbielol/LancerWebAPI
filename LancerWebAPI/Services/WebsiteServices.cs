using System.Text.Json.Nodes;

namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {
        public WebsiteServices() { }


        public async Task<IEnumerable<WebsiteModel>> GetAllPlaces(string location, string query, double distance)
        {
            //Check if similiar query in in DB
            IDatabaseService service = new DatabaseService();

            await service.Read<WebsiteModel>(query, collectionName);

            //Return if true if not continue

            //Go to GoogleAPI and Get Data

            //Send data to DB

            //Return data            
            return new List<WebsiteModel>();
        }

        public void FilterPlaces(List<JsonObject> placesDetails) 
        {
            s
            foreach(JsonObject place in placesDetails) {
                if (placeData["result"] != null)
                {
                    var placeDetails = placeData["result"];
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

                        var jsonObj = new JsonObject
                        {
                            ["Name"] = placeDetails["name"]?.ToString(),
                            ["WebsiteUrl"] = website,
                            ["Rating"] = rating,
                            ["Phone"] = phoneNum,
                            ["Address"] = address
                        };
                        businessNoWebsiteList.Add(jsonObj);
                    }
            }

        }
}
