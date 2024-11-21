using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor.Helpers
{
    public class CategoryBreadCrumb
    {
        public string Department { get; set; }
        public string Breadcrumb { get; set; }
        public string Id { get; set; }
    }

    public class CategoryBreadcrumbsGenerator
    {
        private Dictionary<string, CategoryList> categoryDictionary;

        public List<CategoryBreadCrumb> GenerateBreadcrumbs(List<CategoryList> categories, bool isNewApiVersion = false)
        {
            categoryDictionary = categories.ToDictionary(c => c.Id.ToString(), c => c);

            var breadcrumbs = new List<CategoryBreadCrumb>();
            foreach (var category in categories)
            {
                if (IsInnermostNode(category))
                {
                    string breadcrumb = BuildBreadcrumb(category);
                    string permissionStatus = string.Empty;

                    // Check permission status
                    if (isNewApiVersion 
                        && category.PermissionStatuses != null)
                    {
                        permissionStatus = category.PermissionStatuses.FirstOrDefault(c => 
                            c.Equals("INVITE_ONLY") || c.Equals("NON_MAIN_CATEGORY"));
                        if (!string.IsNullOrEmpty(permissionStatus))
                        {
                            breadcrumb += $" ({permissionStatus})";
                        }
                    }

                    // Check if the category is leaf
                    if (category.IsLeaf == false)
                    {
                        // Skip non leaf categories as they are read-only
                        continue;
                        breadcrumb += $" (NON-LEAF)";
                    }

                    breadcrumbs.Add(new CategoryBreadCrumb()
                    {
                        Department = breadcrumb.Contains(">") ? breadcrumb.Replace(" ", "").Split('>').First()
                        : breadcrumb,
                        Breadcrumb = breadcrumb,
                        Id = category.Id.ToString()
                    });
                }
            }

            return breadcrumbs;
        }

        private bool IsInnermostNode(CategoryList category)
        {
            return categoryDictionary.ContainsKey(category.ParentId.ToString());
        }

        private string BuildBreadcrumb(CategoryList category)
        {
            string breadcrumb = category.LocalDisplayName;

            while (categoryDictionary.ContainsKey(category.ParentId.ToString()))
            {
                category = categoryDictionary[category.ParentId.ToString()];
                breadcrumb = category.LocalDisplayName + " > " + breadcrumb;
            }

            return breadcrumb;
        }
    }
}
