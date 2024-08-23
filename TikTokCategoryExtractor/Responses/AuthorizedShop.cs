using Newtonsoft.Json;
using TikTokCategoryExtractor.Interfaces;

namespace TikTokCategoryExtractor.Responses
{
    public partial class AuthorizedShop : ITikTokResponse
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("shop_list")]
        public List<ShopList> ShopList { get; set; }
    }

    public partial class ShopList
    {
        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("shop_cipher")]
        public string ShopCipher { get; set; }

        [JsonProperty("shop_code")]
        public string ShopCode { get; set; }

        [JsonProperty("shop_id")]
        public string ShopId { get; set; }

        [JsonProperty("shop_name")]
        public string ShopName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
