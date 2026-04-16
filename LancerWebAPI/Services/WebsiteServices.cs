using System.Text.Json.Nodes;
using LancerWebAPI.Database;
using MongoDB.Bson;
using MongoDB.Driver;


namespace LancerWebAPI.Services
{
    public class WebsiteServices : IWebsiteServices
    {

        private readonly PlaceRepository _placeRepo;
        private readonly SearchCacheRepository _searchCacheRepo;
        private readonly GoogleMapsAPIService _googleMapsAPIService;
        public WebsiteServices(GoogleMapsAPIService googleMapsAPIService, PlaceRepository placeRepository, SearchCacheRepository searchCacheRepository) { 
            _googleMapsAPIService = googleMapsAPIService;
            _placeRepo = placeRepository;
            _searchCacheRepo = searchCacheRepository;
        }


        public async Task<IEnumerable<GooglePlaceModel>> GetAllPlaces(string location, string businessType, int distance)
        {
            // Check if location and business exists in DB
            SearchCacheModel searchCache = await _searchCacheRepo.GetCacheAsync(location, businessType);

            // If true, check placeIds list have values. Then retreive list from Places collection
            if (searchCache != null && searchCache.CachedPlacedIds.Any())
            {
                return await _placeRepo.GetByPlaceIDsAsync(searchCache.CachedPlacedIds);
            }

            //Go to GoogleAPI and Get Data
            List<GooglePlaceModel> placesDetails = await _googleMapsAPIService.GetGooglePlaces(location, businessType, distance); 

            //Filter list to return objects that don't have a valid value for website
            List<GooglePlaceModel> filteredPlaces = FilterPlaces(placesDetails);

            // Extract just the IDs from the full business objects
            var extractedIds = filteredPlaces.Select(place => place.Place_Id).ToList();

            //Add Places to SearchCacheModel and create new entry in the database collection.
            await _searchCacheRepo.InsertOneAsync(new SearchCacheModel(location, businessType, extractedIds));

            //Send data to DB
            await _placeRepo.InsertManyAsync(filteredPlaces);

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
