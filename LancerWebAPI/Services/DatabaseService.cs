using MongoDB.Driver;
using System.Numerics;

namespace LancerWebAPI.Services
{
    public class DatabaseService : DataBaseConnectionService
    {

        public DatabaseService(IMongoClient config):base(config) {
            
        }

        public override async Task Create<T>(IEnumerable<T> list)
        {
            Console.WriteLine("Inserting records...");
            try
            {
                await _collection.InsertManyAsync((IEnumerable<GooglePlaceModel>)list);
                Console.WriteLine("Records Inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public override async Task<IEnumerable<T>> Read<T>(String filter)
        {
            // Read from MongoDB and return list of records of type T
            Console.WriteLine("Reading records...");
            List<T> results = new List<T>();
            try
            {
                List<GooglePlaceModel> list = await _collection.Find(x => x.Name == filter).ToListAsync();
                foreach (var item in list)
                {
                    results.Add((T)(object)item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return results;
        }

        public override async Task Update<T>(string query)
        {
        }

        public override async Task Delete<T>(string query)
        {

        }
    }
}
