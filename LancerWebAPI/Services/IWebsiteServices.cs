namespace LancerWebAPI.Services
{
    public interface IWebsiteServices
    {
        public Task<IEnumerable<WebsiteModel>> GetAllPlaces(string location, string query, int distance);
    }
}
