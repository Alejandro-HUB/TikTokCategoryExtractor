using Newtonsoft.Json;
using TikTokCategoryExtractor.Requests;

namespace TikTokCategoryExtractor.Responses
{
    public partial class GetProductList : TikTokResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public GetData Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class GetData
    {
        [JsonProperty("products")]
        public List<GetTikTokProduct> Products { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public partial class GetTikTokProduct
    {
        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("global_sync_failed_reasons")]
        public string GlobalSyncFailedReasons { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sale_regions")]
        public List<string> SaleRegions { get; set; }

        [JsonProperty("skus")]
        public List<GetSkus> Skus { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }

    public partial class GetSkus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public TikTokPrice Price { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }

        [JsonProperty("stock_infos")]
        public List<StockInfo> StockInfos { get; set; }
    }

    public partial class TikTokPrice
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }

        [JsonProperty("price_include_vat")]
        public int PriceIncludeVat { get; set; }
    }
}
