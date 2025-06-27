namespace DevLife_Portal.Common.Services
{
    public interface IAiContentService
    {
        Task<string> GenerateRoastAsync(string code, string result, CancellationToken cancellationToken);
    }
}
