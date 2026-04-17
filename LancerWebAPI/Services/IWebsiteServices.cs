namespace LancerWebAPI.Services
{
    public interface IWebsiteServices
    {
        public Task<IEnumerable<SearchCacheModel>> GetAllSearchs();
        public Task<IEnumerable<GooglePlaceModel>> GetAllPlaces(string location, string query, int distance);
    }
}
