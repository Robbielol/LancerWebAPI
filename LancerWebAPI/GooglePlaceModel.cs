namespace LancerWebAPI
{
    public class GooglePlaceModel
    {
        public string Name { get; set; }
        public string Formatted_Address { get; set; }
        public string Place_Id { get; set; }

        public string WebsiteUri { get; set; }

        public string NationalPhoneNumber { get; set; }
        public decimal Rating { get; set; }
        public decimal UserRatingCount { get; set; }
    }
}
