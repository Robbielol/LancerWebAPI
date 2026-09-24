using System.Text.Json.Serialization;

namespace LancerWebAPI.Models
{
    public class GooglePlacesResponse
    {
        [JsonPropertyName("places")]
        public List<GooglePlaceModel> Places { get; set; }

        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

            }
}
