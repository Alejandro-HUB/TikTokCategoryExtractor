using Newtonsoft.Json;
using TikTokCategoryExtractor.Responses;

namespace TikTokCategoryExtractor.Helpers
{
    public static class TikTokProductImport
    {
        public static List<DataProduct> ImportProductData(TikTokAPIClient client, List<GetTikTokProduct> productData)
        {
            if (productData != null && productData.Any())
            {
                var products = new List<DataProduct>();

                // Now that we have all the products, get product detail
                foreach (var product in productData)
                {
                    var getProductDetailResponse = client.SendRequest<GetProductDetail>(HttpMethod.Get,
                      "/api/products/details", null,
                      $"Failed to get products page", null, new Dictionary<string, string>() { { "product_id", product.Id } });

                    if (getProductDetailResponse.IsSuccess && getProductDetailResponse.Data != null)
                    {
                        products.Add(getProductDetailResponse.Data);
                    }
                }

                return products;
            }

            return null;
        }

        public static List<GetTikTokProduct> GetBasicProductData(TikTokAPIClient client)
        {
            var pageNumber = 1;
            var PAGESIZE = 100;
            var moreProducts = true;
            var productData = new List<GetTikTokProduct>();

            while (moreProducts)
            {
                // Json Body
                var jsonBody = new
                {
                    page_number = pageNumber,
                    page_size = PAGESIZE,
                    search_status = 4 // Active
                };

                // Pull all the products in the TikTok Shop
                var getProductListResponse = client.SendRequest<GetProductList>(HttpMethod.Post,
                       "/api/products/search", JsonConvert.SerializeObject(jsonBody),
                       $"Failed to get products page", null, null);
                if (getProductListResponse.IsSuccess
                    && getProductListResponse?.Data?.Products != null
                    && getProductListResponse.Data.Products.Any())
                {
                    productData.AddRange(getProductListResponse.Data.Products);
                    pageNumber++;
                }
                else
                {
                    moreProducts = false;
                }
            }

            return productData;
        }
    }
}
