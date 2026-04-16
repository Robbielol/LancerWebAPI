using MongoDB.Driver;

namespace LancerWebAPI.Database
{
    public class SearchCacheRepository : MongoRepository<SearchCacheModel>
    {
        public SearchCacheRepository(IMongoClient client) : base(client, "SearchCache")
        {
        }

        public async Task<SearchCacheModel> GetCacheAsync(string city, string businessType)
        {
            var filter = Builders<SearchCacheModel>.Filter.Where(x =>
                x.City.ToLower() == city.ToLower() &&
                x.BusinessType.ToLower() == businessType.ToLower()
            );

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
