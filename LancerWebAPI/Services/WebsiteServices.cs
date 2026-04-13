using System.Text.Json.Nodes;
using MongoDB.Driver;


namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {

        private IDatabaseService local;
        private readonly GoogleMapsAPIService _googleMapsAPIService;
        public WebsiteServices(GoogleMapsAPIService googleMapsAPIService, IMongoClient client) { 
            _googleMapsAPIService = googleMapsAPIService;
            local = new DatabaseService(client); 
        }


        public async Task<IEnumerable<GooglePlaceModel>> GetAllPlaces(string location, string query, int distance)
        {

            //Check if similiar query in in DB
            IEnumerable<GooglePlaceModel> existingPlaces = await local.Read<GooglePlaceModel>(query);

            //Return if has values if not continue
            if (existingPlaces != null && existingPlaces.Any())
            {
                return existingPlaces;
            }

            //Go to GoogleAPI and Get Data
            List<GooglePlaceModel> placesDetails = await _googleMapsAPIService.GetGooglePlaces(location, query, distance); 
            //List<GooglePlaceModel> detailedPlaces = await _googleMapsAPIService.GetGooglePlacesDetails(placesDetails);
            // TODO Safe detailsPlaces to DB, decide to filter first to after. 
            List<GooglePlaceModel> filteredPlaces = FilterPlaces(placesDetails);
            //Send data to DB
            await local.Create<GooglePlaceModel>(filteredPlaces);

            //Return data            
            return filteredPlaces;
        }

        public List<GooglePlaceModel> FilterPlaces(List<GooglePlaceModel> placesDetails)
        {
            List<GooglePlaceModel> placesNoWebsite = new();
            //TODO Change this variable to string builder
            string website = "";

            foreach (GooglePlaceModel place in placesDetails)
            {
                website = place.Website; 
                if ((string.IsNullOrEmpty(website) || website.Contains("facebook") || website.Contains("instagram")) && place.Rating >= 3.9)
                {                  
                    placesNoWebsite.Add(place);
                }
            
            }
            return placesNoWebsite;
        }
    }
}
