namespace TikTokCategoryExtractor.Responses
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class ProductAttributes : TikTokResponse
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("message")]
        public string MessageResponse { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("attributes")]
        public List<ProductAttribute> Attributes { get; set; }
    }

    public partial class ProductAttribute
    {
        [JsonIgnore]
        public string CategoryName { get; set; }

        [JsonIgnore]
        public string CategoryId { get; set; }

        [JsonProperty("attribute_type")]
        public long AttributeType { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("input_type")]
        public InputType InputType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public List<Value> Values { get; set; }
    }

    public partial class InputType
    {
        [JsonProperty("is_customized")]
        public bool IsCustomized { get; set; }

        [JsonProperty("is_mandatory")]
        public bool IsMandatory { get; set; }

        [JsonProperty("is_multiple_selected")]
        public bool IsMultipleSelected { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
