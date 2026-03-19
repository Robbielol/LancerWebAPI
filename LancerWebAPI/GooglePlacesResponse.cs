

using System.Text.Json.Serialization;

namespace LancerWebAPI
{
    public class GooglePlacesResponse
    {
        public List<GooglePlaceModel> Results { get; set; }
        public string Status { get; set; }

        [JsonPropertyName("next_page_token")]
        public string NextPageToken { get; set; }
    }
}
