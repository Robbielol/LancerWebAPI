using LancerWebAPI.Models;
using MongoDB.Driver;

namespace LancerWebAPI.Database
{
    public class UserRepository : MongoRepository<UserModel>
    {
        public UserRepository(IMongoClient client) : base(client, "Users") { }

        public async Task<UserModel> GetUserAsync(string email, string password)
        {
            var filter = Builders<UserModel>.Filter.Where(x => 
                x.Email.ToLower() == email.ToLower() &&
                x.PasswordHash == password
            );

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            var filter = Builders<UserModel>.Filter.Where(x =>
                x.Email.ToLower() == email.ToLower() 
            );

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
