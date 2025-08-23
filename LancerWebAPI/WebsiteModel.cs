using MongoDB.Bson;

namespace LancerWebAPI
{
    public class WebsiteModel
    {
        public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }

    }
}
