namespace TikTokCategoryExtractor.Responses
{
    public partial class RefreshAccessTokenResponse : TikTokResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataAT data { get; set; }
        public string request_id { get; set; }
    }
    public class DataAT
    {
        public string access_token { get; set; }
        public string access_token_expire_in { get; set; }
        public string refresh_token { get; set; }
        public string refresh_token_expire_in { get; set; }
        public string open_id { get; set; }
        public string seller_name { get; set; }
        public string seller_base_region { get; set; }
        public string user_type { get; set; }
    }
}
