using MongoDB.Bson;
using MongoDB.Driver;


namespace LancerWebAPI.Services
{
    public abstract class DataBaseConnectionService : IDatabaseService
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<WebsiteModel> _collection;

        protected DataBaseConnectionService() 
        {
            string connectionString = "mongodb://localhost:27017/";
            try
            {
                Console.WriteLine("connecting to database...");
                _client = new MongoClient(connectionString);
                _database = _client.GetDatabase("local");
                _collection = _database.GetCollection<WebsiteModel>("Websites");
                Console.WriteLine("Connected to database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to database. Error: " +ex.Message);
            }
        }

        public abstract Task Create<T>(string query);

        public abstract Task Read<T>(string query);

        public abstract Task Update<T>(string query);

        public abstract Task Delete<T>(string query);
    }
}
