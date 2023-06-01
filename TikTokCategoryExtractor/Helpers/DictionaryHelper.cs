using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor.Helpers
{
    public static class DictionaryHelper
    {
        public static bool AreAllKeysPresent(List<ProductAttribute> attributes, List<string> keys)
        {
            HashSet<string> attributeNames = new HashSet<string>(attributes.Select(attr => attr.Name));
            HashSet<string> keySet = new HashSet<string>(keys);

            return keySet.IsSubsetOf(attributeNames);
        }

        public static string GetMatchingKeys(List<ProductAttribute> attributes, List<string> keys)
        {
            HashSet<string> attributeNames = new HashSet<string>(attributes.Select(attr => attr.Name));
            HashSet<string> keySet = new HashSet<string>(keys);

            HashSet<string> matchingKeys = new HashSet<string>(keySet.Where(key => attributeNames.Contains(key)));

            return string.Join(", ", matchingKeys);
        }
    }
}
