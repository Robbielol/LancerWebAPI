using MongoDB.Driver;
using System.Numerics;

namespace LancerWebAPI.Database
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {

        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoClient client, string collectionName)
        {
            var database = client.GetDatabase("Lancer");
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task InsertManyAsync(IEnumerable<T> docs)
        {
            await _collection.InsertManyAsync(docs);
        }

        public async Task InsertOneAsync(T doc)
        {
            await _collection.InsertOneAsync(doc);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
