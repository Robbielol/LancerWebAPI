

using System.Text.Json.Serialization;

namespace LancerWebAPI
{
    public class GooglePlacesResponse
    {
        [JsonPropertyName("places")]
        public List<GooglePlaceModel> Places { get; set; }

        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

            }
}
