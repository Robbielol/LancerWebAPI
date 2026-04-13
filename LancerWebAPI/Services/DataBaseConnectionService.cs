using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace LancerWebAPI.Services
{
    public abstract class DataBaseConnectionService : IDatabaseService
    {
        protected IMongoDatabase _database;
        protected IMongoCollection<GooglePlaceModel> _collection;

        protected DataBaseConnectionService(IMongoClient client) 
        {

            try
            {
                Console.WriteLine("connecting to database...");


                // Note: You might want to pull the database name from config too eventually!
                _database = client.GetDatabase("Lancer");
                _collection = _database.GetCollection<GooglePlaceModel>("Websites");

                Console.WriteLine("Connected to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to database. Error: " + ex.Message);
            }
        }

        public abstract Task Create<T>(IEnumerable<T> list);

        public abstract Task<IEnumerable<T>> Read<T>(string filter);

        public abstract Task Update<T>(string query);

        public abstract Task Delete<T>(string query);
    }
}
