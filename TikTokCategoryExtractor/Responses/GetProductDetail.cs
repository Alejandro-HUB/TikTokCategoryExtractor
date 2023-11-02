namespace TikTokCategoryExtractor.Responses
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class GetProductDetail : TikTokResponse
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public DataProduct Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public partial class DataProduct
    {
        [JsonProperty("category_list")]
        public List<CategoryList> CategoryList { get; set; }

        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("is_cod_open")]
        public bool IsCodOpen { get; set; }

        [JsonProperty("package_dimension_unit")]
        public string PackageDimensionUnit { get; set; }

        [JsonProperty("package_height")]
        public long PackageHeight { get; set; }

        [JsonProperty("package_length")]
        public long PackageLength { get; set; }

        [JsonProperty("package_weight")]
        public string PackageWeight { get; set; }

        [JsonProperty("package_width")]
        public long PackageWidth { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_status")]
        public long ProductStatus { get; set; }

        [JsonProperty("skus")]
        public List<Skus> Skus { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }

        [JsonProperty("warranty_period")]
        public WarrantyPeriod WarrantyPeriod { get; set; }

        [JsonProperty("warranty_policy")]
        public string WarrantyPolicy { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("thumb_url_list")]
        public List<Uri> ThumbUrlList { get; set; }

        [JsonProperty("url_list")]
        public List<Uri> UrlList { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class Skus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("sales_attributes")]
        public List<SalesAttribute> SalesAttributes { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }

        [JsonProperty("stock_infos")]
        public List<StockInfo> StockInfos { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }
    }

    public partial class SalesAttribute
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sku_img")]
        public Image SkuImg { get; set; }

        [JsonProperty("value_id")]
        public string ValueId { get; set; }

        [JsonProperty("value_name")]
        public string ValueName { get; set; }
    }

    public partial class StockInfo
    {
        [JsonProperty("available_stock")]
        public long AvailableStock { get; set; }

        [JsonProperty("warehouse_id")]
        public string WarehouseId { get; set; }
    }

    public partial class WarrantyPeriod
    {
        [JsonProperty("warranty_description")]
        public string WarrantyDescription { get; set; }

        [JsonProperty("warranty_id")]
        public long WarrantyId { get; set; }
    }
}
