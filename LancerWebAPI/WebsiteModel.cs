namespace LancerWebAPI
{
    public class WebsiteModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Phone { get; set; }
        public string PostalCode { get; set; }

    }
}
