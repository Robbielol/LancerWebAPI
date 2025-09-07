using MongoDB.Driver;
using System.Numerics;

namespace LancerWebAPI.Services
{
    public class DatabaseService : DataBaseConnectionService
    {
        public DatabaseService():base() {
            
        }

        public override async Task Create<T>(string query)
        {
            Console.WriteLine("Inserting record...");
            WebsiteModel websiteModel = new WebsiteModel
            {
                Name = "Test",
                WebsiteUrl = "www.url.com",
                Address = "123 place St, Vancouver",
                Phone = "123456789"
            };
            try
            {
                await _collection.InsertOneAsync(websiteModel);
                Console.WriteLine("Record Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public override async Task<IEnumerable<T>> Read<T>(string query)
        {
            // Read from MongoDB and return list of records of type T
            Console.WriteLine("Reading records...");
            var results = await _collection.FindAsync(Builders<WebsiteModel>.Filter.Empty);
            var list = results.ToList();
            return list is List<T> typedList ? typedList : new List<T>();
        }

        public override async Task Update<T>(string query)
        {
        }

        public override async Task Delete<T>(string query)
        {

        }
    }
}
