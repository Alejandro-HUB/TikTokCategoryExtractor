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

        [JsonConstructor]
        public Data(
           [JsonProperty("category_list")] List<CategoryList> category_list,
           [JsonProperty("categories")] List<CategoryList> categories)
        {
            CategoryList = category_list ?? categories;
        }
    }

    public partial class CategoryList
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("is_leaf")]
        public bool IsLeaf { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("status")]
        public List<long> Status { get; set; }

        [JsonProperty("permission_statuses")]
        public List<string> PermissionStatuses { get; set; }

        // Custom property for handling either "local_display_name" or "local_name"
        public string LocalDisplayName { get; set; }

        [JsonConstructor]
        public CategoryList(
            [JsonProperty("id")] long id,
            [JsonProperty("is_leaf")] bool isLeaf,
            [JsonProperty("parent_id")] long parentId,
            [JsonProperty("status")] List<long> status,
            [JsonProperty("local_display_name")] string localDisplayName = null,
            [JsonProperty("local_name")] string localName = null,
            List<string> permissionStatuses = null)
        {
            Id = id;
            IsLeaf = isLeaf;
            ParentId = parentId;
            Status = status;
            LocalDisplayName = localDisplayName ?? localName;
            PermissionStatuses = permissionStatuses;
        }
    }
}
