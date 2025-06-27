namespace DevLife_Portal.Common.Services
{
    public interface ICodeExecutionService
    {
        Task<string> ExecuteCodeAsync(string code, string language, CancellationToken cancellationToken);
    }
}
