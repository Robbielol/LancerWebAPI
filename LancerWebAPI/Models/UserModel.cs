using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LancerWebAPI.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public bool PasswordConfirmed { get; set; }

        public DateTime createdDate { get; set; }

    }
}
