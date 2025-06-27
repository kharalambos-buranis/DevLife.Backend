namespace DevLife_Portal.Common.Services
{
    public class MockCodeExecutionService : ICodeExecutionService
    {
        public Task<string> ExecuteCodeAsync(string code, string language, CancellationToken ct)
        {
            return Task.FromResult("Success");
        }
    }
}
