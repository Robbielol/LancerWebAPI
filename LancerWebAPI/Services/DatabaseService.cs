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
                Phone = 123456789
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

        public override void Read<T>(string query)
        {
            
        }

        public override void Update<T>(string query)
        {
        }

        public override void Delete<T>(string query)
        {

        }
    }
}
