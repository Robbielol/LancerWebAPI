using MongoDB.Bson;
using MongoDB.Driver;

namespace LancerWebAPI.Database
{
    public class PlaceRepository : MongoRepository<GooglePlaceModel>
    {
        public PlaceRepository(IMongoClient client) : base(client, "Websites")
        {
        }

        public async Task<IEnumerable<GooglePlaceModel>> GetByPlaceIDsAsync(List<string> objectIDs)
        {
            var filter = Builders<GooglePlaceModel>.Filter.In(x => x.Place_Id, objectIDs);

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
