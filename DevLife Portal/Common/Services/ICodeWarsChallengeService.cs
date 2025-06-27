namespace DevLife_Portal.Common.Services
{
    public interface ICodeWarsChallengeService
    {
        Task<CodewarsChallengeDto?> GetRandomChallengeAsync(string language, string difficulty, CancellationToken cancellationToken);
    }
}
