using Newtonsoft.Json;

namespace TikTokCategoryExtractor.Responses
{
    public partial class DeleteProducts : TikTokResponse
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public DataDP Data { get; set; }

        [JsonProperty("message")]
        public string ResponseMessage { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class DataDP
    {
        [JsonProperty("failed_product_ids")]
        public List<long> FailedProductIds { get; set; }
    }
}
