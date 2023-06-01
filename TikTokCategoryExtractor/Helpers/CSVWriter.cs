using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor.Helpers
{
    public static class CSVWriter
    {
        public static void WriteBreadCrumbsToCsv(List<CategoryBreadCrumb> data, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write column headers
                writer.WriteLine("department,breadcrumb,id");

                // Write data rows
                foreach (var kvp in data)
                {
                    writer.WriteLine($"{QuoteCsvField(kvp.Department)},{QuoteCsvField(kvp.Breadcrumb)},{QuoteCsvField(kvp.Id)}");
                }
            }
        }

        public static string QuoteCsvField(string field)
        {
            // If the field contains a comma, double quotes, or newlines, enclose it in double quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                field = $"\"{field}\"";
            }
            return field;
        }


        public static void WriteCsvFile(List<ProductAttribute> attributes, string fileName)
        {
            try
            {
                string csvFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);

                using (var writer = new StreamWriter(csvFilePath))
                {
                    writer.WriteLine("CategoryName,CategoryId,AttributeType,Id,is_mandatory,is_multiple_selected,is_customized,Name,Values,ids");

                    foreach (ProductAttribute attribute in attributes)
                    {
                        string categoryName = attribute.CategoryName != null ? EscapeCsvField(attribute.CategoryName) : "";
                        string categoryId = attribute.CategoryId != null ? EscapeCsvField(attribute.CategoryId) : "";
                        string attributeType = attribute.AttributeType.ToString();
                        string id = attribute.Id.ToString();
                        string is_mandatory = attribute.InputType?.IsMandatory != null ? EscapeCsvField(attribute.InputType.IsMandatory.ToString()) : "";
                        string is_multiple_selected = attribute.InputType?.IsMultipleSelected != null ? EscapeCsvField(attribute.InputType.IsMultipleSelected.ToString()) : "";
                        string is_customized = attribute.InputType?.IsCustomized != null ? EscapeCsvField(attribute.InputType.IsCustomized.ToString()) : "";
                        string name = attribute.Name != null ? EscapeCsvField(attribute.Name) : "";
                        string values = attribute.ConcatenatedValues;
                        string ids = "";
                        if (attribute.Values != null && attribute.Values.Count > 0)
                        {
                            List<string> idList = attribute.Values.Select(value => EscapeCsvField(value.Id.ToString())).ToList();
                            ids = string.Join(":", idList);
                        }

                        string line = $"{categoryName},{categoryId},{attributeType},{id},{is_mandatory},{is_multiple_selected},{is_customized},{name},{values},{ids}";
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("CSV file exported successfully.");
            }
            catch (Exception e)
            {
            }
        }

        public static string EscapeCsvField(string fieldValue)
        {
            if (fieldValue.Contains(",") || fieldValue.Contains("\"") || fieldValue.Contains("\n"))
            {
                fieldValue = fieldValue.Replace("\"", "\"\"");
                fieldValue = $"\"{fieldValue}\"";
            }
            return fieldValue;
        }
    }
}
