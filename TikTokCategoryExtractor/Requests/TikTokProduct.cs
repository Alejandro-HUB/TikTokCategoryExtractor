using Newtonsoft.Json;

namespace TikTokCategoryExtractor.Requests
{
    public partial class TikTokProduct
    {
        [JsonIgnore]
        public bool ProductAlreadyExists { get; set; } = false;

        [JsonIgnore]
        public bool CreateProduct { get; set; } = false;

        //Product Id should not be serialized for Product Creation
        public bool ShouldSerializeProductId()
        {
            bool serialize = true;
            if (CreateProduct)
            {
                serialize = false;
            }

            return serialize;
        }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("brand_id")]
        public string BrandId { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("delivery_service_ids")]
        public string DeliveryServiceIds { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("exemption_of_identifier_code")]
        public ExemptionOfIdentifierCode ExemptionOfIdentifierCode { get; set; }

        [JsonProperty("images")]
        public List<TikTokImage> Images { get; set; }

        [JsonProperty("is_cod_open")]
        public bool IsCodOpen { get; set; }

        [JsonProperty("outer_product_id")]
        public string OuterProductId { get; set; }

        [JsonProperty("package_dimension_unit")]
        public string PackageDimensionUnit { get; set; }

        [JsonProperty("package_height")]
        public int? PackageHeight { get; set; }

        [JsonProperty("package_length")]
        public int? PackageLength { get; set; }

        [JsonProperty("package_weight")]
        public string PackageWeight { get; set; }

        [JsonProperty("package_width")]
        public int? PackageWidth { get; set; }

        [JsonProperty("product_attributes")]
        public ProductAttributeTS[] ProductAttributes { get; set; }

        [JsonProperty("product_certifications")]
        public List<ProductCertification> ProductCertifications { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_video")]
        public ProductVideo ProductVideo { get; set; }

        [JsonProperty("size_chart")]
        public SizeChart SizeChart { get; set; }

        [JsonProperty("skus")]
        public List<Skus> Skus { get; set; }

        [JsonProperty("warranty_period")]
        public long? WarrantyPeriod { get; set; }

        [JsonProperty("warranty_policy")]
        public string WarrantyPolicy { get; set; }
    }

    public partial class ExemptionOfIdentifierCode
    {
        [JsonProperty("exemption_reason")]
        public List<int> ExemptionReason { get; set; }
    }

    public partial class TikTokImage
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class ProductAttributeTS
    {
        // Constructor with AttributeId parameter
        public ProductAttributeTS(long attributeId)
        {
            AttributeId = attributeId;
        }

        [JsonIgnore]
        public string CategoryId { get; set; }

        [JsonIgnore]
        public string AttributeName { get; set; }

        [JsonIgnore]
        public bool IsCustomized { get; set; }

        [JsonIgnore]
        public AttributeValue[] AcceptedValues { get; set; }

        [JsonIgnore]
        public long AttributeType { get; set; }

        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        [JsonProperty("attribute_values")]
        public AttributeValue[] AttributeValues { get; set; }
    }

    public partial class AttributeValue
    {
        [JsonProperty("value_id")]
        public string ValueId { get; set; }

        [JsonProperty("value_name")]
        public string ValueName { get; set; }
    }

    public partial class ProductCertification
    {
        [JsonProperty("files")]
        public List<TikTokFile> Files { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("images")]
        public List<TikTokImage> Images { get; set; }
    }

    public partial class TikTokFile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class ProductVideo
    {
        [JsonProperty("video_id")]
        public string VideoId { get; set; }
    }

    public partial class SizeChart
    {
        [JsonProperty("img_id")]
        public string ImgId { get; set; }
    }

    public partial class Skus
    {
        [JsonIgnore]
        public bool VariantAlreadyExists { get; set; } = false;

        [JsonIgnore]
        public bool CreateVariant { get; set; } = false;

        [JsonIgnore]
        public string imageId { get; set; }

        //Id should not be serialized for Product Variant Creation
        public bool ShouldSerializeId()
        {
            bool serialize = true;
            if (CreateVariant)
            {
                serialize = false;
            }

            return serialize;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }

        [JsonProperty("outer_sku_id")]
        public string OuterSkuId { get; set; }

        [JsonProperty("product_identifier_code")]
        public ProductIdentifierCode ProductIdentifierCode { get; set; }

        [JsonProperty("sales_attributes")]
        public List<SalesAttribute> SalesAttributes { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }

        [JsonProperty("stock_infos")]
        public List<StockInfo> StockInfos { get; set; }
    }

    public partial class ProductIdentifierCode
    {
        [JsonProperty("identifier_code")]
        public string IdentifierCode { get; set; }

        [JsonProperty("identifier_code_type")]
        public int IdentifierCodeType { get; set; }
    }

    public partial class SalesAttribute
    {
        [JsonIgnore]
        public bool IsCustomized { get; set; }

        [JsonIgnore]
        public AttributeValue[] AcceptedValues { get; set; }

        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        [JsonProperty("attribute_name")]
        public string AttributeName { get; set; }

        [JsonProperty("custom_value")]
        public string CustomValue { get; set; }

        [JsonProperty("sku_img")]
        public TikTokImage SkuImg { get; set; }

        [JsonProperty("value_id")]
        public string ValueId { get; set; }
    }

    public partial class StockInfo
    {
        [JsonProperty("available_stock")]
        public int AvailableStock { get; set; }

        [JsonProperty("warehouse_id")]
        public string WarehouseId { get; set; }
    }
}
