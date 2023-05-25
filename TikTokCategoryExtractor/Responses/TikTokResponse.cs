using TikTokCategoryExtractor.Interfaces;

namespace TikTokCategoryExtractor.Responses
{
    public class TikTokResponse : ITikTokResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
