using Newtonsoft.Json;

namespace UserAuthentication.Entities
{
    public class GenericApiResponse<T>
    {
        [JsonProperty("ResponseCode")]
        public string? ResponseCode { get; set; }
        [JsonProperty("ResponseDescription")]
        public string? ResponseDescription { get; set; }
        [JsonProperty("Data")]
        public T? Data { get; set; }
    }
}
