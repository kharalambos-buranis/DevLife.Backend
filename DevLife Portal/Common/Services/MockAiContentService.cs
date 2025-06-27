namespace DevLife_Portal.Common.Services
{
    public class MockAiContentService : IAiContentService
    {
        public Task<string> GenerateRoastAsync(string code, string result, CancellationToken ct)
        {
            var roast = result == "Success"
                ? "Bravo! Even your compiler smiled. 🧠"
                : "This code is so buggy, it applied for disability. 🐛";

            return Task.FromResult(roast);
        }
    }
}
