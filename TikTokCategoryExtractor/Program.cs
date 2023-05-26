using System.Formats.Asn1;
using System.Text;
using TikTokCategoryExtractor;
using TikTokCategoryExtractor.Responses;

internal class Program
{
    private static string _apiVersion;
    private static Uri _baseURI;
    private static string _accessToken;
    private static string _appKey;
    private static string _appSecret;
    private static TikTokAPIClient _client;
    private static List<ProductAttribute> _attributes;

    private static void Main(string[] args)
    {
        InitializeProperties();

        var tikTokCategories = _client.SendRequest<TikTokCategories>(HttpMethod.Get,
                    "/api/products/categories", null, "Failed to get categories");

        if (tikTokCategories.IsSuccess && tikTokCategories?.Data?.CategoryList != null)
        {
            foreach (var category in tikTokCategories.Data.CategoryList)
            {
                var productAttributes = _client.SendRequest<ProductAttributes>(HttpMethod.Get,
                   "/api/products/attributes", null, "Failed to get category rule", null,
                   new Dictionary<string, string> { { "category_id", category.Id.ToString() } });

                if (productAttributes.IsSuccess && productAttributes?.Data?.Attributes != null)
                {
                    productAttributes.Data.Attributes.ForEach(x => x.CategoryName = category.LocalDisplayName.ToString());
                    _attributes = _attributes.Concat(productAttributes.Data.Attributes).ToList();
                }
            }

            var groups = _attributes.GroupBy(o => o.CategoryName);
            string textFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "report.txt");

            using (var writer = new StreamWriter(textFilePath))
            {
                writer.WriteLine($"Category Count: {tikTokCategories.Data.CategoryList.Count}");
                writer.WriteLine($"Field Count: {_attributes.Count}");
                writer.WriteLine($"Categories With Properties: {groups.Count()}");
                writer.WriteLine($"Categories With Required Properties: {groups.Count(group => group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory))}");
                writer.WriteLine($"Required Properties: {_attributes.Where(x => x?.InputType?.IsMandatory != null && x.InputType.IsMandatory).Count()}");
                foreach (var group in groups)
                {
                    writer.WriteLine("Category: {0}, Count: {1}, Required: {2}", group.Key, group.Count(), group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory));
                }
            }

            Console.Clear();
            Console.WriteLine($"Category Count: {tikTokCategories.Data.CategoryList.Count}");
            Console.WriteLine($"Field Count: {_attributes.Count}");
            Console.WriteLine($"Categories With Properties: {groups.Count()}");
            Console.WriteLine($"Categories With Required Properties: {groups.Count(group => group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory))}");
            Console.WriteLine($"Required Properties: {_attributes.Where(x => x?.InputType?.IsMandatory != null && x.InputType.IsMandatory).Count()}");

            foreach (var group in groups)
            {
                Console.WriteLine("Category: {0}, Count: {1}, Required: {2}", group.Key, group.Count(), group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory));
            }

            //Full Attributes List
            WriteCsvFile(_attributes, "product_attributes.csv");

            //Filtered for unique attributes that are required
            WriteCsvFile(_attributes
                        .Where(item => item?.InputType?.IsMandatory == true && ((item?.Values !=  null && item.Values.Any()) || (item?.Values == null || !item.Values.Any())))
                        .GroupBy(item => item.Name) // Group by item.Name
                        .Select(group => group.First()) // Select the first item from each group
                        .ToList(), "product_attributes_unique.csv");
        }
    }

    private static void InitializeProperties()
    {
        _apiVersion = "202212";
        _baseURI = new Uri("https://open-api.tiktokglobalshop.com");
        _accessToken = "TTP_oh2FfAAAAACgC9Q2hWu8S9I1n-DRzFC-28gJqhKcC-pl19xAaeOUTgAeLGyvKVbEncNCL8hsqYaWMhceKkPrxXb7-P0zudNoE3FFahsXLPNkFRXpOINivX6rO9JYrbI_nF_B5RDK2ShwnoRit01NK531vXGXoy3elOdszQaCdKukCbo_KZTLxg";
        _appKey = "684m8go9eavgo";
        _appSecret = "8e2d5b38a4a3b76dbcc504ae2fc611e2fcd5649b";
        _client = new TikTokAPIClient(_baseURI, _accessToken, _appKey, _appSecret, _apiVersion);
        _attributes = new List<ProductAttribute>();
    }

    private static void WriteCsvFile(List<ProductAttribute> attributes, string fileName)
    {
        try
        {
            string csvFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);

            using (var writer = new StreamWriter(csvFilePath))
            {
                writer.WriteLine("CategoryName,AttributeType,Id,is_mandatory,is_multiple_selected,is_customized,Name,Values,ids");

                foreach (ProductAttribute attribute in attributes)
                {
                    string categoryName = attribute.CategoryName != null ? EscapeCsvField(attribute.CategoryName) : "";
                    string attributeType = attribute.AttributeType.ToString();
                    string id = attribute.Id.ToString();
                    string is_mandatory = attribute.InputType?.IsMandatory != null ? EscapeCsvField(attribute.InputType.IsMandatory.ToString()) : "";
                    string is_multiple_selected = attribute.InputType?.IsMultipleSelected != null ? EscapeCsvField(attribute.InputType.IsMultipleSelected.ToString()) : "";
                    string is_customized = attribute.InputType?.IsCustomized != null ? EscapeCsvField(attribute.InputType.IsCustomized.ToString()) : "";
                    string name = attribute.Name != null ? EscapeCsvField(attribute.Name) : "";
                    string values = "";
                    if (attribute.Values != null && attribute.Values.Count > 0)
                    {
                        List<string> valueList = attribute.Values.Select(value => EscapeCsvField(value.Name)).ToList();
                        values = string.Join(":", valueList);
                    }
                    string ids = "";
                    if (attribute.Values != null && attribute.Values.Count > 0)
                    {
                        List<string> idList = attribute.Values.Select(value => EscapeCsvField(value.Id.ToString())).ToList();
                        ids = string.Join(":", idList);
                    }

                    string line = $"{categoryName},{attributeType},{id},{is_mandatory},{is_multiple_selected},{is_customized},{name},{values},{ids}";
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine("CSV file exported successfully.");
        }
        catch (Exception e)
        {
        }
    }

    private static string EscapeCsvField(string fieldValue)
    {
        if (fieldValue.Contains(",") || fieldValue.Contains("\"") || fieldValue.Contains("\n"))
        {
            fieldValue = fieldValue.Replace("\"", "\"\"");
            fieldValue = $"\"{fieldValue}\"";
        }
        return fieldValue;
    }
}