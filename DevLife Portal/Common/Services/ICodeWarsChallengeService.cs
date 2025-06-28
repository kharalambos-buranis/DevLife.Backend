using DevLife_Portal.Common.Enums;

namespace DevLife_Portal.Common.Services
{
    public interface ICodeWarsChallengeService
    {
        Task<CodewarsChallengeDto?> GetRandomChallengeAsync(string language, Experience difficulty, CancellationToken cancellationToken);
    }
}
