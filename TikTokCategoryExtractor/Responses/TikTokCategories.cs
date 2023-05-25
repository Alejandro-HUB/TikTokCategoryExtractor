namespace TikTokCategoryExtractor.Responses
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class TikTokCategories : TikTokResponse
    {
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
        [JsonProperty("category_list")]
        public List<CategoryList> CategoryList { get; set; }
    }

    public partial class CategoryList
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_leaf")]
        public bool IsLeaf { get; set; }

        [JsonProperty("local_display_name")]
        public string LocalDisplayName { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("status")]
        public List<long> Status { get; set; }
    }
}
