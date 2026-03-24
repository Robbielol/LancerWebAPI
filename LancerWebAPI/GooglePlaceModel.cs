using System.Text.Json.Serialization;

namespace LancerWebAPI
{
    // The New API wraps the name in a "displayName.text" object
    public class DisplayName
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    // 2. Updated Model to map exactly to the New API fields
    public class GooglePlaceModel
    {
        [JsonPropertyName("id")]
        public string Place_Id { get; set; }

        [JsonPropertyName("displayName")]
        public DisplayName DisplayName { get; set; }

        [JsonPropertyName("formattedAddress")]
        public string Formatted_Address { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("websiteUri")]
        public string Website { get; set; }

        [JsonPropertyName("nationalPhoneNumber")]
        public string FormattedPhoneNumber { get; set; }

        // Helper property so your Controller/WebsiteServices can still just call "place.Name"
        [JsonIgnore]
        public string Name => DisplayName?.Text;
    }
}
