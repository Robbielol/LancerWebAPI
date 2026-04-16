

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LancerWebAPI
{
    public class SearchCacheModel
    {
        // Empty constructor required for MongoDB deserialization
        public SearchCacheModel() { }

        // The constructor your controller will use
        public SearchCacheModel(string city, string businessType, List<string> cachedPlaceIds)
        {
            City = city;
            BusinessType = businessType;
            CachedPlaceIds = cachedPlaceIds;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string City { get; set; }

        public string BusinessType { get; set; }

        public List<string> CachedPlaceIds { get; set; } = new List<string>();

        public DateTime DateCached { get; set; } = DateTime.UtcNow;
    }
}
