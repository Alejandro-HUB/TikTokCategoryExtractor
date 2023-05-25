namespace TikTokCategoryExtractor.Interfaces
{
    public interface ITikTokResponse
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
