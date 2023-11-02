using static Program;
using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor.Helpers
{
    public static class ProductAttributesHelper
    {
        public static void GenerateFieldDescriptions(List<ProductAttribute> attributes,
            bool generateFiles) 
        {
            var categoryDescriptions = new List<CategoryDescriptions>();

            foreach (var productAttribute in attributes)
            {
                var existingCategoryDescription = categoryDescriptions.FirstOrDefault(x => x.AttributeName == productAttribute.Name);

                if (existingCategoryDescription == null)
                {
                    var description = new CategoryDescriptions()
                    {
                        AttributeName = productAttribute.Name,
                        CategoryName = productAttribute.CategoryName,
                        unorderedLists = new List<UnorderedLists>()
                    };
                    if (!string.IsNullOrEmpty(productAttribute.ConcatenatedValues))
                    {
                        description.unorderedLists.Add(new UnorderedLists()
                        {
                            CategoryName = productAttribute.CategoryName,
                            CategoryUnorderedList = ConvertListToHtml(productAttribute.ConcatenatedValues)
                        });
                        categoryDescriptions.Add(description);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(productAttribute.ConcatenatedValues))
                    {
                        existingCategoryDescription.unorderedLists.Add(new UnorderedLists()
                        {
                            CategoryName = productAttribute.CategoryName,
                            CategoryUnorderedList = ConvertListToHtml(productAttribute.ConcatenatedValues)
                        });
                    }
                }
            }

            if (categoryDescriptions.Any())
            {
                List<string> descriptions = new List<string>();

                foreach (var categoryDescription in categoryDescriptions)
                {
                    descriptions.Add(ConvertObjectToHtmlDescriptionFile(categoryDescription, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", $"{categoryDescription.AttributeName.Replace("\\", "").Replace("/", "")}.html"), generateFiles));
                }
            }
        }

        public static string GenerateFieldDescription(ProductAttribute attribute)
        {
            var categoryDescription = new CategoryDescriptions();

            var description = new CategoryDescriptions()
            {
                AttributeName = attribute.Name,
                CategoryName = attribute.CategoryName,
                unorderedLists = new List<UnorderedLists>()
            };
            if (!string.IsNullOrEmpty(attribute.ConcatenatedValues))
            {
                description.unorderedLists.Add(new UnorderedLists()
                {
                    CategoryName = attribute.CategoryName,
                    CategoryUnorderedList = ConvertListToHtml(attribute.ConcatenatedValues)
                });
                categoryDescription = description;
            }
            else
            {
                description.unorderedLists = null;
            }

            return ConvertObjectToHtmlDescriptionFile(categoryDescription, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", $"{categoryDescription.AttributeName.Replace("\\", "").Replace("/", "")}.html"), false);
        }

        public static string ConvertObjectToHtmlDescriptionFile(CategoryDescriptions description, string filePath,
            bool writeFile)
        {
            string result = string.Empty;

            if (writeFile)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"<b>{description.AttributeName}</b><br><br>");

                    var groupedCategories = description.unorderedLists
                        .GroupBy(item => item.CategoryUnorderedList)
                        .Select(group => new
                        {
                            CategoryNames = string.Join(", ", group.Select(item => item.CategoryName)),
                            UnorderedList = group.Key
                        });

                    foreach (var group in groupedCategories)
                    {
                        writer.WriteLine($"<br><b>Accepted Values for the Categories: {group.CategoryNames}</b>");
                        writer.WriteLine(group.UnorderedList);
                    }
                }

                Console.WriteLine("HTML description generated successfully.");
                return result;
            }
            else
            {
                string newDescription = $"<b>{description.AttributeName}</b><br><br>\n";

                if (description.unorderedLists != null)
                {
                    var groupedCategories = description.unorderedLists
                       .GroupBy(item => item.CategoryUnorderedList)
                       .Select(group => new
                       {
                           CategoryNames = string.Join(", ", group.Select(item => item.CategoryName)),
                           UnorderedList = group.Key
                       });
                    foreach (var group in groupedCategories)
                    {
                        newDescription += $"<br><b>Accepted Values for the Categories: {group.CategoryNames}</b>\n";
                        newDescription += group.UnorderedList + "\n";
                    }
                }

                return newDescription;
            }
        }


        public static void ConvertListToHtmlFile(string listInput, string filePath)
        {
            string[] items = listInput.Split(':');

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("<ul>");

                foreach (string item in items)
                {
                    writer.WriteLine($"    <li>{item}</li>");
                }

                writer.WriteLine("</ul>");
            }

            Console.WriteLine("HTML list generated successfully.");
        }

        public static string ConvertListToHtml(string listInput)
        {
            string[] items = listInput.Split(':');
            var numericItems = new List<int>();
            var nonNumericItems = new List<string>();

            foreach (string item in items)
            {
                if (int.TryParse(item, out int numericItem))
                {
                    numericItems.Add(numericItem);
                }
                else
                {
                    nonNumericItems.Add(item);
                }
            }

            numericItems.Sort();
            nonNumericItems.Sort();

            var result = "<ul>";

            foreach (int numericItem in numericItems)
            {
                result += $"    <li>{numericItem}</li>";
            }

            foreach (string nonNumericItem in nonNumericItems)
            {
                result += $"    <li>{nonNumericItem}</li>";
            }

            result += "</ul>";

            Console.WriteLine("HTML list generated successfully.");

            return result;
        }

        public static void GenerateEnum(string enumNamesInput, string enumValuesInput, string filePath, string enumName)
        {
            string[] enumNames = enumNamesInput.Split(':');
            string[] enumValues = enumValuesInput.Split(':');

            if (enumNames.Length != enumValues.Length)
            {
                Console.WriteLine("Error: The number of enum names and enum values must be the same.");
                return;
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"public enum {enumName}");
                writer.WriteLine("{");

                for (int i = 0; i < enumNames.Length; i++)
                {
                    writer.WriteLine($"    [Description(\"{enumNames[i]}\")]");
                    writer.WriteLine($"    {SanitizeEnumName(enumNames[i])} = {enumValues[i]},");
                }

                writer.WriteLine("}");
            }

            Console.WriteLine("Enum generated successfully.");
        }

        private static string SanitizeEnumName(string name)
        {
            // Remove special characters and spaces from the enum name
            return string.Concat(name.Where(c => char.IsLetterOrDigit(c))); ;
        }
    }

    public class CategoryDescriptions
    {
        public string AttributeName { get; set; }
        public string CategoryName { get; set; }
        public List<UnorderedLists> unorderedLists { get; set; }
    }

    public class UnorderedLists
    {
        public string CategoryName { get; set; }
        public string CategoryUnorderedList { get; set; }
    }
}
